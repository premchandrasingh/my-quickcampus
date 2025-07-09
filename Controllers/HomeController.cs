using Microsoft.AspNetCore.Mvc;
using My.QuickCampus.Models;
using System.Diagnostics;

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

            var homewordks = await _quickCampusService.GetHomeWorkAsync(studentName);
            return View(homewordks);
        }

        public async Task<IActionResult> Assignment()
        {
            var studentName = SetStudentName();
            var assigments = await _quickCampusService.GetAssignmentAsync(studentName);
            return View(assigments);
        }

        public IActionResult DownloadFile(string value)
        {
            return Redirect(value ?? "/");
        }


        public async Task<IActionResult> ViewFile(string filename)
        {
            var file = await _quickCampusService.GetAssignmentAwsUrl(filename);
            return Redirect(file.Url);
        }

        [HttpPost]
        public IActionResult Sync(int year, int month, string value)
        {
            return Redirect(value ?? "/");
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
