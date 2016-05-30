using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.RemoteQueryable.Samples.WcfClient
{
  public class Person
  {
    public int Age;

    public string Name;

    void ShowName()
    {
      Console.WriteLine(Name);
    }
  }

  public class Freelancer : Person
  {
    public int CountOfProjects;

    public int CalculateAwards()
    {
      return 0;
    }
  }
}
