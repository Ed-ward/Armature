using System;
using System.IO;
using Armature;
using Armature.Core;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable all

namespace Tests.Functional
{
  public class RuntimeParameterValuesTest
  {
    [Test]
    public void should_use_sessional_parameter_of_exact_type()
    {
      const string expected = "megastring";

      // --arrange
      var target = CreateTarget();

      target
       .Treat<Subject>()
       .AsIs()
       .InjectInto(Constructor.MarkedWithInjectAttribute(Subject.StringCtor));

      // --act
      var actual = target.Build<Subject>(expected);

      // --assert
      actual.String1.Should().Be(expected);
    }

    [Test]
    public void should_use_sessional_parameter_of_derived_type()
    {
      var expected = new MemoryStream();

      // --arrange
      var target = CreateTarget();

      target
       .Treat<Subject>()
       .AsIs()
       .InjectInto(Constructor.MarkedWithInjectAttribute(Subject.DisposableCtor));

      // --act
      var actual = target.Build<Subject>(expected);

      // --assert
      actual.Disposable.Should().Be(expected);
    }

    [Test]
    public void typed_and_named_parameters_should_work()
    {
      const string expectedString1 = "dslfj";
      const string expectedString2 = "l;kjsf";

      // --arrange
      var target = CreateTarget();

      target
       .Treat<Subject>()
       .AsIs();

      // --act
      var actual = target
       .Build<Subject>(
          ForParameter.OfType<string>().UseValue(expectedString1),
          ForParameter.Named("string2").UseValue(expectedString2));

      // --assert
      actual.String1.Should().Be(expectedString1);
      actual.String2.Should().Be(expectedString2);
    }

    [Test]
    public void pass_null_as_valid_value()
    {
      // --arrange
      var target = CreateTarget();

      target
       .Treat<Subject>()
       .AsIs()
       .InjectInto(Constructor.MarkedWithInjectAttribute(Subject.StringCtor));

      // --act
      var actual = target.Build<Subject>(ForParameter.OfType<string>().UseValue(null));

      // --assert
      actual.String1.Should().BeNull();
    }

    private static Builder CreateTarget()
      => new(BuildStage.Initialize, BuildStage.Create)
         {
           new SkipToLastUnit
           {
             // inject into constructor
             new IfLastUnitMatches(ConstructorPattern.Instance)
              .UseBuildAction(GetLongestConstructor.Instance, BuildStage.Create),
             new IfLastUnitMatches(MethodArgumentPattern.Instance)
              .UseBuildAction(BuildArgumentByParameter.Instance, BuildStage.Create) // autowiring
           }
         };

    private class Subject
    {
      public const string StringCtor     = "String";
      public const string DisposableCtor = "Disposable";

      public readonly IDisposable Disposable;
      public readonly string      String1;
      public readonly string      String2;

      [Inject(StringCtor)]
      public Subject(string string1) => String1 = string1;

      [Inject(DisposableCtor)]
      public Subject(IDisposable disposable)
      {
        Disposable = disposable;
      }

      public Subject([Inject] string string1, string string2)
      {
        String1 = string1;
        String2 = string2;
      }
    }
  }
}
