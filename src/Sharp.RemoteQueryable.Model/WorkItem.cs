using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class WorkItem : BaseEntity
  {
    [DataMember]
    public virtual string Text { get; set; }

    [DataMember]
    public virtual int Priority { get; set; }
  }
}
