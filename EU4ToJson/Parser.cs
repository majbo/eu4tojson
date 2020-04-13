using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pdoxcl2Sharp;

namespace EU4ToJson
{
    public class Parser : IParadoxRead
    {
        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "players_countries":
                    PlayerCountries = parser.ReadStringList();
                    break;
                case "countries":
                    var countriesParser = parser.Parse(new CountriesParser());
                    foreach (var country in countriesParser.Countries)
                    {
                        Countries.Add(new JProperty(country.Tag, JObject.FromObject(country)));
                    }

                    break;
                case "diplomacy":
                    var diplomacyParser = parser.Parse(new DiplomacyParser());
                    foreach (var keyValue in diplomacyParser.DiplomaciesByType)
                    {
                        var jArray = new JArray(keyValue.Value.Select(p => JObject.FromObject(p)));
                        Diplomacy.Add(new JProperty(keyValue.Key, jArray));
                    }

                    break;
            }
        }

        [JsonProperty(PropertyName = "players_countries")]
        public IList<string> PlayerCountries { get; private set; }

        [JsonProperty(PropertyName = "countries")]
        public JObject Countries { get; private set; } = new JObject();

        [JsonProperty(PropertyName = "diplomacy")]
        public JObject Diplomacy { get; private set; } = new JObject();
    }


    public class CountriesParser : IParadoxRead
    {
        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (parser.CurrentIndent == 1)
                Countries.Add(parser.Parse(new Country(token)));
        }

        public List<Country> Countries { get; } = new List<Country>();
    }

    public class DiplomacyParser : IParadoxRead
    {
        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (parser.CurrentIndent == 1)
            {
                if (!DiplomaciesByType.ContainsKey(token))
                {
                    DiplomaciesByType.Add(token, new List<Diplomacy>());
                }

                DiplomaciesByType[token].Add(parser.Parse(new Diplomacy()));
            }
        }

        public Dictionary<string, List<Diplomacy>> DiplomaciesByType { get; } =
            new Dictionary<string, List<Diplomacy>>();
    }

    public class Diplomacy : IParadoxRead
    {
        [JsonProperty(PropertyName = "first")] public string First { get; set; }

        [JsonProperty(PropertyName = "second")]
        public string Second { get; set; }

        [JsonProperty(PropertyName = "subject_type")]
        public string SubjectType { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "first":
                    First = parser.ReadString();
                    break;
                case "second":
                    Second = parser.ReadString();
                    break;
                case "subject_type":
                    SubjectType = parser.ReadString();
                    break;
            }
        }
    }

    public class Country : IParadoxRead
    {
        public Country(string tag)
        {
            Tag = tag;
        }

        [JsonIgnore] public string Tag { get; }

        [JsonProperty(PropertyName = "development")]
        public float Development { get; private set; }

        [JsonProperty(PropertyName = "raw_development")]
        public float RawDevelopment { get; private set; }

        [JsonProperty(PropertyName = "capped_development")]
        public float CappedDevelopment { get; private set; }

        [JsonProperty(PropertyName = "realm_development")]
        public float RealmDevelopment { get; private set; }

        [JsonProperty(PropertyName = "colors")]
        public Colors Colors { get; private set; }

        [JsonProperty(PropertyName = "institutions")]
        public IList<int> Institutions { get; private set; }

        [JsonProperty(PropertyName = "estimated_monthly_income")]
        public float EstimatedMonthlyIncome { get; private set; }

        [JsonProperty(PropertyName = "technology")]
        public Technology Technology { get; private set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "development":
                    Development = parser.ReadFloat();
                    break;
                case "raw_development":
                    RawDevelopment = parser.ReadFloat();
                    break;
                case "capped_development":
                    CappedDevelopment = parser.ReadFloat();
                    break;
                case "realm_development":
                    RealmDevelopment = parser.ReadFloat();
                    break;
                case "institutions":
                    Institutions = parser.ReadIntList();
                    break;
                case "colors":
                    Colors = parser.Parse(new Colors());
                    break;
                case "estimated_monthly_income":
                    EstimatedMonthlyIncome = parser.ReadFloat();
                    break;
                case "technology":
                    Technology = parser.Parse(new Technology());
                    break;
            }
        }
    }

    public class Technology : IParadoxRead
    {
        [JsonProperty(PropertyName = "adm_tech")]
        public int AdmTech { get; set; }

        [JsonProperty(PropertyName = "dip_tech")]
        public int DipTech { get; set; }

        [JsonProperty(PropertyName = "mil_tech")]
        public int MilTech { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "adm_tech":
                    AdmTech = parser.ReadInt32();
                    break;
                case "dip_tech":
                    DipTech = parser.ReadInt32();
                    break;
                case "mil_tech":
                    MilTech = parser.ReadInt32();
                    break;
            }
        }
    }

    public class Colors : IParadoxRead
    {
        [JsonProperty(PropertyName = "revolutionary_colors")]
        public IList<int> RevolutionaryColors { get; set; }

        [JsonProperty(PropertyName = "map_color")]
        public IList<int> MapColor { get; set; }

        [JsonProperty(PropertyName = "country_color")]
        public IList<int> CountryColor { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "revolutionary_colors":
                    RevolutionaryColors = parser.ReadIntList();
                    break;
                case "map_color":
                    MapColor = parser.ReadIntList();
                    break;
                case "country_color":
                    CountryColor = parser.ReadIntList();
                    break;
            }
        }
    }
}