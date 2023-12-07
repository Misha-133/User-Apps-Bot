using System.Text;

using org.mariuszgromada.math.mxparser;

namespace UserAppBot.Modules;

[CommandContextType(ApplicationCommandContextType.BotDm, ApplicationCommandContextType.PrivateChannel, ApplicationCommandContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall)]
public class CommandModule(ILogger<CommandModule> logger) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("test", "Just a test command")]
    public async Task TestCommand()
        => await RespondAsync("Hello There");

    [SlashCommand("say", "Make the bot say something")]
    public async Task SayCmd(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            await RespondAsync("Can't send an empty message.", ephemeral: true);
            return;
        }

        await RespondAsync(message);
    }

    [NsfwCommand(true)]
    [SlashCommand("msfw-test", "Just a NSFW command")]
    public async Task NsfwCmd()
    {
        await RespondAsync("Hello There!\nc===3", ephemeral: true);
    }

    [SlashCommand("user-slash-command", "User-app slash command")]
    public async Task UserSlashCommand()
    {
        await RespondAsync("Hello There!");
    }

    [UserCommand("Bonk")]
    public async Task SlapCommand(IUser user)
    {
        await RespondAsync($"# [BONK](https://tenor.com/view/vorzek-vorzneck-oglg-og-lol-gang-gif-24901093) {user.Mention}", allowedMentions: AllowedMentions.None);
    }

    [MessageCommand("User-app Message-cmd")]
    public async Task MessageCommand(IMessage message)
    {
        await RespondAsync($"Hello There! | message author: {message.Author}");
    }

    [MessageCommand("Math")]
    public async Task MathCommand(IMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Content))
        {
            await RespondAsync("Can't solve an empty message.", ephemeral: true);
            return;
        }

        try
        {
            var expression = new Expression(message.Content);
            await RespondAsync(embed: new EmbedBuilder().WithDescription($"`{message.Content}` = `{expression.calculate()}`")
                .WithColor(0xff00)
                .Build());
        }
        catch (Exception ex)
        {
            await RespondAsync(embed: new EmbedBuilder()
                .WithDescription($"Error!\n{ex.Message}")
                .WithColor(0xff0000)
                .Build());
        }
    }


    [SlashCommand("lorem", "Ipsum dolor sit amit")]
    public async Task LoremGenerator([MinValue(5)] int? length = 5)
    {
        await RespondAsync($"Lorem ipsum dolor sit amet {GetLorem(length ?? 5)}");
    }

    public static Random rnd = new Random();

    public static string GetLorem(int count)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < count; i++)
        {
            sb.Append(Lorem[rnd.Next(0, Lorem.Length)]);
            sb.Append(' ');
        }

        return sb.ToString();
    }

    public static string[] Lorem = """
                                   Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quod praeceptum quia maius erat, quam ut ab homine videretur, idcirco assignatum est deo. Gloriosa ostentatio in constituendo summo bono. Tum Piso: Quoniam igitur aliquid omnes, quid Lucius noster? Sic enim censent, oportunitatis esse beate vivere. Qua ex cognitione facilior facta est investigatio rerum occultissimarum. Omnes enim iucundum motum, quo sensus hilaretur. Sed venio ad inconstantiae crimen, ne saepius dicas me aberrare; Duo Reges: constructio interrete. Qua ex cognitione facilior facta est investigatio rerum occultissimarum. In qua quid est boni praeter summam voluptatem, et eam sempiternam?
                                   Omnes enim iucundum motum, quo sensus hilaretur. Theophrastus mediocriterne delectat, cum tractat locos ab Aristotele ante tractatos? Suo enim quisque studio maxime ducitur. Esse enim quam vellet iniquus iustus poterat inpune. Nam de isto magna dissensio est. Quo plebiscito decreta a senatu est consuli quaestio Cn. At multis malis affectus. Quis enim potest ea, quae probabilia videantur ei, non probare? Quam multa vitiosa! summum enim bonum et malum vagiens puer utra voluptate diiudicabit, stante an movente? Diodorus, eius auditor, adiungit ad honestatem vacuitatem doloris. Qui autem esse poteris, nisi te amor ipse ceperit?
                                   Aliter enim explicari, quod quaeritur, non potest. Illa sunt similia: hebes acies est cuipiam oculorum, corpore alius senescit; Nam si propter voluptatem, quae est ista laus, quae possit e macello peti? Perturbationes autem nulla naturae vi commoventur, omniaque ea sunt opiniones ac iudicia levitatis. Ita cum ea volunt retinere, quae superiori sententiae conveniunt, in Aristonem incidunt; Quod autem in homine praestantissimum atque optimum est, id deseruit.
                                   """.Split();


    [SlashCommand("big-red-button", "Just a big red button")]
    public async Task BigRedButton()
    {
        await RespondAsync(components: new ComponentBuilder().WithButton("NEVER EVER CLICK THIS", "do_not_click", ButtonStyle.Danger).Build());
    }

    [ComponentInteraction("do_not_click")]
    public async Task DoNotClickAsync()
    {
        await RespondAsync(ephemeral: true, embed: new EmbedBuilder()
                .WithTitle("Totally-not-a-scam.co")
                .WithDescription("# **CONGRATS! YOU WON 69YEARS OF NITRO FOR FREE!!**")
                .Build(),
            components: new ComponentBuilder()
                .WithButton("CLAIM!", style: ButtonStyle.Link, url: "https://www.youtube.com/watch?v=dQw4w9WgXcQ")
                .WithButton("nah", style: ButtonStyle.Link, url: "https://www.youtube.com/watch?v=dQw4w9WgXcQ")
                .Build());
    }

}