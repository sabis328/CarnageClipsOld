using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Drawing;

namespace Carnage_Clips
{
    public class Bungie_API_Client
    {
        public bool CancelAll { get; set; }
        public string Bungie_API_Key { get; set; }
        public string Twitch_API_Key { get; set; }
        public string Twitch_API_Secret { get; set; }

        public bool In_Progress { get; set; }

        private string API_Base_Path = "https://www.bungie.net/Platform/";

        private string PlayerExclusion { get; set; }

        public event EventHandler<Client_Event_Type> API_Client_Event;

        public enum Client_Event_Type
        { 
            AlternateProfileLoadComplete,
            UserSearchComplete,
            UserSearchFail,
            CharacterLoadComplete,
            CharacterLoadFail,
            SingleCarnageComplete,
            SingleCarnageFail,
            AllCarnageComplete,
            AllCarnageFail,
            CheckLinkedComplete,
            CheckLinkedFail,
            RecentPlayerBnetLoaded,
            RecentPlayerBnetFailed,
            CancelAll
        }

        //Returns a list of all users based on a prefix search

        #region Search Destiny users
        public void SearchDestinyAccounts(string UserPrefix,int PageNumber = 0)
        {
            if (!In_Progress && !CancelAll)
            {
                In_Progress = true;
                ServicePointManager.DefaultConnectionLimit = 15;
                try
                {
                    string codeMatch = "";
                    bool matchCode = false;
                    if(UserPrefix.Contains("#"))
                    {
                        string[] prefixArray = UserPrefix.Split('#');
                        UserPrefix = prefixArray[0];
                        
                        codeMatch = prefixArray[1];

                        if (codeMatch.Length > 0)
                        {
                            System.Diagnostics.Debug.Print("Need to match user code");
                            matchCode = true;
                        }
                    }

                    string requestLink = string.Format(API_Base_Path + "{0}{1}{2}", "User/Search/Prefix/",Uri.EscapeDataString( UserPrefix),"/" + PageNumber);
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                    Search_Client.KeepAlive = true;

                    string responseText;
                    //Looking for destinyMemberships container can get bungie names for each as well
                    //starts at destinyMemberships, goes until last index of bungie name code
                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    if (responseText.Contains("destinyMemberships"))
                    {
                        List<Bungie_Profile> AllFoundUsers = ParseBungieProfiles(responseText);

                        if (responseText.Contains("\"hasMore\":true"))
                        {
                            int i = 1;
                            string addition = "start";
                            while (addition != "end")
                            {
                                addition = SearchDestinyAccounts_AdditionPages(UserPrefix, i);
                                List<Bungie_Profile> AdditionalAccounts = ParseBungieProfiles(addition);

                                foreach(Bungie_Profile newProf in AdditionalAccounts)
                                {
                                    AllFoundUsers.Add(newProf);
                                }
                                i++;
                            }
                        }

                        if(matchCode)
                        {
                            foreach(Bungie_Profile checkUser in AllFoundUsers)
                            {
                                if(checkUser.Bungie_User_Code == codeMatch)
                                {
                                    AllFoundUsers = new List<Bungie_Profile>();
                                    AllFoundUsers.Add(checkUser);
                                    break;
                                }
                            }
                        }

                        //Raise Search Complete Event
                        In_Progress = false;
                        API_Client_Event?.Invoke(AllFoundUsers, Client_Event_Type.UserSearchComplete);
                        
                    }
                    else
                    {
                        //Raise searchfail event
                        In_Progress = false;
                        API_Client_Event?.Invoke(null, Client_Event_Type.UserSearchFail);
                    }

                }
                catch
                {
                    In_Progress = false;
                    API_Client_Event?.Invoke(null, Client_Event_Type.UserSearchFail);
                }
            }
        }

        private string SearchDestinyAccounts_AdditionPages(string UserPrefix, int PageNumber)
        {
            ServicePointManager.DefaultConnectionLimit = 15;
            try
            {
                string requestLink = string.Format(API_Base_Path + "{0}{1}{2}", "User/Search/Prefix/", Uri.EscapeDataString(UserPrefix),"/" +  PageNumber);
                HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                Search_Client.Method = "GET";
                Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                Search_Client.KeepAlive = true;

                string responseText;
                
                using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                {
                    responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                    Search_Response.Close();
                }

                if (responseText.Contains("destinyMemeberships"))
                {
                    return responseText;
                }
                else
                {
                    return "end";
                }

            }
            catch
            {
                return "end";
            }
        }

        private List<Bungie_Profile> ParseBungieProfiles(string InputText)
        {
            int start = 0;
            int end = 0;
            int temp = 0;

            List<Bungie_Profile> Users = new List<Bungie_Profile>();
            while (start < InputText.LastIndexOf("destinyMemberships")) 
            {
                start = InputText.IndexOf("destinyMemberships", start) + 18;
                end = InputText.IndexOf("bungieGlobalDisplayNameCode", start);
                temp = end;
                end = InputText.IndexOf("}", temp) + 1;

                string UserMeta = InputText.Substring(start, end - start);
                System.Diagnostics.Debug.Print(UserMeta);


                Users.Add(ParseProfileMeta(UserMeta));
            }
            return Users;


        }

        private Bungie_Profile ParseProfileMeta(string metaInput)
        {
            //FirstProfile Listed is Main profile

            int start = 0;
            int end = 0;

            start = metaInput.IndexOf("membershipType") + 16;
            end = metaInput.IndexOf(",", start);

            string memberType = metaInput.Substring(start, end - start);

            System.Diagnostics.Debug.Print(memberType);

            start = metaInput.IndexOf("membershipId", end) + 15;
            end = metaInput.IndexOf("\",", start);


            string memeberID = metaInput.Substring(start, end - start);
            System.Diagnostics.Debug.Print(memeberID);

            start = metaInput.IndexOf("bungieGlobalDisplayName", end) + 26;
            end = metaInput.IndexOf("\",", start);

            string GlobalDisplayName = metaInput.Substring(start, end - start);
            System.Diagnostics.Debug.Print(GlobalDisplayName);

            start = metaInput.IndexOf("bungieGlobalDisplayNameCode", end) + 29;
            end = metaInput.IndexOf("}", start);

            string GlobalIDCode = metaInput.Substring(start, end - start);
            System.Diagnostics.Debug.Print(GlobalIDCode);

            Bungie_Profile User = new Bungie_Profile();
            User.Bungie_User_Code = GlobalIDCode;
            User.Bungie_Display_Name = GlobalDisplayName;
            User.MainAccountID = memeberID;
            User.MainAccountType = memberType;

            return User;
        }
        #endregion

        //Loads the characters and emblems for a selected user
        #region Load Player Character information
        //Load profile information for a user by ID

        public void Load_Character_Entries(Bungie_Profile InputUser)
        {
            if (!In_Progress && !CancelAll)
            {
                In_Progress = true;
                ServicePointManager.DefaultConnectionLimit = 15;
                try
                {
                     ///Destiny2/{membershipType}/Profile/{destinyMembershipId}/
                    string requestLink = string.Format(API_Base_Path + "{0}{1}{2}", "Destiny2/", InputUser.MainAccountType, "/Profile/" + InputUser.MainAccountID + "/?components=200");
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                    Search_Client.KeepAlive = true;

                    string responseText;
                    //Looking for destinyMemberships container can get bungie names for each as well
                    //starts at destinyMemberships, goes until last index of bungie name code
                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    List<Bungie_Profile.Destiny_Character> UserCharacters = Parse_Character_Meta(responseText,InputUser.MainAccountID);

                    InputUser.Linked_Characters = UserCharacters;

                    foreach(Bungie_Profile.Destiny_Character dchar in InputUser.Linked_Characters)
                    {
                        dchar.AccountOwner = InputUser;
                    }
                    In_Progress = false;
                    API_Client_Event?.Invoke(InputUser, Client_Event_Type.CharacterLoadComplete);
                }
                catch
                {
                    In_Progress = false;
                    API_Client_Event?.Invoke(null, Client_Event_Type.CharacterLoadFail);
                }
            }
        }


        public void Load_Alternate_Character_Entries(Bungie_Profile InputUser)
        {
            if (!In_Progress && !CancelAll)
            {
                In_Progress = true;
                ServicePointManager.DefaultConnectionLimit = 15;
                try
                {
                    ///Destiny2/{membershipType}/Profile/{destinyMembershipId}/
                    string requestLink = string.Format(API_Base_Path + "{0}{1}{2}", "Destiny2/", InputUser.MainAccountType, "/Profile/" + InputUser.MainAccountID + "/?components=200");
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                    Search_Client.KeepAlive = true;

                    string responseText;
                    //Looking for destinyMemberships container can get bungie names for each as well
                    //starts at destinyMemberships, goes until last index of bungie name code
                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    List<Bungie_Profile.Destiny_Character> UserCharacters = Parse_Character_Meta(responseText, InputUser.MainAccountID);

                    InputUser.Linked_Characters = UserCharacters;

                    foreach (Bungie_Profile.Destiny_Character dchar in InputUser.Linked_Characters)
                    {
                        dchar.AccountOwner = InputUser;
                    }
                    In_Progress = false;
                    API_Client_Event?.Invoke(InputUser, Client_Event_Type.AlternateProfileLoadComplete);
                }
                catch
                {
                    In_Progress = false;
                    API_Client_Event?.Invoke(null, Client_Event_Type.CharacterLoadFail);
                }
            }
        }

        private List<Bungie_Profile.Destiny_Character> Parse_Character_Meta(string CharacterMeta, string MainID)
        {
            int start = 0;
            int end = 0;
            List<Bungie_Profile.Destiny_Character> FoundCharacters = new List<Bungie_Profile.Destiny_Character>();

            while(start < CharacterMeta.LastIndexOf(MainID))
            {
                start = CharacterMeta.IndexOf(MainID, end);
                end = CharacterMeta.IndexOf("emblemHash", start); ;

                FoundCharacters.Add(Parse_Character_Entry(CharacterMeta.Substring(start, end - start)));
            }

            return FoundCharacters;
        }

        private Bungie_Profile.Destiny_Character Parse_Character_Entry(string inputMeta)
        {
            System.Diagnostics.Debug.Print(inputMeta);
            ////https://www.bungie.net/common/destiny2_content/icons/06986179c5665b7968bb2c1b8bdb08b4.jpg
            //emblemBackgroundPath


            Bungie_Profile.Destiny_Character ParsedCharacter = new Bungie_Profile.Destiny_Character();

            int start = 0;
            int end = 0;

            start = inputMeta.IndexOf("characterId", 0) + 14;
            end = inputMeta.IndexOf("\",\"", start);

            ParsedCharacter.CharacterID = inputMeta.Substring(start, end - start);

            start = inputMeta.IndexOf("light", end) + 7;
            end = inputMeta.IndexOf(",", start);

            ParsedCharacter.Light_Level = inputMeta.Substring(start, end - start);

            start = inputMeta.IndexOf("classType", end) + 11;
            end = inputMeta.IndexOf(",", start);

            string classType = inputMeta.Substring(start, end - start);

            switch(classType)
            {
                case "0":
                    ParsedCharacter.Class = Bungie_Profile.Destiny_Character.CharacterType.Titan;
                    break;
                case "1":
                    ParsedCharacter.Class = Bungie_Profile.Destiny_Character.CharacterType.Hunter;
                    break;
                case "2":
                    ParsedCharacter.Class = Bungie_Profile.Destiny_Character.CharacterType.Warlock;
                    break;
            }
            start = inputMeta.IndexOf("emblemBackgroundPath", end) + 23;
            end = inputMeta.IndexOf("\",", start);

            ParsedCharacter.EmblemLink = "https://www.bungie.net" + inputMeta.Substring(start, end - start);

            System.Diagnostics.Debug.Print("Loading image : " + ParsedCharacter.EmblemLink);

            HttpWebRequest emblemReq = (HttpWebRequest)WebRequest.Create(ParsedCharacter.EmblemLink);
            emblemReq.AllowWriteStreamBuffering = true;
            emblemReq.Timeout = 5000;

            using(HttpWebResponse emblemResponse = (HttpWebResponse)emblemReq.GetResponse() )
            {
                Stream emblemStream = emblemResponse.GetResponseStream();
                ParsedCharacter.Emblem = System.Drawing.Image.FromStream(emblemStream);
                emblemResponse.Close();
            }
            
            
            return ParsedCharacter;
        }

        #endregion

        //Loads detailed account information for a selected user
        #region Load Detailed information

        public void Load_Bnet_Info(Bungie_Profile userToLoad)
        {
            //https://www.bungie.net/Platform/User/GetMembershipsById/4611686018435413663/-1/

            if (!In_Progress && !CancelAll)
            {
                In_Progress = true;
                ServicePointManager.DefaultConnectionLimit = 15;
                try
                {
                    ///Destiny2/{membershipType}/Profile/{destinyMembershipId}/
                    string requestLink = string.Format("https://www.bungie.net/Platform/User/GetMembershipsById/" + "{0}{1}", userToLoad.MainAccountID, "/-1/");
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                    Search_Client.KeepAlive = true;

                    string responseText;
                   
                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    Bungie_Profile tempHolder = ParseBNetProfile(responseText);
                    if (tempHolder != null)
                    {
                        userToLoad.BunigeNet_Account_ID = tempHolder.BunigeNet_Account_ID;
                        userToLoad.TwitchLinked = tempHolder.TwitchLinked;
                        if (userToLoad.TwitchLinked)
                        {
                            userToLoad.TwitchName = tempHolder.TwitchName;
                        }
                        userToLoad.Alt_Accounts = tempHolder.Alt_Accounts;

                        In_Progress = false;

                        API_Client_Event?.Invoke(userToLoad, Client_Event_Type.CheckLinkedComplete);
                    }
                    else
                    {
                        In_Progress = false;
                        API_Client_Event?.Invoke(null, Client_Event_Type.CheckLinkedFail);
                    }
                }
                catch
                {
                    In_Progress = false;
                    API_Client_Event?.Invoke(null, Client_Event_Type.CheckLinkedFail);
                }
            }
        }





        private Bungie_Profile ParseBNetProfile(string inputMeta)
        {
            Bungie_Profile profContainer;
            int start = 0;
            int end = 0;

            start = inputMeta.IndexOf("destinyMemberships", end);
            end = inputMeta.IndexOf("}]", start);
           
            List<Bungie_Profile.AlternateAccounts> subAccounts = ReadAlts(inputMeta.Substring(start, end - start));

          
            if (inputMeta.Contains("bungieNetUser"))
            {
                start = inputMeta.IndexOf("bungieNetUser", end);
                end = inputMeta.Length;
               
                profContainer = ReadBNet(inputMeta.Substring(start, end - start));

                profContainer.Alt_Accounts = subAccounts;
                return profContainer;
            }
            else
            {
                profContainer = new Bungie_Profile();
                profContainer.Alt_Accounts = subAccounts;


                return profContainer;
            }
           
        }

        private List<Bungie_Profile.AlternateAccounts> ReadAlts(string accountMeta)
        {
            int start = 0;
            int end = 0;
            List<Bungie_Profile.AlternateAccounts> ALTS = new List<Bungie_Profile.AlternateAccounts>();

            while(start < accountMeta.LastIndexOf("membershipType"))
            {
                Bungie_Profile.AlternateAccounts altAcc = new Bungie_Profile.AlternateAccounts();
                start = accountMeta.IndexOf("membershipType", end) + 16;
                end = accountMeta.IndexOf(",", start);

                string accType = accountMeta.Substring(start, end - start);
                altAcc.AccountType = accType;
                switch (accType)
                {
                    case "3":
                        altAcc.Platform = Bungie_Profile.AlternateAccounts.PlatformType.Steam;
                       
                        break;
                    case "1":
                        altAcc.Platform = Bungie_Profile.AlternateAccounts.PlatformType.Xbox;
                        break;
                    case "2":
                        altAcc.Platform = Bungie_Profile.AlternateAccounts.PlatformType.PSN;
                        break;
                }

                start = accountMeta.IndexOf("membershipId", end) + 15;
                end = accountMeta.IndexOf("\",", start);

                altAcc.MembershipID = accountMeta.Substring(start, end - start);
                System.Diagnostics.Debug.Print("Alternate account ID found : " + altAcc.MembershipID);

                start = accountMeta.IndexOf("displayName", end) + 14;
                end = accountMeta.IndexOf("\",", start);

                altAcc.DisplayName = accountMeta.Substring(start, end - start);
                ALTS.Add(altAcc);
            }

            return ALTS;
        }
        private Bungie_Profile ReadBNet(string bnetMeta)
        {

            Bungie_Profile BNetDetail = new Bungie_Profile();
            int start = 0;
            int end = 0;

            start = bnetMeta.IndexOf("membershipId", 0) + 15;
            end = bnetMeta.IndexOf("\",", start);

            BNetDetail.BunigeNet_Account_ID = bnetMeta.Substring(start, end - start);
            if (bnetMeta.Contains("twitchDisplayName"))
            {
                start = bnetMeta.IndexOf("twitchDisplayName", 0) + 20;
                end = bnetMeta.IndexOf("\"", start);

                BNetDetail.TwitchLinked = true;
                BNetDetail.TwitchName = bnetMeta.Substring(start, end - start);
            }
            return BNetDetail;
        }
        #endregion

        //Carnage report loading 
        #region Carnage Repot Loading
        public List<Carnage_Report> RecentMatches { get; set; }
        public List<Bungie_Profile> RecentPlayers { get; set; }
        public  List<string> RecentPlayerCompare { get; set; }
        public int ReportsToLoad { get; set; }
        public int ReportsLoaded { get; set; }
        public void LoadCarnageReportList(Bungie_Profile.Destiny_Character SelectedChar, int count = 50)
        {
            if(!In_Progress && !CancelAll)
            {
                PlayerExclusion = SelectedChar.AccountOwner.MainAccountID;
                RecentMatches = new List<Carnage_Report>();
                RecentPlayers = new List<Bungie_Profile>();
                RecentPlayerCompare = new List<string>();

                HttpWebRequest reportClient;
                ServicePointManager.DefaultConnectionLimit = 15;
                ReportsLoaded = 0;
                ReportsToLoad = count;

                try
                { 
                    reportClient = (HttpWebRequest)WebRequest.Create(string.Format(API_Base_Path + "Destiny2/" + "{0}{1}{2}{3}{4}{5}", SelectedChar.AccountOwner.MainAccountType, "/Account/", SelectedChar.AccountOwner.MainAccountID
                        , "/Character/", SelectedChar.CharacterID, "/Stats/Activities/?count=" + count.ToString()));
                    reportClient.Method = "GET";
                    reportClient.Headers.Add("X-API-KEY", "9efe9b8eba3042afb081121d447fd981");
                    reportClient.KeepAlive = false;

                    string responseBody;
                    using (HttpWebResponse _response = (HttpWebResponse)reportClient.GetResponse())
                    {
                        responseBody = new StreamReader(_response.GetResponseStream()).ReadToEnd();
                        _response.Close();
                    }
                    //System.Diagnostics.Debug.Print(responseBody);
                    int start = 0;
                    int end = 0;
                    List<string> Matches = new List<string>();
                    while (start < responseBody.LastIndexOf("instanceId"))
                    {
                        start = responseBody.IndexOf("instanceId", end) + 13;
                        end = responseBody.IndexOf(",", start) - 1;
                        Matches.Add(responseBody.Substring(start, end - start));
                    }

                    //List<CarnageReport> CarnageReports = new List<CarnageReport>();
                    int i = 1;
                    foreach (string matchID in Matches)
                    {
                        System.Diagnostics.Debug.Print( "Match Number " + i.ToString() + " :   " + matchID);
                        Task.Run(() => LoadSingleCarnageReport(matchID, SelectedChar.AccountOwner));
                        i++;                    }

                }
                catch
                { 
                
                }

            }
        }
        public List<Bungie_Profile.Destiny_Weapon> ProcessWeaponMeta(string inputData, string LinkedMatchID)
        {

            List<Bungie_Profile.Destiny_Weapon> playerWeps = new List<Bungie_Profile.Destiny_Weapon>();

            int start = 0;
            int end = 0;
            int lastIndex = inputData.LastIndexOf("referenceId");
            if (lastIndex == 0)
            {
                //System.Diagnostics.Debug.Print("only one wep");
                lastIndex = 5;
            }
            if (lastIndex < 0)
            {
                return playerWeps;
            }
            while (start < lastIndex)
            {
                start = inputData.IndexOf("referenceId", end) + 13;
                end = inputData.IndexOf(",", start);

                string wepid = inputData.Substring(start, end - start);

                start = inputData.IndexOf("uniqueWeaponKills", end);
                end = inputData.IndexOf("displayValue", start);
                start = end + 15;
                end = inputData.IndexOf("\"", start);

                string wepkill = inputData.Substring(start, end - start);

                start = inputData.IndexOf("uniqueWeaponKillsPrecisionKills", end);
                end = inputData.IndexOf("displayValue", start);
                start = end + 15;
                end = inputData.IndexOf("\"", start);
                if (end < 0)
                {
                    end = inputData.Length;
                }

                string wepPrec = inputData.Substring(start, end - start);

                Bungie_Profile.Destiny_Weapon wep = new Bungie_Profile.Destiny_Weapon();
                wep.WeaponIdentifier = wepid;
                wep.WeaponPrecisionRatio = wepPrec;
                wep.WeaponKills = wepkill;
                wep.IdentifyWeapon();

                playerWeps.Add(wep);

                //System.Diagnostics.Debug.Print("Found : " + playerWeps.Count + "for a user");
            }

            return playerWeps;
        }
        private Bungie_Profile ParseMatchProfile(string inputMeta, string id)
        {
            //System.Diagnostics.Debug.Print("Match Profile to parse : \n" + inputMeta);

            try
            {
                int start = 0;
                int end = 0;

                start = inputMeta.IndexOf("membershipId", end) + 15;
                end = inputMeta.IndexOf("\",", start);

                string memID = inputMeta.Substring(start, end - start);



                start = inputMeta.IndexOf("bungieGlobalDisplayName\"", end) + 26;
                end = inputMeta.IndexOf("\"", start);

                string bdisplay = inputMeta.Substring(start, end - start);

                start = inputMeta.IndexOf("bungieGlobalDisplayNameCode", end) + 29;
                end = inputMeta.IndexOf("}", start);

                string bcode = inputMeta.Substring(start, end - start);

                Bungie_Profile parsedProf = new Bungie_Profile();
                parsedProf.MainAccountID = memID;
                parsedProf.Bungie_Display_Name = bdisplay;
                parsedProf.Bungie_User_Code = bcode;
                parsedProf.LinkedMatches = new List<Carnage_Report>();
                parsedProf.LinkedMatchTimes = new List<DateTime>();

                return parsedProf;
            }
            catch
            {
                Bungie_Profile parsedProf = new Bungie_Profile();
                parsedProf.MainAccountID = "Error";
                parsedProf.Bungie_Display_Name = "Error";
                parsedProf.Bungie_User_Code = "Error";
                parsedProf.LinkedMatches = new List<Carnage_Report>();
                parsedProf.LinkedMatchTimes = new List<DateTime>();
                System.Diagnostics.Debug.Print("Match - " + id + "\n" + inputMeta);
                return parsedProf;
            }
        }
        private void LoadSingleCarnageReport(string matchID, Bungie_Profile inputAccount)
        {
            HttpWebRequest _client;
            ServicePointManager.DefaultConnectionLimit = 15;
            int start = 0;
            int end = 0;
            Carnage_Report PGCR = new Carnage_Report();
            try
            {

                
                PGCR.ActivityRefID = matchID;
                
                _client = (HttpWebRequest)WebRequest.Create("https://stats.bungie.net/Platform/Destiny2/Stats/PostGameCarnageReport/" + matchID + "/");
                _client.Method = "GET";
                _client.Headers.Add("X-API-KEY", "9efe9b8eba3042afb081121d447fd981");

                string matchData;
                using (HttpWebResponse _subresponse = (HttpWebResponse)_client.GetResponse())
                {
                    matchData = new StreamReader(_subresponse.GetResponseStream()).ReadToEnd();
                    _subresponse.Close();
                }
                start = 0;
                end = 0;

                start = matchData.IndexOf("period", end) + 9;
                end = matchData.IndexOf(",", start) - 1;
                string timetoParse = matchData.Substring(start, end - start);
                //System.Diagnostics.Debug.Print("1 : " + timetoParse);


                string temploc;
                string tempact;
                DateTime startAT = DateTime.Parse(timetoParse).ToUniversalTime();
                PGCR.ActivityStart = startAT;

                start = matchData.IndexOf("referenceId", end) + 13;
                end = matchData.IndexOf(",", start);

                temploc = matchData.Substring(start, end - start);

                //System.Diagnostics.Debug.Print("2 : " + temploc);
                PGCR.SetLocation(matchData.Substring(start, end - start));

                start = matchData.IndexOf("directorActivityHash", end) + 22;
                end = matchData.IndexOf(",", start);

                tempact = matchData.Substring(start, end - start);
                //System.Diagnostics.Debug.Print("3 : " + tempact);
                PGCR.SetGameType(matchData.Substring(start, end - start));

                start = 0;
                end = 0;

                List<Bungie_Profile> matchPlayers = new List<Bungie_Profile>();

               
                while (start < matchData.LastIndexOf("membershipId"))
                {
                    start = matchData.IndexOf("membershipId",end);
                    end = matchData.IndexOf("classHash", start);

                    
                    string playerMeta = matchData.Substring(start, end -start);
                   
                    Bungie_Profile matchPlayer = ParseMatchProfile(playerMeta, matchID);
                    matchPlayers.Add(matchPlayer);
                    

                    int wepstart = matchData.IndexOf("weapons", end);
                    if (wepstart > 0)
                    {
                        int wepend = matchData.IndexOf("destinyUserInfo", wepstart);
                        if (wepend < 0)
                        {
                            wepend = matchData.IndexOf("teams", wepstart);
                        }
                        string wepData = matchData.Substring(wepstart, wepend - wepstart);

                       
                        wepstart = 0;
                        wepend = 0;
                        wepstart = wepData.IndexOf("referenceId", 0);
                        int Tempwepend = wepData.LastIndexOf("uniqueWeaponKillsPrecisionKills");
                        wepend = wepData.IndexOf("displayValue", Tempwepend) + 15;
                        Tempwepend = wepData.IndexOf("\"", wepend);
                        string processedWepData = wepData.Substring(wepstart, Tempwepend - wepstart);

                       
                        matchPlayer.MatchWeapons = ProcessWeaponMeta(processedWepData, PGCR.ActivityRefID);
                        //System.Diagnostics.Debug.Print("Weapons and players loaded for " + matchID);
                    }


                    if (!RecentPlayerCompare.Contains(matchPlayer.MainAccountID))
                    {
                        if (matchPlayer.MainAccountID != PlayerExclusion)
                        {
                            RecentPlayerCompare.Add(matchPlayer.MainAccountID);
                            matchPlayer.LinkedMatches.Add(PGCR);
                            matchPlayer.LinkedMatchTimes.Add(PGCR.ActivityStart);
                            RecentPlayers.Add(matchPlayer);
                        }
                    }
                    else
                    {
                        foreach (Bungie_Profile g in RecentPlayers)
                        {
                            if (g.MainAccountID == matchPlayer.MainAccountID)
                            {
                                g.LinkedMatches.Add(PGCR);
                                g.LinkedMatchTimes.Add(PGCR.ActivityStart);
                            }
                        }
                    }

                }

                
                PGCR.ActivityPlayers = matchPlayers;
                ReportsLoaded += 1;
                RecentMatches.Add(PGCR);

                System.Diagnostics.Debug.Print("match " + ReportsLoaded + "/" + ReportsToLoad);
                API_Client_Event?.Invoke(PGCR, Client_Event_Type.SingleCarnageComplete);               
               
               

                if (ReportsLoaded == ReportsToLoad)
                {
                    System.Diagnostics.Debug.Print("All matches loaded");
                    RecentMatches.Sort((x, y) => y.ActivityStart.CompareTo(x.ActivityStart));

                    API_Client_Event?.Invoke(PGCR, Client_Event_Type.AllCarnageComplete);
                    return;
                }

               
            }
            catch (Exception ex)
            {
               ReportsLoaded += 1;
                if (ReportsLoaded == ReportsToLoad)
                {
                    System.Diagnostics.Debug.Print("All matches loaded");
                    RecentMatches.Sort((x, y) => y.ActivityStart.CompareTo(x.ActivityStart));

                    API_Client_Event?.Invoke(PGCR, Client_Event_Type.AllCarnageComplete);
                    return;
                }
                System.Diagnostics.Debug.Print("Failed to load carnage report - " + matchID);
               API_Client_Event?.Invoke(ReportsLoaded,Client_Event_Type.SingleCarnageFail);
            }

        }
        #endregion


        #region Checking Recent Player BNET

        public void Load_Recent_BENT(Bungie_Profile userToLoad)
        {
            //https://www.bungie.net/Platform/User/GetMembershipsById/4611686018435413663/-1/

            if (!In_Progress && !CancelAll)
            {
                In_Progress = true;
                ServicePointManager.DefaultConnectionLimit = 15;
                try
                {
                    ///Destiny2/{membershipType}/Profile/{destinyMembershipId}/
                    string requestLink = string.Format("https://www.bungie.net/Platform/User/GetMembershipsById/" + "{0}{1}", userToLoad.MainAccountID, "/-1/");
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", Bungie_API_Key);
                    Search_Client.KeepAlive = true;

                    string responseText;

                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    Bungie_Profile tempHolder = ReadBNet(responseText);
                    if (tempHolder != null)
                    {
                        userToLoad.BunigeNet_Account_ID = tempHolder.BunigeNet_Account_ID;
                        userToLoad.TwitchLinked = tempHolder.TwitchLinked;
                        if (userToLoad.TwitchLinked)
                        {
                            userToLoad.TwitchName = tempHolder.TwitchName;
                        }
                        userToLoad.Alt_Accounts = tempHolder.Alt_Accounts;

                        In_Progress = false;

                        API_Client_Event?.Invoke(userToLoad, Client_Event_Type.RecentPlayerBnetLoaded);
                    }
                    else
                    {
                        In_Progress = false;
                        API_Client_Event?.Invoke(null, Client_Event_Type.RecentPlayerBnetFailed);
                    }
                }
                catch
                {
                    In_Progress = false;
                    API_Client_Event?.Invoke(null, Client_Event_Type.RecentPlayerBnetFailed);
                }
            }
        }

        #endregion
    }






}
