﻿using System.Collections.Generic;
using System.Windows;
using AutoReservation.AdminGUI.ViewModels.Commands;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.AdminGUI.ViewModels
{
    class AutoViewModel : BindableBase
    {
        private int _index;

        public AutoViewModel()
        {
            Autos = Target.GetAutoList();
            DeleteButtonClick = new RelayCommand(DeleteAuto, () => CanDeleteAuto);
            SaveButtonClick = new RelayCommand(Save, () => CanSave);
            AddButtonClick = new RelayCommand(Add);

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

        public RelayCommand DeleteButtonClick { get; set; }
        public RelayCommand SaveButtonClick { get; set; }
        public RelayCommand AddButtonClick { get; set; }

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

        public void Add()
        {
            Index = -1;
            CurrentAuto = new AutoDto();

            SaveButtonClick.RaiseCanExecuteChanged();
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

        public void Save()
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
