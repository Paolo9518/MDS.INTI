using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDS.Inventario.Api.CrossCutting
{
    public static class Tools
    {
        public static byte[] ConvertToBytes(object obj)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BsonDataWriter(ms))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, new { Value = obj });
                    return ms.ToArray();
                }
            }
        }
    }
}
