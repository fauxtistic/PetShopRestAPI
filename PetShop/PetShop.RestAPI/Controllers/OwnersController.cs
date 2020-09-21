using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IOwnerService _ownerService;

        public OwnersController(IPetService petService, IOwnerService ownerService)
        {
            _petService = petService;
            _ownerService = ownerService;
        }

        // GET: api/<OwnersController>
        [HttpGet]
        public ActionResult<IEnumerable<Owner>> Get([FromQuery] Filter filter)
        {
            List<Owner> owners = new List<Owner>();
            try
            {
                if (string.IsNullOrEmpty(filter.Direction) || string.IsNullOrEmpty(filter.Field) || string.IsNullOrEmpty(filter.Term))
                {
                    owners = _ownerService.GetAllOwners();
                }
                else
                {
                    owners = _ownerService.GetAllOwners(filter);
                }
            }
            catch (InvalidDataException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }

            if (owners.Count == 0)
            {
                return StatusCode(404, "Did not find any entries");
            }
            else
            {
                return Ok(owners);
            }
        }

        // GET api/<OwnersController>/5
        [HttpGet("{id}")]
        public ActionResult<Owner> Get(int id)
        {
            try
            {
                var owner = _ownerService.GetOwnerById(id);
                if (owner == null)
                {
                    return StatusCode(404, $"Did not find owner with ID {id}");
                }
                else
                {
                    return Ok(owner);
                }
            }
            catch (InvalidDataException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }
        }

        // POST api/<OwnersController>
        [HttpPost]
        public ActionResult<Owner> Post([FromBody] Owner owner)
        {
            try
            {
                return StatusCode(201, _ownerService.CreateOwner(owner));
            }
            catch (ArgumentException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }
        }

        // PUT api/<OwnersController>/5
        [HttpPut("{id}")]
        public ActionResult<Owner> Put(int id, [FromBody] Owner owner)
        {
            if (id < 1 || id != owner.OwnerId)
            {
                return StatusCode(500, "Parameter ID and pet ID must be identical");
            }

            try
            {
                owner = _ownerService.EditOwner(owner);
                if (owner == null)
                {
                    return StatusCode(404, $"Did not find owner with ID {id}");
                }
                else
                {
                    return StatusCode(202, owner);
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }
        }

        // DELETE api/<OwnersController>/5
        [HttpDelete("{id}")]
        public ActionResult<Owner> Delete(int id)
        {
            try
            {
                var owner = _ownerService.DeleteOwner(id);
                if (owner == null)
                {
                    return StatusCode(404, $"Did not find owner with ID {id}");
                }
                else
                {
                    return StatusCode(202, $"Owner:\n{owner}\n\n...was successfully deleted");
                }
            }
            catch (InvalidDataException e)
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }

        }
    }
}
