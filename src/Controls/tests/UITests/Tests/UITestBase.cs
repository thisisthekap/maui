using System.Reflection;
using Microsoft.Maui.Appium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using TestUtils.Appium.UITests;

namespace Microsoft.Maui.AppiumTests
{
#if ANDROID
	[TestFixture(TestDevice.Android)]
#elif IOSUITEST
	[TestFixture(TestDevice.iOS)]
#elif MACUITEST
	[TestFixture(TestDevice.Mac)]
#elif WINTEST
	[TestFixture(TestDevice.Windows)]
#else
	[TestFixture(TestDevice.iOS)]
	[TestFixture(TestDevice.Mac)]
	[TestFixture(TestDevice.Windows)]
	[TestFixture(TestDevice.Android)]
#endif
	public class UITestBase : UITestContextTestBase
	{
		readonly TestDevice _testDevice;
		public UITestBase(TestDevice device)
		{
			_testDevice = device;
		}

		protected virtual void FixtureSetup() { }

		protected virtual void FixtureTeardown() { }

		[TearDown]
		public void UITestBaseTearDown()
		{
			var testOutcome = TestContext.CurrentContext.Result.Outcome;
			if (testOutcome == ResultState.Error ||
				testOutcome == ResultState.Failure)
			{
				var logDir = (Path.GetDirectoryName(Environment.GetEnvironmentVariable("APPIUM_LOG_FILE")) ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!;

				_ = App.Screenshot(Path.Combine(logDir, $"{TestContext.CurrentContext.Test.MethodName}-ScreenShot"));

				if (App is IApp2 app2)
				{
					var pageSource = app2.ElementTree;
					File.WriteAllText(Path.Combine(logDir, $"{TestContext.CurrentContext.Test.MethodName}-PageSource.txt"), pageSource);
				}
			}
		}

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			InitialSetup(TestContextSetupFixture.TestContext);
			FixtureSetup();
		}

		[OneTimeTearDown()]
		public void OneTimeTearDown()
		{
			FixtureTeardown();
		}

		public override TestConfig GetTestConfig()
		{
			var appProjectFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "..\\..\\..\\..\\..\\samples\\Controls.Sample.UITests");
			var appProjectPath = Path.Combine(appProjectFolder, "Controls.Sample.UITests.csproj");
			var testConfig = new TestConfig(_testDevice, "com.microsoft.maui.uitests")
			{
				BundleId = "com.microsoft.maui.uitests",
				AppProjectPath = appProjectPath
			};
			var windowsExe = "Controls.Sample.UITests.exe";
			var windoesExePath = Path.Combine(appProjectFolder, $"bin\\{testConfig.Configuration}\\{testConfig.FrameworkVersion}-windows10.0.20348\\win10-x64\\{windowsExe}");

			switch (_testDevice)
			{
				case TestDevice.iOS:
					testConfig.DeviceName = "iPhone X";
					testConfig.PlatformVersion = Environment.GetEnvironmentVariable("IOS_PLATFORM_VERSION") ?? "14.4";
					testConfig.Udid = Environment.GetEnvironmentVariable("IOS_SIMULATOR_UDID") ?? "";
					break;
				case TestDevice.Windows:
					testConfig.DeviceName = "WindowsPC";
					testConfig.AppPath = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WINDOWS_APP_PATH"))
						? windoesExePath
						: Environment.GetEnvironmentVariable("WINDOWS_APP_PATH");
					break;
			}

			return testConfig;
		}

		public void VerifyScreenshot(string? name = null, Assembly? assembly = null)
		{
			if (name == null)
				name = TestContext.CurrentContext.Test.MethodName;

			if (assembly == null)
				assembly = Assembly.GetCallingAssembly();
			string assemblyDirectory = Path.GetDirectoryName(assembly.Location)!;
			string projectRootDirectory = Path.GetFullPath(Path.Combine(assemblyDirectory, "..\\..\\.."));

			Screenshot? screenshot = Driver?.GetScreenshot();
			if (screenshot == null)
			{
				throw new InvalidOperationException("Failed to get screenshot");
			}

			byte[] screenshotBytes = screenshot.AsByteArray;

			string platform = _testDevice switch
			{
				TestDevice.Android => "android",
				TestDevice.iOS => "ios",
				TestDevice.Mac => "mac",
				TestDevice.Windows => "windows",
				_ => throw new NotImplementedException($"Unknown device type {_testDevice}"),
			};

			string baselineDirectory = Path.Combine(projectRootDirectory, "snapshots-baseline");
			string diffsDirectory = Path.Combine(projectRootDirectory, "snapshots-diff");
			string imageFileName = $"{name}-{platform}.png";

			if (!VisualTestingUtils.VerifyBaselineImageExists(baselineDirectory, imageFileName, screenshotBytes, diffsDirectory))
			{
				Assert.Fail(
					$"Baseline snapshot not yet created: {Path.Combine(baselineDirectory, imageFileName)}\n" +
					$"Ensure new snapshot is correct:    {Path.Combine(diffsDirectory, imageFileName)}\n" +
					$"and if so, copy it to the snapshots-baseline directory");
			}

			if (!VisualTestingUtils.VerifyImagesSame(baselineDirectory, imageFileName, screenshotBytes, 0.1, out double percentageDifference, diffsDirectory))
			{
				Assert.Fail($"Snapshot different than baseline: {imageFileName} ({percentageDifference}% difference)");
			}
		}
	}
}
