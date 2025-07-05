using SetlistFm.Client;
using SetlistPlaylistCreator.Domain;

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

            foreach (var setlistSearchResult in result.Setlist)
            {
                var setlist = new Setlist
                {
                    ArtistName = setlistSearchResult?.Artist?.Name ?? "",
                    Name = $"{setlistSearchResult?.Artist?.Name} in {setlistSearchResult?.Venue?.Name}"
                };

                var sets = new List<SetlistSet>();

                foreach (var setSearchResult in setlistSearchResult?.Sets?.Set ?? Enumerable.Empty<SetlistFmSet>())
                {
                    if (setSearchResult is null)
                    {
                        continue;
                    }

                    var set = new SetlistSet
                    {
                        Name = setSearchResult?.Name ?? string.Empty
                    };

                    var songList = new List<SetlistSong>();

                    foreach (var songSearchResult in setSearchResult?.Song ?? Enumerable.Empty<SetlistFmSong>())
                    {
                        var song = new SetlistSong();
                        song.Name = songSearchResult?.Name ?? string.Empty;

                        if (songSearchResult?.Cover is not null)
                        {
                            song.ArtistName = songSearchResult.Cover?.Name ?? string.Empty;
                        }
                        else
                        {
                            song.ArtistName = setlistSearchResult?.Artist?.Name ?? string.Empty;
                        }

                        songList.Add(song);
                    }

                    set.Songs = songList;

                    sets.Add(set);
                }
                setlist.Sets = sets;
                setlists.Add(setlist);
            }

            return setlists;
        }
    }
}