using System.Linq;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// Client API for request objects from remote storage with linq.
  /// </summary>
  public static class RemoteRepository
  {
    /// <summary>
    /// Create query to remote storage.
    /// </summary>
    /// <typeparam name="TResult">Type of querying object(s)</typeparam>
    /// <param name="provider">Data layer provider.</param>
    /// <returns>Querable decorator.</returns>
    public static IQueryable<TResult> CreateQuery<TResult>(IChannelProvider provider)
    {
      return new RemoteQueryable<TResult>(provider);
    }
  }
}
