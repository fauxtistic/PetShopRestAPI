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
    public class PetTypesController : ControllerBase
    {
        private readonly IPetTypeService _petTypesService;

        public PetTypesController(IPetTypeService petTypeService)
        {
            _petTypesService = petTypeService;
        }

        // GET: api/<PetTypesController>
        [HttpGet]
        public ActionResult<IEnumerable<PetType>> Get([FromQuery] Filter filter)
        {
            List<PetType> types = new List<PetType>();
            try
            {
                if (string.IsNullOrEmpty(filter.Direction) || string.IsNullOrEmpty(filter.Field) || string.IsNullOrEmpty(filter.Term))
                {
                    types = _petTypesService.GetAllPetTypes();
                }
                else
                {
                    types = _petTypesService.GetAllPetTypes(filter);
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

            if (types.Count == 0)
            {
                return StatusCode(404, "Did not find any entries");
            }
            else
            {
                return Ok(types);
            }
        }

        // GET api/<PetTypesController>/5
        [HttpGet("{id}")]
        public ActionResult<PetType> Get(int id)
        {
            try
            {
                var petType = _petTypesService.GetPetTypeById(id);
                if (petType == null)
                {
                    return StatusCode(404, $"Did not find pet type with ID {id}");
                }
                else
                {
                    return Ok(petType);
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

        // POST api/<PetTypesController>
        [HttpPost]
        public ActionResult<PetType> Post([FromBody] PetType petType)
        {
            try
            {                
                return StatusCode(201, _petTypesService.CreatePetType(petType));
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

        // PUT api/<PetTypesController>/5
        [HttpPut("{id}")]
        public ActionResult<PetType> Put(int id, [FromBody] PetType petType)
        {
            if (id < 1 || id != petType.PetTypeId)
            {
                return StatusCode(500, "Parameter ID and pet type ID must be identical");
            }

            try
            {
                petType = _petTypesService.EditPetType(petType);
                if (petType == null)
                {
                    return StatusCode(404, $"Did not find pet type with ID {id}");
                }
                else
                {
                    return StatusCode(202, petType);
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

        // DELETE api/<PetTypesController>/5
        [HttpDelete("{id}")]
        public ActionResult<PetType> Delete(int id)
        {
            try
            {
                var petType = _petTypesService.DeletePetType(id);
                if (petType == null)
                {
                    return StatusCode(404, $"Did not find pet type with ID {id}");
                }
                else 
                {
                    return StatusCode(202, $"Pet type:\n{petType}\n\n...was successfully deleted");
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
