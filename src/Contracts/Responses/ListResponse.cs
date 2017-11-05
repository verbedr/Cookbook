using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Contracts.Responses
{
    public class ListResponse<T>
    {
        public T[] Items { get; set; }
    }
}
