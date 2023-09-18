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

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateRaceViewModel form)
        {
            Race race = null;
            if (ModelState.IsValid)
            {
                race = uploadRaceAndImageAsync(form).Result;
                return RedirectToAction("Index");
            } else
            {
                ModelState.AddModelError("", "Photo upload failed.");
            }

            return View(race);
        }

        public async Task<Race> uploadRaceAndImageAsync(CreateRaceViewModel form)
        {
            Race race = null;
            string path = await _photoService.ConvertIFormFileToStringPathAsync(form.photo.ImageFile);
            string fileName = form.photo.ImageFile.FileName;

            BlobServiceClient blobServiceClient = null; // Initialize as null
            _photoService.GetBlobServiceClientSAS(ref blobServiceClient);

            _photoService.UploadFromFileAsync(blobServiceClient.GetBlobContainerClient("run-group-container"), path, fileName);

            var photoUrl = blobServiceClient.Uri.AbsoluteUri;
            race = new Race
            {
                Title = form.Title,
                Description = form.Description,
                Image = "https://rungroup.blob.core.windows.net/run-group-container/" + fileName + "?" + _photoService.sasToken,
                Address = form.Address,
                RaceCategory = form.RaceCategory

            };
            _raceRepository.Add(race);

            return race;
        }
    }
}

