namespace PhoneBookAPI.Models;

public class Contact
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Number { get; set; }

    public int? GroupId { get; set; }

    public Group Group { get; set; }

}