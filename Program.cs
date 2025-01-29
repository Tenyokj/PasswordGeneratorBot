
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;

internal class Program
{
    private static Dictionary<long, UserState> userStates = new(); // Состояния пользователей

    private static void Main()
    {
        Host botHost = new("8156853077:AAGf19r8lgxqhkuyUnSWXiYAcaYwgppBrSg");
        botHost.Start();
        botHost.OnMessage += OnMessage;
        Console.ReadLine();
    }

    private static async void OnMessage(ITelegramBotClient client, Update update)
    {
        if (update.Message == null || update.Message.Text == null || update.Message.Chat == null)
            return;

        long chatId = update.Message.Chat.Id;
        string messageText = update.Message.Text;

        if (!userStates.ContainsKey(chatId))
            userStates[chatId] = new UserState();

        UserState state = userStates[chatId];

        if (state.IsGeneratingPassword)
        {
            await HandlePasswordGeneration(client, chatId, messageText, state);
            return;
        }

        switch (messageText)
        {
            case "/start":
                await client.SendMessage(chatId, "Добро пожаловать! Введите /generatePassword, чтобы создать пароль.");
                break;

            case "/generatePassword":
                state.IsGeneratingPassword = true;
                state.Step = PasswordStep.Length;
                await client.SendMessage(chatId, "Введите длину пароля:");
                break;

            case "/copyright":
                await client.SendMessage(chatId, "Авторские права (c) 2025 Tenyokj. Все права защищены.\n Данный чатбот, включая исходный код, дизайн и функциональность, является интеллектуальной собственностью автора под псевдонимом Tenyokj.\n\nРазработка и публикация данного чатбота осуществлены в соответствии с авторским правом и законодательством о защите интеллектуальной собственности.\n\nЗапрещается:\n   Использовать исходный код или функциональность в\n    коммерческих целях без письменного согласия автора.\n   Вносить изменения и распространять изменённую версию без\n   разрешения.\n\nРазрешается:\n    Использовать чатбота для личных целей и взаимодействия в\n    рамках предоставленных возможностей.\n\nЛюбое нарушение данных условий может повлечь за собой ответственность в соответствии с действующим законодательством.\nКонтакт: Если у вас есть вопросы или предложения, свяжитесь с автором через указаную почту av7794257@gmail.com.", replyParameters: update.Message.MessageId);
                break;

            case "/commands":
                await client.SendMessage(chatId, $"Мои команды:\n/start - старт чата\n/clear - помощь с очисткой чата\n/help - помощь\n/copyright - авторские права\n/generatePassword - сгенерировать пароль", replyParameters: update.Message.MessageId);
                break;

            default:
                await client.SendMessage(chatId, "Неизвестная команда. Введите /start или /generatePassword.");
                break;
        }
    }

    private static async Task HandlePasswordGeneration(ITelegramBotClient client, long chatId, string messageText, UserState state)
    {
        switch (state.Step)
        {
            case PasswordStep.Length:
                if (int.TryParse(messageText, out int length) && length > 0)
                {
                    state.PasswordLength = length;
                    state.Step = PasswordStep.UseUppercase;
                    await client.SendMessage(chatId, "Использовать заглавные буквы? (y/n)");
                }
                else
                {
                    await client.SendMessage(chatId, "Введите корректное число больше 0.");
                }
                break;

            case PasswordStep.UseUppercase:
                state.UseUppercase = messageText.ToLower() == "y";
                state.Step = PasswordStep.UseRussianLetters;
                await client.SendMessage(chatId, "Использовать русские буквы? (y/n)");
                break;

            case PasswordStep.UseRussianLetters:
                state.UseRussianLetters = messageText.ToLower() == "y";
                state.Step = PasswordStep.UseNumbers;
                await client.SendMessage(chatId, "Использовать цифры? (y/n)");
                break;

            case PasswordStep.UseNumbers:
                state.UseNumbers = messageText.ToLower() == "y";
                state.Step = PasswordStep.UseSpecialChars;
                await client.SendMessage(chatId, "Использовать специальные символы? (y/n)");
                break;

            case PasswordStep.UseSpecialChars:
                state.UseSpecialChars = messageText.ToLower() == "y";
                state.IsGeneratingPassword = false;

                string password = GeneratePassword(state.PasswordLength, state.UseUppercase, state.UseRussianLetters, state.UseNumbers, state.UseSpecialChars);
                await client.SendMessage(chatId, $"Сгенерированный пароль: {password}");
                break;
        }
    }

    private static string GeneratePassword(int length, bool useUppercase, bool useRussianLetters, bool useNumbers, bool useSpecialChars)
    {
        string lowercase = "abcdefghijklmnopqrstuvwxyz";
        string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string russian = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
        string uppercaserussian = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        string numbers = "0123456789";
        string specialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        StringBuilder characterPool = new(lowercase);
        if (useUppercase) characterPool.Append(uppercase);
        if (useRussianLetters)
        {
            characterPool.Append(russian);
            if (useUppercase) characterPool.Append(uppercaserussian);
        }
        if (useNumbers) characterPool.Append(numbers);
        if (useSpecialChars) characterPool.Append(specialChars);

        if (characterPool.Length == 0)
            throw new ArgumentException("Вы не выбрали ни одного типа символов для пароля!");

        StringBuilder password = new();
        Random random = new();
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characterPool.Length);
            password.Append(characterPool[index]);
        }

        return password.ToString();
    }

    
}

// Вспомогательные классы
internal class UserState
{
    public bool IsGeneratingPassword { get; set; }
    public PasswordStep Step { get; set; }
    public int PasswordLength { get; set; }
    public bool UseUppercase { get; set; }
    public bool UseRussianLetters { get; set; }
    public bool UseNumbers { get; set; }
    public bool UseSpecialChars { get; set; }
}

internal enum PasswordStep
{
    Length,
    UseUppercase,
    UseRussianLetters,
    UseNumbers,
    UseSpecialChars
}

