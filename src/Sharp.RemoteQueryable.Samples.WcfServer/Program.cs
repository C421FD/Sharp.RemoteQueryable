using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Contracts;

namespace Sharp.RemoteQueryable.Samples.WcfServer
{
  class Program
  {
    static void Main(string[] args)
    {
      Uri httpBaseAddress = new Uri("http://localhost:8888/RemoteQueryableDemo");
      var studentServiceHost = new ServiceHost(typeof(DemoService), httpBaseAddress);
      studentServiceHost.AddServiceEndpoint(typeof(IDemoService), new BasicHttpBinding(), $"{nameof(DemoService)}");

      ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
      serviceBehavior.HttpGetEnabled = true;
      studentServiceHost.Description.Behaviors.Add(serviceBehavior);
      studentServiceHost.Open();
      Console.WriteLine("Service was started on: {0}", httpBaseAddress);
      Console.ReadKey();
    }
  }
}
