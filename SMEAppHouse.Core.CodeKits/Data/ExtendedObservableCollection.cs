using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SMEAppHouse.Core.CodeKits.Data
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public ExtendedObservableCollection()
            : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ExtendedObservableCollection_CollectionChanged);
        }

        void ExtendedObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }

    }
}
