using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
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
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void UpdateKunde(KundeDto kundeToBeUpdated);
        [OperationContract]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void DeleteKunde(KundeDto kundeToBeDeleteed);

        [OperationContract]
        AutoDto GetAutoById(int autoId);
        [OperationContract]
        List<AutoDto> GetAutoList();
        [OperationContract]
        void InsertAuto(AutoDto autoToBeInserted);
        [OperationContract]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void UpdateAuto(AutoDto autoToBeUpdated);
        [OperationContract]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void DeleteAuto(AutoDto autoToBeDeleteed);

        [OperationContract]
        ReservationDto GetReservationById(int reservationId);
        [OperationContract]
        List<ReservationDto> GetReservationList();
        [OperationContract]
        [FaultContract(typeof(AutoUnavailableFault))]
        [FaultContract(typeof(InvalidDateRangeFault))]
        void InsertReservation(ReservationDto reservationToBeInserted);
        [OperationContract]
        [FaultContract(typeof(AutoUnavailableFault))]
        [FaultContract(typeof(InvalidDateRangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void UpdateReservation(ReservationDto reservationToBeUpdated);
        [OperationContract]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void DeleteReservation(ReservationDto reservationToBeDeleteed);
    }
}
