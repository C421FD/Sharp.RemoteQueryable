using Sharp.RemoteQueryable.Server;

namespace Sharp.RemoteQueryable.Tests.Infrastructure
{
  public static class RemoteExecutorWrapper
  {
    public static object Invoke(string request)
    {
      using (var session = NHibernateHelper.OpenSession())
      {
        return RemoteQueryExecutor.Do(request, session);
      }
    }
  }
}
