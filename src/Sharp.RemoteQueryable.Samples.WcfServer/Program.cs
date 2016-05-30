using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WcfServer
{
  class Program
  {
    private static readonly Uri HttpBaseAddress = new Uri("http://localhost:8888/RemoteQueryableDemo");

    static void Main(string[] args)
    {
      // Uncomment for initialize demo data
      // DemoData.Generate();
      InitializeService();
      Console.WriteLine("Service was started on: {0}", HttpBaseAddress);
      Console.ReadKey();
    }

    private static void InitializeService()
    {

      var studentServiceHost = new ServiceHost(typeof(DemoService), HttpBaseAddress);
      studentServiceHost.AddServiceEndpoint(typeof(IDemoService), new BasicHttpBinding(), $"{nameof(DemoService)}");

      ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
      serviceBehavior.HttpGetEnabled = true;
      studentServiceHost.Description.Behaviors.Add(serviceBehavior);
      studentServiceHost.Open();
    }
  }
}
