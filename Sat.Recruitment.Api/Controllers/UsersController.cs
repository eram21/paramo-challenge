using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>Result</returns>
        [HttpPost]
        public async Task<Result> CreateUser([Required][FromBody] UserDto userDTO)
        {
            try
            {
                return await _userServices.Create(userDTO);
            }
            catch (Exception e)
            {
                return Result.Error(e);
            }
        }
    }
}