namespace Sharp.RemoteQueryable.Client
{
  /// <summary>
  /// Transport layer provider.
  /// </summary>
  public interface IChannelProvider
  {
    /// <summary>
    /// Send request to remote server.
    /// </summary>
    /// <param name="request">Query, serialized in a string.</param>
    /// <returns>Result of query.</returns>
    T SendRequest<T>(string request);
  }
}
