using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RussianСertification.Data;
using RussianСertification.Models;
using Microsoft.AspNetCore.Identity;


namespace RussianСertification.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: users
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Index() => View(await _context.Users.ToListAsync());
    }
}