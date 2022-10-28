using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;

namespace AgerDevice.Core.ViewModels
{
    public class UnitViewModel
    {
        public Guid? Id { get; set; }
        public string? SerialNumber { get; set; }
        public bool? IsConnected { get; set; }
        public List<Guid>? PairedIds { get; set; }
        public string? Name { get; set; }

        public UnitViewModel() { }

        public UnitViewModel(Unit model) {
            Id = model.Id;
            SerialNumber = model.SerialNumber;
            IsConnected = model.IsConnected;
            PairedIds = model.ParsePairings();
            Name = model.Name;
        }
    }
}