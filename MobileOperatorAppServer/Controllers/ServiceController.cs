using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Models;
using MobileOperatorAppServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MobileOperatorAppServer.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private Context context;

        public ServiceController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index(int page = 1)
        {
            List<ServiceModel> services = context.Services.ToList();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = services.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = services.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult Create(string name, string description, string priceStr, string internetQuantityStr, string minutesQuantityStr, string otherMinutesQuantityStr, string smsQuantityStr) 
        {
            if (!double.TryParse(internetQuantityStr, out double internetQuantity)
                || !int.TryParse(minutesQuantityStr, out int minutesQuantity)
                || !int.TryParse(otherMinutesQuantityStr, out int otherMinutesQuantity)
                || !int.TryParse(smsQuantityStr, out int smsQuantity))
            {
                TempData["ErrorMessage"] = "Помилка! Введена кількість некоректна";
                return RedirectToAction("Index");
            }

            if (internetQuantity < -1 || internetQuantity > 100000
                || minutesQuantity < -1 || minutesQuantity > 100000
                || otherMinutesQuantity < -1 || otherMinutesQuantity > 100000
                || smsQuantity < -1 || smsQuantity > 100000)
            {
                TempData["ErrorMessage"] = "Помилка! Кількість має бути в межах від -1 до 100000, де \n\"-1\" - безліміт, \n\"0\" - послуга недоступна";
                return RedirectToAction("Index");
            }

            if (!decimal.TryParse(priceStr, out decimal price))
            {
                TempData["ErrorMessage"] = "Помилка! Введено некоректну ціну";
                return RedirectToAction("Index");
            }

            if (price < 10 || price > 10000)
            {
                TempData["ErrorMessage"] = "Помилка! Ціна має бути від 10 до 10000";
                return RedirectToAction("Index");
            }

            context.Services.Add(new ServiceModel
            {
                Name = name,
                Description = description,
                Price = price,
                InternetQuantity = internetQuantity,
                MinutesQuantity = minutesQuantity,
                OtherMinutesQuantity = otherMinutesQuantity,
                SMSQuantity = smsQuantity
            });
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ServiceModel service = context.Services.FirstOrDefault(a => a.Id == id);

            List<UserConnectedServicesModel> userConnectedServices = context.UserConnectedServices.Include(s => s.Service).Where(s => s.Service.Id == id).ToList();

            if (userConnectedServices.Count > 0)
            {
                TempData["ErrorMessage"] = "Помилка! Є користувачі, які використовують цю послугу";
                return RedirectToAction("Index");
            }

            context.Services.Remove(service);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id, string name, string description, string priceStr, string internetQuantityStr, string minutesQuantityStr, string otherMinutesQuantityStr, string smsQuantityStr) 
        {
            ServiceModel service = context.Services.FirstOrDefault(s => s.Id == id);

            if (!double.TryParse(internetQuantityStr, out double internetQuantity)
                || !int.TryParse(minutesQuantityStr, out int minutesQuantity)
                || !int.TryParse(otherMinutesQuantityStr, out int otherMinutesQuantity)
                || !int.TryParse(smsQuantityStr, out int smsQuantity))
            {
                TempData["ErrorMessage"] = "Помилка! Введена кількість некоректна";
                return RedirectToAction("Index");
            }

            if (internetQuantity < -1 || internetQuantity > 100000
                || minutesQuantity < -1 || minutesQuantity > 100000
                || otherMinutesQuantity < -1 || otherMinutesQuantity > 100000
                || smsQuantity < -1 || smsQuantity > 100000)
            {
                TempData["ErrorMessage"] = "Помилка! Кількість має бути в межах від -1 до 100000, де \n\"-1\" - безліміт, \n\"0\" - послуга недоступна";
                return RedirectToAction("Index");
            }

            if (!decimal.TryParse(priceStr, out decimal price))
            {
                TempData["ErrorMessage"] = "Помилка! Введено некоректну ціну";
                return RedirectToAction("Index");
            }

            if (price < 10 || price > 10000)
            {
                TempData["ErrorMessage"] = "Помилка! Ціна має бути від 10 до 10000";
                return RedirectToAction("Index");
            }

            service.Name = name;
            service.Description = description;
            service.Price = price;
            service.InternetQuantity = internetQuantity;
            service.MinutesQuantity = minutesQuantity;
            service.OtherMinutesQuantity = otherMinutesQuantity;
            service.SMSQuantity = smsQuantity;

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
