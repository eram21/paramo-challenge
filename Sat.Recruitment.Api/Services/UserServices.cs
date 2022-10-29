using Sat.Recruitment.Api.Data;
using Sat.Recruitment.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepository _repository;
        public UserServices(IRepository repository)
        {
            _repository = repository;
        }

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
            
            var isEnumParse = Enum.TryParse(userDTO.UserType, true, out UserType type);
            
            if (!isEnumParse)
                throw new ValidationException($"We have a problem... The UserType ({userDTO.UserType}) is invalid, try with: Normal, SuperUser or Premium.");

            newUser.Type = type;

            return newUser;
        }
        
        private void CalculateUserMoney(User user)
        {
            switch (user.Type)
            {
                case UserType.Normal:
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.12);
                        //If new user is normal and has more than USD100
                        var gif = user.Money * percentage;
                        user.Money += gif;
                    }
                    else if (user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = user.Money * percentage;
                        user.Money += gif;
                    }
                    break;
                case UserType.SuperUser:
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.20);
                        var gif = user.Money * percentage;
                        user.Money += gif;
                    }
                    break;
                case UserType.Premium:
                    if (user.Money > 100)
                    {
                        var gif = user.Money * 2;
                        user.Money += gif;
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task CheckForDupliatesUsers(User user)
        {
            var users = await _repository.GetAllUsers();
            string errors = string.Empty;
            
            var normalizeEmail = Utils.Utils.NormalizeEmail(user.Email);
            if (users.Exists(userFile => userFile.Email.Equals(normalizeEmail, StringComparison.InvariantCultureIgnoreCase)))
                errors += $"This email ({user.Email}) is duplicate, please try with another. {Environment.NewLine}";

            if (users.Exists(userFile => userFile.Phone.Equals(user.Phone, StringComparison.InvariantCultureIgnoreCase)))
                errors += $"This phone ({user.Phone}) is duplicate, please try with another. {Environment.NewLine}";

            if (users.Exists(userFile => 
                    userFile.Name.Equals(user.Name, StringComparison.InvariantCultureIgnoreCase) &&
                    userFile.Address.Equals(user.Address, StringComparison.InvariantCultureIgnoreCase)))
                errors += $"This name ({user.Name}) and address ({user.Address}) are duplicate, please try with another. {Environment.NewLine}";

            if (!string.IsNullOrWhiteSpace(errors))
                throw new ValidationException(errors);
        }

        public async Task<Result> Create(UserDto userDto)
        {
            #region MapUserDtoToUser

            var user = MapUserDtoToUser(userDto);

            #endregion

            #region Calculate Money

            CalculateUserMoney(user);

            #endregion

            #region Business Validations Check For Duplicates

            await CheckForDupliatesUsers(user);

            #endregion

            #region Add User to Repository

            _repository.InserUser(user);

            #endregion

            return Result.Ok("User create.");
        }
    }
}