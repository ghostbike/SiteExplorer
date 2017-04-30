using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteExplorer
{
    public partial class Launcher : Form
    {
        public List<SiteSeries> siteSeries = new List<SiteSeries>();

        public Launcher()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            siteSeries = SiteSeries.ReadObservations(@"C:\Users\krys\Documents\SiteData.txt");
            //MessageBox.Show("Process Began");

            this.Hide();
            Explorer explorerForm = new Explorer();
            //Application.Run(explorerForm);
            explorerForm.Show();
            //this.Close();



        }
    }
}
