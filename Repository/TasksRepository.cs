using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using angular_netcore.Models;

namespace angular_netcore.Repository
{
    public class TasksRepository : ITasksRepository
    {

        private readonly AngularNetCoreDbContext _Context;
        private readonly ILogger _Logger;

        public TasksRepository(AngularNetCoreDbContext context, ILoggerFactory loggerFactory) {
          _Context = context;
          _Logger = loggerFactory.CreateLogger("TasksRepository");
        }

        public async Task<List<Tasks>> GetTasksAsync()
        {
            return await _Context.Tasks.OrderBy(c => c.name)
                                 .ToListAsync();
        }

        public async Task<PagingResult<Tasks>> GetTasksPageAsync(int skip, int take)
        {
            var totalRecords = await _Context.Tasks.CountAsync();
            var customers = await _Context.Tasks
                                 .OrderBy(c => c.name)
                                 .Skip(skip)
                                 .Take(take)
                                 .ToListAsync();
            return new PagingResult<Tasks>(customers, totalRecords);
        }

        public async Task<Tasks> GetTaskAsync(int id)
        {
            return await _Context.Tasks
                                 .SingleOrDefaultAsync(c => c.id == id);
        }

        public async Task<Tasks> InsertTaskAsync(Tasks customer)
        {
            _Context.Add(customer);
            try
            {
              await _Context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
               _Logger.LogError($"Error in {nameof(InsertTaskAsync)}: " + exp.Message);
            }

            return customer;
        }

        public async Task<bool> UpdateTaskAsync(Tasks customer)
        {
            //Will update all properties of the Task
            _Context.Tasks.Attach(customer);
            _Context.Entry(customer).State = EntityState.Modified;
            try
            {
              return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
               _Logger.LogError($"Error in {nameof(UpdateTaskAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            //Extra hop to the database but keeps it nice and simple for this demo
            //Including orders since there's a foreign-key constraint and we need
            //to remove the orders in addition to the customer
            var customer = await _Context.Tasks
                                .SingleOrDefaultAsync(c => c.id == id);
            _Context.Remove(customer);
            try
            {
              return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
               _Logger.LogError($"Error in {nameof(DeleteTaskAsync)}: " + exp.Message);
            }
            return false;
        }

    }
}