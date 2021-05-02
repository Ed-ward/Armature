﻿using System;
using System.Diagnostics;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   Matches any sequence of building units, thus passing the unit under construction to <see cref="IPatternTreeNode.Children" /> and merge their
  ///   build actions with its own.
  /// </summary>
  public class SkipToLastUnit : PatternTreeNodeWithChildren
  {
    public SkipToLastUnit() : this(WeightOf.SkipToLastUnit) { }
    public SkipToLastUnit(int weight) : base(weight) { }

    public override BuildActionBag BuildActions
      => throw new NotSupportedException(
           "This pattern is used to skip a building unit sequence to the unit under construction (the last one) and pass it to children."
         + "It can't contain build actions due to they are used to build the unit under construction only."
         );

    /// <summary>
    ///   Decreases the matching weight by each skipped unit then pass unit under construction to children nodes
    /// </summary>
    public override WeightedBuildActionBag? GatherBuildActions(ArrayTail<UnitId> unitSequence, long inputWeight)
    {
      var unitsToSkipCount = unitSequence.Length;
      var matchingWeight = inputWeight + Weight * unitsToSkipCount;

      using(Log.Block(LogLevel.Verbose, ToString, unitsToSkipCount)) // pass group method, do not call ToString method
      {
        var lastUnitAsTail = unitSequence.GetTail(unitSequence.Length - 1);
        return GetChildrenActions(lastUnitAsTail, matchingWeight);
      }
    }

    private string ToString(int unitsToSkip) => $"{GetType().GetShortName()}{{ Weight={Weight:n0}, Skipped={unitsToSkip} }}";

    #region Equality

    [DebuggerStepThrough]
    public override bool Equals(IPatternTreeNode other) => Equals((object) other);

    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
      if(ReferenceEquals(null, obj)) return false;
      if(ReferenceEquals(this, obj)) return true;

      return obj is SkipToLastUnit other && Weight == other.Weight;
    }

    [DebuggerStepThrough]
    public override int GetHashCode() => Weight.GetHashCode();

    #endregion
  }
}
