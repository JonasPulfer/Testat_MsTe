using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        public ReservationAvailabilityTest()
        {
            // Prepare reservation
            Reservation reservation = Target.GetById(1);
            reservation.Von = DateTime.Today;
            reservation.Bis = DateTime.Today.AddDays(1);
            Target.Update(reservation);
        }

        [Fact]
        public void ScenarioOkay01Test()
        {
            Reservation res = Target.GetById(1);
            res.Bis = new DateTime(2020, 12, 30);
            Target.Update(res);
            Assert.Equal("30.12.2020 00:00:00", res.Bis.ToString());

        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            Reservation res = Target.GetById(4);
            res.Bis = new DateTime(2020, 06, 20);
            Target.Update(res);
            Assert.Equal("20.06.2020 00:00:00", res.Bis.ToString());
        }

        [Fact]
        public void ScenarioOkay03Test()
        {
            Reservation res = new Reservation();
            res.AutoId = 2;
            res.KundeId = 2;
            res.Von = new DateTime(2020, 01, 30);
            res.Bis = new DateTime(2020, 02, 15);
            Target.Insert(res);
            Assert.Equal("15.02.2020 00:00:00", res.Bis.ToString());
        }

        [Fact]
        public void ScenarioOkay04Test()
        {
            Reservation res = new Reservation();
            res.AutoId = 2;
            res.KundeId = 3;
            res.Von = new DateTime(2020, 02, 15);
            res.Bis = new DateTime(2020, 02, 17);
            Target.Insert(res);
            Assert.Equal("17.02.2020 00:00:00", res.Bis.ToString());
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            Reservation res = new Reservation();
            res.AutoId = 2;
            res.KundeId = 2;
            res.Von = new DateTime(2020, 01, 05);
            res.Bis = new DateTime(2020, 01, 15);
            Assert.Throws<AutoUnavailableException>(
                () => Target.Insert(res));
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            Reservation res = Target.GetById(2);
            res.Bis = new DateTime(2020, 05, 20);
            Assert.Throws<AutoUnavailableException>(
                () => Target.Update(res));
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            Reservation res = Target.GetById(4);
            res.Von = new DateTime(2020, 01, 12);
            res.Bis = new DateTime(2020, 01, 18);
            Assert.Throws<AutoUnavailableException>(
                () => Target.Update(res));
        }

        [Fact]
        public void ScenarioNotOkay04Test()
        {
            Reservation res = Target.GetById(4);
            res.Von = new DateTime(2020, 01, 05);
            res.Bis = new DateTime(2020, 01, 30);
            Assert.Throws<AutoUnavailableException>(
                () => Target.Update(res));
        }

        [Fact]
        public void ScenarioNotOkay05Test()
        {
            Reservation res = new Reservation();
            res.AutoId = 3;
            res.KundeId = 2;
            res.Von = new DateTime(2020, 01, 15);
            res.Bis = new DateTime(2020, 01, 30);
            Assert.Throws<AutoUnavailableException>(
                () => Target.Insert(res));
        }
    }
}
