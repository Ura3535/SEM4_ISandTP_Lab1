using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class Parcel : Entity
{
    public string Info { get; set; } = null!;

    public double Weight { get; set; }

    public int SenderId { get; set; }

    public int ReciverId { get; set; }

    public int DeparturePointsId { get; set; }

    public int DeliveryPointsId { get; set; }

    public int Price { get; set; }

    public int StatusId { get; set; }

    public int CurrentLocationId { get; set; }

    public string? DeliveryAddress { get; set; }

    public virtual ICollection<Courier> Couriers { get; set; } = new List<Courier>();

    public virtual PostalFacility CurrentLocation { get; set; } = null!;

    public virtual PostalFacility DeliveryPoints { get; set; } = null!;

    public virtual PostalFacility DeparturePoints { get; set; } = null!;

    public virtual Client Reciver { get; set; } = null!;

    public virtual Client Sender { get; set; } = null!;

    public virtual ParcelStatus Status { get; set; } = null!;
}
