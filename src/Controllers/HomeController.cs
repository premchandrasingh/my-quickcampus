using Microsoft.AspNetCore.Mvc;
using My.QuickCampus.Models;
using My.QuickCampus.QuickCampus;
using My.QuickCampus.Services;

namespace My.QuickCampus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuickCampusService _quickCampusService;

        public HomeController(ILogger<HomeController> logger, QuickCampusService quickCampusService)
        {
            _logger = logger;
            _quickCampusService = quickCampusService;
        }

        private string SetStudentName()
        {
            var studentName = _quickCampusService.GetStudentName();
            ViewData["StudentName"] = studentName;
            return studentName;
        }

        public IActionResult Index()
        {
            SetStudentName();
            return View();
        }

        public async Task<IActionResult> Homework()
        {
            var studentName = SetStudentName();

            HomeWorkViewModel homewordks = await _quickCampusService.GetHomeWorkAsync(studentName);
            return View(homewordks);
        }

        public async Task<IActionResult> Assignment()
        {
            var studentName = SetStudentName();
            HomeWorkViewModel assigments = await _quickCampusService.GetAssignmentAsync(studentName);
            return View(assigments);
        }

        public IActionResult DownloadFile(string filename)
        {
            return Redirect(filename ?? "/");
        }


        public async Task<IActionResult> ViewFile(string filename, string taskType, string fileType)
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(taskType))
            {
                return RedirectToAction("Index");
            }
            fileType = fileType ?? "pdf";

            var _taskType = taskType.ToLowerInvariant();
            if (_taskType != "homework" && _taskType != "assignment")
            {
                ViewData["ErrorMessage"] = "Invalid file type.";
                return View("Error");
            }

            var parts = filename.Split("___");
            if (parts.Length < 2)
            {
                return RedirectToAction("Index");
            }

            var _fileName = parts[1];
            var file = await _quickCampusService.GetAwsUrlAsync(_fileName, _taskType, fileType);

            // get bytes from the URL
            var httpFact = HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var client = httpFact.CreateClient();
            var response = await client.GetAsync(file.Url);
            if (!response.IsSuccessStatusCode)
            {
                ViewData["ErrorMessage"] = $"{response.StatusCode} - Failed to download the file.";
                return View("Error");
            }
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            if (fileBytes == null || fileBytes.Length == 0)
            {
                ViewData["ErrorMessage"] = "File is empty or not found.";
                return View("Error");
            }

            return File(fileBytes, "application/octet-stream", filename);
        }

        [HttpPost]
        public async Task<IActionResult> SyncHomework(int year, int month, string value)
        {
            var studentName = SetStudentName();
            var result = await _quickCampusService.SyncDataAsync(studentName, year, month, true);
            if (!result.IsSuccess || result.SyncedData == null)
            {
                ViewData["ErrorMessage"] = result.Message;
                return View("Error");
            }

            return View("Homework", result.SyncedData);
        }


        [HttpPost]
        public async Task<IActionResult> SyncAssignment(int year, int month, string value)
        {
            var studentName = SetStudentName();
            var result = await _quickCampusService.SyncDataAsync(studentName, year, month, true);
            if (!result.IsSuccess || result.SyncedData == null)
            {
                ViewData["ErrorMessage"] = result.Message;
                return View("Error");
            }

            return View("Assignment", result.SyncedData);
        }



        [HttpPost]
        public IActionResult SaveToken(SaveTokenBindingModel model, string value)
        {
            if (!string.IsNullOrEmpty(model.Token))
            {
                _quickCampusService.SetToken(model.Token);
            }
            return Redirect(value);
        }

        [HttpPost]
        public IActionResult SwitchStudent(string student, string value)
        {
            if (!string.IsNullOrEmpty(student))
            {
                _quickCampusService.SetStudentName(student);
            }

            return Redirect(value);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorCode = "Error", ErrorMessage = "An error occured." });
        }

    }
}
