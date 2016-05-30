using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [ServiceKnownType(typeof(Teamleader))]
  [DataContract]
  public class Developer : BaseEntity
  {
    [DataMember]
    public virtual string Name { get; set; }

    [DataMember]
    public virtual int SkillLevel { get; set; }

    [DataMember(Name = "WorkItems")]
    private List<WorkItem> LocalWorkItem
    {
      get { return new List<WorkItem>(this.WorkItems); }
      set { this.WorkItems = value; }
    }

    public virtual IList<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
  }
}
