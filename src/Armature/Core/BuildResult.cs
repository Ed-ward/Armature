﻿using System.Diagnostics;
using JetBrains.Annotations;

namespace Armature.Core
{
  /// <summary>
  ///   Represents a result of building an until, null is a valid value of the <see cref="Value" />.
  /// </summary>
  public class BuildResult
  {
    [CanBeNull]
    public readonly object Value;

    [DebuggerStepThrough]
    public BuildResult(object value) => Value = value;

    [DebuggerStepThrough]
    public override string ToString() => Value?.ToString() ?? "null";
  }
}