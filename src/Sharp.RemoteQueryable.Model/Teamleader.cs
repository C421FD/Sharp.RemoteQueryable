using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class Teamleader : Developer
  {
    [DataMember]
    public virtual int Experience { get; set; }
  }
}
