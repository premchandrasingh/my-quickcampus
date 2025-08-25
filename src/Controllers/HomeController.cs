using Microsoft.AspNetCore.Mvc;
using My.QuickCampus.Models;
using My.QuickCampus.Services;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

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


        public async Task<IActionResult> ViewFile(string filename, string taskType, string fileType, string title)
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
            if (file == null || string.IsNullOrEmpty(file.Url))
            {
                _logger.LogError("Failed to get file name from AWS. {filename}", _fileName);
                return ErrorPrivate($"Failed to get file name from AWS. {_fileName}", "GetAwsUrlFailed");
            }

            // get bytes from the URL
            var httpFact = HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var client = httpFact.CreateClient();
            var response = await client.GetAsync(file.Url);
            if (!response.IsSuccessStatusCode)
            {
                return ErrorPrivate($"{response.StatusCode} - Failed to download the file.", "AWSFileDownloadFailed");
            }
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return ErrorPrivate("File is empty or not found.", "EmptyFile");
            }

            // Add file name to the downloaded pdf file
            if (Path.GetExtension(_fileName).ToLower() == ".pdf")
            {
                try
                {
                    PdfDocument pdfDocument = PdfReader.Open(new MemoryStream(fileBytes), PdfDocumentOpenMode.Modify, PdfSharpCore.Pdf.IO.enums.PdfReadAccuracy.Moderate);
                    // Get the first page of the document.
                    PdfPage page = pdfDocument.Pages[0];
                    // Create a graphics object to draw on the page.
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Define the font and size.
                    XFont font = new XFont("Arial", 10, XFontStyle.Regular);

                    parts = filename.Split("__", 4);
                    // Define the text to be added.
                    string text = filename;
                    if (parts.Length >= 3)
                        text = $"{parts[2]}:- {filename}";

                    XColor customColor = XColor.FromArgb(181, 230, 29); // custom light green color
                    var textBg = parts[0] == "iii" ? new XSolidBrush(customColor) : XBrushes.Yellow;
                    int xPos = 35;
                    // Draw the text on the page.
                    gfx.DrawString(text, font, XBrushes.Red, new XPoint(xPos, 25));
                    if (!string.IsNullOrEmpty(title))
                    {
                        var bgWidth = gfx.MeasureString(title, font).Width;
                        gfx.DrawRectangle(textBg, xPos, 30, bgWidth, 15);
                        gfx.DrawString(title, font, XBrushes.Red, new XPoint(xPos, 40));
                    }

                    _logger.LogInformation("File name added to the downloaded pdf file - {filename}", text);

                    var stream2 = new MemoryStream();
                    pdfDocument.Save(stream2, true);
                    return File(stream2.ToArray(), "application/octet-stream", filename);
                }
                catch (Exception ex)
                {
                    _logger.LogError("FAILED to add file name to the downloaded pdf file. Error - {error}", ex.Message);
                }
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
                return ErrorPrivate(result.Message);
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
                return ErrorPrivate(result.Message);

            }

            return View("Assignment", result.SyncedData);
        }

        [HttpPost]
        public async Task<IActionResult> DirectLogin(string value)
        {
            var result = await _quickCampusService.GetTokenAsnc("lia");
            if (result.IsSuccess)
                _quickCampusService.SetToken(result.AcccessToken);
            else
                return ErrorPrivate(result.ErrorMessage, "LoginError");

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
        [HttpGet]
        public IActionResult CompressPdf()
        {
            return View();
        }

        /*
        [HttpPost]
        public IActionResult CompressPdf(string filePath)
        {
            // https://github.com/iron-software/IronPdf.Examples/blob/main/how-to/pdf-compression/section3.cs

            var pdf = IronPdf.PdfDocument.FromFile(filePath);
            var compressionOptions = new IronPdf.CompressionOptions();

            // Configure image compression
            compressionOptions.CompressImages = true;
            compressionOptions.JpegQuality = 70;
            compressionOptions.HighQualityImageSubsampling = false;
            compressionOptions.ShrinkImages = true;
            // Configure tree structure compression
            compressionOptions.RemoveStructureTree = true;

            pdf.Compress(compressionOptions);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var outputPath = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, $"{fileNameWithoutExtension}_compressed.pdf");
            pdf.SaveAs(outputPath);

            return View();
        }
        */



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return ErrorPrivate("An error occured.");
        }

        private IActionResult ErrorPrivate(string errorMessage, string errorCode = "Error")
        {
            return View("Error", new ErrorViewModel() { ErrorMessage = errorMessage, ErrorCode = errorCode });
        }



    }
}
