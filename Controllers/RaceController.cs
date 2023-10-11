using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.Services;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> raceList = await _raceRepository.GetAll();

            return View(raceList); 
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = curUserId };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateRaceViewModel form)
        {
            Race race = null;
            var imageUrl = await _photoService.AddPhotoAsync(form.photo.ImageFile);
            //string imageUrl = form.photo.ImageFile.FileName;
            if (ModelState.IsValid)
            {
                race = _raceRepository.Add(form, imageUrl.Url.ToString());
                //await _photoService.AddPhotoAsync(form.photo.ImageFile);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed.");
            }

            return View(race);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            string raceImageUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVM);
            }

            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

            if (userRace == null)
            {
                return View("Error");
            }

            if (raceVM.Image != null)
            {
                await _photoService.AddPhotoAsync(raceVM.Image);
                string imageUrl = raceVM.Image.FileName;
                raceImageUrl = imageUrl;
            }
            else
            {
                raceImageUrl = userRace.Image;
            }

            var race = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = raceImageUrl,
                AddressId = raceVM.AddressId,
                Address = raceVM.Address,
            };

            _raceRepository.Update(race);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");
            return View(raceDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);

            if (raceDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(raceDetails.Image))
            {
                _photoService.DeletePhotoAsync(raceDetails.Image);
            }

            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}

