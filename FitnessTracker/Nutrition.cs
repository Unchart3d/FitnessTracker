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
using Newtonsoft.Json;
using System.Net.Http;

namespace FitnessTracker
{
    public partial class Nutrition : Form
    {
        public class Exercise
        {
            public string Name { get; set; }
            [JsonProperty("bodyPart")]
            public string BodyPart { get; set; }
            public string Equipment { get; set; }
            public string Target { get; set; }
        }
        public Nutrition()
        {
            InitializeComponent();

            listView1.Columns.Add("Breakfast", 150);
            listView1.Columns.Add("Calories", 10000);

            listView2.Columns.Add("Lunch", 150);
            listView2.Columns.Add("Calories", 10000);

            listView3.Columns.Add("Dinner", 150);
            listView3.Columns.Add("Calories", 10000);

            LoadData(listView4);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
        }

        private async void libraryToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string breakfast = textBox1.Text;
            decimal calories = numericUpDown1.Value;

            ListViewItem list = new ListViewItem(breakfast);
            list.SubItems.Add(calories.ToString());
            listView1.Items.Add(list);

            textBox1.Clear();
            numericUpDown1.Value = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string lunch = textBox2.Text;
            decimal calories = numericUpDown2.Value;

            ListViewItem list = new ListViewItem(lunch);
            list.SubItems.Add(calories.ToString());
            listView2.Items.Add(list);

            textBox2.Clear();
            numericUpDown2.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dinner = textBox3.Text;
            decimal calories = numericUpDown3.Value;

            ListViewItem list = new ListViewItem(dinner);
            list.SubItems.Add(calories.ToString());
            listView3.Items.Add(list);

            textBox3.Clear();
            numericUpDown3.Value = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string currentDate = dateTimePicker1.Value.ToShortDateString();

            ListViewItem dateItem = new ListViewItem("Date: " + currentDate);
            listView4.Items.Add(dateItem);

            foreach (ListViewItem item in listView1.Items)
            {
                string breakfast = item.SubItems[0].Text; 
                string calories = item.SubItems[1].Text; 

                ListViewItem newItem = new ListViewItem("");
                newItem.SubItems.Add(breakfast); 
                newItem.SubItems.Add(calories); 
                newItem.SubItems.Add(""); 
                listView4.Items.Add(newItem); 
            }

            foreach (ListViewItem item in listView2.Items)
            {
                string lunch = item.SubItems[0].Text;
                string calories = item.SubItems[1].Text;

                ListViewItem newItem = new ListViewItem("");
                newItem.SubItems.Add(lunch);
                newItem.SubItems.Add(calories);
                newItem.SubItems.Add("");
                listView4.Items.Add(newItem);
            }

            foreach (ListViewItem item in listView3.Items)
            {
                string dinner = item.SubItems[0].Text;
                string calories = item.SubItems[1].Text;

                ListViewItem newItem = new ListViewItem("");
                newItem.SubItems.Add(dinner);
                newItem.SubItems.Add(calories);
                newItem.SubItems.Add("");
                listView4.Items.Add(newItem);
            }

            CalculateAndDisplayTotal();

            SaveData();

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listView4.Items.Clear();
            string filePath = "CalorieTracker.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void SaveData()
        {
            var items = listView4.Items.Cast<ListViewItem>()
                                  .Select(item => new
                                  {
                                      Date = item.SubItems.Count > 0 ? item.SubItems[0].Text : "",
                                      Text = item.SubItems.Count > 1 ? item.SubItems[1].Text : "",
                                      Number = item.SubItems.Count > 2 ? item.SubItems[2].Text : "",
                                      numberr = item.SubItems.Count > 3 ? item.SubItems[3].Text : ""
                                  })
                                  .ToList();

            string json = System.Text.Json.JsonSerializer.Serialize(items);

            File.WriteAllText("CalorieTracker.json", json);
        }
        private void LoadData(ListView listview)
        {
            try
            {
                if (File.Exists("CalorieTracker.json"))
                {
                    string json = File.ReadAllText("CalorieTracker.json");

                    var items = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(json);

                    foreach (var item in items)
                    {
                        var listViewItem = new ListViewItem(new[] {
                    item.Date,
                    item.Text,
                    item.Number,
                    item.numberr
                });
                        listView4.Items.Add(listViewItem);
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
            public string numberr { get; set; }
        }

        private void CalculateAndDisplayTotal()
        {
            try
            {
                decimal totalCalories = 0;

                foreach (ListViewItem item in listView1.Items)
                {
                    decimal calories;
                    if (decimal.TryParse(item.SubItems[1].Text, out calories))
                    {
                        totalCalories += calories;
                    }
                }

                foreach (ListViewItem item in listView2.Items)
                {
                    decimal calories;
                    if (decimal.TryParse(item.SubItems[1].Text, out calories))
                    {
                        totalCalories += calories;
                    }
                }

                foreach (ListViewItem item in listView3.Items)
                {
                    decimal calories;
                    if (decimal.TryParse(item.SubItems[1].Text, out calories))
                    {
                        totalCalories += calories;
                    }
                }

                ListViewItem totalItem = new ListViewItem("Total");
                totalItem.SubItems.Add("");
                totalItem.SubItems.Add("");
                totalItem.SubItems.Add(totalCalories.ToString()); 
                listView4.Items.Add(totalItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total calories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bodyWeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            BodyWeightProgress body = new BodyWeightProgress();
            body.Show();
        }

        private void workoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Progress progress = new Progress();
            progress.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
