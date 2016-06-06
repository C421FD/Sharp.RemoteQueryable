using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DataVirtualization;
using Sharp.RemoteQueryable.Samples.Model;

namespace Sharp.RemoteQueryable.Samples.WpfWcfClient
{
  /// <summary>
  /// Demo implementation of IItemsProvider returning dummy customer items after
  /// a pause to simulate network/disk latency.
  /// </summary>
  public class DemoWorkItemProvider : IItemsProvider<WorkItem>
  {
    /// <summary>
    /// Fetches the total number of items available.
    /// </summary>
    /// <returns></returns>
    public int FetchCount()
    {
      return RemoteRepository.CreateQuery<WorkItem>(new DemoChannelProvider())
        .Count();
    }

    /// <summary>
    /// Fetches a range of items.
    /// </summary>
    /// <param name="startIndex">The start index.</param>
    /// <param name="count">The number of items to fetch.</param>
    /// <returns></returns>
    public IList<WorkItem> FetchRange(int startIndex, int count)
    {
       return RemoteRepository.CreateQuery<WorkItem>(new DemoChannelProvider())
        .Skip(startIndex)
        .Take(count)
        .ToList();
    }
  }
}
