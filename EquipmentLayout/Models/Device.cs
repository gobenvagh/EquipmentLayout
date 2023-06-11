using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace EquipmentLayout.Models
{
    public class DeviceTemplate : IArea
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        public int Count { get;set; }

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
        }

        public DeviceTemplate Clone()
        {
            var clone = new DeviceTemplate(Width, Height, Name);
            //clone.WorkArea = this.WorkArea.Clone();
            //clone.ServiceArea = this.WorkArea.Clone();
            return clone;
        }

    }

    public interface IRectItem : IArea
    {
        int X { get; }
        int Y { get; }
    }

    public interface IArea
    {
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
        protected Device(DeviceTemplate deviceTemplate, int X, int Y)
        {
            this.deviceTemplate = deviceTemplate;
        }
    }

}
