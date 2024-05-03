using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class Courier : Entity
{
    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public int? ParcelId { get; set; }

    public virtual Parcel? Parcel { get; set; }
}
