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
        

        public static TourList GetTourListFile()
        {
            string toursString = AccessFiles.GetFileContent("Tours.json"); 
            TourList? tourList = JsonSerializer.Deserialize<TourList>(toursString);

            return (tourList != null)? tourList : new TourList();
        }

        public static TourList GetTourListDb()
        {
            TourList tourList = new TourList();
            var Tours = AccessDatabase.GetTours();
            foreach (var t in Tours) 
            {
                float distance = (t.Distance ==null)? 0 : (float)t.Distance;
                TimeOnly duration = (t.Duration == null) ? new TimeOnly() : (TimeOnly)t.Duration;
                string name = (t.Name == null) ? "" : t.Name;
                string description = (t.Description == null) ? "" : t.Description;
                string information = (t.Information == null) ? "" : t.Information;
                Transport transport;
                Enum.TryParse(t.TransportType, out transport);
                tourList.ChangeTour(new BusinessLayer.Tour(t.TourId, name, description, t.FromCoord, t.ToCoord, transport, distance, duration, information, new LogList()));
            }
            foreach (var tour in tourList.tours)
            {
                if (tour != null)
                {
                    var Logs = AccessDatabase.GetLogs(tour.ID);
                    foreach (var l in Logs) 
                    {
                        TimeOnly totaltime = (l.TotalTime == null) ? new TimeOnly() : (TimeOnly)l.TotalTime;
                        int rating = (l.Rating == null) ? 0 : (int)l.Rating;
                        string comment = (l.Comment == null) ? "" : l.Comment;
                        tour.logs.ChangeLog(new TourLog(l.LogId, l.DateCreated, comment, l.Difficulty, l.TotalDistance, totaltime, rating));    
                    }
                }
            }
            return tourList;
        }

        public static TourList GetTourList(string Search)
        {
            TourList matchingTours = new TourList(); matchingTours.tours = [];

            if (Search == "") { return GetTourListDb(); }

            TourList tours = GetTourListDb();

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
            var NewTour = TransformToDb(tour);
            AccessDatabase.ChangeTour(NewTour);

        }
        private static void UpdateTourList_DeleteLog(int tourID, int logID)
        {
            AccessDatabase.DeleteLog(logID);
        }
        private static void UpdateTourList_ChangeLog(int tourID, TourLog log)
        {
            var NewLog = TransformToDb(log, tourID);
            AccessDatabase.ChangeLog(NewLog);
        }

        private static DataAccessDatabase.Tour TransformToDb(Tour tour)
        {
            var DbTour = new DataAccessDatabase.Tour();
            DbTour.TourId = tour.ID;
            DbTour.Name = tour.Name;
            DbTour.Description = tour.Description;
            DbTour.FromCoord = tour.From;
            DbTour.ToCoord = tour.To;
            DbTour.TransportType = tour.TransportType;
            DbTour.Distance = tour.tourDistance;
            DbTour.Duration = tour.estimatedTime;
            DbTour.Information = tour.routeInformation;
            return DbTour;
        }

        private static Log TransformToDb(TourLog Log, int TourId)
        {
            var DbLog = new Log();
            DbLog.LogId = Log.ID;
            DbLog.DateCreated = Log.dateTime;
            DbLog.Comment = Log.comment;
            DbLog.Difficulty = (int)Log.difficulty;
            DbLog.TotalDistance = Log.totalDistance;
            DbLog.TotalTime = Log.totalTime;
            DbLog.Rating = (int)Log.Rating;
            DbLog.Tour = TourId;
            return DbLog;
        }

        public static void ChangeTour(Tour tour)
        {
            log.Info("Changing Tour: " + tour.ID);
            TourList tourList = GetTourListDb();
            tourList.ChangeTour(tour);

            UpdateTourList(tourList);
            UpdateTourList_ChangeTour(tour);
        }
        public static void DeleteTour(int tourID)
        {
            log.Info("Deleting Tour: " + tourID);
            TourList tourList = GetTourListDb();
            tourList.DeleteTour(tourID);

            UpdateTourList(tourList);
            UpdateTourList_DeleteTour(tourID);
        }
        public static void ChangeLog(int tourID, TourLog logInfo)
        {
            log.Info("Changing Log: " + logInfo.ID);
            TourList tourList = GetTourListDb();
            tourList.ChangeTourLog(tourID, logInfo);

            UpdateTourList(tourList);
            UpdateTourList_ChangeLog(tourID, logInfo);
        }
        public static void DeleteLog(int tourID, int logID)
        {
            log.Info("Deleting Log: " + logID);
            TourList tourList = GetTourListDb();
            tourList.DeleteTourLog(tourID, logID);

            UpdateTourList(tourList);
            UpdateTourList_DeleteLog(tourID, logID);
        }

        public static bool ExportTour(int currentTourID, string Format)
        {
            Tour tourToExport = GetTourListDb().getTour(currentTourID);
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
                    Tour reportedTour = GetTourListDb().getTour(CurrentTourID);
                    return reportedTour.generateReport();

                case "summarize_report":
                    reportPath = AccessFiles.getExportPath("Generate Report");
                    TourList tours = GetTourListDb();
                    return tours.generateReport();
            }

            return false;
        }
    }
}
