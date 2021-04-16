﻿using System;
using System.Diagnostics;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   Base class for matchers matching an "inject point" by exact type matching
  /// </summary>
  public abstract record UnitIsInjectPointOfTypeMatcher : IUnitIdMatcher
  {
    private readonly Type _type;

    [DebuggerStepThrough]
    protected UnitIsInjectPointOfTypeMatcher(Type parameterType) => _type = parameterType ?? throw new ArgumentNullException(nameof(parameterType));

    public bool Matches(UnitId unitId) => unitId.Key == SpecialKey.InjectValue && GetInjectPointType(unitId) == _type;

    protected abstract Type? GetInjectPointType(UnitId unitId);

    [DebuggerStepThrough]
    public override string ToString() => string.Format(LogConst.OneParameterFormat, GetType().GetShortName(), _type.ToLogString());
  }
}