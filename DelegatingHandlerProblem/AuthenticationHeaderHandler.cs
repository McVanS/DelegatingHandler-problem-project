using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DelegatingHandlerProblem
{
    public sealed class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public AuthenticationHeaderHandler(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string currentToken = await GetTokenAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(currentToken))
            {

                if (request.Headers.Contains("bearer"))
                {
                    request.Headers.Remove("bearer");
                }

                request.Headers.Add("bearer", currentToken);

            }

            return await base.SendAsync(request, cancellationToken);
        }

        public async Task<string> GetTokenAsync()
        {
            if (_protectedLocalStorage is null)
            {
                return string.Empty;
            }

            var cacheResult = await _protectedLocalStorage.GetAsync<string>("TokenPurpose", "TokenKey").ConfigureAwait(false);

            return (cacheResult.Success && cacheResult.Value is not null)
                ? cacheResult.Value
                : string.Empty;

        }
    }
}