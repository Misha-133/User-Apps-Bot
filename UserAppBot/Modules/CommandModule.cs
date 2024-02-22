using System.Text;

using org.mariuszgromada.math.mxparser;

namespace UserAppBot.Modules;

[CommandContextType(InteractionContextType.BotDm, InteractionContextType.PrivateChannel, InteractionContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
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

	[SlashCommand("length", "Get the length of provided string")]
	public async Task LengthCmd(string message)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			await RespondAsync("Empty string", ephemeral: true);
			return;
		}

		await RespondAsync(message.Length.ToString());
	}

	[NsfwCommand(true)]
	[SlashCommand("nsfw-test", "Just a NSFW command")]
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
				.WithButton("nah", "nah_btn", style: ButtonStyle.Secondary)
				.Build());
	}

	[ComponentInteraction("nah_btn")]
	public async Task NahBtn()
	{
		await RespondAsync(ephemeral: true, embed: new EmbedBuilder()
				.WithDescription("Are you sure?")
				.Build(),
			components: new ComponentBuilder()
				.WithButton("i'm sure", style: ButtonStyle.Link, url: "https://www.youtube.com/watch?v=dQw4w9WgXcQ")
				.WithButton("go back", "do_not_click", style: ButtonStyle.Success)
				.Build());
    }

	[SlashCommand("options-test", "Just a test command with a lot of parameters")]
	public async Task OptionsTestAsync([Summary("parameter0", "Just a description")] string? parameter1 = null,
		[Summary("parameter1", "Just a description")]
		string? parameter2 = null,
		[Summary("parameter2", "Just a description")]
		string? parameter3 = null,
		[Summary("parameter3", "Just a description")]
		string? parameter4 = null,
		[Summary("parameter4", "Just a description")]
		string? parameter5 = null,
		[Summary("parameter5", "Just a description")]
		string? parameter6 = null,
		[Summary("parameter6", "Just a description")]
		string? parameter7 = null,
		[Summary("parameter7", "Just a description")]
		string? parameter8 = null,
		[Summary("parameter8", "Just a description")]
		string? parameter9 = null,
		[Summary("parameter9", "Just a description")]
		string? parameter10 = null,
		[Summary("parameter10", "Just a description")]
		string? parameter11 = null,
		[Summary("parameter11", "Just a description")]
		string? parameter12 = null,
		[Summary("parameter12", "Just a description")]
		string? parameter13 = null,
		[Summary("parameter13", "Just a description")]
		string? parameter14 = null,
		[Summary("parameter14", "Just a description")]
		string? parameter15 = null,
		[Summary("parameter15", "Just a description")]
		string? parameter16 = null,
		[Summary("parameter16", "Just a description")]
		string? parameter17 = null,
		[Summary("parameter17", "Just a description")]
		string? parameter18 = null,
		[Summary("parameter18", "Just a description")]
		string? parameter19 = null,
		[Summary("parameter19", "Just a description")]
		string? parameter20 = null)
	{
		await RespondAsync("Ok");
	}

	[SlashCommand("options-test-2", "Just second a test command with a lot of parameters")]
	public async Task OptionsTestAsync1([Summary("parameter0", "Just a description")] string? parameter1,
		[Summary("parameter1", "Just a description")]
		string? parameter2,
		[Summary("parameter2", "Just a description")]
		string? parameter3,
		[Summary("parameter3", "Just a description")]
		string? parameter4,
		[Summary("parameter4", "Just a description")]
		string? parameter5,
		[Summary("parameter5", "Just a description")]
		string? parameter6,
		[Summary("parameter6", "Just a description")]
		string? parameter7,
		[Summary("parameter7", "Just a description")]
		string? parameter8,
		[Summary("parameter8", "Just a description")]
		string? parameter9,
		[Summary("parameter9", "Just a description")]
		string? parameter10,
		[Summary("parameter10", "Just a description")]
		string? parameter11,
		[Summary("parameter11", "Just a description")]
		string? parameter12,
		[Summary("parameter12", "Just a description")]
		string? parameter13,
		[Summary("parameter13", "Just a description")]
		string? parameter14,
		[Summary("parameter14", "Just a description")]
		string? parameter15,
		[Summary("parameter15", "Just a description")]
		string? parameter16,
		[Summary("parameter16", "Just a description")]
		string? parameter17,
		[Summary("parameter17", "Just a description")]
		string? parameter18,
		[Summary("parameter18", "Just a description")]
		string? parameter19,
		[Summary("parameter19", "Just a description")]
		string? parameter20)
	{
		await RespondAsync("Ok");
	}

 	[SlashCommand("update-test", "TestUpdate")]
  	public async Task TestUpdateAsync()
   	{
		await RespondAsync(components: new ComponentBuilder().WithButton("UPDATE", "update-btn-0", style: ButtonStyle.Success)
  								     .WithButton("UPDATE", "deferred-update-btn-0", style: ButtonStyle.Danger).Build());
    	}

     	[ComponentInteraction("update-btn-*")]
      	public async Task UpdateBtnAsync(int count)
        {
       		var interaction = (IComponentInteraction)Context.Interaction;
	 	await interaction.UpdateAsync(x => 
   						{
   							x.Content = $"Get Updated LOL #{count} <https://www.youtube.com/watch?v=dQw4w9WgXcQ>";
							x.Components = new ComponentBuilder().WithButton("UPDATE", $"update-btn-{count + 1}")
  								                             .WithButton("DEFERRED UPDATE", $"deferred-update-btn-{count + 1}", style: ButtonStyle.Danger).Build();
	 					});
	}

 	[ComponentInteraction("deferred-update-btn-*")]
      	public async Task DeferredUpdateBtnAsync(int count)
        {
       		var interaction = (IComponentInteraction)Context.Interaction;

  		await interaction.DeferAsync();
	 	await interaction.ModifyOriginalResponseAsync(x => 
   						{
   							x.Content = $"Get Updated LOL #{count} <https://www.youtube.com/watch?v=dQw4w9WgXcQ>";
							x.Components = new ComponentBuilder().WithButton("UPDATE", $"update-btn-{count + 1}")
  								                             .WithButton("DEFERRED UPDATE", $"deferred-update-btn-{count + 1}", style: ButtonStyle.Danger).Build();
	 					});
	}

}
