using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceTemplate : IArea, INotifyPropertyChanged
    {
        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public DeviceTemplate(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Name = name;
        }

        public DeviceTemplate Clone()
        {
            return new DeviceTemplate(Width, Height, Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IArea
    {
        int Width { get; }
        int Height { get; }
    }

    public class Device : IArea, INotifyPropertyChanged
    {
        private DeviceTemplate _deviceTemplate;
        public DeviceTemplate DeviceTemplate
        {
            get => _deviceTemplate;
            set
            {
                _deviceTemplate = value;
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(Height));
            }
        }

        private Point _position;
        public Point Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
            }
        }

        public int Width => DeviceTemplate?.Width ?? 0;
        public int Height => DeviceTemplate?.Height ?? 0;
        public string Name { get; set; }

        public int X => (int)Position.X;
        public int Y => (int)Position.Y;

        public Device(DeviceTemplate deviceTemplate, Point position, string name)
        {
            DeviceTemplate = deviceTemplate;
            Position = position;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
