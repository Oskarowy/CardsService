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

        private static IEnumerable<T> GetAll<T>()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblies = assembly.GetReferencedAssemblies();

            foreach (var assemblyName in assemblies)
            {
                assembly = Assembly.Load(assemblyName);

                foreach (var ti in assembly.DefinedTypes)
                {
                    if (ti.ImplementedInterfaces.Contains(typeof(T)))
                    {
                        yield return (T)assembly.CreateInstance(ti.FullName);
                    }
                }
            }
        }
    }
}
