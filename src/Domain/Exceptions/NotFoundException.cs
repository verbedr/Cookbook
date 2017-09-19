using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Exceptions
{
    public class NotFoundException : CookbookException
    {
        public NotFoundException(object id, string type) : base(404, 0, "Default", id, type)
        {
        }
    }
}
