using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using SampleODataApp.Models;

namespace SampleODataApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly SampleODataDbContext _sampleODataDbContext;
        public PersonController(SampleODataDbContext sampleODataDbContext)
        {
            _sampleODataDbContext = sampleODataDbContext;
        }

        [EnableQuery]
        public IQueryable<Person> Get()
        {
            return _sampleODataDbContext.Persons.AsQueryable();
        }

        [EnableQuery]
        public async Task<Person> Get(int key)
        {
            return await _sampleODataDbContext.Persons.FirstOrDefaultAsync(p => p.Id == key);
        }

        public async Task<IActionResult> Post([FromBody]Person person)
        {
            if (ModelState.IsValid)
            {
                _sampleODataDbContext.Persons.Add(person);
                await _sampleODataDbContext.SaveChangesAsync();
                return Created($"?key={person.Id}", person);
            }

            return BadRequest(ModelState);
        }

        [AcceptVerbs("PUT")]
        // [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            _sampleODataDbContext.Entry(person).State = EntityState.Modified;
            try
            {
                _sampleODataDbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var person = await _sampleODataDbContext.Persons.FindAsync(key);
            if (person == null)
            {
                return NotFound();
            }

            _sampleODataDbContext.Persons.Remove(person);
            await _sampleODataDbContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        private bool PersonExists(int key)
        {
            return _sampleODataDbContext.Persons.Any(p => p.Id == key);
        }
    }
}