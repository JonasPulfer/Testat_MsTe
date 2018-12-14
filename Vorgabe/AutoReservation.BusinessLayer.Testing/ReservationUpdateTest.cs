using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        [Fact]
        public void UpdateReservationTest()
        {
            Reservation result = Target.GetById(4);
            result.Bis = new DateTime(2020, 06, 20);
            Target.Update(result);

            Assert.Equal("20.06.2020 00:00:00", result.Bis.ToString());

        }
    }
}
