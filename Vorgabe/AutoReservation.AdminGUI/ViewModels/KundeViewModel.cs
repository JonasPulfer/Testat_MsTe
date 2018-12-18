using System.Collections.Generic;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.AdminGUI.ViewModels
{
    class KundeViewModel : BindableBase
    {
        private int _index;
        public List<KundeDto> Kunden { get; set; }
        public RelayCommand DeleteButtonClick { get; set; }
        public RelayCommand SaveButtonClick { get; set; }
        public RelayCommand AddButtonClick { get; set; }
        public RelayCommand RefreshButtonClick { get; set; }
        public KundeDto CurrentKunde { get; set; }
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                SelectedIndexChanged();
            }
        }

        public bool CanDeleteKunde => CurrentKunde != null;
        public bool CanSaveKunde => CurrentKunde != null;

        public KundeViewModel()
        {
            Kunden = Target.GetKundeList();
            DeleteButtonClick = new RelayCommand(DeleteKunde, () => CanDeleteKunde);
            SaveButtonClick = new RelayCommand(SaveKunde, () => CanSaveKunde);
            AddButtonClick = new RelayCommand(AddKunde);
            RefreshButtonClick = new RelayCommand(Refresh);

            Index = -1;
        }

        public void SelectedIndexChanged()
        {
            if (Index >= 0)
            {
                CurrentKunde = Kunden[Index];
                DeleteButtonClick.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentKunde));
            }
            else
            {
                CurrentKunde = null;
                DeleteButtonClick.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentKunde));
            }
        }

        public void AddKunde()
        {
            Index = -1;
            CurrentKunde = new KundeDto();

            OnPropertyChanged(nameof(CurrentKunde));
            SaveButtonClick.RaiseCanExecuteChanged();
        }

        public bool CheckInput()
        {
            if (CurrentKunde.Vorname == "" || CurrentKunde.Nachname == "" || CurrentKunde.Geburtsdatum.ToShortDateString() == "")
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

        public void SaveKunde()
        {
            if (!CheckInput())
            {
                return;
            }

            foreach (KundeDto collectionDto in Kunden)
            {
                if (CurrentKunde.Id == collectionDto.Id)
                {
                    Target.UpdateKunde(CurrentKunde);
                    Refresh();
                    return;
                }
            }

            KundeDto kundeToBeInserted = new KundeDto
            {
                Vorname = CurrentKunde.Vorname,
                Geburtsdatum = CurrentKunde.Geburtsdatum,
                Nachname = CurrentKunde.Nachname
            };

            Target.InsertKunde(kundeToBeInserted);
            Refresh();
        }

        public void Refresh()
        {
            Kunden = Target.GetKundeList();
            CurrentKunde = null;

            OnPropertyChanged(nameof(Kunden));
            OnPropertyChanged(nameof(CurrentKunde));
        }

        public void DeleteKunde()
        {
            Target.DeleteKunde(CurrentKunde);

            Refresh();
        }
    }
}
