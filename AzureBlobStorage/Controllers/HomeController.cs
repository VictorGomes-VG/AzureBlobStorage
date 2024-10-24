using AzureBlobStorage.Models;
using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobsService blobsService;

        public HomeController(BlobsService blobsService)
        {
            this.blobsService = blobsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListBlobs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadBlob() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile blob)
        {
            if(blob != null && blob.Length > 0) 
            {
                await blobsService.UploadBlobAsync(blob.FileName);

                return RedirectToAction("ListBlobs");
            }

            return View();
        }
    }
}
