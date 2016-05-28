namespace Sharp.RemoteQueryable.Client
{
  /// <summary>
  /// 
  /// </summary>
  public interface IChannelProvider
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    T SendRequest<T>(string request);
  }
}
