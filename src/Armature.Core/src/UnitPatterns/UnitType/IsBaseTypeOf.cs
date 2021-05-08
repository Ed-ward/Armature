﻿using System;

namespace Armature.Core
{
  /// <summary>
  /// Checks if a unit is a subtype of the specified base type.
  /// </summary>
  public record IsBaseTypeOf : TypePatternBase, IUnitPattern
  {
    public IsBaseTypeOf(Type baseType, object? key) : base(baseType, key) { }

    public bool Matches(UnitId unitId) => Key.Matches(unitId.Key) && unitId.GetUnitTypeSafe()?.IsAssignableFrom(Type) == true;
  }
}