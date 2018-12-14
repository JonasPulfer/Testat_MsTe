using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

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
            try { kundeManager.Update(kundeToBeUpdated.ConvertToEntity()); }
            catch (OptimisticConcurrencyException<Kunde>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Update",
                    ProblemType = "Optimstic Cocurrency Error during updating on Kunde!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
            
        }

        public void DeleteKunde(KundeDto kundeToBeDeleteed)
        {
            WriteActualMethod();
            try { kundeManager.Delete(kundeToBeDeleteed.ConvertToEntity()); }
            catch (OptimisticConcurrencyException<Kunde>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Delete",
                    ProblemType = "Optimstic Cocurrency Error during deleting on Kunde!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
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
            try { autoManager.Update(autoToBeUpdated.ConvertToEntity()); }
            catch (OptimisticConcurrencyException<Auto>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Update",
                    ProblemType = "Optimstic Cocurrency Error during updating on Auto!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
        }

        public void DeleteAuto(AutoDto autoToBeDeleteed)
        {
            WriteActualMethod();
            try { autoManager.Delete(autoToBeDeleteed.ConvertToEntity()); }
            catch (OptimisticConcurrencyException<Auto>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Delete",
                    ProblemType = "Optimstic Cocurrency Error during deleting on Auto!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
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
            try { reservationManager.Insert(reservationToBeInserted.ConvertToEntity()); }
            catch (AutoUnavailableException)
            {
                AutoUnavailableFault auf = new AutoUnavailableFault
                {
                    Operation = "Insert",
                    ProblemType = "Auto is not available during this Time Range!"
                };
                throw new FaultException<AutoUnavailableFault>(auf);
            }
            catch (InvalidDateRangeException)
            {
                InvalidDateRangeFault idf = new InvalidDateRangeFault
                {
                    Operation = "Insert",
                    ProblemType = "Date is invalid!"
                };
                throw new FaultException<InvalidDateRangeFault>(idf);
            }
        }

        public void UpdateReservation(ReservationDto reservationToBeUpdated)
        {
            WriteActualMethod();
            try { reservationManager.Update(reservationToBeUpdated.ConvertToEntity()); }
            catch (AutoUnavailableException)
            {
                AutoUnavailableFault auf = new AutoUnavailableFault
                {
                    Operation = "Insert",
                    ProblemType = "Auto is not available during this Time Range!"
                };
                throw new FaultException<AutoUnavailableFault>(auf);
            }
            catch (InvalidDateRangeException)
            {
                InvalidDateRangeFault idf = new InvalidDateRangeFault
                {
                    Operation = "Insert",
                    ProblemType = "Date is invalid!"
                };
                throw new FaultException<InvalidDateRangeFault>(idf);
            }
            catch (OptimisticConcurrencyException<Reservation>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Update",
                    ProblemType = "Optimstic Cocurrency Error during updating on Reservation!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
        }

        public void DeleteReservation(ReservationDto reservationToBeDeleteed)
        {
            WriteActualMethod();
            try { reservationManager.Delete(reservationToBeDeleteed.ConvertToEntity()); }
            catch (OptimisticConcurrencyException<Reservation>)
            {
                OptimisticConcurrencyFault ocf = new OptimisticConcurrencyFault
                {
                    Operation = "Delete",
                    ProblemType = "Optimstic Cocurrency Error during deleting on Reservation!"
                };
                throw new FaultException<OptimisticConcurrencyFault>(ocf);
            }
        }

        public bool CheckAutoAvailability(ReservationDto reservation)
        {
            return reservationManager.CheckAutoAvailability(reservation.ConvertToEntity());   
        }

        public bool CheckDate(ReservationDto reservation)
        {
            return reservationManager.CheckDate(reservation.ConvertToEntity());   
        }
    }
}