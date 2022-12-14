using AutoMapper;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net;

namespace MagicVillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly ILoggingCustom _logger;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IVillaRepository _villaRepository;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, ILoggingCustom logger, IMapper mapper, IVillaRepository villaRepository)
        {
            _dbVillaNumber = dbVillaNumber;
            _logger = logger;
            _response = new();
            this._mapper = mapper;
            _villaRepository = villaRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        [Authorize(Roles = "aladin")]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(vn => vn.VillaNo == villaNumberCreateDTO.VillaNo) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Villa Number already exists!" };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                if (await _villaRepository.GetAsync(v => v.Id == villaNumberCreateDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Villa doesn't exist!" };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                if (villaNumberCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = false;
                    _response.ErrorMessages = new List<string> { "Nothing to create! Check payload of request." };
                    return BadRequest(_response);
                }
                if (villaNumberCreateDTO.VillaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Villa Number can not be zero!" };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
                villaNumber.CreatedAt = DateTime.Now;
                await _dbVillaNumber.CreateAsync(villaNumber);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.IsSucces = true;

                var postedVillaNumber = await _dbVillaNumber.GetLastAsync();
                return CreatedAtRoute("GetVillaNumber", new { villaNo = postedVillaNumber.VillaNo }, _response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        //[MapToApiVersion("1.0")]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.Log("Getting all Villa Numbers", "inf");
                _response.Result = await _dbVillaNumber.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _logger.Log("Get Villa error with id: " + villaNo, "err");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Get Villa Number error with villaNumber: " + villaNo };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(vn => vn.VillaNo == villaNo);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "No record is found!" };
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        [Authorize(Roles = "aladin")]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber([FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO, int villaNo)
        {
            try
            {
                if (villaNo != villaNumberUpdateDTO.VillaNo || villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Bad payload!" };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                var existingVillaNumber = await _dbVillaNumber.GetAsync(vn => vn.VillaNo == villaNo, false);
                if (existingVillaNumber != null)
                {
                    VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                    villaNumber.UpdatedAt = DateTime.Now;
                    await _dbVillaNumber.UpdateAsync(villaNumber);

                    _response.StatusCode = HttpStatusCode.Created;
                    _response.Result = villaNumber;
                    return CreatedAtAction("GetVillaNumber", new { villaNo }, _response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
            _response.IsSucces = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            _response.ErrorMessages = new List<string> { "Not found" };
            return NotFound(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [Authorize(Roles = "anubis")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _logger.Log("Delete Villa error with id: " + villaNo, "err");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Delete Villa Number error with villaNumber: " + villaNo };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(vn => vn.VillaNo == villaNo);
                if (villaNumber != null)
                {
                    await _dbVillaNumber.RemoveAsync(villaNumber);
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSucces = true;
                    _response.Result = villaNumber;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPatch]
        [Authorize(Roles = "aladin")]
        public async Task<ActionResult<APIResponse>> PartialUpdate(int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> patch)
        {
            try
            {
                if (patch == null || villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Bad payload!" };
                    _response.IsSucces = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(vn => vn.VillaNo == villaNo, false);
                if (villaNumber != null)
                {
                    VillaNumberUpdateDTO villaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);
                    patch.ApplyTo(villaNumberUpdateDTO);
                    if (villaNumberUpdateDTO.VillaNo != villaNo)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages = new List<string> { "Villa No can not be updated!" };
                        _response.IsSucces = false;
                        return BadRequest(_response);
                    }
                    var updatedVillaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                    updatedVillaNumber.CreatedAt = villaNumber.CreatedAt;
                    updatedVillaNumber.UpdatedAt = DateTime.Now;
                    await _dbVillaNumber.UpdateAsync(updatedVillaNumber);
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.Result = updatedVillaNumber;
                    return CreatedAtAction("GetVillaNumber", new { villaNo = updatedVillaNumber.VillaNo }, _response);
                }
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSucces = false;
            return BadRequest(_response);
        }
    }
}
