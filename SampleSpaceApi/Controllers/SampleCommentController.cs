using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SampleSpaceApi.Contracts.SampleComment;
using SampleSpaceCore.Abstractions.Services;
using SampleSpaceCore.Models;

namespace SampleSpaceApi.Controllers;

[ApiController]
[Route("api/sample-comment")]
public class SampleCommentController(ISampleCommentServices commentServices) : ControllerBase
{
    [HttpGet("get-comments")]
    public async Task<IActionResult> GetSampleComments([FromQuery(Name = "sample-guid")] Guid sampleGuid)
    {
        var (sampleComments, error) = await commentServices.GetSampleComments(sampleGuid);
        
        if (!string.IsNullOrEmpty(error))
            return BadRequest(error);
        
        if (sampleComments == null)
            return NotFound();
        
        return Ok(sampleComments); 
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPost("create-new-comment")]
    public async Task<IActionResult> CreateNewComment(CreateSampleCommentRequest createCommentRequest)
    {
        var (requestComment, requestError) = SampleComment.Create(Guid.NewGuid(), createCommentRequest.SampleGuid,
            createCommentRequest.UserGuid, DateTime.Now, createCommentRequest.Comment, null);

        if (!string.IsNullOrEmpty(requestError))
            return BadRequest(requestError);
        
        var (newCommentGuid, createError) = await commentServices.CreateNewComment(requestComment!);
        
        if (!string.IsNullOrEmpty(createError))
            return BadRequest(createError);
        
        return Ok(newCommentGuid);
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpPut("edit-comment")]
    public async Task<IActionResult> DeleteSampleComment(EditSampleCommentRequest editCommentRequest)
    {
        var (comment, getError) = await commentServices.GetSampleComment(editCommentRequest.CommentGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != comment!.UserGuid)
        //     return Forbid();

        var (modifiedComment, modifiedError) = comment!.Edit(editCommentRequest.Comment);
        
        if(!string.IsNullOrEmpty(modifiedError))
            return  BadRequest(modifiedError);
        
        var (successfully, editError) = await commentServices.EditComment(modifiedComment!);
        
        if(!string.IsNullOrEmpty(editError))
            return  BadRequest(editError);

        return successfully ? Ok() : BadRequest("Server error");
    }
    
    // Раскомментировать после развертывания на сервере
    //[Authorize]
    [HttpDelete("delete-comment")]
    public async Task<IActionResult> DeleteSampleComment([FromQuery(Name = "comment-guid")] Guid commentGuid)
    {
        var (comment, getError) = await commentServices.GetSampleComment(commentGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        // Раскомментировать после развертывания на сервере
        // var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        //
        // if (new Guid(loginUserGuid) != comment!.UserGuid)
        //     return Forbid();

        var (successfully, deleteError) = await commentServices.DeleteComment(commentGuid);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
}