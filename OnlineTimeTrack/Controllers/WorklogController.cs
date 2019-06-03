using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;
using OnlineTimeTrack.Contexts;

namespace OnlineTimeTrack.Controllers
{
    [Route("api/Worklog")]

    public class WorklogController : ControllerBase
    {
        private readonly IUserService _userService;
        private IWorklogService _worklogService;

        public WorklogController(IWorklogService worklogService,IUserService userService)
        {
            _worklogService = worklogService;
            _userService = userService;
        } 

        [HttpPost]
        public async Task<Response<Worklog>> Worklog([FromBody]Worklog worklog)
        {
            var userID = _userService.GetUserIDFromContext(HttpContext);

            if (userID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Not a valid user",null);
            }

            if (worklog == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);

            }

            worklog.UserID = userID.GetValueOrDefault();
            
            
            try
            {
                var newWorklog = await _worklogService.Worklog(worklog);
                return Response<Worklog>.CreateResponse(true, "Successfully uploaded.", null);
            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(true, e.Message, null);
            }

        }
     /*   [HttpGet("Worklog")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Ok("Id");
        }*/
    }
}







  




























