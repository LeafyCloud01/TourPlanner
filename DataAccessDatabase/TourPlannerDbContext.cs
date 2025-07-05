using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDatabase
{
    public class TourPlannerDbContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=http://127.0.0.1:5433;Database=tour_planner;Username=tour_server;Password=L9vCSjLsY07EAtwC;");
        }
    }

    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From_Coord {  get; set; }
        public string To_Coord { get; set; }
        public string Transport_Type { get; set; }
        public float Distance { get; set; }
        public DateTime Duration { get; set; }
        public string Information { get; set; }
    }

    public class Log
    {
        public int Id { get; set; }
        public DateTime Date_Created { get; set; }
        public string Comment { get; set; }
        public int Difficulty { get; set; }
        public float Total_Distance { get; set; }
        public DateTime Total_Time { get; set; }
        public int Rating { get; set; }
        public int Tour_Id { get; set; }
    }
}
