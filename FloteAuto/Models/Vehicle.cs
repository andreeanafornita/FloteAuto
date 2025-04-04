using SQLite;
using System;

namespace FloteAuto.Models
{
    public class Vehicle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NumarInmatriculare { get; set; }
        public string Marca { get; set; }
        public string Model { get; set; }
        public DateTime DataExpirareITP { get; set; }
        public DateTime DataExpirareActe { get; set; }
        public string ImagePath { get; set; }

        public double latitudine { get; set; }
        public double longitudine { get; set; }
    }
}
