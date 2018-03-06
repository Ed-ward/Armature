﻿using System;
using Armature.Core;
using Armature.Framework;
using JetBrains.Annotations;

namespace Armature
{
  public class TreatSugar<T> : AdjusterSugar
  {
    private readonly IUnitSequenceMatcher _unitSequenceMatcher;

    public TreatSugar([NotNull] IUnitSequenceMatcher unitSequenceMatcher) : base(unitSequenceMatcher)
    {
      _unitSequenceMatcher = unitSequenceMatcher;
    }

    /// <summary>
    /// Treat Unit as is w/o any redirections
    /// </summary>
    public AdjusterSugar AsIs()
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Create, CreateByReflectionBuildAction.Instance, 0);
      return new AdjusterSugar(_unitSequenceMatcher);
    }

    /// <summary>
    /// Pass the <see cref="instance"/> to any consumer of an Unit 
    /// </summary>
    public void AsInstance([CanBeNull] T instance)
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Cache, new SingletonBuildAction(instance), 0);
    }

    /// <param name="addDefaultCreateAction">If <see cref="AddCreationBuildStep.Yes"/> adds a build step
    /// <see cref="Default.CreationBuildAction"/> for <see cref="UnitInfo"/>(<see name="TRedirect"/>, null)
    /// as a creation build step.</param>
    public AdjusterSugar As<TRedirect>(AddCreationBuildStep addDefaultCreateAction)
    {
      return As<TRedirect>(null, addDefaultCreateAction);
    }

    /// <param name="token"></param>
    /// <param name="addDefaultCreateAction">If <see cref="AddCreationBuildStep.Yes"/> adds a build step
    /// <see cref="Default.CreationBuildAction"/> for <see cref="UnitInfo"/>(<see name="TRedirect"/>, <see cref="token"/>)
    /// as a creation build step.</param>
    /// <typeparam name="TRedirect"></typeparam>
    public AdjusterSugar As<TRedirect>(object token = null, AddCreationBuildStep addDefaultCreateAction = AddCreationBuildStep.Yes)
    {
      var redirectTo = typeof(TRedirect);

      //Todo: should this check be moved inside RedirectTypeBuildAction?
      if(!typeof(T).IsAssignableFrom(redirectTo))
        throw new Exception("Not assignable");

      _unitSequenceMatcher.AddBuildAction(BuildStage.Redirect, new RedirectTypeBuildAction(redirectTo, token), 0);

      var nextBuildStep = _unitSequenceMatcher;
      if (addDefaultCreateAction == AddCreationBuildStep.Yes)
      {
        nextBuildStep = new WeakUnitSequenceMatcher(Match.Type<TRedirect>(token), UnitSequenceMatchingWeight.WeakMatchingTypeUnit);

        _unitSequenceMatcher
          .AddOrGetUnitMatcher(nextBuildStep).AddBuildAction(BuildStage.Create, Default.CreationBuildAction, 0);
      }

      return new AdjusterSugar(nextBuildStep);
    }

    public void CreatedBy([NotNull] Func<UnitBuilder, T> factoryMethod)
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Create, new CreateWithFactoryMethodBuildAction<T>(factoryMethod), 0);
    }

    public void CreatedBy<T1>([NotNull] Func<UnitBuilder, T1, T> factoryMethod)
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Create, new CreateWithFactoryMethodBuildAction<T1, T>(factoryMethod), 0);
    }

    public void CreatedBy<T1, T2>([NotNull] Func<UnitBuilder, T1, T2, T> factoryMethod)
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Create, new CreateWithFactoryMethodBuildAction<T1, T2, T>(factoryMethod), 0);
    }

    public void CreatedBy<T1, T2, T3>([NotNull] Func<UnitBuilder, T1, T2, T3, T> factoryMethod)
    {
      _unitSequenceMatcher.AddBuildAction(BuildStage.Create, new CreateWithFactoryMethodBuildAction<T1, T2, T3, T>(factoryMethod), 0);
    }
  }
}