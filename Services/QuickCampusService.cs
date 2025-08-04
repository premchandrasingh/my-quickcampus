using Microsoft.EntityFrameworkCore;
using My.QuickCampus.Data;
using My.QuickCampus.Entities;
using My.QuickCampus.QuickCampus;
using My.QuickCampus.Result;
using System.Net;

namespace My.QuickCampus.Services
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
        private readonly AppDbContext _dbContext;
        private HttpContext _httpContext;


        public QuickCampusService(ILogger<QuickCampusService> logger
            , IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor
            , AppDbContext dbContext)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _httpContext = _httpContextAccessor.HttpContext;
        }

        public async Task<QuickCampusApiResponse> GetHomeWorkAsync(string student, DateTime? startDate = null, DateTime? endDate = null)
        {
            // c224de9e-459c-4890-a438-41e8415082ea
            var now = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);

            var _startDate = (startDate ?? new DateTime(now.Year, now.Month, 1)).ToString("dd-MM-yyyy");
            var _endDate = (endDate ?? new DateTime(now.Year, now.Month, lastDayOfMonth)).ToString("dd-MM-yyyy");
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
            else
            {
                _logger.LogWarning("Failed to fetch homework data. Status Code: {StatusCode}", response.StatusCode);
            }
            return null;
        }

        public async Task<QuickCampusApiResponse> GetAssignmentAsync(string student, DateTime? startDate = null, DateTime? endDate = null)
        {
            var now = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);

            var _startDate = (startDate ?? new DateTime(now.Year, now.Month, 1)).ToString("dd-MM-yyyy");
            var _endDate = (endDate ?? new DateTime(now.Year, now.Month, lastDayOfMonth)).ToString("dd-MM-yyyy");
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
            else
            {
                _logger.LogWarning("Failed to fetch assignment data. Status Code: {StatusCode}", response.StatusCode);
            }
            return null;
        }


        public async Task<QuickCampusAwsUrlApiResponse> GetHomeworkAwsUrl(string fileName)
        {
            return await GetAwsUrlAsync(fileName, "homework");
        }

        public async Task<QuickCampusAwsUrlApiResponse> GetAssignmentAwsUrl(string fileName)
        {
            return await GetAwsUrlAsync(fileName, "assigment");
        }

        public async Task<QuickCampusAwsUrlApiResponse> GetAwsUrlAsync(string fileName, string type = "assigment")
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var payload = new { fileName, tagname = type == "assigment" ? "academic-cms-assignment-doc" : "academic-cms-homework-doc" };
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


        public async Task<SyncResult> SyncDataAsync(string studentName, int year, int month, bool isHomework)
        {

            var dbDrade = await _dbContext.Grades
                .Where(x => x.Student.Name == studentName && x.IsCurrentClass)
                .FirstOrDefaultAsync();

            if (dbDrade == null)
            {
                _logger.LogWarning("No current grade found for student {StudentName}", studentName);
                return null;
            }

            var syncType = isHomework ? "homework" : "assignment";

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var currentLastDayOfMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var currentEndDay = new DateTime(currentYear, currentMonth, currentLastDayOfMonth);

            var lastDayOfMonth = DateTime.DaysInMonth(year, month);
            var startDay = new DateTime(year, month, 1);
            var endDay = new DateTime(year, month, lastDayOfMonth);

            if (endDay >= currentEndDay)
            {
                _logger.LogWarning("Cannot sync data before month end {Year}-{Month}", year, month);
                return SyncResult.Failed(syncType, SyncResult.FailedType.MonthNotEndError, $"Cannot sync data before month end ${year}-${month}");
            }

            var dbSyncRec = await _dbContext.QuickCampusSyncs
               .Where(x => x.GradeId == dbDrade.GradeId && x.SourceYear == year && x.SourceMonth == month && x.SyncType == syncType)
               .FirstOrDefaultAsync();

            if (dbSyncRec != null)
            {
                _logger.LogWarning("Data already synced. {Year}-{Month}", year, month);
                return SyncResult.Failed(syncType, SyncResult.FailedType.AlreadySynced, $"Data already synced. {year}-{month}");
            }

            QuickCampusApiResponse data = null;
            var affected = 0;
            if (isHomework)
            {
                data = await GetHomeWorkAsync(studentName, startDay, endDay);
                var homeworks = data.Data.Select(x => x.ConvertToHomework(dbDrade.GradeId)).ToList();
                if (homeworks.Count > 0)
                {
                    _dbContext.Homeworks.AddRange(homeworks);
                    affected = await _dbContext.SaveChangesAsync();
                }
                _logger.LogInformation("Homework synced: {Count}", homeworks.Count);
            }
            else
            {
                data = await GetAssignmentAsync(studentName, startDay, endDay);
                var assignments = data.Data.Select(x => x.ConvertToAssignment(dbDrade.GradeId)).ToList();
                if (assignments.Count > 0)
                {
                    _dbContext.Assignments.AddRange(assignments);
                    affected = await _dbContext.SaveChangesAsync();
                }
                _logger.LogInformation("Assignments synced: {Count}", assignments.Count);
            }

            if (affected > 0)
            {
                var syncRecord = new QuickCampusSync
                {
                    GradeId = dbDrade.GradeId,
                    SourceYear = year,
                    SourceMonth = month,
                    SyncType = syncType,
                    Status = "Success",
                    Message = $"{affected} '{syncType.ToUpper()}' records synced successfully for {year}-{month}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _dbContext.QuickCampusSyncs.Add(syncRecord);
                await _dbContext.SaveChangesAsync();

                return SyncResult.Success(syncType, data, syncRecord.Message);
            }

            _logger.LogInformation("No '{SyncType}' record synced for {Year}-{Month}", syncType.ToUpper(), year, month);
            return SyncResult.Success(syncType, null, $"No '{syncType.ToUpper()}' record synced.");
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
                jsonOpt.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                jsonOpt.Converters.Add(new JsonDateOnlyConverter("dd-MM-yyyy"));
                jsonOpt.Converters.Add(new JsonDateConverter("dd-MM-yyyy"));

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
