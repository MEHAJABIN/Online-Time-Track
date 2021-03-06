﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTimeTrack.Models;
using OnlineTimeTrack.Services;

namespace OnlineTimeTrack.Controllers
{
    [Route("api/Timelog")]
    public class TimelogController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IWorklogService _worklogService;
        private readonly ITimelogService _timelogService;
       

        public TimelogController( IUserService userService, IWorklogService worklogService, ITimelogService timelogService)
        {
            _worklogService = worklogService;
            _userService = userService;
            _timelogService = timelogService;
           
        }




        [HttpPost]
        public async Task<Response<Timelog>> Timelog([FromBody]Timelog timelog)
        {
          
           

            if (timelog == null)
            {
                return Response<Timelog>.CreateResponse(false, "Please provide valid Timelog Id.", null);

            }

         
            try
            {
                var newTimelog = await _timelogService.Timelog(timelog);

                return Response<Timelog>.CreateResponse(true, "Successfully uploaded.",newTimelog);
            }
            catch (Exception e)
            {
                return Response<Timelog>.CreateResponse(false, e.Message, null);
            }
        }



        [HttpGet("GetById")]
        public async Task<Response<Timelog>> GetById([FromQuery]long? id)

        {
            if (id == null)
            {
                return Response<Timelog>.CreateResponse(false, "Please provide valid Timelog Id.", null);
            }

            try
            {
                var ExistingId = await _timelogService.GetById(id.GetValueOrDefault());

                if (ExistingId == null)
                {
                    return Response<Timelog>.CreateResponse(false, "Not a valid Id", null);
                }

                return Response<Timelog>.CreateResponse(true, "Successfully uploaded.", ExistingId);
            }
            catch (Exception e)
            {
                return Response<Timelog>.CreateResponse(false, e.Message, null);
            }
        }




       [HttpPut("UpdateTimelog")]
        public async Task<Response<Timelog>> UpdateTimelog([FromBody] Timelog TimelogID)
        {
           
            if (TimelogID == null)
            {
                return Response<Timelog>.CreateResponse(false, "Please provide valid Timelog Id.", null);
            }
            try
            {
                var ExistingTimelog = await _timelogService.UpdateTimelog(TimelogID);

                if (ExistingTimelog == null)
                {
                    return Response<Timelog>.CreateResponse(false, "Not a valid Id", null);
                }
                return Response<Timelog>.CreateResponse(true, "Successfully updated.", ExistingTimelog);

            }
            catch (Exception e)
            {
                return Response<Timelog>.CreateResponse(false, e.Message, null);
            }
        }

    }
}







