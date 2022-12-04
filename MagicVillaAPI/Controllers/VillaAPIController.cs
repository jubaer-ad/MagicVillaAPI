using AutoMapper;
using MagicVillaAPI.Data;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
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

        
        private readonly VillaDBContext _db;

        /*
         * The following code is for using custom logger named interface ILoggingCustom
         */
        private readonly ILoggingCustom _logger;
        private readonly IMapper _mapper;

        public VillaAPIController(ILoggingCustom logger, VillaDBContext db, IMapper mapper)
        {
            this._logger = logger;
            this._db = db;
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
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
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
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
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
            if (await _db.Villas.FirstOrDefaultAsync(v => v.Name == villaCreateDTO.Name) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists");
                return BadRequest(ModelState);
            }
            if (villaCreateDTO == null) return BadRequest();
            //if (villa.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);
            //villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villa);

            Villa model = _mapper.Map<Villa>(villaCreateDTO);
            model.CreatedAt = DateTime.Now;

            //Villa model = new Villa()
            //{
            //    Name = villaCreateDTO.Name,
            //    Details = villaCreateDTO.Details,
            //    Rate = villaCreateDTO.Rate,
            //    Occupency = villaCreateDTO.Occupency,
            //    Sqft = villaCreateDTO.Sqft,
            //    CreatedAt = DateTime.Now,
            //    ImageUrl = villaCreateDTO.ImageUrl,
            //    Amenity = villaCreateDTO.Amenity
            //};
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            //var test = _db.Villas.FromSql("");
            var postedVilla = await _db.Villas.AsNoTracking().OrderBy(v => v.Id).LastAsync();

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

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //if (villa == null) return NotFound();
            //villa.Name = villaDTO.Name;
            //villa.Occupency = villaDTO.Occupency;
            //villa.Sqft= villaDTO.Sqft;
            Villa? existingVilla = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (existingVilla != null)
            {
                Villa model = _mapper.Map<Villa>(villaUpdateDTO);
                model.CreatedAt = existingVilla.CreatedAt;
                model.UpdatedAt = DateTime.Now;

                //Villa model = new Villa()
                //{
                //    Id = villaUpdateDTO.Id,
                //    Name = villaUpdateDTO.Name,
                //    Details = villaUpdateDTO.Details,
                //    Rate = villaUpdateDTO.Rate,
                //    Occupency = villaUpdateDTO.Occupency,
                //    Sqft = villaUpdateDTO.Sqft,
                //    ImageUrl = villaUpdateDTO.ImageUrl,
                //    Amenity = villaUpdateDTO.Amenity,
                //    CreatedAt = existingVilla.CreatedAt,
                //    UpdatedAt = DateTime.Now
                //};
                _db.Villas.Update(model);
                await _db.SaveChangesAsync();
                return CreatedAtRoute("GetVilla", new { id = id }, model);
            }
            return NotFound();
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
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return NotFound();
            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);

            //VillaUpdateDTO villaUpdateDTO = new()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Details = villa.Details,
            //    Rate = villa.Rate,
            //    Occupency = villa.Occupency,
            //    Sqft = villa.Sqft,
            //    ImageUrl = villa.ImageUrl,
            //    Amenity = villa.Amenity
            //};
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);

            if (!ModelState.IsValid) return BadRequest();
            Villa model = _mapper.Map<Villa>(villaUpdateDTO);
            model.CreatedAt = villa.CreatedAt;
            model.UpdatedAt = DateTime.Now;

            //Villa model = new Villa()
            //{
            //    Id = villaUpdateDTO.Id,
            //    Name = villaUpdateDTO.Name,
            //    Details = villaUpdateDTO.Details,
            //    Rate = villaUpdateDTO.Rate,
            //    Occupency = villaUpdateDTO.Occupency,
            //    Sqft = villaUpdateDTO.Sqft,
            //    ImageUrl = villaUpdateDTO.ImageUrl,
            //    Amenity = villaUpdateDTO.Amenity,
            //    CreatedAt = villa.CreatedAt,
            //    UpdatedAt = DateTime.Now
            //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return NotFound();
            //VillaStore.villaList.Remove(villa);
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    } 
}
