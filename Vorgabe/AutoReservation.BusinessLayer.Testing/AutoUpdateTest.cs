using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());

        [Fact]
        public void UpdateAutoTest()
        {
            
            Auto result = Target.GetById(1);
            result.Marke = "Ferrari";
            Target.Update(result);

            Assert.Equal("Ferrari", result.Marke);
        }
    }
}
