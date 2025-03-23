using BusinessLayer;

namespace BusinessUnitTests
{
    public class LogTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public LogList logs = new LogList();
        TourLog log1 = new(new(), "", 1, 0, new(), 0);
        TourLog log2 = new(new(), "", 3, 0, new(), 0);
        TourLog log3 = new(new(), "", 2, 0, new(), 0);

        [Test]
        public void AddNewLog()
        {
            
            Assert.Pass();
        }
        [Test]
        public void EditLog()
        {

            Assert.Pass();
        }
        [Test]
        public void DeleteLog()
        {

            Assert.Pass();
        }
        [Test]
        public void CalculateDifficulty()
        {
            logs.logs.Add(log1);
            logs.logs.Add(log2);
            logs.logs.Add(log3);

            float difficulty = logs.CalculateDifficulty();

            Assert.That(difficulty == 6);
        }
        [Test]
        public void CalculateAverageTimes()
        {

            Assert.Pass();
        }
    }
}