
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Xml.Linq;

namespace BusinessLayer
{
    public enum Transport { Walking, Bicycle, Vehicle }

    public class TourList
    {
        public TourList()
        {
            tours = [];
        }

        public TourList(List<Tour> tours)
        {
            this.tours = tours;
        }

        public List<Tour> tours { get; set; }

        public bool ChangeTour(Tour tour)
        {
            int exists = -1;
            for (int i = 0; i < tours.Count; i++)
            {
                if (tours[i].ID == tour.ID) { exists = i; break; }
            }
            if(exists != -1)
            {
                tours[exists] = tour;
                return true;
            }
            else
            {
                tours.Add(tour);
                return false;
            }
        }
        public void DeleteTour(int tourID)
        {
            int exists = -1;
            for (int i = 0; i < tours.Count; i++)
            {
                if (tours[i].ID == tourID) { exists = i; break; }
            }
            tours.RemoveAt(exists);
        }

        public Tour? getTour(int tourID)
        {
            for (int i = 0; i < tours.Count; i++)
            {
                if (tours[i].ID == tourID) { return tours[i]; }
            }
            return null;
        }

        public void getTours(string SearchText)
        {
            List<Tour> matchingTours = [];
            for (int i = 0; i < tours.Count; i++)
            {
                bool includes_match = tours[i].includesMatch(SearchText);
                if (includes_match) { matchingTours.Add(tours[i]); }
            }
            tours = matchingTours;
        }

        public ObservableCollection<Tour> GetTours()
        {
            var tourDisplays = new ObservableCollection<Tour>();

            for (int i = 0; i < tours.Count; i++)
            {
                tourDisplays.Add(tours[i]);
            }

            return tourDisplays;
        }

        public bool ChangeTourLog(int tourID, TourLog logInfo)
        {
            bool isEdit = false;
            for (int i = 0; i < tours.Count; i++)
            {
                if (tours[i].ID == tourID)
                {
                    isEdit = tours[i].logs.ChangeLog(logInfo);
                    tours[i].UpdatePopularity();
                    tours[i].UpdateChildFriendliness();
                }
            }
            return isEdit;
        }

        public void DeleteTourLog(int tourID, int logID)
        {
            for (int i = 0; i < tours.Count; i++)
            {
                if (tours[i].ID == tourID)
                {
                    tours[i].logs.DeleteLog(logID);
                }
            }
        }

        internal bool generateReport()
        {
            throw new NotImplementedException();
        }
    }

    public class Tour
    {
        public Tour()
        {
            this.ID = -1;
            this.name = "";
            this.description = "";
            this.from = "";
            this.to = "";
            this.transportType = new();
            this.tourDistance = 0;
            this.estimatedTime = new();
            this.routeInformation = "";
            this.logs = new();
        }
        public Tour(int ID, string name, string description, string from, string to, Transport transportType, float tourDistance, TimeOnly estimatedTime, string routeInformation, LogList logs)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.from = from;
            this.to = to;
            this.transportType = transportType;
            this.tourDistance = tourDistance;
            this.estimatedTime = estimatedTime;
            this.routeInformation = routeInformation;
            this.logs = logs;

            UpdatePopularity();
            UpdateChildFriendliness();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public Transport transportType { get; set; }
        public float tourDistance { get; set; }
        public TimeOnly estimatedTime { get; set; }
        public string routeInformation { get; set; }

        public LogList logs;

        public int popularity = 0;
        public float childFriendliness = 0;

        [JsonIgnore] public string Name { get { return name; } }
        [JsonIgnore] public string Description { get { return description; } }
        [JsonIgnore] public string From { get { return from; } }
        [JsonIgnore] public string To { get { return to; } }
        [JsonIgnore] public string TransportType { get { return transportType.ToString(); } }
        [JsonIgnore] public float TourDistance { get { return tourDistance; } }
        [JsonIgnore] public TimeOnly EstimatedTime { get { return estimatedTime; } }
        [JsonIgnore] public string RouteInformation { get { return routeInformation; } }
        [JsonIgnore] public int Popularity { get { return popularity; } }
        [JsonIgnore] public float ChildFriendliness { get { return childFriendliness; } }

        public void UpdatePopularity()
        {
            popularity = logs.logs.Count;
        }
        public void UpdateChildFriendliness()
        {
            float difficulty = logs.CalculateDifficulty();
            float timeAverage = logs.CalculateAverageTimes();

            childFriendliness = 0; // add calculation using difficulty, timeAverage & tourDistance
        }

        public bool includesMatch(string Search)
        {
            //standard search values
            if(name.ToLower().Contains(Search.ToLower())) { return true; }
            if(from.ToLower().Contains(Search.ToLower())) { return true; }
            if(to.ToLower().Contains(Search.ToLower())) { return true; }
            if (TransportType.ToLower().Contains(Search.ToLower())) { return true; }

            // long text search values
            if (description.ToLower().Contains(Search.ToLower())) { return true; }

            // number search values
            if (Search.Contains("" + tourDistance)) { return true; }
            if (Search.Contains("" + estimatedTime.TimeSum())) { return true; }
            if (Search.Contains("" + popularity)) { return true; }
            if (Search.Contains("" + childFriendliness)) { return true; }

            // log search values
            if (logs.includesMatch("Search")) { return true; }

            return false;
        }

        internal bool generateReport()
        {
            throw new NotImplementedException();
        }
    }

    public class LogList
    {
        public LogList()
        {
            logs = [];
        }

        public List<TourLog> logs { get; set; }

        public bool ChangeLog(TourLog log)
        {
            int exists = -1;
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].ID == log.ID) { exists = i; break; }
            }
            if (exists != -1)
            {
                logs[exists] = log;
                return true;
            }
            else
            {
                logs.Add(log);
                return false;
            }
        }
        public void DeleteLog(int logID)
        {
            int exists = -1;
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].ID == logID) { exists = i; break; }
            }
            if(exists != -1) { logs.RemoveAt(exists); }
        }

        public float CalculateAverageTimes() 
        {
            float avgTimes = 0;

            for (int i = 0; i < logs.Count; i++)
            {
                avgTimes += logs[i].totalTime.TimeSum();
            }

            return avgTimes;
        } 
        public float CalculateDifficulty() 
        {
            float difficulty = 0;

            for(int i = 0; i < logs.Count; i++)
            {
                difficulty += logs[i].difficulty;
            }

            return difficulty/logs.Count;
        }

        public ObservableCollection<TourLog> GetLogs()
        {
            var logDisplays = new ObservableCollection<TourLog>();

            for (int i = 0; i < logs.Count; i++)
            {
                logDisplays.Add(logs[i]);
            }

            return logDisplays;
        }

        public bool includesMatch(string SearchText)
        {
            for (int i = 0; i < logs.Count; i++)
            {
                bool includes_match = logs[i].includesMatch(SearchText);
                if (includes_match) { return true; }
            }
            return false;
        }
    }
    public class TourLog
    {
        public TourLog()
        {
            this.ID = -1;
            this.dateTime = new();
            this.comment = "A comment";
            this.difficulty = 0;
            this.totalDistance = 0;
            this.totalTime = new();
            this.rating = 1;
        }
        public TourLog(int ID, DateTime dateTime, string comment, float difficulty, float totalDistance, TimeOnly totalTime, float rating) 
        { 
            this.ID = ID;
            this.dateTime = dateTime;
            this.comment = comment;
            this.difficulty = difficulty;
            this.totalDistance = totalDistance;
            this.totalTime = totalTime;
            this.rating = rating;
        }

        public int ID {  get; set; }
        public DateTime dateTime { get; set; }
        public string comment { get; set; }
        public float difficulty { get; set; }
        public float totalDistance { get; set; }
        public TimeOnly totalTime { get; set; }
        public float rating { get; set; }
        

        [JsonIgnore] public string Date { get { return dateTime.Date.ToString(); } }
        [JsonIgnore] public string Time { get { return dateTime.TimeOfDay.ToString(); } }
        [JsonIgnore] public string Comment { get { return comment; } }
        [JsonIgnore] public float Difficulty { get { return difficulty; } }
        [JsonIgnore] public float TotalDistance { get { return totalDistance; } }
        [JsonIgnore] public string TotalTime { get { return totalTime.TimeSum().ToString(); } }
        [JsonIgnore] public float Rating { get { return rating; } }

        public bool includesMatch(string Search)
        {
            //standard search values

            // long text search values
            if (comment.ToLower().Contains(Search.ToLower())) { return true; }

            // number search values
            if (Search.Contains("" + Date)) { return true; }
            if (Search.Contains("" + totalTime.TimeSum())) { return true; }
            if (Search.Contains("" + totalDistance)) { return true; }
            if (Search.Contains("" + difficulty)) { return true; }
            if (Search.Contains("" + rating)) { return true; }

            return false;
        }
    }

    public class Time
    {
        public Time()
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        public Time(int hours, int minutes, int seconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }

        public int hours;
        public int minutes;
        public int seconds;

        public float TimeSum()
        {
            return hours + (minutes / 10) + (seconds / 100);
        }
    }

}
