using Goke.Core.Options;

namespace Goke.Aspnet.Core
{
    public interface IHubClientFactory
    {
        Task<HubClientService> CreateHubAsync(string hubName);
        Task<HubClientService> CreateHubAsync(string baseUrl, string path, string authProvider);
    }
}
