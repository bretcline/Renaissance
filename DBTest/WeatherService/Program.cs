using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using ForecastIO;
using ForecastIO.Extensions;

namespace WeatherService
{
    class Program
    {
        // e47b5bff548c65eedbec88695b6b8f25
        static void Main(string[] args)
        {

            // 05b5076ddac68f83741aaeb31dce6516
            // latitude 43.525569 and longitude 5.438356

            var request = new ForecastIORequest("05b5076ddac68f83741aaeb31dce6516", 43.525569f, 5.438356f, Unit.si);
            var response = request.Get();

            // Date/Time is represented by a Unix Timestamp
            var currentTime = response.daily.data[0].time.ToDateTime().ToLocalTime();
            var queryTime = response.currently.time.ToDateTime( ).ToLocalTime();

            Console.WriteLine(response.daily.data[0].ToString());
            
            
        }
    }
}
