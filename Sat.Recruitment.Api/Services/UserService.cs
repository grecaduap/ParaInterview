using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Enums;
using Sat.Recruitment.Api.Interfaces;

namespace Sat.Recruitment.Api.Services
{
  public class UserService: IUserService
  {
    private readonly IConfiguration _config;

    public UserService(IConfiguration configuration)
    {
      _config = configuration;
    }
    public decimal GetGiftByUserType(UserTypeEnum userType, decimal money )
    {
      decimal gift = 0;
      var toplimit = 0;
      var bottomLimit = 0;
      var validTopLimit = false;
      var validBottomLimit = false;
      var validTopPercentage = false;
      var validBottomPercentage = false;
      decimal giftTopPercentage = 0;
      decimal giftBottomPercentage = 0;

      switch (userType)
      {
        case UserTypeEnum.Normal:

          validTopLimit  = int.TryParse(_config.GetSection("NormalClient:TopLimit").Value, out toplimit);
          validBottomLimit = int.TryParse(_config.GetSection("NormalClient:BottomLimit").Value, out bottomLimit);
          validTopPercentage = decimal.TryParse(_config.GetSection("NormalClient:TopPercentage").Value, out giftTopPercentage);
          validBottomPercentage = decimal.TryParse(_config.GetSection("NormalClient:BottomPercentage").Value, out giftBottomPercentage);

          if (validTopLimit && validBottomLimit && validTopPercentage && validBottomPercentage)
          {
            if (money > toplimit)
            {
              gift = money * giftTopPercentage;
            }
            else if (money < toplimit && money > bottomLimit)
            {
              gift = money * giftBottomPercentage;
            }
          }
          break;
        case UserTypeEnum.SuperUser:
         
            validTopLimit = int.TryParse(_config.GetSection("SuperClient:TopLimit").Value, out toplimit);
            validTopPercentage = decimal.TryParse(_config.GetSection("SuperClient:TopPercentage").Value, out giftTopPercentage);
            if (validTopLimit && validTopPercentage)
            {
              if (money > toplimit)
              {
                gift = money * giftTopPercentage;
              }
            } 
            break;
        case UserTypeEnum.Premium:
          validTopLimit = int.TryParse(_config.GetSection("PremiumClient:TopLimit").Value, out toplimit);
          validTopPercentage = decimal.TryParse(_config.GetSection("PremiumClient:TopPercentage").Value, out giftTopPercentage);
          if (validTopLimit && validTopPercentage)
          {
            if (money > toplimit)
            {
              gift = money * giftTopPercentage;
            }
          }
          break;
       
      }

      return gift;

    }

    public string NormalizedEmail(string email)
    {
      var aux = email.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
      var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
      aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);
      return string.Join("@", aux[0], aux[1]);
    }

    public string ValidateErrors(User user)
    {
      var errors = "";
      if (user.Name == null)
        //Validate if Name is null
        errors = "The name is required";
      if (user.Email == null)
        //Validate if Email is null
        errors = errors + " The email is required";
      if (user.Address == null)
        //Validate if Address is null
        errors = errors + " The address is required";
      if (user.Phone == null)
        //Validate if Phone is null
        errors = errors + " The phone is required";

      return errors;
    }
  }
}
