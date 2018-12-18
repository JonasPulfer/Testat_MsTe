using System.Collections.Generic;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;

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
        }

        public void SelectedIndexChanged()
        {
            if (Index >= 0)
            {
                CurrentReservation = Reservationen[Index];
                DeleteButtonClick.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentReservation));
            }
        }

        public void AddReservation()
        {
            Index = -1;
            CurrentReservation = new ReservationDto();

            OnPropertyChanged(nameof(CurrentReservation));
            SaveButtonClick.RaiseCanExecuteChanged();
        }

        public bool CheckInput()
        {
            if (CurrentReservation.Auto == null || CurrentReservation.Kunde == null || CurrentReservation.Von.ToShortDateString() == "" || CurrentReservation.Bis.ToShortDateString() == "")
            {
                string messageBoxText = "invalid names! Please check the input.";
                string caption = "Invalid Input";

                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon);

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
                    Target.UpdateReservation(CurrentReservation);
                    Refresh();
                    return;
                }
            }

            ReservationDto reservationToBeInserted = new ReservationDto
            {
                Kunde = CurrentReservation.Kunde,
                Auto = CurrentReservation.Auto,
                Bis = CurrentReservation.Bis,
                Von = CurrentReservation.Von
            };

            Target.InsertReservation(reservationToBeInserted);
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
            Target.DeleteReservation(CurrentReservation);

            Refresh();
        }
    }
}
