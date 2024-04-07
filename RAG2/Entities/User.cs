using System;
using System.Collections.Generic;

namespace RAG2.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Sex { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HomeNumber { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
