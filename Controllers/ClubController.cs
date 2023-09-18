 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupSocialMedia.Data;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.Services;
using RunGroupSocialMedia.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupSocialMedia.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel form)
        {
            Club club = null;
            if (ModelState.IsValid)
            {
                club = uploadClubAndImageAsync(form).Result;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed.");
            }

            return View(club);
        }

        public async Task<Club> uploadClubAndImageAsync(CreateClubViewModel form)
        {
            Club club = null;
            string path = await _photoService.ConvertIFormFileToStringPathAsync(form.photo.ImageFile);
            string fileName = form.photo.ImageFile.FileName;

            BlobServiceClient blobServiceClient = null; // Initialize as null
            _photoService.GetBlobServiceClientSAS(ref blobServiceClient);

            _photoService.UploadFromFileAsync(blobServiceClient.GetBlobContainerClient("run-group-container"), path, fileName);

            var photoUrl = blobServiceClient.Uri.AbsoluteUri;
            club = new Club
            {
                Title = form.Title,
                Description = form.Description,
                Image = "https://rungroup.blob.core.windows.net/run-group-container/" + fileName + "?" + _photoService.sasToken,
                Address = form.Address,
                ClubCategory = form.ClubCategory

            };
            _clubRepository.Add(club);

            return club;
        }
    }
}

