using System;
using System.Collections.Generic;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Data.Entities;

public partial class Blueprint : IEntity
{
    public Blueprint()
    {
        #region Generated Constructor
        #endregion
    }

    #region Generated Properties
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public List<string> Tags { get; set; } = null!;

    public string FileId { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    #endregion

    #region Generated Relationships
    public virtual User User { get; set; } = null!;

    #endregion

}
