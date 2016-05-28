using System.Linq;
using System.Linq.Expressions;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// Queryable for invocation after NHibernate query.
  /// </summary>
  /// <typeparam name="T">Type of Result.</typeparam>
  internal class PostQueryable<T> : BaseQueryable<T>
  {
    public static IQueryable<T> WrapQuery(IQueryable<T> sourceQuery)
    {
      return sourceQuery;
    }

    #region Ctors

    public PostQueryable(IChannelProvider channelProvider) : base(channelProvider) { }

    public PostQueryable(AbstractQueryProvider provider, Expression expression) : base(provider, expression) { }

    public PostQueryable() { Expression = Expression.Constant(this); }

    #endregion
  }
}
