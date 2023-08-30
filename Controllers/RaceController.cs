﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class RaceController : Controller
    {
        private readonly AppDbContext _context;

        public RaceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Race> raceList = _context.Races.ToList();

            return View(raceList);
        }
    }
}
