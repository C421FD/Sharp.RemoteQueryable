using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// Extensions.
  /// </summary>
  public static class Ex
  {
    /// <summary>
    /// Wrap query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceQuery"></param>
    /// <returns></returns>
    public static IQueryable<T> PostQuery<T>(this IQueryable<T> sourceQuery)
    {
      var postQueryable = sourceQuery as PostQueryable<T>;
      if (postQueryable != null)
        throw new ArgumentException("Source query is already PostQueryable<T>", nameof(sourceQuery));

      var remoteQueryable = sourceQuery as Client.RemoteQueryable<T>;
      if (remoteQueryable == null)
        throw new ArgumentException("Source query is not RemoteQueryable<T>", nameof(sourceQuery));

      var query = Expression
        .Call(null, typeof (PostQueryable<T>).GetMethod(nameof(PostQueryable<T>.WrapQuery)), new [] {sourceQuery.Expression});

      return sourceQuery.Provider.CreateQuery<T>(query);
    }
  }
}
