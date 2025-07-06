using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace DataAccessAPI
{
    public class AccessAPI
    {
        public AccessAPI()
        {
        }

        public static async Task<string> GetRouteData(double fromlat, double fromlon, double tolat, double tolon)
        {
            OpenRouteService OpenRouteService = new();
            using (var http = new HttpClient { BaseAddress = OpenRouteService.BaseUrl })
            {
                http.DefaultRequestHeaders.Clear();
                http.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
                http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", $"{OpenRouteService.Key}");

                using (var content = new StringContent($"{{\"coordinates\":[[{fromlat},{fromlon}],[{tolat},{tolon}]]}}"))
                {
                    using (var response = await http.PostAsync("/v2/directions/driving-car", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
            }
        }

        public static List<int> GetMapCoords(double fromlat, double fromlon, double tolat, double tolon)
        {
            OpenStreetMap OpenStreetMap = new();
            List<double> Coords = [fromlat, fromlon, tolat, tolon];
            var result = CalculateMap(Coords, 15);
            return result;
        }

        private static List<int> CalculateMap(List<double> Coords, int zoom)
        {
            if (zoom < 0)
            {
                return new List<int> { 0 };
            }
            var res1 = CalculateCoords(Coords[0], Coords[1], zoom);
            var res2 = CalculateCoords(Coords[2], Coords[3], zoom);
            if (res1[0] == res2[0] && res1[1] == res2[1])
            {
                res1.Add(zoom);
                return res1;
            }
            else
            {
                var result = CalculateMap(Coords, zoom-1);
                return result;
            }
        }

        private static List<int> CalculateCoords(double lon, double lat, int zoom)
        {
            List<int> coords = new List<int>();
            int x = (int)(Math.Floor((lon + 180.0) / 360.0 * (1 << zoom)));
            var latRad = lat / 180 * Math.PI;
            int y = (int)Math.Floor((1 - Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI) / 2 * (1 << zoom));
            coords.Add(x);
            coords.Add(y);
            return coords;
        }
    }
}
