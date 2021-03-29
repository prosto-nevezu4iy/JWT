using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Application.Common.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
        public IEnumerable<Role> Roles { get; set; }

        public AuthorizeAttribute(params Role[] roles)
        {
            Roles = roles ?? new Role[] { };
        }
    }
}
