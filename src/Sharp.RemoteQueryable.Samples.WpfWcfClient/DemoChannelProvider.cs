using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Sharp.RemoteQueryable.Client;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WpfWcfClient
{
  public class DemoChannelProvider : IChannelProvider
  {
    private readonly Type[] builtInScalarTypes = new Type[] { typeof (string), typeof (int), typeof (long), typeof (bool) };

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
}
