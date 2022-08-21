
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Services;

public partial class BotUpdateHandler : IUpdateHandler
{
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;


    public BotUpdateHandler(
        ILogger<BotUpdateHandler> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient,
                                        Exception exception,
                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Error ocured with Telegram bot : {e.Message}", exception);

        return Task.CompletedTask;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient,
                                        Update update,
                                        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update type {update.Type} received ", update.Type);

        if (update.Type == UpdateType.Message)
        {
            return HandleMessageAsync(botClient, update.Message, cancellationToken);
        }
        else
        {
            return HandleUnknownUpdate(botClient, update, cancellationToken);
        }

    }

    private async Task HandleUnknownUpdate(ITelegramBotClient botClient,
                                     Update update,
                                     CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update type {update.Type} received ", update.Type);

          await botClient.SendTextMessageAsync(
               chatId: update.Message!.Chat.Id,
               text: $"Bunday malumot qabul qilmayman ifodani kiriting",
               parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
               cancellationToken: cancellationToken);
    }
}

