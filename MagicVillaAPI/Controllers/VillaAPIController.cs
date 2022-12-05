using AutoMapper;
using MagicVillaAPI.Data;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        /*
         * The following code is used to used default logger and Serilog
         * 
        private readonly ILogger<VillaAPIController> _logger;

        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }

        */

        
        private readonly IVillaRepository _dbVilla;

        /*
         * The following code is for using custom logger named interface ILoggingCustom
         */
        private readonly ILoggingCustom _logger;
        private readonly IMapper _mapper;

        public VillaAPIController(ILoggingCustom logger, IVillaRepository dbVilla, IMapper mapper)
        {
            this._logger = logger;
            this._dbVilla = dbVilla;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.Log("Getting all Villas", "inf");
            // return Ok(VillaStore.villaList);
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get Villa error with id: " + id, "err");
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id);
            if (villa == null) return NotFound();

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            //if (ModelState.IsValid) return BadRequest(ModelState);

            //if (VillaStore.villaList.FirstOrDefault(v => v.Name == villa.Name) != null)
            if (await _dbVilla.GetAsync(v => v.Name == villaCreateDTO.Name) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists");
                return BadRequest(ModelState);
            }
            if (villaCreateDTO == null) return BadRequest();

            Villa model = _mapper.Map<Villa>(villaCreateDTO);
            model.CreatedAt = DateTime.Now;

            await _dbVilla.CreateAsync(model);
            //var test = _db.Villas.FromSql("");
            var postedVilla = await _dbVilla.GetLastAsync();

            return CreatedAtRoute("GetVilla", new { id = postedVilla.Id }, postedVilla);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            if (villaUpdateDTO == null || id != villaUpdateDTO.Id) return BadRequest();

            Villa? existingVilla = await _dbVilla.GetAsync(v => v.Id == id, false);

            if (existingVilla != null)
            {
                Villa model = _mapper.Map<Villa>(villaUpdateDTO);
                model.CreatedAt = existingVilla.CreatedAt;
                model.UpdatedAt = DateTime.Now;
                await _dbVilla.UpdateAsync(model);

                return CreatedAtRoute("GetVilla", new { id = id }, model);
            }
            return BadRequest("Record by Id not found.");
        }

        /*
         * For Patch support we need to install two more NuGet with same .Net core version
         * 1. Microsoft.AspNetCore.JsonPatch
         * 2. Microsoft.AspNetCore.Mvc.NewtonsoftJson
         * 
         * For more information refer to https://jsonpatch.com/
         * */
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbVilla.GetAsync(v => v.Id == id, false);
            if (villa == null) return NotFound();
            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(villaUpdateDTO, ModelState);

            if (!ModelState.IsValid) return BadRequest();
            Villa model = _mapper.Map<Villa>(villaUpdateDTO);
            model.CreatedAt = villa.CreatedAt;
            model.UpdatedAt = DateTime.Now;

            await _dbVilla.UpdateAsync(model);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _dbVilla.GetAsync(v => v.Id == id, false);
            if (villa == null) return NotFound();
            await _dbVilla.RemoveAsync(villa);

            return NoContent();
        }
    } 
}
