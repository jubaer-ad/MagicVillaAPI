using AutoMapper;
using MagicVillaAPI.Data;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVillaAPI.Controllers.v1
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("1.0")]
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
        protected APIResponse _response;

        public VillaAPIController(ILoggingCustom logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(CacheProfileName = "Default30")]
        //[ResponseCache(Duration = 30)]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            _logger.Log("Getting all Villas", "inf");
            try
            {
                _response.Result = await _dbVilla.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ResponseCache(CacheProfileName = "Default30")]
		public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Get Villa error with id: " + id, "err");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Get Villa error with id: " + id };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No record is found!" };
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "aladin")]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(v => v.Name == villaCreateDTO.Name) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa already Exists");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = false;
                    IEnumerable<string> allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(r => r.ErrorMessage));
                    _response.ErrorMessages = allErrors.ToList();
                    return BadRequest(_response);
                }
                if (villaCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = false;
                    _response.ErrorMessages = new List<string> { "Nothing to create! Check payload of request." };
                    return BadRequest(_response);
                }

                Villa villa = _mapper.Map<Villa>(villaCreateDTO);
                villa.CreatedAt = DateTime.Now;
                await _dbVilla.CreateAsync(villa);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.IsSucces = true;

                var postedVilla = await _dbVilla.GetLastAsync();

                return CreatedAtRoute("GetVilla", new { id = postedVilla.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "aladin")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = false;
                    _response.ErrorMessages = new List<string> { "Update failed! Check payload of request." };
                    return BadRequest(_response);
                }

                Villa? existingVilla = await _dbVilla.GetAsync(v => v.Id == id, false);

                if (existingVilla != null)
                {
                    Villa villa = _mapper.Map<Villa>(villaUpdateDTO);
                    villa.CreatedAt = existingVilla.CreatedAt;
                    villa.UpdatedAt = DateTime.Now;
                    await _dbVilla.UpdateAsync(villa);

                    _response.StatusCode = HttpStatusCode.Created;
                    _response.IsSucces = true;
                    _response.Result = villa;

                    return CreatedAtRoute("GetVilla", new { id }, _response);
                }
                return BadRequest("Record by Id not found.");
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
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
        [Authorize(Roles = "aladin")]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "anubis")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = true;
                    _response.ErrorMessages = new List<string> { "Id can not be 0!)" };
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(v => v.Id == id, false);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSucces = true;
                    _response.ErrorMessages = new List<string> { "Record doesn't exist with id: " + id };
                    return NotFound(_response);
                }
                await _dbVilla.RemoveAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;
                _response.Result = villa;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
