using System;
using System.Collections.Generic;
using System.Text;
using ImageProcessor.Core;
using ImageProcessor.DomainModels;
using SendGrid.Helpers.Mail;

namespace ImageProcessor.Features.ClientFeedback.Factories
{
    public class SendGridMessageFactory
    {
        public SendGridMessage CreateAnalysisCompletedMessage(string userEmail, ImageAnalysisData analysisResult)
        {
            var message = new SendGridMessage();
            message.AddTo(userEmail);
            message.AddContent("text/html", string.Join(", ", analysisResult.Celebrities));
            message.SetFrom(new EmailAddress(ConfigurationManager.SendGrid.SystemEmailAddress));
            message.SetSubject("Results of automated image analysis");
            return message;
        }
    }
}
