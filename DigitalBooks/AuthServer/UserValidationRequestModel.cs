using AuthServer.DatabaseEntity;

internal class UserValidationRequestModel
{
    private readonly DigitalbooksContext context = new DigitalbooksContext();

    public string UserName { get; set; }
    public string Password { get; set; }

    public bool IsValidate(string username, string password)
    {
        if (context.UserTables.Any(x => x.UserName == username && x.Password == AuthServer.EncryptDecrypt.EncodePasswordToBase64(password)))
           // AuthServer.EncryptDecrypt.EncodePasswordToBase64(password)
           { 
                return true;
        }
        else
            return false;
    }
}
