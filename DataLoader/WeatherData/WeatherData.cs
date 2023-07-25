using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForecastIO;
using ForecastIO.Extensions;
using JaxisExtensions;


namespace WeatherData
{
    public class WeatherData
    {
        public void LoadData( string key, float lat, float lon )
        {

            // 05b5076ddac68f83741aaeb31dce6516
            // latitude 43.525569 and longitude 5.438356

            var request = new ForecastIORequest(key, lat, lon, Unit.si);
            var response = request.Get();


            Console.WriteLine(response.daily.data[0].ToString());
            var queryTime = response.currently.time.ToDateTime().ToLocalTime();

            var weather = new Jaxis.POS.Data.DailyWeatherForecast();

            weather.QueryDate = queryTime;
            weather.Lat = Convert.ToDecimal(lat);
            weather.Lon = Convert.ToDecimal(lon);

            CopyData(response.daily.data[0], weather);

            weather.Save();

        }

        protected void CopyData( DailyForecast daily, Jaxis.POS.Data.DailyWeatherForecast data)
        {
            // Date/Time is represented by a Unix Timestamp
            var currentTime = daily.time.ToDateTime().ToLocalTime();

            data.ForecastDate = currentTime;

            data.ApparentTemperatureMax = daily.apparentTemperatureMax;
            data.ApparentTemperatureMaxTime = daily.apparentTemperatureMaxTime;
            data.ApparentTemperatureMin = daily.apparentTemperatureMin;
            data.ApparentTemperatureMinTime = daily.apparentTemperatureMinTime;
            data.CloudCover = daily.cloudCover;
            data.DewPoint = daily.dewPoint;
            data.Humidity = daily.humidity;
            data.Icon = daily.icon;
            data.MoonPhase = daily.moonPhase;
            data.Ozone = daily.ozone;
            data.PrecipAccumulation = daily.precipAccumulation;
            data.PrecipIntensity = daily.precipIntensity;
            data.PrecipIntensityMax = daily.precipIntensityMax;
            data.PrecipIntensityMaxTime = daily.precipIntensityMaxTime;
            data.PrecipProbability = daily.precipProbability;
            data.PrecipType = daily.precipType;
            data.Pressure = daily.pressure;
            data.Summary = daily.summary;
            data.SunriseTime = daily.sunriseTime;
            data.SunsetTime = daily.sunsetTime;
            data.TemperatureMax = daily.temperatureMax;
            data.TemperatureMaxTime = daily.temperatureMaxTime;
            data.TemperatureMin = daily.temperatureMin;
            data.TemperatureMinTime = daily.temperatureMinTime;
            data.Visibility = daily.visibility;
            data.WindBearing = daily.windBearing;
            data.WindSpeed = daily.windSpeed;
        }
    }
}
