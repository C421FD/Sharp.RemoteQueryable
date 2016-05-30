using System.Runtime.Serialization;
using System.ServiceModel;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [ServiceKnownType(typeof(Developer))]
  [ServiceKnownType(typeof(Teamleader))]
  [ServiceKnownType(typeof(Team))]
  [ServiceKnownType(typeof(WorkItem))]
  [DataContract]
  public class BaseEntity
  {
    [DataMember]
    public virtual int Id { get; set; }
  }
}
