using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;

namespace FitnessTracker
{
    public partial class BodyWeightProgress : Form
    {
        public BodyWeightProgress()
        {
            InitializeComponent();
            LoadData();
            PopulateChart();
        }

        private void weightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            BodyWeightProgress bodyWeightProgress = new BodyWeightProgress();
            bodyWeightProgress.Show();
        }

        private void workoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Progress progress = new Progress();
            progress.Show();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }

        private void libraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Library library = new Library();
            library.Show();
        }
        private void nutritionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Nutrition nutrition = new Nutrition();
            nutrition.Show();
        }

        private void trackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracker tracker = new Tracker();
            tracker.Show();
        }

        private void exitApplicaionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToString();
            decimal weight = numericUpDown1.Value;

            ListViewItem list = new ListViewItem(date);
            list.SubItems.Add(weight.ToString());
            listView1.Items.Add(list);
            SaveData();
            numericUpDown1.Value = 0;
        }
        private void SaveData()
        {
            var items = listView1.Items.Cast<ListViewItem>()
                                  .Select(item => new
                                  {
                                      Date = item.SubItems.Count > 0 ? item.SubItems[0].Text : "",
                                      Text = item.SubItems.Count > 1 ? item.SubItems[1].Text : "",
                                  })
                                  .ToList();

            string json = System.Text.Json.JsonSerializer.Serialize(items);

            File.WriteAllText("weight.json", json);
        }
        private void LoadData()
        {
            try
            {
                if (File.Exists("weight.json"))
                {
                    string json = File.ReadAllText("weight.json");

                    var items = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(json);

                    foreach (var item in items)
                    {
                        var listViewItem = new ListViewItem(new[] {
                    item.Date,
                    item.Text,
                });
                        listView1.Items.Add(listViewItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class Item
        {
            public string Date { get; set; }
            public string Text { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string filePath = "Weight.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void PopulateChart()
        {
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Weight", 
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column 
            };

            foreach (ListViewItem item in listView1.Items)
            {
                string xValue = item.SubItems[0].Text;
                double yValue;
                if (double.TryParse(item.SubItems[1].Text, out yValue))
                {
                    series.Points.AddXY(xValue, yValue);
                }
            }

            chart1.Series.Add(series);
        }

    }
}
