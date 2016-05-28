using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable.Server
{
  public class NhibernateExpressionEvaluator : ExpressionVisitor
  {
    /// <summary>
    /// 
    /// </summary>
    private IQueryable queryableSource;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceExpression"></param>
    /// <param name="newSource"></param>
    /// <returns></returns>
    public Expression Evaluate(Expression sourceExpression, IQueryable newSource)
    {
      this.queryableSource = newSource;
      return this.Visit(sourceExpression);
    }

    #region Base class

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
      var nextQueryAfterPostQuery = this.GetNextQueryAfterPostQuery(m);
      if (nextQueryAfterPostQuery is ConstantExpression)
        return this.queryableSource.Expression;

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
            newArguments[index] = this.queryableSource.Expression;
        }

        return Expression.Call(query.Object, query.Method, newArguments);
      }

      return base.VisitMethodCall(query);
    }

    private Expression GetNextQueryAfterPostQuery(MethodCallExpression m)
    {
      var stack = new Stack<MethodCallExpression>();
      stack.Push(m);
      while (stack.Any())
      {
        var internalCallExpression = stack.Pop();
        if (internalCallExpression != null)
        {
          if (internalCallExpression.Method.Name == nameof(PostQueryable<object>.WrapQuery))
            return internalCallExpression.Arguments.Single();

          foreach (var arg in internalCallExpression.Arguments.OfType<MethodCallExpression>())
            stack.Push(arg);
        }
      }
      return null;
    }

    protected override Expression VisitConstant(ConstantExpression c)
    {
      if (c.Type.IsGenericType && typeof(RemoteQueryable<>).IsAssignableFrom(c.Type.GetGenericTypeDefinition()))
        return this.queryableSource.Expression;

      return c;
    }

    #endregion
  }
}
