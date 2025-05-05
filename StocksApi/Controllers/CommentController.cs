using Microsoft.AspNetCore.Mvc;
using StocksApi.Dtos.Comments;
using StocksApi.Mappers;
using StocksApi.Repositories.Interfaces;
using StocksApi.Services.Interfaces;

namespace StocksApi.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IStockService _stockService;
        public CommentController(ICommentService commentService, IStockService stockService)
        {
            _commentService = commentService;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentService.GetAllAsync();
            if (comments == null || !comments.Any())
                return NotFound("No comments found.");
            var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();

            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (!await _stockService.StockExists(stockId))
                return BadRequest("Stock does not exist");

            var comment = commentDto.ToCommentFromCreate(stockId);
            await _commentService.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
        {
            var updatedComment = await _commentService.UpdateAsync(id, commentDto.ToCommentFromUpdate());
            if (updatedComment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedComment = await _commentService.DeleteAsync(id);
            if (deletedComment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(deletedComment);
        }
    }
}
