using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IDateTimeService
    {
        DateTime Now();
        DateTime UtcNow();
    }
}
