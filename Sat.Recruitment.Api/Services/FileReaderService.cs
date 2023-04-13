using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Interfaces;
using System.Reflection.PortableExecutable;
using Sat.Recruitment.Api.Enums;

namespace Sat.Recruitment.Api.Services
{
  public class FileReaderService: IFileReaderService
  {
    private readonly IConfiguration _config;


    public FileReaderService(IConfiguration configuration)
    {
      _config = configuration;
    }

    private StreamReader ReadUsersFromFile()
    {
      var fullPath = Directory.GetCurrentDirectory() + _config["FilePath"];

      FileStream fileStream = new FileStream(fullPath, FileMode.Open);

      StreamReader reader = new StreamReader(fileStream);
      return reader;
    }

    public void SaveUserTotheFile(User user)
    {
      var fullPath = Directory.GetCurrentDirectory() + _config["FilePath"];
      var newLine = $"\n{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserType.ToString()},{user.Money}";
      using (StreamWriter writer = File.AppendText(fullPath))
      {
        // write the new line to the file, separated by commas
        writer.Write(newLine);
      }
    }
    public List<User> GetUsers()
    {
      var users = new List<User>();
      var reader = ReadUsersFromFile();

      using (reader)
      {
        while (reader.Peek() >= 0)
        {
          var line = reader.ReadLineAsync().Result;
          var user = new User
          {
            Name = line.Split(',')[0].ToString(),
            Email = line.Split(',')[1].ToString(),
            Phone = line.Split(',')[2].ToString(),
            Address = line.Split(',')[3].ToString(),
            UserType = GetUserType(line.Split(',')[4].ToString()),
            Money = decimal.Parse(line.Split(',')[5].ToString()),
          };
          users.Add(user);
        }

      }

      return users;
    }

    private UserTypeEnum GetUserType(string type)
    {
      var result = UserTypeEnum.Normal;

      if (type.Contains(UserTypeEnum.Normal.ToString()))
      {
        return UserTypeEnum.Normal;
      }
      else if (type.Contains(UserTypeEnum.SuperUser.ToString()))
      {
        return UserTypeEnum.SuperUser;
      }
      else if(type.Contains(UserTypeEnum.Premium.ToString()))
      {
        return  UserTypeEnum.Premium;
      }

      return result;
    }
  }
}
