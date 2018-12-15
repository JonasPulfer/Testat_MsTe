using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;

namespace AutoReservation.AdminGUI.ViewModels
{
    class AutoViewModel : BindableBase
    {
        public AutoViewModel()
        {
            Autos = Target.GetAutoList();
        }

        private List<AutoDto> _autos;

        public List<AutoDto> Autos
        {
            get
            {
                return _autos;
            }
            set
            {
                _autos = new List<AutoDto>(value);
            }
        }

        public AutoDto CurrentAuto { get; set; }

        public void SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ListBox ourList = (ListBox)sender;
            int index = ourList.SelectedIndex;
            if (index >= 0)
            {
                CurrentAuto = Autos[index];
                OnPropertyChanged(nameof(CurrentAuto));
            }
        }
    }
}
