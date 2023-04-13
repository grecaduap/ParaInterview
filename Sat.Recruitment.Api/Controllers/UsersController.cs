using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Enums;
using Sat.Recruitment.Api.Interfaces;

namespace Sat.Recruitment.Api.Controllers
{
  [ApiController]
  [Route("api/User")]
  public partial class UsersController : ControllerBase
  {
    private readonly IFileReaderService _fileReaderService;
    private readonly IUserService _userService;


    public UsersController(IFileReaderService fileReaderService, IUserService userService)
    {
      _fileReaderService = fileReaderService;
      _userService = userService;
    }

    [HttpPost]
    [Route("/create-user")]
    public async Task<Result> CreateUser(User user)
    {
      var errors = _userService.ValidateErrors(user);

      if (!string.IsNullOrEmpty(errors))
        return new Result
        {
          IsSuccess = false,
          Errors = errors
        };

      try
      {
        user.Money += _userService.GetGiftByUserType(user.UserType, user.Money);
        user.Email = _userService.NormalizedEmail(user.Email);
        var users = _fileReaderService.GetUsers();
        var existingUser = users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
        if (existingUser!= null && existingUser.Equals(user))
        {
          return new Result
          {
            IsSuccess = false,
            Message = "The user is duplicated"
          };
        }
        
        _fileReaderService.SaveUserTotheFile(user);

        return new Result
        {
          IsSuccess = true,
          Message = "User Created"
        };
      }
      catch (Exception e)
      {
        return new Result
        {
          IsSuccess = false,
          Errors = e.Message
        };
      }
    }

  }
}