using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DebugTools
{
    public class Jsonizer
    {
        public static void GimmeDatJson<T>(T obj)
        {
            var opt = new JsonSerializerOptions() { 
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string strJson = JsonSerializer.Serialize(obj, opt);
            Debug.WriteLine(strJson);
        }
    }
}
