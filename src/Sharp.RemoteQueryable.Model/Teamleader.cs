using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class Teamleader : Developer
  {
    [DataMember]
    public IList<Developer> TeamMembership { get; set; } = new List<Developer>();
  }
}
