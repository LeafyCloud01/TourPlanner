
using DataAccessDatabase;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Bouncycastleconnector;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Xml.Linq;

namespace BusinessLayer
{
    public enum Transport { Walking, Hiking, Bicycle, Vehicle }

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

        public ObservableCollection<Tour> Transform(ObservableCollection<Tour> TourDisplays)
        {
            for (int i = 0; i < tours.Count; i++)
            {
                TourDisplays.Add(tours[i]);
            }

            return TourDisplays;
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

        public bool generateReport(string ReportPath)
        {
            var writer = new PdfWriter(ReportPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            Table t1 = new Table(4).UseAllAvailableWidth()
                .AddHeaderCell("Name")
                .AddHeaderCell("Average Time")
                .AddHeaderCell("Average Distance")
                .AddHeaderCell("Average Rating");

            for (int i = 0; i < tours.Count; i++)
            {
                t1.AddCell(tours[i].name);
                t1.AddCell(tours[i].getAverageTime() + "");
                t1.AddCell(tours[i].getAverageDistance() + "");
                t1.AddCell(tours[i].getAverageRating() + "");
            }

            document.Add(t1);
            document.Close();

            return true;
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
        public Tour(DataAccessDatabase.Tour t)
        {
            float distance = (t.Distance == null) ? 0 : (float)t.Distance;
            TimeOnly duration = (t.Duration == null) ? new TimeOnly() : (TimeOnly)t.Duration;
            if (!Enum.TryParse(t.TransportType, out Transport transport))
            {
                transport = Transport.Walking;
            }

            this.ID = t.TourId;
            this.name = t.Name ?? "";
            this.description = t.Description ?? "";
            this.from = t.FromCoord;
            this.to = t.ToCoord;
            this.transportType = transport;
            this.tourDistance = distance;
            this.estimatedTime = duration;
            this.routeInformation = t.Information ?? "";
            this.logs = new();

            UpdatePopularity();
            UpdateChildFriendliness();
        }

        public DataAccessDatabase.Tour Transform(DataAccessDatabase.Tour DbTour)
        {
            DbTour.TourId = this.ID;
            DbTour.Name = this.name;
            DbTour.Description = this.description;
            DbTour.FromCoord = this.from;
            DbTour.ToCoord = this.to;
            DbTour.TransportType = this.TransportType;
            DbTour.Distance = this.tourDistance;
            DbTour.Duration = this.estimatedTime;
            DbTour.Information = this.routeInformation;
            return DbTour;
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public Transport transportType { get; set; }
        public float tourDistance { get; set; }
        [JsonIgnore] public TimeOnly estimatedTime { get; set; }
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

            childFriendliness = difficulty + (float)estimatedTime.ToTimeSpan().TotalHours + tourDistance / 100; 
        }

        public TimeSpan getAverageTime()
        {
            TimeSpan averageTime = TimeSpan.Zero;

            for (int i = 0; i < logs.logs.Count; i++)
            {
                averageTime.Add(logs.logs[i].totalTime.ToTimeSpan());
            }

            return (logs.logs.Count == 0)? averageTime : averageTime / logs.logs.Count;
        }
        public float getAverageRating()
        {
            float averageRating = 0;

            for (int i = 0; i < logs.logs.Count; i++)
            {
                averageRating += logs.logs[i].Rating;
            }

            return (logs.logs.Count == 0) ? averageRating : averageRating / logs.logs.Count;
        }
        public float getAverageDistance()
        {
            float averageDistance = 0;

            for (int i = 0; i < logs.logs.Count; i++)
            {
                averageDistance += logs.logs[i].totalDistance;
            }

            return (logs.logs.Count == 0) ? averageDistance : averageDistance / logs.logs.Count;
        }

        public List<double> ParseCoordinates()
        {
            var coordinates = new List<double>();
            var temp = this.from.Replace(" ", "");
            var Coords = temp.Split(',');
            foreach (var coords in Coords)
            {
                var ValOut = double.Parse(coords);
                coordinates.Add(ValOut);
            }
            temp = this.to.Replace(" ", "");
            Coords = temp.Split(',');
            foreach (var coords in Coords)
            {
                var ValOut = double.Parse(coords);
                coordinates.Add(ValOut);
            }
            return coordinates;
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
            if (Search.Contains("" + estimatedTime.ToTimeSpan())) { return true; }
            if (Search.Contains("" + popularity)) { return true; }
            if (Search.Contains("" + childFriendliness)) { return true; }

            // log search values
            if (logs.includesMatch("Search")) { return true; }

            return false;
        }

        public bool generateReport(string ReportPath)
        {
            var writer = new PdfWriter(ReportPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            Paragraph p1 = new Paragraph(this.name).SetFontSize(18);

            Paragraph p2 = new Paragraph(this.description).SetFontSize(12);

            //ImageData i1 = ImageDataFactory.Create(routeInformation);

            Paragraph p3 = new Paragraph("From: " + this.from).SetFontSize(12);
            Paragraph p4 = new Paragraph("To: " + this.to).SetFontSize(12);
            Paragraph p5 = new Paragraph("Transport Type: " + this.transportType.ToString()).SetFontSize(12);
            Paragraph p6 = new Paragraph("Distance: " + this.tourDistance).SetFontSize(12);
            Paragraph p7 = new Paragraph("Estimated Time: " + this.estimatedTime).SetFontSize(12);
            Paragraph p8 = new Paragraph("Popularity: " + this.popularity).SetFontSize(12);
            Paragraph p9 = new Paragraph("Child Friendliness: " + this.childFriendliness).SetFontSize(12);

            document
                .Add(p1).Add(p2)
                //.Add(new Image(i1))
                .Add(p3).Add(p4).Add(p5).Add(p6).Add(p7).Add(p8).Add(p9);

            Table logReport = logs.getReport();

            document.Add(logReport);

            document.Close();

            return true;
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

        public Table getReport()
        {
            Table lr1 = new Table(6).SetFontSize(12).UseAllAvailableWidth()
                .AddHeaderCell("Date Created")
                .AddHeaderCell("Comment")
                .AddHeaderCell("Difficulty")
                .AddHeaderCell("Total Distance")
                .AddHeaderCell("Total Time")
                .AddHeaderCell("Rating");


            for (int i = 0; i < logs.Count; i++)
            {
                lr1.AddCell(logs[i].dateTime.ToString());
                lr1.AddCell(logs[i].Comment);
                lr1.AddCell(logs[i].difficulty + "");
                lr1.AddCell(logs[i].totalDistance + "");
                lr1.AddCell(logs[i].totalTime + "");
                lr1.AddCell(logs[i].rating + "");
            }

            return lr1;
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
        public TourLog(DataAccessDatabase.Log l)
        {
            TimeOnly totaltime = (l.TotalTime == null) ? new TimeOnly() : (TimeOnly)l.TotalTime;
            int rating = (l.Rating == null) ? 0 : (int)l.Rating;

            this.ID = l.LogId;
            this.dateTime = l.DateCreated;
            this.comment = l.Comment ?? "";
            this.difficulty = l.Difficulty;
            this.totalDistance = l.TotalDistance;
            this.totalTime = totaltime;
            this.rating = rating;
        }

        public DataAccessDatabase.Log Transform(DataAccessDatabase.Log DbLog, int TourID)
        {
            DbLog.LogId = this.ID;
            DbLog.DateCreated = this.dateTime;
            DbLog.Comment = this.comment;
            DbLog.Difficulty = (int)this.difficulty;
            DbLog.TotalDistance = this.totalDistance;
            DbLog.TotalTime = this.totalTime;
            DbLog.Rating = (int)this.Rating;
            DbLog.Tour = TourID;
            return DbLog;
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
        [JsonIgnore] public string TotalTime { get { return totalTime.ToTimeSpan().ToString(); } }
        [JsonIgnore] public float Rating { get { return rating; } }

        public bool includesMatch(string Search)
        {
            //standard search values

            // long text search values
            if (comment.ToLower().Contains(Search.ToLower())) { return true; }

            // number search values
            if (Search.Contains("" + Date)) { return true; }
            if (Search.Contains("" + totalTime.ToTimeSpan())) { return true; }
            if (Search.Contains("" + totalDistance)) { return true; }
            if (Search.Contains("" + difficulty)) { return true; }
            if (Search.Contains("" + rating)) { return true; }

            return false;
        }
    }
}
