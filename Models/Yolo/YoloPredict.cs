namespace AwesomeImages.Models.Yolo;

/// <summary>
/// Base output from yolov7 non-maxsuppression
/// </summary>
public class YoloPredict
{
    /// <summary>
    /// index of batch
    /// </summary>
    public int BatchId { get; set; }

    /// <summary>
    /// bounding box with shape x y height width
    /// </summary>
    public int[] Bbox { get; init; } = [0, 0, 0, 0];

    /// <summary>
    /// box with shape x0 y0 x1 y1
    /// </summary>
    public int[] Box { get; init; } = [0, 0, 0, 0];

    /// <summary>
    /// category index
    /// </summary>
    public int ClassIdx { get; init; }

    /// <summary>
    /// category name
    /// </summary>
    public string? ClassName { get; init; }

    /// <summary>
    /// confident score
    /// </summary>
    public float Score { get; init; }
}