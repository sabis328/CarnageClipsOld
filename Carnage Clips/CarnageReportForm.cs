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
        Bnet_Client ReportsClient;

        public CarnageReportForm(MainForm parentForm)
        {

            InitializeComponent();
            
            parent = parentForm;
            ReportsClient = new Bnet_Client();
            ReportsClient.API_key = "9efe9b8eba3042afb081121d447fd981";
            ReportsClient.Client_Event += Report_Client_Event;
        }

        #region Updated Player Searching and Sorting

        //Neew event Handler for reprot client 
        private void Report_Client_Event(object sender, Bnet_Client.BNet_Client_Event_Type e)
        {
            switch(e)
            {
                case Bnet_Client.BNet_Client_Event_Type.Carnage_List_Loaded:
                    DisplayLoadedMatches((List<CarnageReport>)sender);
                    SetStatusMessage("Loading post game reports", 0, ReportsClient.ReportsToLoad);
                    break;

                case Bnet_Client.BNet_Client_Event_Type.Single_Carnage_Loaded:
                    SetStatusMessage("Loading post game reports", 1, ReportsClient.ReportsToLoad);
                    AddPlayersToMatches((CarnageReport)sender);

                    if (ReportsClient.ReportsLoaded == ReportsClient.ReportsToLoad)
                    {
                        System.Diagnostics.Debug.Print("All reports loaded");
                        SortRecentPlayers();
                    }
                    break;
                case Bnet_Client.BNet_Client_Event_Type.Single_Carnage_Fail:
                    SetStatusMessage("Loading post game reports", 1, ReportsClient.ReportsToLoad);
                    System.Diagnostics.Debug.Print("Match failed to load");

                    if (ReportsClient.ReportsLoaded == ReportsClient.ReportsToLoad)
                    {
                        System.Diagnostics.Debug.Print("All reports loaded");
                        SortRecentPlayers();
                    }
                    break;
                case Bnet_Client.BNet_Client_Event_Type.Details_Loaded:
                    RecentChecked++;
                    SetStatusMessage("Checking recent players for twitch accounts ", 1, RecentToCheck);
                    BNet_Profile holder = (BNet_Profile)sender;
                    if(holder.Has_Twitch)
                    {
                        Recent_With_Twitch.Add(holder);
                    }
                    if(RecentChecked == RecentToCheck)
                    {
                        DisplayTwitchLinkedPlayers();

                        IsBusy = false;
                    }
                    break;
                case Bnet_Client.BNet_Client_Event_Type.Details_Failed:
                    RecentChecked++;
                    SetStatusMessage("Checking recent players for twitch accounts ", 1, RecentToCheck);
                    if (RecentChecked == RecentToCheck)
                    {
                        DisplayTwitchLinkedPlayers();
                        IsBusy = false;
                    }
                    break;

                    
            }

            System.Diagnostics.Debug.Print(RecentChecked + "/" + RecentToCheck);

        }

        List<string> RecentCompare;
        List<BNet_Profile> RecentProcessed;
        List<BNet_Profile> Recent_With_Twitch;

        //Loads all players with a linked twitch account into the tree for a display 
        //while the tool then searches through the vods
        private void DisplayTwitchLinkedPlayers()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { DisplayTwitchLinkedPlayers(); });
                return;
            }

            treePlayers.Nodes.Clear();

            Check_Alternate_Twitch_Naming();


            foreach(BNet_Profile twitchLinked in Recent_With_Twitch)
            {
                TreeNode PlayerNode = new TreeNode(twitchLinked.GlobalName());
                PlayerNode.Nodes.Add(twitchLinked.TwitchName);
                PlayerNode.Expand();

                treePlayers.Nodes.Add(PlayerNode);
            }

            QueueTwitchVods();
        }

        private void Check_Alternate_Twitch_Naming()
        {
            foreach (BNet_Profile CheckAgainst in RecentProcessed)
            {
                if (CheckAgainst.BungieName != null)
                {
                    string AltTwitch = null;
                    if (CheckAgainst.BungieName.ToLower().Trim().Contains("twtich.tv/") || CheckAgainst.BungieName.ToLower().Trim().Contains("twitch/") || CheckAgainst.BungieName.ToLower().Trim().Contains("ttv") || CheckAgainst.BungieName.ToLower().Trim().Contains("ttv/") || CheckAgainst.BungieName.ToLower().Trim().Contains("ttvbtw")
                                   || CheckAgainst.BungieName.ToLower().Trim().Contains("twitch-") || CheckAgainst.BungieName.ToLower().Trim().Contains("ttv.") || CheckAgainst.BungieName.ToLower().Trim().Contains("t.tv") || CheckAgainst.BungieName.ToLower().Trim().Contains("live") || CheckAgainst.BungieName.ToLower().Trim().Contains("twitch_") || CheckAgainst.BungieName.ToLower().Trim().Contains("twitch"))
                    {
                        System.Diagnostics.Debug.Print("Matched a pattern name : " + CheckAgainst.BungieName);

                        string tempName = CheckAgainst.BungieName.ToLower();
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



                        if (Recent_With_Twitch.Contains(CheckAgainst))
                        {
                            System.Diagnostics.Debug.Print("Comparing twitch linked " + CheckAgainst.GlobalName());

                            BNet_Profile subCheck = Recent_With_Twitch[Recent_With_Twitch.IndexOf(CheckAgainst)];
                            if (subCheck.Has_Twitch)
                            {
                                if (AltTwitch != null)
                                {
                                    if (subCheck.TwitchName.ToLower().Trim() != AltTwitch.ToLower().Trim())
                                    {
                                        System.Diagnostics.Debug.Print("Alternate Twitch Naming Found for " + subCheck.GlobalName());
                                        System.Diagnostics.Debug.Print("Alternate name : " + AltTwitch);


                                        BNet_Profile AlternateProfile = new BNet_Profile();
                                        AlternateProfile.LinkedMatches = subCheck.LinkedMatches;
                                        AlternateProfile.BungieName = "Alternate account : " + subCheck.BungieName;
                                        AlternateProfile.BungieCode = subCheck.BungieCode;
                                        AlternateProfile.TwitchName = AltTwitch;
                                        AlternateProfile.Has_Twitch = true;

                                        Recent_With_Twitch.Add(AlternateProfile);

                                        System.Diagnostics.Debug.Print("Added alternate twitch profile added");
                                    }
                                }
                            }
                        }
                        else
                        {
                            CheckAgainst.TwitchName = AltTwitch;
                            CheckAgainst.Has_Twitch = true;
                            Recent_With_Twitch.Add(CheckAgainst);
                        }
                    }
                }
            }
        }

        //Loops through all recent players in the list and removes duplicate instances
        //if a duplicate is found it adds the match time to their linked matches
        //fires the twitch linked search at the end 
        private void SortRecentPlayers()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { SortRecentPlayers(); });
                return;
            }
            treePlayers.Nodes.Clear();
            RecentCompare = new List<string>();
            RecentProcessed = new List<BNet_Profile>();

            foreach(TreeNode matchNode in treeCarnageReports.Nodes)
            {
                CarnageReport postGame = (CarnageReport)matchNode.Tag;

                foreach(BNet_Profile player in postGame.MatchPlayers)
                {
                    if(player.GlobalName() != parent.SelectedBNet.GlobalName())
                    {
                        if(!RecentCompare.Contains(player.GlobalName()))
                        {
                            RecentCompare.Add(player.GlobalName());
                            player.LinkedMatches.Add(postGame);
                            RecentProcessed.Add(player);
                        }
                        else
                        {
                            foreach(BNet_Profile prof in RecentProcessed)
                            {
                                if(prof.GlobalName() == player.GlobalName())
                                {
                                    prof.LinkedMatches.Add(postGame);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            System.Diagnostics.Debug.Print("All recent players loaded");
            System.Diagnostics.Debug.Print("Checking and readding players to all matches");

            foreach(BNet_Profile player in RecentProcessed)
            {
                TreeNode PlayerNode = new TreeNode(player.GlobalName());
                PlayerNode.Tag = player;
                foreach(CarnageReport linked in player.LinkedMatches)
                {
                    PlayerNode.Nodes.Add(linked.instanceId);
                }
                treePlayers.Nodes.Add(PlayerNode);
            }

            Check_Recent_Twitch_Link();

        }


        //Loads all the players for a match into the tree
        private void AddPlayersToMatches(CarnageReport inputMatch)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { AddPlayersToMatches(inputMatch); });
                return;
            }

            foreach (TreeNode MatchNode in treeCarnageReports.Nodes)
            {
                CarnageReport compare = (CarnageReport)MatchNode.Tag;
                if(compare.instanceId == inputMatch.instanceId)
                {
                    MatchNode.Nodes.Clear();
                    MatchNode.Nodes.Add("Place Definition : " + parent.ManifestActivityDefinition(inputMatch.referenceId));
                    MatchNode.Nodes.Add("Date code : " + inputMatch.matchDateString);
                    MatchNode.Nodes.Add("Location Hash : " + inputMatch.referenceId);
                    MatchNode.Nodes.Add("Activity Type Hash : " + inputMatch.directorActivityHash);
                    foreach(BNet_Profile player in inputMatch.MatchPlayers)
                    {
                        MatchNode.Nodes.Add(player.GlobalName());
                    }
                    MatchNode.Tag = inputMatch;
                    break;
                }
            }
        }


        //Loads the list of matches while the user waits for the players to populate
        private void DisplayLoadedMatches(List<CarnageReport> Matches)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { DisplayLoadedMatches(Matches); });
                return;
            }

            treeCarnageReports.Nodes.Clear();
            foreach(CarnageReport match in Matches)
            {
                TreeNode MatchNode = new TreeNode(parent.ManifestActivityDefinition(match.directorActivityHash));
                MatchNode.Tag = match;

                treeCarnageReports.Nodes.Add(MatchNode);
            }
        }

        private int RecentChecked = 0;
        private int RecentToCheck = 0;
        public void Check_Recent_Twitch_Link()
        {

            System.Diagnostics.Debug.Print("Checking players for linked twitch accounts");
            Recent_With_Twitch = new List<BNet_Profile>();
            RecentChecked = 0;
            RecentToCheck = RecentProcessed.Count;
            SetStatusMessage("Checking recent players for twitch accounts ", 0, RecentToCheck);

            int i = 1;
            foreach (BNet_Profile player in RecentProcessed)
            {
                Bnet_Client subClient = new Bnet_Client();
                subClient.API_key = ReportsClient.API_key;
                subClient.Client_Event += Report_Client_Event;

                
                Task.Run(() => subClient.Load_Detailed_Bnet(player));

                System.Diagnostics.Debug.Print("Player queued " + i.ToString());
                i++;
            }


        }

        #endregion

        //New set character method
        public Destiny_Character CurrentCharacter;
        public void UpdateSelectedCharacter(Destiny_Character SelectedChar)
        {
            if (!IsBusy)
            {

                if (parent.SelectedBNet != null && SelectedChar != CurrentCharacter )
                {
                    IsBusy = true;
                    treePlayers.Nodes.Clear();
                    treeCarnageReports.Nodes.Clear();
                    CurrentCharacter = SelectedChar;
                    lblCarnageReports.Text = parent.SelectedBNet.SelectedMemebership.platformDisplayName + " | " + SelectedChar.classHash;
                    ReportsClient.LoadedProfile = parent.SelectedBNet;
                    Task.Run(() => ReportsClient.Load_Carnage_Report_List(SelectedChar, parent.CarnageCountReq,parent.MatchFilterCode));
                    
                }

            }

        }



        #region Searching and loading twitch vods
        private List<TreeNode> ResetStreamNodes;
        private int ChannelsToCrawl = 0;
        private int CHannelsCrawled = 0;


        //Depricated method for loading users into vods
        private void QueueTwitchVods()
        {
            IsBusy = true;
            ResetStreamNodes = new List<TreeNode>();
            ChannelsToCrawl = Recent_With_Twitch.Count;
            CHannelsCrawled = 0;

            progressBar1.Value = 0;
            progressBar1.Maximum = ChannelsToCrawl;
            SetStatusMessage("Checking users for vods", 0, ChannelsToCrawl);

            foreach (BNet_Profile checkUser in Recent_With_Twitch)
            {
                Task.Run(() => Check_User_Vods(checkUser));
            }
        }

        public void Check_User_Vods(BNet_Profile inputUser)
        {
            if(parent.Main_Twitch_Client.isValidated)
            {
                TTV_Client vodClient = new TTV_Client();
                vodClient.Twitch_API_Key = parent.Main_Twitch_Client.Twitch_API_Key;
                vodClient.Twitch_API_Secret = parent.Main_Twitch_Client.Twitch_API_Secret;
                vodClient.Twitch_API_Token = parent.Main_Twitch_Client.Twitch_API_Token;

                vodClient.Client_Event += VodClient_Client_Event;

                vodClient.Check_and_match_Vods(inputUser.TwitchName);

                if (vodClient.Current_Channel != null)
                {
                    if(vodClient.Current_Channel.ChannelVideos != null)
                    {
                        //vodClient.Current_Channel.SetChannelIcon();
                        TreeNode MatchedStreamerNode = new TreeNode(inputUser.GlobalName() + " | twitch.tv/" + inputUser.TwitchName);
                        MatchedStreamerNode.Tag = inputUser;

                        foreach(Twitch_Vod vod in vodClient.Current_Channel.ChannelVideos)
                        {
                            int i = 0;

                            while(i < inputUser.LinkedMatches.Count)
                            {
                                DateTime vodCreated = vod.returnVodTime();
                              
                                DateTime AccountforDuration = vod.returnVodTime();
                                AccountforDuration += vod.returnVodDuration();
                                DateTime CheckAgainst = inputUser.LinkedMatches[i].returnMatchDate();

                                if (vodCreated.Date == CheckAgainst.Date || CheckAgainst.Ticks < AccountforDuration.Ticks)
                                {
                                    if (CheckAgainst.Ticks > vodCreated.Ticks && CheckAgainst.Ticks < AccountforDuration.Ticks)
                                    {

                                        TimeSpan offset = CheckAgainst.TimeOfDay - vodCreated.TimeOfDay;

                                        System.Diagnostics.Debug.Print("offseting : " + offset.ToString());

                                        if (offset.Hours < 0)
                                        {
                                            offset = offset.Add(new TimeSpan(24, 0, 0));
                                            System.Diagnostics.Debug.Print("Corrected Negative offset : " + offset.ToString());
                                        }


                                        string twitchLink = "http://www.twitch.tv";

                                        //if (vod.Video_Duration.Contains("."))
                                        //{
                                          //  int Voddays = 0;
                                          //
                                            //string[] daySplit = vod.videoDuration.ToString().Split('.');
                                            //
                                            //Voddays = Convert.ToInt32(daySplit[0].ToString());

                                            //Voddays *= 24;
                                            //
                                            //Voddays += offset.Hours;

                                            //twitchLink = vod.videoLink + "?t=" + Voddays.ToString() + "h" + offset.Minutes + "m" + offset.Seconds + "s";

                                        //}
                                        //else
                                        //{
                                            twitchLink = vod.Video_Url + "?t=" + offset.Hours + "h" + offset.Minutes + "m" + offset.Seconds + "s";
                                        //}




                                        System.Diagnostics.Debug.Print("ADDING MATCH NODE : for " + inputUser.GlobalName() + " | linked matches found : " + inputUser.LinkedMatches.Count.ToString() + " on match : " + i.ToString());
                                        TreeNode twitchNode = new TreeNode(twitchLink);

                                        twitchNode.Nodes.Add(inputUser.LinkedMatches[i].matchDateString);
                                        twitchNode.Nodes.Add(inputUser.LinkedMatches[i].referenceId + " | " + inputUser.LinkedMatches[i].directorActivityHash);
                                        twitchNode.Tag = twitchLink;
                                        twitchNode.Nodes[0].Tag = twitchLink;
                                        twitchNode.Nodes[1].Tag = twitchLink;
                                        MatchedStreamerNode.Nodes.Add(twitchNode);

                                    }

                                }
                                i += 1;
                            }
                        }


                        if (MatchedStreamerNode.Nodes.Count > 0)
                        {
                            vodClient.Current_Channel.SetChannelIcon();
                            System.Diagnostics.Debug.Print("Added match to node list");
                            ResetStreamNodes.Add(MatchedStreamerNode);
                        }

                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("Channel " + inputUser.TwitchName + " has no VoDs");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Print("Could not find channel : " + inputUser.TwitchName);
                }

                SetStatusMessage("Processing linked twitch accounts for vods ", 1, ChannelsToCrawl);
                CHannelsCrawled += 1;

                if (CHannelsCrawled >= ChannelsToCrawl)
                {
                    IsBusy = false;
                    DisplayVODS();

                }
            }
        }

        private void DisplayVODS()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { DisplayVODS(); });
                return;
            }
            IsBusy = false;
            treePlayers.Nodes.Clear();

            ResetStreamTree(ResetStreamNodes);
        }



        //Somewhat depricated method for clearing and resorting the vods found
        private void ResetStreamTree(List<TreeNode> streamers)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                { ResetStreamTree(streamers); });
                return;
            }
            List<DateTime> MatchedTimes = new List<DateTime>();
           

            foreach (TreeNode streamerNode in streamers)
            {
                if (streamerNode.Nodes.Count > 1)
                {

                    List<TreeNode> SubMatches = new List<TreeNode>();
                    List<DateTime> SubMatchTimes = new List<DateTime>();
                    foreach (TreeNode linkNode in streamerNode.Nodes)
                    {
                        SubMatchTimes.Add(DateTime.Parse(linkNode.Nodes[0].Text));
                        System.Diagnostics.Debug.Print(linkNode.Nodes[0].Text);
                    }

                    SubMatchTimes.Sort((x, y) => y.CompareTo(x));
                    foreach (DateTime matchCompare in SubMatchTimes)
                    {
                        foreach (TreeNode readd in streamerNode.Nodes)
                        {
                            if (DateTime.Parse(readd.Nodes[0].Text).ToString() == matchCompare.ToString())
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

                    MatchedTimes.Add(DateTime.Parse(streamerNode.Nodes[0].Nodes[0].Text));
                }
                else
                {
                    MatchedTimes.Add(DateTime.Parse(streamerNode.Nodes[0].Nodes[0].Text));
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
                    if (DateTime.Parse(streamerNode.Nodes[0].Nodes[0].Text).ToString() == compareTo.ToString() && !reAddStreams.Contains(streamerNode))
                    {
                        reAddStreams.Add(streamerNode);
                    }
                }
            }

            foreach (TreeNode stream in reAddStreams)
            { 
                treePlayers.Nodes.Add(stream); 
            }
            treePlayers.Update();

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
        private void VodClient_Client_Event(object sender, TTV_Client.Twitch_Client_Event e)
        {
            // new NotImplementedException();
        }



        #endregion





        #region Status updating
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

        #endregion

        
       
       






        private void button1_Click(object sender, EventArgs e)
        {

            if (!IsBusy)
            {
                IsBusy = true;
                treePlayers.Nodes.Clear();
                treeCarnageReports.Nodes.Clear();
                lblStatus.Text = "Loading carnage report list";
                lblCarnageReports.Text = parent.SelectedBNet.SelectedMemebership.platformDisplayName + " | " + CurrentCharacter.classHash;
                ReportsClient.LoadedProfile = parent.SelectedBNet;
                Task.Run(() => ReportsClient.Load_Carnage_Report_List(CurrentCharacter, parent.CarnageCountReq, parent.MatchFilterCode));
            }
        }

        private void treePlayers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(string))
            {
                Clipboard.SetText((string)e.Node.Tag);
                e.Node.Expand();
            }
        }

        private void treePlayers_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (treePlayers.Nodes.Count > 0)
            {
                string StreamList = "";
                foreach (TreeNode streamHeader in treePlayers.Nodes)
                {
                    StreamList += streamHeader.Text + "\n\n";
                    foreach (TreeNode streamText in streamHeader.Nodes)
                    {
                        StreamList += streamText.Text + "\n" + streamText.Nodes[0].Text + "\n" + streamText.Nodes[1].Text + "\n\n";
                    }
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string ticks = Environment.TickCount.ToString();
                path += "\\" + ticks + ".txt";

                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine(StreamList);
                sw.Close();

                System.Diagnostics.Process.Start(path);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
