
internal class AccessValidator
{
    public AccessValidator(TokenRequest tokenRequest)
    {
        TokenRequest = tokenRequest;
    }

    public TokenRequest TokenRequest { get; }

    internal bool IsValid()
    {
        return true;
    }
}