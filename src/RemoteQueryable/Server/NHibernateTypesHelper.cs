using System;
using System.Linq;
using System.Reflection;

namespace Sharp.RemoteQueryable.Server
{
  public static class NHibernateTypesHelper
  {
    private static readonly Assembly nhibernateAssembly;

    public static Type SessionType { get; private set; }

    public static Type LinqExtensionType { get; private set; }

    public static bool IsSessionObject(object sessionObject)
    {
      return SessionType.IsInstanceOfType(sessionObject);
    }

    static NHibernateTypesHelper()
    {
      nhibernateAssembly = AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(asm => asm.FullName.Contains("NHibernate")) ?? Assembly.Load("NHibernate");

      SessionType = nhibernateAssembly.GetTypes()
        .Single(p => p.FullName.Equals("NHibernate.ISession", StringComparison.OrdinalIgnoreCase));

      LinqExtensionType = nhibernateAssembly.GetTypes()
        .Single(p => p.FullName.Equals("NHibernate.Linq.LinqExtensionMethods", StringComparison.OrdinalIgnoreCase));
    }
  }
}
