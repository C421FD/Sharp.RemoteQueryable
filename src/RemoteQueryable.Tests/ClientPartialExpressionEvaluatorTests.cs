using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharp.RemoteQueryable.Client;

namespace Sharp.RemoteQueryable.Tests
{
  [TestClass]
  public class ClientPartialExpressionEvaluatorTests
  {
    private ClientExpressionVisitor visitor = new ClientExpressionVisitor();

    private Fixture fixture = new Fixture();

 

    [TestMethod]
    public void FieldEvaluatorTest()
    {
      var fieldGetter = Expression.MakeMemberAccess(Expression.Constant(fixture), typeof(Fixture).GetField("field"));
      var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(4), fieldGetter);
      var evaluatedExpression = (BinaryExpression)visitor.Evaluate(binaryExpression);
      evaluatedExpression.Right.GetType().Should().Be(typeof (ConstantExpression));
      ((ConstantExpression) evaluatedExpression.Right).Value.Should().Be(fixture.field);
    }

    [TestMethod]
    public void PropertyEvaluatorTest()
    {
      var propertyGetter = Expression.MakeMemberAccess(Expression.Constant(fixture), typeof(Fixture).GetProperty("Property"));
      var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(string.Empty), propertyGetter);
      var evaluatedExpression = (BinaryExpression)visitor.Evaluate(binaryExpression);
      evaluatedExpression.Right.GetType().Should().Be(typeof(ConstantExpression));
      ((ConstantExpression)evaluatedExpression.Right).Value.Should().Be(fixture.Property);
    }

    [TestMethod]
    public void MethodWithoutArgumentEvaluatorTest()
    {
      var methodGetter = Expression.Call(Expression.Constant(fixture), typeof(Fixture).GetMethod("MethodWithoutArguments"));
      var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(true), methodGetter);
      var evaluatedExpression = (BinaryExpression)visitor.Evaluate(binaryExpression);
      evaluatedExpression.Right.GetType().Should().Be(typeof(ConstantExpression));
      ((ConstantExpression)evaluatedExpression.Right).Value.Should().Be(fixture.MethodWithoutArguments());
    }

    [TestMethod]
    public void MethodWithArgumentEvaluatorTest()
    {
      var methodGetter = Expression
        .Call(Expression.Constant(fixture), typeof(Fixture).GetMethod("MethodWithArguments"), new [] {Expression.Constant(4)});
      var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(4.ToString()), methodGetter);
      var evaluatedExpression = (BinaryExpression)visitor.Evaluate(binaryExpression);
      evaluatedExpression.Right.GetType().Should().Be(typeof(ConstantExpression));
      ((ConstantExpression)evaluatedExpression.Right).Value.Should().Be(fixture.MethodWithArguments(4));
    }

    public class Fixture
    {
      public int field = 1;

      public string Property { get; set; } = "Property";

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
