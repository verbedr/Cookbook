using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Exceptions
{
    public class SecurityException : CookbookException
    {
        public SecurityException(string messageKey = null, params object[] args) : base(403, 0, messageKey ?? "Default", args)
        {
        }
    }
}
