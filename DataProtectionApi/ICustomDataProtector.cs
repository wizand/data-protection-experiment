public interface ICustomDataProtector
{
    public string Encrypt(string plainTextData);
    public string Decrypt(string encryptedDat);

}