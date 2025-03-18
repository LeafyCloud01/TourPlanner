
namespace BusinessLayer
{
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
    }

    public class Tour
    {
        public Tour()
        {
            
        }

        public string name;
        public string description;
        public string from;
        public string to;
        public string transportType;
        public float tourDistance;
        public Time estimatedTime;
        public string routeInformation;

        public LogList logs;

        public int popularity;
        public float childFriendliness;

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
    }
    public class TourLog
    {
        public TourLog() { }

        public DateTime dateTime;
        public string comment;
        public float difficulty;
        public float totalDistance;
        public Time totalTime;
        public float rating;
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
