using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;
using OnlineTimeTrack.Models.Data_Manager;

namespace OnlineTimeTrack.Controllers
{
    
    [Route("api/Project")]

    public class ProjectController : ControllerBase
    {
        private IProjectService _projectService;

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
                return Response<Project>.CreateResponse(true, "Successfully uploaded.", null);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(true, e.Message, null);
            }
        }
            
            
        



      /*  [HttpGet("Project")] 
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









    