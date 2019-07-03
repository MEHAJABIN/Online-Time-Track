using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OnlineTimeTrack.Controllers
{
    [Authorize]
    [Route("api/Project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
       
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;

            
        }

        
        [HttpPost]
        public async Task<Response<Project>> Project([FromBody]Project project)
        {
            
            if (project  == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid Project Id.",null);
            }
            try
            {
                var newProject= await _projectService.Project(project);
                return Response<Project>.CreateResponse(true, "Successfully uploaded.", newProject);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }
        }





        [HttpGet("GetById")]
        public async Task<Response<Project>> GetById([FromQuery]long? id)

        {
            if (id == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid Project Id.", null);
            }

            try
            {
                var ExistingId = await _projectService.GetById(id.GetValueOrDefault());
                if (ExistingId == null)
                {
                    return Response<Project>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<Project>.CreateResponse(true, "Successfully loaded.", ExistingId);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }
        }




        [HttpPut("UpdateProject")]

        public async Task<Response<Project>> UpdateProject([FromBody] Project ProjectID)

        {
            if (ProjectID == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid Project Id.", null);

            }
            try
            {
                var ExistingProject = await _projectService.UpdateProject(ProjectID);
                if (ExistingProject == null)
                {
                    return Response<Project>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<Project>.CreateResponse(true, "Successfully updated.", ExistingProject);

            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }
        }


        [HttpDelete("DeleteProject")]
        public async Task<Response<Project>> DeleteProject([FromBody] Project ProjectID)

        {
            if (ProjectID == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid Project Id.", null);

            }
            try
            {
                var ExistingProject= await _projectService.DeleteProject(ProjectID);
                if (ExistingProject == null)
                {
                    return Response<Project>.CreateResponse(false, "Not a valid Project Id", null);
                }

                return Response<Project>.CreateResponse(true, "Successfully deleted.", ExistingProject);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }
        }





        [HttpGet("GetAllProjects")]
        public async Task<Response<IEnumerable<Project>>> GetAllProjects([FromQuery] int start, int limit)
        {

            try
            {
                var projects = await _projectService.GetAllProjects(start, limit);

                if (projects == null)
                {
                    return Response<IEnumerable<Project>>.CreateResponse(false, "Not  valid ", null);
                }
                return Response<IEnumerable<Project>>.CreateResponse(true, "Successfully loaded", projects);

                
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Project>>.CreateResponse(false, e.Message, null);
            }

        }
    }
}





















