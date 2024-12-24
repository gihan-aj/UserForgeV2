﻿using Microsoft.AspNetCore.Mvc;
using SharedKernal;

namespace WebAPI.Helpers
{
    public static class ErrorHandler
    {
        public static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Description,
                Status = status,
                Extensions = { { nameof(errors), errors } }
            };
    }
}
