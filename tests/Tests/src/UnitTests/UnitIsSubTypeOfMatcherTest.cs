﻿using System;
using System.IO;
using Armature.Core.UnitMatchers.UnitType;
using FluentAssertions;
using NUnit.Framework;
using Tests.Common;

namespace Tests.UnitTests
{
  public class UnitIsSubTypeOfMatcherTest
  {
    [Test]
    public void should_match_exact_type([Values(null, "key")] object key)
    {
      var target = new UnitIsSubTypeOfMatcher(typeof(int), key);

      target.Matches(Unit.OfType<int>(key)).Should().BeTrue();
    }

    [Test]
    public void should_match_base_type([Values(null, "key")] object key)
    {
      var target = new UnitIsSubTypeOfMatcher(typeof(Stream), key);

      target.Matches(Unit.OfType<MemoryStream>(key)).Should().BeTrue();
    }

    [Test]
    public void should_match_interface([Values(null, "key")] object key)
    {
      var target = new UnitIsSubTypeOfMatcher(typeof(IDisposable), key);

      target.Matches(Unit.OfType<MemoryStream>(key)).Should().BeTrue();
    }
  }
}