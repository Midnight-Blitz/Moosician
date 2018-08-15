using Discord;
using NadekoBot.Common.Collections;
using System;
using System.Collections.Generic;

namespace NadekoBot.Core.Services.Database.Models
{
    public class BotConfig : DbEntity
    {
        public HashSet<BlacklistItem> Blacklist { get; set; }
        public ulong BufferSize { get; set; } = 4000000;
        public bool ForwardMessages { get; set; } = true;
        public bool ForwardToAllOwners { get; set; } = true;

        public float CurrencyGenerationChance { get; set; } = 0.02f;
        public int CurrencyGenerationCooldown { get; set; } = 10;
        
        public List<PlayingStatus> RotatingStatusMessages { get; set; } = new List<PlayingStatus>();

        public bool RotatingStatuses { get; set; } = false;
        public string RemindMessageFormat { get; set; } = "❗⏰**I've been told by %user% to remind you, '%message%'**⏰❗";
        
        //currency
        public string CurrencySign { get; set; } = "🌸";
        public string CurrencyName { get; set; } = "Cowbell";
        public string CurrencyPluralName { get; set; } = "Cowbells";

        public int TriviaCurrencyReward { get; set; } = 0;
        /// <summary> UNUSED </summary> 
        [Obsolete("Use MinBet instead.")]
        public int MinimumBetAmount { get; set; } = 2;
        public float BetflipMultiplier { get; set; } = 1.5f;
        public int CurrencyDropAmount { get; set; } = 1;
        public int? CurrencyDropAmountMax { get; set; } = null;
        public float Betroll67Multiplier { get; set; } = 2;
        public float Betroll91Multiplier { get; set; } = 4;
        public float Betroll100Multiplier { get; set; } = 10;
        public int TimelyCurrency { get; set; } = 0;
        public int TimelyCurrencyPeriod { get; set; } = 0;
        public int MinWaifuPrice { get; set; } = 50;
        public float DailyCurrencyDecay { get; set; } = 0;
        public DateTime LastCurrencyDecay { get; set; } = DateTime.MinValue;

        public HashSet<EightBallResponse> EightBallResponses { get; set; } = new HashSet<EightBallResponse>();
        public HashSet<RaceAnimal> RaceAnimals { get; set; } = new HashSet<RaceAnimal>();

        public string DMHelpString { get; set; } = "Type `.help` or `.h` for command help.";
        public string HelpString { get; set; } = @"You can use `.modules` to see an updated list of all modules.
Modules order the hundreds of Moosician commands available into sections.
Currently, this consists of: `Admin`, `CustomReactions`, `Gambling`, `Games`,
`Help`, `Music`, `Permissions`, `Searches`, `Utility`, and `Xp`.

You can use `.commands ModuleName`
(Example: `.commands Music`) shows all of the commands in that module.

For help with a specific command, use `.help CommandName` or `.h name`
(Example: `.help .h`)


**LIST OF COMMANDS CAN BE FOUND ON THIS LINK**
(Please note, these are a bit inconsistant with Moosician's commands.
For now, please use the help screen here.)
<http://nadekobot.me/commands>
(Some time in the future, there will be a resource for this that's almost always updated, and will replace the link above.)

For bot support, please ask *Midnight#0042*. If it involves moderation, please ask a Moderator or Admin instead.";

        public int MigrationVersion { get; set; }

        public string OkColor { get; set; } = "00e584";
        public string ErrorColor { get; set; } = "ee281f";
        public string Locale { get; set; } = null;
        public IndexedCollection<StartupCommand> StartupCommands { get; set; }
        public HashSet<BlockedCmdOrMdl> BlockedCommands { get; set; }
        public HashSet<BlockedCmdOrMdl> BlockedModules { get; set; }
        public int PermissionVersion { get; set; } = 2;
        public string DefaultPrefix { get; set; } = ".";
        public bool CustomReactionsStartWith { get; set; } = false;
        public int XpPerMessage { get; set; } = 3;
        public int XpMinutesTimeout { get; set; } = 5;
        public int DivorcePriceMultiplier { get; set; } = 150;
        public float PatreonCurrencyPerCent { get; set; } = 1.0f;        
        public int WaifuGiftMultiplier { get; set; } = 1; 
        public int MinimumTriviaWinReq { get; set; } 
        public int MinBet { get; set; } = 0; 
        public int MaxBet { get; set; } = 0; 
        public ConsoleOutputType ConsoleOutputType { get; set; } = ConsoleOutputType.Normal;

        public string UpdateString { get; set; } = "The source has released a new update!";
        public UpdateCheckType CheckForUpdates { get; set; } = UpdateCheckType.Release;
        public DateTime LastUpdate { get; set; } = new DateTime(2018, 5, 5, 0, 0, 0, DateTimeKind.Utc);
        public bool CurrencyGenerationPassword { get; set; }
    }

    public enum UpdateCheckType
    {
        Release, Commit, None
    }

    public class BlockedCmdOrMdl : DbEntity
    {
        public string Name { get; set; }

        public override bool Equals(object obj) =>
            (obj as BlockedCmdOrMdl)?.Name?.ToUpperInvariant() == Name.ToUpperInvariant();

        public override int GetHashCode() =>
            Name.GetHashCode(System.StringComparison.InvariantCulture);
    }

    public enum ConsoleOutputType
    { 
        Normal,
        Simple
    }

    public class StartupCommand : DbEntity, IIndexed
    {
        public int Index { get; set; }
        public string CommandText { get; set; }
        public ulong ChannelId { get; set; }
        public string ChannelName { get; set; }
        public ulong? GuildId { get; set; }
        public string GuildName { get; set; }
        public ulong? VoiceChannelId { get; set; }
        public string VoiceChannelName { get; set; }
        public int Interval { get; set; }
    }

    public class PlayingStatus : DbEntity
    {
        public string Status { get; set; }
        public ActivityType Type { get; set; }
    }

    public class BlacklistItem : DbEntity
    {
        public ulong ItemId { get; set; }
        public BlacklistType Type { get; set; }
    }

    public enum BlacklistType
    {
        Server,
        Channel,
        User
    }

    public class EightBallResponse : DbEntity
    {
        public string Text { get; set; }

        public override int GetHashCode() =>
            Text.GetHashCode(StringComparison.InvariantCulture);

        public override bool Equals(object obj) =>
            (obj is EightBallResponse response)
            ? response.Text == Text
            : base.Equals(obj);
    }

    public class RaceAnimal : DbEntity
    {
        public string Icon { get; set; }
        public string Name { get; set; }

        public override int GetHashCode() =>
            Icon.GetHashCode(StringComparison.InvariantCulture);


        public override bool Equals(object obj) =>
            (obj is RaceAnimal animal)
            ? animal.Icon == Icon
            : base.Equals(obj);
    }
}