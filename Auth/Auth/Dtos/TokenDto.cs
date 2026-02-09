namespace Auth.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenDto(string access, string refresh) 
        {
            AccessToken = access;
            RefreshToken = refresh;
        }
    }
}
