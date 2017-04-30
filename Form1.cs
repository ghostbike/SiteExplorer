using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SiteExplorer
{
    public partial class Explorer : Form
    {



        //...code to handle form mouse over
        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        List<SiteSeries> locSiteSeries = new List<SiteSeries>();

        public BindingList<Site> sites = new BindingList<Site> ();

        public Explorer()
        {
            InitializeComponent();

            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

           SectionsExample frm2 = new SectionsExample();
            frm2.Show();
        }

        public void generateSiteList(List<SiteSeries> siteSeries) {


            var sites_query = from s in siteSeries
                              group s by s.SiteCode into sitesGrouped
                              select new { site_code = sitesGrouped.Key };


            dataGridView1.DataSource = sites_query.ToList();

            //Clear out the existing sites list before adding
            sites.Clear();
        
            foreach (var ts in sites_query) {


                var site_data = from t in siteSeries
                                where t.SiteCode == ts.site_code
                                orderby t.TimeSeries
                                select t;

                SortedDictionary<DateTime, double> siteValues = new SortedDictionary<DateTime, double>();
                
                foreach (var t in site_data) {
                    siteValues.Add(t.TimeSeries, t.ValueSeries);
                }

                Site site = new Site(ts.site_code, siteValues );

                sites.Add(site);


            }

            textBox1.Text = sites.Count.ToString();

            //Load Sample Data...
            //var selectedSite = sites.Where(s => s.SiteCode == 777);
            //foreach (var s in selectedSite) {
            //    s.FlaggedValues = new SortedDictionary<DateTime, double>()
            //    {
            //        { new DateTime(2016,8,10), 1000 },
            //        { new DateTime (2016, 8,17), 1000}
            //    };
   
            //}
                
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                //DataGridViewRow row = this.dataGridView1.SelectedRows[0];
                int r = Convert.ToInt32(row.Cells[0].Value);

                var chart_data = from s in sites
                                 where s.SiteCode == r
                                 select s;

                string RecordName = r.ToString();

                chart1.Series.Add(RecordName);

                foreach (var ts in chart_data)
                {
                    foreach (KeyValuePair<DateTime, double> kvp in ts.SiteValues)
                    {
                        chart1.Series[RecordName].Points.AddXY(kvp.Key, kvp.Value);

                    }

                    //if (ts.FlaggedValues != null)
                    //{
                    //    foreach (var fv in ts.FlaggedValues)
                    //    {

                    //        var dataPoint = chart1.Series[RecordName].Points.Where(o => o.XValue == fv.Key.ToOADate());
                    //        foreach (var markThisPoint in dataPoint)
                    //        {
                    //            markThisPoint.MarkerStyle = MarkerStyle.Diamond;
                    //            markThisPoint.MarkerColor = Color.Blue;
                    //            markThisPoint.MarkerSize = 15;
                    //        }

                    //    }
                }

                // chart1.Series[RecordName].Points[8].MarkerStyle = MarkerStyle.Diamond;
                // chart1.Series[RecordName].Points[8].MarkerColor = Color.Blue;
                //chart1.Series[RecordName].Points[8].MarkerSize = 15;






                chart1.Palette = ChartColorPalette.Bright;

                chart1.Series[RecordName].ChartType = SeriesChartType.Line;
                ////chart1.Series[RecordName].Color = Color.Black;
                chart1.Series[RecordName].BorderWidth = 1;

            }
        
            


        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
              
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;

                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2 &&
                            Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show("X=" + DateTime.FromOADate(prop.XValue).ToString("MM/dd/yy") + ", Y=" + prop.YValues[0], this.chart1,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            locSiteSeries.Clear(); 
            locSiteSeries = SiteSeries.ReadObservations(@"C:\Users\krys\Documents\SiteData.txt");
            //MessageBox.Show("Process Began");
            generateSiteList(locSiteSeries);
            
        }

        /// <summary>
        /// Function to modify the dictionary for the flagged weeks
        /// </summary>
        /// <param name="site"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="addRecord"></param>
        private void UpdateFlags(Site site, DateTime key, Double value, bool addRecord) {

            if (addRecord)
            {
                try { site.FlaggedValues.Add(key, value); }
                catch (Exception exception) { Console.WriteLine(exception.ToString()); }
            }
            else {
                site.FlaggedValues.Remove(key);
            }

        }

        /// <summary>
        /// Handle setting markers on the chart by clicking on the datapoints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            //capture the datapoint from the mouse
            Point newPoint = Point.Empty;
            newPoint = e.Location;

            //MessageBox.Show(Math.Round(chart1.ChartAreas[0].AxisX.PixelPositionToValue(newPoint.X),0).ToString());

            //Loop through the datapoints in the chart
            //check if it matches the rounded value of the x coordinate
            //if it does then update the raw data behind the chart (the dictionary) and 
            //recreate the chart with the new data...

            foreach (DataPoint dx in chart1.Series[0].Points) {
                //Console.WriteLine(chart1.ChartAreas[0].AxisX.PixelPositionToValue(newPoint.X).ToString());
                
                if (dx.XValue == Math.Round(chart1.ChartAreas[0].AxisX.PixelPositionToValue(newPoint.X),0)) {
                    DateTime newX = DateTime.FromOADate(dx.XValue);
                    double newY = chart1.ChartAreas[0].AxisY.PixelPositionToValue(newPoint.Y);


                    foreach (var ts in sites.Where(s=> s.SiteCode == Convert.ToInt32(chart1.Series[0].Name))) {
                        ts.SiteValues[newX] = newY;
                    }

                }
                //Redo the chart based on the updated dictionary
                dataGridView1_SelectionChanged(this, new EventArgs());
            }
            
        }

        private void buttonExtract_Click(object sender, EventArgs e)
        {
            foreach (Site s in sites.Where(n => n.FlaggedValues.Count>0)){
                if (s.FlaggedValues.Count>0)
                    Console.WriteLine("{0} {1} {2}",s.SiteCode,s.FlaggedValues.Keys.Min().ToString("MM/dd/yy"),s.FlaggedValues.Keys.Max().ToString("MM/dd/yy"));
            }

            Console.WriteLine("Console Test");
        }
    }
}
