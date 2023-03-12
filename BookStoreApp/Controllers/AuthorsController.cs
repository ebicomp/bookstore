using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Authors;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDbContext dbContext , IMapper mapper , ILogger<AuthorsController> logger)
        {
            this._dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadonlyDto>>> GetAuthors()
        {
            try
            {
                var authors = await _dbContext.Authors.ToListAsync();
                var result = mapper.Map<IEnumerable<AuthorReadonlyDto>>(authors);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Performing Get");
                return StatusCode(500, Messages.Error500Message);
            }

        }

        // GET: api/values
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadonlyDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _dbContext.Authors.FirstOrDefaultAsync(q => q.Id == id);
                if (author == null)
                {
                    logger.LogWarning($"Record not found {nameof(GetAuthor)} - id {id}");
                    return NotFound();
                }
                var result = mapper.Map<AuthorReadonlyDto>(author);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"error in getting one author {nameof(GetAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
            

        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAuthor([FromBody]AuthorCreateDto author)
        {
            var mappedAuthor = mapper.Map<Author>(author);
            _dbContext.Authors.Add(mappedAuthor);
            await _dbContext.SaveChangesAsync();
            return Ok(author);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAuthor(int id, [FromBody]AuthorUpdateDto author)
        {
            var authorFromDb = await _dbContext.Authors.FirstOrDefaultAsync(q => q.Id == id);

            if (author == null)
                return NotFound();

            mapper.Map(author, authorFromDb);

            _dbContext.Authors.Update(authorFromDb);
            await _dbContext.SaveChangesAsync();
            return Ok(authorFromDb);

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(q => q.Id == id);

            if (author == null)
                return NotFound();

            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

