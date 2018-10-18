using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace opi.v1
{
    public class Api
    {
        private const string Apiv1 = "https://osu.ppy.sh/api";
        private string ApiKey { get; }

        public Api(string apiKey) => ApiKey = apiKey;

        #region SYNC
        public Beatmap GetBeatmap(int beatmapId = -1, PlayMode mode = PlayMode.All, bool convertedMaps = false,
            string beatmapMd5 = "", int limit = 500) =>
            GetBeatmapAsync(beatmapId, mode, convertedMaps, beatmapMd5, limit).GetAwaiter().GetResult();
        public Beatmap[] GetBeatmapSet(int beatmapSetId = -1, PlayMode mode = PlayMode.All,
            bool convertedMaps = false, string beatmapMd5 = "", int limit = 500)
            => GetBeatmapSetAsync(beatmapSetId, mode, convertedMaps, beatmapMd5, limit).GetAwaiter().GetResult();
        public User GetUser(int userId, PlayMode mode = PlayMode.Osu, int eventDays = 1) =>
            GetUserAsync(userId, mode, eventDays).GetAwaiter().GetResult();
        public User GetUser(string userName, PlayMode mode = PlayMode.Osu, int eventDays = 1) =>
            GetUserAsync(userName, mode, eventDays).GetAwaiter().GetResult();
        public Score[] GetScores(int beatmapId, int userId = -1, PlayMode mode = PlayMode.Osu, int mods = 0,
            int limit = 50) =>
            GetScoresAsync(beatmapId, userId, mode, mods, limit).GetAwaiter().GetResult();
        public Score[] GetScores(int beatmapId, string userName = "", PlayMode mode = PlayMode.Osu, int mods = 0,
            int limit = 50) =>
            GetScoresAsync(beatmapId, userName, mode, mods, limit).GetAwaiter().GetResult();
        public UserScore[] GetUserBest(int userId = -1, PlayMode mode = PlayMode.Osu, int limit = 10) =>
            GetUserBestAsync(userId, mode, limit).GetAwaiter().GetResult();
        public UserScore[] GetUserBest(string userName = "", PlayMode mode = PlayMode.Osu, int limit = 10) =>
            GetUserBestAsync(userName, mode, limit).GetAwaiter().GetResult();
        public UserScore[] GetUserRecent(int userId = -1, PlayMode mode = PlayMode.Osu, int limit = 10) =>
            GetUserRecentAsync(userId, mode, limit).GetAwaiter().GetResult();
        public UserScore[] GetUserRecent(string userName = "", PlayMode mode = PlayMode.Osu, int limit = 10) =>
            GetUserRecentAsync(userName, mode, limit).GetAwaiter().GetResult();
        public MultiPlayer[] GetMatch(int matchId) =>
            GetMatchAsync(matchId).GetAwaiter().GetResult();
        public File GetReplay(PlayMode mode, int beatmapId, int userId) =>
            GetReplayAsync(mode, beatmapId, userId).GetAwaiter().GetResult();
        #endregion
        
        #region ASYNC
        public async Task<Beatmap> GetBeatmapAsync(int beatmapId = -1, PlayMode mode = PlayMode.All,
            bool convertedMaps = false, string beatmapMd5 = "", int limit = 500)
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
        public async Task<Beatmap[]> GetBeatmapSetAsync(int beatmapSetId = -1, PlayMode mode = PlayMode.All,
            bool convertedMaps = false, string beatmapMd5 = "", int limit = 500)
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
        public async Task<User> GetUserAsync(int userId, PlayMode mode = PlayMode.Osu, int eventDays = 1)
        {
            if (eventDays > 31)
                throw new ArgumentOutOfRangeException(nameof(eventDays));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for Users");

            string queryString = $"?k={ApiKey}&u={userId}&m={(int) mode}&type=id&event_days={eventDays}";
            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user{queryString}"));
            });

            User[] rResult = JsonConvert.DeserializeObject<User[]>(result);
            return rResult.Length <= 0 ? null : rResult[0];
        }
        public async Task<User> GetUserAsync(string userName, PlayMode mode = PlayMode.Osu, int eventDays = 1)
        {
            if (eventDays > 31)
                throw new ArgumentOutOfRangeException(nameof(eventDays));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for Users");

            string queryString = $"?k={ApiKey}&u={userName}&m={(int) mode}&type=string&event_days={eventDays}";
            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user{queryString}"));
            });

            User[] rResult = JsonConvert.DeserializeObject<User[]>(result);
            return rResult.Length <= 0 ? null : rResult[0];
        }
        public async Task<Score[]> GetScoresAsync(int beatmapId, int userId = -1, PlayMode mode = PlayMode.Osu,
            int mods = 0, int limit = 50)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for Scores");

            string queryString = $"?k={ApiKey}&b={beatmapId}";

            if (userId > 0)
                queryString += $"&u={userId}&type=id";

            queryString += $"&m={mode}&mods={mods}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_scores{queryString}"));
            });

            Score[] rResult = JsonConvert.DeserializeObject<Score[]>(result);
            return rResult;
        }
        public async Task<Score[]> GetScoresAsync(int beatmapId, string userName = "", PlayMode mode = PlayMode.Osu,
            int mods = 0, int limit = 50)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for Scores");

            string queryString = $"?k={ApiKey}&b={beatmapId}";

            if (userName.Length > 0)
                queryString += $"&u={userName}&type=string";

            queryString += $"&m={mode}&mods={mods}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_scores{queryString}"));
            });

            Score[] rResult = JsonConvert.DeserializeObject<Score[]>(result);
            return rResult;
        }
        public async Task<UserScore[]> GetUserBestAsync(int userId, PlayMode mode = PlayMode.Osu, int limit = 10)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for User Best");

            string queryString = $"?k={ApiKey}&u={userId}&type=id&m={mode}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user_best{queryString}"));
            });

            UserScore[] rResult = JsonConvert.DeserializeObject<UserScore[]>(result);
            return rResult;
        }
        public async Task<UserScore[]> GetUserBestAsync(string userName, PlayMode mode = PlayMode.Osu, int limit = 10)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for User Best");

            string queryString = $"?k={ApiKey}&u={userName}&type=string&m={mode}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user_best{queryString}"));
            });

            UserScore[] rResult = JsonConvert.DeserializeObject<UserScore[]>(result);
            return rResult;
        }
        public async Task<UserScore[]> GetUserRecentAsync(int userId, PlayMode mode = PlayMode.Osu, int limit = 10)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for User Best");

            string queryString = $"?k={ApiKey}&u={userId}&type=id&m={mode}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user_recent{queryString}"));
            });

            UserScore[] rResult = JsonConvert.DeserializeObject<UserScore[]>(result);
            return rResult;
        }
        public async Task<UserScore[]> GetUserRecentAsync(string userName, PlayMode mode = PlayMode.Osu, int limit = 10)
        {
            if (limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for User Best");

            string queryString = $"?k={ApiKey}&u={userName}&type=string&m={mode}&limit={limit}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_user_recent{queryString}"));
            });

            UserScore[] rResult = JsonConvert.DeserializeObject<UserScore[]>(result);
            return rResult;
        }
        public async Task<File> GetReplayAsync(PlayMode mode, int beatmapId, int userId)
        {
            if (mode == PlayMode.All)
                throw new ArgumentException("PlayMode can not be set to All for Replays");

            if (beatmapId <= 0)
                throw new ArgumentException();
            if (userId <= 0)
                throw new ArgumentException();
            
            string queryString = $"?k={ApiKey}&m={(int)mode}&b={beatmapId}&u={userId}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_replay{queryString}"));
            });

            File rResult = JsonConvert.DeserializeObject<File>(result);
            return rResult;
        }
        public async Task<MultiPlayer[]> GetMatchAsync(int matchId)
        {
            string queryString = $"?k={ApiKey}&mp={matchId}";

            string result = await Task.Run(() =>
            {
                using (WebClient client = new WebClient())
                    return client.DownloadString(new Uri($"{Apiv1}/get_match{queryString}"));
            });

            MultiPlayer[] rResult = JsonConvert.DeserializeObject<MultiPlayer[]>(result);
            return rResult;
        }
        #endregion
    }

    #region CLASSES
    public class File
    {
        [JsonProperty("content")]
        public string Content;
        
        [JsonProperty("encoding")]
        public string Encoding;
    }
    public class Match
    {
        [JsonProperty("match_id")] public int MatchId;

        [JsonProperty("name")] public string MatchName;

        [JsonProperty("start_time")] public DateTime StartTime;

        [JsonProperty("end_time")] public DateTime EndTime;
    }
    public class MatchScore
    {
        [JsonProperty("slot")] public int Slot;

        [JsonProperty("team")] public Team Team;

        [JsonProperty("user_id")] public int UserId;

        [JsonProperty("score")] public int TotalScore;

        [JsonProperty("maxcombo")] public int MaxCombo;

        [JsonProperty("rank")] public string Grade;

        [JsonProperty("count50")] public int Count50;

        [JsonProperty("count100")] public int Count100;

        [JsonProperty("count300")] public int Count300;

        [JsonProperty("countmiss")] public int CountMiss;

        [JsonProperty("countgeki")] public int CountGeki;

        [JsonProperty("countkatu")] public int CountKatu;

        [JsonProperty("perfect")] public bool Perfect;

        [JsonProperty("pass")] public bool Pass;
    }
    public class Game
    {
        [JsonProperty("game_id")] public int GameId;

        [JsonProperty("start_time")] public DateTime StartTime;

        [JsonProperty("end_time")] public DateTime EndTime;

        [JsonProperty("beatmap_id")] public int BeatmapId;

        [JsonProperty("play_mode")] public PlayMode PlayMode;

        [JsonProperty("match_type")] public int MatchType;

        [JsonProperty("scoring_type")] public ScoringType ScoringType;

        [JsonProperty("team_type")] public Team TeamType;

        [JsonProperty("mods")] public int Mods;

        [JsonProperty("scores")] public MatchScore[] Scores;
    }
    public class MultiPlayer
    {
        [JsonProperty("match")] public Match Match;

        [JsonProperty("games")] public Game[] Games;
    }
    public class UserScore
    {
        [JsonProperty("beatmap_id")] public int BeatmapId;

        [JsonProperty("score")] public int TotalScore;

        [JsonProperty("maxcombo")] public int MaxCombo;

        [JsonProperty("count300")] public int Count300;

        [JsonProperty("count100")] public int Count100;

        [JsonProperty("count50")] public int Count50;

        [JsonProperty("countmiss")] public int CountMiss;

        [JsonProperty("countkatu")] public int CountKatu;

        [JsonProperty("countgeki")] public int CountGeki;

        [JsonProperty("perfect")] public bool Perfect;

        [JsonProperty("enabled_mods")] public int Mods;

        [JsonProperty("user_id")] public int UserId;

        [JsonProperty("date")] public DateTime Date;

        [JsonProperty("rank")] public string Grade;

        [JsonProperty("pp")] public float PerformancePoints;
    }
    public class Score
    {
        [JsonProperty("score_id")] public int ScoreId;

        [JsonProperty("score")] public int TotalScore;

        [JsonProperty("username")] public string Username;

        [JsonProperty("count300")] public int Count300;

        [JsonProperty("count100")] public int Count100;

        [JsonProperty("count50")] public int Count50;

        [JsonProperty("countmiss")] public int CountMiss;

        [JsonProperty("maxcombo")] public int MaxCombo;

        [JsonProperty("countkatu")] public int CountKatu;

        [JsonProperty("countgeki")] public int CountGeki;

        [JsonProperty("perfect")] public bool Perfect;

        [JsonProperty("enabled_mods")] public int Mods;

        [JsonProperty("user_id")] public int UserId;

        [JsonProperty("date")] public DateTime Date;

        [JsonProperty("rank")] public string Grade;

        [JsonProperty("pp")] public float PerformancePoints;

        [JsonProperty("replay_available")] public bool ReplayAvaible;
    }
    public class User
    {
        [JsonProperty("user_id")] public int UserId;

        [JsonProperty("username")] public string UserName;

        [JsonProperty("count300")] public long Count300;

        [JsonProperty("count100")] public long Count100;

        [JsonProperty("count50")] public long Count50;

        [JsonProperty("playcount")] public long PlayCount;

        [JsonProperty("ranked_score")] public long RankedScore;

        [JsonProperty("total_score")] public long TotalScore;

        [JsonProperty("pp_rank")] public float GlobalRank;

        [JsonProperty("pp_raw")] public float PerformancePointsRaw;

        [JsonProperty("level")] public float Level;

        [JsonProperty("accuracy")] public float Accuracy;

        [JsonProperty("count_rank_ss")] public int CountSs;

        [JsonProperty("count_rank_ssh")] public int CountSsh; // i've just noticed it now. can we connect to our SS+ ?

        [JsonProperty("count_rank_s")] public int CountS;

        [JsonProperty("count_rank_sh")] public int CountSh;

        [JsonProperty("count_rank_a")] public int CountA;

        [JsonProperty("country")] public string Country;

        [JsonProperty("total_seconds_played")] public long TotalSecondsPlayed;

        [JsonProperty("pp_country_rank")] public int CountryRank;

        [JsonProperty("events")] public UserEvent[] Events;
    }
    public class UserEvent
    {
        [JsonProperty("display_html")] public string DisplayHtml;

        [JsonProperty("beatmap_id")] public string BeatmapId;

        [JsonProperty("beatmapset_id")] public string BeatmapSetId;

        [JsonProperty("date")] public DateTime Date;

        [JsonProperty("epicfactor")] public byte EpicFactor;
    }
    public class Beatmap
    {
        [JsonProperty("approved")] public RankedStatus RankedStatus;

        [JsonProperty("approved_date")] public DateTime? RankedDate;

        [JsonProperty("last_update")] public DateTime LastUpdate;

        [JsonProperty("artist")] public string Artist;

        [JsonProperty("beatmap_id")] public int BeatmapId;

        [JsonProperty("beatmapset_id")] public int BeatmapSetId;

        [JsonProperty("bpm")] public float Bpm;

        [JsonProperty("creator")] public string Creator;

        [JsonProperty("creator_id")] public int CreatorId;

        [JsonProperty("difficultyrating")] public double Difficulty;

        [JsonProperty("diff_size")] public float Cs;

        [JsonProperty("diff_overall")] public float Od;

        [JsonProperty("diff_approach")] public float Ar;

        [JsonProperty("diff_drain")] public float Hp;

        [JsonProperty("hit_length")] public int HitLength;

        [JsonProperty("genre_id")] public Genre Genre;

        [JsonProperty("language_id")] public Language Language;

        [JsonProperty("title")] public string Title;

        [JsonProperty("total_length")] public int TotalLength;

        [JsonProperty("version")] public string DifficultyName;

        [JsonProperty("file_md5")] public string BeatmapMd5;

        [JsonProperty("mode")] public PlayMode PlayMode;

        [JsonProperty("tags")] public string Tags;

        [JsonProperty("favourite_count")] public int FavouriteCount;

        [JsonProperty("playcount")] public int PlayCount;

        [JsonProperty("passcount")] public int PassCount;
    }
    #endregion
    
    #region ENUMS
    public enum Team
    {
        None,
        Red,
        Blue
    }
    public enum ScoringType
    {
        Score,
        Accuracy,
        Combo,
        ScoreV2
    }
    public enum RankedStatus
    {
        Graveyard = -2,
        Wip = -1,
        Pending,
        Ranked,
        Approved,
        Qualified,
        Loved
    }
    public enum PlayMode
    {
        Osu,
        Taiko,
        Ctb,
        Mania,
        All
    }
    public enum Language
    {
        Any,
        Other,
        English,
        Japanese,
        Chinese,
        Instrumental,
        Korean,
        French,
        German,
        Swedish,
        Spanish,
        Italian
    }
    public enum Genre
    {
        Any,
        Unspecified,
        Game,
        Anime,
        Rock,
        Pop,
        Other,
        Novelty,
        HipHop = 9,
        Electronic,
    }
    #endregion
}
