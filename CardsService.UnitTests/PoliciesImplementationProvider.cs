using CardsService.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CardsService.UnitTests
{
    public class PoliciesImplementationProvider
    {
        public IEnumerable<IActionPolicy> Implementations => GetAll<IActionPolicy>();

        private List<IActionPolicy?> GetAll<IActionPolicy>()
        {
            var interfaceType = typeof(IActionPolicy);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .Select(type => (IActionPolicy)Activator.CreateInstance(type))
            .ToList();
        }
    }
}
