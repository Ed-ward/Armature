﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Armature.Core;
using Armature.Core.BuildActions;
using Armature.Core.BuildActions.Creation;
using Armature.Core.BuildActions.Property;
using Armature.Extensibility;


namespace Armature
{
  public class PropertyValueTuner : BuildActionExtensibility
  {
    public PropertyValueTuner(IUnitMatcher propertyUnitMatcher, IBuildAction getPropertyAction, int weight)
      : base(propertyUnitMatcher, getPropertyAction, weight) { }

    /// <summary>
    ///   Inject the <paramref name="value" /> into the property
    /// </summary>
    public PropertyValueBuildPlan UseValue(object? value) => new(UnitMatcher, BuildAction, new SingletonBuildAction(value), Weight);

    /// <summary>
    ///   For building a value for the property use <see cref="PropertyInfo.PropertyType" /> and <paramref name="key" />
    /// </summary>
    public PropertyValueBuildPlan UseKey(object key)
    {
      if(key is null) throw new ArgumentNullException(nameof(key));

      return new PropertyValueBuildPlan(UnitMatcher, BuildAction, new CreatePropertyValueBuildAction(key), Weight);
    }

    /// <summary>
    ///   For building a value for the property use factory method />
    /// </summary>
    public PropertyValueBuildPlan UseFactoryMethod(Func<IBuildSession, object> factoryMethod)
      => new(UnitMatcher, BuildAction, new CreateByFactoryMethodBuildAction<object>(factoryMethod), Weight);

    /// <summary>
    ///   For building a value for the property use <see cref="PropertyInfo.PropertyType" /> and <see cref="InjectAttribute.InjectionPointId" /> as a key
    /// </summary>
    public PropertyValueBuildPlan UseInjectPointIdAsKey()
      => new(UnitMatcher, BuildAction, CreatePropertyValueForInjectPointBuildAction.Instance, Weight);
  }

  [SuppressMessage("ReSharper", "UnusedTypeParameter")]
  public class PropertyValueTuner<T> : PropertyValueTuner
  {
    public PropertyValueTuner(IUnitMatcher propertyUnitMatcher, IBuildAction getPropertyAction, int weight)
      : base(propertyUnitMatcher, getPropertyAction, weight) { }
  }
}
