using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System.ComponentModel;
using System.Windows.Controls;

namespace EquipmentLayout.ViewModels
{
    public class DeviceTemplateViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly DeviceTemplate _model;

        public DeviceTemplate Model => _model;

        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height == value) return;
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }


        public TextBox WidthTextBox { get; set; }
        public TextBox HeightTextBox { get; set; }

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

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if (_count == value) return;
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public DeviceTemplateViewModel Clone()
        {
            var cloneModel = _model.Clone();
            return new DeviceTemplateViewModel(cloneModel)
            {
                Width = Width,
                Height = Height,
                Count = Count
            };
        }

        public DeviceTemplateViewModel(DeviceTemplate model)
        {
            _model = model;
            Width = model.Width;
            Height = model.Height;
            Count = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Обновляем значения ширины и высоты в связанных текстовых полях
            if (propertyName == nameof(Width) && WidthTextBox != null)
                WidthTextBox.Text = Width.ToString();
            if (propertyName == nameof(Height) && HeightTextBox != null)
                HeightTextBox.Text = Height.ToString();
        }
    }
}
