using RestSharp;
using TwitterClient;
using Typed;

public class Program {
    const string HADITH_URI = "https://cdn.jsdelivr.net/gh/fawazahmed0/hadith-api@1/";
    static readonly string[] EDITIONS = new string[] {"bukhari", "muslim"};
    static readonly Dictionary<string,int> EditionHadithCount = new()
    {
        {"bukhari", 7563},
        {"muslim", 7563}
    };

    static List<int> rejectedHadiths = new();

    //Twitter stuff
    const string CONS_KEY = "OkMgs3ulVPamrbrCtP2zqCehm";
    const string CONS_SEC = "ExuCWmG3kD7WomaEWPNSAciDyCoOiWw1TLmnHSKNtXfwupcCGG";
    const string ACCESS_TOK = "1742231578737016833-ab9gnXV8AkNZa5ecdmWfctPzevqOnt";
    const string ACCESS_SEC = "v8vzGgotQ1l8CszlsdF4iA8mpIqdsPBxa75HcuCO2Ejd2";

    static async Task Main(string[] args)
    {   
        HadithRoot found = await PickHadith();
        SendTweet(ConstructTweet(found));
    }

    static async Task<HadithRoot> PickHadith()
    {
        Random rnd = new();
        var client = new RestClient(HADITH_URI); //client to initiate requests

        string edition = EDITIONS[rnd.Next(EDITIONS.Length)];
        int lastHadith = EditionHadithCount[edition];

        while (true) {
            int rndHadNum = rnd.Next(lastHadith + 1); //Pick a random hadith number from 1-last


            try {
                var response = await client.GetJsonAsync<HadithRoot>(
                $"editions/ara-{edition}1/{rndHadNum}.json");
                if (response is null) continue;

                var hadith = response.Hadiths?[0];

                if (!IsValidHadith(hadith)) continue;
                return response;
            } catch {
                continue;
            }
        }
    }

    static bool IsValidHadith(Hadith? hadith) {
        if (hadith is null || hadith.Text is null) return false; // null?
        if (rejectedHadiths.Contains(hadith.Hadithnumber)) return false; //Rejected list
        if (hadith.Grades?.Count > 0 
            && hadith.Grades?.Find(x => x.Value.Contains("Sahih")) is null) return reject(); //Not sahih
        string shortHadith = ShortenArabicHadtih(hadith.Text); //shortens it
        if (shortHadith.Length > 230) return reject(); //Too long for twitter

        return true;

        bool reject() {
            rejectedHadiths.Add(hadith.Hadithnumber);
            return false;
        }
    }


    public static string ShortenArabicHadtih(string text) 
    {
        int idx = text.IndexOf("صلى الله عليه"); //Where we need to stop shortening
        string shrt = text.Substring(0, idx);
        int lastAnIdx = shrt.LastIndexOf("عن "); // take the last An
        // take also another in case the last is prophets pbuh's
        int slastAnIdx = shrt.Substring(0, lastAnIdx - 1).LastIndexOf("عن "); 
        if (shrt.Substring(lastAnIdx, 8).Equals("عن النبي")  //Switch em
            || shrt.Substring(lastAnIdx, 9).Equals("عن الرسول"))
            lastAnIdx = slastAnIdx;

        return text.Substring(lastAnIdx, text.Length - lastAnIdx); //Voila
    }

    static string ConstructTweet(HadithRoot hadithRoot)
    {
        if (hadithRoot.Hadiths is null) return "";

        Hadith had = hadithRoot.Hadiths[0];
        string shawty = ShortenArabicHadtih(had.Text);
        string rawi = hadithRoot?.Metadata?.Name ?? "";
        rawi =    rawi.Equals("Sahih al Bukhari") ? "البخاري" 
                : rawi.Equals("Sahih Muslim") ? "مسلم" : ""; 
        shawty += "\nرواه " + rawi;
        return shawty;
    }


    static void SendTweet(string text) {
        var userClient = new API(ACCESS_TOK, ACCESS_SEC, CONS_KEY, CONS_SEC);
        userClient.Post("/2/tweets", new Parameters("text", text));
    }
}



       

