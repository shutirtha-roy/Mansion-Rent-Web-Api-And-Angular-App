﻿namespace MansionRentBackend.Application.BusinessObjects;

public sealed class LocalUser
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}