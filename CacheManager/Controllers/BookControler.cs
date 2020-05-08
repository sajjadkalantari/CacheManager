using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CacheManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CacheManager.Controllers
{
    [ApiController]   
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {            
            _bookService = bookService;
        }

        [HttpGet]
        [Route("book/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _bookService.GetBook(id);

            if(result != null)
                return Ok(result);

            return NotFound();
        }
    }
}
