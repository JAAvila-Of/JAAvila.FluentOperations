using JAAvila.FluentOperations.Extensions;
using JAAvila.FluentOperations.Model;
using Microsoft.AspNetCore.Mvc;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// ASP.NET Core-specific extension methods for <see cref="QualityReport"/>
/// that produce RFC 7807 Problem Details responses.
/// </summary>
public static class QualityReportAspNetExtensions
{
    /// <summary>
    /// Converts a <see cref="QualityReport"/> to a <see cref="ValidationProblemDetails"/> instance
    /// conforming to RFC 7807.
    /// <para>
    /// Only <see cref="Severity.Error"/>-level failures are included in the <c>Errors</c> dictionary.
    /// Warning and Info failures are excluded.
    /// </para>
    /// </summary>
    /// <param name="report">The quality report to convert.</param>
    /// <param name="title">Optional title. Defaults to "Validation Failed".</param>
    /// <param name="detail">Optional detailed explanation.</param>
    /// <param name="statusCode">Optional HTTP status code. Defaults to 400.</param>
    /// <returns>A ValidationProblemDetails instance with errors grouped by property name.</returns>
    public static ValidationProblemDetails ToProblemDetails(
        this QualityReport report,
        string? title = null,
        string? detail = null,
        int? statusCode = null)
    {
        ArgumentNullException.ThrowIfNull(report);

        var errors = report.ToErrorDictionary();

        var problemDetails = new ValidationProblemDetails
        {
            Title = title ?? "Validation Failed",
            Status = statusCode ?? 400
        };

        foreach (var kvp in errors)
        {
            problemDetails.Errors[kvp.Key] = kvp.Value;
        }

        if (detail is not null)
        {
            problemDetails.Detail = detail;
        }

        return problemDetails;
    }
}
