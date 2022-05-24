namespace Application.Extentions;

public interface IServiceRequest<in TRequest, TResponse> where TRequest : IServiceResponse<TResponse>
{
    Task<TResponse> Execute(TRequest request);
}