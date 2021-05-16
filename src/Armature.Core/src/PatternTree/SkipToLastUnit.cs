﻿using System;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   Matches any sequence of building units, thus passing the unit under construction to <see cref="IPatternTreeNode.Children" /> and merge their
  ///   build actions with its own.
  /// </summary>
  public class SkipToLastUnit : PatternTreeNodeWithChildrenBase
  {
    public SkipToLastUnit(int weight = WeightOf.SkipToLastUnit) : base(weight) { }

    public override BuildActionBag BuildActions
      => throw new NotSupportedException(
           "This pattern is used to skip a building unit sequence to the unit under construction (the last one) and pass it to children."
         + "It can't contain build actions due to they are used to build the unit under construction only."
         );

    /// <summary>
    ///   Decreases the matching weight by each skipped unit then pass unit under construction to children nodes
    /// </summary>
    public override WeightedBuildActionBag? GatherBuildActions(ArrayTail<UnitId> unitSequence, int inputWeight)
    {
      var unitsToSkipCount = unitSequence.Length;
      var matchingWeight   = inputWeight - Weight * unitsToSkipCount;

      WeightedBuildActionBag? actionBag = null;
      using(Log.Deferred(LogLevel.Trace, LogMatchingState))
      {
        var lastUnitAsTail = unitSequence.GetTail(unitSequence.Length - 1);
        actionBag = GetChildrenActions(lastUnitAsTail, matchingWeight);
        return actionBag;
      }

      void LogMatchingState(Action? blockContent)
      {
         if(actionBag is not null)
          using(Log.Block(LogLevel.Trace, ToString, unitsToSkipCount))
            blockContent?.Invoke();
      }
    }

    private string ToString(int unitsToSkip) => $"{GetType().GetShortName()}{{ Weight={Weight:n0}, Skipped={unitsToSkip} }}";
  }
}
