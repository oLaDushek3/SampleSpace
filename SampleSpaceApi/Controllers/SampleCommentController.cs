using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize]
    [HttpPost("create-comment")]
    public async Task<IActionResult> CreateComment(CreateSampleCommentRequest request)
    {
        var (requestComment, requestError) = SampleComment.Create(Guid.NewGuid(), request.SampleGuid,
            request.UserGuid, DateTime.Now, request.Comment, null);

        if (!string.IsNullOrEmpty(requestError))
            return BadRequest(requestError);
        
        var (newCommentGuid, createError) = await commentServices.CreateNewComment(requestComment!);
        
        if (!string.IsNullOrEmpty(createError))
            return BadRequest(createError);
        
        return Ok(newCommentGuid);
    }
    
    [Authorize]
    [HttpPut("edit-comment")]
    public async Task<IActionResult> EditComment(EditSampleCommentRequest request)
    {
        var (comment, getError) = await commentServices.GetSampleComment(request.CommentGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        
        if (new Guid(loginUserGuid) != comment!.UserGuid)
            return Forbid();

        var (modifiedComment, modifiedError) = comment!.Edit(request.Comment);
        
        if(!string.IsNullOrEmpty(modifiedError))
            return  BadRequest(modifiedError);
        
        var (successfully, editError) = await commentServices.EditComment(modifiedComment!);
        
        if(!string.IsNullOrEmpty(editError))
            return  BadRequest(editError);

        return successfully ? Ok() : BadRequest("Server error");
    }
    
    [Authorize]
    [HttpDelete("delete-comment")]
    public async Task<IActionResult> DeleteSampleComment([FromQuery(Name = "comment-guid")] Guid commentGuid)
    {
        var (comment, getError) = await commentServices.GetSampleComment(commentGuid);
        
        if(!string.IsNullOrEmpty(getError))
            return  BadRequest(getError);

        var loginUserGuid = User.FindFirst(ClaimTypes.Authentication)!.Value;
        
        if (new Guid(loginUserGuid) != comment!.UserGuid)
            return Forbid();

        var (successfully, deleteError) = await commentServices.DeleteComment(commentGuid);
        
        if(!string.IsNullOrEmpty(deleteError))
            return  BadRequest(deleteError);
        
        return successfully ? Ok() : BadRequest("Server error");
    }
}