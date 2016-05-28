using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable
{
  public abstract class AbstractQueryProvider : IQueryProvider
  {
    #region Methods

    protected abstract IQueryable CreateQueryOverride(Expression expression);

    protected abstract IQueryable<TElement> CreateQueryOverride<TElement>(Expression expression);

    protected abstract object ExecuteOverride(Expression expression);

    protected abstract TResult ExecuteOverride<TResult>(Expression expression);

    #endregion

    #region IQueryProvider

    public IQueryable CreateQuery(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      ExpressionValidator.Validate(expression);
      return this.CreateQueryOverride(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      ExpressionValidator.Validate(expression);
      return this.CreateQueryOverride<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      ExpressionValidator.Validate(expression);
      return this.ExecuteOverride(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      ExpressionValidator.Validate(expression);
      return this.ExecuteOverride<TResult>(expression);
    }

    #endregion
  }
}
