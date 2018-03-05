using System.Threading.Tasks;

namespace DoggoBot.CognitiveServices
{
    public interface IComputerVision
    {
        Task<string> MakeAnalysisRequest(byte[] imageData);
    }
}