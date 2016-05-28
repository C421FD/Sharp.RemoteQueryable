using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable.Server
{
  public static class ExpressionModifier
  {
    private static readonly NhibernateExpressionEvaluator nhibernateExpressionEvaluator = new NhibernateExpressionEvaluator();

    private static readonly PostQueryEvaluator postQueryEvaluator = new PostQueryEvaluator();

    public static Expression GetNhibernatePartialExpression(Expression expression, IQueryable nhibernateSource)
    {
      return nhibernateExpressionEvaluator.Evaluate(expression, nhibernateSource);
    }

    public static Expression GetPostQueryPartialExpression(Expression expression, IQueryable enumerableSource)
    {
      return postQueryEvaluator.Evaluate(expression, enumerableSource);
    }
  }
}
