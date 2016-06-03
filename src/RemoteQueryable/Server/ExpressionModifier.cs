using System.Linq;
using System.Linq.Expressions;

namespace Sharp.RemoteQueryable.Server
{
  /// <summary>
  /// Wrapper for expression visitors.
  /// </summary>
  internal static class ExpressionModifier
  {
    #region Fields

    private static readonly NhibernateExpressionVisitor NhibernateExpressionVisitor = new NhibernateExpressionVisitor();

    private static readonly PostQueryExpressionVisitor PostQueryExpressionVisitor = new PostQueryExpressionVisitor();

    #endregion

    #region Methods

    /// <summary>
    /// Cut post expression part and fake queryable source and set new queryable source.
    /// </summary>
    /// <param name="expression">Source expression.</param>
    /// <param name="newQueryableSource">New queryable data source.</param>
    /// <returns>Modified expression for invoking in nhibernate context.</returns>
    public static Expression GetNhibernatePartialExpression(Expression expression, IQueryable newQueryableSource)
    {
      return NhibernateExpressionVisitor.Modify(expression, newQueryableSource);
    }

    /// <summary>
    /// Cut nhibernate expression part and set new queryable source.
    /// </summary>
    /// <param name="expression">Source expression.</param>
    /// <param name="newQueryableSource">New queryable data source</param>
    /// <returns></returns>
    public static Expression GetPostQueryPartialExpression(Expression expression, IQueryable newQueryableSource)
    {
      return PostQueryExpressionVisitor.Modify(expression, newQueryableSource);
    }

    #endregion
  }
}
