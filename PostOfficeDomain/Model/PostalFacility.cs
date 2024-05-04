using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOfficeDomain.Model;

public partial class PostalFacility : Entity
{
    [Display(Name = "Назва віділення")]
    public string Name { get; set; } = null!;

    [Display(Name = "Тип приміщення")]
    public int FacilityTypeId { get; set; }

    [Display(Name = "Адреса")]
    public string Address { get; set; } = null!;

    [Display(Name = "Графік роботи")]
    public string? WorkSchedule { get; set; }

    [Display(Name = "Максимальна вага посилок")]
    public double? WeightRestrictions { get; set; }

    [Display(Name = "Тип приміщення")]
    public virtual FacilityType FacilityType { get; set; } = null!;

    public virtual ICollection<Parcel> ParcelCurrentLocations { get; set; } = new List<Parcel>();

    public virtual ICollection<Parcel> ParcelDeliveryPoints { get; set; } = new List<Parcel>();

    public virtual ICollection<Parcel> ParcelDeparturePoints { get; set; } = new List<Parcel>();
}
