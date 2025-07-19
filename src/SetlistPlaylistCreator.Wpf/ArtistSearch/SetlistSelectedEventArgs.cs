using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.Wpf.ArtistSearch
{
    /// <summary>
    /// Event arguments for the SetlistSelected event.
    /// </summary>
    public class SetlistSelectedEventArgs
        : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetlistSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="selectedSetlist">The setlist selected.</param>
        public SetlistSelectedEventArgs(Setlist selectedSetlist)
        {
            ArgumentNullException.ThrowIfNull(selectedSetlist, nameof(selectedSetlist));

            SelectedSetlist = selectedSetlist;
        }

        /// <summary>
        /// Gets the selected setlist.
        /// </summary>
        public Setlist SelectedSetlist { get; }
    }
}
