using Npgsql;
using System.IO;

namespace DataAccessDatabase
{
    public class DbConnection
    {
        public readonly NpgsqlDataSource? dataBase;

        public DbConnection()
        {
            string connectionInfo = ReadInfo();
            try
            {
                dataBase = NpgsqlDataSource.Create(connectionInfo);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("NpgSqlException: {0}", e);
            }
        }

        private string ReadInfo()
        {
            //config file access
            string ReturnString = File.ReadAllText(@"DatabaseConfig.txt");
            return ReturnString;
        }

        ~DbConnection() 
        {
            dataBase!.Clear();
        }
    }

}
