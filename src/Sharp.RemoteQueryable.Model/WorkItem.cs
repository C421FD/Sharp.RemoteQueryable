using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class WorkItem : BaseEntity
  {
    [DataMember]
    public string Text { get; set; }

    [DataMember]
    public int Priority { get; set; }
  }
}
