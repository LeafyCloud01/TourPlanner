using Npgsql;

namespace DataAccessDatabase
{
    public class DbConnection
    {
        private string connectionInfo = "Host=http://127.0.0.1:5433;Database=tour_planner;Username=tour_server;Password=L9vCSjLsY07EAtwC;";
        public readonly NpgsqlDataSource? dataBase;

        public DbConnection(/*string connectionInfo*/)
        {
            try
            {
                dataBase = NpgsqlDataSource.Create(connectionInfo);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("NpgSqlException: {0}", e);
            }
        }
    }

}
