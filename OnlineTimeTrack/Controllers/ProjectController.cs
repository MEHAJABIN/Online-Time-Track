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
                return Response<Project>.CreateResponse(true, "Successfully uploaded.", null);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(true, e.Message, null);
            }
        }
            
         

 





       // [HttpPut("Project")]


       




       //  [HttpDelete("Project")]



     

    }
}









    