using Microsoft.ML.OnnxRuntime.Tensors;

namespace AwesomeImages.Models.Yolo;

public class Feed(DenseTensor<float> tensor) 
{
    public long Id { get; set; }
    public int OriginHeight { get; set; }
    public int OriginWidth { get; set; }
    public float[] Ratio { get; set; } = [0, 0];
    public float[] DwDh { get; set; } = [0, 0];
    public DenseTensor<float> Tensor { get; set; } = tensor; 
}