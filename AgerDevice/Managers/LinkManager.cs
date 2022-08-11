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
    public class LinkManager
    {
        private readonly ILinkRepository _linkRepository;

        public LinkManager(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task CreateAsync(Link link)
        {
            await _linkRepository.CreateAsync(link);
        }

        public async Task<PagedResult<Link>> QueryAsync(LinkQuery query = null)
        {
            return await _linkRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(Link link)
        {
            await _linkRepository.UpdateAsync(link);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _linkRepository.DeleteAsync(userId);
        }
    }
}
