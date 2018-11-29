namespace AutoReservation.Dal.Entities
{
    public class Auto
    {
        public int Id { get; set; }

        public string Marke { get; set; }

        public int Tagestarif { get; set; }

        public byte[] RowVersion { get; set; } //Nullable hinzuf�gen

        public int AutoKlasse { get; set; }

        public int? Basistarif { get; set; } //existiert nur f�r Luxusklassen

        public ICollection<Reservation> Reservation { get; set; }
    }
}
