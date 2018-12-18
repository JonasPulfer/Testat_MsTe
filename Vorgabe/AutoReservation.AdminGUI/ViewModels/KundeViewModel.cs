using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.AdminGUI.ViewModels
{
    class KundeViewModel : BindableBase
    {
        private int _index;
        private int _counter;
        public List<KundeDto> Kunden { get; set; }
        public RelayCommand DeleteButtonClick { get; set; }
        public RelayCommand SaveButtonClick { get; set; }
        public RelayCommand AddButtonClick { get; set; }
        public RelayCommand RefreshButtonClick { get; set; }
        public RelayCommand<object> SortCommand { get; set; }
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
            SortCommand = new RelayCommand<object>(Sort);

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

        public void Sort(object parameter)
        {

            string column = parameter as string;
            List<KundeDto> KundenSorted;
            switch (column)
            {
                case "Vorname":
                    if (_counter == 1)
                    {
                        KundenSorted = Kunden
                           .OrderByDescending(s => s.Vorname)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter = 0;
                    }
                    else
                    {
                        KundenSorted = Kunden
                           .OrderBy(s => s.Vorname)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter++;
                    }
                    break;
                case "Nachname":
                    if (_counter == 1)
                    {
                       KundenSorted = Kunden
                           .OrderByDescending(s => s.Nachname)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter = 0;
                    }
                    else
                    {
                        KundenSorted = Kunden
                           .OrderBy(s => s.Nachname)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter++;
                    }
                    break;
                case "Geburtsdatum":
                    if (_counter == 1)
                    {
                        KundenSorted = Kunden
                           .OrderByDescending(s => s.Geburtsdatum)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter = 0;
                    }
                    else
                    {
                        KundenSorted = Kunden
                           .OrderBy(s => s.Geburtsdatum)
                           .ToList();
                        Kunden = KundenSorted;
                        _counter++;
                    }
                    break;

            }

            OnPropertyChanged(nameof(Kunden));
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
