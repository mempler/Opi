using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using opi.shared;

namespace opi.v1
{
    // This api is not as complex as v2 so we only need 1 file.
    public class Api
    {
        private const string Apiv1 = "https://osu.ppy.sh/api";
        private string ApiKey { get; }

        public Api(string apiKey) => ApiKey = apiKey;
	    
        public Beatmap GetBeatmap(int beatmapId = -1, PlayMode mode = PlayMode.All, bool convertedMaps = false, string beatmapMd5 = "", int limit = 500) => GetBeatmapAsync(beatmapId, mode, convertedMaps, beatmapMd5, limit).GetAwaiter().GetResult();
        public async Task<Beatmap> GetBeatmapAsync(int beatmapId = -1, PlayMode mode = PlayMode.All, bool convertedMaps = false, string beatmapMd5 = "", int limit = 500)
        {
            string queryString = $"?k={ApiKey}";
            
            if (beatmapId >= 0)
                queryString += $"&b={beatmapId}";
            
            switch (mode)
            {
                case PlayMode.Osu:
                case PlayMode.Taiko:
                case PlayMode.Ctb:
                case PlayMode.Mania:
                    queryString += $"&m={(int) mode}";
                    break;
            }

            queryString += $"&a={Convert.ToInt32(convertedMaps)}";
            if (beatmapMd5.Length > 0)
                queryString += $"&h={beatmapMd5}";

            queryString += $"&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_beatmaps{queryString}"));
            });

	        Beatmap[] rResult = JsonConvert.DeserializeObject<Beatmap[]>(result);
	        return rResult.Length <= 0 ? null : rResult[0];
        }
	    
	    public Beatmap[] GetBeatmapSet(int beatmapSetId = -1, PlayMode mode = PlayMode.All, bool convertedMaps = false, string beatmapMd5 = "", int limit = 500) => GetBeatmapSetAsync(beatmapSetId, mode, convertedMaps, beatmapMd5, limit).GetAwaiter().GetResult();
	    public async Task<Beatmap[]> GetBeatmapSetAsync(int beatmapSetId = -1, PlayMode mode = PlayMode.All, bool convertedMaps = false, string beatmapMd5 = "", int limit = 500)
	    {
		    string queryString = $"?k={ApiKey}";
            
		    if (beatmapSetId >= 0)
			    queryString += $"&s={beatmapSetId}";
            
		    switch (mode)
		    {
			    case PlayMode.Osu:
			    case PlayMode.Taiko:
			    case PlayMode.Ctb:
			    case PlayMode.Mania:
				    queryString += $"&m={(int) mode}";
				    break;
		    }

		    queryString += $"&a={Convert.ToInt32(convertedMaps)}";
		    if (beatmapMd5.Length > 0)
			    queryString += $"&h={beatmapMd5}";

		    queryString += $"&limit={limit}";

		    string result = await Task.Run(() =>
		    {
			    using (WebClient client = new WebClient())
				    return client.DownloadString(new Uri($"{Apiv1}/get_beatmaps{queryString}"));
		    });

		    Beatmap[] rResult = JsonConvert.DeserializeObject<Beatmap[]>(result);
		    return rResult;
	    }

	    public class User
	    {
		    [JsonProperty(PropertyName = "user_id")]
		    public int UserId;
		    [JsonProperty(PropertyName = "username")]
		    public string UserName;
			    
		    [JsonProperty(PropertyName = "count300")]
		    public long Count300;
		    [JsonProperty(PropertyName = "count100")]
		    public long Count100;
		    [JsonProperty(PropertyName = "count50")]
		    public long Count50;
			    
		    [JsonProperty(PropertyName = "playcount")]
		    public long PlayCount;
		    [JsonProperty(PropertyName = "ranked_score")]
		    public long RankedScore;
		    [JsonProperty(PropertyName = "total_score")]
		    public long TotalScore;
		    [JsonProperty(PropertyName = "pp_rank")]
		    public float GlobalRank;
		    [JsonProperty(PropertyName = "pp_raw")]
		    public float PerformancePointsRaw;
		    [JsonProperty(PropertyName = "level")]
		    public float Level;
		    [JsonProperty(PropertyName = "accuracy")]
		    public float Accuracy;
		    
		    [JsonProperty(PropertyName = "count_rank_ss")]
		    public int CountSs;
		    [JsonProperty(PropertyName = "count_rank_ssh")]
		    public int CountSsh; // i've just noticed it now. can we connect to our SS+ ?
		    [JsonProperty(PropertyName = "count_rank_s")]
		    public int CountS;
		    [JsonProperty(PropertyName = "count_rank_sh")]
		    public int CountSh;
		    [JsonProperty(PropertyName = "count_rank_a")]
		    public int CountA;
		    
		    [JsonProperty(PropertyName = "country")]
		    public string Country;
		    [JsonProperty(PropertyName = "total_seconds_played")]
		    public long TotalSecondsPlayed;
		    [JsonProperty(PropertyName = "pp_country_rank")]
		    public int CountryRank;
		    
		    [JsonProperty(PropertyName = "events")]
		    public UserEvent[] Events;
	    }
	    public class UserEvent
	    {
		    [JsonProperty(PropertyName = "display_html")]
		    public string DisplayHtml;
		    [JsonProperty(PropertyName = "beatmap_id")]
		    public string BeatmapId;
		    [JsonProperty(PropertyName = "beatmapset_id")]
		    public string BeatmapSetId;
		    [JsonProperty(PropertyName = "date")]
		    public DateTime Date;
		    [JsonProperty(PropertyName = "epicfactor")]
		    public byte EpicFactor;
	    }
        public class Beatmap
        {
	        [JsonProperty(PropertyName = "approved")]
	        public RankedStatus RankedStatus;
	        
	        [JsonProperty(PropertyName = "approved_date")]
		    public DateTime? RankedDate;
	        
	        [JsonProperty(PropertyName = "last_update")]
	        public DateTime LastUpdate;
	        
	        [JsonProperty(PropertyName = "artist")]
	        public string Artist;
	        
	        [JsonProperty(PropertyName = "beatmap_id")]
	        public int BeatmapId;
	        
	        [JsonProperty(PropertyName = "beatmapset_id")]
	        public int BeatmapSetId;
	        
	        [JsonProperty(PropertyName = "bpm")]
	        public float Bpm;
	        
	        [JsonProperty(PropertyName = "creator")]
	        public string Creator;
	        
	        [JsonProperty(PropertyName = "creator_id")]
	        public int CreatorId;
	        
	        [JsonProperty(PropertyName = "difficultyrating")]
	        public double Difficulty;
	        
	        [JsonProperty(PropertyName = "diff_size")]
	        public float Cs;
	        
	        [JsonProperty(PropertyName = "diff_overall")]
	        public float Od;
	        
	        [JsonProperty(PropertyName = "diff_approach")]
	        public float Ar;
	        
	        [JsonProperty(PropertyName = "diff_drain")]
	        public float Hp;
	        
	        [JsonProperty(PropertyName = "hit_length")]
	        public int HitLength;
	        
	        [JsonProperty(PropertyName = "genre_id")]
	        public Genre Genre;
	        
	        [JsonProperty(PropertyName = "language_id")]
	        public Language Language;
	        
	        [JsonProperty(PropertyName = "title")]
	        public string Title;
	        
	        [JsonProperty(PropertyName = "total_length")]
	        public int TotalLength;
	        
	        [JsonProperty(PropertyName = "version")]
	        public string DifficultyName;
	        
	        [JsonProperty(PropertyName = "file_md5")]
	        public string BeatmapMd5;
	        
	        [JsonProperty(PropertyName = "mode")]
	        public PlayMode PlayMode;
	        
	        [JsonProperty(PropertyName = "tags")]
	        public string Tags;
	        
	        [JsonProperty(PropertyName = "favourite_count")]
	        public int FavouriteCount;
		        
	        [JsonProperty(PropertyName = "playcount")]
	        public int PlayCount;
		        
	        [JsonProperty(PropertyName = "passcount")]
	        public int PassCount;
        }
    }
}