using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApi.Dtos.Comments;
using StocksApi.Extensions;
using StocksApi.Mappers;
using StocksApi.Models;
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
        private readonly UserManager<User> _userManager;
        public CommentController(ICommentService commentService, IStockService stockService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _stockService = stockService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var comments = await _commentService.GetAllAsync(cancellationToken);
            if (comments == null || !comments.Any())
                return NotFound("No comments found.");
            var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var comment = await _commentService.GetByIdAsync(id, cancellationToken);
            if (comment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto, CancellationToken cancellationToken)
        {
            if (!await _stockService.StockExists(stockId))
                return BadRequest("Stock does not exist");

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var comment = commentDto.ToCommentFromCreate(stockId);
            comment.UserId = appUser.Id;

            await _commentService.CreateAsync(comment, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto, CancellationToken cancellationToken)
        {
            var updatedComment = await _commentService.UpdateAsync(id, commentDto.ToCommentFromUpdate(), cancellationToken);
            if (updatedComment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var deletedComment = await _commentService.DeleteAsync(id, cancellationToken);
            if (deletedComment == null)
                return NotFound($"Comment with ID {id} not found.");

            return Ok(deletedComment);
        }
    }
}
