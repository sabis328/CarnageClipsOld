using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Carnage_Clips
{
    public partial class CarnageReportForm : Form
    {

        public bool IsBusy { get; set; }
        MainForm parent;
        Bungie_Profile SelectedUserAccount;
        Bungie_Profile.Destiny_Character SelectedAccountCharacter;
        Bungie_API_Client ReportClient;
        public CarnageReportForm(MainForm parentForm)
        {

            InitializeComponent();
            
            parent = parentForm;
            ReportClient = new Bungie_API_Client();
            ReportClient.API_Client_Event += ReportClient_API_Client_Event;
            ReportClient.Bungie_API_Key = "9efe9b8eba3042afb081121d447fd981";
        }
        public void SetCharacter(Bungie_Profile.Destiny_Character inputCHar)
        {
            if (!IsBusy)
            {
                if (SelectedAccountCharacter != inputCHar)
                {

                    IsBusy = true;
                    treePlayers.Nodes.Clear();
                    treeCarnageReports.Nodes.Clear();

                    SelectedUserAccount = inputCHar.AccountOwner;
                    SelectedAccountCharacter = inputCHar;

                    lblCarnageReports.Text = SelectedUserAccount.Bungie_Display_Name + " | " + inputCHar.Class;

                    Task.Run(() => ReportClient.LoadCarnageReportList(inputCHar, parent.CarnageCountReq));
                }
            }
        }
        private void SetStatusMessage(string status = "", int add = 0, int setmax = 0)
        {

            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { SetStatusMessage(status, add, setmax); });
                return;
            }
            lblStatus.Text = status;
            lblStatus.Update();

            if (setmax > 0)
            {
                progressBar1.Maximum = setmax;
            }

            if (progressBar1.Value + add <= progressBar1.Maximum)
            {
                progressBar1.Value += add;

                lblStatus.Text = status +" - " + progressBar1.Value + "/" + progressBar1.Maximum;
            }
            else
            {
                progressBar1.Value = progressBar1.Maximum;
            }

            if (progressBar1.Value == progressBar1.Maximum)
            {
                lblStatus.Text = "Idle";
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
            }
            progressBar1.Update();

            lblStatus.Update();
        }


        private void SetIdle()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { SetIdle(); });
                return;
            }

            progressBar1.Value = 0;
            progressBar1.Maximum = 0;
            lblStatus.Text = "Idle";
        }

        private void ReportClient_API_Client_Event(object sender, Bungie_API_Client.Client_Event_Type e)
        {
            switch(e)
            {
                case Bungie_API_Client.Client_Event_Type.SingleCarnageComplete:
                    SetStatusMessage("Loading matches ",ReportClient.ReportsLoaded, ReportClient.ReportsToLoad);
                    break;
                case Bungie_API_Client.Client_Event_Type.AllCarnageComplete:
                    SetIdle();
                    IsBusy = false;
                    Display_Matches_and_Players();
                    break;
                case Bungie_API_Client.Client_Event_Type.SingleCarnageFail:
                    break;
                case Bungie_API_Client.Client_Event_Type.AllCarnageFail:
                    break;
            }
        }


        private void Display_Matches_and_Players()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { Display_Matches_and_Players(); });
                return;
            }

            foreach (Carnage_Report PGCR in ReportClient.RecentMatches)
            {
                TreeNode MatchNode = new TreeNode(PGCR.ActivityTypeID + " | " + PGCR.ActivitySpaceID);
                MatchNode.Nodes.Add(PGCR.ActivityStart.ToString());
                MatchNode.Nodes.Add("Game hash :" + PGCR.ActivityHash);
                MatchNode.Nodes.Add("Location hash :" + PGCR.LocationHash);
                MatchNode.Tag = PGCR;

                foreach (Bungie_Profile player in PGCR.ActivityPlayers)
                {
                    TreeNode PlayerNode = new TreeNode(player.Bungie_Display_Name + "#" + player.Bungie_User_Code);
                    if (player.MatchWeapons != null)
                    {
                        foreach (Bungie_Profile.Destiny_Weapon wep in player.MatchWeapons)
                        {
                            TreeNode wepNode = new TreeNode(wep.WeaponIdentifier);
                            wepNode.Nodes.Add("Kills : " + wep.WeaponKills);
                            wepNode.Nodes.Add("Precision Ratio : " + wep.WeaponPrecisionRatio);

                            if (wep.Suspected)
                            {
                                wepNode.BackColor = Color.Red;
                                PlayerNode.BackColor = Color.Red;
                                MatchNode.BackColor = Color.Red;
                            }
                            PlayerNode.Nodes.Add(wepNode);
                        }
                    }
                    MatchNode.Nodes.Add(PlayerNode);

                }
                treeCarnageReports.Nodes.Add(MatchNode);
            }

            foreach (Bungie_Profile sortedPlayer in ReportClient.RecentPlayers)
            {
                TreeNode RecentPlayerNode = new TreeNode(sortedPlayer.Bungie_Display_Name + "#" + sortedPlayer.Bungie_User_Code);
                RecentPlayerNode.Tag = sortedPlayer;
                treePlayers.Nodes.Add(RecentPlayerNode);
            }
            lblStatus.Text = "Recent players loaded, checking for twitch accounts";

            Task.Run(() => QueueRecentPlayerList());
        }

        #region Check Recent Players for twitch


        public List<Bungie_API_Client> RecentPlayerClients;
        public int PlayersToCheck = 0;
        public int PlayersChecked = 0;
        private void QueueRecentPlayerList()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { QueueRecentPlayerList(); });
                return;
            }

            if (treePlayers.Nodes.Count > 0)
            {
                
                TwitchLinkedPlayers = new List<Bungie_Profile>();
                RecentPlayerClients = new List<Bungie_API_Client>();
                PlayersToCheck = treePlayers.Nodes.Count;
                PlayersChecked = 0;

                SetStatusMessage("Checking players for linked twitch accounts ",0 , PlayersToCheck);


                foreach (TreeNode PlayerNode in treePlayers.Nodes)
                {
                    Bungie_API_Client playerClinent = new Bungie_API_Client();
                    playerClinent.API_Client_Event += PlayerClinent_API_Client_Event;
                    playerClinent.Bungie_API_Key = "9efe9b8eba3042afb081121d447fd981";

                    RecentPlayerClients.Add(playerClinent);
                    
                    Task.Run(() => playerClinent.Load_Recent_BENT((Bungie_Profile)PlayerNode.Tag));

                    
                }
            }
        }

        private void ProcessRecentPlayerList()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { ProcessRecentPlayerList(); });
                return;
            }

            foreach (Bungie_Profile CheckAgainst in ReportClient.RecentPlayers)
            {

                string AltTwitch = null;
                if (CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("twtich.tv/") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("twitch/") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("ttv") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("ttv/") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("ttvbtw")
                               || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("twitch-") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("ttv.") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("t.tv") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("live") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("twitch_") || CheckAgainst.Bungie_Display_Name.ToLower().Trim().Contains("twitch"))
                {
                    System.Diagnostics.Debug.Print("Matched a pattern name : " + CheckAgainst.Bungie_Display_Name);

                    string tempName = CheckAgainst.Bungie_Display_Name.ToLower();
                    tempName = tempName.Replace("twitch.tv/", " ");
                    tempName = tempName.Replace("twitch/", " ");
                    tempName = tempName.Replace("ttv", " ");
                    tempName = tempName.Replace("ttvbtw", " ");
                    tempName = tempName.Replace("t.tv", " ");
                    tempName = tempName.Replace("live", " ");
                    tempName = tempName.Replace("twitch_", " ");
                    tempName = tempName.Replace("twitch", " ");
                    tempName = tempName.Replace("ttv/", " ");
                    tempName = tempName.Replace("ttv.", " ");
                    tempName = tempName.Replace("(btw)", " ");


                    tempName = tempName.Trim();
                    tempName = Regex.Replace(tempName, "[^a-zA-Z0-9 _]", string.Empty);
                    AltTwitch = tempName;



                    if (TwitchLinkedPlayers.Contains(CheckAgainst))
                    {
                        Bungie_Profile subCheck = TwitchLinkedPlayers[TwitchLinkedPlayers.IndexOf(CheckAgainst)];
                        if (subCheck.TwitchLinked)
                        {
                            if (AltTwitch != null)
                            {
                                if (subCheck.TwitchName.ToLower().Trim() != AltTwitch.ToLower().Trim())
                                {
                                    System.Diagnostics.Debug.Print("Alternate Twitch Naming Found for " + subCheck.Bungie_Display_Name + subCheck.Bungie_User_Code);
                                }
                            }
                        }
                    }
                    else
                    {
                        CheckAgainst.TwitchName = AltTwitch;
                        CheckAgainst.TwitchLinked = true;
                        TwitchLinkedPlayers.Add(CheckAgainst);
                    }
                }
            }


            //Can loop back through to add back into matches here

            if(TwitchLinkedPlayers.Count > 0)
            {
                //Search for twitch VODS after resettinge the player tree

                treePlayers.Nodes.Clear();

                foreach(Bungie_Profile linkedPlayer in TwitchLinkedPlayers)
                {
                    TreeNode twitchPlayer = new TreeNode(linkedPlayer.Bungie_Display_Name + " | twitch.tv/" + linkedPlayer.TwitchName);
                    twitchPlayer.Tag = linkedPlayer;
                    treePlayers.Nodes.Add(twitchPlayer);
                }

                SetStatusMessage("Checking for vods",0, TwitchLinkedPlayers.Count);
                Task.Run(() => CheckTwitchVODS());
            }
        }

        public List<Bungie_Profile> TwitchLinkedPlayers;
        private void PlayerClinent_API_Client_Event(object sender, Bungie_API_Client.Client_Event_Type e)
        {
            
            PlayersChecked += 1;

            switch (e)
            {
                case Bungie_API_Client.Client_Event_Type.RecentPlayerBnetLoaded:
                    
                    Bungie_Profile ReturnedUser = (Bungie_Profile)sender;
                    if(ReturnedUser.TwitchLinked)
                    {
                        TwitchLinkedPlayers.Add(ReturnedUser);

                        System.Diagnostics.Debug.Print("Twitch foudn for " + ReturnedUser.Bungie_Display_Name);
                    }
                    SetStatusMessage("Checking players for linked twitch accounts ",PlayersChecked, PlayersToCheck);
                    break;
                case Bungie_API_Client.Client_Event_Type.RecentPlayerBnetFailed:
         
                    SetStatusMessage("Checking players for linked twitch accounts ",PlayersChecked, PlayersToCheck);
                    break;
            }

            if(PlayersToCheck == PlayersChecked)
            {
                
                SetIdle();
                ProcessRecentPlayerList();
            }
           
        }
        #endregion

        #region Checking for VODS and Resetting StreamTree

        private void CheckTwitchVODS()
        {
            List<TreeNode> ResetStreamNodes = new List<TreeNode>();
            Twitch_Client vodClient = new Twitch_Client();
            vodClient.Twitch_ClientID = "abvhdv9zyqhefmnbjz3fljxx3hpc7u";
            vodClient.Twitch_Client_Secret = "h7bled1w6wracl3bytlhqwra3d7pr8";
            vodClient.Validate_Twitch_Client();


            if (vodClient.Is_Validated == Twitch_Client.Twitch_Validation_Status.Success)
            {
                try
                {
                    foreach (Bungie_Profile linkedGuardian in TwitchLinkedPlayers)
                    {
                        vodClient.Twitch_Find_Channels(linkedGuardian.TwitchName, true);

                        if (vodClient.Found_Channels.Count > 0)
                        {

                            TwitchCreator possibleStreaamer = vodClient.Found_Channels[0];
                            vodClient.Load_Channel_Videos(possibleStreaamer);

                            //linkedGuardian.liveNow = possibleStreaamer.Live_Now;

                            System.Diagnostics.Debug.Print("user " + possibleStreaamer.Username + " is live ");

                            if (possibleStreaamer.Channel_Saved_Videos != null && possibleStreaamer.Channel_Saved_Videos.Count > 0)
                            {

                                TreeNode MatchedStreamNode = new TreeNode(linkedGuardian.Bungie_Display_Name + " | twitch.tv/" + linkedGuardian.TwitchName);
                                MatchedStreamNode.Tag = linkedGuardian;

                                foreach (TwitchVideo vod in possibleStreaamer.Channel_Saved_Videos)
                                {
                                    int i = 0;

                                    while (i < linkedGuardian.LinkedMatchTimes.Count)
                                    {
                                        DateTime AccountforDuration = vod.videoCreated;
                                        AccountforDuration += vod.videoDuration;
                                        DateTime CheckAgainst = linkedGuardian.LinkedMatchTimes[i];

                                        if (vod.videoCreated.Date == CheckAgainst.Date || CheckAgainst.Ticks < AccountforDuration.Ticks)
                                        {
                                            if (CheckAgainst.Ticks > vod.videoCreated.Ticks && CheckAgainst.Ticks < AccountforDuration.Ticks)
                                            {
                                                
                                                TimeSpan offset = CheckAgainst.TimeOfDay - vod.videoCreated.TimeOfDay;

                                                System.Diagnostics.Debug.Print("offseting : " + offset.ToString());

                                                if (offset.Hours < 0)
                                                {
                                                    offset = offset.Add(new TimeSpan(24, 0, 0));
                                                    System.Diagnostics.Debug.Print("Corrected Negative offset : " + offset.ToString());
                                                }


                                                string twitchLink = "http://www.twitch.tv";

                                                if (vod.videoDuration.ToString().Contains("."))
                                                {
                                                    int Voddays = 0;

                                                    string[] daySplit = vod.videoDuration.ToString().Split('.');

                                                    Voddays = Convert.ToInt32(daySplit[0].ToString());

                                                    Voddays *= 24;

                                                    Voddays += offset.Hours;

                                                    twitchLink = vod.videoLink + "?t=" + Voddays.ToString() + "h" + offset.Minutes + "m" + offset.Seconds + "s";

                                                }
                                                else
                                                {
                                                    twitchLink = vod.videoLink + "?t=" + offset.Hours + "h" + offset.Minutes + "m" + offset.Seconds + "s";
                                                }




                                                System.Diagnostics.Debug.Print("ADDING MATCH NODE : for " + linkedGuardian.Bungie_Display_Name + " | linked matches found : " + linkedGuardian.LinkedMatches.Count.ToString() + " on match : " + i.ToString());
                                                TreeNode twitchNode = new TreeNode(twitchLink);

                                                twitchNode.Nodes.Add(linkedGuardian.LinkedMatchTimes[i].ToString());
                                                twitchNode.Nodes.Add(linkedGuardian.LinkedMatches[i].ActivityTypeID + " | " + linkedGuardian.LinkedMatches[i].ActivitySpaceID);
                                                twitchNode.Tag = twitchLink;
                                                twitchNode.Nodes[0].Tag = twitchLink;
                                                twitchNode.Nodes[1].Tag = twitchLink;
                                                MatchedStreamNode.Nodes.Add(twitchNode);

                                            }

                                        }
                                        i += 1;
                                    }
                                }

                                if (MatchedStreamNode.Nodes.Count > 0)
                                {
                                    ResetStreamNodes.Add(MatchedStreamNode);
                                }

                            }


                        }

                        //UpdateStatusMessage
                        SetStatusMessage("Processing linked twitch accounts for vods " + (progressBar1.Value + 1).ToString() + "/" + progressBar1.Maximum.ToString(), 1, 0);
                    }

                    ResetStreamTree(ResetStreamNodes);
                }
                catch
                { SetStatusMessage("Error occured loading vods, try refreshing matches"); }

                IsBusy = false;
            }
            else
            {
                SetStatusMessage("Failed to validate twitch client", 0, 0);
                IsBusy = false;
            }
        }

        private void ResetStreamTree(List<TreeNode> streamers)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { ResetStreamTree(streamers); });
                return;
            }
            List<DateTime> MatchedTimes = new List<DateTime>();
            treePlayers.Nodes.Clear();
            foreach (TreeNode streamerNode in streamers)
            {
                //treeviewStreamList.Nodes.Add(streamerNode);
                //Sort by date and time here and sub match date and time here
                //lRecentMatches.Sort((x, y) => y.ActivityStart.CompareTo(x.ActivityStart));
                //Create a list of dates for each streamer, sort the sub dates, then sort the main streamers by sub date[0]
                if (streamerNode.Nodes.Count > 1)
                {

                    List<TreeNode> SubMatches = new List<TreeNode>();
                    List<DateTime> SubMatchTimes = new List<DateTime>();
                    foreach (TreeNode linkNode in streamerNode.Nodes)
                    {
                        SubMatchTimes.Add(Convert.ToDateTime(linkNode.Nodes[0].Text));

                    }

                    SubMatchTimes.Sort((x, y) => y.CompareTo(x));
                    foreach (DateTime matchCompare in SubMatchTimes)
                    {
                        foreach (TreeNode readd in streamerNode.Nodes)
                        {
                            if (readd.Nodes[0].Text == matchCompare.ToString())
                            {
                                SubMatches.Add(readd);
                            }
                        }
                    }

                    streamerNode.Nodes.Clear();
                    foreach (TreeNode addTo in SubMatches)
                    {
                        streamerNode.Nodes.Add(addTo);
                    }

                    MatchedTimes.Add(Convert.ToDateTime(streamerNode.Nodes[0].Nodes[0].Text));
                }
                else
                {
                    MatchedTimes.Add(Convert.ToDateTime(streamerNode.Nodes[0].Nodes[0].Text));
                }

            }
            MatchedTimes.Sort((x, y) => y.CompareTo(x));
            //Reorder all streamerNodes

            List<TreeNode> reAddStreams = new List<TreeNode>();
            foreach (DateTime compareTo in MatchedTimes)
            {
                foreach (TreeNode streamerNode in streamers)
                {
                    //System.Diagnostics.Debug.Print("Comparing time " + streamerNode.Nodes[0].Nodes[0])
                    if (streamerNode.Nodes[0].Nodes[0].Text == compareTo.ToString() && !reAddStreams.Contains(streamerNode))
                    {
                        reAddStreams.Add(streamerNode);
                    }
                }
            }

            foreach (TreeNode stream in reAddStreams)
            { treePlayers.Nodes.Add(stream); }

            lblPlayerReports.Text = "Streams Found : " + treePlayers.Nodes.Count;

            #region LIVE NOW CHECKING
            //Additional Loop here to run through and show if somebody is pressently live, indicating that somebody might have streamed this match
            //List<Guardian> CurrentLive = new List<Guardian>();
            //foreach (Guardian g in TwitchLinkedGuardians)
            //{
            //  if (g.liveNow)
            //{
            //  CurrentLive.Add(g);

            //}
            //}

            //foreach (Guardian g in CurrentLive)
            //{
            //  System.Diagnostics.Debug.Print("Live user ; " + g.MainDisplayName);

            //}
            //foreach (TreeNode matchNode in treeviewCarnageList.Nodes)
            //{
            //  System.Diagnostics.Debug.Print("Checking match : " + matchNode.Text);
            //CarnageReport pgcr = (CarnageReport)matchNode.Tag;
            //bool foundLive = false;
            //foreach (TreeNode matchSubNodes in matchNode.Nodes)
            //{
            //  foreach (Guardian g in CurrentLive)
            //{
            //if (matchSubNodes.Text.ToLower() == g.MainDisplayName.ToLower())
            //  {
            //System.Diagnostics.Debug.Print(matchNode.Text + " Contained user : " + g.MainDisplayName);
            //  foundLive = true;
            //System.Diagnostics.Debug.Print("updating node colors");
            //matchSubNodes.BackColor = Color.LimeGreen;
            //matchSubNodes.ForeColor = Color.Black;

            //}

            //}


            //}
            //if (foundLive)
            //{
            //  matchNode.Text += " : a user from this match is currently live";
            //matchNode.BackColor = Color.Yellow;
            //}
            //}
            //treeviewCarnageList.Update();
            #endregion
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {

            if (!IsBusy)
            {
                IsBusy = true;
                PlayersChecked = 0;
                PlayersToCheck = 0;
                treeCarnageReports.Nodes.Clear();
                treePlayers.Nodes.Clear();

                if (SelectedAccountCharacter != null)
                {
                    Task.Run(() => ReportClient.LoadCarnageReportList(SelectedAccountCharacter, parent.CarnageCountReq));
                }
            }
        }
    }
}
