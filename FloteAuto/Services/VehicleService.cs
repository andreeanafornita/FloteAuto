using FloteAuto.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Buffers.Text;
using System.Net.Http;

namespace FloteAuto.Services
{
    public class VehicleService
    {
        private SQLiteAsyncConnection? _db;

        // Inițializează conexiunea la baza de date
        public async Task Init()
        {
            if (_db != null) return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fleet.db");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Vehicle>(); // Creează tabelul pentru vehicule
        }

        // Adaugă un vehicul
        public async Task AddVehicle(Vehicle vehicle)
        {
            await Init();

            (double lat, double lon) = GenerateRandomLocation();
            vehicle.latitudine = lat;
            vehicle.longitudine = lon;

            await _db.InsertAsync(vehicle);
        }

        // Generarea unei locatii la intamplare
        private (double, double) GenerateRandomLocation()
        {
            Random random = new Random();

            // Granitele Romaniei
            double minLat = 42.6, maxLat = 47.3;
            double minLon = 20.2, maxLon = 28.8;

            double latitudine = minLat + (random.NextDouble() * (maxLat - minLat));
            double longitudine = minLon + (random.NextDouble() * (maxLon - minLon));

            return (latitudine, longitudine);
        }

        //Sterge un vehicul
        public async Task DeleteVehicle(Vehicle vehicle)
        {
            await Init();
            if (vehicle?.Id != null)
            {
                await _db.DeleteAsync(vehicle); // Șterge vehiculul pe baza ID-ului
            }
            else
            {
                throw new ArgumentNullException("Vehiculul nu are un ID valid.");
            }
        }

        //Modifica un vehicul
        public async Task UpdateVehicle(Vehicle vehicle)
        {
            await Init();
            await _db.UpdateAsync(vehicle);
        }

        // Obține toate vehiculele
        public async Task<List<Vehicle>> GetVehicles()
        {
            await Init();
            return await _db.Table<Vehicle>().ToListAsync();
        }

        // Obține vehiculele după un anumit nume
        public async Task<List<Vehicle>> GetVehiclesByName(string name)
        {
            await Init();
            return await _db.Table<Vehicle>().Where(v => v.Marca == name).ToListAsync();
        }

        // Exportă vehiculele într-un fișier JSON
        public async Task ExportToJson(string filePath)
        {
            var vehicles = await GetVehicles();
            var json = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Exportă vehiculele într-un fișier XML
        public async Task ExportToXml(string filePath)
        {
            var vehicles = await GetVehicles();
            var serializer = new XmlSerializer(typeof(List<Vehicle>));

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, vehicles);
            }
        }
    }
}

