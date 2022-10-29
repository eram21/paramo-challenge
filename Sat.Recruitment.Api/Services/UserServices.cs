using Sat.Recruitment.Api.Models;
using System;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserServices : IUserServices
    {
        private User MapUserDtoToUser(UserDto userDTO)
        {
            var newUser = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Address = userDTO.Address,
                Phone = userDTO.Phone,
                Money = userDTO.Money
            };
            
            Enum.TryParse(userDTO.UserType, out UserType type);
            newUser.Type = type;

            newUser.Email = Utils.Utils.NormalizeEmail(newUser.Email);

            return newUser;
        }       

        public async Task<Result> Create(UserDto userDto)
        {
            //return null;
            var user = MapUserDtoToUser(userDto);

            #region Calculate Money
            if (newUser.UserType == "Normal")
            {
                if (decimal.Parse(money) > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = decimal.Parse(money) * percentage;
                    newUser.Money = newUser.Money + gif;
                }
                if (decimal.Parse(money) < 100)
                {
                    if (decimal.Parse(money) > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = decimal.Parse(money) * percentage;
                        newUser.Money = newUser.Money + gif;
                    }
                }
            }
            if (newUser.UserType == "SuperUser")
            {
                if (decimal.Parse(money) > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = decimal.Parse(money) * percentage;
                    newUser.Money = newUser.Money + gif;
                }
            }
            if (newUser.UserType == "Premium")
            {
                if (decimal.Parse(money) > 100)
                {
                    var gif = decimal.Parse(money) * 2;
                    newUser.Money = newUser.Money + gif;
                }
            }
            #endregion

            #region Business Validations Check For Duplicates

            #region Open Repository
            var reader = ReadUsersFromFile();

            //Normalize email


            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(user);
            }
            reader.Close();
            #endregion

            try
            {
                var isDuplicated = false;
                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email
                        ||
                        user.Phone == newUser.Phone)
                    {
                        isDuplicated = true;
                    }
                    else if (user.Name == newUser.Name)
                    {
                        if (user.Address == newUser.Address)
                        {
                            isDuplicated = true;
                            throw new Exception("User is duplicated");
                        }

                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");

                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created"
                    };
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");

                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    };
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }
        }
        #endregion

            #region Add User to Repository

        #endregion
    }
}