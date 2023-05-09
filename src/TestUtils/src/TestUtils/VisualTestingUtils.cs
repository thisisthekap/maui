using System.IO;
using ImageMagick;

namespace Microsoft.Maui
{
	public static class VisualTestingUtils
	{
		public static bool VerifyBaselineImageExists(string baselineDirectory, string imageFileName, byte[] actualImageBytes, string diffsDirectory)
		{
			if (!File.Exists(Path.Combine(baselineDirectory, imageFileName)))
			{
				Directory.CreateDirectory(diffsDirectory);

				MagickImage actualImage = new MagickImage(new MemoryStream(actualImageBytes));

				actualImage.Write(Path.Combine(diffsDirectory, imageFileName));

				return false;
			}

			return true;
		}

		public static bool VerifyImagesSame(string baselineDirectory, string imageFileName, byte[] actualImageBytes, double percentageDifferenceThreshold,
			out double percentageDifference, string diffsDirectory)
		{
			MagickImage baselineImage = new MagickImage(Path.Combine(baselineDirectory, imageFileName));

			MagickImage actualImage = new MagickImage(new MemoryStream(actualImageBytes));

			ErrorMetric test = ErrorMetric.Fuzz;
			MagickImage diffImage = new MagickImage();
			diffImage.Format = MagickFormat.Png;

			percentageDifference = baselineImage.Compare(actualImage, test, diffImage, Channels.Red) * 100.0;

			if (percentageDifference > percentageDifferenceThreshold)
			{
				Directory.CreateDirectory(diffsDirectory);
				actualImage.Write(Path.Combine(diffsDirectory, imageFileName));
				diffImage.Write(Path.Combine(diffsDirectory, Path.GetFileNameWithoutExtension(imageFileName) + "-diff.png"));

				return false;
			}

			return true;
		}
	}
}