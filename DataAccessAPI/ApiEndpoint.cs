using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessAPI
{
    public abstract class ApiEndpoint
    {
        public Uri BaseUrl { get; set; }
        public string Key { get; set; }
        public ApiEndpoint()
        {
            BaseUrl = new Uri("");
            Key = "";
        }
    }

    public class OpenRouteService : ApiEndpoint
    {
        public OpenRouteService() 
        {
            BaseUrl = new Uri("https://api.openrouteservice.org");
            Key = "This is where I would put my API key. IF I HAD ONE";
        }
    }

    public class OpenStreetMap : ApiEndpoint
    {
        public OpenStreetMap() 
        {
            BaseUrl = new Uri("https://tile.openstreetmap.org/");
        }
    }
}
