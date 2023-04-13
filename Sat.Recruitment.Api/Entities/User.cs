using Microsoft.AspNetCore.Mvc.RazorPages;
using Sat.Recruitment.Api.Enums;
using System;

namespace Sat.Recruitment.Api.Entities
{
  public class User
  {
      public string Name { get; set; }
      public string Email { get; set; }
      public string Address { get; set; }
      public string Phone { get; set; }
      public UserTypeEnum UserType { get; set; }
      public decimal Money { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      User other = (User)obj;
      return Name == other.Name && other.Address == Address && other.Phone == Phone && other.UserType == UserType;
    }
  }
}
