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
    }
}
