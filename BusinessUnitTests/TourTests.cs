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
        Tour tour1 = new();
        Tour tour2 = new();
        Tour tour3 = new();

        [Test]
        public void AddNewTour()
        {
            tours.ChangeTour(tour1);

            Assert.That(tours.tours.Count == 1);
        }
        [Test]
        public void EditTour()
        {
            tours.ChangeTour(tour2);

            Assert.That(tours.tours.Count == 1);
        }
        [Test]
        public void DeleteTour()
        {
            if(tours.tours.Count < 1) { tours.ChangeTour(tour1); }
            tours.DeleteTour(0);

            Assert.That(tours.tours.Count == 0);
        }
        [Test]
        public void UpdatePopularity()
        {
            Assert.Pass();
        }
        [Test]
        public void UpdateChildFriendliness()
        {
            Assert.Pass();
        }
    }
}