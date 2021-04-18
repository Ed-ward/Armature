﻿using System;
using System.Diagnostics;
using Armature.Core;
using Armature.Extensibility;


namespace Armature
{
  public class TreatingOpenGenericTuner : UnitSequenceExtensibility
  {
    [DebuggerStepThrough]
    public TreatingOpenGenericTuner(IPatternTreeNode parentNode) : base(parentNode) { }

    /// <summary>
    ///   Build an object of the specified <paramref name="openGenericType"/> instead. Also use default creation strategy for that type.
    ///   See <see cref="Default.CreationBuildAction"/> for details.
    /// </summary>
    public Tuner AsCreated(Type openGenericType, object? key = null) => As(openGenericType, key).CreatedByDefault();

    /// <summary>
    ///   Build an object of the specified <paramref name="openGenericType"/> instead. 
    /// </summary>
    public OpenGenericCreationTuner As(Type openGenericType, object? key = null)
    {
      ParentNode.UseBuildAction(BuildStage.Create, new RedirectOpenGenericType(openGenericType, key));
      return new OpenGenericCreationTuner(ParentNode, openGenericType, key);
    }

    /// <summary>
    ///   Use default creation strategy for a unit. See <see cref="Default.CreationBuildAction"/> for details.
    /// </summary>
    public Tuner AsIs()
    {
      ParentNode.UseBuildAction(BuildStage.Create, Default.CreationBuildAction);
      return new Tuner(ParentNode);
    }
  }
}
