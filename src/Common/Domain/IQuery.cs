using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public interface IQuery<in TFilter, TProjection>
        where TFilter : class
        where TProjection : class
    {
        Task<TProjection> ExecuteAsync(TFilter request);
    }
}
