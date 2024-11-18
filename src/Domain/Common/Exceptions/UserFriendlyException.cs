using System;

namespace AppDomain.Common.Exceptions;

public class UserFriendlyException(string errorMessage, Exception exc = null) : Exception($"The following error occurred \"{errorMessage}\"", exc);