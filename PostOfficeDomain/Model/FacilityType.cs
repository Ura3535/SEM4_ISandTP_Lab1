using System;
using System.Collections.Generic;

namespace PostOfficeDomain.Model;

public partial class FacilityType : Entity
{
    public string Type { get; set; } = null!;

    public virtual ICollection<PostalFacility> PostalFacilities { get; set; } = new List<PostalFacility>();
}
