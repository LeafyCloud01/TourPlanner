using Npgsql;
using System.IO;

namespace DataAccessDatabase
{
    public class DbConnection
    {
        public readonly NpgsqlDataSource? dataBase;

        public DbConnection(string ConfigPath)
        {
            string connectionInfo = ReadConnectionInfo(ConfigPath);
            try
            {
                dataBase = NpgsqlDataSource.Create(connectionInfo);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("NpgSqlException: {0}", e);
            }
        }

        private string ReadConnectionInfo(string ConfigPath)
        {
            //config file access
            foreach (string line in File.ReadLines(ConfigPath))
            {
                if (line.Contains("Database="))
                {
                    return line;
                }
            }
            return "";
        }

        public void GetToursLogs()
        {

        }

        public void AddTour()
        {

        }

        public void AddLog()
        {

        }

        public void ModifyTour()
        {

        }

        public void ModifyLog()
        {

        }

        public void DeleteTour()
        {

        }

        public void DeleteLog() 
        { 
        
        }

        ~DbConnection() 
        {
            if (dataBase != null)
            {
                dataBase.Clear();
            }
        }
    }

}
