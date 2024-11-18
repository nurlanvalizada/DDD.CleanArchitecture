using System;

namespace AppDomain.Common.Exceptions
{
    public class UserFriendlyException(string errorMessage, Exception exc) : Exception($"The following error occurred \"{errorMessage}\"", exc)
    {
        public UserFriendlyException(string errorMessage) : this(errorMessage, null)
        {
            
        }
    }
}
