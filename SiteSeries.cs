using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SiteExplorer
{
    public class SiteSeries
    {
        public int SiteCode { get; set; }
        public  DateTime TimeSeries { get; set; }
        public double ValueSeries { get; set; }
        public bool Flagged { get; set; }

        public SiteSeries(int siteCode, DateTime timeSeries, double valueSeries)
        {
            this.SiteCode = siteCode;
            this.TimeSeries = timeSeries;
            this.ValueSeries = valueSeries;
        }


        public static SiteSeries SiteSeriesFactory(string data)
        {
            var commaSeperated = data.Split((char)9);
            var siteCode = Convert.ToInt32(commaSeperated[0]);
            var timeSeries = Convert.ToDateTime(commaSeperated[1]);
            var valueSeries = Convert.ToDouble(commaSeperated[2].Replace(",","").Replace("\"",""));

            return new SiteSeries(siteCode, timeSeries, valueSeries);
        }


        public static List<SiteSeries> ReadObservations(string dataPath)
        {

            var data =
                File.ReadAllLines(dataPath)
                .Skip(1)
                .Select(SiteSeriesFactory)
                .ToList();

            return data;

        }
    }
}
