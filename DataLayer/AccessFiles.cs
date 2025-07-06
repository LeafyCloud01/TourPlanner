
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace DataAccessFiles
{
    public static class AccessFiles
    {
        public static string directory = "/DataLayer/Files/";
        public static string root = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

        public static Dictionary<string, List<string>> fileFormats = new Dictionary<string, List<string>>
        {
            { "JSON", ["json", "JSON Files (*.json)|*.json|All Files (*.*)|*.*" ] }
        };

        public static string GetFilePath(string FileName)
        {
            return Path.Combine(root + directory, FileName);
        }
        public static string GetFilePath(string FileName, string Directory)
        {
            return Path.Combine(root + Directory, FileName);
        }
        public static void CreateFile(string FileName)
        {
            File.Create(GetFilePath(FileName));
            if(FileName == "TestFile") { File.WriteAllText(GetFilePath(FileName), "This is a Test File"); }
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

        public static T? Import<T>(string Format)
        {
            OpenFileDialog importDialog = new OpenFileDialog
            {
                Filter = fileFormats[Format][1],
                Title = "Import tour from " + fileFormats[Format][0]
            };

            if (importDialog.ShowDialog() == true)
            {
                T? imported = default; 

                switch (Format)
                {
                    case "JSON":
                        string importJSON = File.ReadAllText(importDialog.FileName);
                        imported = JsonSerializer.Deserialize<T>(importJSON);
                    break;
                }
                    
                    return imported;
            }
            return default;
        }

        public static bool Export<T>(string Format, T DataToExport)
        {
            SaveFileDialog exportDialog = new SaveFileDialog
            {
                Filter = fileFormats[Format][1],
                Title = "Export tour as " + fileFormats[Format][0], 
                OverwritePrompt = true
            };

            if (exportDialog.ShowDialog() == true)
            {
                switch (Format)
                {
                    case "JSON":
                        string filePath = exportDialog.FileName;
                        string exportJSON = JsonSerializer.Serialize<T>(DataToExport, new JsonSerializerOptions { WriteIndented = true });
                        if(filePath != "") { File.WriteAllText(filePath, exportJSON); }
                        else { return false; }
                    break;
                }

                return true;
            }
            return false;
        }

        public static string getExportPath(string title, string Extension = "")
        {
            SaveFileDialog exportDialog = new SaveFileDialog
            {
                Title = title,
                OverwritePrompt = true
            };

            if (exportDialog.ShowDialog() == true) { return (Extension != "") ? exportDialog.FileName + "." + Extension : exportDialog.FileName; }
            else { return ""; }
        }
    }
}
