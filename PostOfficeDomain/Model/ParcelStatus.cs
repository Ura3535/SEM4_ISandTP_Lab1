using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class ParcelStatus : Entity
{
    public string Status { get; set; } = null!;

    public virtual ICollection<Parcel> Parcels { get; set; } = new List<Parcel>();
}
