using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailUtils
{
    public static class MailUtils
    {

        /// <summary>
        /// Gửi Email
        /// </summary>
        /// <param name="_from">Địa chỉ email gửi</param>
        /// <param name="_to">Địa chỉ email nhận</param>
        /// <param name="_subject">Chủ đề của email</param>
        /// <param name="_body">Nội dung (hỗ trợ HTML) của email</param>
        /// <param name="client">SmtpClient - kết nối smtp để chuyển thư</param>
        /// <returns>Task</returns>
        public static async Task<string> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client)
        {
            // Tạo nội dung Email
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);


            try
            {
                await client.SendMailAsync(message);
                return "Gui mail thanh cong";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "gui mail that bai";
            }
        }


        public static async Task<string> SendGmail(string _from, string _to, string _subject, string _body, SmtpClient client)
        {
            // Tạo nội dung Email
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);

            try
            {
                await client.SendMailAsync(message);
                return "Gui mail thanh cong";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "gui mail that bai";
            }
        }
    }


}