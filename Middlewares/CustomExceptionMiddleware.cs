using MagicVilla_VillaAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace MagicVilla_VillaAPI.Middlewares
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandlexExceptionAsync(context, ex);
			}
		}

		public static Task HandlexExceptionAsync(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json";

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			var response = new APIResponse
			{
				IsSuccess = false,
				StatusCode = HttpStatusCode.InternalServerError,
				ErrorMessages = new List<string> { ex.Message }
			};

			var jsonResponse = JsonConvert.SerializeObject(response);
			return context.Response.WriteAsync(jsonResponse);
		}
	}
}
