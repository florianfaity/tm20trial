using System.Text;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Domain.Service;

public class EncryptionService: IEncryptionService
{
    public string GeneratePassword()
    {
        var password = new StringBuilder();
        var random = new Random();

        bool nonAlphanumeric = true;
        bool digit = true;
        bool lowercase = true;
        bool uppercase = true;

        while (password.Length < 10)
        {
            char c = (char)random.Next(32, 126);

            password.Append(c);

            if (char.IsDigit(c))
            {
                digit = false;
            }
            else if (char.IsLower(c))
            {
                lowercase = false;
            }
            else if (char.IsUpper(c))
            {
                uppercase = false;
            }
            else if (!char.IsLetterOrDigit(c))
            {
                nonAlphanumeric = false;
            }
        }

        if (nonAlphanumeric)
        {
            password.Append((char)random.Next(33, 48));
        }
        if (digit)
        {
            password.Append((char)random.Next(48, 58));
        }
        if (lowercase)
        {
            password.Append((char)random.Next(97, 123));
        }
        if (uppercase)
        {
            password.Append((char)random.Next(65, 91));
        }

        return password.ToString();
    }

}
