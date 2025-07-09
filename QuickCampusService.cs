using My.QuickCampus.QuickCampus;
using System.Net;

namespace My.QuickCampus
{
    public class QuickCampusService
    {
        private const string TOKEN_COOKIENAME = "quickcampus_token";
        private const string STUDENT_COOKIENAME = "student_name";
        private Dictionary<string, string> _studentMap = new Dictionary<string, string>()
        {
            { "lia", "31090e22-1408-4cac-9faf-6fc6df06de26" },
            { "leo", "f321032d-0af6-4379-ad5a-09a880518855" },
            { "lenin", "c224de9e-459c-4890-a438-41e8415082ea" }
        };

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;


        public QuickCampusService(ILogger<QuickCampusService> logger
            , IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
        }

        public async Task<QuickCampusApiResponse> GetHomeWorkAsync(string student, DateTime? startDate = null, DateTime? endDate = null)
        {
            // c224de9e-459c-4890-a438-41e8415082ea
            var now = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);

            var _startDate = (startDate ?? new DateTime(now.Year, now.Month, 1)).ToString("dd-MM-yyyy");
            var _endDate = (startDate ?? new DateTime(now.Year, now.Month, lastDayOfMonth)).ToString("dd-MM-yyyy");
            ;
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var studentId = _studentMap.GetValueOrDefault(student.ToLowerInvariant(), null);
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await client.GetAsync($"https://erpapi.quickcampus.online/student/academic-post-type?studentInfoUid={studentId}&type=HOMEWORK&fromDate={_startDate}&toDate={_endDate}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = Deserialize<QuickCampusApiResponse>(content);
                    return data;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing QuickCampusApiResponse");
                }
            }
            return null;
        }

        public async Task<QuickCampusApiResponse> GetAssignmentAsync(string student, DateTime? startDate = null, DateTime? endDate = null)
        {
            var now = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);

            var _startDate = (startDate ?? new DateTime(now.Year, now.Month, 1)).ToString("dd-MM-yyyy");
            var _endDate = (startDate ?? new DateTime(now.Year, now.Month, lastDayOfMonth)).ToString("dd-MM-yyyy");
            ;
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var studentId = _studentMap.GetValueOrDefault(student.ToLowerInvariant(), null);

            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await client.GetAsync($"https://erpapi.quickcampus.online/student/academic-post-type?studentInfoUid={studentId}&type=ASSIGNMENT&fromDate={_startDate}&toDate={_endDate}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = Deserialize<QuickCampusApiResponse>(content);
                    return data;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing QuickCampusApiResponse");
                }
            }
            return null;
        }

        public async Task<QuickCampusAwsUrlApiResponse> GetHomeworkAwsUrl(string fileName)
        {
            return await GetAwsUrl(fileName, "homework");
        }

        public async Task<QuickCampusAwsUrlApiResponse> GetAssignmentAwsUrl(string fileName)
        {
            return await GetAwsUrl(fileName, "assigment");
        }

        private async Task<QuickCampusAwsUrlApiResponse> GetAwsUrl(string fileName, string type = "assigment")
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var payload = new { fileName = fileName, tagname = type == "assigment" ? "academic-cms-assignment-doc" : "" };
            var response = await client.PutAsJsonAsync($"https://erpapi.quickcampus.online/users/aws-get-url", payload);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = Deserialize<QuickCampusAwsUrlApiResponse>(content);
                    return data;
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing QuickCampusAwsUrlApiResponse");
                }
            }
            return null;
        }



        public string GetToken()
        {
            if (_httpContext.Request.Cookies.TryGetValue(TOKEN_COOKIENAME, out var token))
                return token;
            return null;
        }

        public string GetStudentName()
        {
            if (_httpContext.Request.Cookies.TryGetValue(STUDENT_COOKIENAME, out var strudent))
                return strudent;
            return "Lia";
        }

        public void SetToken(string token)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(30);
            cookieOptions.Path = "/";
            _httpContext.Response.Cookies.Append(TOKEN_COOKIENAME, token, cookieOptions);
        }

        public void SetStudentName(string studentName)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(30);
            cookieOptions.Path = "/";
            _httpContext.Response.Cookies.Append(STUDENT_COOKIENAME, studentName, cookieOptions);
        }


        private T Deserialize<T>(string content)
        {
            try
            {
                var jsonOpt = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                };
                return System.Text.Json.JsonSerializer.Deserialize<T>(content, jsonOpt);
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing content to type {Type}", typeof(T).Name);
                return default;
            }
        }
    }
}
