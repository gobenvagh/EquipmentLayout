using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.ViewModels
{
    public class ProvZoneViewModel
    {
        public string Name { get; set; }
        public ObservableCollection<IRectItem> RectItems { get; set; }

        public ObservableCollection<ObstacleViewModel> ObstacleViewModels { get; set; }

        public ObservableCollection<DeviceViewModel> DeviceViewModels { get; set; }
        public RectItemsProvider RectItemsProvider { get; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ProvZoneViewModel()
        {
            RectItems = new ObservableCollection<IRectItem>();
            ObstacleViewModels = new ObservableCollection<ObstacleViewModel>();
            DeviceViewModels = new ObservableCollection<DeviceViewModel>();

            RectItemsProvider = new RectItemsProvider(DeviceViewModels, ObstacleViewModels, RectItems);

        }

    }
}
