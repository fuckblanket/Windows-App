﻿using System;
using System.Net;

namespace LightVPN.Client.Auth.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown when the API returns a undesired response
    /// </summary>
    internal sealed class InvalidResponseException : Exception
    {
        internal InvalidResponseException(string message) : base(message)
        {
        }

        public InvalidResponseException(string message, string responseString, HttpStatusCode code)
        {
            Code = code;
            ResponseString = responseString;
        }

        public HttpStatusCode Code { get; }
        public string ResponseString { get; }
    }
}