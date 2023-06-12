using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System.ComponentModel;
using System.Windows.Media;

namespace EquipmentLayout.ViewModels
{
    public class ObstacleViewModel : BaseViewModel, IRectItem
    {
        private Obstacle _model;

        public Obstacle Model => _model;

        public Brush Color => new SolidColorBrush(System.Windows.Media.Colors.IndianRed);


        private string _name;

        public string Name 
        { 
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int X
        {
            get => _model.X;
            set
            {
                if (_model.X == value) return;
                _model.X = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get => _model.Y;
            set
            {
                if (_model.Y == value) return;
                _model.Y = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _model.Width;
            set
            {
                if (_model.Width == value) return;
                _model.Width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _model.Height;
            set
            {
                if (_model.Height == value) return;
                _model.Height = value;
                OnPropertyChanged();
            }
        }

        public ObstacleViewModel Clone()
        {
            var clone = new ObstacleViewModel(this.Model.Clone());
            clone.Name = this.Name;
            return clone;
        }

        public ObstacleViewModel()
        {
            Name = "Препятствие";
            _model = new Obstacle(0, 0, 0, 0);
        }

        public ObstacleViewModel(int x, int y, int width, int height)
        {
            Name = "Препятствие";
            _model = new Obstacle(x, y, width, height);
        }

        public ObstacleViewModel(Obstacle model)
        {
            Name = "Препятствие";
            this._model = model;
        }


    }
}
