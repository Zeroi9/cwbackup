// This class is used to read data
using System;
using System.IO;
using System.Windows.Forms;

class Read
{
    // defines the directory variable
    public string directory;

    // constructor
    public Read()
    {

        // if the directory.txt file doesn't exist, let the user navigate to the folder and 
        // it'll create directory.txt with the path.
        if (!File.Exists("directory.txt"))
        {
            createDirectory();
        }
        // if the directory.txt exists, read the path.
        else
        {
            readDirectory();
        }
    }

    private void createDirectory()
    {
        // variable that will be set to 1 when the folder contains Cube.exe
        int directoryFound = 0;
        // new FolderBrowserDialog
        FolderBrowserDialog selectFolder = new FolderBrowserDialog();
        do
        {
            MessageBox.Show("Please navigate and select your CubeWorld folder, containing Cube.exe.");
            DialogResult result = selectFolder.ShowDialog();
            // directory from the selected path
           
            if (result == DialogResult.OK)
            {
                MessageBox.Show(selectFolder.SelectedPath);
                if (File.Exists(selectFolder.SelectedPath + "/Cube.exe"))
                {
                    directoryFound = 1;
                    directory = selectFolder.SelectedPath;
                }
            }
            else
            {
                MessageBox.Show("You didn't select a folder, closing program!");
                Environment.Exit(0);
                
            }
        }

        while (directoryFound == 0 || string.IsNullOrEmpty(selectFolder.SelectedPath));

        // new StreamWriter object 
        StreamWriter SW = new StreamWriter("directory.txt");
        // write the directory
        SW.WriteLine(directory);
        // stop writing
        SW.Close();
    }

    private void readDirectory()
    {
        // StreamReader object
        StreamReader SR = new StreamReader("directory.txt");
        // read until end
        string temp = "";
        while (!SR.EndOfStream)
        {
            temp = SR.ReadLine();
        }
        // stop reading
        SR.Close();
        directory = temp;

        if (directory == "")
        {
            createDirectory();
        }
    }

}