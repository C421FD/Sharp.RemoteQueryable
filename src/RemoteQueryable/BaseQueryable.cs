using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Sharp.RemoteQueryable.Client;
using Newtonsoft.Json;

namespace Sharp.RemoteQueryable
{
  [JsonObject(MemberSerialization.OptOut)]
  public abstract class BaseQueryable<T> : IQueryable<T>, IOrderedQueryable<T>
  {
    #region Properties and fields

    /// <summary>
    /// 
    /// </summary>
    protected volatile bool isInSerializingContext;

    /// <summary>
    /// 
    /// </summary>
    protected IEnumerable<T> enumerable;

    #endregion

    #region Process of serializing

    [OnSerializing]
    internal void OnSerializing(StreamingContext context)
    {
      isInSerializingContext = true;
    }

    [OnSerialized]
    internal void OnSerialized(StreamingContext context)
    {
      isInSerializingContext = false;
    }

    #endregion

    #region IQueryable

    public IEnumerator<T> GetEnumerator()
    {
      if (enumerable == null && !isInSerializingContext)
        enumerable = Provider.Execute<IEnumerable<T>>(Expression) ?? Enumerable.Empty<T>();

      return enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return isInSerializingContext ? Enumerable.Empty<T>().GetEnumerator() : GetEnumerator();
    }

    public Expression Expression { get; set; }

    public Type ElementType { get; set; }

    [JsonIgnore]
    public IQueryProvider Provider { get; set; }

    #endregion

    #region Ctors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="channelProvider"></param>
    protected BaseQueryable(IChannelProvider channelProvider)
    {
      if (channelProvider == null)
        throw new ArgumentNullException(nameof(channelProvider));

      Expression = Expression.Constant(this);
      Provider = new DefaultRemoteQueryableProvider<T>(channelProvider);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="expression"></param>
    protected BaseQueryable(AbstractQueryProvider provider, Expression expression)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof(provider));
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      Expression = expression;
      this.Provider = provider;
    }

    protected BaseQueryable()
    {
      Expression = Expression.Constant(this);
    }

    #endregion
  }
}
