using System;
using System.Reflection;

#pragma warning disable 7035

[assembly: CLSCompliant(true)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
 [assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyProduct("Armature")]
[assembly: AssemblyCopyright("Copyright © 2018 Ed Pavlov")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("0.9")]
[assembly: AssemblyInformationalVersion("0.9 Beta")]
//[assembly: AssemblyFileVersion("0.9.*.*")]
