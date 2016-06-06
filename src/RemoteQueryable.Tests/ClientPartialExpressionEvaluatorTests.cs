using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable.Tests
{
  [TestClass]
  public class ClientPartialExpressionEvaluatorTests
  {
    ClientExpressionVisitor visitor = new ClientExpressionVisitor();

    [TestMethod]
    public void VariableEvaluatorTest()
    {
   //   Expression.MakeBinary(Expression.Constant(null), Expression.MakeMemberAccess())
  //    this.visitor.Evaluate(Expression)
    }

    [TestMethod]
    public void FieldEvaluatorTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod]
    public void PropertyEvaluatorTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod]
    public void MethodWithoutArgumentEvaluatorTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod]
    public void MethodWithArgumentEvaluatorTest()
    {
      throw new NotImplementedException();
    }

    [TestMethod]
    public void LambdaEvaluatorTest()
    {
      throw new NotImplementedException();
    }

    public class Fixture
    {
      public int field = 1;

      public string Property = "Property";

      public bool MethodWithoutArguments()
      {
        return true;
      }

      public string MethodWithArguments(int b)
      {
        return b.ToString();
      }
    }
  }
}
