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
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.Json;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json.Serialization;
using System.Net.Http;

namespace FitnessTracker
{
    public partial class Progress : Form
    {
        private const string FileName = "fitnessData.json";
        private Dictionary<string, (NumericUpDown Goal, NumericUpDown Progress)> controlMappings;
        public Progress()
        {
            InitializeComponent();

            InitializeControlMappings();
            LoadData();
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

        private void bodyWeightToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void Progress_Load(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown1, numericUpDown2, progressBar1, label17);
            UpdateProgressBar(numericUpDown3, numericUpDown10, progressBar2, label18);
            UpdateProgressBar(numericUpDown4, numericUpDown11, progressBar5, label19);
            UpdateProgressBar(numericUpDown5, numericUpDown12, progressBar3, label20);
            UpdateProgressBar(numericUpDown6, numericUpDown13, progressBar4, label21);
            UpdateProgressBar(numericUpDown7, numericUpDown14, progressBar6, label22);
            UpdateProgressBar(numericUpDown8, numericUpDown15, progressBar8, label23);
            UpdateProgressBar(numericUpDown9, numericUpDown16, progressBar7, label24);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void progressBar5_Click(object sender, EventArgs e)
        {

        }
        private void UpdateProgressBar(NumericUpDown goalControl, NumericUpDown progressControl, System.Windows.Forms.ProgressBar progressBar, Label label)
        {
            int goal = (int)goalControl.Value;
            int progress = (int)progressControl.Value;

            int progressPercentage = goal != 0 ? (int)(((double)progress / goal) * 100) : 0;

            progressPercentage = Math.Max(0, Math.Min(100, progressPercentage));

            progressBar.Value = progressPercentage;

            label.Text = $"{progress} / {goal} ({progressPercentage}%)";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown1, numericUpDown2, progressBar1, label17);
            SaveData();
        }
        private void SaveData()
        {
            var data = new Dictionary<string, NumericUpDownData>();

            foreach (var mapping in controlMappings)
            {
                var (goalControl, progressControl) = mapping.Value;

                data[mapping.Key] = new NumericUpDownData
                {
                    Goal = (int)goalControl.Value,
                    Progress = (int)progressControl.Value
                };
            }

            string jsonData = JsonConvert.SerializeObject(data);

            File.WriteAllText(FileName, jsonData);
        }
        private void LoadData()
        {
            if (File.Exists(FileName))
            {
                string jsonData = File.ReadAllText(FileName);

                var data = JsonConvert.DeserializeObject<Dictionary<string, NumericUpDownData>>(jsonData);

                if (data != null)
                {
                    foreach (var mapping in controlMappings)
                    {
                        if (data.TryGetValue(mapping.Key, out var numericUpDownData))
                        {
                            var (goalControl, progressControl) = mapping.Value;
                            goalControl.Value = numericUpDownData.Goal;
                            progressControl.Value = numericUpDownData.Progress;
                        }
                    }
                }
            }
        }
        public class NumericUpDownData
        {
            public int Goal { get; set; }
            public int Progress { get; set; }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown9.Value = 0;
            numericUpDown10.Value = 0;
            numericUpDown11.Value = 0;
            numericUpDown12.Value = 0;
            numericUpDown13.Value = 0;
            numericUpDown14.Value = 0;
            numericUpDown15.Value = 0;
            numericUpDown16.Value = 0;
            
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown3, numericUpDown10, progressBar2, label18);
            SaveData();
        }
        private void InitializeControlMappings()
        {
            controlMappings = new Dictionary<string, (NumericUpDown Goal, NumericUpDown Progress)>
        {
            { "goal1", (numericUpDown1, numericUpDown2) },
            { "goal2", (numericUpDown3, numericUpDown10) },
            { "goal3", (numericUpDown4, numericUpDown11) },
            { "goal4", (numericUpDown5, numericUpDown12) },
            { "goal5", (numericUpDown6, numericUpDown13) },
            { "goal6", (numericUpDown7, numericUpDown14) },
            { "goal7", (numericUpDown8, numericUpDown15) },
            { "goal8", (numericUpDown9, numericUpDown16) }
        };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown4, numericUpDown11, progressBar5, label19);
            SaveData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown5, numericUpDown12, progressBar3, label20);
            SaveData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown6, numericUpDown13, progressBar4, label21);
            SaveData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown7, numericUpDown14, progressBar6, label22);
            SaveData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown9, numericUpDown16, progressBar7, label24);
            SaveData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateProgressBar(numericUpDown8, numericUpDown15, progressBar8, label23);
            SaveData();
        }
    }
}
