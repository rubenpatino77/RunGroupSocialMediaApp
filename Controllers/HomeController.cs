using System.Diagnostics;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunGroupSocialMedia.Interfaces;
using RunGroupSocialMedia.Models;
using RunGroupSocialMedia.Services;

namespace RunGroupSocialMedia.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IPhotoService _photoService;

    public HomeController(ILogger<HomeController> logger, IPhotoService photoService)
    {
        _logger = logger;
        _photoService = photoService;
    }

    public async Task<IActionResult> IndexAsync()
    {

        BlobServiceClient blobServiceClient = null; // Initialize as null
        _photoService.GetBlobServiceClientSAS(ref blobServiceClient);

        var containerNames = await _photoService.ListContainers(blobServiceClient, null, null);
        _logger.LogInformation("List of Containers: {0}", string.Join(", ", containerNames));
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

