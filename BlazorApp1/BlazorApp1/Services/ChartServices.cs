using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Util;

namespace CampusSafety.Admin.Services
{
    public class ChartService
    {
        public async Task<LineConfig> GetTrendChartConfig(string dateRange, string metric)
        {
            var config = new LineConfig
            {
                Options = new LineOptions
                {
                    Responsive = true,
                    Plugins = new Plugins
                    {
                        Legend = new Legend
                        {
                            Display = true,
                            Position = Position.Top
                        },
                        Title = new Title
                        {
                            Display = true,
                            Text = "Incident Trends"
                        }
                    },
                    Scales = new Scales
                    {
                        X = new CartesianLinearAxis
                        {
                            Display = true,
                            Title = new Title
                            {
                                Display = true,
                                Text = "Date"
                            }
                        },
                        Y = new CartesianLinearAxis
                        {
                            Display = true,
                            Title = new Title
                            {
                                Display = true,
                                Text = metric == "count" ? "Incident Count" : 
                                      metric == "severity" ? "Average Severity" : "Response Time (hours)"
                            }
                        }
                    }
                }
            };

            // Sample data
            var dataset = new LineDataset<double>(new[] { 12.0, 19.0, 3.0, 5.0, 2.0, 3.0 })
            {
                Label = metric == "count" ? "Incidents" : metric == "severity" ? "Avg Severity" : "Response Time",
                BackgroundColor = ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(128, 79, 70, 229)),
                BorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(79, 70, 229)),
                Fill = true,
                Tension = 0.4
            };

            config.Data.Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            config.Data.Datasets.Add(dataset);

            return config;
        }

        public async Task<PieConfig> GetCategoryChartConfig()
        {
            var config = new PieConfig
            {
                Options = new PieOptions
                {
                    Responsive = true,
                    Plugins = new Plugins
                    {
                        Legend = new Legend
                        {
                            Position = Position.Right
                        }
                    }
                }
            };

            var dataset = new PieDataset<double>(new[] { 35.0, 25.0, 18.0, 12.0, 10.0 })
            {
                BackgroundColor = new[]
                {
                    ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 239, 68, 68)),  // Red
                    ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 59, 130, 246)),  // Blue
                    ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 16, 185, 129)),  // Green
                    ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 245, 158, 11)),  // Yellow
                    ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 139, 92, 246))   // Purple
                }
            };

            config.Data.Labels = new[] { "Hazards", "Security", "Maintenance", "Theft", "Other" };
            config.Data.Datasets.Add(dataset);

            return config;
        }

        public async Task<BarConfig> GetTimeOfDayChartConfig()
        {
            var config = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Plugins = new Plugins
                    {
                        Legend = new Legend { Display = false }
                    },
                    Scales = new Scales
                    {
                        X = new CartesianLinearAxis
                        {
                            Title = new Title { Text = "Time of Day" }
                        },
                        Y = new CartesianLinearAxis
                        {
                            Title = new Title { Text = "Incidents" },
                            BeginAtZero = true
                        }
                    }
                }
            };

            var dataset = new BarDataset<double>(new[] { 2.0, 1.0, 0.0, 3.0, 8.0, 12.0, 15.0, 18.0, 20.0, 14.0, 9.0, 4.0 })
            {
                BackgroundColor = ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 79, 70, 229)),
                BorderColor = ColorUtil.FromDrawingColor(System.Drawing.Color.FromArgb(255, 79, 70, 229)),
                BorderWidth = 1,
                BorderRadius = 6
            };

            config.Data.Labels = new[] { "12AM", "2AM", "4AM", "6AM", "8AM", "10AM", "12PM", "2PM", "4PM", "6PM", "8PM", "10PM" };
            config.Data.Datasets.Add(dataset);

            return config;
        }
    }
}