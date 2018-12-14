using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public List<Reservation> List

        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Reservationen.ToList();
                }
            }
        }

        public Reservation GetById(int key)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Reservation reservation = context
                     .Reservationen
                     .Single(r => r.ReservationsNr == key);
                return reservation;
            }
        }

        public void Insert(Reservation reservationToBeInserted)
        {
            if (!CheckDate(reservationToBeInserted))
            {
                throw new InvalidDateRangeException("Invalid Date Range!");
            }

            if (!CheckAutoAvailability(reservationToBeInserted))
            {
                throw new AutoUnavailableException("Auto is not available");
            }

            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(reservationToBeInserted).State = EntityState.Added;

                context.SaveChanges();

                int id = reservationToBeInserted.ReservationsNr;
            }
        }

        public void Update(Reservation reservationToBeUpdated)
        {
            if(!CheckDate(reservationToBeUpdated))
            {
                throw new InvalidDateRangeException("Invalid Date Range!");
            }

            if(!CheckAutoAvailability(reservationToBeUpdated))
            {
                throw new AutoUnavailableException("Auto is unvailable!");
            }

            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(reservationToBeUpdated).State = EntityState.Modified;

                context.SaveChanges();

            }
        }

        public void Delete(Reservation ReservationToBeDeleted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(ReservationToBeDeleted).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }

        public bool CheckDate(Reservation reservation)
        {
            if (reservation.Bis < reservation.Von || reservation.Von == reservation.Bis)
            {
               return false;
            }

            return true;
        }

        public bool CheckAutoAvailability(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {

                List<Reservation> reservations = context
                     .Reservationen
                     .Where(r => r.AutoId == reservation.AutoId)
                     .ToList();
                foreach(Reservation res in reservations)
                {
                    if (res.ReservationsNr != reservation.ReservationsNr)
                    {
                        if ((reservation.Von < res.Bis && reservation.Bis > res.Bis) || 
                            (reservation.Bis > res.Von && reservation.Von < res.Bis))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
