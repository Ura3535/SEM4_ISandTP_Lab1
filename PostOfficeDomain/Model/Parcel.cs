using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOfficeDomain.Model;

public partial class Parcel : Entity
{
    [Display(Name = "Інформація про посилку")]
    public string Info { get; set; } = null!;

    [Display(Name = "Вага посилки")]
    public double Weight { get; set; }

    [Display(Name = "Номер телефону відправника")]
    public int SenderId { get; set; }

    [Display(Name = "Номер телефону отримувача")]
    public int ReciverId { get; set; }

    [Display(Name = "Відділення відправки")]
    public int DeparturePointsId { get; set; }

    [Display(Name = "Відділення отримки")]
    public int DeliveryPointsId { get; set; }

    [Display(Name = "Ціна")]
    public int Price { get; set; }

    [Display(Name = "Статус")]
    public int StatusId { get; set; }

    [Display(Name = "Поточне місце перебування посилки")]
    public int CurrentLocationId { get; set; }

    [Display(Name = "Адреса доставки кур'єром")]
    public string? DeliveryAddress { get; set; }

    public virtual ICollection<Courier> Couriers { get; set; } = new List<Courier>();

    [Display(Name = "Поточне місце перебування посилки")]
    public virtual PostalFacility CurrentLocation { get; set; } = null!;

    [Display(Name = "Відділення отримки")]
    public virtual PostalFacility DeliveryPoints { get; set; } = null!;

    [Display(Name = "Відділення відправки")]
    public virtual PostalFacility DeparturePoints { get; set; } = null!;

    [Display(Name = "Отримувач")]
    public virtual Client Reciver { get; set; } = null!;

    [Display(Name = "Відправник")]
    public virtual Client Sender { get; set; } = null!;

    [Display(Name = "Статус")]
    public virtual ParcelStatus Status { get; set; } = null!;
}
