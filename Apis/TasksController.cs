using angular_netcore.Infrastructure;
using angular_netcore.Models;
using angular_netcore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace angular_netcore.Apis
{
    [Route("api/tasks")]
    public class TasksApiController : Controller
    {
        ITasksRepository _TasksRepository;
        ILogger _Logger;

        public TasksApiController(ITasksRepository casksRepo, ILoggerFactory loggerFactory) {
            _TasksRepository = casksRepo;
            _Logger = loggerFactory.CreateLogger(nameof(TasksApiController));
        }

        // GET api/casks
        [HttpGet]
        [NoCache]
        [ProducesResponseType(typeof(List<Tasks>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> Tasks()
        {
            try
            {
                var casks = await _TasksRepository.GetTasksAsync();
                return Ok(casks);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // GET api/casks/page/10/10
        [HttpGet("page/{skip}/{take}")]
        [NoCache]
        [ProducesResponseType(typeof(List<Tasks>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> TasksPage(int skip, int take)
        {
            try
            {
                var pagingResult = await _TasksRepository.GetTasksPageAsync(skip, take);
                Response.Headers.Add("X-InlineCount", pagingResult.TotalRecords.ToString());
                return Ok(pagingResult.Records);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // GET api/casks/5
        [HttpGet("{id}", Name = "GetTaskRoute")]
        [NoCache]
        [ProducesResponseType(typeof(Tasks), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> Tasks(int id)
        {
            try
            {
                var task = await _TasksRepository.GetTaskAsync(id);
                return Ok(task);
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // POST api/casks
        [HttpPost]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> CreateTask([FromBody]Tasks task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }

            try
            {
                var newTask = await _TasksRepository.InsertTaskAsync(task);
                if (newTask == null)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return CreatedAtRoute("GetTaskRoute", new { id = newTask.id },
                        new ApiResponse { Status = true, Task = newTask });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // PUT api/casks/5
        [HttpPut("{id}")]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> UpdateTask(int id, [FromBody]Tasks task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }

            try
            {
                var status = await _TasksRepository.UpdateTaskAsync(task);
                if (!status)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return Ok(new ApiResponse { Status = true, Task = task });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        // DELETE api/casks/5
        [HttpDelete("{id}")]
        // [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                var status = await _TasksRepository.DeleteTaskAsync(id);
                if (!status)
                {
                    return BadRequest(new ApiResponse { Status = false });
                }
                return Ok(new ApiResponse { Status = true });
            }
            catch (Exception exp)
            {
                _Logger.LogError(exp.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

    }
}
