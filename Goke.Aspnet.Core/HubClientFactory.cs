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
        ICookieProvider cookieProvider
        ) : IHubClientFactory
    {

        //private readonly HubOptions hubOptions = options.Value;
        HubClientService? hub;

        /// <summary>
        /// hubName is in HubOptions
        /// </summary>
        /// <param name="hubName"></param>
        /// <returns></returns>
        public async Task<HubClientService> CreateHubAsync(string hubName)
        {
            var hubOptions = options.Value;
            var option = hubOptions.Connections.First(h => h.HubName.Equals(hubName, StringComparison.OrdinalIgnoreCase));

            hub = await CreateHubAsync(hubOptions.BaseUrl, option.Path, option.AuthProvider);
            return hub;
        }

        public async Task<HubClientService> CreateHubAsync(string baseUrl, string path, string authProvider)
        {
            hub ??= authProvider switch
            {
                "Cookie" => await CreateCookieHubAsync(baseUrl, path),
                "AccessToken" => await CreateAccessTokenHubAsync(baseUrl, path),
                _ => throw new InvalidOperationException($"Unsupported auth provider: {authProvider}"),
            };

            return hub;
        }


        protected async Task<HubClientService> CreateCookieHubAsync(string baseUrl, string hubPath)
        {
            var service = new HubClientService();

            await service.InitializeAsync(
                hubUri: CreateHubUri(baseUrl, hubPath),
                cookies: await CreateAuthCookies());

            return service;
        }

        protected async Task<HubClientService> CreateAccessTokenHubAsync(string baseUrl, string hubPath)
        {
            var service = new HubClientService();

            await service.InitializeAsync(
                hubUri: CreateHubUri(baseUrl, hubPath),
                accessTokenProvider: GetAccessTokenAsync
                );

            return service;
        }

        private static Uri CreateHubUri(string baseUrl, string relativePath) => new(new Uri(baseUrl), relativePath);

        private Task<string?> GetAccessTokenAsync() => accessTokenProvider.GetAccessTokenAsync();

        private Task<CookieContainer?> CreateAuthCookies() => cookieProvider.CreateAuthCookies();
    }
}
