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
using Newtonsoft.Json;

namespace Carnage_Clips
{
    public partial class MainForm : Form
    {
        public dynamic ActivityManifest { get; set; }
        public string ManifestActivityDefinition(string Key)
        {
            //dynamic manifestJson = JsonConvert.DeserializeObject<dynamic>(ManifestData);
            string returnValue = ActivityManifest[Key]["originalDisplayProperties"]["name"].ToString();
            return returnValue;
        }

        public TTV_Client Main_Twitch_Client { get; set; }
        
        public BNet_Profile SelectedBNet { get; set; }

        #region Selecting Characters to load reports for
        //Swap to the characters for the selected platform
        public void UpdateSelectedPlatform(string platformCode)
        {
            string matchCode = "1";
            switch(platformCode)
            {
                case "Xbox":
                    matchCode = "1";
                    break;
                case "Steam":
                    matchCode = "3";
                    break;
                case "Playstation":
                    matchCode = "2";
                    break;
                default:
                    matchCode = SelectedBNet.SelectedMemebership.platformCode;
                    break;
            }

            if (matchCode != SelectedBNet.SelectedMemebership.platformCode)
            {
                SetPointerLocation(new Point(0, btnUserSearch.Location.Y - 137));
                foreach (Destiny_Membership Platformaccount in SelectedBNet.DestinyMemberships)
                {
                    if(Platformaccount.platformCode == matchCode)
                    {
                        SelectedBNet.SelectedMemebership = Platformaccount;
                        break;
                    }
                }
            }
            DisplayPlatformCharacters();
        }
        //Show the correct buttons and emblems for the selected platform
        public void DisplayPlatformCharacters()
        {
            switch(SelectedBNet.SelectedMemebership.PlatformCharacters.Count)
            {
                case 1:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = false;
                    btnCharacter3.Visible = false;
                    btnCharacter1.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[0].Emblem;
                    btnCharacter1.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[0];
                    btnCharacter1.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[0].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[0].light;
                    break;
                case 2:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = true;
                    btnCharacter3.Visible = false;
                    btnCharacter1.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[0].Emblem;
                    btnCharacter1.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[0];
                    btnCharacter1.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[0].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[0].light;
                    btnCharacter2.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[1].Emblem;
                    btnCharacter2.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[1];
                    btnCharacter2.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[1].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[1].light;
                    break;
                case 3:
                    btnCharacter1.Visible = true;
                    btnCharacter2.Visible = true;
                    btnCharacter3.Visible = true;
                    btnCharacter1.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[0].Emblem;
                    btnCharacter1.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[0];
                    btnCharacter1.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[0].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[0].light;
                    btnCharacter2.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[1].Emblem;
                    btnCharacter2.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[1];
                    btnCharacter2.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[1].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[1].light;
                    btnCharacter3.BackgroundImage = SelectedBNet.SelectedMemebership.PlatformCharacters[2].Emblem;
                    btnCharacter3.Tag = SelectedBNet.SelectedMemebership.PlatformCharacters[2];
                    btnCharacter3.Text = SelectedBNet.SelectedMemebership.PlatformCharacters[2].classHash + " - " + SelectedBNet.SelectedMemebership.PlatformCharacters[2].light;
                    break;
            }
        }
        //Set the selected user from the search form, from there everything is set by selected membership
        public void SetSelectedUser(BNet_Profile selectedAccount)
        {
            comboBox1.Items.Clear();
            SelectedBNet = selectedAccount;
            if (selectedAccount.CrossSaveCode == "0")
            {
                foreach (Destiny_Membership Platformaccount in selectedAccount.DestinyMemberships)
                {
                    comboBox1.Items.Add(Platformaccount.PlatformType());
                }
            }
            else
            {
                comboBox1.Items.Add("Crossave Override : " + selectedAccount.SelectedMemebership.PlatformType());
            }
            comboBox1.SelectedIndex = 0;
            panelCharacterContainer.Visible = true;
            DisplayPlatformCharacters();
        }
        #endregion

        SearchForm GuardianSearchForm;
        CarnageReportForm CarnageForm;


        public int CarnageCountReq { get; set; }

        public string MatchFilterCode { get; set; }
        public string CurrentFilter()
        {
            //no,pvp,pve,trials,iron banner
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    return "0";
                case 1:
                    return "5";
                case 2:
                    return "7";
                case 3:
                    return "84";
                case 4:
                    return "19";
                default:
                    return "0";
            }
        }
        #region Show hide sub forms
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
            GuardianSearchForm.Visible = false;
            CarnageForm.Visible = true;
            CarnageForm.BringToFront();
        }

        private void ShowSearchForm()
        {
            GuardianSearchForm.Visible = true;
            CarnageForm.Visible = false;
            GuardianSearchForm.BringToFront();
        }
        #endregion

        #region Auto Updater
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
                System.Diagnostics.Debug.Print(Application.ProductVersion.ToString() + " | " + LatestVerion);
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

        #endregion






        public MainForm()
        {
            Task.Run(() => AutoUpdate());
            InitializeComponent();
            HideCharacterPanel();
            CarnageCountReq = 5;
            comboBox2.SelectedIndex = 0;
            CarnageForm = new CarnageReportForm(this);
            CarnageForm.TopLevel = false;
            panelFormContainer.Controls.Add(CarnageForm);
            CarnageForm.Visible = false;
            CarnageForm.Dock = DockStyle.Fill;
            


            GuardianSearchForm = new SearchForm(this);
            GuardianSearchForm.TopLevel = false;
            GuardianSearchForm.Dock = DockStyle.Fill;

            panelFormContainer.Controls.Add(GuardianSearchForm);
            GuardianSearchForm.Visible = true;
            GuardianSearchForm.BringToFront();

            Task.Run(() => ValidateTwitch());

            Task.Run(() => DownloadManifest());
        }

        public void DownloadManifest()
        {
            string requestURL = "https://www.bungie.net/common/destiny2_content/json/en/DestinyActivityDefinition-92361e7e-7970-47ac-9a0c-7e1503b370bc.json";
            
            HttpWebRequest reportClient = (HttpWebRequest)WebRequest.Create(requestURL);
            reportClient.Method = "GET";
            reportClient.Headers.Add("X-API-KEY", "9efe9b8eba3042afb081121d447fd981" );
            reportClient.KeepAlive = false;

            string responseBody = "";
            try
            {
                using (HttpWebResponse _response = (HttpWebResponse)reportClient.GetResponse())
                {
                    responseBody = new StreamReader(_response.GetResponseStream()).ReadToEnd();
                    _response.Close();
                }

                ActivityManifest = JsonConvert.DeserializeObject<dynamic>(responseBody);


            }
            catch
            {

            }
        }   

        public void ValidateTwitch()
        {
            Main_Twitch_Client = new TTV_Client();
            Main_Twitch_Client.Twitch_API_Key = "abvhdv9zyqhefmnbjz3fljxx3hpc7u";
            Main_Twitch_Client.Twitch_API_Secret = "h7bled1w6wracl3bytlhqwra3d7pr8";
            Main_Twitch_Client.Client_Event += Main_Twitch_Client_Twitch_Validation_Event;
            Main_Twitch_Client.Validate_Client();
        }

        private void Main_Twitch_Client_Twitch_Validation_Event(object sender, TTV_Client.Twitch_Client_Event e)
        {
            switch(e)
            {
                case TTV_Client.Twitch_Client_Event.Validate_Suceess:
                    Main_Twitch_Client.isValidated = true;
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        #region Character Buttons and show/hide forms
        private void btnCharacter1_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter1.Location.Y + 45));
            ShowCarnageForm();
            CarnageForm.UpdateSelectedCharacter((Destiny_Character)btnCharacter1.Tag);
        }

        private void btnCharacter2_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter2.Location.Y + 45));
            ShowCarnageForm();
            CarnageForm.UpdateSelectedCharacter((Destiny_Character)btnCharacter2.Tag);
        }

        private void btnCharacter3_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCharacter3.Location.Y + 45)) ;
            ShowCarnageForm();
            CarnageForm.UpdateSelectedCharacter((Destiny_Character)btnCharacter3.Tag);
        }

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnUserSearch.Location.Y - 137));
            ShowSearchForm();
        }

        private void btnCarnageSettings_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0, btnCarnageSettings.Location.Y - 137));
            if (SelectedBNet != null)
            {
                ShowCarnageForm();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetPointerLocation(new Point(0,btnDashboard.Location.Y - 137));
        }

        #endregion

        #region Move main form controls
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
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Something has changed the index through code or ui
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CarnageCountReq =(int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateSelectedPlatform(comboBox1.SelectedItem.ToString());
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            //User selecting mode
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatchFilterCode = CurrentFilter();

        }
    }
}
