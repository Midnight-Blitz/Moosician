using Discord;
using Discord.Commands;
using System.Text.RegularExpressions;
using NadekoBot.Core.Common.TypeReaders.Models;
using NadekoBot.Extensions;
using System.Threading.Tasks;
using NadekoBot.Common.Attributes;
using Discord.WebSocket;
///using NadekoBot.Modules.Cantrips.Services;
using NadekoBot.Core.Services.Database.Models;
using System.Text;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NadekoBot.Common.Replacements;
using System.IO;
using NadekoBot.Core.Services;
using NadekoBot.Common;
using NadekoBot.Core.Common;
using NLog;
using NadekoBot.Common.ModuleBehaviors;
using NadekoBot.Core.Services.Database;
using NadekoBot.Core.Services.Impl;
using Newtonsoft.Json;
/// I don't know for sure what to use here yet. This will be cleaned up when the command is finished.

namespace NadekoBot.Modules.Cantrips
{
    public partial class Cantrips
    {
        [Group]
        public class Respects : NadekoSubmodule
        {
            ///private readonly DbService _db;

            ///public Respects(DbService db)
            ///{
            ///    _db = db;
            ///}

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task Respect()
            {
                var embed = new EmbedBuilder()
                    .WithOkColor()
                    .WithTitle(GetText("respects"))
                    .WithDescription(Context.User.Mention + " " + Format.Bold(GetText("respects_paid")));

                ///using (var uow = _db.UnitOfWork)
                ///{
                ///    total = ///Need
                ///    if (total != null)
                ///    {
                ///        total.UseCount += 1;
                ///        uow.Complete();
                ///    } 
                ///    if (total == null)
                ///        return;
                ///        
                ///    current =
                ///    {
                ///        ///Add current session totals here.
                ///    }
                ///    individual =
                ///    {
                ///        ///Add individual total stats here, as determined by another, individual counter, and UID.
                ///    }
                ///}

                ///var embed = new EmbedBuilder()
                ///    .WithOkColor()
                ///    .WithTitle(GetText("respects"))
                ///    .WithDescription(Context.User.Mention + " " + GetText("respects_paid", Format.Bold(GetText("respect_total"))));

                await Context.Channel.EmbedAsync(embed)
                    .ConfigureAwait(false);
            }
        }
    }
}