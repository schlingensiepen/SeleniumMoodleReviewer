using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SubmissionForm : UserControl
    {
        public SubmissionForm()
        {
            InitializeComponent();
        }

        private Submission _Submission;
        public Submission Submission
        {
            get { return _Submission; }
            set
            {
                _Submission = value;
                try
                {
                    textBox1.Text = _Submission.initStructure();
                    textBox2.Text = "";
                    textBox3.Text = "";

                    textBox2.Focus();

                    if (listBox1.Items.Count == 0)
                    {
                        listBox1.Items.AddRange(ConfigStore.CommentStrings);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    textBox3.Text = ex.Message;
                    textBox2.Text = "";
                }
            }
        }

        

        private void SubmissionForm_Load(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.AddRange(new []
                {
                    "Falsches Format",
                    "Mehrere Main"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                textBox3.Text = ex.Message;
                textBox2.Text = "";
            }
        }

        public void openFiles()
        {
            Submission.openFiles(ConfigStore.Extensions);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            textBox3.Text = listBox1.SelectedItem.ToString();
        }

        public void store()
        {
            Submission.value = (string.IsNullOrWhiteSpace(textBox2.Text)) ? "0" : textBox2.Text;
            Submission.msg = textBox3.Text;

            List<string> comments = new List<string>();
            foreach (var listBox1Item in listBox1.Items)
            {
                comments.Add(listBox1Item.ToString());
                if (listBox1Item.ToString().Equals(Submission.msg)) return;
            }
            listBox1.Items.Add(Submission.msg);
            comments.Add(Submission.msg);
            ConfigStore.CommentStrings = comments.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFiles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "100";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
    }
}
