
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Services;
public partial class BotUpdateHandler
{
    private async Task HandleMessageAsync(
        ITelegramBotClient botClient,
        Message? message,
        CancellationToken cancellationToken)
    {
        var from = message?.From;

        _logger.LogInformation("Received message from {from.FirstName} ", from?.FirstName);

        var handler = message?.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(botClient, message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken),
        };

        await handler;
    }
    private async Task HandleUnknownMessageAsync(ITelegramBotClient botClient,
                                           Message? message,
                                           CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received message type {message.Type}", message?.Type);

         await botClient.SendTextMessageAsync(
               chatId: message!.Chat.Id,
               text: $"🧐Bunday malumot qabul qilmayman ifodani to'g'ri kiriting",
               parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
               cancellationToken: cancellationToken);

    }

    private async Task HandleTextMessageAsync(ITelegramBotClient botClient,
                                              Message? message,
                                              CancellationToken cancellationToken)
    {
        _logger.LogInformation("From: {from.Firstname} : {message.Text}   ", message?.From?.FirstName, message?.Text);

        var text = message?.Text;

        if (text == "/start" || text == "start")
        {
            await botClient.SendTextMessageAsync(
               chatId: message!.Chat.Id,
               text: $"👋Salom `{message?.From?.FirstName}` \n\n Calculate 🤖 botga xush kelibsiz\n\n" +
                      "bu yerga matematik amallar ifodasini kiritsangiz men sizga hisoblab beraman, ifodani quyidagicha kiriting\n\n" +
                      " 2 ^ 4 / 3   yoki  (3 * 8)/ 4 ",
               parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
               cancellationToken: cancellationToken);
        }
        else if (text[text.Length - 1] == '=')
        {
            await botClient.SendTextMessageAsync(
                chatId: message!.Chat.Id,
                text: $" '=' yozmasdan kiriting",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        else if (char.IsDigit(text![0]) || text[0] == '(')
        {
            string exp = CountExp(text);

            await botClient.SendTextMessageAsync(
                chatId: message!.Chat.Id,
                text: $"javob {exp}",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        else
        {
            await botClient.SendTextMessageAsync(
              chatId: message!.Chat.Id,
              text: $"🤦‍♂️ ifodani to'g'ri kiriting",
              parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

    }
}

