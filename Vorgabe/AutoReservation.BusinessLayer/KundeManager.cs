using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public List<Kunde> List

        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Kunden.ToList();
                }
            }
        }

        public Kunde GetById(int key)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Kunde kunde = context
                     .Kunden
                     .Single(k => k.Id == key);
                return kunde;
            }
        }

        public void Insert(Kunde kundeToBeInserted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(kundeToBeInserted).State = EntityState.Added;

                context.SaveChanges();

                int id = kundeToBeInserted.Id;
            }
        }

        public void Update(Kunde kundeToBeUpdated)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {

                context.Entry(kundeToBeUpdated).State = EntityState.Modified;

                context.SaveChanges();

            }
        }
        public void Delete(Kunde KundeToBeDeleted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(KundeToBeDeleted).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}
