using System.Collections.Generic;
using System.Threading.Tasks;
using angular_netcore.Models;

namespace angular_netcore.Repository
{
    public interface IStatesRepository
    {
        Task<List<State>> GetStatesAsync();
    }
}