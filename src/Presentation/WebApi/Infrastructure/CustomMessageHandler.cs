using Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Infrastructure
{
    public class CustomMessageHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string corsWhiteListStr = WebSetting.Get("CorsWhiteList");
            IList<string> whiteList = corsWhiteListStr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            HttpResponseMessage response;
            if (request.Method == HttpMethod.Options)
            {
                response = new HttpResponseMessage();
                response.Headers.Add("Access-Control-Allow-Origin", whiteList);
                response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                response = await base.SendAsync(request, cancellationToken);
                response.Headers.Add("Access-Control-Allow-Origin", whiteList);
            }

            return response;
        }
    }
}