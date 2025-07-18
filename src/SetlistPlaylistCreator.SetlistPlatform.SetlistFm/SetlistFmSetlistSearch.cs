using SetlistFm.Client;
using SetlistPlaylistCreator.Domain;
using System.Text;

namespace SetlistPlaylistCreator.SetlistPlatform.SetlistFm
{
    /// <summary>
    /// A SetlistFm-specific implementation of <see cref="ISetlistSearch"/>.
    /// </summary>
    public class SetlistFmSetlistSearch : ISetlistSearch
    {
        private readonly ISetlistSearchClient _setlistSearchClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetlistFmSetlistSearch"/> class.
        /// </summary>
        /// <param name="setlistSearchClient">A client used to search setlists.</param>
        public SetlistFmSetlistSearch(ISetlistSearchClient setlistSearchClient)
        {
            ArgumentNullException.ThrowIfNull(setlistSearchClient, nameof(setlistSearchClient));

            _setlistSearchClient = setlistSearchClient;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<Setlist>> SearchForSetlistsAsync(string artist, CancellationToken cancellationToken)
        {
            var result = await _setlistSearchClient.SearchSetlistsAsync(artist, cancellationToken).ConfigureAwait(false);

            var setlists = new List<Setlist>();

            // Iterate through all of the setlists returned from SetlistFM.
            foreach (var setlistSearchResult in result.Setlist.Where(setlist => setlist is not null))
            {
                var sets = new List<SetlistSet>();

                // The Sets property is an object containing a list of sets called Set. This is just based on the JSON payload structure.
                foreach (var setSearchResult in setlistSearchResult.Sets.Set.Where(set => set is not null))
                {
                    var songList = new List<SetlistSong>();

                    foreach (var songSearchResult in setSearchResult.Song)
                    {
                        // If the song's a cover, the arist name should be taken from the Cover property. Otherwise, use the name of the setlist's artist.
                        var songArtistName = songSearchResult.Cover?.Name ?? setlistSearchResult.Artist.Name;
                        var song = new SetlistSong(
                            Name: songSearchResult.Name ?? string.Empty,
                            ArtistName: songArtistName ?? string.Empty
                        );

                        songList.Add(song);
                    }

                    // In practice, the name of the set is often not populated.
                    var set = new SetlistSet(
                        Name: setSearchResult.Name ?? string.Empty,
                        Songs: songList
                    );

                    sets.Add(set);
                }

                var setlistArtistName = setlistSearchResult.Artist.Name ?? string.Empty;
                var setlistNameBuilder = new StringBuilder();
                setlistNameBuilder.Append(setlistArtistName);
                if (!string.IsNullOrEmpty(setlistSearchResult.Venue.Name))
                {
                    setlistNameBuilder.Append($" in {setlistSearchResult.Venue.Name}");
                }

                var setlist = new Setlist(
                    Name: setlistNameBuilder.ToString(),
                    ArtistName: setlistArtistName,
                    Sets: sets
                );

                setlists.Add(setlist);
            }

            return setlists;
        }
    }
}