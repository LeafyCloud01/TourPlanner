//using BusinessLayer;
//using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;

namespace DataAccessDatabase
{
    public class AccessDatabase
    {
        //public readonly NpgsqlDataSource? dataBase;

        public AccessDatabase ()
        {

        }
        /*public AccessDatabase(string ConfigPath)
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
        }*/

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

        public static List<Tour> GetTours()
        {
            List<Tour> Tours;
            using (var context = new TourPlannerContext())
            {
                Tours = context.Tours.ToList();
            }
            return Tours;
        }

        public static List<Log> GetLogs() 
        {
            List<Log> Logs;
            using (var context = new TourPlannerContext())
            {
                Logs = context.Logs.ToList();
            }
            return Logs;
        }

        /*public void AddTour(Tour Tour)
        {
            if (dataBase != null)
            {
                using var Connection = dataBase.OpenConnection();
                using var Insert = dataBase.CreateCommand("INSERT INTO tours (name, description, from_coord, to_coord, transport_type, distance, duration, information) VALUES ($1,$2,$3,$4,$5,$6,$7,$8);");
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.name);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.description);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.from);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.to);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Enum.GetName(typeof(Transport), Tour.transportType)!);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Tour.tourDistance);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Time, Tour.estimatedTime);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.routeInformation);
                Insert.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void AddLog(int TourId, TourLog Log)
        {
            if (dataBase != null)
            {
                using var Connection = dataBase.OpenConnection();
                using var Insert = dataBase.CreateCommand("INSERT INTO logs (date_created, comment, difficulty, total_distance, total_time, rating, tour_id) VALUES ($1,$2,$3,$4,$5,$6,$7);");
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Timestamp, Log.dateTime);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Log.comment);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, Log.difficulty);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Log.totalDistance);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Time, Log.totalTime);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Log.rating);
                Insert.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, TourId);
                Insert.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void ModifyTour(Tour Tour)
        {
            if (dataBase != null) 
            {
                using var Connection = dataBase.OpenConnection();
                using var Update = dataBase.CreateCommand("UPDATE tours SET name = $1, description = $2, from_coord = $3, to_coord = $4, transport_type = $5, distance = $6, duration = $7, information = $8 WHERE tour_id = $9;");
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.name);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.description);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.from);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.to);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Enum.GetName(typeof(Transport), Tour.transportType)!);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Tour.tourDistance);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Time, Tour.estimatedTime);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Tour.routeInformation);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, Tour.ID);
                Update.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void ModifyLog(TourLog Log)
        {
            if (dataBase != null)
            {
                using var Connection = dataBase.OpenConnection();
                using var Update = dataBase.CreateCommand("UPDATE logs SET date_created = $1, comment = $2, difficulty = $3, total_distance = $4, total_time = $5, rating = $6 WHERE log_id = $7;");
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Timestamp, Log.dateTime);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, Log.comment);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, Log.difficulty);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Log.totalDistance);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Time, Log.totalTime);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Real, Log.rating);
                Update.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer,Log.ID);
                Update.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void DeleteTour(int TourId)
        {
            if (dataBase != null)
            {
                using var Connection = dataBase.OpenConnection();
                using var DeleteLogs = dataBase.CreateCommand("DELETE FROM logs WHERE tour_id = $1;");
                DeleteLogs.Parameters.Add(TourId);
                using var DeleteTour = dataBase.CreateCommand("DELETE FROM tours WHERE tour_id = $1;");
                DeleteTour.Parameters.Add(TourId);
                DeleteLogs.ExecuteNonQuery();
                DeleteTour.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void DeleteLog(int LogId) 
        {
            if (dataBase != null)
            {
                using var Connection = dataBase.OpenConnection();
                using var Delete = dataBase.CreateCommand("DELETE FROM logs WHERE log_id = $1;");
                Delete.Parameters.Add(LogId);
                Delete.ExecuteNonQuery();
                Connection.Close();
            }
        }

        ~AccessDatabase() 
        {
            if (dataBase != null)
            {
                dataBase.Clear();
            }
        }*/
    }
    
}
