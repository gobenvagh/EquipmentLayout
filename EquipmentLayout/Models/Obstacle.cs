using System.Windows;

namespace EquipmentLayout.Models
{
    public class Obstacle : IArea
    {
        public Point Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Obstacle(Point position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }
}
