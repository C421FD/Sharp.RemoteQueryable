using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WcfClient
{
  class Program
  {
    static void Main(string[] args)
    {
      var provider = new DemoChannelProvider();
      var result = RemoteRepository.CreateQuery<BaseEntity>(provider).ToList();
      Console.ReadKey();
    }
  }
}
