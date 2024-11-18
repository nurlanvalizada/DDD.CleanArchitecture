using System;

namespace Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now();
    DateTime UtcNow();
}