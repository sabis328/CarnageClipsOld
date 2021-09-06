using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
namespace Carnage_Clips
{
    public partial class MainForm : Form
    {
        UserSearchForm SearchForm;
        CarnageReportForm CarnageForm;
        public int CarnageCountReq { get; set; }
        public Bungie_Profile Selected_User { get; set; }
        public Bungie_Profile Original_Profile { get; set; }
        public void Update_Selected_User(Bungie_Profile user, bool fromALt = false)
        {
            Selected_User = user;
            panelCharacterContainer.Visible = true;
            switch(user.Linked_Characters.Count)
            {
                case 1:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = false;
                    btnCharacter3.Visible = false;
                    btnCharacter1.BackgroundImage = user.Linked_Characters[0].Emblem;
                    btnCharacter1.Tag = user.Linked_Characters[0];
                    btnCharacter1.Text = user.Linked_Characters[0].Class + " - " + user.Linked_Characters[0].Light_Level;
                    break;
                case 2:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = true;
                    btnCharacter3.Visible = false;
                    btnCharacter1.BackgroundImage = user.Linked_Characters[0].Emblem;
                    btnCharacter2.BackgroundImage = user.Linked_Characters[1].Emblem;
                    btnCharacter1.Tag = user.Linked_Characters[0];
                    btnCharacter2.Tag = user.Linked_Characters[1];
                    btnCharacter1.Text = user.Linked_Characters[0].Class + " - " + user.Linked_Characters[0].Light_Level;
                    btnCharacter2.Text = user.Linked_Characters[1].Class + " - " + user.Linked_Characters[1].Light_Level;
                    break;
                case 3:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = true;
                    btnCharacter3.Visible = true;
                    btnCharacter1.BackgroundImage = user.Linked_Characters[0].Emblem;
                    btnCharacter2.BackgroundImage = user.Linked_Characters[1].Emblem;
                    btnCharacter3.BackgroundImage = user.Linked_Characters[2].Emblem;
                    btnCharacter1.Tag = user.Linked_Characters[0];
                    btnCharacter2.Tag = user.Linked_Characters[1];
                    btnCharacter3.Tag = user.Linked_Characters[2];
                    btnCharacter1.Text = user.Linked_Characters[0].Class + " - " + user.Linked_Characters[0].Light_Level;
                    btnCharacter2.Text = user.Linked_Characters[1].Class + " - " + user.Linked_Characters[1].Light_Level;
                    btnCharacter3.Text = user.Linked_Characters[2].Class + " - " + user.Linked_Characters[2].Light_Level;

                    break;
            }

            if (!fromALt)
            {
                try
                {
                    comboBox1.Items.Clear();
                    NeedToLoad = false;
                    Original_Profile = user;
                    foreach (Bungie_Profile.AlternateAccounts alt in user.Alt_Accounts)
                    {
                        comboBox1.Items.Add(alt.Platform.ToString());
                    }

                    comboBox1.SelectedIndex = 0;
                    NeedToLoad = true;
                }
                catch
                {
                    System.Diagnostics.Debug.Print("Error has occured changing account platform");
                }
            }
            else
            {
                NeedToLoad = true;
                
                
            }
        }

        private bool NeedToLoad = false;
        private void UpdateSelectedPlatform()
        {
            if(NeedToLoad)
            {
                System.Diagnostics.Debug.Print("Reloading Characted for " + comboBox1.SelectedItem.ToString());
                
                foreach(Bungie_Profile.AlternateAccounts alt in Original_Profile.Alt_Accounts)
                {
                    if(comboBox1.SelectedItem.ToString() == alt.Platform.ToString())
                    {
                        
                        SearchForm.Load_ALternate_Character_Set(alt);
                    }
                }

                HideCharacterPanel();
                ShowSearchForm();
            }
        }
        public void HideCharacterPanel()
        {
            panelCharacterContainer.Visible = false;
            btnCharacter1.Visible = false;
            btnCharacter2.Visible = false;
            btnCharacter3.Visible = false;
        }

        private void SetPointerLocation(Point location)
        {
            panelSelectionIndication.Location = location;
        }

        private void ShowCarnageForm()
        {
            SearchForm.Visible = false;
            CarnageForm.Visible = true;
            CarnageForm.BringToFront();
        }

        private void ShowSearchForm()
        {
            SearchForm.Visible = true;
            CarnageForm.Visible = false;
            SearchForm.BringToFront();
        }


        public void AutoUpdate()
        {
            //https://raw.githubusercontent.com/sabis328/GuardianTheaterDesktop/main/Guardian%20Theater%20Desktop/Properties/AssemblyInfo.cs
            //https://github.com/sabis328/GuardianTheaterDesktop/blob/main/Guardian%20Theater%20Desktop/bin/Debug/Guardian%20Theater%20Desktop.exe?raw=true

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/sabis328/CarnageClips/main/Carnage%20Clips/Properties/AssemblyInfo.cs");
            string LatestVerion;
            using (HttpWebResponse response = (HttpWebResponse)client.GetResponse())
            {
                string text = new StreamReader(response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Debug.Print(text);
                int start = 0;
                int end = 0;

                //AssemblyFileVersion("

                string fileVersionSearch = "AssemblyFileVersion(\"";
                start = text.IndexOf(fileVersionSearch, end) + fileVersionSearch.Length;
                end = text.IndexOf("\"", start);
                LatestVerion = text.Substring(start, end - start);
            }

            if (Application.ProductVersion != LatestVerion)
            {
                UpdateGuardianClient();
            }
        }

        private void UpdateGuardianClient()
        {

            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { UpdateGuardianClient(); });
                return;
            }
            System.Diagnostics.Debug.Print("updating application");
            WebClient Downloader = new WebClient();

            byte[] filebuffer = Downloader.DownloadData("https://github.com/sabis328/CarnageClips/blob/main/Carnage%20Clips/bin/Debug/Carnage%20Clips.exe?raw=true");
            var appLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string CurrentPath = Path.GetFileName(appLoc);
            string CurrentPathTrimmed = Path.Combine(Path.GetDirectoryName(appLoc), Path.GetFileNameWithoutExtension(appLoc));

            File.WriteAllBytes(CurrentPathTrimmed + "Update.exe", filebuffer);

            using (var BatchUpdater = new StreamWriter(File.Create(CurrentPathTrimmed + "Update.bat")))
            {
                BatchUpdater.WriteLine("@ECHO OFF");
                BatchUpdater.WriteLine("TIMEOUT /t 1 /nobreak > NUL");
                BatchUpdater.WriteLine("TASKKILL /IM \"{0}\" > NUL", CurrentPath);
                BatchUpdater.WriteLine("MOVE \"{0}\" \"{1}\"", CurrentPathTrimmed + "Update.exe", appLoc);
                BatchUpdater.WriteLine("DEL \"%~f0\" & START \"\" /B \"{0}\"", appLoc);
            }

            ProcessStartInfo startBatch = new ProcessStartInfo(CurrentPathTrimmed + "Update.bat");
            startBatch.WorkingDirectory = Path.GetDirectoryName(appLoc);
            Process.Start(startBatch);

            Environment.Exit(0);
        }








        public MainForm()
        {
            Task.Run(() => AutoUpdate());
            InitializeComponent();
            HideCharacterPanel();
            CarnageCountReq = 5;
            SearchForm = new UserSearchForm(this);
            SearchForm.TopLevel = false;

            CarnageForm = new CarnageReportForm(this);
            CarnageForm.TopLevel = false;

            panelFormContainer.Controls.Add(SearchForm);
            panelFormContainer.Controls.Add(CarnageForm);
            SearchForm.Visible = true;
            CarnageForm.Visible = false;
            SearchForm.Dock = DockStyle.Fill;
            CarnageForm.Dock = DockStyle.Fill;
            SearchForm.BringToFront();
        }

        private void btnCharacter1_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter1.Location.Y + 45));
            ShowCarnageForm();
            CarnageForm.SetCharacter((Bungie_Profile.Destiny_Character)btnCharacter1.Tag);
        }

        private void btnCharacter2_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter2.Location.Y + 45));
            ShowCarnageForm();
            CarnageForm.SetCharacter((Bungie_Profile.Destiny_Character)btnCharacter2.Tag);
        }

        private void btnCharacter3_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter3.Location.Y + 45)) ;
            ShowCarnageForm();
            CarnageForm.SetCharacter((Bungie_Profile.Destiny_Character)btnCharacter3.Tag);
        }

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnUserSearch.Location.Y - 100));
            ShowSearchForm();
        }

        private void btnCarnageSettings_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCarnageSettings.Location.Y - 100));
            if (Selected_User != null)
            {
                ShowCarnageForm();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0,btnDashboard.Location.Y - 100));
        }

        private bool mouseDown = false;
        private Point lastLocation;
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedPlatform();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CarnageCountReq =(int) numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
