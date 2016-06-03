using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Remotion.Linq.Parsing;
using Sharp.RemoteQueryable.Tests.Infrastructure;
using Sharp.RemoteQueryable.Tests.Model;

namespace Sharp.RemoteQueryable.Tests
{
  [TestClass]
  public class RemoteQueryableTests
  {
    #region Infrastructure

    private readonly List<User> users = new List<User>();

    [TestInitialize]
    public void Initialize()
    {
      this.users.Add(new User() { Id = 1, Login = "A", Name = "AA", Password = "AAA", IsAdministrator = true });
      this.users.Add(new User() { Id = 2, Login = "B", Name = "BB", Password = "BBB", IsAdministrator = true });
      this.users.Add(new User() { Id = 3, Login = "C", Name = "CC", Password = "CCC", IsAdministrator = true });
      this.users.Add(new User() { Id = 4, Login = "D", Name = "DD", Password = "DDD" });
      this.users.Add(new User() { Id = 8, Login = "H", Name = "HH", Password = "HHH" });
      this.users.Add(new User() { Id = 6, Login = "F", Name = "FF", Password = "FFF" });
      this.users.Add(new User() { Id = 5, Login = "E", Name = "EE", Password = "EEE" });
      this.users.Add(new User() { Id = 7, Login = "G", Name = "GG", Password = "GGG" });

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
    public void LoadAll()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .ToList();
    }

    [TestMethod]
    public void Where()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Where(p => p.Name.Equals("BB"))
        .ToList();

      result.Count.Should().Be(1, "request return more than one element");
      result.Single().Id.Should().Be(2, "request return other object instead etalon");
    }

    [TestMethod]
    public void First()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .First();

      result.Id.Should().Be(1, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstWithPredicate()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .First(p => p.IsAdministrator);

      result.Id.Should().Be(1, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstOrDefault()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .First();

      result.Id.Should().Be(1, "request return other object instead etalon");
    }

    [TestMethod]
    public void FirstOrDefaultWithPredicateShouldReturnValue()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .FirstOrDefault(p => p.IsAdministrator);

      result.Should().NotBeNull("request return null result instead correct value");
    }

    [TestMethod]
    public void FirstOrDefaultWithPredicateShouldReturnNull()
    {
      var result = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .FirstOrDefault(p => p.Id == 9999);

      result.Should().BeNull("request return result instead null value");
    }

    [TestMethod]
    public void LastShouldThrowEx()
    {
      try
      {
        RemoteRepository
         .CreateQuery<User>(new TestChannelProvider())
         .Last();

        throw new Exception("'Last operator should throw error'");
      }
      catch (NotSupportedException) { }
    }

    [TestMethod]
    public void LastWithPredicate()
    {
      try
      {
        RemoteRepository
         .CreateQuery<User>(new TestChannelProvider())
         .Last(p => p.IsAdministrator);

        throw new Exception("'Last operator should throw error'");
      }
      catch (NotSupportedException) { }
    }

    [TestMethod]
    public void LastOrDefaultShouldThrowEx()
    {
      try
      {
        RemoteRepository
         .CreateQuery<User>(new TestChannelProvider())
         .LastOrDefault();

        throw new Exception("'LastOrDefault' operator should throw error'");
      }
      catch (NotSupportedException) { }
    }

    [TestMethod]
    public void Count()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Count()
        .Should()
        .Be(this.users.Count, $"selected objects count not equal source objects count {this.users.Count}");
    }

    [TestMethod]
    public void CountWithPredicate()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Count(p => p.IsAdministrator)
        .Should()
        .Be(this.users.Count(p => p.IsAdministrator), $"selected objects count not equal source objects count {this.users.Count(p => p.IsAdministrator)}");
    }

    [TestMethod]
    public void Any()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Count(p => p.IsAdministrator)
        .Should();
    }

    [TestMethod]
    public void AnyWithPredicateShouldReturnTrue()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Any(p => p.Login == "A")
        .Should();
    }

    [TestMethod]
    public void AnyWithPredicateShouldReturnFalse()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Any(p => p.Login == "ABC")
        .Should();
    }

    [TestMethod]
    public void Single()
    {
      try
      {
        var result = RemoteRepository
          .CreateQuery<User>(new TestChannelProvider())
          .Single(p => p.Id == 1);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error in invoke 'Single' method, reason {0}", ex.Message));
      }
    }

    [TestMethod]
    public void Contains()
    {
      RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Contains(this.users.First())
        .Should()
        .BeTrue("target element not found in repository");
    }

    [TestMethod]
    public void Take()
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
    public void SkipAndTake()
    {
      var chunk = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
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
    public void Sum()
    {
      var sum = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Sum(p => p.Id);

      sum.Should().Be(this.users.Sum(p => p.Id), "sum not equal etalon");
    }

    [TestMethod]
    public void Max()
    {
      var max = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Max(p => p.Id);

      max.Should().Be(this.users.Max(p => p.Id), "max not equal etalon");
    }

    [TestMethod]
    public void Min()
    {
      var min = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Min(p => p.Id);

      min.Should().Be(this.users.Min(p => p.Id), "min not equal etalon");
    }

    [TestMethod]
    public void Average()
    {
      var average = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .Average(p => p.Id);

      average.Should().Be(this.users.Average(p => p.Id), "average not equal etalon");
    }

    [TestMethod]
    public void OrderByAsc()
    {
      var orderedCollection = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .OrderBy(p => p.Id)
        .ToList();

      orderedCollection
        .SequenceEqual(this.users.OrderBy(p => p.Id))
        .Should()
        .BeTrue("ordered collection not equal to etalon");
    }

    [TestMethod]
    public void OrderByDesc()
    {
      var orderedCollection = RemoteRepository
        .CreateQuery<User>(new TestChannelProvider())
        .OrderByDescending(p => p.Id)
        .ToList();

      orderedCollection
        .SequenceEqual(this.users.OrderByDescending(p => p.Id))
        .Should()
        .BeTrue("ordered collection not equal to etalon");
    }

    [TestMethod]
    public void TakeWhileShouldThrowEx()
    {
      try
      {
        RemoteRepository
          .CreateQuery<User>(new TestChannelProvider())
          .TakeWhile(p => p.Id < 6)
          .Count();
      }
      catch (ParserException) { }
    }
  }
}
