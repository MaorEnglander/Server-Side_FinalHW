using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();

    public static User FromObject(object v)
    {
        if (v is User u) return u;
        throw new InvalidCastException("Cannot convert object to User");
    }
}
