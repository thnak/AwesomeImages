using System.Collections.Concurrent;
using AwesomeImages.Models.Yolo;
using AwesomeImages.Processer;

namespace AwesomeImages.Data;

public class DataLoader : IDisposable
{
    private int MaxProcess => Environment.ProcessorCount - 1;
    public BlockingCollection<Feed> Feeds { get; set; }
    public int BatchSize { get; set; }
    public int CollectionSize { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }

    private CancellationTokenSource Cts { get; set; } = new();
    private string[] ImagePaths { get; set; } = [];
    private string[] ImageExtensions { get; set; } = ["*.jpg", "*.png"];

    public DataLoader(string folder, int height, int width, int batchSize = 1, int collectionSize = 12)
    {
        Height = height;
        Width = width;
        BatchSize = batchSize;
        CollectionSize = collectionSize;
        Feeds = new BlockingCollection<Feed>(collectionSize);
        List<string> images = new List<string>();
        foreach (var extension in ImageExtensions)
        {
            var files = Directory.GetFiles(folder, extension);
            images.AddRange(files);
        }

        ImagePaths = images.ToArray();
        Task.Factory.StartNew(Run, Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void Run()
    {
        var rangePartitioner = Partitioner.Create(ImagePaths, true);

        Parallel.ForEach(rangePartitioner, Process);
        Feeds.CompleteAdding();
    }

    private void Process(string imagePath, ParallelLoopState state, long index)
    {
        // Preprocess the image and add it to the queue
        var preprocessedData = ImageProcessor.PreprocessImage(imagePath, Width, Height);
        var feed = new Feed(preprocessedData.Item1)
        {
            Id = index
        };
        Feeds.Add(feed, Cts.Token);
    }

    public int Count => ImagePaths.Length;

    public void Dispose()
    {
        Feeds.Dispose();
        Cts.Dispose();
    }
}