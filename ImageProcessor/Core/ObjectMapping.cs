using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImageProcessor.Core.Extensions;
using ImageProcessor.EventModels;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Build.Framework;

namespace ImageProcessor.Core
{
    public class ObjectMapping
    {
        static ObjectMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
                cfg.CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
            });
        }

        public TDestination Convert<TSource, TDestination>(TSource source, TDestination destination)
        {
            var result =  Mapper.Map(source, destination);
            destination = result;
            return result;;
        }
    }
}
