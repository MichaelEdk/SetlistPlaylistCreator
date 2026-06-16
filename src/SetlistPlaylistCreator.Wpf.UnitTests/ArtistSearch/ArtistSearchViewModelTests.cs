using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.SetlistPlatform;
using SetlistPlaylistCreator.Wpf.ArtistSearch;

namespace SetlistPlaylistCreator.Wpf.UnitTests.ArtistSearch
{
    [TestClass]
    public class ArtistSearchViewModelTests
    {
        [TestMethod]
        public void Constructor_WithValidSetlistSearch_SetsPropertiesCorrectly()
        {
            // Act
            var viewModel = CreateArtistSearchViewModel();

            // Assert
            Assert.AreEqual(string.Empty, viewModel.ArtistSearchTerm);
            Assert.IsNotNull(viewModel.Setlists);
            Assert.AreEqual(0, viewModel.Setlists.Count);
            Assert.IsNotNull(viewModel.SearchArtists);
            Assert.IsNotNull(viewModel.SelectSetlist);
        }

        [TestMethod]
        public void Constructor_WithNullSetlistSearch_ThrowsArgumentNullException()
        {
            // Act & Assert
            var actualException = Assert.ThrowsException<ArgumentNullException>(() =>
                new ArtistSearchViewModel(null!));

            Assert.AreEqual("setlistSearch", actualException.ParamName);
        }

        [TestMethod]
        public void ArtistSearchTerm_WhenSet_RaisesPropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var propertyChangedRaised = false;
            string? propertyName = null;

            viewModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyName = e.PropertyName;
            };

            // Act
            viewModel.ArtistSearchTerm = "Metallica";

            // Assert that the property changed event was raised
            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(nameof(ArtistSearchViewModel.ArtistSearchTerm), propertyName);
            Assert.AreEqual("Metallica", viewModel.ArtistSearchTerm);
        }

        [TestMethod]
        public void ArtistSearchTerm_WhenSetToSameValue_DoesNotRaisePropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var initialTerm = viewModel.ArtistSearchTerm;
            var propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, e) => propertyChangedRaised = true;

            // Act
            viewModel.ArtistSearchTerm = initialTerm;

            // Assert
            Assert.IsFalse(propertyChangedRaised);
        }

        [TestMethod]
        public void Setlists_WhenSet_RaisesPropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var propertyChangedRaised = false;
            string? propertyName = null;
            var testSetlists = CreateTestSetlists();

            viewModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyName = e.PropertyName;
            };

            // Act
            viewModel.Setlists = testSetlists;

            // Assert
            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(nameof(ArtistSearchViewModel.Setlists), propertyName);
            Assert.AreSame(testSetlists, viewModel.Setlists);
        }

        [TestMethod]
        public void SearchArtists_CanExecute_WithEmptySearchTerm_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            viewModel.ArtistSearchTerm = string.Empty;

            // Act
            var canExecute = viewModel.SearchArtists.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void SearchArtists_CanExecute_WithNullSearchTerm_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            viewModel.ArtistSearchTerm = null!;

            // Act
            var canExecute = viewModel.SearchArtists.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void SearchArtists_CanExecute_WithWhitespaceSearchTerm_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            viewModel.ArtistSearchTerm = "   ";

            // Act
            var canExecute = viewModel.SearchArtists.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void SearchArtists_CanExecute_WithValidSearchTerm_ReturnsTrue()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            viewModel.ArtistSearchTerm = "Metallica";

            // Act
            var canExecute = viewModel.SearchArtists.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public async Task SearchArtists_Execute_CallsSetlistSearchAndUpdatesSetlists()
        {
            // Arrange
            var setlistSearch = Substitute.For<ISetlistSearch>();
            var testSetlists = CreateTestSetlists();
            setlistSearch.SearchForSetlistsAsync("Metallica", Arg.Any<CancellationToken>())
                .Returns(testSetlists);

            var viewModel = CreateArtistSearchViewModel(setlistSearch);
            viewModel.ArtistSearchTerm = "Metallica";

            // Act
            viewModel.SearchArtists.Execute(null);

            // Wait a bit for the async operation to complete
            await Task.Delay(100);

            // Assert
            await setlistSearch.Received(1).SearchForSetlistsAsync("Metallica", Arg.Any<CancellationToken>());
            Assert.AreEqual(testSetlists.Count, viewModel.Setlists.Count);
            Assert.AreEqual(testSetlists.First().Name, viewModel.Setlists.First().Name);
        }

        [TestMethod]
        public async Task SearchArtists_Execute_WithEmptyResults_UpdatesSetlistsToEmptyList()
        {
            // Arrange
            var setlistSearch = Substitute.For<ISetlistSearch>();
            var emptySetlists = new List<Setlist>();
            setlistSearch.SearchForSetlistsAsync("UnknownArtist", Arg.Any<CancellationToken>())
                .Returns(emptySetlists);

            var viewModel = CreateArtistSearchViewModel(setlistSearch);
            viewModel.ArtistSearchTerm = "UnknownArtist";

            // Act
            viewModel.SearchArtists.Execute(null);

            // Wait a bit for the async operation to complete
            await Task.Delay(100);

            // Assert
            await setlistSearch.Received(1).SearchForSetlistsAsync("UnknownArtist", Arg.Any<CancellationToken>());
            Assert.AreEqual(0, viewModel.Setlists.Count);
        }

        [TestMethod]
        public void SelectSetlist_CanExecute_WithValidSetlist_ReturnsTrue()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var testSetlist = CreateTestSetlist();

            // Act
            var canExecute = viewModel.SelectSetlist.CanExecute(testSetlist);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void SelectSetlist_CanExecute_WithNullSetlist_ReturnsFalse()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();

            // Act
            var canExecute = viewModel.SelectSetlist.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void SelectSetlist_Execute_WithValidSetlist_RaisesSetlistSelectedEvent()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var testSetlist = CreateTestSetlist();
            var eventRaised = false;
            SetlistSelectedEventArgs? eventArgs = null;

            viewModel.SetlistSelected += (sender, e) =>
            {
                eventRaised = true;
                eventArgs = e;
            };

            // Act
            viewModel.SelectSetlist.Execute(testSetlist);

            // Assert
            Assert.IsTrue(eventRaised);
            Assert.IsNotNull(eventArgs);
            Assert.AreSame(testSetlist, eventArgs.SelectedSetlist);
        }


        [TestMethod]
        public void ArtistSearchTerm_PropertyChange_UpdatesSearchArtistsCanExecute()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            
            // Verify initial state
            Assert.IsFalse(viewModel.SearchArtists.CanExecute(null));

            // Act
            viewModel.ArtistSearchTerm = "Metallica";

            // Assert
            Assert.IsTrue(viewModel.SearchArtists.CanExecute(null));

            // Act - clear the search term
            viewModel.ArtistSearchTerm = "";

            // Assert
            Assert.IsFalse(viewModel.SearchArtists.CanExecute(null));
        }

        [TestMethod]
        public async Task SearchArtists_Execute_UsesCancellationTokenNone()
        {
            // Arrange
            var setlistSearch = Substitute.For<ISetlistSearch>();
            var testSetlists = CreateTestSetlists();
            setlistSearch.SearchForSetlistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(testSetlists);

            var viewModel = CreateArtistSearchViewModel(setlistSearch);
            viewModel.ArtistSearchTerm = "Metallica";

            // Act
            viewModel.SearchArtists.Execute(null);

            // Wait a bit for the async operation to complete
            await Task.Delay(100);

            // Assert
            await setlistSearch.Received(1).SearchForSetlistsAsync("Metallica", CancellationToken.None);
        }

        [TestMethod]
        public void SetlistSelected_Event_CanBeSubscribedAndUnsubscribed()
        {
            // Arrange
            var viewModel = CreateArtistSearchViewModel();
            var eventRaisedCount = 0;

            void Handler(object? sender, SetlistSelectedEventArgs e) => eventRaisedCount++;

            // Act - Subscribe
            viewModel.SetlistSelected += Handler;
            viewModel.SelectSetlist.Execute(CreateTestSetlist());

            // Assert
            Assert.AreEqual(1, eventRaisedCount);

            // Act - Unsubscribe
            viewModel.SetlistSelected -= Handler;
            viewModel.SelectSetlist.Execute(CreateTestSetlist());

            // Assert
            Assert.AreEqual(1, eventRaisedCount); // Should not have increased
        }

        private static List<Setlist> CreateTestSetlists()
        {
            return 
            [
                CreateTestSetlist("Master of Puppets Tour", "Metallica"),
                CreateTestSetlist("The Black Album Tour", "Metallica")
            ];
        }

        private static Setlist CreateTestSetlist(string name = "Test Setlist", string artistName = "Test Artist")
        {
            var songs = new List<SetlistSong>
            {
                new("Song 1", artistName),
                new("Song 2", artistName)
            };

            var sets = new List<SetlistSet>
            {
                new("Set 1", songs)
            };

            return new Setlist(name, artistName, sets);
        }

        private static ArtistSearchViewModel CreateArtistSearchViewModel(
            ISetlistSearch? setlistSearch = null)
        {
            return new(
                setlistSearch ?? Substitute.For<ISetlistSearch>()
            );
        }
    }
}
