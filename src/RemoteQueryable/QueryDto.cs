using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using Serialize.Linq.Nodes;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// Query container.
  /// </summary>
  [Serializable]
  public class QueryDto
  {
    public ExpressionNode SerializedExpression { get; set; }

    public string RequestedTypeName { get; set; }

    public string RequestedTypeAssemblyName { get; set; }

    /// <summary>
    /// Create query dto from expression and result type.
    /// </summary>
    /// <param name="expression">Client linq expression.</param>
    /// <param name="type">Type of result.</param>
    /// <returns>Query data transfer object.</returns>
    public static QueryDto CreateQueryDto(Expression expression, Type type)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof(expression));

      if (type == null)
        throw new ArgumentNullException(nameof(type));

      var serializedExpression = expression.ToExpressionNode();
      return new QueryDto(serializedExpression, type.FullName, type.Assembly.FullName);
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="serializedExpression"></param>
    /// <param name="requestedTypeName"></param>
    /// <param name="requestedTypeAssemblyName"></param>
    private QueryDto(ExpressionNode serializedExpression, string requestedTypeName, string requestedTypeAssemblyName)
    {
      this.SerializedExpression = serializedExpression;
      this.RequestedTypeName = requestedTypeName;
      this.RequestedTypeAssemblyName = requestedTypeAssemblyName;
    }

    /// <summary>
    /// 
    /// </summary>
    protected QueryDto() { }
  }
}
