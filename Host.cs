using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

public class Host
{
    public Action<ITelegramBotClient, Update>? OnMessage;
    private TelegramBotClient _bot;  

    public Host(string token)
    {
        _bot = new TelegramBotClient(token);

    }

    public void Start()
    {
        _bot.StartReceiving(UpdateHandler, ErrorHandler);
        Console.WriteLine("Бот запущен!");
    }

    private async Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        // throw new NotImplementedException();
        Console.WriteLine("Ошибка:" + exception.Message);
        await Task.CompletedTask;
    }

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        //throw new NotImplementedException();
        Console.WriteLine($"Пришло сообцение: {update.Message?.Text ?? "[Не текст]"}");
        OnMessage?.Invoke(client, update);
        await Task.CompletedTask;
    }
}