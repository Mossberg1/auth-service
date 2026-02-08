namespace Auth.Interfaces
{
    public interface IHashService
    {
        string Hash(string str);
        bool Verify(string str, string hash);
    }
}
