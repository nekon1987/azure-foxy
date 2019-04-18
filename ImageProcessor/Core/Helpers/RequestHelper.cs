using System;
using System.IO;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ImageProcessor.Core.Helpers
{
    public class RequestHelper
    {
        public FoxyResponse<ImageData> ExtractSingleImageFromRequest(HttpRequest request)
        {
            try
            {
                var result = new ImageData();
                var formFile = request.Form.Files[0];

                result.Name = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim().Value;
                using (var reader = new BinaryReader(formFile.OpenReadStream()))
                {
                    result.Bytes = reader.ReadBytes(Convert.ToInt32(formFile.Length));
                }

                return FoxyResponse<ImageData>.Success(result);
            }
            catch (Exception e)
            {
                return FoxyResponse<ImageData>.Failure(e.Message);
            }
           
        }
    }
}
