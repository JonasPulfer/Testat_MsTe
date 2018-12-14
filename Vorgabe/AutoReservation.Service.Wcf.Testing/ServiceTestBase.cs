using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.Service.Wcf.Testing
{
    public abstract class ServiceTestBase
        : TestBase
    {
        protected abstract IAutoReservationService Target { get; }

        #region Read all entities

        [Fact]
        public void GetAutosTest()
        {
            ICollection<AutoDto> autos = Target.GetAutoList();
            Assert.Equal(4, autos.Count);
        }

        [Fact]
        public void GetKundenTest()
        {
            ICollection<KundeDto> kunden = Target.GetKundeList();
            Assert.Equal(4, kunden.Count);
        }

        [Fact]
        public void GetReservationenTest()
        {
            ICollection<ReservationDto> reservationen = Target.GetReservationList();
            Assert.Equal(4, reservationen.Count);
        }

        #endregion

        #region Get by existing ID

        [Fact]
        public void GetAutoByIdTest()
        {
            AutoDto fiat = Target.GetAutoById(1);
            Assert.Equal("Fiat Punto", fiat.Marke);
            Assert.Equal(50, fiat.Tagestarif);
            Assert.Equal(AutoKlasse.Standard , fiat.AutoKlasse);
        }

        [Fact]
        public void GetKundeByIdTest()
        {
            KundeDto annaNass = Target.GetKundeById(1);
            Assert.Equal("Nass", annaNass.Nachname);
            Assert.Equal("Anna", annaNass.Vorname);
            Assert.Equal(new DateTime(1981, 05, 05), annaNass.Geburtsdatum);
        }

        [Fact]
        public void GetReservationByNrTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            Assert.Equal(1, reservation.Auto.Id);
            Assert.Equal(1, reservation.Kunde.Id);
            Assert.Equal(new DateTime(2020, 01, 10), reservation.Von);
            Assert.Equal(new DateTime(2020, 01, 20), reservation.Bis);
        }

        #endregion

        #region Get by not existing ID

        [Fact]
        public void GetAutoByIdWithIllegalIdTest()
        {
            AutoDto auto = Target.GetAutoById(10);
            Assert.Null(auto);
        }

        [Fact]
        public void GetKundeByIdWithIllegalIdTest()
        {
            KundeDto kunde = Target.GetKundeById(10);
            Assert.Null(kunde);
        }

        [Fact]
        public void GetReservationByNrWithIllegalIdTest()
        {
            ReservationDto reservation = Target.GetReservationById(10);
            Assert.Null(reservation);
        }

        #endregion

        #region Insert

        [Fact]
        public void InsertAutoTest()
        {
            AutoDto auto = new AutoDto
            {
                AutoKlasse = AutoKlasse.Luxusklasse,
                Basistarif = 1000,
                Marke = "Tesla",
                Tagestarif = 1500
            };

            Target.InsertAuto(auto);
        }

        [Fact]
        public void InsertKundeTest()
        {
            KundeDto kunde = new KundeDto
            {
                Geburtsdatum = new DateTime(1997, 05, 21),
                Nachname = "Muster",
                Vorname = "Hans"
            };

            Target.InsertKunde(kunde);
        }

        [Fact]
        public void InsertReservationTest()
        {
            ReservationDto reservation = new ReservationDto
            {
                Auto = Target.GetAutoById(1),
                Bis = new DateTime(2018, 12, 24),
                Kunde = Target.GetKundeById(2),
                Von = new DateTime(2018, 12, 14)
            };

            Target.InsertReservation(reservation);
        }

        #endregion

        #region Delete  

        [Fact]
        public void DeleteAutoTest()
        {
            Target.DeleteAuto(Target.GetAutoById(1));
            Assert.Null(Target.GetAutoById(1));
        }

        [Fact]
        public void DeleteKundeTest()
        {
            Target.DeleteKunde(Target.GetKundeById(1));
            Assert.Null(Target.GetKundeById(1));
        }

        [Fact]
        public void DeleteReservationTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            Target.DeleteReservation(reservation);
            reservation = Target.GetReservationById(1);
            Assert.Null(reservation);
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateAutoTest()
        {
            AutoDto auto = Target.GetAutoById(1);
            auto.Marke = "TestMarke";
            Target.UpdateAuto(auto);

            auto = Target.GetAutoById(1);
            Assert.Equal("TestMarke", auto.Marke);
        }

        [Fact]
        public void UpdateKundeTest()
        {
            KundeDto kunde = Target.GetKundeById(1);
            kunde.Vorname = "TestVorname";
            Target.UpdateKunde(kunde);

            kunde = Target.GetKundeById(1);
            Assert.Equal("TestVorname", kunde.Vorname); 
        }

        [Fact]
        public void UpdateReservationTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            DateTime date = new DateTime(1999, 09, 09);
            reservation.Von = date;
            Target.UpdateReservation(reservation);

            reservation = Target.GetReservationById(1);
            Assert.Equal(date, reservation.Von);
        }

        #endregion

        #region Update with optimistic concurrency violation

        [Fact]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            AutoDto auto1 = Target.GetAutoById(1);
            AutoDto auto2 = Target.GetAutoById(1);

            auto1.Marke = "TestMarke";
            auto2.Marke = "ErrorMarke";

            Target.UpdateAuto(auto1);

            Assert.Throws<FaultException<OptimisticConcurrencyFault>>(() => Target.UpdateAuto(auto2));
        }

        [Fact]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            KundeDto kunde1 = Target.GetKundeById(1);
            KundeDto kunde2 = Target.GetKundeById(1);

            kunde1.Vorname = "TestVorname";
            kunde2.Vorname = "ErrorVorname";

            Target.UpdateKunde(kunde1);

            Assert.Throws<FaultException<OptimisticConcurrencyFault>>(() => Target.UpdateKunde(kunde2));
        }

        [Fact]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            ReservationDto res1 = Target.GetReservationById(1);
            ReservationDto res2 = Target.GetReservationById(1);

            res1.Kunde = Target.GetKundeById(2);
            res2.Kunde = Target.GetKundeById(3);

            Target.UpdateReservation(res1);

            Assert.Throws<FaultException<OptimisticConcurrencyFault>>(() => Target.UpdateReservation(res2));

        }

        #endregion

        #region Insert / update invalid time range

        [Fact]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            ReservationDto reservation = new ReservationDto
            {
                Auto = Target.GetAutoById(1),
                Bis = new DateTime(2018, 12, 14),
                Kunde = Target.GetKundeById(2),
                Von = new DateTime(2018, 12, 24)
            };

            Assert.Throws<FaultException<InvalidDateRangeFault>>(() => Target.InsertReservation(reservation));
        }

        [Fact]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            ReservationDto reservation = new ReservationDto
            {
                Auto = Target.GetAutoById(2),
                Kunde = Target.GetKundeById(2),
                Von = new DateTime(2020, 01, 05),
                Bis = new DateTime(2020, 01, 15)
            };

            Assert.Throws <FaultException<AutoUnavailableFault>>(() => Target.InsertReservation(reservation));
        }

        [Fact]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            reservation.Von = new DateTime(2018, 12, 24);
            reservation.Bis = new DateTime(2018, 12, 14);

            Assert.Throws<FaultException<InvalidDateRangeFault>>(() => Target.UpdateReservation(reservation));
        }

        [Fact]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            reservation.Auto = Target.GetAutoById(2);
            reservation.Von = new DateTime(2020, 01, 05);
            reservation.Bis = new DateTime(2020, 01, 15);

            Assert.Throws<FaultException<AutoUnavailableFault>>(() => Target.UpdateReservation(reservation));
        }

        #endregion

        #region Check Availability

        [Fact]
        public void CheckAvailabilityIsTrueTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            reservation.Auto = Target.GetAutoById(2);
            reservation.Von = new DateTime(2021, 01, 05);
            reservation.Bis = new DateTime(2021, 01, 15);
            Assert.True(Target.CheckAutoAvailability(reservation));
        }

        [Fact]
        public void CheckAvailabilityIsFalseTest()
        {
            ReservationDto reservation = Target.GetReservationById(1);
            reservation.Auto = Target.GetAutoById(2);
            reservation.Von = new DateTime(2020, 01, 05);
            reservation.Bis = new DateTime(2020, 01, 15);
            Assert.False(Target.CheckAutoAvailability(reservation));
        }

        #endregion
    }
}
