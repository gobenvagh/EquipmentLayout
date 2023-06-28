using CsvHelper;
using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using EquipmentLayout.Views;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }

        public ObservableCollection<IRectItem> RectItems => Zone.RectItems;

        public ObservableCollection<ObstacleViewModel> ObstacleViewModels => Zone.ObstacleViewModels;

        public ObservableCollection<DeviceViewModel> DeviceViewModels => Zone.DeviceViewModels;

        public ObservableCollection<IProperty> Properties { get; set; }

        public DelegateCommand SaveCommand{ get; }
        public DelegateCommand CalcCommand { get; }
        public DelegateCommand DeleteTemplateCommand { get; }
        public DelegateCommand AddObstacleCommand { get; }
        public DelegateCommand OpenTemplateEditorCommand { get; }
        public DelegateCommand AddTemplateCommand { get; }
        public DelegateCommand AddZone { get; }
        public DelegateCommand RenameZone { get; }
        public DelegateCommand DeleteZone { get; }


        public ObservableCollection<ProvZoneViewModel> Zones { get; set; }

        private ProvZoneViewModel _zone;

        public ProvZoneViewModel Zone
        {
            get => _zone;
            set
            {
                _zone = value;
                OnPropertyChanged(nameof(Zone));
                OnPropertyChanged(nameof(DeviceViewModels));
                OnPropertyChanged(nameof(ObstacleViewModels));
                OnPropertyChanged(nameof(RectItems));

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

            Zones = new ObservableCollection<ProvZoneViewModel>();

            Zone = new ProvZoneViewModel()
            {
                Width = 460,
                Height = 330,
                Name = "Основное помещение"
            };

            Zones.Add(Zone);

            var zone2 = new ProvZoneViewModel()
            {
                Width = 460,
                Height = 330,
                Name = "Второе помещение"
            };

            Zones.Add(zone2);

            SaveCommand = new DelegateCommand(SaveCommand_Executed);
            CalcCommand = new DelegateCommand(CalcCommand_Executed1);
            AddTemplateCommand = new DelegateCommand(AddTemplateCommand_Executed);
            DeleteTemplateCommand = new DelegateCommand(DeleteTemplateCommand_Executed);
            AddObstacleCommand = new DelegateCommand(AddObstacleCommand_Executed);
            AddZone = new DelegateCommand(AddZone_Executed);
            DeleteZone = new DelegateCommand(DeleteZone_Executed);
            RenameZone = new DelegateCommand(RenameZone_Executed);

            InicializeDefaultState();
        }

        private void SaveCommand_Executed()            
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xlsx"; // Установка расширения файла по умолчанию
            dialog.AddExtension = true; // Добавление расширения, если не указано пользователем

            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                var devices = Zones.SelectMany(z => z.DeviceViewModels.Select(d => d.Model)).ToList();
                new ExcelExporter().ExportToExel(Zones.ToList(), DeviceTemplateViewModels.Select(d=>d.Model).ToList(), fileName);
            }


        }

        private void AddZone_Executed()
        {
            var dialog = new DialogZoneName();
            dialog.txtContent.Content = "Введите имя зоны";
            dialog.Title = "Выбор имени";
            if (dialog.ShowDialog() == true)
            {
                var zone = new ProvZoneViewModel();
                zone.Width = 300;
                zone.Height = 200;
                zone.Name = dialog.txtResult.Text;
                Zones.Add(zone);
            }
        }

        private void DeleteZone_Executed()
        {
            var zone = Zone;
            Zone = Zones.FirstOrDefault(z=>z != Zone);
            if(Zone == null)
            {
                MessageBox.Show("Нельзя удалить все зоны");
            }

            Zones.Remove(zone);
        }

        private void RenameZone_Executed()
        {
            var dialog = new DialogZoneName();
            dialog.txtContent.Content = "Введите имя зоны";
            dialog.Title = "Выбор имени";
            dialog.txtResult.Text = Zone.Name;
            if (dialog.ShowDialog() == true)
            { 
                Zone.Name = dialog.txtResult.Text;
            }

        }

        private void InicializeDefaultState()
        {



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

            DeviceTemplateViewModels.Add(vm_template2);
        }

        private string GroupToStr(IEnumerable<Device> group)
        {
            var count = group.Count();
            var frst = group.FirstOrDefault();
            var tmp = frst.Template.Name;
            return $"Кол-во: {count}, тип: {tmp}, ширина: {frst.Width}, длина: {frst.Height}";
        }

        private void CalcCommand_Executed1()
        {
            bool solved = false;
            var s = 0;

            var factory = new DeviceFactory();
            var devices = new List<Device>();
            foreach (var deviceTemplateViewModel in DeviceTemplateViewModels)
            {
                for (int i = 0; i < deviceTemplateViewModel.Count; i++)
                {
                    var deviceTemplate = deviceTemplateViewModel.Model;
                    var device = factory.GetDevice(new Point(), deviceTemplate, false);
                    devices.Add(device);
                }
            }

            while (!solved)
            {
                Zone = Zones[s++];

                var obstaclesVm = this.ObstacleViewModels.ToList();
                var obstacles = obstaclesVm.Select(x => x.Model).ToList();
                RectItems.Clear();

                var parentRects = GetParentRects();

                var solutions = new Solver().PlaceEquipment(devices, parentRects[0], obstacles);

                var rectItems = new List<IRectItem>(RectItems);

                var notSolvedDevices = devices.Where(d => !solutions.Contains(d)).ToList();

                if (solutions.Count > 0)
                {
                    for (int i = 0; i < solutions.Count(); i++)
                    {
                        var device = new DeviceViewModel(solutions[i]);

                        var solution = solutions[i];

                        RectItems.Add(device);
                        RectItems.Add(device.WorkArea);
                        RectItems.Add(device.ServiceArea);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Не удалось разместить оборудование без пересечений в зоне.");
                }
                foreach (var ob in obstaclesVm)
                    RectItems.Add(ob);

                OnPropertyChanged(nameof(RectItems));

                devices = notSolvedDevices;
                if (devices.Count == 0 || Zones.Count == s)
                    solved = true;
            }

            if (devices.Any())
            {
                string txt = devices
                    .GroupBy(d => d.Template)
                    .Select(g =>
                    GroupToStr(g))
                    .Aggregate((l, r) => l + '\n' + r);
                MessageBox.Show(txt, "Не удалось расставить устройства");
            }
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
