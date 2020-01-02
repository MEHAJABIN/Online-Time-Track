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
        private readonly ITimelogService _timelogService;

        public WorklogController(IWorklogService worklogService, IUserService userService, IProjectService projectService,ITimelogService timelogService)
        {
            _worklogService = worklogService;
            _userService = userService;
            _projectService = projectService;
            _timelogService = timelogService;
        }


        [HttpPost]
        public async Task<Response<Worklog>> Worklog([FromBody]Worklog worklog)
        {
            

            if (worklog == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);

            }

            try
            {
                var newWorklog = await _worklogService.Worklog(worklog);

                return Response<Worklog>.CreateResponse(true, "Successfully uploaded.", newWorklog);
            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }


        [HttpGet("GetById")]
        public async Task<Response<Worklog>> GetById([FromQuery] int id)

        {

            try
            {
                var ExistingId = await _worklogService.GetById(id);

                if (ExistingId == null)
                {
                    return Response<Worklog>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<Worklog>.CreateResponse(true, "Successfully uploaded.", ExistingId);
            }
            catch (Exception e)
            {
                return Response<Worklog>.CreateResponse(false, e.Message, null);
            }
        }





        [HttpPut("UpdateWorklog")]
        public async Task<Response<Worklog>> UpdateWorklog([FromBody] Worklog WorklogID)
        {

            if (WorklogID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);
            }

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

            if (WorklogID == null)
            {
                return Response<Worklog>.CreateResponse(false, "Please provide valid Worklog Id.", null);

            }

           
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
        public async Task<Response<IEnumerable<Worklog>>> GetProjectWorklog([FromQuery] int ProjectId, long? WorklogID, long? UserID, long? ProjectID,
           string ProjectTitle, string Feature, string FullName, string Address)

        {
            
            try
            {
                var worklog = await _worklogService.GetProjectWorklog(ProjectId,WorklogID,UserID,ProjectID,ProjectTitle,Feature,FullName,Address);
                if (worklog == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklog);
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
                var worklog = await _worklogService.GetAll(Features.ToString());
                if (worklog == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklog);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("UserID")]
        public async Task<Response<IEnumerable<Worklog>>>GetUserWorklog([FromQuery] long? WorklogID, int UserID,long? ProjectID,long? TimelogID,
        int? EstimateWorkTime, string ProjectTitle, string Feature, string FullName, string Address, DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd)
       
        {
           
            try
            {
                 var worklog = await _worklogService.GetUserWorklog(  WorklogID, UserID, ProjectID, TimelogID,
               ProjectTitle, Feature, FullName, Address, EstimateWorkTime, ActualWorkTimeStart, ActualWorkTimeEnd);

                if (worklog == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not  valid ", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklog);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
          
        }
    



        [HttpGet("GetAllWorklogs")]
        public async Task<Response<IEnumerable<Worklog>>>GetAllWorklogs([FromQuery] int start,int limit, long? WorklogID, int UserID, long? ProjectID, string Features, int? EstimateWorkTime,
        DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd, string ProjectTitle, string FullName, string Address)

        {
           
            try
            {
                var worklog = await _worklogService.GetAllWorklogs(start, limit, WorklogID, UserID, ProjectID, Features, EstimateWorkTime, ActualWorkTimeStart, ActualWorkTimeEnd, ProjectTitle, FullName, Address);

                if (worklog == null)
                {
                    return Response<IEnumerable<Worklog>>.CreateResponse(false, "Not  valid ", null);
                }

                return Response<IEnumerable<Worklog>>.CreateResponse(true, "Successfully uploaded.", worklog);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Worklog>>.CreateResponse(false, e.Message, null);
            }
          
        }
    }
}













































































































