using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DoggoBot.CognitiveServices;
using Newtonsoft.Json.Linq;

namespace DoggoBot
{
    public class Discord : IDiscord
    {
        private readonly IComputerVision _computerVision;

        public Discord(IComputerVision computerVision)
        {
            _computerVision = computerVision;
        }

        public async Task MessageReceived(SocketMessage message)
        {
            if (!message.Author.IsBot)
            {
                var hasDoggo = false;

                foreach (var attachment in message.Attachments)
                {
                    hasDoggo = await HasDoggo(attachment.Url);                 
                }

                foreach (var embed in message.Embeds)
                {
                    hasDoggo = await HasDoggo(embed.Url);
                }

                if (message.Attachments.Any() || message.Embeds.Any())
                {
                    if (hasDoggo)
                    {
                        await message.Channel.SendMessageAsync("Doggo Image Removed!");
                        await message.DeleteAsync(new RequestOptions { AuditLogReason = "Doggo Image" });                     
                    }
                }
            }
        }

        private async Task<bool> HasDoggo(string url)
        {
            var imageData = Helpers.GetImageAsByteArray(url);

            if (imageData != null)
            {
                var imageDescriptionResult = await _computerVision.MakeAnalysisRequest(imageData);
                var parsedJson = JObject.Parse(imageDescriptionResult);

                var imageDescriptionTags = (from tag in parsedJson["tags"]
                                            select new ComputerVisionTag
                                            {
                                                Name = tag["name"].ToString(),
                                                Confidence = decimal.Parse(tag["confidence"].ToString())
                                            }).AsQueryable();

                return imageDescriptionTags.Any(tag => tag.Name == "dog" && tag.Confidence > 0.85m);
            }

            return false;
        }
    }
}