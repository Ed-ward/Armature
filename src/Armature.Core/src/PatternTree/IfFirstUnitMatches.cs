﻿using System;
using System.Diagnostics;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   Checks if the first unit in the building unit sequence matches the specified patter.
  /// </summary>
  public class IfFirstUnitMatches : PatternTreeNodeWithChildren, IEquatable<IfFirstUnitMatches>
  {
    private readonly IUnitPattern _pattern;

    public IfFirstUnitMatches(IUnitPattern pattern) : this(pattern, QueryWeight.StrictMatchingUnit) { }

    public IfFirstUnitMatches(IUnitPattern pattern, int weight) : base(weight) => _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));

    /// <summary>
    ///   Checks if the first unit in the building unit sequence matches the specified patter.
    ///   If it is the unit under construction, returns build actions for it, if no, pass the rest of the sequence to each child and returns merged actions.
    /// </summary>
    public override BuildActionBag? GatherBuildActions(ArrayTail<UnitId> unitSequence, int inputWeight)
      => _pattern.Matches(unitSequence[0]) ? GetOwnOrChildrenBuildActions(unitSequence, inputWeight) : null;

    [DebuggerStepThrough]
    public override string ToString() => string.Format("{0}<{1:n0}>.{2}", GetType().GetShortName(), Weight, _pattern);

    #region Equality

    public bool Equals(IfFirstUnitMatches? other)
    {
      if(ReferenceEquals(null, other)) return false;
      if(ReferenceEquals(this, other)) return true;

      return Equals(_pattern, other._pattern) && Weight == other.Weight;
    }

    public override bool Equals(IPatternTreeNode other) => Equals(other as IfFirstUnitMatches);

    public override bool Equals(object obj) => Equals(obj as IfFirstUnitMatches);

    public override int GetHashCode()
    {
      unchecked
      {
        return (_pattern.GetHashCode() * 397) ^ Weight;
      }
    }

    #endregion
  }
}