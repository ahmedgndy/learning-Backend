

public interface IPaymentFactory
{
    IPaymentService GetService(string provider);
}

public class PaymentFactory : IPaymentFactory
{
    private readonly IEnumerable<IPaymentService> _service;
    private readonly IHostEnvironment _env;

    public PaymentFactory(IEnumerable<IPaymentService> services, IHostEnvironment env)
    {
        _service = services;
        _env = env;
    }
    public IPaymentService GetService(string provider)
    {
        var service = _service.FirstOrDefault( s =>
            s.Name.Equals(provider, StringComparison.OrdinalIgnoreCase)
        );
        
        if (service == null)
        {
            throw new NotImplementedException();
        }
        return service;
    }
}