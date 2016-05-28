using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Sharp.RemoteQueryable.Client
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
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
