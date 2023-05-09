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
#endif
	public class AppiumPlatformsTestBase : AppiumUITestBase
	{
		TestDevice _testDevice;

		public AppiumPlatformsTestBase(TestDevice device)
		{
			_testDevice = device;
		}

		[TearDown]
		public void TearDown()
		{
			var testOutcome = TestContext.CurrentContext.Result.Outcome;
			if (testOutcome == ResultState.Error ||
				testOutcome == ResultState.Failure)
			{
				var logDir = (Path.GetDirectoryName(Environment.GetEnvironmentVariable("APPIUM_LOG_FILE")) ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!;

				var pageSource = Driver?.PageSource!;
				File.WriteAllText(Path.Combine(logDir, $"{TestContext.CurrentContext.Test.MethodName}-PageSource.txt"), pageSource);

				var screenshot = Driver?.GetScreenshot();
				screenshot?.SaveAsFile(Path.Combine(logDir, $"{TestContext.CurrentContext.Test.MethodName}-ScreenShot.png"));
			}

			//this crashes on Android
			if (!IsAndroid && !IsWindows)
				Driver?.ResetApp();
		}


		[OneTimeSetUp()]
		public void OneTimeSetup()
		{
			InitialSetup();
		}

		[OneTimeTearDown()]
		public void OneTimeTearDown()
		{
			Teardown();
		}

		public override TestConfig GetTestConfig()
		{
			var testConfig = new TestConfig(_testDevice, "com.microsoft.maui.uitests")
			{
				BundleId = "com.microsoft.maui.uitests",
			};
			switch (_testDevice)
			{
				case TestDevice.Android:
					//_appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "com.microsoft.maui.uitests");
					// activity { com.microsoft.maui.uitests / crc64fa090d87c1ce7f0b.MainActivity}
					//_appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, "MainActivity");
					break;
				case TestDevice.iOS:
					testConfig.DeviceName = "iPhone X";
					testConfig.PlatformVersion = Environment.GetEnvironmentVariable("IOS_PLATFORM_VERSION") ?? "14.4";
					testConfig.Udid = Environment.GetEnvironmentVariable("IOS_SIMULATOR_UDID") ?? "";
					break;
				case TestDevice.Mac:

					break;
				case TestDevice.Windows:
					testConfig.DeviceName = "WindowsPC";
					testConfig.AppPath = Environment.GetEnvironmentVariable("WINDOWS_APP_PATH") ?? "C:\\my-projects\\maui\\src\\Controls\\samples\\Controls.Sample.UITests\\bin\\Debug\\net7.0-android\\Controls.Sample.UITests.dll";
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
			string projectRootDirectory = assembly.Location;

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

			bool baselineImageExists = !VisualTestingUtils.VerifyBaselineImageExists(baselineDirectory, imageFileName, screenshotBytes, diffsDirectory);
			Assert.True(baselineImageExists, $"Baseline image doesn't exist: {imageFileName}");

			bool screenshotsMatch = VisualTestingUtils.VerifyImagesSame(baselineDirectory, imageFileName, screenshotBytes, 1.0, out double percentageDifference, diffsDirectory);
			Assert.True(screenshotsMatch, $"Screenshot different than baseline: {imageFileName} ({percentageDifference}% difference)");
		}
	}
}
