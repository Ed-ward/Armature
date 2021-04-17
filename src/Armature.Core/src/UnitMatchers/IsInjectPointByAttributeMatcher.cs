﻿using System;
using System.Diagnostics;

namespace Armature.Core
{
  /// <summary>
  /// Base class for matchers matching if a building unit is an argument for an "inject point" marked with the attribute
  /// </summary>
  public abstract record IsInjectPointByAttributeMatcher<T> : IUnitIdMatcher where T : Attribute
  {
    private readonly Predicate<T>? _predicate;

    /// <param name="predicate">Predicate matches the attribute for a desired state</param>
    [DebuggerStepThrough]
    protected IsInjectPointByAttributeMatcher(Predicate<T>? predicate) => _predicate = predicate;

    public bool Matches(UnitId unitId)
    {
      if(unitId.Key != SpecialKey.InjectValue) return false;

      var attribute = GetAttribute(unitId);
      return attribute is not null && (_predicate is null || _predicate(attribute));
    }

    protected abstract T? GetAttribute(UnitId unitId);
  }
}