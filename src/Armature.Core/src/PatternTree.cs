﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   The collection of build plans. Build plan of the unit is the tree of units sequence matchers containing build actions.
  ///   All build plans are contained as a forest of trees.
  ///   See <see cref="IPatternTreeNode" /> for details.
  /// </summary>
  /// <remarks>
  ///   This class implements <see cref="IEnumerable" /> and has <see cref="Add" /> method in order to make possible compact and readable initialization like
  ///   new Builder(...)
  ///   {
  ///     new SkipToLastUnit
  ///     {
  ///       new IfLastUnitMatches(new ConstructorPattern(), 0)
  ///         .AddBuildAction(BuildStage.Create, new GetLongestConstructorBuildAction()),
  ///       new IfLastUnitMatches(new MethodArgumentPattern(), ParameterMatchingWeight.Lowest)
  ///         .AddBuildAction(BuildStage.Create, new RedirectParameterInfoBuildAction())
  ///     }
  ///   };
  /// </remarks>
  public class PatternTree : IPatternTreeNode, IEnumerable
  {
    private readonly Root _root = new();

    public ICollection<IPatternTreeNode> Children => _root.Children;

    public WeightedBuildActionBag? GatherBuildActions(ArrayTail<UnitId> unitSequence, int inputWeight = 0)
      => _root.GatherBuildActions(unitSequence, 0);

    public void PrintToLog()
    {
      using(Log.Enabled())
        _root.PrintToLog();
    }

    public BuildActionBag BuildActions                   => throw new NotSupportedException();
    public bool           Equals(IPatternTreeNode other) => throw new NotSupportedException();

    #region Syntax sugar

    public void             Add(IPatternTreeNode patternTreeNode) => Children.Add(patternTreeNode);
    IEnumerator IEnumerable.GetEnumerator()                       => throw new NotSupportedException();

    #endregion

    /// <summary>
    ///   Reuse implementation of <see cref="PatternTreeNodeWithChildren" /> to implement <see cref="PatternTree" /> public interface
    /// </summary>
    private class Root : PatternTreeNodeWithChildren
    {
      [DebuggerStepThrough]
      public Root() : base(0) { }

      [DebuggerStepThrough]
      public override WeightedBuildActionBag? GatherBuildActions(ArrayTail<UnitId> unitSequence, int inputWeight)
        => GetChildrenActions(unitSequence, inputWeight);

      [DebuggerStepThrough]
      public override bool Equals(IPatternTreeNode? other) => throw new NotSupportedException();
    }
  }
}
