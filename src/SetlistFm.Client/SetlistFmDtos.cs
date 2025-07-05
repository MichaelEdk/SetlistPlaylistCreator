namespace SetlistFm.Client
{
    /// <summary>
    /// Represents an artist in the Setlist.fm API.
    /// </summary>
    public class SetlistFmArtist
    {
        /// <summary>
        /// Gets or sets the MusicBrainz Identifier (MBID) of the artist.
        /// </summary>
        public string? Mbid { get; set; }

        /// <summary>
        /// Gets or sets the name of the artist.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the sort name of the artist.
        /// </summary>
        public string? SortName { get; set; }

        /// <summary>
        /// Gets or sets the disambiguation comment for the artist.
        /// </summary>
        public string? Disambiguation { get; set; }

        /// <summary>
        /// Gets or sets the URL to the artist's Setlist.fm page.
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// Represents a city in the Setlist.fm API.
    /// </summary>
    public class SetlistFmCity
    {
        /// <summary>
        /// Gets or sets the identifier of the city.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the state of the city.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the state code of the city.
        /// </summary>
        public string? StateCode { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of the city.
        /// </summary>
        public SetlistFmCoordinates? Coords { get; set; }

        /// <summary>
        /// Gets or sets the country of the city.
        /// </summary>
        public SetlistFmCountry? Country { get; set; }
    }

    /// <summary>
    /// Represents geographical coordinates.
    /// </summary>
    public class SetlistFmCoordinates
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Long { get; set; }
    }

    /// <summary>
    /// Represents a country in the Setlist.fm API.
    /// </summary>
    public class SetlistFmCountry
    {
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// Represents a venue in the Setlist.fm API.
    /// </summary>
    public class SetlistFmVenue
    {
        /// <summary>
        /// Gets or sets the identifier of the venue.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the city where the venue is located.
        /// </summary>
        public SetlistFmCity? City { get; set; }

        /// <summary>
        /// Gets or sets the URL to the venue's Setlist.fm page.
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// Represents a tour in the Setlist.fm API.
    /// </summary>
    public class SetlistFmTour
    {
        /// <summary>
        /// Gets or sets the name of the tour.
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// Represents a cover artist for a song.
    /// </summary>
    public class SetlistFmCover
    {
        /// <summary>
        /// Gets or sets the MusicBrainz Identifier (MBID) of the cover artist.
        /// </summary>
        public string? Mbid { get; set; }

        /// <summary>
        /// Gets or sets the name of the cover artist.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the sort name of the cover artist.
        /// </summary>
        public string? SortName { get; set; }

        /// <summary>
        /// Gets or sets the disambiguation comment for the cover artist.
        /// </summary>
        public string? Disambiguation { get; set; }

        /// <summary>
        /// Gets or sets the URL to the cover artist's Setlist.fm page.
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// Represents a song in a setlist.
    /// </summary>
    public class SetlistFmSong
    {
        /// <summary>
        /// Gets or sets the name of the song.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the cover artist for the song, if applicable.
        /// </summary>
        public SetlistFmCover? Cover { get; set; }

        /// <summary>
        /// Gets or sets additional information about the song.
        /// </summary>
        public string? Info { get; set; }
    }

    /// <summary>
    /// Represents a collection of sets in a setlist.
    /// </summary>
    public class SetlistFmSets
    {
        /// <summary>
        /// Gets or sets the list of sets.
        /// </summary>
        public List<SetlistFmSet> Set { get; set; } = new();
    }

    /// <summary>
    /// Represents a set in a setlist, which may contain multiple songs.
    /// </summary>
    public class SetlistFmSet
    {
        /// <summary>
        /// Gets or sets the encore number, if the set is an encore.
        /// </summary>
        public int? Encore { get; set; }

        /// <summary>
        /// Gets or sets the list of songs in the set.
        /// </summary>
        /// <remarks>The name of the collection in the payload is singular.</remarks>
        public List<SetlistFmSong> Song { get; set; } = new();

        /// <summary>
        /// Gets or sets the name of the set.
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// Represents a setlist in the Setlist.fm API.
    /// </summary>
    public class SetlistFmSetlist
    {
        /// <summary>
        /// Gets or sets the identifier of the setlist.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the version identifier of the setlist.
        /// </summary>
        public string? VersionId { get; set; }

        /// <summary>
        /// Gets or sets the event date of the setlist.
        /// </summary>
        public string? EventDate { get; set; }

        /// <summary>
        /// Gets or sets the last updated date of the setlist.
        /// </summary>
        public string? LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the artist for the setlist.
        /// </summary>
        public SetlistFmArtist? Artist { get; set; }

        /// <summary>
        /// Gets or sets the venue for the setlist.
        /// </summary>
        public SetlistFmVenue? Venue { get; set; }

        /// <summary>
        /// Gets or sets the tour for the setlist.
        /// </summary>
        public SetlistFmTour? Tour { get; set; }

        /// <summary>
        /// Gets or sets the sets in the setlist.
        /// </summary>
        public SetlistFmSets? Sets { get; set; }

        /// <summary>
        /// Gets or sets additional information about the setlist.
        /// </summary>
        public string? Info { get; set; }

        /// <summary>
        /// Gets or sets the URL to the setlist's Setlist.fm page.
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// Represents the result of a setlist search in the Setlist.fm API.
    /// </summary>
    public class SetlistFmSetlistSearchResult
    {
        /// <summary>
        /// Gets or sets the type of the search result.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Gets or sets the total number of results.
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// Gets or sets the list of setlists returned by the search.
        /// </summary>
        public List<SetlistFmSetlist> Setlist { get; set; } = new();
    }
}