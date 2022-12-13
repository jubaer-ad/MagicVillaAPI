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

namespace MagicVillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly ILoggingCustom _logger;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private readonly IVillaRepository _villaRepository;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, ILoggingCustom logger, IMapper _mapper, IVillaRepository villaRepository)
        {
            _dbVillaNumber = dbVillaNumber;
            _logger = logger;
            _response = new();
            this._mapper = _mapper;
            _villaRepository = villaRepository;
        }    

        [HttpGet]
        //[MapToApiVersion("2.0")]
        public (string, string, int, char) GetVNs()
        {
            var holder = ("key", "value", 6, 'c');
            Console.WriteLine(holder.GetType());
            return holder;
        }
    }
}
