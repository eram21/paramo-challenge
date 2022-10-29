using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Data
{
    public class Repository : IRepository
    {
        private IConfiguration _configuration;
        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + _configuration["PathUserFile"];

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);

            return reader;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var _users = new List<User>();
            var reader = ReadUsersFromFile();

            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = Utils.Utils.NormalizeEmail(line.Split(',')[1].ToString()),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString().Replace(".", ",")),
                };

                Enum.TryParse(line.Split(',')[4].ToString(), true, out UserType type);
                user.Type = type;

                _users.Add(user);
            }
            reader.Close();

            return _users;
        }

        public void InserUser(User user)
        {
            var path = Directory.GetCurrentDirectory() + _configuration["PathUserFile"];
            string[] userArray = { user.Name, user.Email, user.Phone, user.Address, user.Type.ToString(), user.Money.ToString().Replace(",",".") };
          //  Agustina,Agustina@gmail.com,+534645213542,Garay y Otra Calle,SuperUser,112234
            File.AppendAllText(path, string.Join(',', userArray) + Environment.NewLine);
        }
    }
}