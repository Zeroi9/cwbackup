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

namespace Cubeworld_Character_Backup
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            // to make the form be on top
            this.Hide();
            this.Show();
            // Load all listboxes
            loadListBox(1);
            loadListBox(2);
            loadListBox(3);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        // Load all backups into the listboxes.
        private void loadListBox(int type)
        {
            // Character backup
            if (type == 1)
            {
                // since listbox1 is always loaded on form load, check if directories exists - if not, create them
                if (!Directory.Exists("backups") || !Directory.Exists("backups/worlds") || !Directory.Exists("backups/characters"))
                {
                    Directory.CreateDirectory("backups");
                    Directory.CreateDirectory("backups\\worlds");
                    Directory.CreateDirectory("backups\\characters");
                }
                // since this is called on every time we do something related to characters backups, we don't 
                // want the same backups to be listed more than once, so we clear the listbox.
                listBox1.Items.Clear();
                // get all files from backups/characters/.
                string[] saves = Directory.GetFiles("backups\\characters");
                // for each file, remove backups/characters/ and add it to the listbox.
                foreach (string file in saves)
                {
                    listBox1.Items.Add(file.Replace("backups\\characters\\", ""));
                }
            }
                // World backup
            else if (type == 2)
            {
                Read directoryReader = new Read();
                string CubeDirectory = directoryReader.directory;
                // Get all files in the save directory
                string[] saves = Directory.GetFiles(CubeDirectory + "/Save");

                // char array
                char[] fileChar;
                // foreach file in the save directory, split the / so the last index in
                // filePath contains the filename.
                foreach (string file in saves)
                {
                    int temp = 0;
                    string[] filePath = file.Split('/');
                    foreach (string word in filePath)
                    {
                        temp++;
                    }
                    // replace the Save\ from the filename.
                    string fileName = filePath[temp - 1].Replace("Save\\", "");
                    fileChar = fileName.ToCharArray();
                    // if filename doesn't begins with m(map) or c(characters), add them to the listbox.
                    if (fileChar[0] != 'm' && fileChar[0] != 'c' && fileName != "worlds.db")
                        listBox2.Items.Add(fileName);
                }
            }
            else
            {
                // this is load listbox3(world backups)
                listBox3.Items.Clear();
                // get files from backup folder
                string[] saves = Directory.GetFiles("backups\\worlds");
                // foreach file, add them to the list and replace the path.
                foreach (string file in saves)
                {
                    listBox3.Items.Add(file.Replace("backups\\worlds\\", ""));
                }
            }
            }
        


        // This is to backup current characters.db
        private void button1_Click(object sender, EventArgs e)
        {

            Read directoryReader = new Read();
            string CubeDirectory = directoryReader.directory;

            // this is Backup current characters
            // Current time
            DateTime time = DateTime.Now;
            string today = "MMM ddd d HH:mm yyyy";
            today = time.ToString(today);
            today = today.Replace(" ", "_");
            today = today.Replace(":", ".");
            // Generate new characters backup
            string backupName = "characters/characters_" + today.Replace(" ", "_");
            int fileExists = 1;
            int temp = 0;
            /*
             This do { } will:
             * increase temp by 1
             * check if file exists, like characters_date_temp.db, until it doesn't exist, then do { } is done.
             */
            do
            {
                temp++;
                if (File.Exists("backups/" + backupName + ".db"))
                {
                    backupName += "_" + temp.ToString();
                }
                else
                    fileExists = 0;
            }
            while (fileExists == 1);
            temp = 0;
            // Copy characters.db from cubedirectory to backups folder
            File.Copy(CubeDirectory + "\\Save\\characters.db", "backups/" + backupName + ".db");
            // refresh list box
            loadListBox(1);
            // messagebox
            MessageBox.Show("Your characters has been backed up with the name: " + backupName);
                
       }

        // Delete selected backup
        private void button3_Click(object sender, EventArgs e)
        {
            string backup = listBox1.GetItemText(listBox1.SelectedItem);
            // if no backup selected
            if (backup == "")
            {
                // show messagebox
                MessageBox.Show("Select a backup file first!");
            }
            else
            {
                // delete it
                File.Delete("backups/characters/" + backup);
                // refresh list box
                loadListBox(1);
            }
        }

        // Load character backup
        private void button2_Click(object sender, EventArgs e)
        {
            // Get selected backup
            string backup = listBox1.GetItemText(listBox1.SelectedItem);
            // If no backup selected, show a messagebox
            if (backup == "")
            {
                MessageBox.Show("Select a backup file first!");
            }
            else
            {
                Read directoryReader = new Read();
                string CubeDirectory = directoryReader.directory;
                // If the file exists in cubedirectory
                if (File.Exists(CubeDirectory + "\\Save\\characters.db"))
                {
                    // delete it
                    File.Delete(CubeDirectory + "\\Save\\characters.db");
                }
                // Copy from backups to cubedirectory saves.
                File.Copy("backups/characters/" + backup, CubeDirectory + "\\Save\\characters.db");
                MessageBox.Show("The selected backup has been restored!");
            }
        }

        // The link in bottom right corner
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cubeworldforum.org/");
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // This is to backup a world
        private void button4_Click(object sender, EventArgs e)
        {
            Read directoryReader = new Read();
            string CubeDirectory = directoryReader.directory;
            if (listBox2.GetItemText(listBox2.SelectedItem) == "")
            {
                MessageBox.Show("You need to select a world!");
            }
            // this is Backup world
            // Generate world backup
            string backupName = "worlds/" + listBox2.GetItemText(listBox2.SelectedItem);
            int fileExists = 1;
            int temp = 0;
            /*
             This do { } will:
             * increase (int) temp by 1
             * first run: if file exists (backups/worlds/world.db), change it to (temp)world.db.
             * second run: if file exists(backups/worlds/(temp)world.db), increase temp by 1.
             * When the file doesn't exist, set fileExists to 0 and the do { } is done.
             */
            do
            {
                temp++;
                if (File.Exists("backups/" + backupName))
                {
                    backupName = "worlds/" + listBox2.GetItemText(listBox2.SelectedItem);
                    backupName = backupName.Replace("world_", "(" + temp + ")" + "world_");
                }
                else
                    fileExists = 0;
            }
            while (fileExists == 1);
            temp = 0;
            // Backup the world
            File.Copy(CubeDirectory + "\\Save\\" + listBox2.GetItemText(listBox2.SelectedItem), "backups/" + backupName);
            // refresh list box
            loadListBox(3);
            // Show a messagebox
            MessageBox.Show("Your world has been backed up with the name: " + backupName);
        }

        // Load world backup
        private void button5_Click(object sender, EventArgs e)
        {
            // Get the selected world.
            string backup = listBox3.GetItemText(listBox3.SelectedItem);
            string backup_full = listBox3.GetItemText(listBox3.SelectedItem);
            // If no world selected, show a messagebox
            if (backup == "")
            {
                MessageBox.Show("Select a backup file first!");
            }
                // Else
            else
            {
                // this is a bad way but I don't know how to do it otherwise lol
                // Take the world name into a character array
                char[] loadWorldName = backup.ToCharArray();
                
                int startIndex = 0;
                int endIndex = 0;
                // For each character in the worldname:
                for (int i = 0; i < loadWorldName.Length; i++)
                {
                    // if character equals to a (, set the startIndex to that character index.
                    if (loadWorldName[i] == '(')
                        startIndex = i;
                    // if character equals to a ), set the endIndex to that character index.
                    if (loadWorldName[i] == ')')
                        endIndex = i;
                }

                // if the end index isn't 0 (that means that there's 2 or more backups of the same world
                if (endIndex != 0)
                {
                    // Replace the ( ) and everything inbetween with *.
                    while (startIndex <= endIndex)
                    {
                        loadWorldName[startIndex] = '*';
                        startIndex++;
                    }
                }

                // give backup string the worldname variable.
                backup = new string(loadWorldName);
                // Remove *.
                backup = backup.Replace("*", "");
                // Input could be (1)world_123.db, output is world_123.db. 

                // New Read object to get the CubeDirectory
                Read directoryReader = new Read();
                string CubeDirectory = directoryReader.directory;
                // If the file exists in the cubedirectory
                if (File.Exists(CubeDirectory + "\\Save\\" + backup))
                {
                    // delete it
                    File.Delete(CubeDirectory + "\\Save\\" + backup);
                }
                // copy the backup to the cubedirectory
                File.Copy("backups/worlds/" + backup_full, CubeDirectory + "\\Save\\" + backup);
                // messagebox
                MessageBox.Show("Your backup has been restored!");
            }
        }

        // This is to delete a backup.
        private void button6_Click(object sender, EventArgs e)
        {
            // Get selected world.
            string delete = listBox3.GetItemText(listBox3.SelectedItem);
            // If no world is selected, show a messagebox.
            if (delete == "")
            {
                MessageBox.Show("You need to select a world first!");
            }
                // Else
            else
            {
                // if the file exists, delete it.
                if (File.Exists("backups/worlds/" + delete))
                {
                    File.Delete("backups/worlds/" + delete);
                }
                    // Else
                else
                {
                    // Show a messagebox.
                    MessageBox.Show("World not found!");
                }
                // Update the listbox.
                loadListBox(3);
            }
        }
        }
                
        }
