using Goke.Core.Authentication;
using Goke.Core.Options;
using Goke.Core.Services;
using Microsoft.Extensions.Options;
using System.Net;

namespace Goke.Aspnet.Core
{
    public class HubClientFactory(
        IOptions<HubOptions> options,
        IAccessTokenProvider accessTokenProvider, 
        ICookieProvider cookieProvider): IHubClientFactory
    {

        private readonly HubOptions hubOptions = options.Value;
        HubClientService? hub;

        public async Task<HubClientService> CreateHubAsync()
        {
            var option = hubOptions.Connections.FirstOrDefault(h => h.HubName.Equals("chat", StringComparison.OrdinalIgnoreCase));

            hub ??= option.AuthProvider switch
            {
                "Cookie" => await CreateCookieHubAsync(option.Path),
                "AccessToken" => await CreateAccessTokenHubAsync(option.Path),
                _ => throw new InvalidOperationException($"Unsupported auth provider: {option.AuthProvider}"),
            };

            return hub;
        }

        public async Task<HubClientService> CreateCookieHubAsync(
        string hubPath)
        {
            var service = new HubClientService();

            await service.InitializeAsync(
                hubUri: CreateHubUri(hubPath),
                cookies: await CreateAuthCookies());

            return service;
        }

        public async Task<HubClientService> CreateAccessTokenHubAsync(
        string hubPath)
        {
            var service = new HubClientService();

            await service.InitializeAsync(
                hubUri: CreateHubUri(hubPath),
                accessTokenProvider: GetAccessTokenAsync
                );

            return service;
        }

        private Uri CreateHubUri(string relativePath) => new(new Uri(hubOptions.BaseUrl), relativePath);

        private Task<string?> GetAccessTokenAsync() => accessTokenProvider.GetAccessTokenAsync();

        private Task<CookieContainer?> CreateAuthCookies() => cookieProvider.CreateAuthCookies();
    }
}
