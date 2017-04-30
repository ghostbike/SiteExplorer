using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Media;


namespace SiteExplorer
{
    public partial class SectionsExample : Form
    {
        public SectionsExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(7),
                        new ObservableValue(4)
                    },
                    PointGeometry = DefaultGeometries.None,
                    StrokeThickness = 4,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                   
                    
                    
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(18),
                        new ObservableValue(8),
                        new ObservableValue(7),
                        new ObservableValue(5)
                    },
                    PointGeometry = DefaultGeometries.None,
                    StrokeThickness = 4,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };

            cartesianChart1.AxisY.Add(new Axis
            {
                Sections = new SectionsCollection
                {
                    new AxisSection
                    {
                        FromValue = 8.5,
                        ToValue = 8.5,
                        Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 213, 72))
                    },
                    new AxisSection
                    {
                        Label = "Good",
                        FromValue = 4,
                        ToValue = 8,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(204,204,204),
                            Opacity = .4
                        }
                    },
                    new AxisSection
                    {
                        Label = "Bad",
                        FromValue = 0,
                        ToValue = 4,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(254,132,132),
                            Opacity = .4
                        }
                    }
                }
            });

        }
    }
}
