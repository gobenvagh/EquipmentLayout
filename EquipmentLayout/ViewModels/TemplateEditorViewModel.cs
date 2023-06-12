using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System.Collections.ObjectModel;

namespace EquipmentLayout.ViewModels
{
    internal class TemplateEditorViewModel: BaseViewModel
    {
        private DeviceTemplateViewModel _template;

        public ObservableCollection<IProperty> TemplateProperties { get; set; }

        public ObservableCollection<IProperty> ServiceAreaProperties { get; set; }

        public ObservableCollection<IProperty> WorkAreaProperties { get; set; }
        
        public ObservableCollection<IRectItem> RectItems { get; set; }

        public TemplateEditorViewModel(DeviceTemplateViewModel template)
        {
            var builder = new DeviceTemplatePropertiesBuilder();
            this._template = template;
            TemplateProperties = new ObservableCollection<IProperty>(builder.BuildProperties(template));
            WorkAreaProperties = new ObservableCollection<IProperty>(builder.BuildPropertiesZone(template.WorkArea));
            ServiceAreaProperties = new ObservableCollection<IProperty>(builder.BuildPropertiesZone(template.ServiceArea));
            RectItems = new ObservableCollection<IRectItem>();
            RectItems.Add(_template.GetDevice(0,0));
            RectItems.Add(_template.WorkArea);
            RectItems.Add(_template.ServiceArea);
        }
    }
}