using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class Developer : BaseEntity
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int SkillLevel { get; set; }

    [DataMember]
    public IList<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
  }
}
