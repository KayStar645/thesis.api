namespace Core.Application.Features.Payments.Commands.BasePayment
{
    public interface IBasePayment
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
    }
}
