using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Services
{
    public class BookInstanceService : IBookInstanceService
    {
        private readonly IBookInstanceRepository _bookInstanceRepository;

        public BookInstanceService(IBookInstanceRepository bookInstanceRepository)
        {
            _bookInstanceRepository = bookInstanceRepository;
        }

        public async Task<BookInstance?> GetBookInstance(int id)
        {
            return await _bookInstanceRepository.GetBookInstance(id);
        }

        public async Task<List<BookInstance>?> ListBookInstances()
        {
            return await _bookInstanceRepository.ListBookInstances();
        }

        public async Task<BookInstance> CreateBookInstance(BookInstance bookInstance)
        {
            return await _bookInstanceRepository.CreateBookInstance(bookInstance);
        }

        public async Task<BookInstance> UpdateBookInstance(BookInstance bookInstance)
        {
            return await _bookInstanceRepository.UpdateBookInstance(bookInstance);
        }

        public async Task<bool> DeleteBookInstance(int id)
        {
            return await _bookInstanceRepository.DeleteBookInstance(id);
        }
    }
}
