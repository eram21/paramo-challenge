using Sat.Recruitment.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Data
{
    public interface IRepository
    {
        Task<List<User>> GetAllUsers();
        Task InsertUser(User user);
    }
}