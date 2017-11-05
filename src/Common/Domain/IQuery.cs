using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain
{
    public interface IQuery<in TFilter, TProjection>
        where TFilter : class
        where TProjection : class
    {
        Task<IQueryable<TProjection>> ExecuteAsync(TFilter request);
    }
}
