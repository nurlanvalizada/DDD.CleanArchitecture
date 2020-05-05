using System;

namespace AppDomain.Common.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException(string errorMessage) : this(errorMessage, null)
        {
            
        } 
        public UserFriendlyException(string errorMessage, Exception exc) : base($"The following error occured \"{errorMessage}\"", exc)
        {
            
        }
    }
}
