namespace VideoGameStore.Models
{
    public class AuthenticateResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
