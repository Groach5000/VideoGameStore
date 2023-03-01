namespace VideoGameStore.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
