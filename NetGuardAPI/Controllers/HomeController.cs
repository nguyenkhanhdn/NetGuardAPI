using Microsoft.AspNetCore.Mvc;
using NetGuardAPI.Models;
using System.Diagnostics;

namespace NetGuardAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TextPredict()
        {
            return View();
        }
        public IActionResult TrainModel()
        {
            SentimentEngine se  = new SentimentEngine();
            se.TrainingModel();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ImagePredict()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ImgPredict()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<SentimentPrediction>> ImgPredict(IFormFile fileUpload)
        {
            if (fileUpload != null && fileUpload.Length > 0)
            {
                var fileext = Path.GetExtension(fileUpload.FileName);
                var myUniqueFileName = Guid.NewGuid() + fileext;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", myUniqueFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await fileUpload.CopyToAsync(stream);
                }

                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                NetGaurd4KidMLModel.ModelInput modelInput = new NetGaurd4KidMLModel.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                //var sortedScoresWithLabel = ImgMLModel.Predict(sampleData);
                var sortedScoresWithLabel = NetGaurd4KidMLModel.PredictAllLabels(modelInput);

               
                var maxScoreLabel = sortedScoresWithLabel.OrderByDescending(x => x.Value).FirstOrDefault();
                var result = new PredictResult
                {
                    Prediction = maxScoreLabel.Key.ToString(),
                    Probability = maxScoreLabel.Value
                };

                ViewBag.Prediction = maxScoreLabel.Key.ToString();
                ViewBag.Probability = maxScoreLabel.Value;
            }
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
}
