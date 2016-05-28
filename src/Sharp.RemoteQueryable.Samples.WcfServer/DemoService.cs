using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WcfServer
{
  public class DemoService : IDemoService
  {
    public object GetScalar(string query)
    {
      return 1;
    }

    public BaseEntity GetSingle(string query)
    {
      return new BaseEntity();
    }

    public IEnumerable<BaseEntity> GetEnumerable(string query)
    {
      return Enumerable.Empty<BaseEntity>();
    }
  }
}
