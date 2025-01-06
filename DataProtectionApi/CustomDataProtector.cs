using Microsoft.AspNetCore.DataProtection;

public class CustomDataProtector : ICustomDataProtector
{

    private IDataProtector _dataProtector;

    public CustomDataProtector(IDataProtectionProvider provider)
    {
        _dataProtector = provider.CreateProtector("SharedKeysProtector");
    }

    public string Decrypt(string encryptedDat)
    {
        
        return _dataProtector.Unprotect(encryptedDat);
    }

    public string Encrypt(string plainTextData)
    {
        return _dataProtector.Protect(plainTextData);

    }

}