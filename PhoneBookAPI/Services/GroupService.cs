using PhoneBookAPI.Data;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Services;

public interface IGroupService
{
    List<Group> GetAllGroups();
    Group GetGroupByName(string name);
    Group GetGroupById(int id);
    void CreateGroup(Group newGroup);
    void UpdateGroup(int id, Group group);
    void DeleteGroup(int id);
}

public class GroupService : IGroupService
{
    private readonly PhoneBookDbContext _dbContext;

    public GroupService(PhoneBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Group> GetAllGroups()
    {
        return _dbContext.Groups.ToList();
    }

    public Group GetGroupById(int id)
    {
        var group = _dbContext.Groups.FirstOrDefault(g => g.Id == id);
        return group == null ? throw new InvalidOperationException("Group not found") : group;
    }

    public Group GetGroupByName(string name)
    {
        var group = _dbContext.Groups.FirstOrDefault(g => g.Name == name);
        return group ?? throw new InvalidOperationException("Group not found");
    }

    public void CreateGroup(Group newGroup)
    {
        if (newGroup == null)
        {
            throw new ArgumentNullException(nameof(newGroup));
        }

        var newGroupName = newGroup.Name;
        if (string.IsNullOrEmpty(newGroupName))
        {
            throw new ArgumentException("Group name cannot be empty.", nameof(newGroup));
        }

        var existingGroup = _dbContext.Groups.FirstOrDefault(g => g.Name == newGroupName);
        if (existingGroup != null)
        {
            throw new InvalidOperationException("A group with this name already exists.");
        }

        _dbContext.Groups.Add(newGroup);
        _dbContext.SaveChanges();
    }

    public void UpdateGroup(int id, Group group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        var groupToUpdate = _dbContext.Groups.FirstOrDefault(g => g.Id == id) ?? throw new InvalidOperationException("Group not found");
        groupToUpdate.Name = group.Name;
        groupToUpdate.Description = group.Description;
        _dbContext.SaveChanges();
    }

    public void DeleteGroup(int id)
    {
        var groupToDelete = (_dbContext.Groups.FirstOrDefault(g => g.Id == id) ?? throw new InvalidOperationException("Group not found"));
        _dbContext.Remove(groupToDelete);
        _dbContext.SaveChanges();
    }
}
