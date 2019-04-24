#addin "nuget:?package=Cake.Coverlet&version=2.2.1"
#tool "nuget:?package=ReportGenerator&version=4.1.1"
#addin "nuget:?package=Cake.Json&version=3.0.0"
#addin "Cake.Plist"
#addin "Cake.AndroidAppManifest"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var coverageOutputName = Argument("coverageOutputName", "");
var versionName = Argument("versionName", $"1.0.{DateTime.Now.ToString("yyMMdd")}");
var androidManifestPath = "AssinaturaDigital/AssinaturaDigital.Android/Properties/AndroidManifest.xml";
var iOSPlistPath = "AssinaturaDigital/AssinaturaDigital.iOS/Info.plist";
var configurationFilePath = Argument("configurationFilePath", "");
var iOSAppCenterSecret = Argument("iOSAppCenterSecret", "");
var androidAppCenterSecret = Argument("androidAppCenterSecret", "");
var secondsToGenerateToken = Argument("secondsToGenerateToken", "60");

Task("Hot-Reload")
  .Description("Starts hot reloading process")
  .Does(() => 
{
  Information("Starting Hot-Reload task");

  StartProcess("adb", "forward tcp:8000 tcp:8000");

  var processSettings = new ProcessSettings()
  {
    Arguments = $"./tools/Xamarin.Forms.HotReload.Observer.exe u=http://127.0.0.1:8000,http://127.0.0.1:4291",
  };
 
  using (var hotReloadProcess = StartAndReturnProcess("mono", processSettings))
  {
    hotReloadProcess.WaitForExit();
  }
});

Task("Unit-Tests")
  .Description("Runs unit tests")
  .Does(() =>
{
  Information("Starting Unit-Tests task");
  var testSettings = new DotNetCoreTestSettings { Configuration = configuration};
  DotNetCoreTest("./AssinaturaDigital/AssinaturaDigital.UnitTests/", testSettings);
});

Task("Code-Coverage")
  .Description("Runs unit tests with code coverage")
  .Does(() =>
{
  Information("Starting Code-Coverage task");
  var testSettings = new DotNetCoreTestSettings { Configuration = configuration};
  var coverletSettings = ReturnCoverletSettings();
  Information("Running unit tests with code coverage");
  DotNetCoreTest("./AssinaturaDigital/AssinaturaDigital.UnitTests/", testSettings, coverletSettings);
  GenerateCoverageReport();
});

CoverletSettings ReturnCoverletSettings()
{
  coverageOutputName = $"AssinaturaDigitalCoverage";

  return new CoverletSettings
  {
    CollectCoverage = true,
    CoverletOutputFormat = CoverletOutputFormat.opencover,
    CoverletOutputDirectory = Directory(@".\CoverageResults\"),
    CoverletOutputName = coverageOutputName,
    ExcludeByAttribute = new List<string> { "Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute" },
    Exclude = new List<string> 
    { 
      "[*Tests?]*", 
      "[*]AssinaturaDigital.Views.*", 
      "[*]AssinaturaDigital.Extensions.*",
      "[*]AssinaturaDigital.Converters.*",
      "[*]AssinaturaDigital.Utilities.*",
      "[*]AssinaturaDigital.Services.Fakes.*",
    },
  };
}

void GenerateCoverageReport()
{
  if(string.IsNullOrWhiteSpace(coverageOutputName))
    return;

  var reportGeneratorSettings = new ReportGeneratorSettings()
  {
    HistoryDirectory = new DirectoryPath("./CoverageResults/ReportsHistory"),
    ReportTypes = new List<ReportGeneratorReportType> 
    { 
      ReportGeneratorReportType.HtmlInline_AzurePipelines, 
      ReportGeneratorReportType.Badges 
    }
  };
  var coverageResultsName = $"./CoverageResults/{coverageOutputName}.opencover.xml";
  Information($"Generating coverage report for {coverageResultsName}");
  ReportGenerator(coverageResultsName, "./CoverageResults/ReportGeneratorOutput", reportGeneratorSettings);
}

Task("Coverage-Report")
  .Description("Generates coverage report")
  .Does(() =>
{
  Information("Starting Coverage-Report task");
  GenerateCoverageReport();
});

Task("Bump-iOS")
  .Description("Bumps iOS's public version")
  .Does(() =>
  { 
    Information("Starting Bump-iOS task");
    BumpiOSVersion(iOSPlistPath);
  });

Task("Bump-Android")
  .Description("Bumps Android's public version")
  .Does(() =>
  {  
    Information("Starting Bump-Android task");
    BumpAndroidVersion(androidManifestPath);
  });

Task("Bump")
  .IsDependentOn("Bump-iOS")
  .IsDependentOn("Bump-Android");

void BumpAndroidVersion(FilePath androidPath)
{
  var androidManifest = DeserializeAppManifest(androidPath);

  androidManifest.VersionName = $"{versionName}";
  Information("Bumping Android to versionName " + androidManifest.VersionName);

  androidManifest.WriteToFile(androidPath.FullPath);
}

void BumpiOSVersion(FilePath iOSPath)
{
  dynamic iOSPlist = DeserializePlist(iOSPath);

  iOSPlist["CFBundleVersion"] = $"{versionName}";
  Information("Bumping iOS to versioName " + iOSPlist["CFBundleVersion"]);

  SerializePlist(iOSPath, iOSPlist);
}

Task("Set-Configuration")
  .Description("Set configuration file values")
  .Does(() =>
  {
    Information("Starting Set-Configuration task");
    SetConfiguration();
  });

void SetConfiguration()
{
  var json = ParseJsonFromFile(new FilePath(configurationFilePath));
  Information(json);

  Information($"iOS's AppCenter secret: {iOSAppCenterSecret}");
  json["IOSAppCenterSecret"] = iOSAppCenterSecret;

  Information($"Android's AppCenter secret: {androidAppCenterSecret}");
  json["AndroidAppCenterSecret"] = androidAppCenterSecret;

  Information($"Seconds to generate token: {secondsToGenerateToken}");
  json["SecondsToGenerateToken"] = secondsToGenerateToken;

  Information("Saving configs.json file");
  Information(json);
  SerializeJsonToFile(configurationFilePath, json);
}

Task("Default")
  .IsDependentOn("Unit-Tests");

RunTarget(target);