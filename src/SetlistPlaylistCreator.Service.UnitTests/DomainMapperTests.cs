using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.StreamingPlatform;

namespace SetlistPlaylistCreator.Service.UnitTests
{
    [TestClass]
    public class DomainMapperTests
    {
        [TestMethod]
        public void Map_StreamingPlatformSongToSong_ReturnsCorrectMapping()
        {
            // Arrange
            var streamingPlatformSongs = new List<StreamingPlatformSong>
            {
                new("Hey Jude", ["The Beatles"], "spotify:track:1"),
                new("Imagine", ["John Lennon"], "spotify:track:2"),
                new("Come Together", ["The Beatles", "John Lennon"], "spotify:track:3")
            };

            // Act
            var result = DomainMapper.Map(streamingPlatformSongs);

            // Assert
            Assert.AreEqual(3, result.Count);
            
            var firstSong = result.First();
            Assert.AreEqual("Hey Jude", firstSong.Title);
            Assert.AreEqual("spotify:track:1", firstSong.Id);
            Assert.AreEqual(1, firstSong.Artists.Count);
            Assert.AreEqual("The Beatles", firstSong.Artists.First().Name);

            var thirdSong = result.Skip(2).First();
            Assert.AreEqual("Come Together", thirdSong.Title);
            Assert.AreEqual("spotify:track:3", thirdSong.Id);
            Assert.AreEqual(2, thirdSong.Artists.Count);
            Assert.IsTrue(thirdSong.Artists.Any(a => a.Name == "The Beatles"));
            Assert.IsTrue(thirdSong.Artists.Any(a => a.Name == "John Lennon"));
        }

        [TestMethod]
        public void Map_SongToStreamingPlatformSong_ReturnsCorrectMapping()
        {
            // Arrange
            var songs = new List<Song>
            {
                new("Yesterday", "spotify:track:4", [new("The Beatles")]),
                new("Let It Be", "spotify:track:5", [new("The Beatles"), new("Paul McCartney")])
            };

            // Act
            var result = DomainMapper.Map(songs);

            // Assert
            Assert.AreEqual(2, result.Count);

            var firstSong = result.First();
            Assert.AreEqual("Yesterday", firstSong.SongName);
            Assert.AreEqual("spotify:track:4", firstSong.Id);
            Assert.AreEqual(1, firstSong.ArtistNames.Count);
            Assert.AreEqual("The Beatles", firstSong.ArtistNames.First());

            var secondSong = result.Skip(1).First();
            Assert.AreEqual("Let It Be", secondSong.SongName);
            Assert.AreEqual("spotify:track:5", secondSong.Id);
            Assert.AreEqual(2, secondSong.ArtistNames.Count);
            Assert.IsTrue(secondSong.ArtistNames.Contains("The Beatles"));
            Assert.IsTrue(secondSong.ArtistNames.Contains("Paul McCartney"));
        }

        [TestMethod]
        public void Map_ArtistNamesToArtists_ReturnsCorrectMapping()
        {
            // Arrange
            var artistNames = new List<string> { "The Beatles", "John Lennon", "Paul McCartney" };

            // Act
            var result = DomainMapper.Map(artistNames);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(a => a.Name == "The Beatles"));
            Assert.IsTrue(result.Any(a => a.Name == "John Lennon"));
            Assert.IsTrue(result.Any(a => a.Name == "Paul McCartney"));
        }

        [TestMethod]
        public void Map_EmptyStreamingPlatformSongCollection_ReturnsEmptyCollection()
        {
            // Arrange
            var streamingPlatformSongs = new List<StreamingPlatformSong>();

            // Act
            var result = DomainMapper.Map(streamingPlatformSongs);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Map_EmptySongCollection_ReturnsEmptyCollection()
        {
            // Arrange
            var songs = new List<Song>();

            // Act
            var result = DomainMapper.Map(songs);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Map_EmptyArtistNamesCollection_ReturnsEmptyCollection()
        {
            // Arrange
            var artistNames = new List<string>();

            // Act
            var result = DomainMapper.Map(artistNames);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Map_StreamingPlatformSongWithEmptyArtistNames_CreatesEmptyArtistCollection()
        {
            // Arrange
            var streamingPlatformSongs = new List<StreamingPlatformSong>
            {
                new("Instrumental Track", [], "spotify:track:instrumental")
            };

            // Act
            var result = DomainMapper.Map(streamingPlatformSongs);

            // Assert
            Assert.AreEqual(1, result.Count);
            var song = result.First();
            Assert.AreEqual("Instrumental Track", song.Title);
            Assert.AreEqual("spotify:track:instrumental", song.Id);
            Assert.AreEqual(0, song.Artists.Count);
        }

        [TestMethod]
        public void Map_SongWithEmptyArtists_CreatesEmptyArtistNamesCollection()
        {
            // Arrange
            var songs = new List<Song>
            {
                new("Instrumental Track", "spotify:track:instrumental", [])
            };

            // Act
            var result = DomainMapper.Map(songs);

            // Assert
            Assert.AreEqual(1, result.Count);
            var song = result.First();
            Assert.AreEqual("Instrumental Track", song.SongName);
            Assert.AreEqual("spotify:track:instrumental", song.Id);
            Assert.AreEqual(0, song.ArtistNames.Count);
        }
    }
}
