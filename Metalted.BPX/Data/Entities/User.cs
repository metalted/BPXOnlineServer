using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Data.Entities;

public partial class User : IEntity
{
    public User()
    {
        #region Generated Constructor
        Auths = new HashSet<Auth>();
        Blueprints = new HashSet<Blueprint>();
        #endregion
    }

    #region Generated Properties
    public int Id { get; set; }

    public decimal SteamId { get; set; }

    public string SteamName { get; set; } = null!;

    public bool Banned { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    #endregion

    #region Generated Relationships
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual ICollection<Auth> Auths { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual ICollection<Blueprint> Blueprints { get; set; }

    #endregion

}
