using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace TrainApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Train> Trains { get; set; }
    }

    public class Train
    {
        public int TrainId { get; set; }
        public string? TrainNumber { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TicketPrice { get; set; }
    }

    class Program
    {
        static void Main()
        {

            var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
               
                AddTrain(context);

                GetTrains(context);

                UpdateTrain(context);

                DeleteTrain(context);
            }
        }

        static void AddTrain(AppDbContext context)
        {
            var train = new Train
            {
                TrainNumber = "586",
                Destination = "Odesa",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(2),
                NumberOfSeats = 96,
                TicketPrice = 496.00m
            };

            var train_2 = new Train
            {
                TrainNumber = "179",
                Destination = "Lviv",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(9),
                NumberOfSeats = 54,
                TicketPrice = 980.00m
            };

            context.Trains.Add(train);
            context.SaveChanges();
            Console.WriteLine("Train added.");
        }

        static void GetTrains(AppDbContext context)
        {
            var trains = context.Trains.ToList();
            foreach (var train in trains)
            {
                Console.WriteLine($"ID: {train.TrainId}, Number: {train.TrainNumber}, Destination: {train.Destination}");
            }
        }

        static void UpdateTrain(AppDbContext context)
        {
            var train = context.Trains.FirstOrDefault();
            if (train != null)
            {
                train.Destination = "Kherson";
                context.Trains.Update(train);
                context.SaveChanges();
                Console.WriteLine("Train updated.");
            }
        }

        static void DeleteTrain(AppDbContext context)
        {
            var train = context.Trains.FirstOrDefault();
            if (train != null)
            {
                context.Trains.Remove(train);
                context.SaveChanges();
                Console.WriteLine("Train deleted.");
            }
        }
    }
}
