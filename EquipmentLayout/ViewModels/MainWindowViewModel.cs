using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Google.OrTools.Sat;

namespace EquipmentLayout.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<DeviceTemplateViewModel> DeviceTemplateViewModels { get; set; }
        public ObservableCollection<LayoutModel> LayoutModels { get; set; }

        public LayoutModel SelectedLayoutModel { get; set; }
        private DeviceTemplateViewModel _selectedDeviceTemplate;

        public DelegateCommand CreateLayoutCommand { get; set; }
        public DelegateCommand SwitchLayoutCommand { get; set; }
        public DelegateCommand CalcCommand { get; set; }
        public DelegateCommand AddObstacleCommand { get; set; }
        public DelegateCommand DeleteTemplateCommand { get; set; }
        public DelegateCommand AddTemplateCommand { get; set; }


        private Rectangle _zone;

        public Rectangle Zone
        {
            get => _zone;
            set
            {
                _zone = value;
                OnPropertyChanged(nameof(Zone));
            }
        }

        public ObservableCollection<Device> RectItems { get; set; }
        public ObservableCollection<Obstacle> Obstacles { get; set; }


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

        public ObservableCollection<Property<DeviceTemplateViewModel>> Properties { get; set; }

        private TextBox _widthTextBox;
        public TextBox WidthTextBox
        {
            get => _widthTextBox;
            set
            {
                _widthTextBox = value;
                OnPropertyChanged(nameof(WidthTextBox));
            }
        }

        private TextBox _heightTextBox;
        public TextBox HeightTextBox
        {
            get => _heightTextBox;
            set
            {
                _heightTextBox = value;
                OnPropertyChanged(nameof(HeightTextBox));
            }
        }

        private TextBox _xTextBox;
        public TextBox XTextBox
        {
            get => _xTextBox;
            set
            {
                _xTextBox = value;
                OnPropertyChanged(nameof(XTextBox));
            }
        }

        private TextBox _yTextBox;
        public TextBox YTextBox
        {
            get => _yTextBox;
            set
            {
                _yTextBox = value;
                OnPropertyChanged(nameof(YTextBox));
            }
        }

        private string _obstacleWidth;
        public string ObstacleWidth
        {
            get => _obstacleWidth;
            set
            {
                _obstacleWidth = value;
                OnPropertyChanged(nameof(ObstacleWidth));
            }
        }

        private string _obstacleHeight;
        public string ObstacleHeight
        {
            get => _obstacleHeight;
            set
            {
                _obstacleHeight = value;
                OnPropertyChanged(nameof(ObstacleHeight));
            }
        }

        private string _obstacleX;
        public string ObstacleX
        {
            get => _obstacleX;
            set
            {
                _obstacleX = value;
                OnPropertyChanged(nameof(ObstacleX));
            }
        }

        private string _obstacleY;
        public string ObstacleY
        {
            get => _obstacleY;
            set
            {
                _obstacleY = value;
                OnPropertyChanged(nameof(ObstacleY));
            }
        }

        private void UpdateProperties()
        {
            var model = _selectedDeviceTemplate;

            if (model == null)
            {
                Properties = null;
                OnPropertyChanged(nameof(Properties));
                return;
            }

            var properties = new List<Property<DeviceTemplateViewModel>>
            {
                new Property<DeviceTemplateViewModel>("Имя", model.Name, model,
                    (x, v) => x.Name = (string)v,
                    (x) => x.Name),

                new Property<DeviceTemplateViewModel>("Ширина", model.Width, model,
                    (x, v) => x.Width = int.Parse(v.ToString()),
                    (x) => x.Width),

                new Property<DeviceTemplateViewModel>("Высота", model.Height, model,
                    (x, v) => x.Height = int.Parse(v.ToString()),
                    (x) => x.Height)
            };

            Properties = new ObservableCollection<Property<DeviceTemplateViewModel>>(properties);
            OnPropertyChanged(nameof(Properties));
        }

        private void CalcCommand_Executed()
        {
            RectItems.Clear();
            foreach (var deviceTemplateViewModel in DeviceTemplateViewModels)
            {
                for (int i = 0; i < deviceTemplateViewModel.Count; i++)
                {
                    var deviceTemplate = deviceTemplateViewModel.Model;
                    var device = new Device(deviceTemplate, new Point(), "Device");
                    RectItems.Add(device);
                }
            }

            var childRects = DeviceTemplateViewModels
                .SelectMany(vm => Enumerable.Range(0, vm.Count).Select(_ => new int[] { vm.Width, vm.Height }))
                .ToList();
            var parentRects = GetParentRects();
            var solutions = Solver.PlaceEquipment(childRects, parentRects);

            if (solutions.Count > 0)
            {
                for (int i = 0; i < RectItems.Count; i++)
                {
                    var device = RectItems[i];
                    var solution = solutions[i];
                    device.Position = new Point(solution[0], solution[1]);
                }
            }

            OnPropertyChanged(nameof(RectItems));
        }

        private List<int[]> GetParentRects()
        {
            var parentRects = new List<int[]>();
            parentRects.Add(new int[] { (int)Zone.Width, (int)Zone.Height });
            return parentRects;
        }

        private void DeleteTemplateCommand_Executed()
        {
            DeviceTemplateViewModels.Remove(SelectedDeviceTemplate);
        }

        private void AddTemplateCommand_Executed()
        {
            if (SelectedDeviceTemplate != null)
            {
                var deviceTemplate = SelectedDeviceTemplate.Clone();
                DeviceTemplateViewModels.Add(deviceTemplate);
            }
            else
            {
                var template2 = new DeviceTemplate(200, 100, "Name");
                var vm_template2 = new DeviceTemplateViewModel(template2);
                DeviceTemplateViewModels.Add(vm_template2);
            }
        }






        private void AddObstacleCommand_Executed()
        {
            if (int.TryParse(ObstacleWidth, out int width) &&
                int.TryParse(ObstacleHeight, out int height) &&
                int.TryParse(ObstacleX, out int x) &&
                int.TryParse(ObstacleY, out int y))
            {
                var obstacle = new Obstacle(new Point(x, y), width, height);
                Obstacles.Add(obstacle);
            }

            ObstacleWidth = "0";
            ObstacleHeight = "0";
            ObstacleX = "0";
            ObstacleY = "0";
        }



        private void OnLoad()
        {
            WidthTextBox = GetTextBoxByName("WidthTextBox");
            HeightTextBox = GetTextBoxByName("HeightTextBox");
            XTextBox = GetTextBoxByName("XTextBox");
            YTextBox = GetTextBoxByName("YTextBox");
            var initialLayout = new LayoutModel();
            initialLayout.Name = "Layout 1";
            LayoutModels = new ObservableCollection<LayoutModel>();
            LayoutModels.Add(initialLayout);
            SelectedLayoutModel = initialLayout;
        }

        private TextBox GetTextBoxByName(string name)
        {
            var textBox = new TextBox();
            textBox.Name = name;
            return textBox;
        }

        public MainWindowViewModel()
        {
            DeviceTemplateViewModels = new ObservableCollection<DeviceTemplateViewModel>();
            RectItems = new ObservableCollection<Device>();
            CalcCommand = new DelegateCommand(CalcCommand_Executed);
            AddTemplateCommand = new DelegateCommand(AddTemplateCommand_Executed);
            DeleteTemplateCommand = new DelegateCommand(DeleteTemplateCommand_Executed);
            AddObstacleCommand = new DelegateCommand(AddObstacleCommand_Executed);

            LayoutModels = new ObservableCollection<LayoutModel>();
            Zone = new Rectangle()
            {
                Width = 1100,
                Height = 700,
            };
            CreateLayoutCommand = new DelegateCommand(CreateLayoutCommand_Executed);
            SwitchLayoutCommand = new DelegateCommand(SwitchLayoutCommand_Executed);
            Obstacles = new ObservableCollection<Obstacle>();

            var factory = new DeviceFactory();

            var template = new DeviceTemplate(100, 100, "MyDevice");
            var vm_template = new DeviceTemplateViewModel(template);
            DeviceTemplateViewModels.Add(vm_template);

            var template2 = new DeviceTemplate(200, 100, "MyDevice2");
            var vm_template2 = new DeviceTemplateViewModel(template2);

            var position = new Point(100, 50);
            var device1 = factory.GetDevice(position, template2, "Device1");

            var position2 = new Point(20, 30);
            var device2 = factory.GetDevice(position2, template, "Device2");

            DeviceTemplateViewModels.Add(vm_template2);
            OnLoad();
        }

        private int layoutCount = 1;
        private void CreateLayoutCommand_Executed()
        {
            layoutCount++;
            var newLayout = new LayoutModel();
            newLayout.Name = $"Layout {layoutCount}";
            LayoutModels.Add(newLayout);
            SelectedLayoutModel = newLayout;
            Obstacles.Clear();
        }

        private void SwitchLayoutCommand_Executed()
        {
            layoutCount++;
            Obstacles.Clear();
        }
    }

    public class Property<T>
    {
        private T _model;

        public string Name { get; set; }
        public object Value
        {
            get => _getter(_model);
            set => _setter(_model, value);
        }

        private Action<T, object> _setter;
        private Func<T, object> _getter;

        public Property(string name, object value, T model, Action<T, object> setter, Func<T, object> getter)
        {
            _model = model;
            _setter = setter;
            _getter = getter;
            Name = name;
            Value = value;
        }
    }
}
