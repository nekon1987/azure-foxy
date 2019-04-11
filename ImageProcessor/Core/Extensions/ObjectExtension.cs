using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Newtonsoft.Json;

namespace ImageProcessor.Core.Extensions
{
    public static class ObjectExtension
    {
        public static dynamic ToDynamic(this object sourceObject)
        {
            string serializedObject = JsonConvert.SerializeObject(sourceObject);
            dynamic dynamicObject = JsonConvert.DeserializeObject(serializedObject);
            return dynamicObject;
        }
    }
}
