using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable
{
  public static class Ex
  {
    public static IQueryable<T> PostQuery<T>(this IQueryable<T> sourceQuery)
    {
      var query = Expression
        .Call(null, typeof (PostQueryable<T>).GetMethod(nameof(PostQueryable<T>.WrapQuery)), new [] {sourceQuery.Expression});

      return sourceQuery.Provider.CreateQuery<T>(query);
    }
  }
}
