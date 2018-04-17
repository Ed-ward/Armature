﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Resharper.Annotations;
using Armature.Core.Logging;

namespace Armature.Core.BuildActions.Property
{
  /// <summary>
  /// Builds a list of properties by <see cref="PropertyInfo.Name"/>
  /// </summary>
  public class GetPropertyByNameBuildAction : IBuildAction
  {
    private readonly IReadOnlyCollection<string> _names;

    public GetPropertyByNameBuildAction([NotNull] params string[] names)
    {
      if (names is null || names.Length == 0) throw new ArgumentNullException(nameof(names));
      _names = names;
    }

    public void Process(IBuildSession buildSession)
    {
      var unitType = buildSession.GetUnitUnderConstruction().GetUnitType();
      
      var properties = 
      _names.Select(
          name =>
            {
              var property = unitType.GetProperty(name);
              if (property == null)
                throw new ArmatureException(string.Format("There is no property {0} in type {1}", _names, unitType.AsLogString()));

              return property;
            })
        .ToArray();
      
      buildSession.BuildResult = new BuildResult(properties);
    }

    public void PostProcess(IBuildSession buildSession) { }

    public override string ToString() => string.Format(LogConst.OneParameterFormat, GetType().GetShortName(), string.Join(", ", _names));
  }
}