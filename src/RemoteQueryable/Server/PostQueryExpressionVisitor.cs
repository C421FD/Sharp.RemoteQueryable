using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable.Server
{
  /// <summary>
  /// Expression tree modifier for conversion source expression to 'post' expression.
  /// </summary>
  internal class PostQueryExpressionVisitor : ExpressionVisitor
  {
    #region Methods

    /// <summary>
    /// Execute modify process.
    /// </summary>
    /// <param name="sourceExpression">Query expression from client.</param>
    /// <param name="newSource">New data source for query.</param>
    /// <returns>Expression with new data source.</returns>
    public Expression Modify(Expression sourceExpression, IQueryable newSource)
    {
      var methodCallExpression = sourceExpression as MethodCallExpression;
      if (methodCallExpression != null && GetNextQueryAfterPostQuery(methodCallExpression) == null)
        return null;

      queryableSource = newSource;
      return Visit(sourceExpression);
    }

    #endregion

    #region Base class

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
      if (m.Method.Name == nameof(PostQueryable<object>.WrapQuery))
        return queryableSource.Expression;

      return base.VisitMethodCall(m);
    }

    #endregion
  }
}
