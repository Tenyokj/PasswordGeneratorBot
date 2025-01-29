using System.Collections;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types;


internal class Program
{
    private static Dictionary<long, string> userNames = new Dictionary<long, string>(); // Словарь для хранения имени каждого пользователя
    
    private static void Main()
    {
        Host TenyokjBot = new Host("7594240714:AAErWeybbQ8xyMilLCsjA1VyC4wpXYpfr6k");
        TenyokjBot.Start();
        TenyokjBot.OnMessage += OnMessage;
        Console.ReadLine();
    }

    static string[] BlockMessages = { "Fuck", "fuck", "Bitch", "bitch", "Сука", "сука", "Блять", "блять", "Пиздец", "пиздец"};
    

    private static async void OnMessage(ITelegramBotClient client, Update update)
    {
        if (update.Message == null || update.Message.Text == null || update.Message.Chat == null)
            return; // Проверка на null

        long chatId = update.Message.Chat.Id;

        // Проверка на оскорбления
        if (BlockMessages.Any(word => string.Equals(update.Message.Text, word, StringComparison.OrdinalIgnoreCase)))
        {
            await client.SendMessage(chatId, "ban", replyParameters: update.Message.MessageId);
            return;
        }

        // Установка имени по умолчанию
        if (!userNames.ContainsKey(chatId))
        {
            userNames[chatId] = "Dustin"; // Имя по умолчанию
        }

        string botName = userNames[chatId]; // Получаем текущее имя бота
        // Обработка команд
        switch (update.Message.Text)
        {

            case "/start":
                await client.SendMessage(chatId, "Добро пожаловать, как я могу к вам обращатся?", replyParameters: update.Message.MessageId);
                break;

            case "/clear":
                await client.SendMessage(chatId, "К сожалению, Telegram API не позволяет очистить чат. Вы можете начать новый или очистить историю чата вручную.", replyParameters: update.Message.MessageId);
                break;

            case "/help":
                await client.SendMessage(chatId, "Что вас интересует?\nКоманды: /commands\nОбратная связь: /feedback\nОпции: /options\nПравила пользования: /rules\nАвторские права: /copyright", replyParameters: update.Message.MessageId);
                break;

            case "/options":
                await client.SendMessage(chatId, "Опции:\nСмена имени: /change_bot_name\nСмена языка: /change_bot_language", replyParameters: update.Message.MessageId);
                break;

            case "/commands":
                await client.SendMessage(chatId, $"Мои команды:\n/start - старт чата\n/hey_{botName} - обратиться к боту\n/clear - помощь с очисткой чата\n/help - помощь\n/feedback - обратная связь\n/options - опции\n/rules - правила пользования\n/copyright - авторские права", replyParameters: update.Message.MessageId);
                break;

            case "/change_bot_name":
                await client.SendMessage(chatId, "Выберите имя:\nДастин: /change_to_Dustin\nКейт: /change_to_Kate\nДругое: /change_to_TenyokjBot", replyParameters: update.Message.MessageId);
                break;

            case "/change_to_Dustin":
                userNames[chatId] = "Dustin";
                await client.SendMessage(chatId, "Имя бота изменено на 'Дастин'!", replyParameters: update.Message.MessageId);
                break;

            case "/change_to_Kate":
                userNames[chatId] = "Kate";
                await client.SendMessage(chatId, "Имя бота изменено на 'Кейт'!", replyParameters: update.Message.MessageId);
                break;

            case "/change_to_TenyokjBot":
                userNames[chatId] = "TenyokjBot";
                await client.SendMessage(chatId, "Имя бота изменено на 'TenyokjBot'!", replyParameters: update.Message.MessageId);
                break;

            case "Как тебя зовут?":
                await client.SendMessage(chatId, $"Ты можешь посмотреть имя этого чата вверху, в левом углу!\nНо, если тебе так будет удобней то, можешь обращаться ко мне на имя, {botName}!\nЕсли ты хочешь меня о чем-то спросить то, пиши команду: /hey_{botName}", replyParameters: update.Message.MessageId);
                break;

            case "Кто твой создатель?":
            case "Кто твой разработчик?":
            case "Кто тебя сделал?":
            case "Кто тебя разработал?":
                await client.SendMessage(chatId, "Меня разработал человек, который носит псевдоним 'Tenyokj', к сожалению больше об этом человеке я ничего не знаю.", replyParameters: update.Message.MessageId);
                break;

            case "Сколько тебе лет?":
                await client.SendMessage(chatId, "Я не могу ответить на ваш вопрос, я всего лишь бот и у меня нет возраста как у человеческих факторов.", replyParameters: update.Message.MessageId);
                break;

            case "Кто ты?":
            case "Кто ты такой?":
            case "Какова твоя задача?":
                await client.SendMessage(chatId, "Я ЧатБот 'TenyokjBot', разработчик создал меня для того чтобы я отвечал на ваши вопросы, по его Игре 'Shrek's Basement'!", replyParameters: update.Message.MessageId);
                break;

            case "Пока":
            case "До-свидания!":
                await client.SendMessage(chatId, "До-свидания, хорошего дня,ночи!", replyParameters: update.Message.MessageId);
                break;
            case "Привет!":
                await client.SendMessage(chatId, "Привет, как твои дела?", replyParameters: update.Message.MessageId);
                break;
            case "У меня все хорошо, а у тебе как?":
                await client.SendMessage(chatId, "Я рад что у тебя все хорошо, а у меня всегда все отлично!", replyParameters: update.Message.MessageId);
                break;
            case "У меня все плохо, а у тебя как дела?":
            case "У меня не очень, а у тебя как дела?":
            case "У меня так-себе, а у тебя как дела?":
                await client.SendMessage(chatId, "У меня все всегда отлично! А что у тебя стряслось?", replyParameters: update.Message.MessageId);
                break;
            case "Можешь скинуть свой исходный код?":
                await client.SendMessage(chatId, "К сожелению я не могу скинуть свой исходный код в Телеграм но, он есть на GitHub моего разработчика, для того чтобы получить ссылку на GitHub, вам нужно прочесть авторские права, для этого введите команду: /copyright", replyParameters: update.Message.MessageId);
                break;
            case "Я прочитал авторские права.":
            case "Я прочитал авторские права и все принял.":
                await client.SendMessage(chatId, "Спасибо, вот ссылка на мой исходный код https://github/TenyokjBot", replyParameters: update.Message.MessageId);
                break;
            case "Как у тебя дела?":
            case "Как ты?":
            case "Как твои дела?":
                await client.SendMessage(chatId, "У меня всегда всё отлично!", replyParameters: update.Message.MessageId);
                break;
            case "Ты типо ChatGpt?":
            case "Ты подобие ChatGpr?":
            case "Ты как ChatGpt?":
                await client.SendMessage(chatId, "Нет, я не подобие ChatGpt или различных нейросетей, ChatGpt - это искусственный интелект (ИИ), а я чат-бот и я не могу считывать информацию из разных источников как это может ChatGpt.\nЯ закодирован на определеные команды и ничего больше!\nВсе ответы которые ты видел, придуманы не мной, а написаны разработчиком и ты не сможешь спросить у меня что угодно, у меня есть определеные команды и запросы на которые я могу отвечать но, ничего больше и если ты меня спросиш о чем нибудь чего нету в моем коде то, я просто отвечу тебе таким же сообщением, а если ты пришлешь мне гиф, картинку или стикер то, я отвечу '[не текст].\nНа многое я не способен, я это первый подобный проэкт моего разработчика, и я не имею много команд и запросов на которые я смог бы отвечать так что много-го я сделать не смогу.", replyParameters: update.Message.MessageId);
                break;

            case "/rules":
                await client.SendMessage(chatId, "Правила пользования:\n1. Запрещены все слова вне цензуры!\n2. Не говорить слова оскорбительного характера в сторону бота!\n3. Не говорить слова оскорбительного характера в сторону владельца бота!\n4. Не отсылать бессмысленные сообщения!\n\nЕсли пользователь не будет придерживаться второго и третьего правила, он будет выгнан из чата.\nЕсли пользователь не будет придерживаться первого и четвёртого правила, он получит предупреждение, а при повторении будет также выгнан из чата!", replyParameters: update.Message.MessageId);
                break;
            
            case "/copyright":
                await client.SendMessage(chatId, "Авторские права (c) 2025 Tenyokj. Все права защищены.\n Данный чатбот, включая исходный код, дизайн и функциональность, является интеллектуальной собственностью автора под псевдонимом Tenyokj.\n\nРазработка и публикация данного чатбота осуществлены в соответствии с авторским правом и законодательством о защите интеллектуальной собственности.\n\nЗапрещается:\n   Использовать исходный код или функциональность в\n    коммерческих целях без письменного согласия автора.\n   Вносить изменения и распространять изменённую версию без\n   разрешения.\n\nРазрешается:\n    Использовать чатбота для личных целей и взаимодействия в\n    рамках предоставленных возможностей.\n\nЛюбое нарушение данных условий может повлечь за собой ответственность в соответствии с действующим законодательством.\nКонтакт: Если у вас есть вопросы или предложения, свяжитесь с автором через указаную почту av7794257@gmail.com.", replyParameters: update.Message.MessageId);
                break;

            default:
                // Обработка команды hey_<botName>
                if (update.Message.Text.StartsWith($"/hey_"))
                {
                    if (update.Message.Text == $"/hey_{botName}")
                    {
                        await client.SendMessage(chatId, "Я вас слушаю!", replyParameters: update.Message.MessageId);
                    }
                }
                else
                {
                    // Ответ на общий текст
                    await client.SendMessage(chatId, update.Message.Text ?? "[Не текст]", replyParameters: update.Message.MessageId);
                }
                break;
        }
    }
}

