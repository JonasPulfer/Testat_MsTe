using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService
        : IAutoReservationService
    {
        private KundeManager kundeManager = new KundeManager();
        private AutoManager autoManager = new AutoManager();
        private ReservationManager reservationManager = new ReservationManager();

        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public KundeDto GetKundeById(int kundeId)
        {
            WriteActualMethod();
            return kundeManager.GetById(kundeId).ConvertToDto();
        }      

        public List<KundeDto> GetKundeList()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(kundeManager.List);
        }

        public void InsertKunde(KundeDto kundeToBeInserted)
        {
            WriteActualMethod();
            kundeManager.Insert(kundeToBeInserted.ConvertToEntity());
        }

        public void UpdateKunde(KundeDto kundeToBeUpdated)
        {
            WriteActualMethod();
            kundeManager.Update(kundeToBeUpdated.ConvertToEntity());
        }

        public void DeleteKunde(KundeDto kundeToBeDeleteed)
        {
            WriteActualMethod();
            kundeManager.Delete(kundeToBeDeleteed.ConvertToEntity());
        }

        public AutoDto GetAutoById(int autoId)
        {
            WriteActualMethod();
            return autoManager.GetById(autoId).ConvertToDto();
        }

        public List<AutoDto> GetAutoList()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(autoManager.List);
        }

        public void InsertAuto(AutoDto autoToBeInserted)
        {
            WriteActualMethod();
            autoManager.Insert(autoToBeInserted.ConvertToEntity());
        }

        public void UpdateAuto(AutoDto autoToBeUpdated)
        {
            WriteActualMethod();
            autoManager.Update(autoToBeUpdated.ConvertToEntity());
        }

        public void DeleteAuto(AutoDto autoToBeDeleteed)
        {
            WriteActualMethod();
            autoManager.Delete(autoToBeDeleteed.ConvertToEntity());
        }

        public ReservationDto GetReservationById(int reservationId)
        {
            WriteActualMethod();
            return reservationManager.GetById(reservationId).ConvertToDto();
        }

        public List<ReservationDto> GetReservationList()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(reservationManager.List);
        }

        public void InsertReservation(ReservationDto reservationToBeInserted)
        {
            WriteActualMethod();
            reservationManager.Insert(reservationToBeInserted.ConvertToEntity());
        }

        public void UpdateReservationo(ReservationDto reservationToBeUpdated)
        {
            WriteActualMethod();
            reservationManager.Update(reservationToBeUpdated.ConvertToEntity());
        }

        public void DeleteReservation(ReservationDto reservationToBeDeleteed)
        {
            WriteActualMethod();
            reservationManager.Delete(reservationToBeDeleteed.ConvertToEntity());
        }
    }
}