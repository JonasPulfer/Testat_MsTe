namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public int ReservationsNr { get; set; }

        public int AutoId { get; set; }

        public Auto Auto { get; set; }

        public int KundeID { get; set; }

        public Kunde Kunde { get; set; }

        public DateTime Von { get; set; }

        public DateTime Bis { get; set; }

        public byte[] RowVersion { get; set; }
    }

}
