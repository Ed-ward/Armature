﻿using Armature;
using Armature.Core;
using Armature.Interface;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Tests.Functional
{
  public class AutoWiringTest
  {
    [Test]
    public void explicit_parameter_value_should_have_advantage()
    {
      const string expected = "expected 09765";

      // --arrange
      var target = FunctionalTestHelper.CreateBuilder();

      target
        .Treat<Consumer>()
        .AsIs()
        .UsingParameters(
          For.ParameterId(Consumer.PointId).UseValue(expected));

      target
        .Treat<string>(Consumer.PointId)
        .AsInstance("938754");

      target
        .Treat<string>()
        .AsInstance("lsdjfkl");

      // --act
      var actual = target.Build<Consumer>();

      // --assert
      actual.StringValue.Should().Be(expected, "Because expected value registered UsingParameters for Consumer");
    }

    private static Builder CreateTarget() => FunctionalTestHelper.CreateBuilder();

    [UsedImplicitly]
    private class Consumer
    {
      public const string PointId = "PointId 9387";
      public readonly string StringValue;

      public Consumer([Inject(PointId)] string stringValue) => StringValue = stringValue;
    }
  }
}