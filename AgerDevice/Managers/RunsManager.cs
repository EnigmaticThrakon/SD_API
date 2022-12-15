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
    public class RunsManager
    {
        private readonly IRunsRepository _runsRepository;

        public RunsManager(IRunsRepository runsRepository)
        {
            _runsRepository = runsRepository;
        }

        public async Task CreateAsync(Run run)
        {
            await _runsRepository.CreateAsync(run);
        }

        public async Task<PagedResult<Run>> QueryAsync(RunQuery? query = null)
        {
            return await _runsRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(Run run)
        {
            await _runsRepository.UpdateAsync(run);
        }
    }
}
