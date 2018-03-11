﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Armature.Core
{
  public static class UnitInfoExtension
  {
    /// <summary>
    ///   Returns a <see cref="Type" /> if <see cref="UnitInfo.Id" /> is a type, otherwise throws exception.
    /// </summary>
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [DebuggerStepThrough]
    [NotNull]
    public static Type GetUnitType([NotNull] this UnitInfo unitInfo) =>
      unitInfo != null ? (Type)unitInfo.Id : throw new ArgumentNullException(nameof(unitInfo));

    /// <summary>
    ///   Returns a <see cref="Type" /> if <see cref="UnitInfo.Id" /> is a type, otherwise null.
    /// </summary>
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [DebuggerStepThrough]
    [CanBeNull]
    public static Type GetUnitTypeSafe([NotNull] this UnitInfo unitInfo) =>
      unitInfo != null ? unitInfo.Id as Type : throw new ArgumentNullException(nameof(unitInfo));
  }
}