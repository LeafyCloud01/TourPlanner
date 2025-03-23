
using System.IO;

namespace DataLayer
{
    public static class FileAccess
    {
        public static string directory = "Files/";
        public static string root = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

        public static string GetFilePath(string FileName)
        {
            return Path.Combine(root + directory, FileName);
        }
        public static void CreateFile(string FileName)
        {
            File.Create(GetFilePath(FileName));
        }
        public static void DeleteFile(string FileName)
        {
            File.Delete(GetFilePath(FileName));
        }
        public static string GetFileContent(string FileName)
        {
            string FileContent = (File.Exists(GetFilePath(FileName))) ? File.ReadAllText(GetFilePath(FileName)) : "";

            return FileContent;
        }
        public static void SetFileContent(string FileName, string Content) 
        {
            File.WriteAllText(GetFilePath(FileName), Content);
        }
    }

}
