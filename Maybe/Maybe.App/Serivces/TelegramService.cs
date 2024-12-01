using EnsureThat;
using Maybe.Domain.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maybe.App.Serivces
{
    public class TelegramService
    {
        private readonly TelegramBotClient _botClient;

        private readonly string _channelId;
        private readonly string _notifyChannelId;

        public TelegramService(string botToken, string channelId, string notifyChannelId) 
        {
            EnsureArg.IsNotNullOrWhiteSpace(channelId, nameof(channelId));
            EnsureArg.IsNotNullOrWhiteSpace(botToken, nameof(botToken));

            _botClient = new TelegramBotClient(botToken);
            _channelId = channelId;
            _notifyChannelId = notifyChannelId;
        }

        public async Task<Message> MakePublication(Idea idea)
        {
            EnsureArg.IsNotNull(idea, nameof(idea));
            EnsureArg.IsNotNullOrWhiteSpace(idea.Name, nameof(idea.Name));
            EnsureArg.IsNotNullOrWhiteSpace(idea.Description, nameof(idea.Description));
            EnsureArg.IsNotNullOrWhiteSpace(idea.Question, nameof(idea.Question));

            return await _botClient.SendMessage(
                       chatId: _channelId,
                       text: idea.ToString(),
                       parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }

        public async Task<Message> MakeNotification(string notification)
        {
            EnsureArg.IsNotNullOrWhiteSpace(notification, nameof(notification));

            return await _botClient.SendMessage(
                chatId: _notifyChannelId,
                text: notification,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
