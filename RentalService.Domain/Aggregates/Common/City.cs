﻿using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Domain.Aggregates.Common;

public class City
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int CountryId { get; private set; }

    private readonly List<UserBasicInfo> _userBasicInfos = new List<UserBasicInfo>();
    public IEnumerable<UserBasicInfo> UserBasicInfos => _userBasicInfos;
    

}