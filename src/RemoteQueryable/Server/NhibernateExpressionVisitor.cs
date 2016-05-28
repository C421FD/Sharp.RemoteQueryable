using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable.Server
{
  /// <summary>
  /// Expression tree modifier for conversion source expression to nhibernate expression.
  /// </summary>
  internal class NhibernateExpressionVisitor : ExpressionVisitor
  {
    /// <summary>
    /// Execute modify process.
    /// </summary>
    /// <param name="sourceExpression">Query expression from client.</param>
    /// <param name="newSource">New data source for query.</param>
    /// <returns>Expression with new data source.</returns>
    public Expression Modify(Expression sourceExpression, IQueryable newSource)
    {
      queryableSource = newSource;
      return Visit(sourceExpression);
    }

    #region Base class

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
      var nextQueryAfterPostQuery = GetNextQueryAfterPostQuery(m);
      if (nextQueryAfterPostQuery is ConstantExpression)
        return queryableSource.Expression;

      var query = nextQueryAfterPostQuery as MethodCallExpression ?? m;
      if (query.Method.Name == nameof(PostQueryable<object>.WrapQuery))
        query = (MethodCallExpression)query.Arguments.Single();

      var constantArgument = query.Arguments.FirstOrDefault(e => e is ConstantExpression && e.Type.IsGenericType && e.Type.GetGenericTypeDefinition() == typeof(EnumerableQuery<>));
      if (constantArgument != null)
      {
        var constantArgumentPosition = query.Arguments.IndexOf(constantArgument);
        var newArguments = new Expression[query.Arguments.Count];
        for (int index = 0; index < newArguments.Length; index++)
        {
          if (index != constantArgumentPosition)
            newArguments[index] = query.Arguments[index];
          else
            newArguments[index] = queryableSource.Expression;
        }

        return Expression.Call(query.Object, query.Method, newArguments);
      }

      return base.VisitMethodCall(query);
    }



    protected override Expression VisitConstant(ConstantExpression c)
    {
      if (c.Type.IsGenericType && typeof(Sharp.RemoteQueryable.Client.RemoteQueryable<>).IsAssignableFrom(c.Type.GetGenericTypeDefinition()))
        return queryableSource.Expression;

      return c;
    }

    #endregion
  }
}
