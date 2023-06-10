using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceTemplate : IArea
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public int Count { get; set; }

        Area WorkArea { get; set; }
        Area ServiceArea { get; set; }

        class Area
        {
            public int Width { get; set; }
            public int Height { get; set; }

            public Point Position { get; set; }

            public Area Clone()
            {
                var clone = new Area();
                clone.Width = Width;
                clone.Height = Height;
                clone.Position = Position;
                return clone;
            }
        }

        public DeviceTemplate(int width, int height, string name)
        {
            this.Width = width;
            this.Height = height;
            this.Name = name;
        }

        public DeviceTemplate Clone()
        {
            var clone = new DeviceTemplate(Width, Height, Name);
            //clone.WorkArea = this.WorkArea.Clone();
            //clone.ServiceArea = this.WorkArea.Clone();
            return clone;
        }

    }

    public interface IArea
    {
        int Width { get; }
        int Height { get; }
    }

    public class Device : IArea, INotifyPropertyChanged
    {
        public DeviceTemplate deviceTemplate;
        public DeviceTemplate DeviceTemplate
        {
            get { return deviceTemplate; }
            set
            {
                deviceTemplate = value;
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(Height));
            }
        }

        private Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                position = value;
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
            this.DeviceTemplate = deviceTemplate;
            this.Position = position;
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
