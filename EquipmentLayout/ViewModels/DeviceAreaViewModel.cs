using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EquipmentLayout.ViewModels
{
    public class DeviceAreaViewModel : BaseViewModel, IRectItem
    {
        Device _device;
        Area _model;

        public string Name => string.Empty;

        private AreaType _type => _model.AreaType;

        public Brush Color => new SolidColorBrush(GetColor()) { Opacity = 0.5 };

        private Color GetColor()
        {
            if (_type == AreaType.ServiceArea)
                return Colors.LightGreen;
            if (_type == AreaType.WorkArea)
                return Colors.LightBlue;
            return Colors.White;

        }

        internal void DeviceSize_ChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            ResetXY();
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Width => _model.Width;

        public int Height => _model.Height;

        public DeviceAreaViewModel(Area model, Device device)
        {
            _device = device;
            _model = model;
            ResetXY();
        }

        private void ResetXY()
        {
            X = _device.X + _model.X;
            Y = _device.Y + _model.Y;
        }
    }
}
