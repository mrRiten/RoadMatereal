using RoadMatereal.Models;

namespace RoadMatereal.Services
{
    public class EmailService(RedisStore redisStore, EmailSender emailSender) : BackgroundService
    {
        private readonly RedisStore _redisStore = redisStore;
        private readonly EmailSender _emailSender = emailSender;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string emailListKey = "emails";

            while (!stoppingToken.IsCancellationRequested)
            {
                var emails = _redisStore.ListRange(emailListKey);

                foreach (var email in emails)
                { 
                    // Send email to the recipient
                    await _emailSender.SendEmailAsync("sender@example.com", email, "Subject", "Body");

                    // Remove the email from the list
                    _redisStore.ListLeftPop(emailListKey);
                }

                // Wait for a while before checking the list again
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
