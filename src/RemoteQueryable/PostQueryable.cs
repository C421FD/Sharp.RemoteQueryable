using System.Linq;
using System.Linq.Expressions;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable
{
  public class PostQueryable<T> : BaseQueryable<T>
  {
    public static IQueryable<T> WrapQuery(IQueryable<T> sourceQuery)
    {
      return sourceQuery;
    }

    #region Ctors

    public PostQueryable(IChannelProvider channelProvider) : base(channelProvider) { }

    public PostQueryable(AbstractQueryProvider provider, Expression expression) : base(provider, expression) { }

    public PostQueryable() { this.Expression = Expression.Constant(this); }

    #endregion
  }
}
