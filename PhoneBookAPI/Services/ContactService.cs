using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Data;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Services;

public interface IContactService
{
    List<Contact> GetAllContacts();
    List<Contact> GetContactsByName(string name);
    List<Contact> GetContactsByNumber(string number);
    void UpdateContact(int id, Contact contact);
    void DeleteContact(int id);
    void AddContact(Contact contact);
}

public class ContactService : IContactService
{
    private readonly PhoneBookDbContext _dbContext;

    public ContactService(PhoneBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Contact> GetAllContacts()
    {
        return _dbContext.Contacts.Include(c => c.Group).ToList();
    }

    public List<Contact> GetContactsByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return _dbContext.Contacts.ToList();
        }
        return _dbContext.Contacts.Where(contact => EF.Functions.Like(contact.Name, $"%{name}%")).ToList();
    }

    public List<Contact> GetContactsByNumber(string number)
    {
        if (string.IsNullOrEmpty(number))
        {
            return _dbContext.Contacts.ToList();
        }
        else
        {
            return _dbContext.Contacts.Where(contact => EF.Functions.Like(contact.Number, $"%{number}%")).ToList();
        }
    }

    public void AddContact(Contact newContact)
    {
        if (newContact == null)
        {
            throw new ArgumentNullException(nameof(newContact));
        }

        var newContactNumber = newContact.Number;
        if (string.IsNullOrEmpty(newContactNumber))
        {
            throw new ArgumentException("Contact number cannot be empty.", nameof(newContact));
        }

        if (newContact.GroupId.HasValue && newContact.GroupId.Value > 0)
        {
            var group = _dbContext.Groups.FirstOrDefault(g => g.Id == newContact.GroupId.Value) ?? throw new InvalidOperationException("Invalid Group Id. Group does not exist.");
            newContact.Group = group;
        }
        else
        {
            var noGroup = _dbContext.Groups.FirstOrDefault(g => g.Name == "");
            if (noGroup == null)
            {
                noGroup = new Group { Name = "", Description = "" };
                _dbContext.Groups.Add(noGroup);
                _dbContext.SaveChanges();
            }
            newContact.Group = noGroup;
        }

        _dbContext.Contacts.Add(newContact);
        _dbContext.SaveChanges();
    }

    public void UpdateContact(int id, Contact contact)
    {
        if (contact == null)
        {
            throw new ArgumentNullException(nameof(contact));
        }

        var contactToUpdate = _dbContext.Contacts.FirstOrDefault(c => c.Id == id) ?? throw new InvalidOperationException("Contact not found");
        contactToUpdate.Name = contact.Name;
        contactToUpdate.Number = contact.Number;
        _dbContext.SaveChanges();
    }

    public void DeleteContact(int id)
    {
        var contactToDelete = (_dbContext.Contacts.FirstOrDefault(c => c.Id == id) ?? throw new InvalidOperationException("Contact not found"));
        _dbContext.Remove(contactToDelete);
        _dbContext.SaveChanges();
    }
}
