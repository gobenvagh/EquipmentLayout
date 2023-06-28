using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.ViewModels
{
    public class ProvZoneViewModel : BaseViewModel
    {
        private string _name;
        public string Name 
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
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
