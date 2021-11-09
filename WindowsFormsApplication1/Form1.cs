using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        IWebDriver Driver;

        public Form1()
        {
            InitializeComponent();
        }
        private bool startBrowser()
        {
            try
            {
                Console.WriteLine("Starting Browser");
                Driver = new ChromeDriver(Path.GetDirectoryName(Application.ExecutablePath));
                Driver.Url = textBox1.Text;
                ConfigStore.Url = textBox1.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            next();
        }


        private IWebElement[] Elements = null;
        private IWebElement Parent = null;
        private IWebElement GradeInput = null;
        private IWebElement CommentInput = null;
        public bool setBrowserElement()
        {
            try
            {
                string chunks = string.Join(" and ",
                    CurrentSubmission.Chunks.Select(s => "contains(text(), '" + s + "')"));
                string xpath = "//a[" + chunks + "]";

                Elements = null;
                Parent = null;
                GradeInput = null;
                CommentInput = null;

                Elements = Driver.FindElements(By.XPath(xpath)).ToArray();
                if (Elements.Length != 1)
                {
                    Console.WriteLine("Found " + Elements.Length + " elements, returning.");
                    return false;
                }
                var element = Elements[0];
                Parent = element.FindElement(By.XPath("./../.."));

                if (Parent == null) return false;

                GradeInput = Parent.FindElement(By.XPath(".//input[@type='text']"));
                if (GradeInput == null) return false;

                CommentInput = Parent.FindElement(By.XPath(".//textarea"));
                if (CommentInput == null) return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public static string nlUpdate(string value)
        {
	        return value
		        .Replace(Environment.NewLine, "\n")
		        .Replace("\r", "\n")
		        .Replace("\n", Environment.NewLine);
        }

        private Submission CurrentSubmission;
        public void next()
        {
            if (Submissions.Count == 0) return;
            try
            {
                CurrentSubmission = Submissions.Pop();
                this.submissionForm1.Submission = CurrentSubmission;
                panel1.Controls.Clear();
                var files = CurrentSubmission.findFiles(ConfigStore.Extensions).ToArray();
                int width = panel1.Width / files.Length;
                width = (width < 350) ? 350 : width;
                for (int i = 0; i < files.Length; i++)
                {
                    TextBox textBox = new TextBox()
                    {
                        Multiline = true,
                        Width = width,
                        Height = panel1.Height - 40,
                        Location = new Point(i * width + 20, 20),
                        Text = nlUpdate(File.ReadAllText(files[i])),
                        ScrollBars = ScrollBars.Both
                    };
                    panel1.Controls.Add(textBox);
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (!setBrowserElement()) next();
            Text = Submissions.Count + " left";
        }

        private string TargetFolder = "";
        string createTargetFolder()
        {
            string tmpFolder = Path.GetTempPath();
            string targetFolder;
            do
            {
                targetFolder = Path.Combine(tmpFolder, Guid.NewGuid().ToString());
            } while (Directory.Exists(targetFolder));
            return targetFolder;
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            if (!startBrowser()) return;
            if (!openZipFolder()) return;
            if (!createSubmissions()) return;
        }

        private bool createSubmissions()
        {
            try
            {
                foreach (var submissionFolder in Directory.GetDirectories(TargetFolder))
                {
                    try
                    {                        
                        Submission newSubmission = new Submission(submissionFolder);
                        Console.WriteLine(newSubmission);
                        Submissions.Push(newSubmission);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        private bool openZipFolder()
        {
            try
            {
	            MessageBox.Show("Unpack your Moodle-Download to C:\\Moodle");

                // The moodle-Zip-Archive is not 
                /*                 
                OpenFileDialog dlg = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = false,
                    DefaultExt = "*.zip"
                };
                if (dlg.ShowDialog() != DialogResult.OK) return true;
                TargetFolder = createTargetFolder();
                Console.WriteLine("Extract:");
                Console.WriteLine(dlg.FileName);
                Console.WriteLine(TargetFolder);
                ZipFile.ExtractToDirectory(dlg.FileName, TargetFolder, Encoding.UTF8);
                */
                TargetFolder = @"C:\Moodle";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private Stack<Submission> Submissions = new Stack<Submission>();

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigStore.Url;
            submissionForm1.ParentForm = this;
        }

        public void store()
        {
	        submissionForm1.store();

	        setBrowserElement();

	        if (GradeInput != null)
		        GradeInput.SendKeys(CurrentSubmission.value);
	        if (CommentInput != null)
		        CommentInput.SendKeys(CurrentSubmission.msg);
	        next();

        }

        private void StoreBtn_Click(object sender, EventArgs e)
        {
	        store();
        }
    }
}
