using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.Contracts
{
  [ServiceContract]
  [ServiceKnownType("GetKnownTypes", typeof(Helper))]
  public interface IDemoService
  {
    [OperationContract]
    object GetScalar(string query);

    [OperationContract]
    BaseEntity GetSingle(string query);

    [OperationContract]
    IEnumerable<BaseEntity> GetEnumerable(string query);
  }

  public static class Helper
  {
    public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
    {
      return new List<Type> {typeof (Developer), typeof (Teamleader), typeof (Team), typeof (WorkItem)};
    }
  }
}
