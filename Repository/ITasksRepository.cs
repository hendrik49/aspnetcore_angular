using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using angular_netcore.Models;

namespace angular_netcore.Repository
{
    public interface ITasksRepository
    {     
        Task<List<Tasks>> GetTasksAsync();
        Task<PagingResult<Tasks>> GetTasksPageAsync(int skip, int take);
        Task<Tasks> GetTaskAsync(int id);
        
        Task<Tasks> InsertTaskAsync(Tasks customer);
        Task<bool> UpdateTaskAsync(Tasks customer);
        Task<bool> DeleteTaskAsync(int id);
    }
}