namespace CAF.Core.Interface
{
    public interface ICreateUser
    {
        string CreateUserId { get; set; }
    }
    public interface ICreateUserId
    {
        long? CreateUserId { get; set; }
    }
}
