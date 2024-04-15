namespace hackaton_file_import.common.Models
{
    public class TokenValidationSettings
    {
        public static string SectionName = "TokenValidation";

        public string OAuthServiceUrl { get; set; }
        public string VerifyTokenS2sPath { get; set; }
        public string AuthenticateS2sPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
    }
}
