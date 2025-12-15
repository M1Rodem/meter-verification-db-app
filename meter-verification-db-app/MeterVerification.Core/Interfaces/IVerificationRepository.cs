using MeterVerification.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Interfaces
{
    public interface IVerificationRepository
    {
        Task<IEnumerable<Verification>> GetAllWithDetailsAsync(); // все данные с деталями
        Task AddAsync(Verification entity); // добавлять тестовые данные
    }
}