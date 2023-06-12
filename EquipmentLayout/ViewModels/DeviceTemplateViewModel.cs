using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using EquipmentLayout.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquipmentLayout.ViewModels
{
    public class DeviceTemplateViewModel : BaseViewModel
    {
        private DeviceTemplate _model;

        private DeviceFactory _factory;

        public DelegateCommand OpenTemplateEditorCommand { get; }

        public DeviceTemplate Model => _model;


        private void OpenTemplateEditorCommand_Executed()
        {
            var editor = new TemplateEditorWindow();
            var vm = new TemplateEditorViewModel(this);
            editor.DataContext = vm;
            editor.Show();
        }

        public int Width 
        { 
            get => _model.Width;
            set
            {
                if (_model.Width == value) return; else _model.Width = value; OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _model.Height;
            set
            {
                if (_model.Height == value) return; else _model.Height = value; OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name == value) return; else _model.Name = value; OnPropertyChanged();
            }
        }

        public int Count
        {
            get => _model.Count;
            set
            {
                if (_model.Count == value) return; else _model.Count = value; OnPropertyChanged();
            }
        }


        public AreaViewModel WorkArea { get; set; }

        public AreaViewModel ServiceArea { get; set; }

        public Device GetDevice(int x, int y)
        {
            return _factory.GetDevice(new Point(x, y), this._model, false);
        }

        public DeviceTemplateViewModel Clone()
        {
            var cloneModel = this._model.Clone();
            return new DeviceTemplateViewModel(cloneModel);
        }


        public DeviceTemplateViewModel(DeviceTemplate model)
        {
            _model = model;
            OpenTemplateEditorCommand = new DelegateCommand(OpenTemplateEditorCommand_Executed);
            _factory = new DeviceFactory();
            WorkArea = new AreaViewModel(_model.WorkArea);
            ServiceArea = new AreaViewModel(_model.ServiceArea);
        }

        public DeviceTemplateViewModel()
        {
            _model = new DeviceTemplate();
            OpenTemplateEditorCommand = new DelegateCommand(OpenTemplateEditorCommand_Executed);
            _factory = new DeviceFactory();
            WorkArea = new AreaViewModel(_model.WorkArea);
            ServiceArea = new AreaViewModel(_model.ServiceArea);
        }

    }
}
