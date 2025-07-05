using BusinessLayer;
using System.Linq;

namespace BusinessUnitTests
{
    public class LogTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public TourList tours = new TourList();
        Tour tour1 = new(0, "tour 1", "description", "", "", new(), 0, new(), "", new());
        Tour tour2 = new(1, "tour 2", "baa baa baa", "", "", new(), 0, new(), "", new());
        Tour tour3 = new(2, "tour 3", "a fun description", "", "", new(), 0, new(), "", new());

        public LogList logs = new LogList();
        TourLog log1 = new(0, new(), "A fun tour", 1, 0, new(), 0);
        TourLog log1_new = new(0, new(), "A fun but difficult", 4, 0, new(), 0);
        TourLog log2 = new(1, new(), "An ok tour", 3, 0, new(), 0);
        TourLog log3 = new(2, new(), "An exciting tour", 2, 0, new(), 0);

        [Test]
        public void AddNewLog_ToLogList()
        {
            if (logs.logs.Count > 0) { logs.logs.Clear(); }
            logs.ChangeLog(log1);

            Assert.That(logs.logs.Count == 1 && logs.logs[0] == log1);
        }
        [Test]
        public void AddNewLog_ThroughTourList()
        {
            if (logs.logs.Count > 0) { logs.logs.Clear(); }
            tour1.logs = logs;
            tours.tours = [tour1];
            tours.ChangeTourLog(tour1.ID, log1);

            Assert.That(tours.tours[0].logs.logs.Count == 1 && tours.tours[0].logs.logs[0] == log1);
        }
        [Test]
        public void AddNewLog_AddedToCorrectTour()
        {
            if (logs.logs.Count > 0) { logs.logs.Clear(); }
            tour1.logs = logs;
            tour2.logs = logs;
            tour3.logs = logs;
            tours.tours = [tour1, tour2, tour3];
            tours.ChangeTourLog(tour2.ID, log1);

            Assert.That(tours.tours[1].logs.logs.Count == 1 && tours.tours[0].logs.logs[0] == log1);
        }
        [Test]
        public void EditLog()
        {
            logs.logs = [log1];
            logs.ChangeLog(log1_new);

            Assert.That(logs.logs.Count == 1 && logs.logs[0] == log1_new);
        }
        [Test]
        public void EditLog_ThroughTourList()
        {
            tour1.logs.logs = [log1];
            tours.tours = [tour1];
            tours.ChangeTourLog(tour1.ID, log1_new);

            Assert.That(tours.tours[0].logs.logs.Count == 1 && tours.tours[0].logs.logs[0] == log1_new);
        }
        [Test]
        public void EditLog_EditingCorrectTour()
        {
            tour1.logs.logs = [log2];
            tour2.logs.logs = [log1];
            tour3.logs.logs = [log3];
            tours.tours = [tour1, tour2, tour3];
            tours.ChangeTourLog(tour2.ID, log1_new);

            bool tour1_correct = tours.tours[0].logs.logs.Count == 1 && tours.tours[0].logs.logs[0] == log2;
            bool tour2_correct = tours.tours[1].logs.logs.Count == 1 && tours.tours[1].logs.logs[0] == log1_new;
            bool tour3_correct = tours.tours[2].logs.logs.Count == 1 && tours.tours[2].logs.logs[0] == log3;

            Assert.That(tour1_correct && tour2_correct && tour3_correct);
        }
        [Test]
        public void DeleteLog()
        {
            logs.logs = [log1];
            logs.DeleteLog(log1.ID);

            Assert.That(logs.logs.Count == 0);
        }
        [Test]
        public void DeleteLog_ThroughTourList()
        {
            tour1.logs.logs = [log1];
            tours.tours = [tour1];
            tours.DeleteTourLog(tour1.ID, log1.ID);

            Assert.That(tours.tours[0].logs.logs.Count == 0);
        }
        [Test]
        public void CalculateDifficulty()
        {
            logs.logs = [log1, log2, log3];

            float difficulty = logs.CalculateDifficulty();

            Assert.That(difficulty == 2);
        }
        [Test]
        public void CalculateAverageTimes()
        {

            Assert.Fail();
        }
        [Test]
        public void PartialSearch()
        {
            bool includesSearchText = log1_new.includesMatch("but");

            Assert.That(includesSearchText);
        }
        [Test]
        public void FullSearch()
        {
            logs.logs = [log1, log1_new];
            bool includesSearchText = logs.includesMatch("An");

            Assert.That(!includesSearchText);
        }
    }
}