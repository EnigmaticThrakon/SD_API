using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Repositories;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Managers
{
    public class RunDataManager
    {
        private readonly IRunDataRepository _runDataRepository;

        public RunDataManager(IRunDataRepository runDataRepository)
        {
            _runDataRepository = runDataRepository;
        }

        public async Task CreateAsync(RunData run_data)
        {
            await _runDataRepository.CreateAsync(run_data);
        }

        public async Task<PagedResult<RunData>> QueryAsync(RunDataQuery? query = null)
        {
            return await _runDataRepository.QueryAsync(query);
        }
    }
}
