using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using SetlistPlaylistCreator.Wpf.Playlist;

namespace SetlistPlaylistCreator.Wpf.UnitTests
{
    [TestClass]
    public class PlaylistViewModelTests
    {
        [TestMethod]
        public void Constructor_NullSetlistPlaylistCreatorService_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
                new PlaylistViewModel(null!));
            Assert.AreEqual("setlistPlaylistCreatorService", exception.ParamName);
        }

        [TestMethod]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange & Act
            var viewModel = CreatePlaylistViewModel();

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.Playlist);
            Assert.IsNotNull(viewModel.PlaylistRowViewModels);
            Assert.AreEqual(0, viewModel.Playlist.Count);
            Assert.AreEqual(0, viewModel.PlaylistRowViewModels.Count);
        }

        [TestMethod]
        public void Playlist_SetValue_UpdatesProperty()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();
            var firstMatch = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var testPlaylist = new List<SearchedSong>
            {
                new(firstMatch, [firstMatch], "Song 1", "Artist 1")
            };

            // Act
            viewModel.Playlist = testPlaylist;

            // Assert
            Assert.AreSame(testPlaylist, viewModel.Playlist);
        }

        [TestMethod]
        public void Playlist_SetValue_RaisesPropertyChanged()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();
            var propertyName = string.Empty;
            viewModel.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

            // Act
            viewModel.Playlist = [];

            // Assert
            Assert.AreEqual(nameof(viewModel.Playlist), propertyName);
        }

        [TestMethod]
        public void PlaylistRowViewModels_SetValue_UpdatesProperty()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();
            var testRowViewModels = new List<PlaylistRowViewModel>
            {
                new() { Id = Guid.NewGuid() }
            };

            // Act
            viewModel.PlaylistRowViewModels = testRowViewModels;

            // Assert
            Assert.AreSame(testRowViewModels, viewModel.PlaylistRowViewModels);
        }

        [TestMethod]
        public void PlaylistRowViewModels_SetValue_RaisesPropertyChanged()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();
            var propertyName = string.Empty;
            viewModel.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

            // Act
            viewModel.PlaylistRowViewModels = [];

            // Assert
            Assert.AreEqual("_playlistRowViewModels", propertyName);
        }

        [TestMethod]
        public async Task PopulateSetlistAsync_NullSetlist_ThrowsArgumentNullException()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                viewModel.PopulateSetlistAsync(null!));
            Assert.AreEqual("setlist", exception.ParamName);
        }

        [TestMethod]
        public async Task PopulateSetlistAsync_ValidSetlist_CallsServiceProposePlaylistAsync()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var firstMatch = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var searchedSongs = new List<SearchedSong>
            {
                new(firstMatch, [firstMatch], "Song 1", "Artist 1")
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);

            var viewModel = CreatePlaylistViewModel(mockService);

            // Act
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Assert
            await mockService.Received(1).ProposePlaylistAsync(testSetlist);
        }

        [TestMethod]
        public async Task PopulateSetlistAsync_ValidSetlist_PopulatesPlaylistRowViewModels()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var song1 = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var song2 = new StreamingPlatformSong("Song 2", ["Artist 2"], "id2");
            var searchedSongs = new List<SearchedSong>
            {
                new(song1, [song1], "Song 1", "Artist 1"),
                new(song2, [song2], "Song 2", "Artist 2")
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);

            var viewModel = CreatePlaylistViewModel(mockService);

            // Act
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Assert
            Assert.AreEqual(2, viewModel.PlaylistRowViewModels.Count);
            
            var row1 = viewModel.PlaylistRowViewModels[0];
            Assert.AreSame(searchedSongs[0], row1.SearchedSong);
            Assert.AreSame(song1, row1.SelectedSong);
            Assert.AreEqual("Artist 1", row1.SetlistArtist);
            Assert.AreEqual("Song 1", row1.SetlistSongName);
            Assert.AreNotEqual(Guid.Empty, row1.Id);

            var row2 = viewModel.PlaylistRowViewModels[1];
            Assert.AreSame(searchedSongs[1], row2.SearchedSong);
            Assert.AreSame(song2, row2.SelectedSong);
            Assert.AreEqual("Artist 2", row2.SetlistArtist);
            Assert.AreEqual("Song 2", row2.SetlistSongName);
            Assert.AreNotEqual(Guid.Empty, row2.Id);
        }

        [TestMethod]
        public async Task PopulateSetlistAsync_SearchedSongWithNoMatches_CreatesRowViewModelWithFirstMatchAsSelected()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var firstMatch = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var searchedSongs = new List<SearchedSong>
            {
                new(firstMatch, [], "Song 1", "Artist 1") // Empty matches collection
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);

            var viewModel = CreatePlaylistViewModel(mockService);

            // Act
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Assert
            Assert.AreEqual(1, viewModel.PlaylistRowViewModels.Count);
            var row = viewModel.PlaylistRowViewModels[0];
            Assert.AreSame(firstMatch, row.SelectedSong);
            Assert.AreEqual("Artist 1", row.SetlistArtist);
            Assert.AreEqual("Song 1", row.SetlistSongName);
        }

        [TestMethod]
        public void CreatePlaylist_NoSelectedSongs_CannotExecute()
        {
            // Arrange
            var viewModel = CreatePlaylistViewModel();

            // Act
            var canExecute = viewModel.CreatePlaylist.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public async Task CreatePlaylist_WithSelectedSongs_CanExecute()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var song1 = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var searchedSongs = new List<SearchedSong>
            {
                new(song1, [song1], "Song 1", "Artist 1")
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);
            mockService.CreatePlaylistAsync(Arg.Any<string>(), Arg.Any<IReadOnlyCollection<StreamingPlatformSong>>()).Returns(Task.FromResult(true));

            var viewModel = CreatePlaylistViewModel(mockService);
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Act
            var canExecute = viewModel.CreatePlaylist.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public async Task CreatePlaylist_Execute_CallsServiceWithSelectedSongs()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var song1 = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var song2 = new StreamingPlatformSong("Song 2", ["Artist 2"], "id2");
            var searchedSongs = new List<SearchedSong>
            {
                new(song1, [song1], "Song 1", "Artist 1"),
                new(song2, [song2], "Song 2", "Artist 2")
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);
            mockService.CreatePlaylistAsync(Arg.Any<string>(), Arg.Any<IReadOnlyCollection<StreamingPlatformSong>>()).Returns(Task.FromResult(true));

            var viewModel = CreatePlaylistViewModel(mockService);
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Act
            viewModel.CreatePlaylist.Execute(null);
            
            // Allow async execution to complete
            await Task.Delay(100);

            // Assert
            await mockService.Received(1).CreatePlaylistAsync(
                "Test Setlist",
                Arg.Is<IReadOnlyCollection<StreamingPlatformSong>>(songs => songs.Count == 2));
        }

        [TestMethod]
        public async Task CreatePlaylist_WithSomeNullSelectedSongs_CallsServiceWithOnlyNonNullSongs()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var song1 = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var firstMatch = new StreamingPlatformSong("First Match", ["Artist"], "first-id");
            var searchedSongs = new List<SearchedSong>
            {
                new(song1, [song1], "Song 1", "Artist 1"),
                new(firstMatch, [], "Song 2", "Artist 2") // No matches - will use first match as placeholder
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);
            mockService.CreatePlaylistAsync(Arg.Any<string>(), Arg.Any<IReadOnlyCollection<StreamingPlatformSong>>()).Returns(Task.FromResult(true));

            var viewModel = CreatePlaylistViewModel(mockService);
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Manually set one song to null to simulate user deselection
            viewModel.PlaylistRowViewModels[1].SelectedSong = null;

            // Act
            viewModel.CreatePlaylist.Execute(null);
            
            // Allow async execution to complete
            await Task.Delay(100);

            // Assert
            await mockService.Received(1).CreatePlaylistAsync(
                "Test Setlist",
                Arg.Is<IReadOnlyCollection<StreamingPlatformSong>>(songs => songs.Count == 1));
        }

        [TestMethod]
        public async Task CreatePlaylist_WithManuallyChangedSelectedSongs_CallsServiceWithUpdatedSongs()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var song1 = new StreamingPlatformSong("Song 1", ["Artist 1"], "id1");
            var alternativeSong = new StreamingPlatformSong("Alternative Song", ["Artist 1"], "id-alt");
            var searchedSongs = new List<SearchedSong>
            {
                new(song1, [song1, alternativeSong], "Song 1", "Artist 1")
            };

            mockService.ProposePlaylistAsync(testSetlist).Returns(searchedSongs);
            mockService.CreatePlaylistAsync(Arg.Any<string>(), Arg.Any<IReadOnlyCollection<StreamingPlatformSong>>()).Returns(Task.FromResult(true));

            var viewModel = CreatePlaylistViewModel(mockService);
            await viewModel.PopulateSetlistAsync(testSetlist);

            // Manually change the selected song
            viewModel.PlaylistRowViewModels[0].SelectedSong = alternativeSong;

            // Act
            viewModel.CreatePlaylist.Execute(null);
            
            // Allow async execution to complete
            await Task.Delay(100);

            // Assert
            await mockService.Received(1).CreatePlaylistAsync(
                "Test Setlist",
                Arg.Is<IReadOnlyCollection<StreamingPlatformSong>>(songs => 
                    songs.Count == 1 && songs.First() == alternativeSong));
        }

        [TestMethod]
        public async Task PopulateSetlistAsync_ServiceThrowsException_PropagatesException()
        {
            // Arrange
            var mockService = Substitute.For<ISetlistPlaylistCreatorService>();
            var testSetlist = new Setlist("Test Setlist", "Test Artist", []);
            var expectedException = new InvalidOperationException("Service error");

            mockService.ProposePlaylistAsync(testSetlist).Returns(Task.FromException<IReadOnlyCollection<SearchedSong>>(expectedException));

            var viewModel = CreatePlaylistViewModel(mockService);

            // Act & Assert
            var actualException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                viewModel.PopulateSetlistAsync(testSetlist));
            Assert.AreSame(expectedException, actualException);
        }

        private static PlaylistViewModel CreatePlaylistViewModel(
            ISetlistPlaylistCreatorService? setlistPlaylistCreatorService = null)
        {
            return new PlaylistViewModel(
                setlistPlaylistCreatorService ?? Substitute.For<ISetlistPlaylistCreatorService>()
            );
        }
    }
}
