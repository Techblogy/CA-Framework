namespace CAF.Core.Entities
{
    public abstract class UniqueEntity<T> : DBEntity
    {
        public virtual T Id { get; set; }
    }
}
