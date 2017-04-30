using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteExplorer
{
    public class Site
    {
        public int SiteCode { get; set; }
        public SortedDictionary<DateTime, double> SiteValues { get; set; }
        public SortedDictionary<DateTime, double> FlaggedValues = new SortedDictionary<DateTime, double>();

        public Site(int siteCode, SortedDictionary<DateTime, double> siteValues) {
            this.SiteCode = siteCode;
            this.SiteValues = siteValues;
        }
    }
}
