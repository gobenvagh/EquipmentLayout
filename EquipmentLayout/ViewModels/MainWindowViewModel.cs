using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using EquipmentLayout.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }

        public ObservableCollection<IRectItem> RectItems { get; set; }

        public ObservableCollection<ObstacleViewModel> ObstacleViewModels { get; set; }

        public ObservableCollection<IProperty> Properties { get; set; }

        public ObservableCollection<DeviceViewModel> DeviceViewModels { get; set; }

        public DelegateCommand CalcCommand { get; }
        public DelegateCommand DeleteTemplateCommand { get; }
        public DelegateCommand AddObstacleCommand { get; }
        public DelegateCommand OpenTemplateEditorCommand { get; }
        public RectItemsProvider RectItemsProvider { get; }
        public DelegateCommand AddTemplateCommand { get; }

        private Rectangle _zone;

        public Rectangle Zone
        {
            get => _zone;
            set
            {
                this._zone = value;
                OnPropertyChanged(nameof(Zone));
            }
        }

        private ObstacleViewModel _selectedObstacle;
        public ObstacleViewModel SelectedObstacle
        {
            get => _selectedObstacle;
            set
            {
                if (_selectedObstacle == value) return;
                _selectedObstacle = value;
                UpdateProperties();
                OnPropertyChanged();
            }
        }

        private DeviceTemplateViewModel _selectedDeviceTemplate;
        public DeviceTemplateViewModel SelectedDeviceTemplate
        {
            get => _selectedDeviceTemplate;
            set
            {
                if (_selectedDeviceTemplate == value) return;
                _selectedDeviceTemplate = value;
                UpdateProperties();
                OnPropertyChanged();
            }
        }

        private void UpdateProperties()
        {
            Action<DeviceTemplateViewModel, object> setter = (x, v) => (x as DeviceTemplateViewModel).Name = (string)v;
            Func<DeviceTemplateViewModel, object> getter = (x) => (x as DeviceTemplateViewModel).Name;

            var templateDataContext = _selectedDeviceTemplate;

            if (templateDataContext != null)
            {
                var props = new DeviceTemplatePropertiesBuilder().BuildProperties(templateDataContext);
                this.Properties = new ObservableCollection<IProperty>(props);
                OnPropertyChanged(nameof(Properties));
                return;
            }

            var obstacleDataContext = _selectedObstacle;

            if (obstacleDataContext != null)
            {
                var props = new ObstaclePropertiesBuilder().BuildProperties(obstacleDataContext);
                this.Properties = new ObservableCollection<IProperty>(props);
                OnPropertyChanged(nameof(Properties));
                return;
            }
        }

        public MainWindowViewModel()
        {
            DeviceTemplateViewModels = new ObservableCollection<DeviceTemplateViewModel>();
            RectItems = new ObservableCollection<IRectItem>();
            ObstacleViewModels = new ObservableCollection<ObstacleViewModel>();
            DeviceViewModels = new ObservableCollection<DeviceViewModel>();

            CalcCommand = new DelegateCommand(CalcCommand_Executed1);
            AddTemplateCommand = new DelegateCommand(AddTemplateCommand_Executed);
            DeleteTemplateCommand = new DelegateCommand(DeleteTemplateCommand_Executed);
            AddObstacleCommand = new DelegateCommand(AddObstacleCommand_Executed);

            RectItemsProvider = new RectItemsProvider(DeviceViewModels, ObstacleViewModels, RectItems);

            Zone = new Rectangle()
            {
                Width = 460,
                Height = 330,
            };

            var factory = new DeviceFactory();

            var template = new DeviceTemplate(100, 100, "MyDevice");
            var vm_template = new DeviceTemplateViewModel(template);

            DeviceTemplateViewModels.Add(vm_template);

            var template2 = new DeviceTemplate(200, 100, "MyDevice2");
            var vm_template2 = new DeviceTemplateViewModel(template2);

            var position = new Point(100, 50);
            var device1 = factory.GetDevice(position, template2);
            factory.GetDevice(position, template2);

            var position2 = new Point(20, 30);
            var device2 = factory.GetDevice(position2, template);
            factory.GetDevice(position2, template);
            factory.GetDevice(position2, template);

            var template3 = new DeviceTemplate(75, 90, "MyDevice3");
            factory.GetDevice(position2, template3);
            factory.GetDevice(position2, template3);
            factory.GetDevice(position2, template3);
            var vm_template3 = new DeviceTemplateViewModel(template3);

            DeviceTemplateViewModels.Add(vm_template3);

            //RectItems.Add(device1);
            //RectItems.Add(device2);

            DeviceTemplateViewModels.Add(vm_template2);

        }


        private void CalcCommand_Executed1()
        {
            try
            {
                var obstaclesVm = this.ObstacleViewModels;
                var obstacles = obstaclesVm.Select(x => x.Model).ToList();
                RectItems.Clear();
                var factory = new DeviceFactory();
                foreach (var deviceTemplateViewModel in DeviceTemplateViewModels)
                {
                    for (int i = 0; i < deviceTemplateViewModel.Count; i++)
                    {
                        var deviceTemplate = deviceTemplateViewModel.Model;
                        var device = factory.GetDevice(new Point(), deviceTemplate, false);
                        var deviceVm = new DeviceViewModel(device);
                        RectItems.Add(deviceVm);
                    }
                }



                /*var childRects = DeviceTemplateViewModels
                    .SelectMany(vm => Enumerable.Range(0, vm.Count).Select(_ => new int[] { vm.Model.Width, vm.Model.Height }))
                    .ToList();
*/
                var childRects = GenChildRects(DeviceTemplateViewModels);
                var parentRects = GetParentRects();
                var solutions = Solver.PlaceEquipment(childRects, parentRects, obstacles);

                // Размещение оборудования на свободных местах в зоне
                if (solutions.Count > 0)
                {
                    for (int i = 0; i < RectItems.Count(); i++)
                    {
                        if (RectItems[i] is DeviceViewModel device)
                        {
                            var solution = solutions[i];
                            var xOffset = new int[]{ device.X, device.WorkArea.X, device.ServiceArea.X }.Min();
                            var yOffset = new int[] { device.Y, device.WorkArea.Y, device.ServiceArea.Y }.Min();
                            device.X = solution[0] - xOffset;
                            device.Y = solution[1] - yOffset;
                            RectItems.Add(device.WorkArea);
                            RectItems.Add(device.ServiceArea);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Не удалось разместить оборудование без пересечений в зоне.");
                }

                foreach (var ob in obstaclesVm)
                    RectItems.Add(ob);

                OnPropertyChanged(nameof(RectItems));
            }
            catch (InvalidOperationException ex)
            {
                // Обработка ошибки размещения оборудования
                // Отображение сообщения об ошибке пользователю
                MessageBox.Show(ex.Message, "Ошибка размещения оборудования", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<int[]> GenChildRects(ObservableCollection<DeviceTemplateViewModel> deviceTemplateViewModels)
        {
            var childRects = new List<int[]>();
            foreach (var temp in deviceTemplateViewModels)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    var list = new List<int[]>();
                    var model = temp.Model;
                    list.Add(new int[] { 0, 0, model.Width, model.Height });
                    list.Add(new int[]
                    {
                        model.ServiceArea.X,
                        model.ServiceArea.Y,
                        model.ServiceArea.X + model.ServiceArea.Width,
                        model.ServiceArea.Y +  model.ServiceArea.Height
                    });

                    list.Add(new int[]
                    {
                        model.WorkArea.X,
                        model.WorkArea.Y,
                        model.WorkArea.X + model.WorkArea.Width,
                        model.WorkArea.Y +  model.WorkArea.Height
                    });

                    var device = OuterRect(list);



                    var width =  device[2] - device[0];
                    var height = device[3] - device[1];

                    childRects.Add(new int[] { width, height });
                }
            }

            return childRects;
        }

        int[] OuterRect(List<int[]> rects)
        {
            var x0 = rects.Min(r => r[0]);
            var y0 = rects.Min(r => r[1]);
            var x1 = rects.Max(r => r[2]);
            var y1 = rects.Max(r => r[3]);
            return new int[] { x0, y0, x1, y1 };
        }

        private List<int[]> GetParentRects()
        {
            var parentRects = new List<int[]>();
            parentRects.Add(new int[] { (int)Zone.Width, (int)Zone.Height });
            return parentRects;
        }

        private void DeleteTemplateCommand_Executed()
        {
            if (this.SelectedDeviceTemplate != null)
                this.DeviceTemplateViewModels.Remove(this.SelectedDeviceTemplate);
            else if (this.SelectedObstacle != null)
                this.RectItems.Remove(this.SelectedObstacle);

        }

        private void AddTemplateCommand_Executed()
        {
            if (this.SelectedDeviceTemplate != null)
                this.DeviceTemplateViewModels.Add(this.SelectedDeviceTemplate.Clone());
            else
                this.DeviceTemplateViewModels.Add(new DeviceTemplateViewModel());
        }

        private void AddObstacleCommand_Executed()
        {
            ObstacleViewModel obsVm;
            if (this.SelectedObstacle != null)
                obsVm = this.SelectedObstacle.Clone();
            else
                obsVm = new ObstacleViewModel();

            this.RectItems.Add(obsVm);
            OnPropertyChanged(nameof(SelectedObstacle));
        }

    }

    public class AreasProvider
    {

    }

    public class RectItemsProvider
    {
        private ObservableCollection<DeviceViewModel> _devices;
        private ObservableCollection<ObstacleViewModel> _obstacles;
        private ObservableCollection<IRectItem> _rectItems;
        public RectItemsProvider(
            ObservableCollection<DeviceViewModel> devices,
            ObservableCollection<ObstacleViewModel> obsacles,
            ObservableCollection<IRectItem> rectItems)
        {
            _devices = devices;
            _obstacles = obsacles;
            _rectItems = rectItems;
            _rectItems.CollectionChanged += _rectItems_CollectionChanged;
        }

        private void _rectItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _devices.Clear();
                    _obstacles.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems.OfType<DeviceViewModel>())
                        {
                            _devices.Add(item);
                        }
                        foreach (var item in e.NewItems.OfType<ObstacleViewModel>())
                            _obstacles.Add(item);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var item in e.OldItems.OfType<DeviceViewModel>())
                            _devices.Remove(item);
                        foreach (var item in e.OldItems.OfType<ObstacleViewModel>())
                            _obstacles.Remove(item);
                        break;
                    }
            }
        }
    }

    public class ObservableCollectionItemsProvider<S, T>
    {
        private ObservableCollection<S> _source;
        private ObservableCollection<T> _target;

        public ObservableCollectionItemsProvider(ObservableCollection<S> source, ObservableCollection<T> target)
        {
            _source = source;
            _target = target;
            _source.CollectionChanged += _source_CollectionChanged;
        }

        private void _source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (T item in e.NewItems.OfType<T>())
                            _target.Add(item);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (T item in e.OldItems.OfType<T>())
                            _target.Remove(item);
                        break;
                    }
            }
        }
    }

}
