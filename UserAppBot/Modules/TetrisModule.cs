using UserAppBot.Services;

namespace DiscordNetTemplate.Modules;

[CommandContextType(InteractionContextType.BotDm, InteractionContextType.PrivateChannel, InteractionContextType.Guild)]
[IntegrationType(ApplicationIntegrationType.UserInstall)]
public class TetrisModule(TetrisService tetris) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("tetris", "Start a game of tetris")]
    public async Task StartTetrisAsync([Summary("ephemeral", "Sets whether the game should be private")] bool ephemeral = false)
    {
        var game = tetris.StartGame(Context.Interaction);

        await RespondAsync(components: game.Build(), ephemeral: ephemeral, allowedMentions: AllowedMentions.None);
    }

    [ComponentInteraction("tetris-stop-*")]
    public async Task StopTetrisAsync(string stringGameId)
    {
        var gameId = Guid.Parse(stringGameId);
        var game = tetris.GetGame(gameId);
        if (game is null)
        {
            await RespondAsync("Unknown game", ephemeral: true);
            return;
        }

        if (Context.User.Id != game.UserId)
        {
            await RespondAsync("This game is not yours!", ephemeral: true);
            return;
        }

        var interaction = (IComponentInteraction)Context.Interaction;
        game.EndGame();
        await game.UpdateInteraction(interaction);
    }

    [ComponentInteraction("tetris-move-*-*")]
    public async Task MoveTetrisAsync(string move, string stringGameId)
    {
        var gameId = Guid.Parse(stringGameId);
        var game = tetris.GetGame(gameId);
        if (game is null)
        {
            await RespondAsync("Unknown game", ephemeral: true);
            return;
        }

        if (Context.User.Id != game.UserId)
        {
            await RespondAsync("This game is not yours!", ephemeral: true);
            return;
        }

        game.MoveFigure(move switch { "left" => Movement.Left, "right" => Movement.Right });

        var interaction = (IComponentInteraction)Context.Interaction;
        await game.UpdateInteraction(interaction);
    }

    [ComponentInteraction("tetris-down-*")]
    public async Task MoveDownTetrisAsync(string stringGameId)
    {
        var gameId = Guid.Parse(stringGameId);
        var game = tetris.GetGame(gameId);
        if (game is null)
        {
            await RespondAsync("Unknown game", ephemeral: true);
            return;
        }

        if (Context.User.Id != game.UserId)
        {
            await RespondAsync("This game is not yours!", ephemeral: true);
            return;
        }

        while (game.CanMoveFigure(Movement.Down))
            game.MoveFigure(Movement.Down);
        var interaction = (IComponentInteraction)Context.Interaction;
        await game.UpdateInteraction(interaction);
    }

    [ComponentInteraction("tetris-rotate-*")]
    public async Task RotateTetrisAsync(string stringGameId)
    {
        var gameId = Guid.Parse(stringGameId);
        var game = tetris.GetGame(gameId);
        if (game is null)
        {
            await RespondAsync("Unknown game", ephemeral: true);
            return;
        }

        if (Context.User.Id != game.UserId)
        {
            await RespondAsync("This game is not yours!", ephemeral: true);
            return;
        }

        game.MoveFigure(Movement.Rotate);

        var interaction = (IComponentInteraction)Context.Interaction;
        await game.UpdateInteraction(interaction);
    }
}
