using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImageProcessor.Features.ImageAnalysis.Gateways
{
    public class VisionApiGateway
    {
        // How-To on getting the connectivity up and running
        // https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts-sdk/csharp-analyze-sdk
        
        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "54ac46816459495f94b032d6e48f5dd3";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the West US region, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the "westus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string VisionEndpoint = "https://northeurope.api.cognitive.microsoft.com";

        private static readonly ComputerVisionClient ComputerVisionClient = null;

        static VisionApiGateway()
        {
            ComputerVisionClient = new ComputerVisionClient(
               new ApiKeyServiceClientCredentials(subscriptionKey),
               new System.Net.Http.DelegatingHandler[] { });

            ComputerVisionClient.Endpoint = VisionEndpoint;
        }

        public async Task<ImageAnalysisData> AnalyzeBytes(byte[] imageBytes, Guid sessionId)
        {
            using (Stream imageStream = new MemoryStream(imageBytes))
            {
                var analysis = await ComputerVisionClient.AnalyzeImageInStreamAsync(imageStream,
                    details: new List<Details>() {Details.Celebrities});

                var celebrities = ExtractCelebritiesFromAnalysis(analysis);

                return new ImageAnalysisData()
                {
                    Celebrities = celebrities.Select(c => c.Name).ToList(), id = Guid.NewGuid()
                };
            }
        }

        private List<CelebritiesModel> ExtractCelebritiesFromAnalysis(Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models.ImageAnalysis analysis)
        {
            var result = analysis.Categories.First()?.Detail?.Celebrities?.ToList();
            return result ?? new List<CelebritiesModel>();
        }
    }
}
