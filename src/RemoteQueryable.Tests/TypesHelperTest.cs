using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharp.RemoteQueryable.Server;
using Sharp.RemoteQueryable.Tests.Infrastructure;

namespace Sharp.RemoteQueryable.Tests
{
  [TestClass]
  public class TypesHelperTest
  {
    [TestMethod]
    public void CheckSessionObjectTest()
    {
      using (var session = NHibernateHelper.OpenSession())
        NHibernateTypesHelper.IsSessionObject(session);
    }
  }
}
