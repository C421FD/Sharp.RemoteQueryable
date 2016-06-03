using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json; 

namespace Sharp.RemoteQueryable.Client
{
  /// <summary>
  /// Default provider for transfer expression to server side and retrieve result.
  /// </summary>
  /// <typeparam name="T">Type of result.</typeparam>
  public class DefaultRemoteQueryableProvider<T> : AbstractQueryProvider
  {
    #region Properties and fields

    /// <summary>
    /// Transport-layer provider.
    /// </summary>
    private readonly IChannelProvider channelProvider;

    #endregion

    #region IQueryProvider

    protected override IQueryable CreateQueryOverride(Expression expression)
    {
      var enumerableQuery = new EnumerableQuery<T>(expression);
      var resultQueryable = ((IQueryProvider)enumerableQuery).CreateQuery(expression);
      return new RemoteQueryable<T>(this, resultQueryable.Expression);
    }

    protected override IQueryable<TElement> CreateQueryOverride<TElement>(Expression expression)
    {
      var enumerableQuery = new EnumerableQuery<TElement>(expression);
      var resultQueryable = ((IQueryProvider)enumerableQuery).CreateQuery<TElement>(expression);
      return new RemoteQueryable<TElement>(this, resultQueryable.Expression);
    }

    protected override object ExecuteOverride(Expression expression)
    {
      var serializedQuery = SerializeQuery(expression);
      return channelProvider.SendRequest<object>(serializedQuery);
    }

    protected override TResult ExecuteOverride<TResult>(Expression expression)
    {
      var serializedQuery = SerializeQuery(expression);
      return this.channelProvider.SendRequest<TResult>(serializedQuery);
    }

    #endregion

    #region Methods

    private static string SerializeQuery(Expression expression)
    {
      var newInternalQuery = QueryDto.CreateQueryDto(expression, typeof(T));
      var serializedQuery = JsonConvert.SerializeObject(newInternalQuery, new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.All
      });
      return serializedQuery;
    }

    #endregion

    #region Ctors

    public DefaultRemoteQueryableProvider(IChannelProvider channelProvider)
    {
      this.channelProvider = channelProvider;
    }

    #endregion
  }
}
