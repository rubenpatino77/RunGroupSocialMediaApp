using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Repository;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class DashboardController : Controller
    {
        public IDashboardRepository _dashboardRepository { get; }

        public DashboardController(IDashboardRepository dashboradRepository)
        {
            _dashboardRepository = dashboradRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> IndexAsync()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}

