using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sharp.RemoteQueryable.Samples.Model
{
  [DataContract]
  public class Team : BaseEntity
  {
    [DataMember]
    public virtual Teamleader Leader { get; set; }

    [DataMember(Name = "Developers")]
    private List<Developer> LocalDevelopers
    {
      get { return new List<Developer>(this.Developers); }
      set { this.Developers = value; }
    }
     
    public virtual IList<Developer> Developers { get; set; } = new List<Developer>();

    [DataMember]
    public virtual string Title { get; set; }
  }
}
