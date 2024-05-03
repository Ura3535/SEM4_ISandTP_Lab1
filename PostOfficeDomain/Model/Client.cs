using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class Client : Entity
{
    public string Name { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Parcel> ParcelRecivers { get; set; } = new List<Parcel>();

    public virtual ICollection<Parcel> ParcelSenders { get; set; } = new List<Parcel>();
}
