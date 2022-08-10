using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Core.Repositories;

namespace AgerDevice.Managers
{
    public class UnitManager
    {
        private readonly IUnitRepository _unitRepository;

        public UnitManager(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task CreateAsync(Unit unit)
        {
            await _unitRepository.CreateAsync(unit);
        }

        public async Task<PagedResult<Unit>> QueryAsync(UnitQuery query = null)
        {
            return await _unitRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(Unit unit)
        {
            await _unitRepository.UpdateAsync(unit);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _unitRepository.DeleteAsync(userId);
        }

        public async Task MarkDeleted(Unit unit)
        {
            unit.IsDeleted = true;
            await _unitRepository.UpdateAsync(unit);
        }
    }
}
