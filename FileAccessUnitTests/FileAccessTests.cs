using DataAccessFiles;

namespace FileAccessUnitTests
{
    public class FileAccessTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateFile()
        {
            AccessFiles.CreateFile("TestFile");
            bool fileExists = File.Exists(AccessFiles.root + "Files/TestFile");

            Assert.IsTrue(fileExists);
        }

        [Test]
        public void ReadFile()
        {
            if(!File.Exists(AccessFiles.root + "Files/TestFile"))
            {
                AccessFiles.CreateFile("TestFile");
            }
            string FileContent = AccessFiles.GetFileContent("TestFile");

            Assert.IsTrue(FileContent == "This is a Test File");
        }

        [Test]
        public void UpdateFile()
        {
            if (!File.Exists(AccessFiles.root + "Files/TestFile"))
            {
                AccessFiles.CreateFile("TestFile");
            }

            AccessFiles.SetFileContent("TestFile", "This is still a Test File");
            string FileContent = AccessFiles.GetFileContent("TestFile");

            Assert.That(FileContent == "This is still a Test File");
        }

        [Test]
        public void DeleteFile()
        {
            AccessFiles.CreateFile("TestFile");
            bool existed = File.Exists(AccessFiles.root + "Files/TestFile");
            AccessFiles.DeleteFile("TestFile");
            bool fileExists = File.Exists(AccessFiles.root + "Files/TestFile");

            if (existed) { Assert.IsFalse(fileExists); }
            else { Assert.Fail(); }
        }
    }
}