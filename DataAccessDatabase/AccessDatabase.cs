//using BusinessLayer;
//using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;

namespace DataAccessDatabase
{
    public class AccessDatabase
    {

        public AccessDatabase ()
        {
        }
        public static List<Tour> GetTours()
        {
            List<Tour> Tours;
            using (var context = new TourPlannerContext())
            {
                Tours = context.Tours.ToList();
            }
            return Tours;
        }

        public static List<Log> GetLogs(int TourId) 
        {
            List<Log> Logs;
            using (var context = new TourPlannerContext())
            {
                Logs = context.Logs.Where(l => l.Tour.Equals(TourId)).ToList();
            }
            return Logs;
        }

        public static void ChangeTour(Tour Tour)
        {
            using (var context = new TourPlannerContext())
            {
               var oldTour = context.Tours.Where(t => t.TourId.Equals(Tour.TourId)).SingleOrDefault();
                if (oldTour == null)
                {
                    context.Tours.Add(Tour);
                }
                else
                {
                    Tour.Logs = oldTour.Logs;
                    oldTour = Tour;
                }
                context.SaveChanges();
            }
        }

        public static void ChangeLog(Log Log)
        {
            using (var context = new TourPlannerContext())
            {
                var oldLog = context.Logs.Where(l => l.Tour.Equals(Log.Tour)).SingleOrDefault();
                if (oldLog != null)
                {
                    context.Logs.Add(Log);
                }
                else
                {
                    oldLog = Log;
                }
                context.SaveChanges();
            }
        }
        public static void DeleteTour(int TourId)
        {
            using (var context = new TourPlannerContext())
            {
                var Tour = context.Tours.Where(t => t.TourId.Equals(TourId)).SingleOrDefault();
                if (Tour != null)
                {
                    var TourLogs = context.Logs.Where(l => l.Tour.Equals(TourId)).ToList();
                    foreach (var TourLog in TourLogs)
                    {
                        context.Logs.Remove(TourLog);
                    }
                    context.Tours.Remove(Tour);
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteLog(int LogId) 
        {
            using (var context = new TourPlannerContext())
            {
                var Log = context.Logs.Where(l => l.LogId.Equals(LogId)).SingleOrDefault();
                if (Log != null) 
                { 
                    context.Logs.Remove(Log); 
                    context.SaveChanges();
                }
            }
        }
    }
    
}
