using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTNAppCore.Model
{
    public class Driver
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DrivingLicense { get; set; }

        public Ttn Ttn { get; set; }

    }
}
