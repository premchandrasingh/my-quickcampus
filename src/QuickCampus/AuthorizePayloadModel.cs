namespace My.QuickCampus.QuickCampus
{
    public class AuthorizePayloadModel
    {
        public string authenticationToken { get; set; }

        //public string deviceId { get; set; }

        public string deviceToken { get; set; }

        public string securityPin { get; set; }

        public string userName { get; set; }
    }
}
