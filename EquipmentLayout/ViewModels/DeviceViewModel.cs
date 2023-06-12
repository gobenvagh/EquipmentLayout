using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EquipmentLayout.ViewModels
{
    public class DeviceViewModel : BaseViewModel, IRectItem
    {
        Device _model;
        public string Name => _model.Name;

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
            get => _model.Height;
        }

        public AreaViewModel WorkArea => new AreaViewModel(_model.ServiceArea);

        public AreaViewModel ServiceArea => new AreaViewModel(_model.ServiceArea);

        public Brush Color => new SolidColorBrush(System.Windows.Media.Colors.AliceBlue) { Opacity = 0 };

        public DeviceViewModel(Device model)
        {
            _model = model;
        }
    }
}
