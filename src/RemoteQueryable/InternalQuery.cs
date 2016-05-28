using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using Serialize.Linq.Nodes;

namespace Sharp.RemoteQueryable
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public class InternalQuery
  {
    /// <summary>
    /// 
    /// </summary>
    public MethodCallExpressionNode SerializedExpression { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string RequestedTypeName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string RequestedTypeAssemblyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static InternalQuery CreateMessage(Expression expression, Type type)
    {
      var serializedExpression = expression.ToExpressionNode() as MethodCallExpressionNode;
      return new InternalQuery(serializedExpression, type.FullName, type.Assembly.FullName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serializedExpression"></param>
    /// <param name="requestedTypeName"></param>
    /// <param name="requestedTypeAssemblyName"></param>
    private InternalQuery(MethodCallExpressionNode serializedExpression, string requestedTypeName, string requestedTypeAssemblyName)
    {
      this.SerializedExpression = serializedExpression;
      this.RequestedTypeName = requestedTypeName;
      this.RequestedTypeAssemblyName = requestedTypeAssemblyName;
    }

    /// <summary>
    /// 
    /// </summary>
    protected InternalQuery() { }
  }
}
