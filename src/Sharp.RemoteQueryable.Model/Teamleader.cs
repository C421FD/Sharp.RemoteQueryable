using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  [ServiceKnownType(typeof(Developer))]
  public class Teamleader : Developer
  {
    [DataMember]
    public virtual int Experience { get; set; }
  }
}
