using AutoReservation.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract]
    public interface IAutoReservationService
    {
        [OperationContract]
        KundeDto GetKundeById(int kundeId);
        [OperationContract]
        List<KundeDto> GetKundeList();
        [OperationContract]
        void InsertKunde(KundeDto kundeToBeInserted);
        [OperationContract]
        void UpdateKunde(KundeDto kundeToBeUpdated);
        [OperationContract]
        void DeleteKunde(KundeDto kundeToBeDeleteed);

        [OperationContract]
        AutoDto GetAutoById(int autoId);
        [OperationContract]
        List<AutoDto> GetAutoList();
        [OperationContract]
        void InsertAuto(AutoDto autoToBeInserted);
        [OperationContract]
        void UpdateAuto(AutoDto autoToBeUpdated);
        [OperationContract]
        void DeleteAuto(AutoDto autoToBeDeleteed);

        [OperationContract]
        ReservationDto GetReservationById(int reservationId);
        [OperationContract]
        List<ReservationDto> GetReservationList();
        [OperationContract]
        void InsertReservation(ReservationDto reservationToBeInserted);
        [OperationContract]
        void UpdateReservationo(ReservationDto reservationToBeUpdated);
        [OperationContract]
        void DeleteReservation(ReservationDto reservationToBeDeleteed);
    }
}
