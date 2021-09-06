using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carnage_Clips
{
    public partial class UserSearchForm : Form
    {
        MainForm parent;
        Bungie_API_Client searchClient;

        private bool IsBusy = false;
        public UserSearchForm(MainForm parentForm)
        {
            InitializeComponent();
            parent = parentForm;
            searchClient = new Bungie_API_Client();
            searchClient.API_Client_Event += SearchClient_API_Client_Event;
            searchClient.Bungie_API_Key = "9efe9b8eba3042afb081121d447fd981";

        }

        public void Load_ALternate_Character_Set(Bungie_Profile.AlternateAccounts InputAccount)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                Bungie_Profile AltProfile = new Bungie_Profile();
                AltProfile.MainAccountID = InputAccount.MembershipID;
                AltProfile.MainAccountType = InputAccount.AccountType;
                UpdateStatus("Loading platform " + InputAccount.Platform.ToString() + " for " + InputAccount.DisplayName);

                Task.Run(() => searchClient.Load_Alternate_Character_Entries(AltProfile));
            }
        }

        private void UpdateStatus(string StatusMessage = "Idle")
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    UpdateStatus(StatusMessage);
                });
                return;
            }

            lblStatus.Text = StatusMessage;
            lblStatus.Update();

        }
        private void SearchClient_API_Client_Event(object sender, Bungie_API_Client.Client_Event_Type e)
        {
            switch (e)
            {
                case Bungie_API_Client.Client_Event_Type.UserSearchComplete:
                    IsBusy = false;
                    if(sender != null)
                    {
                        Display_Found_Users((List<Bungie_Profile>)sender);
                    }
                    break;
                case Bungie_API_Client.Client_Event_Type.UserSearchFail:
                    IsBusy = false;
                    UpdateStatus("Either no users were found or an error occured, please try again");
                    break;
                case Bungie_API_Client.Client_Event_Type.CharacterLoadComplete:
                    Load_Detailed_UserInformation((Bungie_Profile)sender);
                    break;
                
                case Bungie_API_Client.Client_Event_Type.CheckLinkedComplete:
                    IsBusy = false;
                    if (sender != null)
                    {
                        Display_Detailed_Info((Bungie_Profile)sender);
                        UpdateStatus("Displaying detailed information");
                    }
                    break;
                case Bungie_API_Client.Client_Event_Type.CheckLinkedFail:
                    IsBusy = false;
                    UpdateStatus("An error occured loading detailed information");
                    break;
                case Bungie_API_Client.Client_Event_Type.AlternateProfileLoadComplete:
                    IsBusy = false;
                    Display_Alternate_Account((Bungie_Profile)sender);
                    
                    break;
                default:
                    IsBusy = false;
                    break;

            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IsBusy = true;
            parent.HideCharacterPanel();
            treeUserDetailed.Nodes.Clear();
            lblStatus.Text = "Searching for user " + txtUserSearch.Text;
            Task.Run(() => searchClient.SearchDestinyAccounts(txtUserSearch.Text));

        }


        private void Display_Alternate_Account(Bungie_Profile InputAccount)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Display_Alternate_Account(InputAccount);
                });
                return;
            }
            UpdateStatus("Displaying " + InputAccount.Bungie_Display_Name + " characters");
            parent.Update_Selected_User(InputAccount,true);
        }
        private void Display_Found_Users(List<Bungie_Profile> FoundUsers)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Display_Found_Users(FoundUsers);
                });
                return;
            }

            foreach(Bungie_Profile FoundAccount in FoundUsers)
            {
                ListViewItem userAcc = new ListViewItem(FoundAccount.Bungie_Display_Name + "#" + FoundAccount.Bungie_User_Code);
                userAcc.Tag = FoundAccount;
                lstFoundUsers.Items.Add(userAcc);

                
            }

            lblStatus.Text = "Found " + FoundUsers.Count + " users matching that name";

            if (FoundUsers.Count == 1)
            {
                treeUserDetailed.Nodes.Clear();
                IsBusy = true;
                Bungie_Profile selectedAccount = FoundUsers[0];
                UpdateStatus("Loading characters and detailed information for user " + selectedAccount.Bungie_Display_Name + "#" + selectedAccount.Bungie_User_Code);
                Task.Run(() => searchClient.Load_Character_Entries(selectedAccount));
            }
        }

        private void lstFoundUsers_DoubleClick(object sender, EventArgs e)
        {
            if(lstFoundUsers.SelectedItems[0] != null)
            {

                if (!IsBusy)
                {
                    treeUserDetailed.Nodes.Clear();
                    IsBusy = true;
                    Bungie_Profile selectedAccount = (Bungie_Profile)lstFoundUsers.SelectedItems[0].Tag;
                    UpdateStatus("Loading characters and detailed information for user " + selectedAccount.Bungie_Display_Name + "#" + selectedAccount.Bungie_User_Code);
                    Task.Run(() => searchClient.Load_Character_Entries(selectedAccount));
                }
            }
        }

        private Bungie_Profile Charactercontainer;
        private Bungie_Profile BNetContainer;

        private void Load_Detailed_UserInformation(Bungie_Profile userToLoad)
        {
            Task.Run(() => searchClient.Load_Bnet_Info(userToLoad));
        }


        private void Display_Detailed_Info(Bungie_Profile InputAccount)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Display_Detailed_Info(InputAccount);
                });
                return;
            }

            TreeNode DetailNode = new TreeNode(InputAccount.Bungie_Display_Name + "#" + InputAccount.Bungie_User_Code);
            DetailNode.Nodes.Add("Bungie Profile Code : " + InputAccount.BunigeNet_Account_ID);

            foreach(Bungie_Profile.Destiny_Character dchar in InputAccount.Linked_Characters)
            {
                TreeNode CharNode = new TreeNode(dchar.Class + " : " + dchar.Light_Level);
                CharNode.Nodes.Add(dchar.EmblemLink);
                CharNode.Nodes.Add(dchar.CharacterID);

                DetailNode.Nodes.Add(CharNode);
            }

            if(InputAccount.TwitchLinked)
            {
                TreeNode TwitchNode = new TreeNode("Twitch");
                TwitchNode.Nodes.Add(InputAccount.TwitchName);

                DetailNode.Nodes.Add(TwitchNode);
            }

            if(InputAccount.Alt_Accounts.Count > 0)
            {
                foreach(Bungie_Profile.AlternateAccounts alt in InputAccount.Alt_Accounts)
                {
                    TreeNode AltNode = new TreeNode(alt.Platform.ToString());
                    AltNode.Nodes.Add(alt.DisplayName);
                    AltNode.Tag = alt;
                    DetailNode.Nodes.Add(AltNode);
                }
            }

            treeUserDetailed.Nodes.Add(DetailNode);


            parent.Update_Selected_User(InputAccount);
        }

    }
}
