using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {
        // Example
        public List<Auto> List

        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Autos.ToList();
                }
            }
        }

        public Auto GetById(int key)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Auto auto = context
                     .Autos
                     .SingleOrDefault(a => a.Id == key);
                return auto;
            }
        }

        public void Insert(Auto autoToBeInserted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(autoToBeInserted).State = EntityState.Added;

                context.SaveChanges();

                int id = autoToBeInserted.Id;
            }
        }

        public void Update(Auto AutoToBeUpdated)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {

                context.Entry(AutoToBeUpdated).State = EntityState.Modified;

                context.SaveChanges();

            }
        }

        public void Delete(Auto AutoToBeDeleted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(AutoToBeDeleted).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}