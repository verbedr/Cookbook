using System.Security.Principal;

namespace Cookbook.Castle
{
    public interface IPrincipalFactory
    {
        IPrincipal Resolve();
        void Release(IPrincipal item);
    }
}
