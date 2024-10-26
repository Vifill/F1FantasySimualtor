using Microsoft.AspNetCore.Mvc.RazorPages;

namespace F1FantasySim.Pages
{
    public class BaseViewModel : PageModel
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";

        protected HttpRequestMessage CreateHttpRequestMessage(string url, string cookies)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", UserAgent);
            if (!string.IsNullOrWhiteSpace(cookies))
            {
                AddCookiesToRequest(request, cookies);
            }
            return request;
        }

        protected HttpRequestMessage CreatePostHttpRequestMessage(string url, string cookies)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("User-Agent", UserAgent);
            request.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            request.Headers.Add("Apikey", "fCUCjWrKPu9ylJwRAv8BpGLEgiAuThx7");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.7");
            //request.Headers.Add("Content-Type", "application/json");

            if (!string.IsNullOrWhiteSpace(cookies))
            {
                AddCookiesToRequest(request, cookies);
            }
            return request;
        }

        protected void AddCookiesToRequest(HttpRequestMessage request, string cookies)
        {
            if (!string.IsNullOrWhiteSpace(cookies))
            {
                request.Headers.Add("Cookie", cookies);
            }
        }

    }
}