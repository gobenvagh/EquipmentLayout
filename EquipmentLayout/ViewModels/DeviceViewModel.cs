using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.ViewModels
{
    public class DeviceViewModel : BaseViewModel
    {
        Device _model;

        public int X 
        {
            get => _model.X;
            set
            {
                if (value == _model.X) return;
                _model.X = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get => _model.Y;
            set
            {
                if (value == _model.Y) return;
                _model.Y = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _model.Width;
        }

        public int Height
        {
            get => _model.Width;
        }

        public DeviceViewModel(Device model)
        {
            _model = model;
        }
    }
}
