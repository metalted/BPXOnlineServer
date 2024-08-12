using System;
using System.Collections.Generic;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Data.Entities;

public partial class Auth : IEntity
{
    public Auth()
    {
        #region Generated Constructor
        #endregion
    }

    #region Generated Properties
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string AccessToken { get; set; } = null!;

    public decimal AccessTokenExpiry { get; set; }

    public string RefreshToken { get; set; } = null!;

    public decimal RefreshTokenExpiry { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    #endregion

    #region Generated Relationships
    public virtual User User { get; set; } = null!;

    #endregion

}
