using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] //Get : /api/Buggy/notfount
        public IActionResult GetNotFoundResponse()
        {
            return NotFound();
        }

        [HttpGet("badrequest")] //Get : /api/Buggy/badrequest
        public IActionResult GetBadRequestResponse()
        {
            return BadRequest();
        }

        [HttpGet("badrequest/{id}")] //Get : /api/Buggy/badrequest
        public IActionResult GetBadRequestResponse(int id)
        {
            return BadRequest();
        }

        [HttpGet("servererror")] //Get : /api/Buggy/badrequest
        public IActionResult GetServerErrorResponse()
        {
            throw new Exception();
            return BadRequest();
        }

        [HttpGet("unauthorized")] //Get : /api/Buggy/unauthorized
        public IActionResult GetUnauthorizedResponse()
        {
            return Unauthorized();
        }
    }
}
