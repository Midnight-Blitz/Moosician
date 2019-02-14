using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Common.Replacements;
using NadekoBot.Core.Services;
using NadekoBot.Core.Services.Database.Models;
using NadekoBot.Extensions;
using NadekoBot.Modules.Administration.Services;

/// <summary>
/// TRANSITION TO SERVICE
/// </summary>
namespace NadekoBot.Modules.Administration
{
    public partial class Administration : NadekoTopLevelModule<AdministrationService>
    {
        private readonly DbService _db;

        public Administration(DbService db)
        {
            _db = db;
        }

        public enum List { List = 0, Ls = 0 }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Priority(2)]
        public async Task Delmsgoncmd(List _)
        {
            var guild = (SocketGuild)Context.Guild;
            GuildConfig conf;
            using (var uow = _db.UnitOfWork)
            {
                conf = uow.GuildConfigs.ForId(Context.Guild.Id,
                    set => set.Include(x => x.DelMsgOnCmdChannels));
            }

            var embed = new EmbedBuilder()
                .WithOkColor()
                .WithTitle(GetText("server_delmsgoncmd"))
                .WithDescription(conf.DeleteMessageOnCommand
                    ? "✅"
                    : "❌");

            var str = string.Join("\n", conf.DelMsgOnCmdChannels
                .Select(x =>
                {
                    var ch = guild.GetChannel(x.ChannelId)?.ToString()
                        ?? x.ChannelId.ToString();
                    var prefix = x.State
                        ? "✅ "
                        : "❌ ";
                    return prefix + ch;
                }));

            if (string.IsNullOrWhiteSpace(str))
                str = "-";

            embed.AddField(GetText("channel_delmsgoncmd"), str);

            await Context.Channel.EmbedAsync(embed)
                .ConfigureAwait(false);
        }

        public enum Server { Server }
        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Priority(1)]
        public async Task Delmsgoncmd(Server _ = Server.Server)
        {
            if (_service.ToggleDeleteMessageOnCommand(Context.Guild.Id))
            {
                _service.DeleteMessagesOnCommand.Add(Context.Guild.Id);
                await ReplyConfirmLocalizedAsync("delmsg_on").ConfigureAwait(false);
            }
            else
            {
                _service.DeleteMessagesOnCommand.TryRemove(Context.Guild.Id);
                await ReplyConfirmLocalizedAsync("delmsg_off").ConfigureAwait(false);
            }
        }

        public enum Channel { Channel }
        public enum State { Enable, Disable, Inherit }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        ///[RequireBotPermission(GuildPermission.ManageMessages)]
        [Priority(0)]
        public Task Delmsgoncmd(Channel _, State s, ITextChannel ch)
            => Delmsgoncmd(_, s, ch.Id);

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        ///[RequireBotPermission(GuildPermission.ManageMessages)]
        [Priority(1)]
        public async Task Delmsgoncmd(Channel _, State s, ulong? chId = null)
        {
            chId = chId ?? Context.Channel.Id;
            using (var uow = _db.UnitOfWork)
            {
                var conf = uow.GuildConfigs.ForId(Context.Guild.Id,
                    set => set.Include(x => x.DelMsgOnCmdChannels));

                var obj = new DelMsgOnCmdChannel()
                {
                    ChannelId = chId.Value,
                    State = s == State.Enable,
                };
                var del = conf.DelMsgOnCmdChannels.FirstOrDefault(x => x == obj);
                if (s != State.Inherit)
                    conf.DelMsgOnCmdChannels.Add(obj);
                else
                {
                    if (del != null)
                    {
                        uow._context.Remove(del);
                    }
                }

                await uow.CompleteAsync().ConfigureAwait(false);
            }
            if (s == State.Disable)
            {
                _service.DeleteMessagesOnCommandChannels.AddOrUpdate(chId.Value, false, delegate { return false; });
                await ReplyConfirmLocalizedAsync("delmsg_channel_off").ConfigureAwait(false);
            }
            else if (s == State.Enable)
            {
                _service.DeleteMessagesOnCommandChannels.AddOrUpdate(chId.Value, true, delegate { return true; });
                await ReplyConfirmLocalizedAsync("delmsg_channel_on").ConfigureAwait(false);
            }
            else
            {
                _service.DeleteMessagesOnCommandChannels.TryRemove(chId.Value, out var _);
                await ReplyConfirmLocalizedAsync("delmsg_channel_inherit").ConfigureAwait(false);
            }
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.DeafenMembers)]
        public async Task Deafen(params IGuildUser[] users)
        {
            if (!users.Any())
                return;
            foreach (var u in users)
            {
                try
                {
                    await u.ModifyAsync(usr => usr.Deaf = true).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }
            await ReplyConfirmLocalizedAsync("deafen").ConfigureAwait(false);

        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.DeafenMembers)]
        public async Task UnDeafen(params IGuildUser[] users)
        {
            if (!users.Any())
                return;

            foreach (var u in users)
            {
                try
                {
                    await u.ModifyAsync(usr => usr.Deaf = false).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }
            await ReplyConfirmLocalizedAsync("undeafen").ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task DelVoiChanl([Remainder] IVoiceChannel voiceChannel)
        {
            await voiceChannel.DeleteAsync().ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("delvoich", Format.Bold(voiceChannel.Name)).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task CreatVoiChanl([Remainder] string channelName)
        {
            var ch = await Context.Guild.CreateVoiceChannelAsync(channelName).ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("createvoich", Format.Bold(ch.Name)).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task DelTxtChanl([Remainder] ITextChannel toDelete)
        {
            await toDelete.DeleteAsync().ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("deltextchan", Format.Bold(toDelete.Name)).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task CreaTxtChanl([Remainder] string channelName)
        {
            var txtCh = await Context.Guild.CreateTextChannelAsync(channelName).ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("createtextchan", Format.Bold(txtCh.Name)).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task SetTopic([Remainder] string topic = null)
        {
            var channel = (ITextChannel)Context.Channel;
            topic = topic ?? "";
            await channel.ModifyAsync(c => c.Topic = topic).ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("set_topic").ConfigureAwait(false);

        }
        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task SetChanlName([Remainder] string name)
        {
            var channel = (ITextChannel)Context.Channel;
            await channel.ModifyAsync(c => c.Name = name).ConfigureAwait(false);
            await ReplyConfirmLocalizedAsync("set_channel_name").ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Edit(ulong messageId, [Remainder] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var msg = await Context.Channel.GetMessageAsync(messageId);

            if (!(msg is IUserMessage umsg) || msg.Author.Id != Context.Client.CurrentUser.Id)
                return;

            var rep = new ReplacementBuilder()
                    .WithDefault(Context)
                    .Build();

            if (CREmbed.TryParse(text, out var crembed))
            {
                rep.Replace(crembed);
                await umsg.ModifyAsync(x =>
                {
                    x.Embed = crembed.ToEmbed().Build();
                    x.Content = crembed.PlainText?.SanitizeMentions() ?? "";
                }).ConfigureAwait(false);
            }
            else
            {
                await umsg.ModifyAsync(x => x.Content = text.SanitizeMentions())
                    .ConfigureAwait(false);
            }
        }
    }
}