using System.ComponentModel;

namespace SetlistPlaylistCreator.Wpf
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaiseAndSetIfChanged<T>(ref T existingObject, T newValue, string propertyName)
        {
            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(existingObject, newValue))
            {
                return;
            }

            existingObject = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
