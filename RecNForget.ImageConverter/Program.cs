﻿// https://efundies.com/replace-a-color-in-an-image-with-csharp/

using Efundies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.ImageConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			string basePath = @"..\..\..\icon_generation\icons";
			DirectoryInfo baseDirectory = new DirectoryInfo(basePath);

			DirectoryInfo blackImagesFolder = new DirectoryInfo(Path.Combine(basePath, "black"));

			DirectoryInfo disabledImagesFolder = new DirectoryInfo(Path.Combine(basePath, "disabled"));
			if (!disabledImagesFolder.Exists)
			{
				disabledImagesFolder.Create();
			}

			DirectoryInfo whiteImagesFolder = new DirectoryInfo(Path.Combine(basePath, "white"));
			if (!whiteImagesFolder.Exists)
			{
				whiteImagesFolder.Create();
			}

			foreach (var blackImage in baseDirectory.GetFiles("*.png", SearchOption.TopDirectoryOnly))
			{
				var tmpBmp = new Bitmap(blackImage.FullName);

				ReplaceAllButThisColor(
					tmpBmp,
					Color.Transparent,
					Color.White);

				tmpBmp.Save(Path.Combine(whiteImagesFolder.FullName, blackImage.Name.Replace(".png", ".white.png")));

				ReplaceAllButThisColor(
					tmpBmp,
					Color.Transparent,
					Color.LightGray);

				tmpBmp.Save(Path.Combine(disabledImagesFolder.FullName, blackImage.Name.Replace(".png", ".disabled.png")));
			}
		}

		// replace one color with another
		private static void ReplaceAllButThisColor(Bitmap bmp, Color allButThisColor, Color newColor)
		{
			var lockedBitmap = new LockBitmap(bmp);
			lockedBitmap.LockBits();

			for (int y = 0; y < lockedBitmap.Height; y++)
			{
				for (int x = 0; x < lockedBitmap.Width; x++)
				{
					if (
						lockedBitmap.GetPixel(x, y).A == allButThisColor.A
						&&
						lockedBitmap.GetPixel(x, y).R == allButThisColor.R
						&&
						lockedBitmap.GetPixel(x, y).G == allButThisColor.G
						&&
						lockedBitmap.GetPixel(x, y).B == allButThisColor.B
						)
					{
						// this is the allButThisColor dont do anything
					}
					else
					{
						lockedBitmap.SetPixel(x, y, newColor);
					}
				}
			}
			lockedBitmap.UnlockBits();
		}
	}
}