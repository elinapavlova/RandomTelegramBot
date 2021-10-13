namespace Infrastructure.Configurations
{
    public class AppConfiguration
    {
        public string Token { get; }
        public AppConfiguration(string token)
        {
            Token = token;
        }
    }
}