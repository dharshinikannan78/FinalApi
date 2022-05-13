using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationApllication.Data;
using RegistrationApllication.Modal;
using System.Collections.Generic;
using System.Linq;

namespace RegistrationApllication.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        public readonly UserDbContext dataModel;
        public RegistrationController(UserDbContext userData)
        {
            dataModel = userData;

        }

        [HttpGet("AllCandidates")]
        //[Authorize] 
        public IActionResult GetCandidateDetails(string status)

        {
            List<RegistrationModelClass> EmployeeDetails = new List<RegistrationModelClass>();

            if (status == null)
            {
               
                EmployeeDetails = dataModel.RegistrationDetail.Where(x => x.status == null).Select(x => x).
                   ToList();
                return Ok(EmployeeDetails);
            }
            else if (status == "archived")
            {
                 EmployeeDetails = dataModel.RegistrationDetail.Where(x => x.status == "archived").Select(x => x).
                   ToList();
                
            }
            else if (status == "active")
            {
                EmployeeDetails = dataModel.RegistrationDetail.Where(x => x.status == "active").
                   Select(x => x).ToList();
            }
            else
            {
               return NotFound();
            }

            return Ok(EmployeeDetails);
           
          
        }
      
        [HttpPost("Registration")]
        
        public IActionResult RegistrationCandidateForm([FromBody] RegistrationModelClass obj)
        {
            obj.ApplicantStatus = "Open";
            dataModel.RegistrationDetail.Add(obj);
            dataModel.SaveChanges();
            return Ok(obj);
        }

        [HttpPut("updateCandidateDetails")]
        
        public IActionResult updateCandidateDetails([FromBody] RegistrationModelClass obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var status = dataModel.RegistrationDetail.AsNoTracking().FirstOrDefault(x => x.CandidateId == obj.CandidateId);
            if (status == null)
            {
                return BadRequest();
            }
            else
            {
                dataModel.Entry(obj).State = EntityState.Modified;
                dataModel.SaveChanges();
                return Ok(obj);
            }

        }

        [HttpGet("Email")]
       
        public IActionResult getEmail(string obj)
        {
            var CandidateEmail = dataModel.RegistrationDetail.Where(a => a.EmailId == obj).FirstOrDefault();

            if (CandidateEmail == null)
            {
                return Ok(new
                {
                    message = "You Can Enter"
                }); ;
            }
            else
            {
                return Ok(new
                {
                    message = "already Exist"
                });
            }
        }

        [HttpGet("applicant")]
        
        public IActionResult applicantStatus(int obj)
        {
            var appl = dataModel.RegistrationDetail.Where(a => a.CandidateId == obj).FirstOrDefault();
            if (appl == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(appl);
            }
        }

        [HttpDelete("delete")]
        
        public IActionResult deleteCandidate(int id)
        {
            var deleteCandidateDetails = dataModel.RegistrationDetail.Find(id);
            if (deleteCandidateDetails == null)
            {
                return NotFound();
            }
            else
            {
                dataModel.RegistrationDetail.Remove(deleteCandidateDetails);
                dataModel.SaveChanges();
                return Ok();
            }
        }
    }
}
