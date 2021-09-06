using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carnage_Clips
{
    public class Carnage_Report
    {
        public string ActivityRefID { get; set; }
        public string ActivitySpaceID { get; set; }
        public string ActivityTypeID { get; set; }
        public void SetGameType(string hash)
        {
            ActivityHash = hash;
            switch (hash)
            {
                case "1738383283":
                    ActivityTypeID = "Harbinger";
                    break;
                case "5517242":
                    ActivityTypeID = "Empire Hunt";
                    break;
                case "3881495763":
                    ActivityTypeID = "Raid";
                    break;
                case "70223475":
                    ActivityTypeID = "Patrol";
                    break;
                case "2936791966":
                    ActivityTypeID = "Lost Sector";
                    break;
                case "1928961926":
                    ActivityTypeID = "Patrol";
                    break;
                case "1717505396":
                    ActivityTypeID = "Control";
                    break;
                case "2865450620":
                    ActivityTypeID = "Survival";
                    break;
                case "588019350":
                    ActivityTypeID = "Trials";
                    break;
                case "25688104":
                    ActivityTypeID = "Override";
                    break;
                case "2865532048":
                    ActivityTypeID = "Override";
                    break;
                case "1683791010":
                    ActivityTypeID = "Iron Banner";
                    break;
                case "3076038389":
                    ActivityTypeID = "Private Match";
                    break;
                case "1813752023":
                    ActivityTypeID = "Patrol";
                    break;
                case "1070981430":
                    ActivityTypeID = "Lost Sector";
                    break;
                case "3029388710":
                    ActivityTypeID = "Nightfall:Legend";
                    break;
                case "2205920677":
                    ActivityTypeID = "Empire Hunt";
                    break;
                case "135431604":
                    ActivityTypeID = "Gambit";
                    break;
                case "1077850348":
                    ActivityTypeID = "Dungeon";
                    break;
                case "743628305":
                    ActivityTypeID = "Vangaurd Strikes";
                    break;
                case "3029388705":
                    ActivityTypeID = "Nightfall:Master";
                    break;
                case "1655431815":
                    ActivityTypeID = "Expunge";
                    break;
                case "2019961998":
                    ActivityTypeID = "Lost Sector";
                    break;
                case "2122313384":
                    ActivityTypeID = "Raid";
                    break;
                case "3933916447":
                    ActivityTypeID = "Override";
                    break;
                case "3029388711":
                    ActivityTypeID = "Nightfall:Hero";
                    break;
                case "2936791996":
                    ActivityTypeID = "Lost Sector";
                    break;
                case "3293630130":
                    ActivityTypeID = "Nightfall:Legend";
                    break;
                case "506197732":
                    ActivityTypeID = "Patrol";
                    break;
                case "2829206727":
                    ActivityTypeID = "Lost Sector";
                    break;
                case "3293630131":
                    ActivityTypeID = "Nightfall:Hero";
                    break;
                case "2936791995":
                    ActivityTypeID = "Lost Sector";
                    break;
                default:
                    ActivityTypeID = "Unkown Game Mode";
                    break;
            }
        }
        public void SetLocation(string hash)
        {

            LocationHash = hash;
            switch (hash)
            {
                case "2936791995":
                    ActivitySpaceID = "Exodus Garden 2A:Master";
                    break;
                case "3293630131":
                    ActivitySpaceID = "Cosmodrome";
                    break;
                case "2829206727":
                    ActivitySpaceID = "K1 Communication:Legend";
                    break;
                case "506197732":
                    ActivitySpaceID = "Moon";
                    break;
                case "3293630130":
                    ActivitySpaceID = "Cosmodrome";
                    break;
                case "2936791996":
                    ActivitySpaceID = "Exodus Garden 2A:Legend";
                    break;
                case "3029388711":
                    ActivitySpaceID = "Nessus";
                    break;
                case "3933916447":
                    ActivitySpaceID = "Tangled Shore";
                    break;
                case "2122313384":
                    ActivitySpaceID = "The Last Wish";
                    break;
                case "2019961998":
                    ActivitySpaceID = "The Empty Tank:Legend";
                    break;
                case "2585182686":
                    ActivitySpaceID = "Fortess";
                    break;
                case "236451195":
                    ActivitySpaceID = "Distant shore";
                    break;
                case "3445454561":
                    ActivitySpaceID = "The Anomaly";
                    break;
                case "1738383283":
                    ActivitySpaceID = "EDZ";
                    break;
                case "5517242":
                    ActivitySpaceID = "Empire Hunt";
                    break;
                case "3881495763":
                    ActivitySpaceID = "Vault of Glass";
                    break;
                case "70223475":
                    ActivitySpaceID = "Tangled Shore";
                    break;
                case "2903548701":
                    ActivitySpaceID = "Radiant Cliffs";
                    break;
                case "4242091248":
                    ActivitySpaceID = "Twilight Gap";
                    break;
                case "1486201898":
                    ActivitySpaceID = "Convergence";
                    break;
                case "3284567441":
                    ActivitySpaceID = "Exodus Blue";
                    break;
                case "1448047125":
                    ActivitySpaceID = "Widows Court";
                    break;
                case "2260508373":
                    ActivitySpaceID = "Bannerfall";
                    break;
                case "1326470716":
                    ActivitySpaceID = "Endless Vale";
                    break;
                case "2905719116":
                    ActivitySpaceID = "Fragment";
                    break;
                case "2865532048":
                    ActivitySpaceID = "Override:Moon";
                    break;
                case "1429033007":
                    ActivitySpaceID = "Bannerfall";
                    break;
                case "25688104":
                    ActivitySpaceID = "Override:Europa";
                    break;
                case "2936791966":
                    ActivitySpaceID = "Exodus Garden 2A:Legend";
                    break;
                case "1928961926":
                    ActivitySpaceID = "Cosmodrome";
                    break;
                case "279800038":
                    ActivitySpaceID = "Midtown";
                    break;
                case "2081877020":
                    ActivitySpaceID = "Fortress";
                    break;
                case "1375350354":
                    ActivitySpaceID = "Twilight Gap";
                    break;
                case "780203647":
                    ActivitySpaceID = "Radiant Cliffs";
                    break;
                case "2133997042":
                    ActivitySpaceID = "Rusted Lands";
                    break;
                case "2361232192":
                    ActivitySpaceID = "Cauldron";
                    break;
                case "528628085":
                    ActivitySpaceID = "Distant Shore";
                    break;
                case "1009746517":
                    ActivitySpaceID = "Wormhaven";
                    break;
                case "2296512046":
                    ActivitySpaceID = "Endless Vale";
                    break;
                case "1960008274":
                    ActivitySpaceID = "Endless Vale";
                    break;
                case "2851089328":
                    ActivitySpaceID = "Pacifica";
                    break;
                case "1141558876":
                    ActivitySpaceID = "Altar of Flame";
                    break;
                case "2238426332":
                    ActivitySpaceID = "Burnout";
                    break;
                case "2755115715":
                    ActivitySpaceID = "Exodus Blue";
                    break;
                case "503710612":
                    ActivitySpaceID = "Convergence";
                    break;
                case "427041827":
                    ActivitySpaceID = "Widows Court";
                    break;
                case "1008195646":
                    ActivitySpaceID = "Fragment";
                    break;
                case "788769683":
                    ActivitySpaceID = "Jav 4";
                    break;
                case "960911914":
                    ActivitySpaceID = "Jav 4";
                    break;
                case "2585183686":
                    ActivitySpaceID = "Fortress";
                    break;
                case "1813752023":
                    ActivitySpaceID = "Europa";
                    break;
                case "1070981430":
                    ActivitySpaceID = "Perdition:Legend";
                    break;
                case "2205920677":
                    ActivitySpaceID = "The Dark Priestess:Master";
                    break;
                case "3029388710":
                    ActivitySpaceID = "Nessus";
                    break;
                case "2505336188":
                    ActivitySpaceID = "The Dead Cliffs";
                    break;
                case "1855216675":
                    ActivitySpaceID = "New Arcadia";
                    break;
                case "1575864965":
                    ActivitySpaceID = "Deep Six";
                    break;
                case "1731870079":
                    ActivitySpaceID = "Legions Folly";
                    break;
                case "1077850348":
                    ActivitySpaceID = "Prophecy";
                    break;
                case "3240321863":
                    ActivitySpaceID = "Arms Dealer";
                    break;
                case "1684420962":
                    ActivitySpaceID = "The Disgraced";
                    break;
                case "3643233460":
                    ActivitySpaceID = "The Scarlet Keep";
                    break;
                case "3029388705":
                    ActivitySpaceID = "Nessus";
                    break;
                case "114977383":
                    ActivitySpaceID = "Exodus Blue";
                    break;
                case "1655431815":
                    ActivitySpaceID = "Tangled Shore";
                    break;
                default:
                    ActivitySpaceID = "Unkown Location";
                    break;
            }
        }

        public string LocationHash { get; set; }
        public string ActivityHash { get; set; }
        public DateTime ActivityStart { get; set; }
        public List<Bungie_Profile> ActivityPlayers { get; set; }
        public enum ActivtyType
        {
            Strike,
            Control,
            Clash,
            Survival,
            Solo_Survival,
            Trials
        }

        public ActivtyType GameType { get; set; }
    }
}
