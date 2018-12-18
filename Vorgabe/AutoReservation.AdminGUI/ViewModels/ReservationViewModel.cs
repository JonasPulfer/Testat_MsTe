using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;

namespace AutoReservation.AdminGUI.ViewModels
{
    class ReservationViewModel : BindableBase
    {
        private int _index;
        public List<ReservationDto> Reservationen { get; set; }
        public List<KundeDto> Kunden { get; set; }
        public List<AutoDto> Autos { get; set; }
        public RelayCommand DeleteButtonClick { get; set; }
        public RelayCommand SaveButtonClick { get; set; }
        public RelayCommand AddButtonClick { get; set; }
        public RelayCommand RefreshButtonClick { get; set; }
        public ReservationDto CurrentReservation { get; set; }
        public int AutoIndex { get; set; }
        public int KundeIndex { get; set; }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                SelectedIndexChanged();
            }
        }
        public bool CanDeleteReservation => CurrentReservation != null;
        public bool CanSaveReservation => CurrentReservation != null;

        public ReservationViewModel()
        {
            Reservationen = Target.GetReservationList();
            Autos = Target.GetAutoList();
            Kunden = Target.GetKundeList();
            DeleteButtonClick = new RelayCommand(DeleteReservation, () => CanDeleteReservation);
            SaveButtonClick = new RelayCommand(SaveReservation, () => CanSaveReservation);
            AddButtonClick = new RelayCommand(AddReservation);
            RefreshButtonClick = new RelayCommand(Refresh);

            Index = -1;
            AutoIndex = -1;
            KundeIndex = -1;
        }

        public void SelectedIndexChanged()
        {
            if (Index >= 0)
            {
                CurrentReservation = Reservationen[Index];
                AutoIndexChanged();
                KundeIndexChanged();

                DeleteButtonClick.RaiseCanExecuteChanged();

                OnPropertyChanged(nameof(CurrentReservation));
            }
        }

        public void AddReservation()
        {
            Index = -1;
            AutoIndex = -1;
            KundeIndex = -1;
            CurrentReservation = new ReservationDto();

            AutoIndexChanged();
            KundeIndexChanged();

            OnPropertyChanged(nameof(CurrentReservation));

            SaveButtonClick.RaiseCanExecuteChanged();
        }

        public bool CheckInput()
        {
            if (Autos[AutoIndex] == null || Kunden[KundeIndex] == null || CurrentReservation.Von.ToShortDateString() == "" || CurrentReservation.Bis.ToShortDateString() == "")
            {
                ShowMsgBox("invalid names! Please check the input.", "Invalid Input");

                return false;
            }

            return true;
        }

        public void SaveReservation()
        {
            if (!CheckInput())
            {
                return;
            }

            foreach (ReservationDto collectionDto in Reservationen)
            {
                if (CurrentReservation.ReservationsNr == collectionDto.ReservationsNr)
                {
                    try
                    {
                        Target.UpdateReservation(CurrentReservation);
                    }
                    catch (FaultException<AutoUnavailableFault>)
                    {
                        ShowMsgBox("Auto is not available during this date range", "Auto Unavailable");
                    }
                    catch (FaultException<InvalidDateRangeFault>)
                    {
                        ShowMsgBox("Date range is invalid", "Invalid Date Range");
                    }
                    catch (FaultException<OptimisticConcurrencyFault>)
                    {
                        ShowMsgBox("Optimistic Concurrency Fault. Someone else is currently editing this reservation", "Optimistic Concurrency Fault");
                    }

                    Refresh();
                    return;
                }
            }

            ReservationDto reservationToBeInserted = new ReservationDto
            {
                Kunde = Kunden[KundeIndex],
                Auto = Autos[AutoIndex],
                Bis = CurrentReservation.Bis,
                Von = CurrentReservation.Von
            };

            try
            {
                Target.InsertReservation(reservationToBeInserted);
            }
            catch (FaultException<AutoUnavailableFault>)
            {
                ShowMsgBox("Auto is not available during this date range", "Auto Unavailable");
            }
            catch (FaultException<InvalidDateRangeFault>)
            {
                ShowMsgBox("Date range is invalid", "Invalid Date Range");
            }
            catch (FaultException<OptimisticConcurrencyFault>)
            {
                ShowMsgBox("Optimistic Concurrency Fault. Someone else is currently editing this reservation", "Optimistic Concurrency Fault");
            }
            Refresh();
        }

        public void Refresh()
        {
            Reservationen = Target.GetReservationList();
            CurrentReservation = null;

            OnPropertyChanged(nameof(Reservationen));
            OnPropertyChanged(nameof(CurrentReservation));
        }

        public void DeleteReservation()
        {
            try
            {
                Target.DeleteReservation(CurrentReservation);
            }
            catch (FaultException<OptimisticConcurrencyFault>)
            {
                ShowMsgBox("Optimistic Concurrency Fault. Someone else is currently editing this reservation", "Optimistic Concurrency Fault");
            }

            Refresh();
        }

        public void AutoIndexChanged()
        {
            if (CurrentReservation.Auto != null)
            {
                for (int i = 0; i < Autos.Count; i++)
                {
                    if (Autos[i].Id == CurrentReservation.Auto.Id)
                    {
                        AutoIndex = i;
                    }
                }
                OnPropertyChanged(nameof(AutoIndex));
            }
        }

        public void KundeIndexChanged()
        {
            if (CurrentReservation.Kunde != null)
            {
                for (int i = 0; i < Kunden.Count; i++)
                {
                    if (Kunden[i].Id == CurrentReservation.Kunde.Id)
                    {
                        KundeIndex = i;
                    }
                }
                OnPropertyChanged(nameof(KundeIndex));
            }
        }

        public void ShowMsgBox(string content, string title)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            MessageBox.Show(content, title, button, icon);
        }
    }
}
