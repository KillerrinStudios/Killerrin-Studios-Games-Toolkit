using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Enumerators
{
    public enum APIResponse
    {
        None,
        Successful,
        Failed,

        //-- Standard Errors
        APIError,
        NetworkError,
        ServerError,
        UnknownError,

        //-- Specialized Errors
        NotSupported,

        // Login
        InfoNotEntered,
        InvalidCredentials,

        //
    }
}
