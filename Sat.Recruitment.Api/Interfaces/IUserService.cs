using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Enums;

namespace Sat.Recruitment.Api.Interfaces
{
  public interface IUserService
  {
    decimal GetGiftByUserType(UserTypeEnum userType, decimal money);
    string NormalizedEmail(string email);
    string ValidateErrors(User user);
  }
}
