
namespace EcommerceProject.Application.Interface.Infrastructure
{
    public interface IEventBus
    {
        void Publish<T>(T @event);
    }
}
