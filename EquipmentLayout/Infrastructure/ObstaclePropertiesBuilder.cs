using EquipmentLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.Infrastructure
{
    internal class ObstaclePropertiesBuilder
    {
        public List<Property<ObstacleViewModel>> BuildProperties(ObstacleViewModel dataContext)
        {
            var properties = new List<Property<ObstacleViewModel>>
            {
                new Property<ObstacleViewModel>( "Имя", dataContext.Name, dataContext,
                (x, v) => x.Name = (string)v,
                (x) => x.Name),

                new Property<ObstacleViewModel>( "Ширина", dataContext.Width, dataContext,
                (x, v) => x.Width = int.Parse(v.ToString()),
                (x) => x.Width),

                new Property<ObstacleViewModel>( "Высота", dataContext.Height, dataContext,
                (x, v) => x.Height = int.Parse(v.ToString()),
                (x) => x.Height),

                new Property<ObstacleViewModel>( "X", dataContext.X, dataContext,
                (x, v) => x.X = int.Parse(v.ToString()),
                (x) => x.X),

                new Property<ObstacleViewModel>( "Y", dataContext.Y, dataContext,
                (x, v) => x.Y = int.Parse(v.ToString()),
                (x) => x.Y)
            };
            return properties;
        }
    }
}
