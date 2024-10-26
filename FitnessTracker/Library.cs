using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
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
using static FitnessTracker.Home;

namespace FitnessTracker
{
    public partial class Library : Form
    {
        public Library()
        {
            InitializeComponent();
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private List<Exercise> exercises;
        public Library(List<Exercise> exercises)
        {
            InitializeComponent();
            this.exercises = exercises;

            foreach (var exercise in exercises)
            {
                string seperator = "~~~~~~~~~~~~~~~~~~~~~~~~~~~";
                string listBoxItem =$"{exercise.Name}~~~~~~~" +
                    $"Body Part: {exercise.BodyPart}~~~~~~~" +
                    $"Equipment: {exercise.Equipment}~~~~~~~" +
                    $"Target: {exercise.Target}";

                listBox1.Items.Add(listBoxItem);
                originalItems.Add(listBoxItem);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private List<string> originalItems = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim().ToLower();

            var matchingItems = originalItems.Where(item => item.ToLower().Contains(searchTerm)).ToList();

            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            foreach (var item in matchingItems)
            {
                listBox1.Items.Add(item);
            }

            listBox1.EndUpdate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            listBox1.Items.Clear();
            foreach (var item in originalItems)
            {
                listBox1.Items.Add(item);
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
    }
}
