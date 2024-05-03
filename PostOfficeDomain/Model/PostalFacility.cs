using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class PostalFacility : Entity
{
    public string Name { get; set; } = null!;

    public int FacilityTypeId { get; set; }

    public string Address { get; set; } = null!;

    public string? WorkSchedule { get; set; }

    public double? WeightRestrictions { get; set; }

    public virtual FacilityType FacilityType { get; set; } = null!;

    public virtual ICollection<Parcel> ParcelCurrentLocations { get; set; } = new List<Parcel>();

    public virtual ICollection<Parcel> ParcelDeliveryPoints { get; set; } = new List<Parcel>();

    public virtual ICollection<Parcel> ParcelDeparturePoints { get; set; } = new List<Parcel>();
}
