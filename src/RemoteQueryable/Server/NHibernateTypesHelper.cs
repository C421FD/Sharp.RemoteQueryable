using System;
using System.Linq;
using System.Reflection;

namespace Sharp.RemoteQueryable.Server
{
  /// <summary>
  /// Helper for loading the NHibernate types implicitly.
  /// </summary>
  /// <remarks>Destiny: decoupling the NHibernate.dll from library.</remarks>
  internal static class NHibernateTypesHelper
  {
    #region Fields and properties

    /// <summary>
    /// Loaded NHibernate assembly.
    /// </summary>
    private static readonly Assembly nhibernateAssembly;

    /// <summary>
    /// ISession.
    /// </summary>
    public static Type SessionType { get; private set; }

    /// <summary>
    /// NHibernate.ISession.
    /// </summary>
    public static Type LinqExtensionType { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Check if object is instance of ISession type.
    /// </summary>
    /// <param name="inspectedObject">Object for inspection.</param>
    /// <returns>Return true, if object is ISession, otherwise false.</returns>
    public static bool IsSessionObject(object inspectedObject)
    {
      return SessionType.IsInstanceOfType(inspectedObject);
    }

    /// <summary>
    /// Helper initialization.
    /// </summary>
    static NHibernateTypesHelper()
    {
      nhibernateAssembly = AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(asm => asm.FullName.Contains("NHibernate")) ?? Assembly.Load("NHibernate");

      SessionType = nhibernateAssembly.GetTypes()
        .Single(p => p.FullName.Equals("NHibernate.ISession", StringComparison.OrdinalIgnoreCase));

      LinqExtensionType = nhibernateAssembly.GetTypes()
        .Single(p => p.FullName.Equals("NHibernate.Linq.LinqExtensionMethods", StringComparison.OrdinalIgnoreCase));
    }

    #endregion
  }
}
