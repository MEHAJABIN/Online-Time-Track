using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OnlineTimeTrack.Contexts;
using Microsoft.Extensions.Options;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;
using Project = OnlineTimeTrack.Models.Project;



namespace OnlineTimeTrack.Controllers
{
    [("api/[Project]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        readonly _projectService ProjectService;

        [HttpPost]
        public async Task<Response<Project>> Project([FromBody] Project project)
        {

            if (project == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid Project Id.", null);
            }

            try
            {

                var newProject = await _projectService.P(project);
                return Response<Project>.CreateResponse(true, "Successfully registered.", newProject);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }



        }
    }
}












        /* try
         {
             if (project.ProjectID == 0)
                 if (project.ProjectID ==)
                 {
                     project.ProjectID = ();
                     _onlineTimeTrackContext.project.Add(project);

                     await _onlineTimeTrackContext.SaveChangesAsync();
                 }
         }*/





















            /*if (project == null)
            {
                return Response<Project>.CreateResponse(false, "Please provide valid project title", null);
            }

            try
            {
                var newProject = await _projectService.ProjectTitle(projectID);
                return Response<User>.CreateResponse(true, "Successfully Added.", newProject);
            }
            catch (Exception e)
            {
                return Response<Project>.CreateResponse(false, e.Message, null);
            }*/





/* [HttpGet]
 public IEnumerable<long> (ProjectID)
 {
     return new long[] {ProjectId};
 }

 [HttpGet]
 public  Value(int id)
 {
     return "value";
 }

 [HttpPost]
 public void SaveNewValue([FromBody]string value)
 {
 }

 [HttpPut]
 public void UpdateValue(int id, [FromBody]string value)
 {
 }

 [HttpDelete]
 public void RemoveValue(int id)
 {
 }*/



