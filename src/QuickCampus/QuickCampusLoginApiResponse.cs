namespace My.QuickCampus.QuickCampus
{
    public class LoginResponse
    {
        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get; private set; }

        public QuickCampusLoginApiResponse QuickCampusResponse { get; private set; }

        public string AuthenticationToken
        {
            get
            {
                return QuickCampusResponse?.authenticationToken;
            }
        }



        public static LoginResponse Success(QuickCampusLoginApiResponse response)
        {
            return new LoginResponse()
            {
                IsSuccess = true,
                ErrorMessage = null,
                QuickCampusResponse = response
            };
        }

        public static LoginResponse Failed(string errorMessage)
        {
            return new LoginResponse()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                QuickCampusResponse = null
            };
        }
    }

    public class QuickCampusLoginApiResponse
    {
        public string authenticationToken { get; set; }

        public List<LoginUserModel> users { get; set; }

        public bool passwordResetCompleted { get; set; }
    }


    public class LoginUserModel
    {
        public int id { get; set; }
        public string uid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public int emailVerifiedCount { get; set; }
        public bool enabled { get; set; }
        public string schoolUid { get; set; }
        public string schoolName { get; set; }
        public string role { get; set; }
        public bool teaching { get; set; }
        public object departmentName { get; set; }
        public object departmentUid { get; set; }
        public string admissionNo { get; set; }
        public object role2 { get; set; }
        public string mobileNo { get; set; }
        public bool hasPermission { get; set; }
        public object employeeNo { get; set; }
        public object employeeUid { get; set; }
        public bool securityPinGenerated { get; set; }
        public bool emailVerified { get; set; }
    }
}
