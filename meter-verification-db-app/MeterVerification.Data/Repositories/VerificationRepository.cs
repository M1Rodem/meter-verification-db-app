using MeterVerification.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MeterVerification.Core.Interfaces;
using MeterVerification.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterVerification.Data.Repositories
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly AppDbContext _context;

        public VerificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Verification>> GetAllWithDetailsAsync()
        {
            return await _context.Verifications
                .Include(v => v.Organization)
                .Include(v => v.DeviceType)
                .Include(v => v.DeviceModification)
                .ToListAsync();
        }

        public async Task AddAsync(Verification entity)
        {
            await _context.Verifications.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
