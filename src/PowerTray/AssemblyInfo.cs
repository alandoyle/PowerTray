using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PowerTray;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(Version.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration(Version.BuildInfo)]
[assembly: AssemblyCompany(Version.PublisherName)]
[assembly: AssemblyProduct(Version.Name)]
[assembly: AssemblyCopyright("Copyright © " + Version.PublisherName + " - " + ((Version.StartYear == Version.BuiltYear) ? Version.StartYear : (Version.StartYear + "-" + Version.BuiltYear)))]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2ea1850a-e82f-463b-a275-da81a11515fa")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Version.Full)]
[assembly: AssemblyFileVersion(Version.Full)]
