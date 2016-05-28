using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class BaseEntity
  {
    [DataMember]
    public int Id { get; set; }
  }
}
