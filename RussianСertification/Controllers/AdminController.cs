using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RussianСertification.Data;
using RussianСertification.Models;


namespace RussianСertification.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "admin")]
        //public IActionResult Index()
        //{
        //    var users = _context.Users.ToList();
        //    //  Users.userslist = _context.Users.ToList();
        //    //return Content(users[0].ToString());

        //    return View(users);
        //}

        // GET: Contacts
        public async Task<IActionResult> Index() => View(await _context.Users.ToListAsync());


    }
}