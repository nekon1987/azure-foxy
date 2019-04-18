using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImageProcessor.Core.Extensions;
using ImageProcessor.EventModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Build.Framework;
using Newtonsoft.Json.Linq;

namespace ImageProcessor.Core
{
    public class ObjectMapping
    {
        static ObjectMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                // https://stackoverflow.com/questions/47473087/using-automapper-to-map-from-dynamic-jobject-to-arbitrary-types-without-creating
                // Problem with mapping from JObjects...

                cfg.CreateMap<JValue, object>().ConvertUsing(s => s.Value);
                cfg.CreateMap<JValue, string>().ConvertUsing(s => System.Convert.ToString(s.Value));
                cfg.CreateMap<JValue, Guid>().ConvertUsing(s => Guid.Parse(s.Value.ToString()));
            });
        }

        public TDestination Convert<TSource, TDestination>(TSource source, TDestination destination)
        {
            var result = Mapper.Map(source, destination);
            return result;
        }
    }
}
