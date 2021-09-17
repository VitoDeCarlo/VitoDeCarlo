using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace VitoDeCarlo.Core.Services
{
    public class TwilioVerifyService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public TwilioVerifyService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<TwilioSendVerificationCodeResponse> StartVerification(int countryCode, string phoneNumber)
        {
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("via", "sms"),
                new KeyValuePair<string, string>("country_code", countryCode.ToString()),
                new KeyValuePair<string, string>("phone_number", phoneNumber),
            });

            var request = new HttpRequestMessage(HttpMethod.Post, 
                "https://api.authy.com/protected/json/phones/verification/start");
            request.Headers.Add("X-Authy-API-Key", _configuration["Twilio:VerifyApiKey"]);
            request.Content = requestContent;
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            // this will throw if the response is not valid
            return JsonConvert.DeserializeObject<TwilioSendVerificationCodeResponse>(content);
        }

        public async Task<TwilioCheckCodeResponse> CheckVerificationCode(int countryCode, string phoneNumber, string verificationCode)
        {
            var queryParams = new Dictionary<string, string>()
            {
                {"country_code", countryCode.ToString()},
                {"phone_number", phoneNumber},
                {"verification_code", verificationCode },
            };

            var url = QueryHelpers.AddQueryString("https://api.authy.com/protected/json/phones/verification/check", queryParams);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Authy-API-Key", _configuration["Twilio:VerifyApiKey"]);
            var response = await _client.SendAsync(request);

            //var response = await _client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            // this will throw if the response is not valid
            return JsonConvert.DeserializeObject<TwilioCheckCodeResponse>(content);
        }

        public class TwilioCheckCodeResponse
        {
            public string Message { get; set; } = null!;
            public bool Success { get; set; }
        }

        public class TwilioSendVerificationCodeResponse
        {
            public string Carrier { get; set; } = null!;
            public bool IsCellphone { get; set; }
            public string Message { get; set; } = null!;
            public string SecondsToExpire { get; set; } = null!;
            public Guid Uuid { get; set; }
            public bool Success { get; set; }
        }
    }
}
