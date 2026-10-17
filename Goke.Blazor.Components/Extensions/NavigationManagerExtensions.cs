using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;

namespace Goke.Blazor.Components.Extensions
{
    public static class NavigationManagerExtensions
    {
        extension(NavigationManager navigationManager)
        {
            public NameValueCollection GetQueryStringCollection()
            {
                var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return query;
            }

            public string? GetQueryStringValue(string key)
            {
                var query = navigationManager.GetQueryStringCollection();
                return query[key];
            }

            // indexer to get query string value by key
            public NameValueCollection QueryString => navigationManager.GetQueryStringCollection();
        }
    }
}
