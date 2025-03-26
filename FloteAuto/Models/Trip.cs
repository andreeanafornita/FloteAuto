using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloteAuto.Models
{
    public class Trip
    {
        public int Id { get; set; }  // va fi generat automat
        public string? VehicleName { get; set; } // ex: Dacia Logan
        public DateTime Date { get; set; }      // data cursei
        public double KmStart { get; set; }     // kilometraj la plecare
        public double KmEnd { get; set; }       // kilometraj la întoarcere
        public string Purpose { get; set; }     // scopul cursei

        // Proprietați calculate automat:
        public double Distance => KmEnd - KmStart; // cât ai mers
        public double EstimatedFuel => (Distance / 100) * 7.0;
    }
}
