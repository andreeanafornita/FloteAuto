using FloteAuto.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
namespace FloteAuto.Services
{
    public class TripService
    {
        private SQLiteAsyncConnection? _db;

        public async Task Init()
        {
            if (_db != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fleet.db");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Trip>();
        }

        public async Task AddTrip(Trip trip)
        {
            await Init();
            await _db.InsertAsync(trip);
        }

        public async Task<List<Trip>> GetTrips()
        {
            await Init();
            return await _db.Table<Trip>().ToListAsync();
        }

        public async Task<List<Trip>> GetTripsByVehicle(string vehicle)
        {
            await Init();
            return await _db.Table<Trip>().Where(t => t.VehicleName == vehicle).ToListAsync();
        }

        public async Task ExportToJson(string filePath)
        {
            var trips = await GetTrips();
            var json = System.Text.Json.JsonSerializer.Serialize(trips, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        public async Task ExportToXml(string filePath)
        {
            var trips = await GetTrips();
            var serializer = new XmlSerializer(typeof(List<Trip>));

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, trips);
            }
        }
    }
}
