namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class UserContact
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Value { get; private set; }

    


    public static UserContact CreateUserContact(string name, string value)
    {
        return new UserContact
        {
            Id = Guid.NewGuid(),
            Name = name,
            Value = value
        };
    }
}