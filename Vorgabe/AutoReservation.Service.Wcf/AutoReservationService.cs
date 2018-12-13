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

        public KundeDto GetKundeById(int kundeId)
        {
            WriteActualMethod();
            return kundeManager.GetById(kundeId).ConvertToDto();
        }


        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public List<KundeDto> GetKundeList()
        {
            return DtoConverter.ConvertToDtos(kundeManager.List);
        }

        public void InsertKunde(KundeDto kundeToBeInserted)
        {
            kundeManager.Insert(kundeToBeInserted.ConvertToEntity());
        }

        public void UpdateKunde(KundeDto kundeToBeUpdated)
        {
            kundeManager.Update(kundeToBeUpdated.ConvertToEntity());
        }

        public void DeleteKunde(KundeDto kundeToBeDeleteed)
        {
            kundeManager.Delete(kundeToBeDeleteed.ConvertToEntity());
        }

        public AutoDto GetAutoById(int autoId)
        {
            return autoManager.GetById(autoId).ConvertToDto();
        }

        public List<AutoDto> GetAutoList()
        {
            return DtoConverter.ConvertToDtos(autoManager.List);
        }

        public void InsertAuto(AutoDto autoToBeInserted)
        {
            autoManager.Insert(autoToBeInserted.ConvertToEntity());
        }

        public void UpdateAuto(AutoDto autoToBeUpdated)
        {
            autoManager.Update(autoToBeUpdated.ConvertToEntity());
        }

        public void DeleteAuto(AutoDto autoToBeDeleteed)
        {
            autoManager.Delete(autoToBeDeleteed.ConvertToEntity());
        }

        public ReservationDto GetReservationById(int reservationId)
        {
            return reservationManager.GetById(reservationId).ConvertToDto();
        }

        public List<ReservationDto> GetReservationList()
        {
            return DtoConverter.ConvertToDtos(reservationManager.List);
        }

        public void InsertReservation(ReservationDto reservationToBeInserted)
        {
            reservationManager.Insert(reservationToBeInserted.ConvertToEntity());
        }

        public void UpdateReservationo(ReservationDto reservationToBeUpdated)
        {
            reservationManager.Update(reservationToBeUpdated.ConvertToEntity());
        }

        public void DeleteReservation(ReservationDto reservationToBeDeleteed)
        {
            reservationManager.Delete(reservationToBeDeleteed.ConvertToEntity());
        }
    }
}