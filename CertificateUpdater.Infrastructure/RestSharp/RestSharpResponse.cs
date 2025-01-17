﻿using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using RestSharp;

namespace CertificateUpdater.Services.RestSharp;
public class RestSharpResponse<TResponse> : RestResponse<TResponse>, IResponse<TResponse> where TResponse : class
{
	public RestSharpResponse(RestRequest request, RestResponse<TResponse> response) : base(request)
	{
		Content = response.Content;
		RawBytes = response.RawBytes;
		ContentEncoding = response.ContentEncoding;
		ContentLength = response.ContentLength;
		ContentType = response.ContentType;
		Cookies = response.Cookies;
		ErrorMessage = response.ErrorMessage;
		ErrorException = response.ErrorException;
		Headers = response.Headers;
		ContentHeaders = response.ContentHeaders;
		IsSuccessStatusCode = response.IsSuccessStatusCode;
		ResponseStatus = response.ResponseStatus;
		ResponseUri = response.ResponseUri;
		Server = response.Server;
		StatusCode = response.StatusCode;
		StatusDescription = response.StatusDescription;
		Request = response.Request;
		RootElement = response.RootElement;
		Data = response.Data;
	}

	public Result<TResult> GetResult<TResult>(
		Func<TResponse, TResult> mappingFunc)
	{
		if (IsSuccessful && Data is not null)
		{
			try
			{
				return Result.Success(mappingFunc(Data));
			}
			catch (Exception ex)
			{
				return Result.Failure<TResult>(new Error(
					"MappingError",
					ex.Message));
			}
		}

		return Result.Failure<TResult>(
				new Error(
					StatusCode.ToString(),
					ErrorException?.Message! ?? "Error while fetching"));
	}
}
