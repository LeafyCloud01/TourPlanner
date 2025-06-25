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

        public static TourList GetTourList(string Search)
        {
            TourList matchingTours = new TourList(); matchingTours.tours = [];

            if (Search == "") { return GetTourList(); }

            TourList tours = GetTourList();

            for(int i = 0; i < tours.tours.Count; i++)
            {
                bool includes_match = tours.tours[i].includesMatch(Search);
                if (includes_match) { matchingTours.tours.Add(tours.tours[i]); }
            }

            return matchingTours;
        }
    }
}
