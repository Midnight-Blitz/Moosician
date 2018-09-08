using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Common.Replacements;
using NadekoBot.Core.Modules.Searches.Common;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
//using NadekoBot.Modules.Cantrips.Common;
//using NadekoBot.Modules.Cantrips.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration = AngleSharp.Configuration;

namespace NadekoBot.Modules.Cantrips
{
    public partial class Cantrips
    {
        [Group]
        public class Casting : NadekoSubmodule
        {
            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [OwnerOnly]
            public async Task Spell()
            {
                var embed = new EmbedBuilder()
                    .WithOkColor()
                    .WithTitle(GetText("casting"))
                    .WithDescription(Context.User.Mention + " " + (GetText("cast")) + Format.Bold(GetText("cast_result")));

                     await Context.Channel.EmbedAsync(embed)
                .ConfigureAwait(false);
            }
        }
    }
}