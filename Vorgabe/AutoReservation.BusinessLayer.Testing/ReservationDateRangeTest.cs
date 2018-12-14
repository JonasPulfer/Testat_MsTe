using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        [Fact]
        public void ScenarioOkay01Test()
        {
            Reservation res = Target.GetById(1);
            DateTime date = new DateTime(2018, 12, 13);
            res.Von = date;
            Target.Update(res);
            Assert.Equal(Target.GetById(1).Von, date);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            Reservation res = Target.GetById(2);
            DateTime date = new DateTime(2020, 01, 11);
            res.Bis = date;
            Target.Update(res);
            Assert.Equal(Target.GetById(2).Bis, date);
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            throw new NotImplementedException("Test not implemented.");
        }
    }
}
