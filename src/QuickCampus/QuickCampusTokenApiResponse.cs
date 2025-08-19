namespace My.QuickCampus.QuickCampus
{
    public class TokenResponse
    {
        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get; private set; }

        public QuickCampusTokenApiResponse QuickCampusResponse { get; private set; }

        public string AcccessToken
        {
            get
            {
                return QuickCampusResponse?.accessToken;
            }
        }

        public string RrefreshToken
        {
            get
            {
                return QuickCampusResponse?.refreshToken;
            }
        }

        public static TokenResponse Success(QuickCampusTokenApiResponse response)
        {
            return new TokenResponse()
            {
                IsSuccess = true,
                ErrorMessage = null,
                QuickCampusResponse = response
            };
        }

        public static TokenResponse Failed(string errorMessage)
        {
            return new TokenResponse()
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                QuickCampusResponse = null
            };
        }
    }

    public class QuickCampusTokenApiResponse
    {
        public int id { get; set; }

        public string uid { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public string schoolUid { get; set; }

        public string admissionNo { get; set; }

        public string rol { get; set; }

        public string accessToken { get; set; }

        public string refreshToken { get; set; }

        public string deviceId { get; set; }

        public string mobileNo { get; set; }

        public string studentInfoUid { get; set; }

        public string schoolName { get; set; }

        public object partnerUid { get; set; }

        public object partnerId { get; set; }

        public object parentPartnerUid { get; set; }

        public object parentPartnerId { get; set; }

        public object parentPartnerReferralUid { get; set; }

        public bool userEmailVerified { get; set; }

        public int emailVerifiedCount { get; set; }

        public bool passwordResetCompleted { get; set; }

        public bool emailVarified { get; set; }

        public bool whatsAppEnabled { get; set; }

        public bool smsenabled { get; set; }

        public bool notificationEnabled { get; set; }

        public bool securityPinGenerated { get; set; }

        public bool loanEnabled { get; set; }

        public bool paymentCompleted { get; set; }

        public string employeeUid { get; set; }

        public List<SchoolAllotment> schoolAllotments { get; set; }

        public List<AcademicSession> academicSessions { get; set; }
    }


    public class SchoolAllotment
    {
        public Module module { get; set; }


        public List<SubModule> subModule { get; set; }
    }


    public class Module
    {
        public string uid { get; set; }

        public string moduleName { get; set; }

        public int priority { get; set; }

        public int accessTypeStaff { get; set; }

        public int accessTypeStudent { get; set; }
    }


    public class SubModule
    {
        public string uid { get; set; }

        public string subModuleName { get; set; }

        public string entityType { get; set; }

        public int priority { get; set; }

        public bool showOnAllotment { get; set; }
    }


    public class UserPermissionsResponse
    {
        public string userName { get; set; }

        public UserResponse userResponse { get; set; }
    }

    public class UserResponse
    {
        public object id { get; set; }

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

        public string role2 { get; set; }

        public string mobileNo { get; set; }

        public bool hasPermission { get; set; }

        public object employeeNo { get; set; }

        public object employeeUid { get; set; }

        public bool securityPinGenerated { get; set; }

        public bool emailVerified { get; set; }
    }


    public class AcademicSession
    {
        public int id { get; set; }

        public string uid { get; set; }

        public string sessionTitle { get; set; }

        public string fromDate { get; set; }

        public string toDate { get; set; }

        public int priority { get; set; }

        public bool registration { get; set; }

        public bool studentSession { get; set; }

        public bool enabled { get; set; }

        public string createdUser { get; set; }

        public string createdDate { get; set; }

        public string editedUser { get; set; }

        public string editedDate { get; set; }
    }

}
