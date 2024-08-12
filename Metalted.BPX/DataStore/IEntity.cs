namespace Metalted.BPX.DataStore;

public interface IEntity
{
    int Id { get; set; }
    DateTime DateCreated { get; set; }
    DateTime? DateUpdated { get; set; }
}
