using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaAPIControllerV2 : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIControllerV2(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new APIResponse();
        }
        [HttpGet("names")]
        public async Task<ActionResult<APIResponse>> GetVillasNames()
        {
            try
            {
                var names = await _db.Villas.Select(v => v.Name).ToListAsync();
                
                if(names == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Villas not found!");
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = names;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
