using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable.Server
{
  public class PostQueryEvaluator : ExpressionVisitor
  {
    private IQueryable queryableSource;

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
      if (m.Method.Name == nameof(PostQueryable<object>.WrapQuery))
        return this.queryableSource.Expression;

      return base.VisitMethodCall(m);
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

    public Expression Evaluate(Expression sourceExpression, IQueryable newSource)
    {
      var methodCallExpression = sourceExpression as MethodCallExpression;
      if (methodCallExpression != null && this.GetNextQueryAfterPostQuery(methodCallExpression) == null)
        return null;

      this.queryableSource = newSource;
      return this.Visit(sourceExpression);
    }
  }
}
