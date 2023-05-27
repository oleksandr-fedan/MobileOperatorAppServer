using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileOperatorAppServer.Models;
using MobileOperatorAppServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MobileOperatorAppServer.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Context context;

        public AdminController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index(int page = 1)
        {
            List<AdminModel> admins = context.Admins.ToList();
            const int pageSize = 10;
            if (page < 1)
                page = 1;

            int recsCount = admins.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = admins.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult Create(string name, string surname, string middleName, string username, string password)
        {
            context.Admins.Add(new AdminModel
            {
                Name = name,
                Surname = surname,
                MiddleName = middleName,
                Username = username,
                Password = password
            });
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) 
        {
            var admin = context.Admins.FirstOrDefault(a => a.Id == id);
            context.Admins.Remove(admin);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id, string name, string surname, string middleName, string username, string password)
        {
            AdminModel admin = context.Admins.FirstOrDefault(a => a.Id == id);

            admin.Name = name;
            admin.Surname = surname;
            admin.MiddleName = middleName;  
            admin.Username = username;
            admin.Password = password;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
