using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace Goke.Aspnet.Core
{
    public interface IHubClientService
    {
        bool IsConnected { get; }
        HubConnectionState State { get; }

        event Func<Task>? Closed;
        event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;
        event Func<Task>? Reconnected;
        event Func<Task>? Reconnecting;

        ValueTask DisposeAsync();
        Task EnsureConnectedAsync(CancellationToken cancellationToken = default);
        Task InitializeAsync(Uri? hubUri, Func<Task<string?>>? accessTokenProvider = null, Func<HttpMessageHandler>? httpMessageHandlerFactory = null, CookieContainer? cookies = null);
        Task<TResult> InvokeAsync<TResult>(string methodName, params object[] args);
        IDisposable On(string methodName, Action handler);
        IDisposable On<T>(string methodName, Action<T> handler);
        IDisposable On<T1, T2, T3, T4, T5>(string methodName, Action<T1, T2, T3, T4, T5> handler);
        IDisposable On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> handler);
        IDisposable On<T1, T2, T3>(string methodName, Action<T1, T2, T3> handler);
        IDisposable On<T1, T2>(string methodName, Action<T1, T2> handler);
        void RemoveHandler(string methodName);
        Task SendAsync(string methodName, params object[] args);
    }
}