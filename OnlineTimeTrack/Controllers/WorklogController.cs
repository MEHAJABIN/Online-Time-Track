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
        private readonly IWorklogService _worklogService;
        private readonly IProjectService _projectService;

        public WorklogController(IWorklogService worklogService, IUserService userService, IProjectService projectService)
        {
            _worklogService = worklogService;
            _userService = userService;
            _projectService = projectService;
        }


        [HttpPost]
        public async Task<Response<Worklog>> Worklog([FromBody]Worklog worklog)
        {
            var userID = _userService.GetUserIDFromContext(HttpContext);

            if (userID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Not a valid user", null);
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
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }


        [HttpGet("GetById")]
        public async Task<Response<Worklog>> GetById([FromQuery]long? id)

        {
            if (id == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);
            }

            try
            {
                var ExistingId = await _worklogService.GetById(id.GetValueOrDefault());

                if (ExistingId == null)
                {
                    return Response<Worklog>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<Worklog>.CreateResponse(true, "Successfully loaded.", ExistingId);
            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }





        [HttpPut("UpdateWorklog")]
        public async Task<Response<Worklog>> UpdateWorklog([FromBody] Worklog WorklogID)
        {
            var userID = _userService.GetUserIDFromContext(HttpContext);

            if (userID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Not a valid user", null);
            }

            if (WorklogID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);
            }


            WorklogID.UserID = userID.GetValueOrDefault();


            try
            {
                var ExistingWorklog = await _worklogService.UpdateWorklog(WorklogID);

                if (ExistingWorklog == null)
                {
                    return Response<Worklog>.CreateResponse(false, "Not a valid Id", null);
                }
                return Response<Worklog>.CreateResponse(true, "Successfully updated.", ExistingWorklog);

            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpDelete("DeleteWorklog")]
        public async Task<Response<Worklog>> DeleteWorklog([FromBody] Worklog WorklogID)

        {
            var userID = _userService.GetUserIDFromContext(HttpContext);

            if (userID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Not a valid user", null);
            }

            if (WorklogID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);

            }

            WorklogID.UserID = userID.GetValueOrDefault();

            try
            {
                var ExistingWorklog = await _worklogService.DeleteWorklog(WorklogID);


                if (ExistingWorklog == null)
                {
                    return Response<Worklog>.CreateResponse(false, "Not a valid Worklog Id", null);
                }

                return Response<Worklog>.CreateResponse(true, "Successfully deleted.", ExistingWorklog);
            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("ProjectID")]
        public async Task<Response<IEnumerable<Worklog>>> GetWorklog([FromQuery] long? ProjectID)

        {
            if (ProjectID == null)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, "Please provide valid Project Id.", null);

            }
            try
            {
                var worklogs = await _worklogService.GetAll(ProjectID.GetValueOrDefault());
                if (worklogs == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklogs);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
        }


        [HttpGet("Feature")]
        public async Task<Response<IEnumerable<Worklog>>> GetFeature([FromQuery] string Features)
        {
            if (string.IsNullOrWhiteSpace(Features))
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, "Please provide valid Feature.", null);

            }
            try
            {
                var worklogs = await _worklogService.GetAll(Features.ToString());
                if (worklogs == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully loaded.", worklogs);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("UserID")]
        public async Task<Response<IEnumerable<Worklog>>> GetUSerWorklog([FromQuery] long? UserID)

        {
            if (UserID == null)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, "Please provide valid User Id.", null);

            }
            try
            {
                var worklogs = await _worklogService.Get(UserID.GetValueOrDefault());
                if (worklogs == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklogs);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("GetAllWorklogs")]
        public async Task<Response<IEnumerable<Worklog>>>GetAllWorklogs([FromQuery] int start,int limit, long? WorklogID, long? UserID, long? ProjectID, string Features, int EstimateWorkTime,
        DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd, string ProjectTitle, string FullName, string Address)

        {
           
            try
            {
                var worklogs = await _worklogService.GetAllWorklogs(start, limit, WorklogID, UserID, ProjectID, Features, EstimateWorkTime, ActualWorkTimeStart, ActualWorkTimeEnd, ProjectTitle, FullName, Address);

                if (worklogs == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not  valid ", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully loaded.", worklogs);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
          
        }
    }
}













































































































