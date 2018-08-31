using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloCosmosDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloCosmosDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private readonly BookmarksDbContext _bookmarksDbContext;
        public BookmarksController(BookmarksDbContext bookmarksDbContext)
        {
            _bookmarksDbContext = bookmarksDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Bookmark>> Get()
        {
            var bookmarks = _bookmarksDbContext.Bookmarks?.ToList();
            return Ok(bookmarks);
        }

        [HttpGet("{id}")]
        public ActionResult<Bookmark> Get(string id)
        {
            var bookmark = _bookmarksDbContext.Bookmarks.FirstOrDefault(x => x.Id == id);
            if (bookmark == null)
            {
                return NoContent();
            }

            return Ok(bookmark);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _bookmarksDbContext.Bookmarks.Add(bookmark);
            await _bookmarksDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get),new{ id = bookmark.Id }, bookmark);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var temp = _bookmarksDbContext.Bookmarks.FirstOrDefault(x => x.Id == id);
            if (bookmark == null)
            {
                return BadRequest();
            }

            temp.Image = bookmark.Image;
            temp.Description = bookmark.Description;
            temp.Title = bookmark.Title;
            temp.Url = bookmark.Url;
            temp.CreatedOn = bookmark.CreatedOn;
            _bookmarksDbContext.Bookmarks.Update(temp);
            await _bookmarksDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var bookmark = _bookmarksDbContext.Bookmarks.FirstOrDefault(x => x.Id == id);
            if (bookmark == null)
            {
                return BadRequest();
            }

            _bookmarksDbContext.Bookmarks.Remove(bookmark);
            await _bookmarksDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}