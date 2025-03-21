using Ardalis.GuardClauses;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AstroArchitecture.Core.Extensions;
public static class DateGuardExtensions
{
    public static DateTime PastDate(
        this IGuardClause guardClause, 
        [NotNull][ValidatedNotNull] DateTime input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input < DateTime.UtcNow)
        {
            throw new ArgumentException("Date cannot be in the past.", parameterName);
        }

        return input;
    }
}