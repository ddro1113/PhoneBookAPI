using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Models;
using PhoneBookAPI.Services;
using System.Xml.Linq;

namespace PhoneBookAPI.Controllers;

[ApiController]
[Route("groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet("getAll")]
    public IEnumerable<Group> GetAllGroups()
    {
        return _groupService.GetAllGroups();
    }

    [HttpGet("getGroupById/{id}")]
    public IActionResult GetGroupById(int id)
    {
        try
        {
            var group = _groupService.GetGroupById(id);
            return Ok(group);
        }
        catch (InvalidOperationException ex) when (ex.Message == "Group not found")
        {
            return NotFound("Group not found.");
        }
    }

    [HttpGet("getGroupByName/{name}")]
    public IActionResult GetGroupByName(string name)
    {
        try
        {
            var group = _groupService.GetGroupByName(name);
            return Ok(group);
        }
        catch (InvalidOperationException ex) when (ex.Message == "Group not found")
        {
            return NotFound("Group not found.");
        }
    }

    [HttpPost("createNewGroup")]
    public IActionResult CreateGroup([FromBody] Group newGroup)
    {
        try
        {
            _groupService.CreateGroup(newGroup);
            return Ok("Group created successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("updateGroup/{Id}")]
    public IActionResult UpdateContact(int Id, [FromBody] Group group)
    {
        try
        {
            _groupService.UpdateGroup(Id, group);
            return Ok("Group updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("deleteGroup/{Id}")]
    public IActionResult DeleteContact(int Id)
    {
        try
        {
            _groupService.DeleteGroup(Id);
            return Ok("Group deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}