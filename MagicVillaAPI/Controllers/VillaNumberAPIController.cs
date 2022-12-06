using AutoMapper;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Logging;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MagicVillaAPI.Controllers
{
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly ILoggingCustom _logger;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, ILoggingCustom logger, IMapper _mapper)
        {
            this._dbVillaNumber = dbVillaNumber;
            this._logger = logger;
            this._response = new();
            this._mapper = _mapper;
        }
        [HttpPost]
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
                if (villaNumberCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSucces = false;
                    _response.ErrorMessages = new List<string> { "Nothing to create! Check payload of request." };
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
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        

            
        [HttpGet]
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
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

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
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


    }
}
