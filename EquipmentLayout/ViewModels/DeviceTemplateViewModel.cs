using EquipmentLayout.Infrastructure;
using EquipmentLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.Sat;

namespace EquipmentLayout.ViewModels
{
    public class DeviceTemplateViewModel : BaseViewModel
    {
        private readonly DeviceTemplate _model;


        public DeviceTemplate Model { get { return _model; } }



        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
