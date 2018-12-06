﻿using AutoReservation.Dal;
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
                     .Single(a => a.Id == key);
                return auto;
            }
        }

        public void insert(Auto autoToBeInserted)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(autoToBeInserted).State = EntityState.Added;

                context.SaveChanges();

                int id = autoToBeInserted.Id;
            }
        }

        public void Update(Auto AutoToBeUpdated, Auto AutoUpdated)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                AutoToBeUpdated = AutoUpdated;

                context.Entry(AutoToBeUpdated).State = EntityState.Modified;

                context.SaveChanges();

            }
        }
    }
}