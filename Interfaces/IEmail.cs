namespace Project_Management_System.Interfaces
{
    public interface IEmail
    {
        bool SendEmail(string ToEmail, string Subject, string Content);
        string Generate2FAToken();
    }
}
