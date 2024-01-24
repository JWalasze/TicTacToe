using System.Net.Mime;
using System.Text;
using Lib.Dtos;
using Lib.Dtos.Extensions;
using Lib.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using webapi.Hubs.Static;

namespace webapi.Hubs;

public class GameHub : Hub
{
    private static readonly ICollection<string> WaitingPlayers = new HashSet<string>();
    private static readonly IDictionary<string, GameInfo> GamesInfo = new Dictionary<string, GameInfo>();

    public async Task FindGame(int playerId, string playerUsername, ILogger<GameHub> logger)
    {
        var playerConnectionId = Context.ConnectionId;
        string? opponentConnectionId = null;
        
        lock (WaitingPlayers)
        {
            if (WaitingPlayers.Count is not 0)
            {
                var randomPlayer = RandomGenerator.Draw(0, WaitingPlayers.Count - 1);
                opponentConnectionId = WaitingPlayers.ToList()[randomPlayer];

                WaitingPlayers.Remove(opponentConnectionId);
            }
            else
            {
                WaitingPlayers.Add(playerConnectionId);
            }
        }

        lock (GamesInfo)
        {
            var playerGameInfo = new GameInfoBuilder()
                .WithPlayerId(playerId)
                .WithPlayerUsername(playerUsername)
                .Build();

            GamesInfo.Add(playerConnectionId, playerGameInfo);
        }

        if (opponentConnectionId is not null)
        {
            logger.LogInformation("Game is being prepared.");

            var gameId = GameLogic.CreateUniqueGameId();
            var (piece1, piece2) = GameLogic.DrawRandomPiece();

            await Groups.AddToGroupAsync(playerConnectionId, gameId);
            await Groups.AddToGroupAsync(opponentConnectionId, gameId);

            GameInfo? playerGameInfo;
            GameInfo? opponentGameInfo;

            lock (GamesInfo)
            {
                GamesInfo.TryGetValue(playerConnectionId, out playerGameInfo);
                GamesInfo.TryGetValue(opponentConnectionId, out opponentGameInfo);

                if (playerGameInfo is null || opponentGameInfo is null)
                {
                    logger.LogError("Error during preparing the game ({gameId}) for: {conn1} and {conn2}", gameId, playerConnectionId, opponentConnectionId);
                    throw new Exception("Couldn't establish a game.");
                }

                playerGameInfo.GameId = gameId;
                playerGameInfo.OpponentConnectionId = opponentConnectionId;
                playerGameInfo.OpponentId = opponentGameInfo.PlayerId;
                playerGameInfo.OpponentUsername = opponentGameInfo.PlayerUsername;
                playerGameInfo.OpponentPiece = piece2;
                playerGameInfo.PlayerPiece = piece1;
               
                opponentGameInfo.GameId = gameId;
                opponentGameInfo.OpponentConnectionId = playerConnectionId;
                opponentGameInfo.OpponentId = playerGameInfo.PlayerId;
                opponentGameInfo.OpponentUsername = playerGameInfo.PlayerUsername;
                opponentGameInfo.OpponentPiece = piece1;
                opponentGameInfo.PlayerPiece = piece2;
            }

            await Clients.Client(playerConnectionId).SendAsync("GetGameDetails", JsonConvert.SerializeObject(playerGameInfo));
            await Clients.Client(opponentConnectionId).SendAsync("GetGameDetails", JsonConvert.SerializeObject(opponentGameInfo));

            logger.LogInformation("Details have been sent to players");
        }
    }

    
    public async Task UpdateBoardAfterMove(string updatedBoard, string whoMadeMove, ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        var deserializedBoard = GameLogic.DeserializeBoard(updatedBoard) ?? throw new JsonException("Deserialization of BOARD can't be performed!");
        var deserializedWhoMadeMove = GameLogic.DeserializeWhoMadeMove(whoMadeMove) ?? throw new JsonException("Deserialization of WHO_MADE_MOVE can't be performed!");

        var whoIsNext = GameLogic.WhoWillBeNext(deserializedWhoMadeMove);
        var whoHasWon = GameLogic.SearchForTheWinner(deserializedBoard);

        var playerConnectionId = Context.ConnectionId;
        GameInfo? playerGameInfo;

        lock (GamesInfo)
        {
            GamesInfo.TryGetValue(Context.ConnectionId, out playerGameInfo);

            if (playerGameInfo is null)
            {
                logger.LogError("Error: Invalid connection ID or the game does not exist!");
                throw new Exception("Couldn't update the game!");
            }

            if (playerGameInfo.HaveNullValues())
            {
                logger.LogError("Error: Some values of GameInfo are invalid!!");
                throw new Exception("Couldn't update the game!");
            }
        }

        if (whoHasWon is not null)
        {
            var httpClient = httpClientFactory.CreateClient();
            var winnerId = GameLogic.MatchWinnerPieceToPlayerId((Piece)whoHasWon, playerGameInfo);

            var content = new StringContent(
                JsonConvert.SerializeObject(new GameUpdateInfoDto {GameId = playerGameInfo.GameId!, WinnerId = winnerId}),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            //Options pattern
            var responseMessage = await httpClient.PatchAsync("https://localhost:7166/api/Game/UpdateGameForWinner", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                logger.LogError("Error during calling the API: {errorMessage}", responseMessage.ToString());
            }

            var newGameStatus = GameLogic.ReturnGameStatusBasedOnWhoWon((Piece)whoHasWon);
            var gameStatus = JsonConvert.SerializeObject(newGameStatus);

            await Clients.Group(playerGameInfo.GameId!).SendAsync("MadeMove", updatedBoard, null, gameStatus);

            await Groups.RemoveFromGroupAsync(playerConnectionId, playerGameInfo.GameId!);
            await Groups.RemoveFromGroupAsync(playerGameInfo.OpponentConnectionId!, playerGameInfo.GameId!);

            logger.LogInformation("In the game {gameId} we have a winner ({whoHasWon}) with ID: {winnerId}", playerGameInfo.GameId, whoHasWon, winnerId);
            return;
        }

        var isBoardFull = GameLogic.CheckIfBoardIsFull(deserializedBoard);
        if (isBoardFull)
        {
            var httpClient = httpClientFactory.CreateClient();

            var content = new StringContent(
                JsonConvert.SerializeObject(new GameUpdateInfoDto { GameId = playerGameInfo.GameId! }),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            //Options pattern
            var responseMessage = await httpClient.PatchAsync("https://localhost:7166/api/Game/UpdateGameForWinner", content);
            if (!responseMessage.IsSuccessStatusCode)
            {
                logger.LogError("Error during calling the API: {errorMessage}", responseMessage.ToString());
            }

            var gameStatus = JsonConvert.SerializeObject(GameStatus.Draw);

            await Clients.Group(playerGameInfo.GameId!).SendAsync("MadeMove", updatedBoard, null, gameStatus);

            await Groups.RemoveFromGroupAsync(playerConnectionId, playerGameInfo.GameId!);
            await Groups.RemoveFromGroupAsync(playerGameInfo.OpponentConnectionId!, playerGameInfo.GameId!);

            logger.LogInformation("In the game {gameId} we have a draw", playerGameInfo.GameId);
            return;
        }
        
        var serializedWhoIsNext = JsonConvert.SerializeObject(whoIsNext);
        var serializedGameStatus = JsonConvert.SerializeObject(GameStatus.StillInGame);

        await Clients.Group(playerGameInfo.GameId!).SendAsync("MadeMove", updatedBoard, serializedWhoIsNext, serializedGameStatus);

        logger.LogInformation("Method 'UpdateBoardAfterMove' has been invoked.");
    }

    
    public async Task SendPlayerReadiness(ILogger<GameHub> logger, IHttpClientFactory httpClientFactory)
    {
        var playerConnectionId = Context.ConnectionId;
        string? opponentConnectionId;
        GameInfo? playerGameInfo;
        GameInfo? opponentGameInfo;

        lock (GamesInfo)
        {
            GamesInfo.TryGetValue(playerConnectionId, out playerGameInfo);

            if (playerGameInfo is null)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                throw new Exception("Couldn't establish a game.");
            }

            opponentConnectionId = playerGameInfo.OpponentConnectionId;

            if (opponentConnectionId is null)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                throw new Exception("Couldn't establish a game.");
            }

            GamesInfo.TryGetValue(opponentConnectionId, out opponentGameInfo);

            if (opponentGameInfo is null)
            {
                logger.LogError("Error: Invalid group name or player does not exist!");
                throw new Exception("Couldn't establish a game.");
            }

            if (playerGameInfo.HaveNullValues())
            {
                logger.LogError("Error: Some values of GameInfo are invalid!!");
                throw new Exception("Couldn't update the game!");
            }

            playerGameInfo.IsPlayerReady = true;
            opponentGameInfo.IsOpponentReady = true;

            logger.LogInformation("We have a ready player: {playerConnectionId} in a game {gameId}", playerConnectionId, playerGameInfo.GameId);

            if (!playerGameInfo.IsPlayerReady || !opponentGameInfo.IsPlayerReady)
            {
                logger.LogInformation("Opponent for {playerConnectionId} is still not ready.", playerConnectionId);
                return;
            }
        }

        if (playerGameInfo.HaveNullValues() || opponentGameInfo.HaveNullValues())
        {
            logger.LogError("Error: Invalid group name or player does not exist!");
            throw new Exception("Couldn't establish a game.");
        }

        await Clients.Group(playerGameInfo.GameId!).SendAsync("StartGame", 
            $"In game with identifier: {playerGameInfo.GameId} we have: {playerConnectionId} and {opponentConnectionId}");

        logger.LogInformation("Game for group: {groupName} is starting now.", playerGameInfo.GameId);

        var httpClient = httpClientFactory.CreateClient();
        var gameToBeSaved = new GameInitialStateDto()
        {
            GameId = playerGameInfo.GameId!,
            Player1Id = (int)playerGameInfo.PlayerId!,
            Player2Id = (int)opponentGameInfo.PlayerId!,
            Player1Piece = (Piece)playerGameInfo.PlayerPiece!,
            Player2Piece = (Piece)opponentGameInfo.PlayerPiece!
        };

        var content = new StringContent(
                JsonConvert.SerializeObject(gameToBeSaved),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        
        var responseMessage = await httpClient.PostAsync("https://localhost:7166/api/Game/SaveInitialGameState", content);

        if (!responseMessage.IsSuccessStatusCode)
        {
            logger.LogError("Error during calling the API: {errorMessage}", responseMessage.ToString());
        }
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        lock (WaitingPlayers)
        {
            WaitingPlayers.Remove(Context.ConnectionId);
        }

        lock (GamesInfo)
        {
            GamesInfo.Remove(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}