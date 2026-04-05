using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/v{version:apiVersion}/VillaAPI")]
	[ApiController]
	[ApiVersion("2.0")]
	public class VillaAPIControllerV2 : ControllerBase
	{
		private readonly IVillaRepository _villaRepo;
		private readonly IMapper _mapper;
		protected APIResponse _response;

		public VillaAPIControllerV2(IVillaRepository villaRepo, IMapper mapper)
		{
			_villaRepo = villaRepo;
			_mapper = mapper;
			_response = new APIResponse();
		}
		[HttpGet("names")]
		public async Task<ActionResult<APIResponse>> GetVillasNames()
		{

			var names = await _villaRepo.GetVillasNamesAsync();
			if (!names.Any())
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Villas not found!");
				return NotFound(_response);
			}

			_response.StatusCode = HttpStatusCode.OK;
			_response.Result = names;
			return Ok(_response);
		}

	}
}
