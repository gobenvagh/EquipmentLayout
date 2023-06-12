using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentLayout.Infrastructure
{
    public interface IProperty
    {
        string Name { get; }
        object Value { get; set; }
    }
 
    public class Property<T> : IProperty
    {
        private T _model;
        public string Name { get; }
        public object Value
        {
            get => _getter(_model);
            set
            {
                if (_setter != null) _setter(_model, value);
            }
        }

        private Action<T, object> _setter;
        private Func<T, object> _getter;

        public Property(string name, object value, T model, Action<T, object> setter, Func<T, object> getter)
        {
            this._model = model;
            this._setter = setter;
            this._getter = getter;
            Name = name;
            Value = value;
        }

        public Property(string name, object value, T model, Func<T, object> getter)
        {
            this._model = model;
            this._getter = getter;
            Name = name;
            Value = value;
        }

    }
}
