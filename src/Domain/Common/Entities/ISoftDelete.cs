namespace AppDomain.Common.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}