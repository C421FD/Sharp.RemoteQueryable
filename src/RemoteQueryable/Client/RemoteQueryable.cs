using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Sharp.RemoteQueryable.Client
{
  /// <summary>
  /// Queryable object, providing invoke expression in a remote server. 
  /// </summary>
  /// <typeparam name="T">Type of result.</typeparam>
  [JsonObject(MemberSerialization.OptOut)]
  public class RemoteQueryable<T> : BaseQueryable<T>
  {
    #region Ctors

    public RemoteQueryable(IChannelProvider channelProvider) : base(channelProvider) { }

    public RemoteQueryable(AbstractQueryProvider provider, Expression expression) : base(provider, expression) { }

    public RemoteQueryable() { this.Expression = Expression.Constant(this); }

    #endregion
  }
}
