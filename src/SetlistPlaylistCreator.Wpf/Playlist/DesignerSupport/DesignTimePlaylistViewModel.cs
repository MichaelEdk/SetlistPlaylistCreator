using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;

namespace SetlistPlaylistCreator.Wpf.Playlist.DesignerSupport
{
    /// <summary>
    /// A design-time view model for the <see cref="PlaylistViewModel"/> class.
    /// </summary>
    internal class DesignTimePlaylistViewModel
        : PlaylistViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimePlaylistViewModel"/> class with sample data."/>
        /// </summary>
        public DesignTimePlaylistViewModel()
            : base(NullSetlistPlaylistCreatorService.Instance)
        {
            // Sample data for design time
            Playlist =
            [
                new(new("Hey Jude", ["The Beatles"], "id1"), [new("Hey Dude", ["The Boatles"], "id6")], "Hey Jude", "The Beatles"),
                new(new("Let It Be", ["The Beatles"], "id2"),[new("Hey Dude", ["The Boatles"], "id6")], "Let It Be", "The Beatles"),
                new(new("Come Together", ["Led Zeppelin"], "id3"),[new("Hey Dude", ["The Boatles"], "id6")], "Come Together", "Led Zeppelin"),
                new(new("Twist and Shout", ["Isley Brothers"], "id4"),[new("Hey Dude", ["The Boatles"], "id6")], "Twist and Shout", "Isley Brothers"),
                new(new("Yesterday", ["The Beatles"], "id5"),[new("Hey Dude", ["The Boatles"], "id6")], "Yesterday", "The Beatles")
            ];
            PlaylistRowViewModels = Playlist
                .Select(playlist =>
                    new PlaylistRowViewModel
                    {
                         Id = Guid.NewGuid(),
                         SearchedSong = playlist,
                         SelectedSong = playlist.FirstMatch,
                         SetlistArtist = playlist.SearchedArtistName,
                         SetlistSongName = playlist.SearchedSongName,
                         IsPopupVisible = true
                    })
                .ToList();
        }

        private class NullSetlistPlaylistCreatorService
            : ISetlistPlaylistCreatorService
        {
            /// <summary>
            /// A static instance of <see cref="NullSetlistPlaylistCreatorService"/> for design time support.
            /// </summary>
            public static NullSetlistPlaylistCreatorService Instance { get; } = new();

            public Task<bool> CreatePlaylistAsync(string playlistName, IReadOnlyCollection<StreamingPlatformSong> songs)
            {
                // This is a no-op for design time support.
                return Task.FromResult(true);
            }

            public Task<IReadOnlyCollection<SearchedSong>> ProposePlaylistAsync(Setlist setlist)
            {
                // This is a no-op for design time support.
                return Task.FromResult<IReadOnlyCollection<SearchedSong>>([]);
            }
        }
    }
}
