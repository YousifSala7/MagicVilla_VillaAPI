using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/v{version:apiVersion}/VillaAPI")]
	[ApiController]
	[ApiVersion("1.0")]
	public class VillaAPIController : ControllerBase
	{
		private readonly IVillaRepository _villaRepo;
		private readonly IMapper _mapper;
		protected APIResponse _response;

		public VillaAPIController(IVillaRepository villaRepo, IMapper mapper)
		{
			_villaRepo = villaRepo;
			_mapper = mapper;
			_response = new APIResponse();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<APIResponse>> GetVillas()
		{
			var villaList = await _villaRepo.GetAllAsync();

			_response.Result = _mapper.Map<List<VillaDTO>>(villaList);
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}

		[HttpGet("{id:int}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetVilla(int id)
		{
			if (id == 0)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { "Bad Request of Villa number" };
				return BadRequest(_response);
			}

			var villa = await _villaRepo.GetAsync(v => v.Id == id);

			if (villa == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { "Villa Not Found!" };
				return NotFound(_response);
			}

			_response.Result = _mapper.Map<VillaDTO>(villa);
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> CreateVilla
			([FromBody] VillaCreateDTO createDTO)
		{
			if (createDTO == null)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>() { "Data is empty!" };
				return BadRequest(_response);
			}

			bool villaExists = await _villaRepo.AnyAsync(v => v.Name == createDTO.Name);

			if (villaExists)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>() { "The villa's name is already exists!" };
				return BadRequest(_response);
			}

			var model = _mapper.Map<Villa>(createDTO);
			model.CreatedDate = DateTime.Now;

			await _villaRepo.CreateAsync(model);

			_response.Result = _mapper.Map<VillaDTO>(model);
			_response.StatusCode = HttpStatusCode.Created;

			return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
		}

		[Authorize]
		[HttpDelete("{id:int}", Name = "DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
		{

			if (id == 0)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>() { "Id not Found" };
				return BadRequest(_response);
			}

			var villa = await _villaRepo.GetAsync(v => v.Id == id);
			if (villa == null)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages = new List<string>() { "Villa not Found!" };
				return NotFound(_response);
			}

			await _villaRepo.RemoveAsync(villa);

			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;
			return Ok(_response);
		}

		[Authorize]
		[HttpPut("{id:int}", Name = "UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateVilla
			(int id, [FromBody] VillaUpdateDTO updateDTO)
		{
			if (updateDTO == null || id != updateDTO.Id)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>() { "Invalid Id" };
				return BadRequest(_response);
			}

			var model = _mapper.Map<Villa>(updateDTO);

			await _villaRepo.UpdateAsync(model);

			_response.StatusCode = HttpStatusCode.NoContent;
			_response.IsSuccess = true;
			return Ok(_response);
		}
	}
}



