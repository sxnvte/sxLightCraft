using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace sxLightCraft
{
    // TODO: make cracked checkbox work
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Pro Tip: you can write in this box the folder name of one of your versions in the versions folder so you can use custom versions!", "sxLightCraft", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool btn2Clicked;

        private void GetVersiosnNames()
        {
            string minecraftDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "versions");
            string[] directories = Directory.GetDirectories(minecraftDir);

            // Wyczyść ComboBox przed dodaniem nowych elementów
            comboBox1.Items.Clear();

            // Dodaj każdy folder do ComboBoxa
            foreach (string directory in directories)
            {
                // Pobierz nazwę folderu
                string folderName = new DirectoryInfo(directory).Name;

                // Dodaj nazwę folderu do ComboBoxa
                comboBox1.Items.Add(folderName);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            bool btn2Clicked = true;
            timer1.Start();
            this.Text = "sxLightCraft | Launching Minecraft";
            MessageBox.Show("Minecraft will be started soon! if you dont have your selected version installed it can load longer!", "sxLightCraft", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // disable the button so if you are running minecraft you cant press this again
            button2.Enabled = false;

            int ram = Convert.ToInt32(textBox1.Text);

            MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
            MSession session = await loginForm.ShowLoginDialog();

            // increase connection limit to fast download
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            //var path = new MinecraftPath("game_directory_path");
            var path = new MinecraftPath(); // use default directory

            var launcher = new CMLauncher(path);

            // show launch progress to console
            launcher.FileChanged += (eventArgs) =>
            {
                Console.WriteLine("[{0}] {1} - {2}/{3}", eventArgs.FileKind.ToString(), eventArgs.FileName, eventArgs.ProgressedFileCount, eventArgs.TotalFileCount);
            };
            launcher.ProgressChanged += (s, EventArgs) =>
            {
                Console.WriteLine("{0}%", EventArgs.ProgressPercentage);
            };

            var versions = await launcher.GetAllVersionsAsync();
            foreach (var item in versions)
            {
                // show all version names
                // use this version name in CreateProcessAsync method.
                Console.WriteLine(item.Name);
            }

            var launchOption = new MLaunchOption
            {
                MaximumRamMb = ram,
                Session = session, // replace this with login session value. ex) Session = MSession.GetOfflineSession("hello")



                //ScreenWidth = 1600,
                //ScreenHeight = 900,
                //ServerIp = "mc.hypixel.net"
            };

            //var process = await launcher.CreateProcessAsync("input version name here", launchOption);

            if (comboBox1.Text == "")
            {
                MessageBox.Show("select version before hitting start!");
                Application.Exit();
            }

            var process = await launcher.CreateProcessAsync(comboBox1.Text, launchOption);
            process.Start();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
            MSession session = await loginForm.ShowLoginDialog();
            MessageBox.Show("Login Success!");
            button1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Hide();
            CrackedForm cf = new CrackedForm();
            cf.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // checker for it is minecraft running
            // timer2.Start();
            string minecraftDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "versions");
            string[] directories = Directory.GetDirectories(minecraftDir);

            
            comboBox1.Items.Clear();

            
            foreach (string directory in directories)
            {
                
                string folderName = new DirectoryInfo(directory).Name;

               
                comboBox1.Items.Add(folderName);
            }

            // auto update

            WebClient wb = new WebClient();

            try
            {
                if (!wb.DownloadString("https://raw.githubusercontent.com/sxnvte/sxLightCraft/master/update-version").Contains("1.0"))
                {
                    DialogResult dialogResult = MessageBox.Show("There is an update for sxLightCraft! do you want to install it?", "sxLightCraft", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Process.Start("");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //nic
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("sxLightCraft has detected an error: " + ex.Message + "; If this error keeps happening please report an issue on Github or contact the creator", "sxLightCraft", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
            loginForm.ShowLogoutDialog();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("LWJGL");
            if (pname.Length == 0)
            {
                await Task.Delay(2500);
                this.Text = "sxLightCraft | Minecraft Running";
                timer1.Stop();
            }
            else
            {
                this.Text = "sxLightCraft | Launching Minecraft";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("LWJGL");
            if (pname.Length == 0)
            {
                this.Text = "sxLightCraft";
            }
            else
            {
                this.Text = "sxLightCraft | Launching Minecraft";
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sxnvte");
        }
    }
}
