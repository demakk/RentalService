using RentalService.Dal;

namespace RentalService.Application;

public class DataContextInjector
{
    protected readonly DataContext _ctx;
    
    public DataContextInjector(DataContext ctx)
    {
        _ctx = ctx;
    }
}