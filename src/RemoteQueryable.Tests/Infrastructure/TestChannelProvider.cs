using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable.Tests.Infrastructure
{
  public class TestChannelProvider : IChannelProvider
  {
    public T SendRequest<T>(string request)
    {
      return (T)RemoteExecutorWrapper.Invoke(request);
    }
  }
}
