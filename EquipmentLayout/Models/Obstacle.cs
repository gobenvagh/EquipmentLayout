using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace EquipmentLayout.Models
{
    public class Obstacle 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Name => string.Empty;

        public Obstacle Clone()
        {
            var clone = new Obstacle(X, Y, Width, Height);
            return clone;
        }

        public Obstacle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
