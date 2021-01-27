using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Models;
using IdentityModel.Client;

namespace Authorization.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<PrincipalAuthorizationResponse> RequestAuthorizationsAsync(this HttpMessageInvoker client, PrincipalAuthorizationsRequest request, CancellationToken cancellationToken = default)
        {
            request.Prepare();
            request.Method = HttpMethod.Post;
            
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<PrincipalAuthorizationResponse>(ex);
            }

            return await ProtocolResponse.FromHttpResponseAsync<PrincipalAuthorizationResponse>(response);
        }

        
    }
}