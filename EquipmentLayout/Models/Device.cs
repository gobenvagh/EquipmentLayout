using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace EquipmentLayout.Models
{
    public class DeviceTemplate
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public int Count { get;set; }

        public Area WorkArea { get; set; }
        public Area ServiceArea { get; set; }

        public DeviceTemplate()
        {
            this.Width = 100;
            this.Height = 100;
            this.Name = "Template";
            Count = 0;

        }


        public DeviceTemplate(int width, int height, string name)
        {
            this.Width = width;
            this.Height = height;
            this.Name = name;
            Count = 0;
            WorkArea = new Area(0, 0, 0, 0, AreaType.WorkArea);
            ServiceArea = new Area(0, 0, 0, 0, AreaType.ServiceArea);
        }

        public DeviceTemplate Clone()
        {
            var clone = new DeviceTemplate(Width, Height, Name);
            //clone.WorkArea = this.WorkArea.Clone();
            //clone.ServiceArea = this.WorkArea.Clone();
            return clone;
        }

    }

    public class Area : IArea
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public AreaType AreaType { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public Area Clone()
        {
            var clone = new Area();
            clone.Width = Width;
            clone.Height = Height;
            clone.X = X;
            clone.Y = Y;
            clone.AreaType = AreaType;
            return clone;
        }

        private Area()
        {

        }

        public Area(int width, int height, int x, int y, AreaType areaType)
        {
            this.Width = width;
            this.Height = height;
            this.X = x;
            this.Y = y;
            this.AreaType = areaType;
        }

    }

    public enum AreaType
    {
        WorkArea,
        ServiceArea,
    }

    public interface IRectItem : IArea
    {
        string Name { get; }
        Brush Color { get; }
    }

    public interface IArea
    {
        int X { get; }
        int Y { get; }

        int Width { get; }
        int Height { get; }
    }

    public class Device : IRectItem
    {
        DeviceTemplate deviceTemplate;
        public int Width { get => deviceTemplate.Width;}
        public int Height { get => deviceTemplate.Height;}

        public int X { get; set; }

        public int Y { get; set; }
        public string Name => deviceTemplate.Name;

        public Brush Color => new SolidColorBrush(System.Windows.Media.Colors.AliceBlue) { Opacity = 0 };

        protected Device(DeviceTemplate deviceTemplate, int X, int Y)
        {
            this.deviceTemplate = deviceTemplate;
        }
    }

}
