using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.ViewModels
{
    public class DeviceTemplateViewModel : BaseViewModel
    {
        public DeviceTemplate _model;

        public DeviceTemplate Model => _model;


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

        public DeviceTemplateViewModel Clone()
        {
            var cloneModel = this._model.Clone();
            return new DeviceTemplateViewModel(cloneModel);
        }


        public DeviceTemplateViewModel(DeviceTemplate model)
        {
            _model = model;
        }
    }
}
