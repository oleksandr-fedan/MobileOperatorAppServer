using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileOperatorAppServer.Models;
using MobileOperatorAppServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MobileOperatorAppServer.Controllers
{
    [Authorize]
    public class TariffController : Controller
    {
        private readonly Context context;

        public TariffController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index(int page = 1)
        {
            List<TariffModel> tariffs = context.Tariffs.ToList();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = tariffs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = tariffs.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult Create(string name, string priceStr, string internetQuantityStr, string minutesQuantityStr, string otherMinutesQuantityStr, string smsQuantityStr)
        {
            if (!double.TryParse(internetQuantityStr, out double internetQuantity) 
                || !int.TryParse(minutesQuantityStr, out int minutesQuantity)
                || !int.TryParse(otherMinutesQuantityStr, out int otherMinutesQuantity)
                || !int.TryParse(smsQuantityStr, out int smsQuantity))
            {
                TempData["ErrorMessage"] = "Помилка! Введена кількість некоректна";
                return RedirectToAction("Index");
            }

            if ( internetQuantity < -1 || internetQuantity > 100000 
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

            context.Tariffs.Add(new TariffModel
            {
                Name = name,
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
            var tariff = context.Tariffs.FirstOrDefault(a => a.Id == id);
            var users = context.Users.Where(u => u.Tariff.Id == tariff.Id).ToList();

            if (users.Count > 0)
            {
                TempData["ErrorMessage"] = "Помилка! Є користувачі, які використовують цей тариф";
                return RedirectToAction("Index");
            }

            context.Tariffs.Remove(tariff);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, string name, string priceStr, string internetQuantityStr, string minutesQuantityStr, string otherMinutesQuantityStr, string smsQuantityStr) 
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

            TariffModel tariff = context.Tariffs.FirstOrDefault(t => t.Id == id);
            tariff.Name = name;
            tariff.Price = price;
            tariff.InternetQuantity = internetQuantity;
            tariff.MinutesQuantity = minutesQuantity;
            tariff.OtherMinutesQuantity = otherMinutesQuantity;
            tariff.SMSQuantity = smsQuantity;
            context.SaveChanges();

            return RedirectToAction("Index"); ;
        }
    }
}
