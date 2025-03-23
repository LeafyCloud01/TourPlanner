using DataAccessFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BusinessManager
    {
        public static TourList GetTourList()
        {
            string toursString = AccessFiles.GetFileContent("Tours");
            TourList? tourList = JsonSerializer.Deserialize<TourList>(toursString);

            return (tourList != null)? tourList : new TourList();
        }
    }
}
