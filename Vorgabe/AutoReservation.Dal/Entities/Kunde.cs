using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public int Id { get; set; }

        public string Nachname { get; set; } // Grösse beachten --> Siehe Vorlesung

        public string Vorname { get; set; }

        public DateTime Geburtsdatum { get; set; } // Anastatt datetime?

        public byte[] RowVersion { get; set; } //Nullable

        public ICollection<Reservation> Reservationen { get; set; }
    }
}
