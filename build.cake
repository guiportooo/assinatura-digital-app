#addin "nuget:?package=Cake.Coverlet&version=2.2.1"
#tool "nuget:?package=ReportGenerator&version=4.1.1"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var coverageOutputName = Argument("coverageOutputName", "");

Task("HotReload")
  .Description("Starts hot reloading process")
  .Does(() => 
{
  Information("Starting hot reloading process");

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
  var testSettings = new DotNetCoreTestSettings { Configuration = configuration};
  Information("Running unit tests");
  DotNetCoreTest("./AssinaturaDigital/AssinaturaDigital.UnitTests/", testSettings);
});

Task("Code-Coverage")
  .Description("Runs unit tests with code coverage")
  .Does(() =>
{
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
    ExcludeByFile = new List<string> { "../AssinaturaDigital/App.xaml.cs" },
    Exclude = new List<string> { "[*Tests?]*", "[*]AssinaturaDigital.Views.*" } 
  };
}

void GenerateCoverageReport()
{
  if(string.IsNullOrWhiteSpace(coverageOutputName))
    return;

  var reportGeneratorSettings = new ReportGeneratorSettings()
  {
    HistoryDirectory = new DirectoryPath("./CoverageResults/ReportsHistory")
  };
  var coverageResultsName = $"./CoverageResults/{coverageOutputName}.opencover.xml";
  Information($"Generating coverage report for {coverageResultsName}");
  ReportGenerator(coverageResultsName, "./CoverageResults/ReportGeneratorOutput", reportGeneratorSettings);
}

Task("Coverage-Report")
  .Description("Generates coverage report")
  .Does(() =>
{
  GenerateCoverageReport();
});

Task("Default")
  .IsDependentOn("Unit-Tests");

RunTarget(target);