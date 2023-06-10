using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System.ComponentModel;

namespace EquipmentLayout.ViewModels
{
    public class ObstacleViewModel : BaseViewModel
    {
        private Obstacle _model;

        public double X
        {
            get => _model.X;
            set
            {
                if (_model.X == value) return;
                _model.X = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get => _model.Y;
            set
            {
                if (_model.Y == value) return;
                _model.Y = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => _model.Width;
            set
            {
                if (_model.Width == value) return;
                _model.Width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => _model.Height;
            set
            {
                if (_model.Height == value) return;
                _model.Height = value;
                OnPropertyChanged();
            }
        }

        public ObstacleViewModel()
        {
            _model = new Obstacle(0, 0, 0, 0);
        }

        public ObstacleViewModel(double x, double y, double width, double height)
        {
            _model = new Obstacle(x, y, width, height);
        }

        public Obstacle ToObstacle()
        {
            return _model;
        }
    }
}
