using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharp.RemoteQueryable.Tests.Infrastructure;
using Sharp.RemoteQueryable.Tests.Model;

namespace Sharp.RemoteQueryable.Tests
{
  [TestClass]
  public class LinqMethodsWithPostQuery
  {
    #region Infrastructure

    private readonly List<User> users = new List<User>();

    [TestInitialize]
    public void Initialize()
    {
      this.users.Add(new User() { Id = 11, Login = "A1", Name = "AA1", Password = "AAA1", IsAdministrator = true });
      this.users.Add(new User() { Id = 21, Login = "B1", Name = "BB1", Password = "BBB1", IsAdministrator = true });
      this.users.Add(new User() { Id = 31, Login = "C1", Name = "CC1", Password = "CCC1", IsAdministrator = true });
      this.users.Add(new User() { Id = 41, Login = "D1", Name = "DD1", Password = "DDD1" });
      this.users.Add(new User() { Id = 81, Login = "H1", Name = "HH1", Password = "HHH1" });
      this.users.Add(new User() { Id = 61, Login = "F1", Name = "FF1", Password = "FFF1" });
      this.users.Add(new User() { Id = 51, Login = "E1", Name = "EE1", Password = "EEE1" });
      this.users.Add(new User() { Id = 71, Login = "G1", Name = "GG1", Password = "GGG1" });

      this.CleanData();
      using (var session = NHibernateHelper.OpenSession())
      {
        foreach (var user in this.users)
          session.Save(user);

        session.Flush();
      }
    }

    [TestCleanup]
    public void CleanUp()
    {
      this.CleanData();
    }

    private void CleanData()
    {
      using (var session = NHibernateHelper.OpenSession())
      {
        foreach (var user in this.users)
          session.Delete(user);

        session.Flush();
      }
    }

    #endregion

    [TestMethod]
    public void CleanPreQueryAndCleanPostQuery()
    {
      RemoteRepository
      .CreateQuery<User>(new TestChannelProvider())
      .PostQuery()
      .ToList();
    }

    [TestMethod]
    public void CleanPreQueryAndWherePostQuery()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Where(p => p.Name.Equals("BB1"))
        .ToList();

      result.Count.Should().Be(1, "request return more than one element");
      result.Single().Id.Should().Be(21, "request return other object instead etalon");
    }

    private int TstMet()
    {
      return 4;
    }

    [TestMethod]
    public void WherePreQueryAndCleanPostQuery()
    {
      int b = 4;
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Where(p => p.Id == TstMet())
        .PostQuery()
        .ToList();

      result.Count.Should().Be(1, "request return more than one element");
      result.Single().Id.Should().Be(21, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstAsPost()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .First();

      result.Id.Should().Be(11, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstWithPredicateAsPost()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .First(p => p.IsAdministrator);

      result.Id.Should().Be(11, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstOrDefaultAsPost()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .First();

      result.Id.Should().Be(11, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstOrDefaultWithPredicateShouldReturnValueAsPost()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .FirstOrDefault(p => p.IsAdministrator);

      result.Should().NotBeNull("request return null result instead correct value");
    }

    [TestMethod]
    public void FirstOrDefaultWithPredicateShouldReturnNullAsPost()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .FirstOrDefault(p => p.Id == 9999);

      result.Should().BeNull("request return result instead null value");
    }

    [TestMethod]
    public void LastAsPost()
    {
      var result = RemoteRepository
       .CreateQuery<User>(new TestChannelProvider())
       .PostQuery()
       .Last();

      result.Should().NotBeNull("request return null instead result value");
    }

    [TestMethod]
    public void LastWithPredicateAsPost()
    {
      var result = RemoteRepository
       .CreateQuery<User>(new TestChannelProvider())
       .PostQuery()
       .Last(p => p.IsAdministrator);

      result.Should().NotBeNull("request return null instead result value");
    }

    [TestMethod]
    public void LastOrDefaultAsPost()
    {
      var result = RemoteRepository
         .CreateQuery<User>(new TestChannelProvider())
         .PostQuery()
         .LastOrDefault();

      result.Should().NotBeNull("request return null instead result value");
    }

    [TestMethod]
    public void CountAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Count()
        .Should()
        .Be(this.users.Count, $"selected objects count not equal source objects count {this.users.Count}");
    }

    [TestMethod]
    public void CountWithPredicateAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Count(p => p.IsAdministrator)
        .Should()
        .Be(this.users.Count(p => p.IsAdministrator), $"selected objects count not equal source objects count {this.users.Count(p => p.IsAdministrator)}");
    }

    [TestMethod]
    public void AnyAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Count(p => p.IsAdministrator)
        .Should();
    }

    [TestMethod]
    public void AnyWithPredicateShouldReturnTrueAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Any(p => p.Login == "A1")
        .Should();
    }

    [TestMethod]
    public void AnyWithPredicateShouldReturnFalseAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Any(p => p.Login == "ABC1")
        .Should();
    }

    [TestMethod]
    public void SingleAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Single(p => p.Id == 11);
    }

    [TestMethod]
    public void ContainsAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Contains(this.users.First())
        .Should()
        .BeTrue("target element not found in repository");
    }

    [TestMethod]
    public void TakeAsPost()
    {
      var chunk = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Take(4)
        .ToList();

      chunk.Count
        .Should()
        .Be(4, "size of taked chunk not equal to etalon");

      chunk
        .SequenceEqual(this.users.Take(4))
        .Should()
        .BeTrue("chunk of taking by request not equal to etalon");
    }

    [TestMethod]
    public void SkipAndTakeAsPost()
    {
      var chunk = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
         .PostQuery()
        .Skip(1)
        .Take(4)
        .ToList();

      chunk.Count
        .Should()
        .Be(4, "size of taked chunk not equal to etalon");

      chunk
        .SequenceEqual(this.users.Skip(1).Take(4))
        .Should()
        .BeTrue("chunk of taking by request not equal to etalon");
    }

    [TestMethod]
    public void SumAsPost()
    {
      var sum = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Sum(p => p.Id);

      sum.Should().Be(this.users.Sum(p => p.Id), "sum not equal etalon");
    }

    [TestMethod]
    public void MaxAsPost()
    {
      var max = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Max(p => p.Id);

      max.Should().Be(this.users.Max(p => p.Id), "max not equal etalon");
    }

    [TestMethod]
    public void MinAsPost()
    {
      var min = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Min(p => p.Id);

      min.Should().Be(this.users.Min(p => p.Id), "min not equal etalon");
    }

    [TestMethod]
    public void AverageAsPost()
    {
      var average = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .Average(p => p.Id);

      average.Should().Be(this.users.Average(p => p.Id), "average not equal etalon");
    }

    [TestMethod]
    public void OrderByAscAsPost()
    {
      var orderedCollection = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .OrderBy(p => p.Id)
        .ToList();

      orderedCollection
        .SequenceEqual(this.users.OrderBy(p => p.Id))
        .Should()
        .BeTrue("ordered collection not equal to etalon");
    }

    [TestMethod]
    public void OrderByDescAsPost()
    {
      var orderedCollection = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .OrderByDescending(p => p.Id)
        .ToList();

      orderedCollection
        .SequenceEqual(this.users.OrderByDescending(p => p.Id))
        .Should()
        .BeTrue("ordered collection not equal to etalon");
    }

    [TestMethod]
    public void TakeWhileAsPost()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .PostQuery()
        .TakeWhile(p => p.Id > 10 && p.Id < 62)
        .Count()
        .Should()
        .Be(4);
    }
  }
}
