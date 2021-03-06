using System;
using System.Drawing;

using ImageProcessing.App.DomainLayer.DomainFactory.Rgb.RgbFilter.Interface;
using ImageProcessing.App.ServiceLayer.Services.ColorMatrix.Implementation;
using ImageProcessing.App.ServiceLayer.Services.ColorMatrix.Interface;

using NSubstitute;

using NUnit.Framework;

namespace ImageProcessing.App.ServiceLayer.UnitTests.Services.RgbFilters
{
    [TestFixture]
    internal sealed class RgbFilterServiceTests : IDisposable
    {
        private IRgbFilterFactory _filterFactory;
        private IColorMatrixService _filterService;
        private Bitmap _frame1920x1080;

        [SetUp]
        public void SetUp()
        {
            _frame1920x1080 = new Bitmap(Properties.Frames._1920x1080frame);

            _filterFactory = Substitute.For<IRgbFilterFactory>();
            _filterService = new ColorMatrixService();
        }

        [TearDown]
        public void Dispose()
        {
            _frame1920x1080.Dispose();
        }
    }
}
