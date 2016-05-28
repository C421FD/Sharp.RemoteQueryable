namespace Sharp.RemoteQueryable.Tests.Model
{
  public class User
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public bool IsAdministrator { get; set; }

    public override bool Equals(object obj)
    {
      var user = obj as User;
      return user?.Id == this.Id;
    }
  }
}
