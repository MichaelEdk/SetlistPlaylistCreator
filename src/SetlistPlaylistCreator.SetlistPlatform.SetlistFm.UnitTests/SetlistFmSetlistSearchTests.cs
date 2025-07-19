using SetlistFm.Client;

namespace SetlistPlaylistCreator.SetlistPlatform.SetlistFm.UnitTests
{
    [TestClass]
    public class SetlistFmSetlistSearchTests
    {
        [TestMethod]
        public void Constructor_NullSetlistSearchClient_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
                new SetlistFmSetlistSearch(null!));
            Assert.AreEqual("setlistSearchClient", exception.ParamName);
        }

        [TestMethod]
        public void Constructor_ValidSetlistSearchClient_CreatesInstance()
        {
            // Act
            var result = CreateSetlistFmSetlistSearch();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_CancellationTokenPassed_PassesToClient()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var cancellationToken = new CancellationToken();
            var searchResult = new SetlistFmSetlistSearchResult { Setlist = [] };

            mockClient.SearchSetlistsAsync("Test Artist", cancellationToken)
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            await search.SearchForSetlistsAsync("Test Artist", cancellationToken);

            // Assert
            await mockClient.Received(1).SearchSetlistsAsync("Test Artist", cancellationToken);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_ClientThrowsException_PropagatesException()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var expectedException = new InvalidOperationException("Client error");

            mockClient.SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromException<SetlistFmSetlistSearchResult>(expectedException));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act & Assert
            var actualException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => search.SearchForSetlistsAsync("Test Artist", CancellationToken.None));

            Assert.AreEqual(expectedException, actualException);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_EmptyResult_ReturnsEmptyCollection()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var emptyResult = new SetlistFmSetlistSearchResult
            {
                Setlist = []
            };
            mockClient.SearchSetlistsAsync("artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(emptyResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("artist", CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_MixedCoverAndNonCoverSongs_HandlesCorrectly()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [
                                        new SetlistFmSong { Name = "Original Song" }, // No cover
                                        new SetlistFmSong
                                        {
                                            Name = "Covered Song",
                                            Cover = new SetlistFmCover { Name = "Original Artist" }
                                        },
                                        new SetlistFmSong
                                        {
                                            Name = "Another Cover",
                                            Cover = new SetlistFmCover { Name = null } // Null cover name
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient.SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            var songs = result.First().Sets.First().Songs.ToList();
            Assert.AreEqual(3, songs.Count);

            // Original song should use setlist artist
            Assert.AreEqual("Original Song", songs[0].Name);
            Assert.AreEqual("Test Artist", songs[0].ArtistName);

            // Covered song should use cover artist
            Assert.AreEqual("Covered Song", songs[1].Name);
            Assert.AreEqual("Original Artist", songs[1].ArtistName);

            // Cover with null name should fall back to setlist artist
            Assert.AreEqual("Another Cover", songs[2].Name);
            Assert.AreEqual("Test Artist", songs[2].ArtistName);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_MultipleSetlists_ReturnsAllSetlists()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Venue 1" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [new SetlistFmSong { Name = "Song 1" }]
                                }
                            ]
                        }
                    },
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Venue 2" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [new SetlistFmSong { Name = "Song 2" }]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient.SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var setlists = result.ToList();
            Assert.AreEqual("Test Artist in Venue 1", setlists[0].Name);
            Assert.AreEqual("Test Artist in Venue 2", setlists[1].Name);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_MultipleSetsAndSongs_ReturnsCorrectlyStructuredData()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [
                                        new SetlistFmSong { Name = "Song 1" },
                                        new SetlistFmSong { Name = "Song 2" }
                                    ]
                                },
                                new SetlistFmSet
                                {
                                    Name = "Encore",
                                    Song = [
                                        new SetlistFmSong { Name = "Encore Song" }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var setlist = result.First();
            Assert.AreEqual(2, setlist.Sets.Count);

            var sets = setlist.Sets.ToList();
            Assert.AreEqual("Set 1", sets[0].Name);
            Assert.AreEqual(2, sets[0].Songs.Count);

            Assert.AreEqual("Encore", sets[1].Name);
            Assert.AreEqual(1, sets[1].Songs.Count);
            Assert.AreEqual("Encore Song", sets[1].Songs.First().Name);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_NullSetlistsInResult_FiltersOutNullSetlists()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [new SetlistFmSong { Name = "Song 1" }]
                                }
                            ]
                        }
                    },
                    null! // Null setlist should be filtered out
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Artist in Test Venue", result.First().Name);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_NullSetsInSetlist_FiltersOutNullSets()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [new SetlistFmSong { Name = "Song 1" }]
                                },
                                null! // Null set should be filtered out
                            ]
                        }
                    }
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var setlist = result.First();
            Assert.AreEqual(1, setlist.Sets.Count);
            Assert.AreEqual("Set 1", setlist.Sets.First().Name);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_NullValues_HandlesNullsGracefully()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = null },
                        Venue = new SetlistFmVenue { Name = null },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = null,
                                    Song = [
                                        new SetlistFmSong { Name = null }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var setlist = result.First();
            Assert.AreEqual(string.Empty, setlist.Name);
            Assert.AreEqual(string.Empty, setlist.ArtistName);

            var set = setlist.Sets.First();
            Assert.AreEqual(string.Empty, set.Name);

            var song = set.Songs.First();
            Assert.AreEqual(string.Empty, song.Name);
            Assert.AreEqual(string.Empty, song.ArtistName);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_SongWithCover_UsesCoverArtistName()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [
                                        new SetlistFmSong
                                        {
                                            Name = "Covered Song",
                                            Cover = new SetlistFmCover { Name = "Original Artist" }
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            var song = result.First().Sets.First().Songs.First();
            Assert.AreEqual("Covered Song", song.Name);
            Assert.AreEqual("Original Artist", song.ArtistName);
        }

        [TestMethod]
        public async Task SearchForSetlistsAsync_ValidSetlistWithSongs_ReturnsCorrectlyMappedSetlist()
        {
            // Arrange
            var mockClient = Substitute.For<ISetlistSearchClient>();
            var searchResult = new SetlistFmSetlistSearchResult
            {
                Setlist = [
                    new SetlistFmSetlist
                    {
                        Artist = new SetlistFmArtist { Name = "Test Artist" },
                        Venue = new SetlistFmVenue { Name = "Test Venue" },
                        Sets = new SetlistFmSets
                        {
                            Set = [
                                new SetlistFmSet
                                {
                                    Name = "Set 1",
                                    Song = [
                                        new SetlistFmSong { Name = "Song 1" },
                                        new SetlistFmSong { Name = "Song 2" }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            mockClient
                .SearchSetlistsAsync("Test Artist", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(searchResult));

            var search = CreateSetlistFmSetlistSearch(mockClient);

            // Act
            var result = await search.SearchForSetlistsAsync("Test Artist", CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var setlist = result.First();
            Assert.AreEqual("Test Artist in Test Venue", setlist.Name);
            Assert.AreEqual("Test Artist", setlist.ArtistName);
            Assert.AreEqual(1, setlist.Sets.Count);

            var set = setlist.Sets.First();
            Assert.AreEqual("Set 1", set.Name);
            Assert.AreEqual(2, set.Songs.Count);

            var songs = set.Songs.ToList();
            Assert.AreEqual("Song 1", songs[0].Name);
            Assert.AreEqual("Test Artist", songs[0].ArtistName);
            Assert.AreEqual("Song 2", songs[1].Name);
            Assert.AreEqual("Test Artist", songs[1].ArtistName);
        }

        private static SetlistFmSetlistSearch CreateSetlistFmSetlistSearch(
            ISetlistSearchClient? setlistSearchClient = null)
        {
            return new(
                setlistSearchClient ?? Substitute.For<ISetlistSearchClient>()
            );
        }
    }
}
