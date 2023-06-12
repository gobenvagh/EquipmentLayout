using EquipmentLayout.Models;
using EquipmentLayout.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.Infrastructure
{
    internal class DeviceTemplatePropertiesBuilder
    {
        public List<Property<DeviceTemplateViewModel>> BuildProperties(DeviceTemplateViewModel dataContext)
        {
            var properties = new List<Property<DeviceTemplateViewModel>>
            {
                new Property<DeviceTemplateViewModel>( "Имя", dataContext.Name, dataContext,
                (x, v) => x.Name = (string)v,
                (x) => x.Name),

                new Property<DeviceTemplateViewModel>( "Ширина", dataContext.Width, dataContext,
                (x, v) => x.Width = int.Parse(v.ToString()),
                (x) => x.Width),

                new Property<DeviceTemplateViewModel>( "Высота", dataContext.Height, dataContext,
                (x, v) => x.Height = int.Parse(v.ToString()),
                (x) => x.Height)
            };
            return properties;
        }

        private string AreaTypeToString(AreaType type)
        {
            if (type == AreaType.ServiceArea)
                return "Зона обслуживания";
            if (type == AreaType.WorkArea)
                return "Рабочая зона";
            throw new NotImplementedException();
        }

        public List<Property<AreaViewModel>> BuildPropertiesZone(AreaViewModel dataContext)
        {
            var properties = new List<Property<AreaViewModel>>
            {
                new Property<AreaViewModel>( "Тип", dataContext, dataContext,
                (x) => AreaTypeToString(dataContext.AreaType)),

                new Property<AreaViewModel>( "Ширина", dataContext.Width, dataContext,
                (x, v) => x.Width = int.Parse(v.ToString()),
                (x) => x.Width),

                new Property<AreaViewModel>( "Высота", dataContext.Height, dataContext,
                (x, v) => x.Height = int.Parse(v.ToString()),
                (x) => x.Height),

                 new Property<AreaViewModel>( "X", dataContext.X, dataContext,
                (x, v) => x.X = int.Parse(v.ToString()),
                (x) => x.X),

                new Property<AreaViewModel>( "Y", dataContext.Y, dataContext,
                (x, v) => x.Y = int.Parse(v.ToString()),
                (x) => x.Y)
            };

            return  properties;
        }

    }
}
