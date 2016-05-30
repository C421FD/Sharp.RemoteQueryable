using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;
using Sharp.RemoteQueryable.Server;

namespace Sharp.RemoteQueryable.Samples.WcfServer
{
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
}
