using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Commands.Base;
using Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public sealed class SendMessageToEmailCommand : Command
    {
        public override string Name { get; init; }

        private string BotName { get; }
        private string BaseHost { get; }
        private int BasePort { get; }
        private bool BaseEnableSsl { get; }
        private string FromAddress { get; }
        private string BasePassword { get; }
        private string BaseSslProtocol { get; }

        public SendMessageToEmailCommand
        (
            string commandName, 
            IOptions<AppOptions> options, 
            IOptions<SmtpClientOptions> smtpOptions
        ) 
            : base(commandName)
        {
            Name = commandName;
            BotName = options.Value.BotName;
            
            BaseHost = smtpOptions.Value.Host;
            BasePort = smtpOptions.Value.Port;
            BaseEnableSsl = smtpOptions.Value.EnableSsl;
            BaseSslProtocol = smtpOptions.Value.SslProtocol;
            
            smtpOptions.Value.Credentials.TryGetValue("UserName", out var fromAddress);
            FromAddress = fromAddress;

            smtpOptions.Value.Credentials.TryGetValue("Password", out var basePassword);
            BasePassword = basePassword;
        }
        
        public override async Task<Message> Execute(Message message, ITelegramBotClient client)
        {
            var result = await ValidateMessage(message);
            return await client.SendTextMessageAsync(message.Chat.Id, result);
        }
        
        private async Task<string> ValidateMessage(Message message)
        {
            var text = message.Text.Split(' ');

            if (text.Length != 3)
                return  "Неверный формат данных. Введите команду в формате: \r\n /email email word";

            var toAddress = text[1];
            var body = text[2];
            var result = await SendToEmail(toAddress, body);
            
            return result;
        }

        private async Task<string> SendToEmail(string toAddress, string text)
        {
            using var client = new SmtpClient();
            try
            {
                Enum.TryParse(BaseSslProtocol, out SslProtocols sslProtocols);
                client.SslProtocols = sslProtocols;

                await client.ConnectAsync (BaseHost, BasePort, BaseEnableSsl);
                await client.AuthenticateAsync (FromAddress, BasePassword);
                await client.SendAsync(Message(toAddress, "Message from telegram bot " + BotName, text));
                
                await client.DisconnectAsync (true);
                
                Console.WriteLine("Sent Message < {0} > from {1} to {2}. Date {3}", text, FromAddress, toAddress, DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while sending message: {0}", ex.Message);
                return "Ошибка при отправке сообщения: " + ex.Message;
            }
            
            return "Письмо отправлено на почту";
        }

        private MimeMessage Message(string toAddress, string subject, string text)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("", FromAddress));
            emailMessage.To.Add(new MailboxAddress("", toAddress));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = text
            };

            return emailMessage;
        }
    }
}