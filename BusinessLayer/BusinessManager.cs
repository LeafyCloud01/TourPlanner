using DataAccessAPI;
using DataAccessDatabase;
using DataAccessFiles;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Jsoup.Nodes;
using log4net;
using log4net.Config;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static log4net.Appender.RollingFileAppender;

namespace BusinessLayer
{
    public class BusinessManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string DataAccessType = "File";

        public static Tour GetMapImage(Tour tour)
        {
            tour.routeInformation = AccessAPI.GetMapCoords(tour.ParseCoordinates());
            return tour;
        }

        public static Tour GetRoute(Tour tour)
        {
            var task = AccessAPI.GetRouteData(tour.ParseCoordinates(), (int)tour.transportType);
            var result = task.Result;
            var json = JsonNode.Parse(result);
            if (json != null)
            {
                tour.tourDistance = (float)json["routes"]["summary"]["distance"];
                tour.estimatedTime.Add(TimeSpan.FromSeconds((double)json["routes"]["summary"]["duration"]));
            }
            return tour;
        }

        public static TourList GetTourList()
        {
            switch (DataAccessType)
            {
                case "DB":
                    return GetTourListDb();
                case "File":
                    return GetTourListFile();
            }
            return new TourList();
        }

        public static TourList GetTourListFile()
        {
            string toursString = AccessFiles.GetFileContent("Tours.json"); 
            TourList? tourList = JsonSerializer.Deserialize<TourList>(toursString);

            return tourList ?? new TourList();
        }

        public static TourList GetTourListDb()
        {
            TourList tourList = new();
            var Tours = AccessDatabase.GetTours();
            foreach (var t in Tours) 
            {
                tourList.ChangeTour(new BusinessLayer.Tour(t));
            }
            foreach (var tour in tourList.tours)
            {
                if (tour != null)
                {
                    var Logs = AccessDatabase.GetLogs(tour.ID);
                    foreach (var l in Logs) 
                    {
                        tour.logs.ChangeLog(new TourLog(l));    
                    }
                }
            }
            return tourList;
        }

        public static TourList GetTourList(string Search)
        {
            TourList matchingTours = new TourList(); matchingTours.tours = [];

            if (Search == "") { return GetTourList(); }

            TourList tours = GetTourList();

            tours.getTours(Search);

            return matchingTours;
        }
        private static void UpdateTourList(TourList tourList)
        {
            string toursString = JsonSerializer.Serialize<TourList>(tourList, new JsonSerializerOptions { WriteIndented = true });
            AccessFiles.SetFileContent("Tours.json", toursString);
        }
        private static void UpdateTourList_DeleteTour(int tourID)
        {
            AccessDatabase.DeleteTour(tourID);
        }
        private static void UpdateTourList_ChangeTour(Tour tour)
        {
            var NewTour = tour.Transform(new DataAccessDatabase.Tour());
            AccessDatabase.ChangeTour(NewTour);

        }
        private static void UpdateTourList_DeleteLog(int tourID, int logID)
        {
            AccessDatabase.DeleteLog(logID);
        }
        private static void UpdateTourList_ChangeLog(int tourID, TourLog log)
        {
            var NewLog = log.Transform(new DataAccessDatabase.Log(), tourID);
            AccessDatabase.ChangeLog(NewLog);
        }

        public static void ChangeTour(Tour tour)
        {
            log.Info("Changing Tour: " + tour.ID);
            TourList tourList = GetTourList();
            tour = GetMapImage(tour);
            tour = GetRoute(tour);
            tourList.ChangeTour(tour);

            UpdateTourList(tourList);
            UpdateTourList_ChangeTour(tour);
        }
        public static void DeleteTour(int tourID)
        {
            log.Info("Deleting Tour: " + tourID);
            TourList tourList = GetTourList();
            tourList.DeleteTour(tourID);

            UpdateTourList(tourList);
            UpdateTourList_DeleteTour(tourID);
        }
        public static void ChangeLog(int tourID, TourLog logInfo)
        {
            log.Info("Changing Log: " + logInfo.ID);
            TourList tourList = GetTourList();
            tourList.ChangeTourLog(tourID, logInfo);

            UpdateTourList(tourList);
            UpdateTourList_ChangeLog(tourID, logInfo);
        }
        public static void DeleteLog(int tourID, int logID)
        {
            log.Info("Deleting Log: " + logID);
            TourList tourList = GetTourList();
            tourList.DeleteTourLog(tourID, logID);

            UpdateTourList(tourList);
            UpdateTourList_DeleteLog(tourID, logID);
        }

        public static bool ExportTour(int currentTourID, string Format)
        {
            Tour tourToExport = GetTourList().getTour(currentTourID);
            if (tourToExport == null) 
            { 
                log.Error("Unable to export tour: tour ID not found in Tour List."); 
                return false;
            }
            else
            {
                bool success = AccessFiles.Export<Tour>(Format, tourToExport);
                
                if(success == true) { log.Info("Successfully Imported JSON File"); }
                else { log.Info("Unable to export JSON File. An error occured during Exporting."); }

                return success;
            } 
        }

        public static bool ImportTour(string Format)
        {
            Tour? importedTour = AccessFiles.Import<Tour>(Format);
            if(importedTour != null) 
            { 
                ChangeTour(importedTour);
                log.Info("Successfully Imported JSON File");
                return true;
            }
            else { 
                log.Error("Error Importing JSON. Tour received is null.");
                return false;
            }
        }

        public static bool GenerateReport(int CurrentTourID, string Type)
        {
            string reportPath = "";

            switch (Type)
            {
                case "tour_report":
                    reportPath = AccessFiles.getExportPath("Generate Report");
                    Tour reportedTour = GetTourList().getTour(CurrentTourID);
                    return reportedTour.generateReport(reportPath);

                case "summarize_report":
                    reportPath = AccessFiles.getExportPath("Generate Report");
                    TourList tours = GetTourList();
                    return tours.generateReport(reportPath);
            }

            return false;
        }
    }
}
