using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.AdminGUI.ViewModels
{
    class AutoViewModel : BindableBase
    {
        private int _index;
        private int _counter;

        public AutoViewModel()
        {
            Autos = Target.GetAutoList();
            DeleteButtonClick = new RelayCommand(DeleteAuto, () => CanDeleteAuto);
            SaveButtonClick = new RelayCommand(SaveAuto, () => CanSave);
            AddButtonClick = new RelayCommand(AddAuto);
            RefreshButtonClick = new RelayCommand(Refresh);
            SortCommand = new RelayCommand<object>(Sort);
            AutoKlassen = new List<AutoKlasse> {AutoKlasse.Luxusklasse, AutoKlasse.Mittelklasse, AutoKlasse.Standard};

            Index = -1;
        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                SelectedIndexChanged();
            }
        }

        public List<AutoDto> Autos { get; set; }
        public List<AutoKlasse> AutoKlassen { get; set; }

        public RelayCommand DeleteButtonClick { get; set; }
        public RelayCommand SaveButtonClick { get; set; }
        public RelayCommand AddButtonClick { get; set; }
        public RelayCommand<object> SortCommand { get; set; }
        public RelayCommand RefreshButtonClick { get; set; }

        public AutoDto CurrentAuto { get; set; }

        public void SelectedIndexChanged()
        {
            if (Index >= 0)
            {
                CurrentAuto = Autos[Index];
                DeleteButtonClick.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentAuto));
            }
        }

        public void AddAuto()
        {
            Index = -1;
            CurrentAuto = new AutoDto();

            OnPropertyChanged(nameof(CurrentAuto));
            SaveButtonClick.RaiseCanExecuteChanged();
        }
        public void Sort(object parameter)
        {

            string column = parameter as string;
            List<AutoDto> AutosSorted;
            switch (column)
            {
                case "Marke":
                    if (_counter == 1)
                    {
                        AutosSorted = Autos
                           .OrderByDescending(s => s.Marke)
                           .ToList();
                        Autos = AutosSorted;
                        _counter = 0;
                    }
                    else
                    {
                        AutosSorted = Autos
                           .OrderBy(s => s.Marke)
                           .ToList();
                        Autos = AutosSorted;
                        _counter++;
                    }
                    break;
                case "Autoklasse":
                    if (_counter == 1)
                    {
                        AutosSorted = Autos
                           .OrderByDescending(s => s.AutoKlasse)
                           .ToList();
                        Autos = AutosSorted;
                        _counter = 0;
                    }
                    else
                    {
                        AutosSorted = Autos
                           .OrderBy(s => s.AutoKlasse)
                           .ToList();
                        Autos = AutosSorted;
                        _counter++;
                    }
                    break;
                case "Tarif":
                    if (_counter == 1)
                    {
                        AutosSorted = Autos
                           .OrderByDescending(s => s.Tagestarif)
                           .ToList();
                        Autos = AutosSorted;
                        _counter = 0;
                    }
                    else
                    {
                        AutosSorted = Autos
                           .OrderBy(s => s.Tagestarif)
                           .ToList();
                        Autos = AutosSorted;
                        _counter++;
                    }
                    break;

            }

            OnPropertyChanged(nameof(Autos));
        }

        public bool CheckInput()
        {
            if (CurrentAuto.Marke == "" || CurrentAuto.AutoKlasse.ToString() == ""  || CurrentAuto.Basistarif.ToString() == "")
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

        public void SaveAuto()
        {
            if (!CheckInput())
            {
                return;
            }

            foreach (AutoDto collectionDto in Autos)
            {
                if (CurrentAuto.Id == collectionDto.Id)
                {
                    Target.UpdateAuto(CurrentAuto);
                    Refresh();
                    return;
                }
            }

            AutoDto autoToBeInserted = new AutoDto
            {
                AutoKlasse = CurrentAuto.AutoKlasse,
                Basistarif = CurrentAuto.Basistarif,
                Marke = CurrentAuto.Marke,
                Tagestarif = CurrentAuto.Tagestarif
            };

            Target.InsertAuto(autoToBeInserted);
            Refresh();
        }

        public void Refresh()
        {
            Autos = Target.GetAutoList();
            CurrentAuto = null;

            OnPropertyChanged(nameof(Autos));
            OnPropertyChanged(nameof(CurrentAuto));
        }

        public bool CanDeleteAuto => CurrentAuto != null;
        public bool CanSave => CurrentAuto != null;

        public void DeleteAuto()
        {
            Target.DeleteAuto(CurrentAuto);

            Refresh();
        }
    }
}
