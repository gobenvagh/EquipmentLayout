using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EquipmentLayout.ViewModels
{
    internal class DeviceAreaViewModel : BaseViewModel, IRectItem
    {
        DeviceViewModel _model;

        public string Name => _model.Name;

        private AreaType _type;

        public Brush Color => _type == AreaType.WorkArea? _model.WorkArea.Color : _model.;

        public int X => throw new NotImplementedException();

        public int Y => throw new NotImplementedException();

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();
    }
}
