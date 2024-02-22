namespace UserAppBot.Modules;

[CommandContextType(InteractionContextType.BotDm, InteractionContextType.PrivateChannel, InteractionContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall)]
[Group("rp", "rp")]
public class SubCommandsModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("bonk", "bonk some1")]
    public async Task BonkCommand(IUser user)
    {
        await RespondAsync($"# [BONK](https://tenor.com/view/vorzek-vorzneck-oglg-og-lol-gang-gif-24901093) {user.Mention}", allowedMentions: AllowedMentions.None);
    }

    [SlashCommand("slap", "slap some1")]
    public async Task SlapCommand(IUser user)
    {
        await RespondAsync($"Slap {user.Mention}", allowedMentions: AllowedMentions.None);
    }
}