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
        private readonly string _pathUserFile;
        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            _pathUserFile = Directory.GetCurrentDirectory() + _configuration["PathUserFile"];
        }

        /// <summary>
        /// Get all users from users file text.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers()
        {
            var _users = new List<User>();
            var reader = Utils.Utils.ReadFromFile(_pathUserFile);

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

        /// <summary>
        /// Insert user into file text.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task InsertUser(User user)
        {
            string[] userArray = { user.Name, user.Email, user.Phone, user.Address, user.Type.ToString(), user.Money.ToString().Replace(",",".") };
            await File.AppendAllTextAsync(_pathUserFile, string.Join(',', userArray) + Environment.NewLine);
        }
    }
}