using Newtonsoft.Json;
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
using Newtonsoft.Json.Serialization;
using static FitnessTracker.Home;
using System.Net.Http;


namespace FitnessTracker
{
    public partial class Tracker : Form
    {
        public Tracker()
        {
            InitializeComponent();
            listView1.Columns.Add("Date", 100);
            listView1.Columns.Add("Workout", 150);
            listView1.Columns.Add("Reps in set", 80);
            listView1.Columns.Add("Weight", 1000);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }
        private void libraryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Library libraryForm = new Library();
            libraryForm.Show();
        }

        private void nutritionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Nutrition nutrition = new Nutrition();
            nutrition.Show();
        }

        private void progressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void trackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracker tracker = new Tracker();
            tracker.Show();
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SaveData()
        {
            var items = listView1.Items.Cast<ListViewItem>()
                                  .Select(item => new
                                  {
                                      Date = item.SubItems.Count > 0 ? item.SubItems[0].Text : "",
                                      Text = item.SubItems.Count > 1 ? item.SubItems[1].Text : "",
                                      Number = item.SubItems.Count > 2 ? item.SubItems[2].Text : "",
                                      Weight = item.SubItems.Count > 3 ? item.SubItems[3].Text : ""
                                  })
                                  .ToList();

            string json = System.Text.Json.JsonSerializer.Serialize(items);

            File.WriteAllText("data.json", json);
        }
        private void LoadData()
        {
            try
            {
                if (File.Exists("data.json"))
                {
                    string json = File.ReadAllText("data.json");

                    var items = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(json);

                    foreach (var item in items)
                    {
                        var listViewItem = new ListViewItem(new[] {
                    item.Date,
                    item.Text,
                    item.Number,
                    item.Weight
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
            public string Number { get; set; }
            public string Weight { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToShortDateString();
            string text = textBox1.Text;
            decimal number1 = numericUpDown1.Value;
            decimal number2 = numericUpDown2.Value;

            bool dateAdded = false;
            bool workoutadded = false;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[0].Text == date)
                {
                    dateAdded = true;
                    foreach (ListViewItem inneritem in listView1.Items)
                    {
                        if (inneritem.SubItems[1].Text == text)
                        {
                            workoutadded = true;
                            break;
                        }
                    }
                    break;
                }
            }

            if (!dateAdded)
            {
                ListViewItem dateItem = new ListViewItem(new[] { date, "", "", "" });
                listView1.Items.Add(dateItem);
            }

            if (!workoutadded)
            {
                ListViewItem wokroutitem = new ListViewItem(new[] { "", text, "", "" });
                listView1.Items.Add (wokroutitem);
            }

            ListViewItem listViewItem = new ListViewItem(new[] { "", "", number1.ToString(), number2.ToString() });
            listView1.Items.Add(listViewItem);

            SaveData();

            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }

        private void Tracker_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string filePath = "data.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void bodyWeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            BodyWeightProgress body = new BodyWeightProgress();
            body.Show();
        }

        private void workoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Progress progress = new Progress();
            progress.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
