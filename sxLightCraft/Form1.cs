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

        }

        private async void button2_Click(object sender, EventArgs e)
        {
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

            var launchOptionCracked = new MLaunchOption
            {
                MaximumRamMb = ram,
                Session = MSession.GetOfflineSession(textBox2.Text), // replace this with login session value. ex) Session = MSession.GetOfflineSession("hello")



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
            button1.Enabled = false;
            label5.Visible = true;
            textBox2.Visible = true;
        }
    }
}
