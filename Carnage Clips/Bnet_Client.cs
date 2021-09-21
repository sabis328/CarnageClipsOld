using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft;

namespace Carnage_Clips
{
    public class Bnet_Client
    {
       
        

        public bool CancelAll { get; set; }
        public string API_key { get; set; }

        private string _api_base_path = "https://www.bungie.net/Platform/";
        private string PlayerExclusion { get; set; }

        private bool inUse = false;
        private bool SearchComplete = false;

        public event EventHandler<BNet_Client_Event_Type> Client_Event;
        public enum BNet_Client_Event_Type
        {

            Search_Complete,
            Search_Fail,
            Search_No_Results,
            Details_Loaded,
            Details_Failed,
            Characters_Loaded,
            Characters_Failed,
            Carnage_List_Loaded,
            Carnage_List_Failed,
            Single_Carnage_Loaded,
            Single_Carnage_Fail

        }
        public void Search_User_Accounts(string inputName)
        {
            if (!inUse && !CancelAll)
            {
                System.Diagnostics.Debug.Print("searching user ; " + inputName);
                SearchComplete = false;
                inUse = true;
                string CodeMatch = "";
                bool matchCode = false;

                if (inputName.Contains("#"))
                {
                    string[] prefixArray = inputName.Split('#');
                    inputName = prefixArray[0];
                    CodeMatch = prefixArray[1];
                    matchCode = true;
                }

                List<BNet_Profile> SearchResults = new List<BNet_Profile>();

                int i = 0;
                while (!SearchComplete && !CancelAll)
                {
                    List<BNet_Profile> AddResults = Search_Accounts(inputName, i);

                    if (AddResults != null)
                    {
                        foreach (BNet_Profile FoundUser in AddResults)
                        {
                            SearchResults.Add(FoundUser);
                        }
                    }
                    else
                    {
                        SearchComplete = true;
                    }

                    i++;

                }

                inUse = false;
                if (SearchResults.Count > 0)
                {
                    if (matchCode)
                    {
                        List<BNet_Profile> resetResults = new List<BNet_Profile>();
                        foreach (BNet_Profile check in SearchResults)
                        {
                            if (check.BungieCode == CodeMatch)
                            {
                                resetResults.Add(check);
                                break;
                            }
                        }
                        if (resetResults.Count > 0)
                        {
                            //Return Single Result
                            Client_Event?.Invoke(resetResults, BNet_Client_Event_Type.Search_Complete);
                        }
                        else
                        {
                            //No results found matching that code
                            Client_Event?.Invoke(null, BNet_Client_Event_Type.Search_No_Results);
                        }
                    }
                    else
                    {
                        //Return Full list of results
                        Client_Event?.Invoke(SearchResults, BNet_Client_Event_Type.Search_Complete);
                    }
                }
                else
                {
                    //No matches found;
                    Client_Event?.Invoke(null, BNet_Client_Event_Type.Search_No_Results);
                }
            }
        }

        private List<BNet_Profile> Search_Accounts(string inputName, int PageNumber)
        {
            ServicePointManager.DefaultConnectionLimit = 15;
            if (!CancelAll)
            {
                try
                {

                    List<BNet_Profile> AccountsFound = new List<BNet_Profile>();

                    string requestLink = string.Format(_api_base_path + "{0}{1}{2}", "User/Search/Prefix/", Uri.EscapeDataString(inputName), "/" + PageNumber.ToString());
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", API_key);
                    Search_Client.KeepAlive = true;

                    string responseText;
                    
                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    if (responseText.Contains("destinyMemberships"))
                    {
                        dynamic resultsJson = JsonConvert.DeserializeObject<dynamic>(responseText);
                        string searchResultsJson = resultsJson["Response"]["searchResults"].ToString();

                        
                        dynamic serializedAccounts = JsonConvert.DeserializeObject<dynamic>(searchResultsJson);

                        foreach(var BNetData in serializedAccounts)
                        {
                            BNet_Profile BnetProf = Process_Search_Result_Profile(BNetData.ToString());
                            
                            AccountsFound.Add(BnetProf);
                        }


                        string more = resultsJson["Response"]["hasMore"].ToString();
                        
                        if (more == "False")
                        {
                            SearchComplete = true;
                            
                        }

                        return AccountsFound;
                    }
                    else
                    {
                        SearchComplete = true;
                        return null; 
                    }
                }
                catch
                {
                    SearchComplete = true;
                    return null; 
                }
            }
            else
            {
                return null;
            }
        }

        private BNet_Profile Process_Search_Result_Profile(string inputJson)
        {
           
            dynamic AccountData = JsonConvert.DeserializeObject<dynamic>(inputJson);
            BNet_Profile BungieProfile = new BNet_Profile();
            BungieProfile.BungieName = AccountData.bungieGlobalDisplayName;
            BungieProfile.BungieCode = AccountData.bungieGlobalDisplayNameCode;
            BungieProfile.BungieNetId = AccountData.bungieMembershipId;
            BungieProfile.DestinyMemberships = new List<Destiny_Membership>();

            string membershipsMeta = AccountData["destinyMemberships"].ToString();
            dynamic mebershipsSerialized = JsonConvert.DeserializeObject<dynamic>(membershipsMeta);
            foreach(var D2Account in mebershipsSerialized)
            {
                Destiny_Membership D2Acc = new Destiny_Membership();
                D2Acc.crossSaveOverride = D2Account.crossSaveOverride;
                D2Acc.MembershipId = D2Account.membershipId;
                D2Acc.platformDisplayName = D2Account.displayName;
                D2Acc.platformCode = D2Account.membershipType;
                BungieProfile.CrossSaveCode = D2Acc.crossSaveOverride;

                BungieProfile.DestinyMemberships.Add(D2Acc);
            }


            if (BungieProfile.CrossSaveCode != "0")
            {
                foreach(Destiny_Membership d2account in BungieProfile.DestinyMemberships)
                {
                    if(d2account.platformCode == BungieProfile.CrossSaveCode)
                    {
                        BungieProfile.SelectedMemebership = d2account;
                        break;
                    }
                }
            }
            else
            {
                BungieProfile.SelectedMemebership = BungieProfile.DestinyMemberships[0];
            }

            return BungieProfile;

        }

        public void Load_Detailed_Bnet(BNet_Profile inputAccount,bool LoadCharacters = false)
        {
            ServicePointManager.DefaultConnectionLimit = 15;
            try
            {
               
                string requestLink = string.Format("https://www.bungie.net/Platform/User/GetMembershipsById/" + "{0}{1}", inputAccount.SelectedMemebership.MembershipId, "/-1/");
                HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                Search_Client.Method = "GET";
                Search_Client.Headers.Add("X-API-KEY", API_key);
               // Search_Client.KeepAlive = true;

                string responseText;

                using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                {
                    responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                    Search_Response.Close();
                }
                if (responseText.Contains("bungieNetUser"))
                {
                    dynamic profileJson = JsonConvert.DeserializeObject<dynamic>(responseText);
                    
                    string BNetMeta = profileJson["Response"]["bungieNetUser"].ToString();
                    dynamic BnetSerialized = JsonConvert.DeserializeObject<dynamic>(BNetMeta);

                    inputAccount.BungieNetId = BnetSerialized.membershipId;
                    if(BNetMeta.Contains("twitchDisplayName"))
                    {
                        inputAccount.Has_Twitch = true;
                        inputAccount.TwitchName = BnetSerialized.twitchDisplayName;
                    }

                    Client_Event?.Invoke(inputAccount, BNet_Client_Event_Type.Details_Loaded);

                }

                else
                {
                    Client_Event?.Invoke(inputAccount, BNet_Client_Event_Type.Details_Failed);
                }
                
            }
            catch
            {
                Client_Event?.Invoke(inputAccount, BNet_Client_Event_Type.Details_Failed);
            }
        }

        private List<Destiny_Character> Set_Platform_Characters(string inputMeta)
        {
            dynamic characterMetaJson = JsonConvert.DeserializeObject<dynamic>(inputMeta);
            string characterMeta = characterMetaJson["Response"]["characters"]["data"].ToString();

            var parsed = JsonConvert.DeserializeObject<Dictionary<string,Destiny_Character>>(characterMeta);
            List<Destiny_Character> ProcessedCharacters = new List<Destiny_Character>();
            foreach(var Entry in parsed)
            {
                Entry.Value.DownloadEmblem();
                
                ProcessedCharacters.Add((Destiny_Character)Entry.Value);
            }
            if (ProcessedCharacters.Count > 0)
            {
                return ProcessedCharacters;
            }
            else
            {
                return null;
            }
        }
        public void Load_Character_Entries(BNet_Profile inputAccount)
        {
            ServicePointManager.DefaultConnectionLimit = 15;
            if (inputAccount.CrossSaveCode == "0")
            {
                foreach (Destiny_Membership PlatformAccount in inputAccount.DestinyMemberships)
                {
                    try
                    {
                       
                        string requestLink = string.Format(_api_base_path + "{0}{1}{2}", "Destiny2/", PlatformAccount.platformCode, "/Profile/" + PlatformAccount.MembershipId + "/?components=200");
                        HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                        Search_Client.Method = "GET";
                        Search_Client.Headers.Add("X-API-KEY", API_key);
                        Search_Client.KeepAlive = true;

                        string responseText;

                        using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                        {
                            responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                            Search_Response.Close();
                        }

                        PlatformAccount.PlatformCharacters = Set_Platform_Characters(responseText);
                        PlatformAccount.ValidCharacters = true;
                    }
                    catch
                    {
                        PlatformAccount.ValidCharacters = false;
                    }
                }
            }
            else
            {
                try
                {
                    string requestLink = string.Format(_api_base_path + "{0}{1}{2}", "Destiny2/", inputAccount.SelectedMemebership.platformCode, "/Profile/" + inputAccount.SelectedMemebership.MembershipId + "/?components=200");
                    HttpWebRequest Search_Client = (HttpWebRequest)WebRequest.Create(requestLink);
                    Search_Client.Method = "GET";
                    Search_Client.Headers.Add("X-API-KEY", API_key);
                    Search_Client.KeepAlive = true;

                    string responseText;

                    using (HttpWebResponse Search_Response = (HttpWebResponse)Search_Client.GetResponse())
                    {
                        responseText = new StreamReader(Search_Response.GetResponseStream()).ReadToEnd();
                        Search_Response.Close();
                    }

                    inputAccount.SelectedMemebership.PlatformCharacters = Set_Platform_Characters(responseText);
                    inputAccount.SelectedMemebership.ValidCharacters = true;

                    foreach (Destiny_Membership check in inputAccount.DestinyMemberships)
                    {
                        if (check.MembershipId == inputAccount.SelectedMemebership.MembershipId)
                        {
                            check.PlatformCharacters = inputAccount.SelectedMemebership.PlatformCharacters;
                            check.ValidCharacters = true;
                            break;
                        }

                    }
                }
                catch
                {
                    inputAccount.SelectedMemebership.ValidCharacters = false;
                    
                }
            }

            Client_Event?.Invoke(inputAccount, BNet_Client_Event_Type.Characters_Loaded);
           
        }

        public BNet_Profile LoadedProfile;
        public Destiny_Character LoadedCharacter;
        public int ReportsToLoad { get; set; }
        public int ReportsLoaded { get; set; }

        public List<CarnageReport> RecentMatches { get; set; }
        
        public List<string> RecentPlayerCompare { get; set; }


        /// <summary>
        /// count = 200 max,
        /// mode filter = 1-84
        /// page count modifier for requests over 200
        /// </summary>
        /// <param name="inputCharacter"></param>
        public void Load_Carnage_Report_List(Destiny_Character inputCharacter, int Count = 5, string modeSpecifier = "0")
        {
            if(!inUse && !CancelAll)
            {
                if(LoadedProfile != null)
                {
                    int PagesNeeded = 1;
                    if (Count > 200)
                    {
                        double roundedPages = Math.Abs((double)Count/ (double)200);
                        roundedPages = Math.Ceiling(roundedPages);
                        PagesNeeded = (int)roundedPages;

                    }

                    int currentPage = 0;
                    
                    HttpWebRequest reportClient;
                    ServicePointManager.DefaultConnectionLimit = 15;
                    
                    RecentMatches = new List<CarnageReport>();

                    int TotalReportsFound = 0;
                    int RowCount = 5;
                    if(Count > 200)
                    {
                        RowCount = 200;
                    }
                    else
                    {
                        RowCount = Count;
                    }

                    while (currentPage <= PagesNeeded)
                    {
                        
                        string requestURL = string.Format("https://www.bungie.net/Platform/Destiny2/{0}/Account/{1}/Character/{2}/Stats/Activities/?mode={3}&count={4}&page={5}",
                            LoadedProfile.SelectedMemebership.platformCode, LoadedProfile.SelectedMemebership.MembershipId, inputCharacter.characterId, modeSpecifier, RowCount, currentPage);
                        
                        reportClient = (HttpWebRequest)WebRequest.Create(requestURL);
                        reportClient.Method = "GET";
                        reportClient.Headers.Add("X-API-KEY", API_key);
                        reportClient.KeepAlive = false;

                        string responseBody = "";
                        try
                        {
                            using (HttpWebResponse _response = (HttpWebResponse)reportClient.GetResponse())
                            {
                                responseBody = new StreamReader(_response.GetResponseStream()).ReadToEnd();
                                _response.Close();
                            }
                            if (responseBody.Contains("activities"))
                            {
                                //System.Diagnostics.Debug.Print(responseBody);
                                dynamic ReportListJson = JsonConvert.DeserializeObject<dynamic>(responseBody);
                                string reportList = ReportListJson["Response"]["activities"].ToString();
                                dynamic reportsSerialized = JsonConvert.DeserializeObject<dynamic>(reportList);


                                TotalReportsFound += reportsSerialized.Count;
                                System.Diagnostics.Debug.Print("Found " + TotalReportsFound.ToString() + " reports matching filter settings on page " + currentPage.ToString());

                                foreach (var postGame in reportsSerialized)
                                {

                                    CarnageReport _CarnageReport = new CarnageReport();
                                    _CarnageReport.matchDateString = postGame["period"];
                                    _CarnageReport.directorActivityHash = postGame["activityDetails"]["directorActivityHash"];
                                    _CarnageReport.referenceId = postGame["activityDetails"]["referenceId"];
                                    _CarnageReport.instanceId = postGame["activityDetails"]["instanceId"];

                                    RecentMatches.Add(_CarnageReport);

                                    if(RecentMatches.Count == Count)
                                    {
                                        currentPage = PagesNeeded + 1;
                                        break;
                                    }
                                }    
                            }
                            else
                            {
                                currentPage++;
                                break;
                            }
                        }
                        catch
                        {
                            currentPage++;
                            break;
                        }
                        currentPage++;
                    }

                    if (RecentMatches.Count > 0)
                    {
                        System.Diagnostics.Debug.Print(RecentMatches.Count + " reports were found matching your filter search");
                        System.Diagnostics.Debug.Print("End of search");

                        Client_Event?.Invoke(RecentMatches, BNet_Client_Event_Type.Carnage_List_Loaded);

                        ReportsToLoad = RecentMatches.Count;
                        ReportsLoaded = 0;
                        foreach(CarnageReport pgcr in RecentMatches)
                        {
                            Task.Run(() => Load_Single_Carnage_Report(pgcr));
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("Error loading carnage report list");
                        Client_Event?.Invoke(RecentMatches, BNet_Client_Event_Type.Carnage_List_Failed);
                    }

                }
            }
        }

        public void Load_Single_Carnage_Report(CarnageReport match)
        {
            ServicePointManager.DefaultConnectionLimit = 15;
            HttpWebRequest _client;

            if(CancelAll)
            {

                return;
            }

            try
            {
                match.MatchPlayers = new List<BNet_Profile>();
                _client = (HttpWebRequest)WebRequest.Create("https://stats.bungie.net/Platform/Destiny2/Stats/PostGameCarnageReport/" + match.instanceId + "/");
                _client.Method = "GET";
                _client.Headers.Add("X-API-KEY", "9efe9b8eba3042afb081121d447fd981");

                string matchData;
                using (HttpWebResponse _subresponse = (HttpWebResponse)_client.GetResponse())
                {
                    matchData = new StreamReader(_subresponse.GetResponseStream()).ReadToEnd();
                    _subresponse.Close();
                }

                //System.Diagnostics.Debug.Print(matchData);

                dynamic postGameJson = JsonConvert.DeserializeObject<dynamic>(matchData);
                dynamic GamePlayerList = JsonConvert.DeserializeObject<dynamic>(postGameJson["Response"]["entries"].ToString());

                foreach(var RecentPlayerData in GamePlayerList)
                {
                    BNet_Profile Recentplayer = ParseRecentPlayer(RecentPlayerData["player"].ToString());
                    match.MatchPlayers.Add(Recentplayer);
                }
                ReportsLoaded++;
                Client_Event?.Invoke(match, BNet_Client_Event_Type.Single_Carnage_Loaded);

            }
            catch
            {
                ReportsLoaded++;
                Client_Event?.Invoke(match, BNet_Client_Event_Type.Single_Carnage_Fail);
            }


        }


        private BNet_Profile ParseRecentPlayer(string inputMeta)
        {
            dynamic PlayerJson = JsonConvert.DeserializeObject<dynamic>(inputMeta);
            BNet_Profile holder = new BNet_Profile();
            holder.BungieName = PlayerJson["destinyUserInfo"]["bungieGlobalDisplayName"];
            holder.BungieCode = PlayerJson["destinyUserInfo"]["bungieGlobalDisplayNameCode"];
            holder.RecentPlayerMemberID = PlayerJson["destinyUserInfo"]["membershipId"];
            holder.LinkedMatches = new List<CarnageReport>();

            Destiny_Membership tempMember = new Destiny_Membership();
            tempMember.MembershipId = holder.RecentPlayerMemberID;

            holder.SelectedMemebership = tempMember;
            //System.Diagnostics.Debug.Print(holder.GlobalName());
            //System.Diagnostics.Debug.Print(holder.RecentPlayerMemberID);
            
            return holder;

        }
    }

   

    public class BNet_Profile
    {

        public string GlobalName()
        {
            return BungieName + "#" + BungieCode;
        }
        public string BungieName { get; set; }
        public string BungieCode { get; set; }
        public string BungieNetId { get; set; }

        public string TwitchName { get; set; }
        public bool Has_Twitch { get; set; }
        
        public string CrossSaveCode { get; set; }
        public List<Destiny_Membership> DestinyMemberships { get; set; }
        public Destiny_Membership SelectedMemebership { get; set; }

        public string RecentPlayerMemberID { get; set; }

        public List<CarnageReport> LinkedMatches { get; set; }
        
        
    }
    public class Destiny_Membership
    {
        public string MembershipId { get; set; }
        public string platformDisplayName { get; set; }
        public string platformCode { get; set; }
        public string crossSaveOverride { get; set; }

        public bool ValidCharacters { get; set; }
        public List<Destiny_Character> PlatformCharacters { get; set; }
        public Destiny_Character SelectedCharacter { get; set; }


        public string PlatformType()
        {
            switch (platformCode)
            {
                case "1":
                    return "Xbox";
                case "2":
                    return "Playstation";
                case "3":
                    return "Steam";
                default:
                    return "Unknown";
            }
        }
    }
    public class Destiny_Character
    {
        public string membershipId { get; set; }
        public string membershipType { get; set; }
        public string characterId { get; set; }
        public string light { get; set; }
        public string raceHash { get; set; }
        public string genderHash { get; set; }
        public string classHash { get; set; }

        public string emblemBackgroundPath { get; set; }

        public System.Drawing.Image Emblem { get; set; }
        public void DownloadEmblem()
        {
            System.Diagnostics.Debug.Print("Downloading emeblem : " + emblemBackgroundPath);
            HttpWebRequest emblemReq = (HttpWebRequest)WebRequest.Create("https://www.bungie.net" + emblemBackgroundPath);
            emblemReq.AllowWriteStreamBuffering = true;
            emblemReq.Timeout = 5000;

            using (HttpWebResponse emblemResponse = (HttpWebResponse)emblemReq.GetResponse())
            {
                Stream emblemStream = emblemResponse.GetResponseStream();
                Emblem = System.Drawing.Image.FromStream(emblemStream);
                emblemResponse.Close();
            }
        }
    }

    
    public class CarnageReport
    {
       public List<BNet_Profile> MatchPlayers { get; set; }
       
        //Activity type
       public string referenceId { get; set; }
       //Activity location
       public string directorActivityHash { get; set; }
       public string instanceId { get; set; }
       //The type of game see sticky note
       public string mode { get; set; }
       public string matchDateString { get; set; }
       

        public DateTime returnMatchDate()
        {
            return DateTime.Parse(matchDateString);
        }
    }
}
