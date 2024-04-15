namespace hackaton_file_import.common.Dtos
{
    internal class VerifyTokenRequestDto
    {
        public string UserToken { get; set; }
        public string[] UserRoles { get; set; }
    }
}