using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Books;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;

        public BooksController(BookStoreDbContext context , IMapper mapper , ILogger<BooksController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadOnlyBookDto>>> GetAllBooks()
        {
            try
            {
                var books = await context.Books
                    .Include(q=>q.Author)
                    .ToListAsync();
                var mappedBooks = mapper.Map<IEnumerable<ReadOnlyBookDto>>(books);
                return Ok(mappedBooks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting List of Books {nameof(GetAllBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }




        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadOnlyBookDto>> GetBook(int id)
        {
            try
            {
                var book = await context.Books.FirstOrDefaultAsync(q => q.Id == id);
                var mappedBook = mapper.Map<ReadOnlyBookDto>(book);
                return Ok(mappedBook);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting  Books {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<CreateBookDto>> CreateBook([FromBody]CreateBookDto fromBook)
        {
            try
            {
                var bookToInsert = mapper.Map<Book>(fromBook);
                context.Add(bookToInsert);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = bookToInsert.Id },bookToInsert);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting  Books {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody]UpdateBook updateBook)
        {
            try
            {
                var fromDbBook = await context.Books.FirstOrDefaultAsync(q => q.Id == id);

                if (fromDbBook == null)
                {
                    return NotFound();
                }
                mapper.Map(updateBook, fromDbBook);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Updating  Books {nameof(UpdateBook)}");
                return StatusCode(500, Messages.Error500Message);
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await context.Books.FirstOrDefaultAsync(q => q.Id == id);
                if (book == null)
                    return NotFound();

                context.Books.Remove(book);
                await context.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Deleting  Books {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }
    }
}

