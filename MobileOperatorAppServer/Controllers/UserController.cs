using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer;
using MobileOperatorAppServer.Models;
using MobileOperatorAppServer.Models.ViewModels;
using MobileOperatorAppServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileOperatorAppServer.Controllers
{
    
    [Authorize]
    public class UserController : Controller
    {
        private readonly Context context;

        public UserController(Context context)
        {
            this.context = context;
        }
        
        public IActionResult Index(int page = 1)
        {
            List<UserModel> users = context.Users.Include(u => u.Tariff).ToList();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = users.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = users.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            var viewModel = new UserTariffViewModel
            {
                Users = data,
                Tariffs = context.Tariffs.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(string name, string surname, string middleName, string phoneNumber, int tariffId)
        {
            TariffModel tariff = context.Tariffs.FirstOrDefault(t => t.Id == tariffId);

            UserModel user = context.Users.FirstOrDefault(u => u.PhoneNumber ==  phoneNumber);

            if (user != null) 
            {
                TempData["ErrorMessage"] = "Помилка! Користувач з таким номером телефону вже існує";
                return RedirectToAction("Index");
            }

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    TempData["ErrorMessage"] = "Помилка! Номер телефону має складатися лише з цифр";
                    return RedirectToAction("Index");
                }
            }

            if (phoneNumber.Length != 9)
            {
                TempData["ErrorMessage"] = "Помилка! Довжина номеру телефону має складатися з 9 цифр";
                return RedirectToAction("Index");
            }

            context.Users.Add(new UserModel
            {
                Name = name,
                Surname = surname,
                MiddleName = middleName,
                PhoneNumber = phoneNumber,
                ConnectionDate = DateTime.Now.Date,
                Balance = 0 - tariff.Price,
                Tariff = tariff
            });

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, string phoneNumber, string name, string surname, string middleName)
        {
            UserModel user = context.Users.FirstOrDefault(u => u.Id == id);
            UserModel anotherUser = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (anotherUser != null && user.Id != anotherUser.Id)
            {
                TempData["ErrorMessage"] = "Помилка! Користувач з таким номером телефону вже існує";
                return RedirectToAction("Index");
            }

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    TempData["ErrorMessage"] = "Помилка! Номер телефону має складатися лише з цифр";
                    return RedirectToAction("Index");
                }
            }

            if (phoneNumber.Length != 9)
            {
                TempData["ErrorMessage"] = "Помилка! Довжина номеру телефону має складатися з 9 цифр";
                return RedirectToAction("Index");
            }

            user.Name = name;
            user.Surname = surname;
            user.MiddleName = middleName;
            user.PhoneNumber = phoneNumber;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            UserModel user = context.Users.FirstOrDefault(u => u.Id == id);

            context.Users.Remove(user);

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Activities(int id, int page = 1) 
        {
            List<ActivityModel> activities = context.Activities
               .Include(a => a.User)
               .Where(a => a.User.Id == id)
               .ToList();

            UserModel user = context.Users.FirstOrDefault(u => u.Id == id);

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = activities.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = activities.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            var viewModel = new UserActivitiesViewModel
            {
                Activities = data,
                User = user
            };

            return View(viewModel);
        }

        public IActionResult AddActivity(int id, string quantityStr, ActivityType type)
        {
            UserModel user = context.Users.Include(u => u.Tariff).Include(u => u.Activities).FirstOrDefault(u => u.Id == id);
            
            if (user.ConnectionDate.AddDays(30) < DateTime.Now.Date)
            {
                TempData["ErrorMessage"] = "Помилка! Термін дії тарифу користувача закінчено";
                return RedirectToAction("Activities", new { id, page = 1 });
            }

            if (!double.TryParse(quantityStr, out double quantity) || quantity < -1)
            {
                TempData["ErrorMessage"] = "Помилка! Введено некоректну кількість";
                return RedirectToAction("Activities", new { id, page = 1 });
            }

            double availableInternet = 0;
            double availableMinutes = 0;
            double availableOtherMinutes = 0;
            double availableSMS = 0;

            var connectedServices = context.UserConnectedServices
                .Include(ucs => ucs.User)
                .Include(ucs => ucs.Service)
                .Where(ucs => ucs.User.Id == id)
                .Select(ucs => ucs.Service)
                .ToList();

            foreach (var service in connectedServices)
            {
                availableInternet += (double)service.InternetQuantity;
                availableMinutes += (int)service.MinutesQuantity;
                availableOtherMinutes += (int)service.OtherMinutesQuantity;
                availableSMS += (int)service.SMSQuantity;
            }

            TariffModel tariff = context.Users.Include(u => u.Tariff).FirstOrDefault(u => u.Id == id).Tariff;

            if (tariff.InternetQuantity == -1)
                availableInternet = -1;
            else
                availableInternet += tariff.InternetQuantity;

            if (tariff.MinutesQuantity == -1)
                availableMinutes = -1;
            else
                availableMinutes += tariff.MinutesQuantity;

            if (tariff.OtherMinutesQuantity == -1)
                availableOtherMinutes = -1;
            else
                availableOtherMinutes += tariff.OtherMinutesQuantity;

            if (tariff.SMSQuantity == -1)
                availableSMS = -1;
            else
                availableSMS += tariff.SMSQuantity;

            var totalQuantity = context.Activities
                .Include(a => a.User)
                .Where(a => a.User.Id == id && a.Type == type)
                .GroupBy(a => new { a.User.Id, a.Type })
                .Select(g => new { g.Key.Id, g.Key.Type, TotalQuantity = g.Sum(a => a.Quantity) })
                .FirstOrDefault();

            switch (type)
            {
                case ActivityType.INTERNET:
                    if (totalQuantity != null)
                        availableInternet -= totalQuantity.TotalQuantity;
                    if (availableInternet == 0)
                    {
                        TempData["ErrorMessage"] = "Помилка! У користувача закінчився інтерент або він недоступний у тарифі";
                        return RedirectToAction("Activities", new { id, page = 1 });
                    }

                    if (quantity > availableInternet && availableInternet != -1)
                    {
                        quantity = availableInternet;
                    }

                    context.Activities.Add(new ActivityModel
                    {
                        Type = type,
                        User = user,
                        Quantity = quantity,
                        Date = DateTime.Now.Date
                    });

                    break;
                case ActivityType.MINUTES:
                    if (totalQuantity != null)
                        availableMinutes -= totalQuantity.TotalQuantity;
                    if (availableMinutes == 0)
                    {
                        TempData["ErrorMessage"] = "Помилка! У користувача закінчилися хвилини або вони недоступні у тарифі";
                        return RedirectToAction("Activities", new { id, page = 1 });
                    }

                    if (quantity > availableMinutes && availableMinutes != -1)
                    {
                        quantity = availableMinutes;
                    }

                    context.Activities.Add(new ActivityModel
                    {
                        Type = type,
                        User = user,
                        Quantity = quantity,
                        Date = DateTime.Now.Date
                    });
                    break;
                case ActivityType.OTHER_MINUTES:
                    if (totalQuantity != null)
                        availableOtherMinutes -= totalQuantity.TotalQuantity;
                    if (availableOtherMinutes == 0)
                    {
                        TempData["ErrorMessage"] = "Помилка! У користувача закінчилися хвилини на інших операторів або вони недоступні у тарифі";
                        return RedirectToAction("Activities", new { id, page = 1 });
                    }

                    if (quantity > availableOtherMinutes && availableOtherMinutes != -1)
                    {
                        quantity = availableOtherMinutes;
                    }

                    context.Activities.Add(new ActivityModel
                    {
                        Type = type,
                        User = user,
                        Quantity = quantity,
                        Date = DateTime.Now.Date
                    });
                    break;
                case ActivityType.SMS:
                    if (totalQuantity != null)
                        availableSMS -= totalQuantity.TotalQuantity;
                    if (availableSMS == 0)
                    {
                        TempData["ErrorMessage"] = "Помилка! У користувача закінчилися SMS повідомлення або вони недоступні у тарифі";
                        return RedirectToAction("Activities", new { id, page = 1 });
                    }

                    if (quantity > availableSMS && availableSMS != -1)
                    {
                        quantity = availableSMS;
                    }

                    context.Activities.Add(new ActivityModel
                    {
                        Type = type,
                        User = user,
                        Quantity = quantity,
                        Date = DateTime.Now.Date
                    });
                    break;
                default:
                    break;
            }

            context.SaveChanges();
            return RedirectToAction("Activities", new { id, page = 1 });
        }

        public IActionResult Services(int id, int page = 1)
        {
            var connectedServices = context.UserConnectedServices
                .Include(ucs => ucs.User)
                .Include(ucs => ucs.Service)
                .Where(ucs => ucs.User.Id == id)
                .Select(ucs => ucs.Service)
                .ToList();

            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = connectedServices.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = connectedServices.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            var viewModel = new UserServicesViewModel
            {
                Services = connectedServices,
                User = context.Users.FirstOrDefault(u => u.Id == id)
            };

            return View(viewModel);
        }
    }
}

