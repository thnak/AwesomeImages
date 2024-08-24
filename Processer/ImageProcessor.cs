using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AwesomeImages.Processer;

public static class ImageProcessor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns>tensor, origin height, origin width</returns>
    public static (DenseTensor<float>, int, int) PreprocessImage(string imagePath, int width, int height)
    {
        // Load the image using ImageSharp
        using var image = Image.Load<Rgb24>(imagePath);
        int originWidth = image.Width;
        int originHeight = image.Height;
        // Resize the image to the desired dimensions (e.g., 224x224)
        image.Mutate(x => x.Resize(width, height));

        // Output tensor

        DenseTensor<float> tensor = new([3, height, width]);

        // Process the image pixel data
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < height; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < width; x++)
                {
                    var pixel = row[x];
                    tensor[0, y, x] = pixel.R / 255f; // Red channel
                    tensor[1, y, x] = pixel.G / 255f; // Green channel
                    tensor[2, y, x] = pixel.B / 255f; // Blue channel
                }
            }
        });

        return (tensor, originHeight, originWidth);
    }
}