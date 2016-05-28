using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable
{
  internal static class ExpressionValidator
  {
    public static void Validate(Expression expression, bool throwOnError = true)
    {
      var message = string.Empty;
      var methodCallExpression = expression as MethodCallExpression;
      if (methodCallExpression != null)
      {
        if (methodCallExpression.Method.Name == nameof(Enumerable.SequenceEqual))
        {
          message = string.Format("Method '{0}' is not supported", nameof(Enumerable.SequenceEqual));
          if (throwOnError)
            throw new InvalidExpressionException(message);
        }
      }
      else
      {
        var constantExpression = expression as ConstantExpression;
        if (constantExpression == null)
        {
          message = string.Format("Expression type should be '{0}' or {1}", nameof(MethodCallExpression), nameof(ConstantExpression));
          if (throwOnError)
            throw new InvalidExpressionException(message);
        }
      }
    }
  }
}
