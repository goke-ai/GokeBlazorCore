namespace Goke.Aspnet.Core
{
    public interface IHubClientFactory
    {
        Task<HubClientService> CreateCookieHubAsync(string hubPath);
        Task<HubClientService> CreateAccessTokenHubAsync(string hubPath);
        Task<HubClientService> CreateHubAsync();
    }
}
