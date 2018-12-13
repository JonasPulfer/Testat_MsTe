using AutoReservation.Dal.Entities;
using System;

namespace AutoReservation.Common.DataTransferObjects
{
    public class ReservationDto
    {
        public int ReservationsNr { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public Auto Auto { get; set; }
        public Kunde Kunde { get; set; }
        public byte[] RowVersion { get; set; }

        public override string ToString()
            => $"{ReservationsNr}; {Von}; {Bis}; {Auto}; {Kunde}";
    }
}
