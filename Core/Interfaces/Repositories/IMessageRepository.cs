using Core.Entities;
using Core.ViewModel.MessageViewModels;

namespace Core.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<bool> ExistsAsync(int id);
}