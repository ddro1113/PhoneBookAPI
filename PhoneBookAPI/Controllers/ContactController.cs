using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Models;
using PhoneBookAPI.Services;

namespace PhoneBookAPI.Controllers;

[ApiController]
[Route("contacts")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet("getAll")]
    public IEnumerable<Contact> GetAllContacts()
    {
        return _contactService.GetAllContacts();
    }

    [HttpGet("getContactsByName")]
    public IEnumerable<Contact> GetContactsByName(string name)
    {
        return _contactService.GetContactsByName(name);
    }

    [HttpGet("getContactsByNumber")]
    public IEnumerable<Contact> GetContactsByNumber(string number)
    {
        return _contactService.GetContactsByName(number);
    }

    [HttpPost("addNewContact")]
    public IActionResult AddNewContact([FromBody] Contact newContact)
    {
        try
        {
            _contactService.AddContact(newContact);
            return Ok("Contact added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("updateContact/{Id}")]
    public IActionResult UpdateContact(int Id, [FromBody] Contact contact)
    {
        try
        {
            _contactService.UpdateContact(Id, contact);
            return Ok("Contact updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("deteContact/{Id}")]
    public IActionResult DeleteContact(int Id)
    {
        try
        {
            _contactService.DeleteContact(Id);
            return Ok("Contact deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}