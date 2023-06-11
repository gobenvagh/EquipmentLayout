using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EquipmentLayout.Infrastructure
{
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
            switch (e.Action){
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach(T item in e.NewItems.OfType<T>())
                            _target.Add(item); 
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (T item in e.NewItems.OfType<T>())
                            _target.Remove(item);
                        break;
                    }
            }
        }
    }
}
