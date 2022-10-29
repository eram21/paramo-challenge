using Sat.Recruitment.Api.Models;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserServices
    {
        Task<Result> Create(UserDto user);
    }
}