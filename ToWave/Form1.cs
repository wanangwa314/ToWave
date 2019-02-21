using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ToWave
{
    public partial class Form1 : Form
    {
        AudioConverter audioConverter;
        List<string> FileNames;
        public Form1()
        {
            InitializeComponent();
            InitUi();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            desDirTextBox.Text = RemoveFileNameFromPath(Path.GetFileName(openFileDialog1.FileNames[0]), openFileDialog1.FileNames[0]);

            StringToList();//Convert Filenames returned to the dialog to the List<string> FileNames
            FillListBox();
        }

        private void InitUi()
        {
            desDirTextBox.Text = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            removeFileBtn.Enabled = false;
            listBox1.SelectedIndexChanged += delegate
            {
                removeFileBtn.Enabled = true;
            };
        }

        private string[] GetOutputFilenames()
        {
            string tempStr;
            string[] filenames = new string[openFileDialog1.FileNames.Length];
            int count = 0;

            foreach (string name in FileNames)
            {
                tempStr = Path.ChangeExtension(name, ".wav");
                filenames[count] = Path.GetFileName(tempStr);
                count++;
            }

            return filenames;
        }

        //Fills the listbox filenames that are to be converted
        private void FillListBox()
        {
            //Clear the listbox incase there are other items in it
            listBox1.Items.Clear();

            //Enumerate through the selected files array 
            //just get the name without the whole path using Path.GetFileName
            foreach(string name in openFileDialog1.FileNames)
            {
                listBox1.Items.Add(Path.GetFileName(name));
            }
        }

        private void removeFileBtn_Click(object sender, EventArgs e)
        {
            FileNames.RemoveAt(listBox1.Items.IndexOf(listBox1.SelectedItem));
            listBox1.Items.Remove(listBox1.SelectedItem);
            removeFileBtn.Enabled = false;
        }

        private void StringToList()
        {
            FileNames = openFileDialog1.FileNames.ToList<string>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = true
            };
            fileDialog.ShowDialog();

            foreach (string name in fileDialog.FileNames)
            {
                listBox1.Items.Add(Path.GetFileName(name));
                FileNames.Add(name);
            }
        }

        private string RemoveFileNameFromPath(string word, string path)
        {
            int index = path.IndexOf(word);
            string pathname = path.Remove(index);

            return pathname;
        }

        private void selectFolderBtn_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            desDirTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void convertBtn_Click(object sender, EventArgs e)
        {
            audioConverter = new AudioConverter(FileNames, GetOutputFilenames(), desDirTextBox.Text);

            try
            {
                audioConverter.Convert();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }           
        }

        
    }
}
