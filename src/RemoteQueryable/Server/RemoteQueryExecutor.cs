using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Sharp.RemoteQueryable.Server
{
  /// <summary>
  /// Server API for execute remote request.
  /// </summary>
  public static class RemoteQueryExecutor
  {
    /// <summary>
    /// Execute remote query.
    /// </summary>
    /// <param name="serializedInternalQuery">Remote query.</param>
    /// <param name="sessionObject">ISession object.</param>
    /// <returns>Query result.</returns>
    public static object Do(string serializedInternalQuery, object sessionObject)
    {
      if (serializedInternalQuery == null)
        throw new ArgumentNullException(nameof(serializedInternalQuery));

      if (sessionObject == null)
        throw new ArgumentNullException(nameof(sessionObject));

      var internalRemoteQuery = DeserializeInternalQuery(serializedInternalQuery);
      var deserializedQuery = DeserializedQueryExpressionAndValidate(internalRemoteQuery);
      var targetType = ResolveType(internalRemoteQuery);
      return Execute(deserializedQuery, targetType, sessionObject);
    }

    /// <summary>
    /// Resolve requested data type.
    /// </summary>
    /// <param name="internalRemoteQuery">Query from server.</param>
    /// <returns>Info of requested type.</returns>
    private static TypeInfo ResolveType(InternalQuery internalRemoteQuery)
    {
      var targetAssemblyName = internalRemoteQuery.RequestedTypeAssemblyName;
      var targetAssembly = GetAssemblyOrThrownEx(internalRemoteQuery, targetAssemblyName);
      var targetType = GetTypeFromAssemblyOrThrownEx(targetAssembly, internalRemoteQuery.RequestedTypeName, 
        targetAssemblyName);

      return targetType;
    }

    private static Expression DeserializedQueryExpressionAndValidate(InternalQuery internalRemoteQuery)
    {
      var deserializedQuery = internalRemoteQuery.SerializedExpression.ToExpression();
      ExpressionValidator.Validate(deserializedQuery);
      return deserializedQuery;
    }

    private static TypeInfo GetTypeFromAssemblyOrThrownEx(Assembly targetAssembly, string requestedTypeName, string targetAssemblyName)
    {
      var targetType = targetAssembly.DefinedTypes
        .FirstOrDefault(type => type.FullName.Equals(requestedTypeName, StringComparison.OrdinalIgnoreCase));

      if (targetType == null)
        throw new InvalidOperationException(string.Format("Type with name '{0}' not found in assembly '{1}'", requestedTypeName, targetAssemblyName));

      return targetType;
    }

    private static Assembly GetAssemblyOrThrownEx(InternalQuery internalRemoteQuery, string targetAssemblyName)
    {
      var targetAssembly = AppDomain.CurrentDomain.GetAssemblies()
        .FirstOrDefault(asm => asm.FullName.Equals(internalRemoteQuery.RequestedTypeAssemblyName, StringComparison.OrdinalIgnoreCase));

      if (targetAssembly == null)
        throw new InvalidOperationException(string.Format("Assembly with name '{0}' not found in server app domain", targetAssemblyName));

      return targetAssembly;
    }

    private static InternalQuery DeserializeInternalQuery(string serializedInternalQuery)
    {
      var internalRemoteQuery = JsonConvert
        .DeserializeObject<InternalQuery>(serializedInternalQuery, new JsonSerializerSettings
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
          TypeNameHandling = TypeNameHandling.All
        });

      return internalRemoteQuery;
    }

    private static object Execute(Expression expression, Type targetType, object sessionObject)
    {
      var queryable = GetNhQueryableFromSession(targetType, sessionObject);
      var nhibernatePartialExpression = ExpressionModifier.GetNhibernatePartialExpression(expression, queryable);

      var expressionResultType = nhibernatePartialExpression.Type;
      object resultFromStorage = null;
      if (expressionResultType.IsGenericType && typeof (IQueryable<>).IsAssignableFrom(expressionResultType.GetGenericTypeDefinition()))
      {
        var genericTypeArgument = expressionResultType.GetGenericArguments().FirstOrDefault();
        resultFromStorage = Activator
          .CreateInstance(typeof(List<>).MakeGenericType(genericTypeArgument), queryable.Provider.Execute(nhibernatePartialExpression));
      }
      else
        resultFromStorage = queryable.Provider.Execute(nhibernatePartialExpression);

      var requestedCollection = resultFromStorage as IEnumerable<object>;
      if (requestedCollection == null)
        return resultFromStorage;

      var resultCollectionType = requestedCollection.GetType();
      if (resultCollectionType.IsGenericType)
        targetType = resultCollectionType.GetGenericArguments().Single();

      var enumerableQueryable = (IQueryable)Activator
        .CreateInstance(typeof (EnumerableQuery<>).MakeGenericType(targetType), new [] { requestedCollection });

      var postQueryPartialExpression = ExpressionModifier
        .GetPostQueryPartialExpression(expression, enumerableQueryable);

      if (postQueryPartialExpression == null)
        return resultFromStorage;

      return enumerableQueryable.Provider.Execute(postQueryPartialExpression);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="sessionObject"></param>
    /// <returns></returns>
    private static IQueryable GetNhQueryableFromSession(Type targetType, object sessionObject)
    {
      var finalQueryMethod = ResolveQueryMethod(targetType);
      var queryable = (IQueryable) finalQueryMethod.Invoke(null, new object[] {sessionObject});
      return queryable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    private static MethodInfo ResolveQueryMethod(Type targetType)
    { 
      var queryMethod = NHibernateTypesHelper.LinqExtensionType.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.IsGenericMethod)
        .Where(m => m.Name.Equals("Query"))
        .Single(m => m.GetParameters().Length == 1 && NHibernateTypesHelper.SessionType.IsAssignableFrom(m.GetParameters().First().ParameterType));

      var finalQueryMethod = queryMethod.MakeGenericMethod(targetType);
      return finalQueryMethod;
    }
  }
}
