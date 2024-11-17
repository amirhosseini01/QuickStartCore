using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Server.Core.Commons;

public static class ResponseBase
{
	private static readonly JsonSerializerOptions CustomJsonSerializerOptions = new()
	{
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	public static JsonResult ReturnJson(object obj, int? statusCode = null)
	{
		return new JsonResult(value: obj)
		{
			SerializerSettings = CustomJsonSerializerOptions,
			StatusCode = statusCode
		};
	}
	
	public static JsonResult ReturnJsonFail<T>(string message, int? statusCode = null)
	{
		return ReturnJson(obj: Failed<T>(message), statusCode: statusCode);
	}
	
	public static JsonResult ReturnJsonFail<T>(List<string> message, int? statusCode = null)
	{
		return ReturnJson(obj: Failed<T>(message), statusCode: statusCode);
	}
	
	public static JsonResult ReturnJsonNotFound<T>()
	{
		return ReturnJsonFail<T>(message: Messages.NotFound, statusCode: StatusCodes.Status404NotFound);
	}
	
	public static JsonResult ReturnJsonInvalidData<T>(ModelStateDictionary modelState)
	{
		return ReturnJsonFail<T>(message: modelState.GetModeStateErrors(), statusCode: StatusCodes.Status400BadRequest);
	}
	
	public static JsonResult ReturnJsonSuccess<T>(T? obj = default)
	{
		return ReturnJson(obj: Success(obj), statusCode: StatusCodes.Status200OK);
	}

	public static ResponseDto<T> Failed<T>(string message, T? obj = default)
	{
		return FailResponse<T>(messages: [message], obj: obj);
	}

	public static ResponseDto<T> Failed<T>(List<string> message, T? obj = default)
	{
		return FailResponse<T>(messages: message, obj: obj);
	}

	public static ResponseDto<T> Success<T>(T? obj = default)
	{
		return SuccessResponse<T>(obj: obj);
	}

	public static ResponseDto<T> Success<T>(string message, T? obj = default)
	{
		return SuccessResponse<T>(messages: [message], obj: obj);
	}

	public static ResponseDto<T> Success<T>(List<string> message, T? obj = default)
	{
		return SuccessResponse<T>(messages: message, obj: obj);
	}

	private static ResponseDto<T> FailResponse<T>(List<string>? messages = null, T? obj = default)
	{
		return new() { IsFailed = true, IsSuccess = false, Messages = messages, Obj = obj };
	}

	private static ResponseDto<T> SuccessResponse<T>(List<string>? messages = null, T? obj = default)
	{
		return new() { IsFailed = false, IsSuccess = true, Messages = messages, Obj = obj };
	}
}

public class ResponseDto<T>
{
	public bool IsSuccess { get; set; }
	public bool IsFailed { get; set; }
	public List<string>? Messages { get; set; }
	public T? Obj { get; set; }
}
