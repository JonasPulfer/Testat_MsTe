using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AutoReservation.Common.Interfaces;

namespace AutoReservation.AdminGUI.ViewModels
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static IAutoReservationService _target;
        public static IAutoReservationService Target
        {
            get
            {
                if (_target == null)
                {
                    ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
                    _target = channelFactory.CreateChannel();
                }
                return _target;
            }
        }
        
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
