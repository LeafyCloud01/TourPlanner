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
        Tour tour1 = new(0, "tour 1", "description", "", "", new(), 0, new(), "", new());
        Tour tour1_new = new(0, "tour 1", "description", "", "", new(), 0, new(), "", new());
        Tour tour2 = new(1, "tour 2", "baa baa baa", "", "", new(), 0, new(), "", new());
        Tour tour3 = new(2, "tour 3", "a fun description", "", "", new(), 0, new(), "", new());

        public LogList logs = new LogList();
        TourLog log1 = new(0, new(), "", 1, 0, new(), 0);
        TourLog log2 = new(1, new(), "", 3, 0, new(), 0);
        TourLog log3 = new(2, new(), "", 2, 0, new(), 0);

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
            Assert.Fail();
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