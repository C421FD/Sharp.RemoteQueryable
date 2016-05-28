using Sharp.RemoteQueryable.Server;

namespace Sharp.RemoteQueryable.Tests.Infrastructure
{
  public static class RemoteExecutorWrapper
  {
    private static readonly RemoteQueryExecutor executor = new RemoteQueryExecutor();

    public static object Invoke(string request)
    {
      using (var session = NHibernateHelper.OpenSession())
      {
        return executor.Do(request, session);
      }
    }
  }
}
