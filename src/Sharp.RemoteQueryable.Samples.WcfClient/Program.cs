using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sharp.RemoteQueryable.Samples.Contracts;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WcfClient
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        var provider = new DemoChannelProvider();
        ShowDevelopers(provider);
        Console.WriteLine("Result: ok");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      Console.ReadKey();
    }

    private static void ShowDevelopers(DemoChannelProvider provider)
    {
      var teamsCount = RemoteRepository.CreateQuery<Team>(provider).Count();
      Console.WriteLine($"In db stored {teamsCount} teams");
      Console.WriteLine("-----------------------");

      ShowTeamsInfo(provider);


    }

    private static void ShowTeamsInfo(DemoChannelProvider provider)
    {
      ShowTeamsWithMoreThenOneMember(provider);
      
    }

    private static void ShowTeamsWithMoreThenOneMember(DemoChannelProvider provider)
    {
      var teamsWithMoreThenOneMember = RemoteRepository
        .CreateQuery<Team>(provider)
        .Where(p => p.Developers.Count > 1);

      Console.WriteLine(
        $"Teams with more then one member {string.Join(",", teamsWithMoreThenOneMember.AsEnumerable().Select(t => t.Title))}");
    }
  }
}
 