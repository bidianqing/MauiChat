using MauiChat.Views;

namespace MauiChat.Services
{
    public class RequestService : IRequestService
    {
        private readonly IDialogService _dialogService;
        private readonly Lazy<HttpClient> _httpClient =
            new(() =>
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri(Urls.Domain);

                return httpClient;
            },
                LazyThreadSafetyMode.ExecutionAndPublication);

        public RequestService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpRequestMessage request = new(HttpMethod.Get, uri);

            return await this.SendAsync<TResult>(request);
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string json)
        {
            HttpRequestMessage request = new(HttpMethod.Post, uri);
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await this.SendAsync<TResult>(request);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data) where TRequest : class, new()
        {
            HttpRequestMessage request = new(HttpMethod.Post, uri);
            string content = JsonSerializer.Serialize(data, typeof(TRequest), new JsonSerializerOptions
            {

            });
            request.Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await this.SendAsync<TResult>(request);
        }

        public async Task<TResult> SendAsync<TResult>(HttpRequestMessage request)
        {
            string token = Preferences.Default.Get("jwt_token", "");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.Value.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.Value.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            return await this.HandleResponse<TResult>(response);
        }

        private async Task<TResult> HandleResponse<TResult>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var stream = await response.Content.ReadAsStringAsync();
                var resultModel = JsonSerializer.Deserialize<ResultModel<TResult>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (!resultModel.Success)
                {
                    await _dialogService.ShowAlertAsync(null, resultModel.Message, "确定");
                }

                return resultModel.Data;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return default(TResult);
            }

            await _dialogService.ShowAlertAsync(null, $"服务器开小差了，状态码：{(int)response.StatusCode}，时间：{DateTime.Now}", "确定");

            return default(TResult);
        }
    }
}
