﻿using System.Linq;
using System.Reflection;
using Armature.Core.Logging;

namespace Armature.Core
{
  /// <summary>
  ///   Builds an argument for the property marked with <see cref="InjectAttribute"/> using <see cref="InjectAttribute.InjectionPointId"/>
  ///   as the <see cref="UnitId.Key"/>
  /// </summary>
  public class BuildArgumentByPropertyInjectPointId : IBuildAction
  {
    public static readonly IBuildAction Instance = new BuildArgumentByPropertyInjectPointId();

    public void Process(IBuildSession buildSession)
    {
      var propertyInfo = (PropertyInfo) buildSession.GetUnitUnderConstruction().Kind!;

      var attribute = propertyInfo
                     .GetCustomAttributes<InjectAttribute>()
                     .SingleOrDefault();

      if(attribute is null)
      {
        Log.WriteLine(LogLevel.Info, () => $"{this} => property is not marked with InjectAttribute");
      }
      else
      {
        var unitInfo = new UnitId(propertyInfo.PropertyType, attribute.InjectionPointId);
        buildSession.BuildResult = buildSession.BuildUnit(unitInfo);
      }
    }

    public void PostProcess(IBuildSession buildSession) { }
  }
}