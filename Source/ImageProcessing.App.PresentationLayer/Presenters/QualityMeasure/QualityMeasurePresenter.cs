using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

using ImageProcessing.App.PresentationLayer.Presenters.Base;
using ImageProcessing.App.PresentationLayer.ViewModel.QualityMeasure;
using ImageProcessing.App.PresentationLayer.Views.QualityMeasure;
using ImageProcessing.App.ServiceLayer.Builders.ChartBuilder.Interface;
using ImageProcessing.App.ServiceLayer.Services.Distributions.BitmapLuminance.Interface;
using ImageProcessing.Microkernel.MVP.Controller.Interface;

namespace ImageProcessing.App.PresentationLayer.Presenters
{
    internal sealed class QualityMeasurePresenter
        : BasePresenter<IQualityMeasureView, QualityMeasureViewModel>
    {
        private readonly IBitmapLuminanceDistributionService _distributionService;
        private readonly IChartSeriesBuilder _builder;

        public QualityMeasurePresenter(IAppController controller, 
                                       IBitmapLuminanceDistributionService distibutionService,
                                       IChartSeriesBuilder builder)
            : base(controller) 
        {
            _distributionService =  distibutionService;
            _builder = builder;
        }

        public override void Run(QualityMeasureViewModel vm)
            => DoWorkBeforeShow(vm);  
        
        private async Task DoWorkBeforeShow(QualityMeasureViewModel vm)
        {
            var chart = View.GetChart;
          
            var map = await Task.Run(
                () => BuildSeries(vm)
            ).ConfigureAwait(true);

            foreach(var pair in map)
            {
                chart.Series[pair.Key] = pair.Value;
            }

            View.Show();
        }

        private Dictionary<string, Series> BuildSeries(QualityMeasureViewModel vm)
        {
            var result = new Dictionary<string, Series>();

            var random = new Random(Guid.NewGuid().GetHashCode());

            while(vm.Queue.TryDequeue(out var bitmap))
            {
                var variance = new List<decimal>();
                var names    = new List<string>();

                for (var graylevel = 0; graylevel < 255; graylevel += 15)
                {
                    names.Add($"{graylevel}-{graylevel + 15}");
                    variance.Add(_distributionService.GetConditionalVariance((graylevel, graylevel + 15), bitmap));
                }

                var key = bitmap.Tag.ToString();

                _builder.SetName(key)
                        .SetColor(Color.FromArgb(random.Next(0, 256),
                                                 random.Next(0, 256),
                                                 random.Next(0, 256))
                        )
                        .SetMarkerStyle(MarkerStyle.None)
                        .SetChartType(SeriesChartType.Column)
                        .SetLabelAngle(-90)
                        .SetVisibleInLegend(true);

                var series = _builder.Build();
                    series.Points.DataBindXY(names, variance);

                result.Add(key, series);

                bitmap.Dispose();
            }

            return result;
        }
    }
}

