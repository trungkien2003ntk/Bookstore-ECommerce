﻿using KKBookstore.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBookstore.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<int>, ISoftDelete
{
    public User()
    {

        IsActive = true;
        IsEmployee = false;
        IsAdmin = false;
        IsDeleted = false;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    [NotMapped]
    public string FullName => $"{LastName} {FirstName}";

    public DateTimeOffset DateOfBirth { get; set; }

    public string? ImageUrl { get; set; }

    public string? UserPreferences { get; set; }

    public LoginType LoginType { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmployee { get; set; }

    public bool IsAdmin { get; set; }

    public UserStatus Status { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedWhen { get; set; }

    public DateTimeOffset CreatedWhen { get; set; }

    public int? CreatedBy { get; set; }

    public User? CreatedByUser { get; set; }

    public int? LastEditedBy { get; set; }

    public DateTimeOffset LastEditedWhen { get; set; }

    public User? LastEditedByUser { get; set; }
}