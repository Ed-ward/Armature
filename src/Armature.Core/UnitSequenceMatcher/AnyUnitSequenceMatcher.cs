﻿using System;
using System.Collections;
using System.Diagnostics;
using Armature.Common;
using Resharper.Annotations;

namespace Armature.Core.UnitSequenceMatcher
{
  /// <summary>
  /// Matches any sequence of building units, thus passing the unit under construction to its children and merge their build actions with its own.
  /// </summary>
  /// <remarks>
  ///   This class implements <see cref="IEnumerable" /> and has <see cref="Add" /> method in order to make possible compact and readable initialization like
  ///   new AnyUnitSequenceMatcher
  ///   {
  ///     new LeafUnitSequenceMatcher(ConstructorMatcher.Instance, 0)
  ///       .AddBuildAction(BuildStage.Create, new GetLongesConstructorBuildAction()),
  ///     new LeafUnitSequenceMatcher(ParameterMatcher.Instance, ParameterMatchingWeight.Lowest)
  ///       .AddBuildAction(BuildStage.Create, new RedirectParameterInfoBuildAction())
  ///   };
  /// </remarks>
  public class AnyUnitSequenceMatcher : UnitSequenceMathcherWithChildren, IEnumerable
  {
    public AnyUnitSequenceMatcher() : this(UnitSequenceMatchingWeight.AnyUnit) { }
    public AnyUnitSequenceMatcher(int weight) : base(weight) { }

    /// <summary>
    ///   Matches any <see cref="UnitInfo" />, so it pass the building unit info into its children and returns merged result
    /// </summary>
    public override MatchedBuildActions GetBuildActions(ArrayTail<UnitInfo> buildingUnitsSequence, int inputWeight)
    {
      var lastItemAsTail = buildingUnitsSequence.GetTail(buildingUnitsSequence.Length - 1);

      return GetChildrenActions(inputWeight + Weight, lastItemAsTail)
        .Merge(GetOwnActions(inputWeight));
    }

    public void Add([NotNull] IUnitSequenceMatcher unitSequenceMatcher) => Children.Add(unitSequenceMatcher);

    #region Equality
    [DebuggerStepThrough]
    public override bool Equals(IUnitSequenceMatcher other) => Equals((object)other);

    [DebuggerStepThrough]
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      return obj is AnyUnitSequenceMatcher other && Weight == other.Weight;
    }

    [DebuggerStepThrough]
    public override int GetHashCode() => Weight.GetHashCode();
    #endregion

    IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
  }
}