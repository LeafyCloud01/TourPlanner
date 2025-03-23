
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BusinessLayer
{
    public enum Transport { }

    public class TourList
    {
        public TourList()
        {
            tours = [];
        }

        public List<Tour> tours;

        public void CreateTour() { }
        public void ModifyTour() { }
        public void DeleteTour() { }

        public ObservableCollection<Tour> GetTours()
        {
            var tourDisplays = new ObservableCollection<Tour>();

            for (int i = 0; i < tours.Count; i++)
            {
                tourDisplays.Add(tours[i]);
            }

            return tourDisplays;
        }
    }

    public class Tour
    {
        public Tour(string name, string description, string from, string to, Transport transportType, float tourDistance, Time estimatedTime, string routeInformation, LogList logs)
        {
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

        public string name { get; set; }
        public string Name { get { return name; } }
        public string description { get; set; }
        public string Description { get { return description; } }
        public string from { get; set; }
        public string From { get { return from; } }
        public string to { get; set; }
        public string To { get { return to; } }
        public Transport transportType { get; set; }
        public Transport TransportType { get { return transportType; } }
        public float tourDistance { get; set; }
        public float TourDistance { get { return tourDistance; } }
        public Time estimatedTime { get; set; }
        public Time EstimatedTime { get { return estimatedTime; } }
        public string routeInformation { get; set; }
        public string RouteInformation { get { return routeInformation; } }

        public LogList logs;

        public int popularity = 0;
        public int Popularity { get { return popularity; } }
        public float childFriendliness = 0;
        public float ChildFriendliness { get { return childFriendliness; } }

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
    }

    public class LogList
    {
        public LogList()
        {
            logs = [];
        }

        public List<TourLog> logs;

        public void CreateLog() { }
        public void ModifyLog() { }
        public void DeleteLog() { }

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

            return difficulty;
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
    }
    public class TourLog
    {
        public TourLog(DateTime dateTime, string comment, float difficulty, float totalDistance, Time totalTime, float rating) 
        { 
            this.dateTime = dateTime;
            this.comment = comment;
            this.difficulty = difficulty;
            this.totalDistance = totalDistance;
            this.totalTime = totalTime;
            this.rating = rating;
        }

        public DateTime dateTime { get; set; }
        public string DateTime { get { return dateTime.Date.ToString(); } }
        public string Time { get { return dateTime.TimeOfDay.ToString(); } }
        public string comment { get; set; }
        public string Comment { get { return comment; } }
        public float difficulty { get; set; }
        public float Difficulty { get { return difficulty; } }
        public float totalDistance { get; set; }
        public float TotalDistance { get { return totalDistance; } }
        public Time totalTime { get; set; }
        public string TotalTime { get { return totalTime.TimeSum().ToString(); } }
        public float rating { get; set; }
        public float Rating { get { return rating; } }
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
