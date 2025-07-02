using DataAccessFiles;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BusinessLayer
{
    public class BusinessManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TourList GetTourList()
        {
            string toursString = AccessFiles.GetFileContent("Tours.json"); 
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
        private static void UpdateTourList(TourList tourList)
        {
            string toursString = JsonSerializer.Serialize<TourList>(tourList, new JsonSerializerOptions { WriteIndented = true });
            AccessFiles.SetFileContent("Tours.json", toursString);
        }

        public static void ChangeTour(Tour tour)
        {
            log.Info("Changing Tour: " + tour.ID);
            TourList tourList = GetTourList(); 
            tourList.ChangeTour(tour); 
            UpdateTourList(tourList);
        }
        public static void DeleteTour(int tourID)
        {
            log.Info("Deleting Tour: " + tourID);
            TourList tourList = GetTourList();
            tourList.DeleteTour(tourID);
            UpdateTourList(tourList);
        }
        public static void ChangeLog(int tourID, TourLog logInfo)
        {
            log.Info("Changing Log: " + logInfo.ID);
            TourList tourList = GetTourList();
            tourList.ChangeTourLog(tourID, logInfo);
            UpdateTourList(tourList);
        }
        public static void DeleteLog(int tourID, int logID)
        {
            log.Info("Deleting Log: " + logID);
            TourList tourList = GetTourList();
            tourList.DeleteTourLog(tourID, logID);
            UpdateTourList(tourList);
        }
    }
}
