using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class Unit : IBase
    {
        public Guid Id { get; set; }
        public DateTime? Modified { get; set; }
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConnected { get; set; }
        public string ConnectionId { get; set; }
        public string PairedIds { get; set; }
        public string? Name { get; set; }
        public bool IsAcquisitioning { get; set; }

        public Unit()
        {
            SerialNumber = String.Empty;
            ConnectionId = String.Empty;
            PairedIds = Newtonsoft.Json.JsonConvert.SerializeObject(new List<Guid>());
        }

        public Unit(List<Guid> pairings)
        {
            SerialNumber = String.Empty;
            ConnectionId = String.Empty;
            PairedIds = SerializePairings(pairings);
        }

        public string SerializePairings(List<Guid> pairings)
        {
            if(pairings != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(pairings);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<Guid>());
            }
        }

        public List<Guid> ParsePairings()
        {
            List<Guid> returnValue = new List<Guid>();
            if (!String.IsNullOrEmpty(PairedIds))
            {
                returnValue = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(PairedIds);
            }

            return returnValue;
        }

        public void UpdatePairings(Guid id, bool add)
        {
            List<Guid> tempList = new List<Guid>();
            if (!String.IsNullOrEmpty(PairedIds))
            {
                tempList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(PairedIds);
            }

            if (add)
            {
                tempList.Add(id);
            }
            else
            {
                tempList.Remove(id);
            }

            PairedIds = Newtonsoft.Json.JsonConvert.SerializeObject(tempList.Distinct().ToList());
        }
    }
}
