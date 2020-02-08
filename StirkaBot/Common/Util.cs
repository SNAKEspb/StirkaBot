using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot.Common
{
    public class Util
    {
        public static Random _random = new Random();

        public static string getRawBody(Stream bodyStream)
        {
            using (StreamReader reader = new StreamReader(bodyStream, System.Text.Encoding.UTF8))
            {
                var result = reader.ReadToEnd();
                return result;
            }
        }

        public static async Task<string> getRawBodyAsync(Stream bodyStream)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(bodyStream, System.Text.Encoding.UTF8))
            {
                var result = await reader.ReadToEndAsync();
                return result;
            }
        }
    }
}
