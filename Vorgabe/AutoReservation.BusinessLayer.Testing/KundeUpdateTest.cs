using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());

        [Fact]
        public void UpdateKundeTest()
        {
            Kunde result = Target.GetById(1);
            result.Nachname = "Meier";
            Target.Update(result);

            Assert.Equal("Meier", result.Nachname);
        }
    }
}
