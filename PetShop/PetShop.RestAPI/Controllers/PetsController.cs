using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetShop.Core.ApplicationService;
using PetShop.Core.Entity;
using PetShop.Infrastructure.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IOwnerService _ownerService;
        private readonly IPetTypeService _petTypesService;

        public PetsController(IPetService petService, IOwnerService ownerService, IPetTypeService petTypeService)
        {
            _petService = petService;
            _ownerService = ownerService;
            _petTypesService = petTypeService;
        }

        /// <summary>
        /// Retrieves a list of all pets
        /// </summary>
        /// <returns>List of all pets</returns>
        [HttpGet] //for example api/pets?direction=asc&field=price&term=1500
        public ActionResult<IEnumerable<Pet>> Get([FromQuery] Filter filter)
        {
            List<Pet> pets = new List<Pet>();
            try
            {
                if (string.IsNullOrEmpty(filter.Direction) || string.IsNullOrEmpty(filter.Field) || string.IsNullOrEmpty(filter.Term))
                {
                    pets = _petService.GetAllPets();
                }
                else
                {
                    pets = _petService.GetAllPets(filter);
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

            if (pets.Count == 0)
            {
                return StatusCode(404, "Did not find any entries");
            }
            else
            {
                return Ok(pets);
            }
                                    
        }

        // GET api/<PetsController>/5
        [HttpGet("{id}")]
        public ActionResult<Pet> Get(int id)
        {
            try
            {
                var pet = _petService.GetPetById(id);
                if (pet == null)
                {
                    return StatusCode(404, $"Did not find pet with ID {id}");
                }
                else
                {
                    return Ok(pet);
                }
            }
            catch (InvalidDataException e) //if ID is lower than 1
            {
               return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }

        }

        // POST api/<PetsController>
        [HttpPost]
        public ActionResult<Pet> Post([FromBody] Pet pet)
        {
            try
            {
                return StatusCode(201, _petService.CreatePet(pet));
            }
            catch (ArgumentException e) //exceptions in ValidatePet
            {
                return StatusCode(500, e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionMessageConstants.NonSpecificUIMessage);
            }

        }

        // PUT api/<PetsController>/5
        [HttpPut("{id}")]
        public ActionResult<Pet> Put(int id, [FromBody] Pet pet)
        {
            if (id < 1 || id != pet.PetId)
            {
                return StatusCode(500, "Parameter ID and pet ID must be identical");
            }

            try
            {
                pet = _petService.EditPet(pet);
                if (pet == null)
                {
                    return StatusCode(404, $"Did not find pet with ID {id}");
                }
                else
                {
                    return StatusCode(202, pet);
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

        // DELETE api/<PetsController>/5
        [HttpDelete("{id}")]
        public ActionResult<Pet> Delete(int id)
        {
            try
            {
                var pet = _petService.DeletePet(id);
                if (pet == null)
                {
                    return StatusCode(404, $"Did not find pet with ID {id}");
                }
                else
                {
                    return Ok($"Pet:\n{pet}\n\n...was successfully deleted");
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
