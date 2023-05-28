using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ArangkadaAPI.Services
{
    /// <summary>
    /// Represents the service interface for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email message.
        /// </summary>
        /// <param name="emailAddress">The email address to send the email messatge to.</param>
        /// <param name="verificationCode">The verification code that will be used in authentication.</param>
        Task SendEmailVerification(string? emailAddress, string? verificationCode);
    }
}