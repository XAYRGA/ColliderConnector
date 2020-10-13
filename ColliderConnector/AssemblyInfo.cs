using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ColliderConnector;
using MelonLoader;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(ColliderConnector.BuildInfo.Description)]
[assembly: AssemblyDescription(ColliderConnector.BuildInfo.Description)]
[assembly: AssemblyCompany(ColliderConnector.BuildInfo.Company)]
[assembly: AssemblyProduct(ColliderConnector.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ColliderConnector.BuildInfo.Author)]
[assembly: AssemblyTrademark(ColliderConnector.BuildInfo.Company)]
[assembly: AssemblyVersion(ColliderConnector.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ColliderConnector.BuildInfo.Version)]
[assembly: MelonInfo(typeof(ColliderConnector.ColliderConnector), ColliderConnector.BuildInfo.Name, ColliderConnector.BuildInfo.Version, ColliderConnector.BuildInfo.Author, ColliderConnector.BuildInfo.DownloadLink)]


// Create and Setup a MelonPluginGame to mark a Plugin as Universal or Compatible with specific Games.
// If no MelonPluginGameAttribute is found or any of the Values for any MelonPluginGame on the Mod is null or empty it will be assumed the Plugin is Universal.
// Values for MelonPluginGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]