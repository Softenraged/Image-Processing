using System;
using System.Drawing;
using System.Reflection;
using System.Resources;

using BenchmarkDotNet.Attributes;

using ImageProcessing.DomainModel.Model.RgbFilters.Implementation.Binary;
using ImageProcessing.DomainModel.Model.RgbFilters.Interface;

namespace ImageProcessing.DomainModel.Benchmark.RgbFilter.Binary
{
    [SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
    internal sealed class BinaryFilterBenchmark : IDisposable
    {
        private IRgbFilter filter = new BinaryFilter();

        private Bitmap _frame1920x1080;
        private Bitmap _frame2560x1440;

        private int frameRate = 60;

        [GlobalSetup]
        public void Setup()
        {
            var manager = new ResourceManager("Frames/Frames.resx", Assembly.GetExecutingAssembly());

            _frame1920x1080 = new Bitmap((Image)manager.GetObject("1920x1080frame"));
            _frame2560x1440 = new Bitmap((Image)manager.GetObject("2560x1440frame"));

        }

        [Benchmark]
        public void ApplyBinaryFilterTo1920x1080()
        {
                filter.Filter(_frame1920x1080);           
        }

        [Benchmark]
        public void ApplyBinaryFilterTo1920x1080Frame60Fps()
        {
            for(var start = 0; start < frameRate; ++start)
            {
                filter.Filter(_frame1920x1080);
            }

        }

        [Benchmark]
        public void ApplyBinaryFilterTo2560x1440()
        {
            filter.Filter(_frame2560x1440);
        }

        [Benchmark]
        public void ApplyBinaryFilterTo2560x1440Frame60Fps()
        {
            for (var start = 0; start < frameRate; ++start)
            {
                filter.Filter(_frame2560x1440);
            }
        }

        [GlobalCleanup]
        public void Dispose()
        {
            _frame1920x1080.Dispose();
            _frame2560x1440.Dispose();
        }

    }
}
