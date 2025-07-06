using BusinessLayer;

namespace BusinessUnitTests
{
    public class TourTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public TourList tours = new TourList();
        Tour tour1 = new(0, "tour 1", "description", "", "", new(), 147, new(3, 0), "", new());
        Tour tour1_new = new(0, "tour 1", "description", "", "", new(), 0, new(), "", new());
        Tour tour2 = new(1, "tour 2", "baa baa baa", "", "", new(), 0, new(), "", new());
        Tour tour3 = new(2, "tour 3", "a fun description", "", "", new(), 0, new(), "", new());

        public LogList logs = new LogList();
        TourLog log1 = new(0, new(), "", 1, 86f, new(1, 52, 0), 2);
        TourLog log2 = new(1, new(), "", 3, 14f, new(0, 33, 0), 3);
        TourLog log3 = new(2, new(), "", 2, 104f, new(2, 15, 0), 2);

        [Test]
        public void AddNewTour()
        {
            if(tours.tours.Count > 0) { tours.tours.Clear(); }
            tours.ChangeTour(tour1);

            Assert.That(tours.tours.Count == 1 && tours.tours[0] == tour1);
        }
        [Test]
        public void EditTour()
        {
            tours.tours = [tour1];
            tours.ChangeTour(tour1_new);

            Assert.That(tours.tours.Count == 1 && tours.tours[0] == tour1_new);
        }
        [Test]
        public void DeleteTour()
        {
            tours.tours = [tour1]; 
            tours.DeleteTour(tour1.ID);

            Assert.That(tours.tours.Count == 0);
        }
        [Test]
        public void UpdatePopularity()
        {
            tours.tours = [tour1];
            tour1.logs.logs = [log1];

            int popularity_1 = tour1.popularity;
            tours.ChangeTourLog(tour1.ID, log2);
            int popularity_2 = tour1.popularity;

            tours.tours = [];
            tour1.logs = new();

            Assert.That(popularity_2 == (popularity_1 + 1) && popularity_2 == 2);
        }
        [Test]
        public void UpdateChildFriendliness()
        {
            tour1.logs.logs = [log1, log2, log3];
            tour1.UpdateChildFriendliness();
            float childFriendliness = tour1.childFriendliness;

            Assert.That(childFriendliness == 1.56);
        }
        public void GetAverageTime()
        {
            tour1.logs.logs = [log1, log2, log3];
            TimeSpan avgTime = tour1.getAverageTime();

            Assert.That(avgTime == new TimeSpan(3, 1, 1));
        }
        public void GetAverageDistance()
        {
            tour1.logs.logs = [log1, log2, log3];
            float avgDistance = tour1.getAverageDistance();

            Assert.That(avgDistance == 68);
        }
        public void GetAverageRating()
        {
            tour1.logs.logs = [log1, log2, log3];
            float avgRating = tour1.getAverageRating();

            Assert.That(avgRating == 7/3);
        }
        [Test]
        public void GetTour()
        {
            tours.tours = [tour1, tour2, tour3];
            Tour retrievedTour = tours.getTour(tour2.ID);

            Assert.That(retrievedTour == tour2);
        }
        [Test]
        public void PartialSearch()
        {
            bool includesSearchText = tour2.includesMatch("baa baa");

            Assert.That(includesSearchText);
        }
        [Test]
        public void FullSearch()
        {
            tours.tours = [tour1, tour2, tour3];
            tours.getTours("description");

            Assert.That(!tours.tours.Contains(tour2));
        }
    }
}