using System.Linq;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// 
  /// </summary>
  public static class RemoteRepository
  {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static IQueryable<TResult> CreateQuery<TResult>(IChannelProvider provider)
    {
      return new RemoteQueryable<TResult>(provider);
    }
  }
}
