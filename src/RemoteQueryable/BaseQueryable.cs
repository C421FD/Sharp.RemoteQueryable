using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable
{
  [JsonObject(MemberSerialization.OptOut)]
  public abstract class BaseQueryable<T> : IQueryable<T>, IOrderedQueryable<T>
  {
    #region Properties and fields

    /// <summary>
    /// 
    /// </summary>
    protected volatile bool isSerializingContext;

    /// <summary>
    /// 
    /// </summary>
    protected IEnumerable<T> enumerable;

    #endregion

    #region Process of serializing

    [OnSerializing]
    internal void OnSerializing(StreamingContext context)
    {
      this.isSerializingContext = true;
    }

    [OnSerialized]
    internal void OnSerialized(StreamingContext context)
    {
      this.isSerializingContext = false;
    }

    #endregion

    #region IQueryable

    public IEnumerator<T> GetEnumerator()
    {
      if (this.enumerable == null && !this.isSerializingContext)
        this.enumerable = this.Provider.Execute<IEnumerable<T>>(this.Expression) ?? Enumerable.Empty<T>();

      return this.enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.isSerializingContext ? Enumerable.Empty<T>().GetEnumerator() : this.GetEnumerator();
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

      this.Expression = Expression.Constant(this);
      this.Provider = new DefaultRemoteQueryableProvider<T>(channelProvider);
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

      this.Expression = expression;
      this.Provider = provider;
    }

    protected BaseQueryable()
    {
      this.Expression = Expression.Constant(this);
    }

    #endregion
  }
}
