using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.RemoteQueryable.Server
{
  internal class ServerExpressionVisitor : ExpressionVisitor
  {
    protected IQueryable queryableSource;

    protected Expression GetNextQueryAfterPostQuery(MethodCallExpression m)
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
  }
}
