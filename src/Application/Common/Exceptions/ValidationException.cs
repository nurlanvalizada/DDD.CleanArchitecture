using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationException() : Exception("One or more validation failures have occurred.")
{
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Failures { get; } = new Dictionary<string, string[]>();
}