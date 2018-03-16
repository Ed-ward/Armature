﻿using System;
using System.Diagnostics;
using System.Reflection;
using Armature.Core;
using Armature.Logging;

namespace Armature.Framework.BuildActions
{
  /// <summary>
  ///   Build action instantiates an object of type <see cref="UnitInfo.Id" /> as Type using reflection
  /// </summary>
  public class CreateByReflectionBuildAction : IBuildAction
  {
    // it has no state, so use a singleton
    public static readonly IBuildAction Instance = new CreateByReflectionBuildAction();

    private CreateByReflectionBuildAction() { }

    public void Process(IBuildSession buildSession)
    {
      if (buildSession.BuildResult == null)
      {
        var type = buildSession.GetUnitUnderConstruction().GetUnitType();

        // ReSharper disable once PossibleNullReferenceException
        if (!type.IsInterface && !type.IsAbstract)
        {
          ConstructorInfo constructor;
          using(Log.Block(LogLevel.Trace, "Looking for constructor"))
            constructor = buildSession.GetConstructorOf(type);

          var parameters = constructor.GetParameters();

          if (parameters.Length == 0 && type.IsValueType) // do not create default value of value type, it can confuse logic
            return;

          try
          {
            object instance;
            if (parameters.Length == 0)
            {
              instance = Activator.CreateInstance(type);
            }
            else
            {
              object[] valuesForParameters;
              using(Log.Block(LogLevel.Trace, "Looking for parameters"))
                valuesForParameters = buildSession.GetValuesForParameters(parameters);
              instance = Activator.CreateInstance(type, valuesForParameters);
            }

            buildSession.BuildResult = new BuildResult(instance);
          }
          catch (TargetInvocationException exception)
          {
            if (exception.InnerException != null)
              throw new Exception("", exception.InnerException);

            throw;
          }
        }
      }
    }

    [DebuggerStepThrough]
    public void PostProcess(IBuildSession buildSession) { }
    
    public override string ToString() => GetType().GetShortName();
  }
}