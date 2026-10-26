using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace Goke.Aspnet.Core
{
    public class HubClientService() : IAsyncDisposable
    {
        private HubConnection? connection;
        private readonly List<IDisposable> subscriptions = [];

        private bool disposed;

        Uri? hubUri;
        Func<Task<string?>>? accessTokenProvider = null;
        Func<HttpMessageHandler>? httpMessageHandlerFactory = null;
        CookieContainer? cookies = null;


        public HubConnectionState State => connection?.State ?? HubConnectionState.Disconnected;
        public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

        private void OnConnectionStateChanged(HubConnectionState state, string message)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(state, message));
        }

        public bool IsConnected => connection?.State == HubConnectionState.Connected;

        public event Func<Task>? Reconnected;
        public event Func<Task>? Reconnecting;
        public event Func<Task>? Closed;

        public async Task InitializeAsync(
            Uri? hubUri,
            Func<Task<string?>>? accessTokenProvider = null,
            Func<HttpMessageHandler>? httpMessageHandlerFactory = null,
            CookieContainer? cookies = null)
        {
            this.hubUri = hubUri;
            this.accessTokenProvider = accessTokenProvider;
            this.httpMessageHandlerFactory = httpMessageHandlerFactory;
            this.cookies = cookies;

             await EnsureConnectedAsync();
        }

        public async Task EnsureConnectedAsync(CancellationToken cancellationToken = default)
        {
            if (connection is not null)
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    OnConnectionStateChanged(HubConnectionState.Connecting, "Connecting to location sharing...");
                    await connection.StartAsync(cancellationToken);
                    OnConnectionStateChanged(connection.State, "Connected to location sharing.");
                }
                return;
            }

            connection = new HubConnectionBuilder()
                .WithUrl(hubUri!, options =>
                {
                    if (accessTokenProvider is not null)
                    {
                        options.AccessTokenProvider = accessTokenProvider;
                    }

                    if (httpMessageHandlerFactory is not null)
                    {
                        options.HttpMessageHandlerFactory = _ => httpMessageHandlerFactory();
                    }

                    if (cookies is not null)
                    {
                        options.Cookies = cookies;
                    }
                })
                .WithAutomaticReconnect()
                .Build();


            connection.Reconnecting += OnReconnecting;

            connection.Reconnected += OnReconnected;

            connection.Closed += OnClosed;

            OnConnectionStateChanged(HubConnectionState.Connecting, "Connecting to chat hub...");
            await connection.StartAsync(cancellationToken);
            OnConnectionStateChanged(connection.State, "Connected to chat hub.");
        }

        public IDisposable On(string methodName, Action handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            var subscription = connection.On(methodName, handler);
            subscriptions.Add(subscription);
            return subscription;
        }

        public IDisposable On<T>(
            string methodName,
            Action<T> handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            return Track(connection.On(methodName, handler));
        }

        public IDisposable On<T1, T2>(
            string methodName,
            Action<T1, T2> handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            return Track(connection.On(methodName, handler));
        }

        public IDisposable On<T1, T2, T3>(
            string methodName,
            Action<T1, T2, T3> handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            return Track(connection.On(methodName, handler));
        }

        public IDisposable On<T1, T2, T3, T4>(
            string methodName,
            Action<T1, T2, T3, T4> handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            return Track(connection.On(methodName, handler));
        }

        public IDisposable On<T1, T2, T3, T4, T5>(
            string methodName,
            Action<T1, T2, T3, T4, T5> handler)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            return Track(connection.On(methodName, handler));
        }


        public async Task<TResult> InvokeAsync<TResult>(
                                        string methodName,
                                        params object[] args)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection not initialized.");
            }

            return args.Length switch
            {
                1 => await connection.InvokeAsync<TResult>(methodName, args[0]),
                2 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1]),
                3 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2]),
                4 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3]),
                5 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4]),
                6 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4], args[5]),
                7 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6]),
                8 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]),
                9 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]),
                10 => await connection.InvokeAsync<TResult>(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]),
                _ => await connection.InvokeAsync<TResult>(methodName),
            };
        }

        public async Task SendAsync(
                            string methodName,
                            params object[] args)
        {
            ThrowIfDisposed();

            if (connection is null)
            {
                throw new InvalidOperationException(
                    "Hub connection is not initialized.");
            }

            switch (args.Length)
            {
                case 1: await connection.SendAsync(methodName, args[0]); break;
                case 2: await connection.SendAsync(methodName, args[0], args[1]); break;
                case 3: await connection.SendAsync(methodName, args[0], args[1], args[2]); break;
                case 4: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3]); break;
                case 5: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4]); break;
                case 6: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4], args[5]); break;
                case 7: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break;
                case 8: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break;
                case 9: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); break;
                case 10: await connection.SendAsync(methodName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]); break;
                default: await connection.SendAsync(methodName); break;
            }
        }



        public void RemoveHandler(string methodName)
        {
            connection?.Remove(methodName);
        }

        public IDisposable Track(IDisposable subscription)
        {
            ObjectDisposedException.ThrowIf(
                disposed,
                nameof(HubClientService));

            subscriptions.Add(subscription);

            return subscription;
        }

        public void Untrack(IDisposable subscription)
        {
            if (subscriptions.Remove(subscription))
            {
                subscription.Dispose();
            }
        }


        public void ClearSubscriptions()
        {
            foreach (var subscription in subscriptions)
            {
                try
                {
                    subscription.Dispose();
                }
                catch
                {
                    // optional logging
                }
            }

            subscriptions.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            //
            // Dispose SignalR method handlers
            //
            ClearSubscriptions();

            //
            // Release delegates
            //
            Reconnected = null;
            Reconnecting = null;
            Closed = null;

            if (connection is not null)
            {
                //
                // Unsubscribe connection lifecycle handlers
                //
                connection.Reconnecting -= OnReconnecting;
                connection.Reconnected -= OnReconnected;
                connection.Closed -= OnClosed;

                await connection.DisposeAsync();

                connection = null;
            }

            GC.SuppressFinalize(this);
        }



        private async Task OnReconnecting(
            Exception? exception)
        {
            OnConnectionStateChanged(HubConnectionState.Reconnecting, "Reconnecting to chat hub...");

            if (Reconnecting is not null)
            {
                await Reconnecting.Invoke();
            }
        }

        private async Task OnReconnected(
            string? connectionId)
        {
            OnConnectionStateChanged(connection.State, "Connected to chat hub...");

            if (Reconnected is not null)
            {
                await Reconnected.Invoke();
            }
        }

        private async Task OnClosed(
            Exception? exception)
        {
            OnConnectionStateChanged(HubConnectionState.Disconnected, "Disconnected from chat hub.");
            if (Closed is not null)
            {
                await Closed.Invoke();
            }
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(
                disposed,
                nameof(HubClientService));
        }
    }

    public sealed record ConnectionStateChangedEventArgs(HubConnectionState State, string Message);
}
