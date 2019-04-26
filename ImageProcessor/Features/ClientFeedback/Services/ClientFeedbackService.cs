using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using ImageProcessor.Features.ClientFeedback.Factories;
using ImageProcessor.Features.ImageAnalysis.Gateways;
using SendGrid.Helpers.Mail;

namespace ImageProcessor.Features.ClientFeedback.Services
{
    public class ClientFeedbackService
    {
        private static readonly SendGridMessageFactory SendGridMessageFactory = new SendGridMessageFactory();
        public async Task<FoxyResponse<SendGridMessage>> CreateAnalysisCompletedEmail(string userEmail, ImageAnalysisData analysisResult)
        {
            try
            {
                var message = SendGridMessageFactory.CreateAnalysisCompletedMessage(userEmail, analysisResult);
                return FoxyResponse<SendGridMessage>.Success(message);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, analysisResult.id);
                return FoxyResponse<SendGridMessage>.Failure("Could not store image analysis results");
            }
        }
    }
}
