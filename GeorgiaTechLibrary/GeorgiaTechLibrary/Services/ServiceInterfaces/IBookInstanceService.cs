using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IBookInstanceService
    {
        Task<BookInstance?> GetBookInstance(int id);
        Task<List<BookInstance>?> ListBookInstances();
        Task<BookInstance> CreateBookInstance(BookInstance bookInstance);
        Task<BookInstance> UpdateBookInstance(BookInstance bookInstance);
        Task<bool> DeleteBookInstance(int id);
    }
}
