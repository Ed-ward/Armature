using System.Collections.Generic;
using System.Linq;
using Armature.Common;

namespace Armature.Core
{
  public abstract class BuildStepBase : IBuildStep
  {
    private List<IBuildStep> _children;

    public IBuildStep GetChildBuldStep(ArrayTail<IBuildStep> buildStepsSequence)
    {
      if (buildStepsSequence.Length == 0) return null;

      IBuildStep result = null;
      if (buildStepsSequence.Length > 1)
      {
        var buildStep = _children.SingleOrDefault(child => child.Equals(buildStepsSequence[0]));
        result = buildStep == null
          ? null
          : buildStep.GetChildBuldStep(buildStepsSequence.GetTail(1));
      }
      else if (Equals(buildStepsSequence.GetLastItem()))
        result = this;

      return result;
    }

    public abstract MatchedBuildActions GetBuildActions(int inputWeight, ArrayTail<UnitInfo> buildSequence);

    public abstract bool Equals(IBuildStep other);

    public BuildStepBase AddChildBuildStep(IBuildStep buildStep)
    {
      Children.Add(buildStep);
      return this;
    }

    protected MatchedBuildActions GetChildrenActions(int weight, ArrayTail<UnitInfo> tail)
    {
      return Children.Aggregate((MatchedBuildActions) null, (current, child) => current.Merge(child.GetBuildActions(weight, tail)));
    }

    private List<IBuildStep> Children
    {
      get { return _children ?? (_children = new List<IBuildStep>()); }
    }
  }
}