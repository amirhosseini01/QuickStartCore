using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Core.Commons;

public static class ResponseBase
{
	private static readonly JsonSerializerOptions CustomJsonSerializerOptions = new()
	{
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	public static JsonResult ReturnJson(object obj)
	{
		return new(obj, CustomJsonSerializerOptions);
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
