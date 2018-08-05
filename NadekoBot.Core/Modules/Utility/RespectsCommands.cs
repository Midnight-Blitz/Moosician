using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Extensions;
using System.Threading.Tasks;
using NadekoBot.Common.Attributes;
using NadekoBot.Modules.Utility.Services;
using NadekoBot.Core.Services.Database.Models;
using System.Text;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using NadekoBot.Core.Common.TypeReaders.Models;
using NadekoBot.Core.Services;
using NadekoBot.Common;
using NadekoBot.Core.Common;

namespace NadekoBot.Modules.Utility
{
    public partial class Utility
    {
        [Group]
        public class Respects : NadekoSubmodule
        {
            private readonly DbService _db;

            public Respects(DbService db)
            {
                _db = db;
            }



        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        public async Task Respect()
            {
                var embed = new EmbedBuilder()
                    .WithOkColor()
                    .WithTitle(GetText("respects"))
                    .WithDescription(Context.User.Mention + " " + GetText("respects_paid", Format.Bold(GetText("respect_total"))));

                ///var start = "";
                ///var current = "";

                await Context.Channel.EmbedAsync(embed)
                    .ConfigureAwait(false);
            }
        }
    }
}