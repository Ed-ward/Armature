﻿using System;
using System.Reflection;
using Armature.Core;
using Armature.Logging;
using JetBrains.Annotations;

namespace Armature.Framework
{
  public class ParameterNameValueBuildStep : ParameterValueBuildStep
  {
    private readonly string _parameterName;

    public ParameterNameValueBuildStep(int weight, [NotNull] string parameterName, [NotNull] Func<ParameterInfo, IBuildAction> getBuildAction)
      : base(getBuildAction, weight)
    {
      if (parameterName == null) throw new ArgumentNullException("parameterName");
      _parameterName = parameterName;
    }

    protected override bool Matches(ParameterInfo parameterInfo)
    {
      var matches = _parameterName == parameterInfo.Name;

      if(!matches)
      {
        Log.Info("Does not match");
        Log.Info("MatchName={0}", _parameterName);
        Log.Info("ParameterName={0}", parameterInfo.Name);
      }

      return matches;
    }
  }
}