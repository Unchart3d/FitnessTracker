using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace FitnessTracker
{
    public partial class Home : Form
    {
        public class Exercise
        {
            public string Name { get; set; }
            [JsonProperty("bodyPart")]
            public string BodyPart { get; set; }
            public string Equipment { get; set; }
            public string Target { get; set; }
        }
        public Home()
        {
            InitializeComponent();
            InitializeListView();
            LoadData();
        }
        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.Scrollable = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Results", 1000);

           
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void libraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var exercises = await FetchExerciseData();

            Library libraryForm = new Library(exercises);
            libraryForm.Show();
        }
        private async Task<List<Exercise>> FetchExerciseData()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://exercisedb.p.rapidapi.com/exercises?limit=10000"),
                Headers =
        {
            { "X-RapidAPI-Key", "4e8c8c9f19msh8b1853959fdf505p12a5e2jsn9f097e0886d3" },
            { "X-RapidAPI-Host", "exercisedb.p.rapidapi.com" },
        },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                var exercises = JsonConvert.DeserializeObject<List<Exercise>>(body, new JsonSerializerSettings
                {

                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });

                return exercises;
            }
        }

        private void trackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tracker tracker = new Tracker();
            tracker.Show();
        }

        private void progressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void nutritionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Nutrition nutrition = new Nutrition();
            nutrition.Show();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home home = new Home();
            home.Show();
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

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count;i++)
            {
                object item = checkedListBox1.Items[i];
                bool isChecked = checkedListBox1.GetItemChecked(i);
                
                Dictionary<object, string> itemValueMap = new Dictionary<object, string>()
                {
                    { "Gain Weight", "I recommend you bulk and remember to count your calories." },
                    { "Lose Weight", "I recommend you cut and remember to count your calories." },
                    { "Gain Strength", "I recommend you either as you workout, go heavier on the weight with less reps, go lighter on the weight with more reps." },
                    { "Endurance", "I recommend you work on your cardio as well as lighter weight heavy reps." },
                    { "Tone Yourself", "I recommend you cut and as you workout, go lighter on the weight with more reps." }
                };

                if (isChecked)
                {
                    string valuetoadd = itemValueMap[item];
                    listView1.Items.Add(valuetoadd);
                }
            }
            if (radioButton1.Checked) { listView1.Items.Add("I suggest you start of slow. " + 
                "This means you should start with lighter weight since you haven't done this before. " + 
                "Also don't be ashamed for how much you can lift."); }
            else if (radioButton2.Checked) { listView1.Items.Add("I suggest you still start off lighter on the weights, " + 
                "but you can add more weights to your workouts. Also you can start counting some calories but don't worry to much about it."); }
            else if (radioButton3.Checked) { listView1.Items.Add("I suggest you start counting your calories and lifting to hit your max, " + 
                "but make sure you can hit that max 4 times. Also work on your 1 rep max max pr."); }
            else if (radioButton4.Checked) { listView1.Items.Add("You know what your doing but this app will help you keep track of all your needs... I hope"); }

            SaveData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i<checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            if (radioButton1.Checked) { radioButton1.Checked = false; }
            else if (radioButton2.Checked) { radioButton2.Checked = false; }
            else if (radioButton3.Checked) { radioButton3.Checked = false; }
            else {radioButton4.Checked = false; }
            listView1.Clear();
            string filePath = "suggestions.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            InitializeListView();
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

            File.WriteAllText("suggestions.json", json);
        }
        private void LoadData()
        {
            try
            {
                if (File.Exists("suggestions.json"))
                {
                    string json = File.ReadAllText("suggestions.json");

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
    }
}
