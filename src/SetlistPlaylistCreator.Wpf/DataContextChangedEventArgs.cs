namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// Arguments passed into the event when the main window's view model is changed.
    /// </summary>
    public class DataContextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new data context to use.
        /// </summary>
        public object NewDataContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newDataContext">The new data context to use.</param>
        public DataContextChangedEventArgs(object newDataContext)
        {
            ArgumentNullException.ThrowIfNull(newDataContext, nameof(newDataContext));

            NewDataContext = newDataContext;
        }
    }
}
