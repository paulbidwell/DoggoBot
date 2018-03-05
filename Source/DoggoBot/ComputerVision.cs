using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DoggoBot.CognitiveServices;
using Microsoft.Extensions.Configuration;

namespace DoggoBot
{
    public class ComputerVision : IComputerVision
    {
        private readonly IConfiguration _configuration;

        public ComputerVision(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> MakeAnalysisRequest(byte[] imageData)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration["ComputerVisionApiKey"]);

            var uri = $"{_configuration["ComputerVisionEndpoint"]}?visualFeatures=Tags&language=en";

            using (var content = new ByteArrayContent(imageData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}