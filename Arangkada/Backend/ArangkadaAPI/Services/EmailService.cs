using SendGrid;
using SendGrid.Helpers.Mail;


namespace ArangkadaAPI.Services
{
    public class EmailService : IEmailService
    {

        public async Task SendEmailVerification(string? emailAddress, string? verificationCode)
        {
            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("olivein123@gmail.com", "Adrian from Arangkada");
                var subject = "Arangkada Verification Code";
                var to = new EmailAddress(emailAddress);
                var plainTextContent = verificationCode;
                var htmlContent = plainTextContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Body.ReadAsStringAsync().Result);
                Console.WriteLine(response.Headers.ToString());
                Console.WriteLine(response.IsSuccessStatusCode ? "Email queued successfully!" : "Something went wrong!");
                Console.WriteLine(apiKey.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
