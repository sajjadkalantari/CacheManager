using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CacheManager.Controllers
{
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService bookService)
        {
            _commentService = bookService;
        }

        [HttpGet]
        [Route("comment/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _commentService.GetComment(id);

            if (result != null)
                return Ok(result);

            return NotFound();
        }
    }
}
