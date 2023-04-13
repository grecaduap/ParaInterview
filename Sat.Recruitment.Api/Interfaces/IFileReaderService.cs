using Sat.Recruitment.Api.Entities;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Interfaces
{
  public interface IFileReaderService
  {
    List<User> GetUsers();
    void SaveUserTotheFile(User user);
  }
}
