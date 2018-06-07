using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SupportWheel.Api.Models;
using SupportWheel.Api.Services;

namespace SupportWheel.Api.Controllers
{
    [Route("api/Engineer")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IEngineerService _service;

        public EngineerController(IEngineerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Engineer>> GetAll()
        {
            return Ok(_service.GetAll().ToList());
        }

        [HttpGet("{id}", Name = "GetEngineer")]
        public ActionResult<Engineer> GetById(long id)
        {
            var engineer = _service.GetById(id);
            if (engineer == null)
            {
                return NotFound();
            }
            return engineer;
        }
        
        [HttpPost]
        public IActionResult Create(Engineer engineer)
        {
            _service.Create(engineer);

            return CreatedAtRoute("GetEngineer", new { id = engineer.Id }, engineer);
        }

        [HttpPut]
        public IActionResult Update(Engineer engineer)
        {
            _service.Update(engineer);            
            return Ok(engineer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}