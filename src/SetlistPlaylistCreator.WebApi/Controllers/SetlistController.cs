using Microsoft.AspNetCore.Mvc;
using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.SetlistPlatform;

namespace SetlistPlaylistCreator.WebApi.Controllers
{
    public class SetlistController : ControllerBase
    {
        private readonly ISetlistSearch _setlistSearch;

        public SetlistController(ISetlistSearch setlistSearch)
        {
            _setlistSearch = setlistSearch;
        }

        [HttpGet]
        [Route("search")]
        public async Task<List<Setlist>> SearchSetlists(string artistName)
        {
            var result = await _setlistSearch.SearchForSetlistsAsync(artistName, CancellationToken.None).ConfigureAwait(true);
            return result.ToList();
        }
    }
}
