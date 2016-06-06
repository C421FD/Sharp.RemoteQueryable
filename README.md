Welcome to RemoteQueryable
=====================
RemoteQueryable - this is a library to fulfill client requests on the server side using NHibernate library.

RemoteQueryable over WCF sample
---------------------
1 Implement IChannelProvider for client-side
```
public class DemoChannelProvider : IChannelProvider
{
  private readonly Type[] builtInScalarTypes = new Type[] { typeof (string), 
    typeof (int), typeof (long), typeof (bool) };

  private readonly IDemoService demoService;

  public DemoChannelProvider()
  {
    var endpoint = new EndpointAddress($"http://localhost:8888/RemoteQueryableDemo/DemoService");
    this.demoService = ChannelFactory<IDemoService>.CreateChannel(new BasicHttpBinding()
    {
      ReceiveTimeout = TimeSpan.FromSeconds(30)
    }, endpoint);
  }

  public T SendRequest<T>(string request)
  {
    var requestedType = typeof(T);

    if (typeof(BaseEntity).IsAssignableFrom(requestedType))
      return (T)((object)this.demoService.GetSingle(request));

    if (this.builtInScalarTypes.Any(t => t == requestedType))
      return (T)this.demoService.GetScalar(request);

    if (requestedType.IsGenericType && typeof(IEnumerable<>) == requestedType.GetGenericTypeDefinition())
    {
      var elementsType = requestedType.GetGenericArguments().Single();
      var resultCollection = this.demoService.GetEnumerable(request);
      var ofTypeMethod = typeof(Enumerable)
        .GetMethod(nameof(Enumerable.OfType), BindingFlags.Static | BindingFlags.Public);

      var genericOfTypeMethod = ofTypeMethod.MakeGenericMethod(elementsType);
      return (T)genericOfTypeMethod.Invoke(null, new[] { resultCollection });
    }

    return default(T);
  }
}
```
2 Define method on client
```
public IList<WorkItem> FetchWorkItems()
{ 
  return RemoteRepository.CreateQuery<WorkItem>(new DemoChannelProvider())
    .Skip(10)
    .Take(5)
    .ToList();
}
```
3 On server-side implement IDemoService
```
  public class DemoService : IDemoService
  {
    public object GetScalar(string query)
    {
      using (var session = NHibernateHelper.OpenSession())
        return RemoteQueryExecutor.Do(query, session);
    }

    public BaseEntity GetSingle(string query)
    {
      using (var session = NHibernateHelper.OpenSession())
        return (BaseEntity)RemoteQueryExecutor.Do(query, session);
    }

    public IEnumerable<BaseEntity> GetEnumerable(string query)
    {
      using (var session = NHibernateHelper.OpenSession())
        return (IEnumerable<BaseEntity>)RemoteQueryExecutor.Do(query, session);
    }
  }
```

Linq samples
---------------------
1 Full translation to sql
```
RemoteRepository.CreateQuery<WorkItem>(new DemoChannelProvider())
  .Where(p => p.Priority > 10)
  .Count();
// result: select Count(*) from workitem where priority > 10
```

2 Partial translation to sql, partial executing in memory
```
RemoteRepository.CreateQuery<WorkItem>(new DemoChannelProvider())
  .Where(p => p.Priority > 10)
  .Where(p => p.IsActive == true)
  .PostQuery()
  .Where(p => p.IsClosed == true)
  .Count();
// sql party: 
//    select Count(*) from workitem where priority > 10 and isactive = true
// in memory party: 
//    var resultFromDb = ExecuteQuery(query);
//    resultFromDb.Where(p => p.IsClosed == true)
//     .Count();

```
