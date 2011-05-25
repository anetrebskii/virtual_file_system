using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VFS.Contract
{
    /// <summary>
    /// Represent authentication result
    /// </summary>
    public sealed class AuthenticationResult
    {
        /// <summary>
        /// Count authenticated users on a server
        /// </summary>
        public int CountAuthenticatedUsers { get; set; }

        /// <summary>
        /// Authentication is success
        /// </summary>
        
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Initilize instance of the class <see cref="AuthenticationResult"/>
        /// </summary>
        private AuthenticationResult()
        {
        }

        /// <summary>
        /// Create instance of the class <see cref="AuthenticationResult"/>
        /// to represent success authentications
        /// </summary>
        /// <param name="countAuthenticatedUsers">Count authenticated users on a server</param>
        /// <returns>Authentication result. it represent success authentication</returns>
        public static AuthenticationResult Success(int countAuthenticatedUsers)
        {
            return new AuthenticationResult() 
            { 
                CountAuthenticatedUsers = countAuthenticatedUsers,
                IsSuccess = true
            };
        }

        /// <summary>
        /// Create instance of the class <see cref="AuthenticationResult"/>
        /// to represent failed authentications
        /// </summary>
        /// <returns>Authentication result. it represent failed authentication</returns>
        public static AuthenticationResult Failed()
        {
            return new AuthenticationResult() 
            { 
                CountAuthenticatedUsers = -1 
            };
        }
    }
}
