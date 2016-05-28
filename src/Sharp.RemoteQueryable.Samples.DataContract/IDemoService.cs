using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.Contracts
{
  [ServiceContract]
  public interface IDemoService
  {
    [OperationContract]
    object GetScalar(string query);

    [OperationContract]
    BaseEntity GetSingle(string query);

    [OperationContract]
    IEnumerable<BaseEntity> GetEnumerable(string query);
  }
}
