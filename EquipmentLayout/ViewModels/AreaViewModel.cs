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
    public class AreaViewModel : BaseViewModel, IArea, IRectItem
    {
        private Area _model;

        public int Width
        {
            get => _model.Width;
            set
            {
                _model.Width = value;
                OnPropertyChanged();
            }
        }
        public int Height
        {
            get => _model.Height;
            set
            {
                _model.Height = value;
                OnPropertyChanged();
            }
        }

        public AreaType AreaType
        {
            get => _model.AreaType;
            set
            {
                _model.AreaType = value;
                OnPropertyChanged();
            }
        }

        public int X
        {
            get => _model.X;
            set
            {
                _model.X = value;
                OnPropertyChanged();
            }
        }
        public int Y
        {
            get => _model.Y;
            set
            {
                _model.Y = value;
                OnPropertyChanged();
            }
        }

        public string Name => string.Empty;

        public Brush Color => new SolidColorBrush(GetColor()) { Opacity = 0.5 };

        private Color GetColor()
        {
            if (AreaType == AreaType.ServiceArea)
                return Colors.LightGreen;
            if (AreaType == AreaType.WorkArea)
                return  Colors.LightBlue;
            return Colors.White;

        }

        public AreaViewModel(Area model)
        {
            this._model = model;
        }
    }
}
