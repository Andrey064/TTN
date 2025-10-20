using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TTNAppCore.Model
{
    public class Ttn
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        [Required]
        public string Num { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public float Amount { get; set; }
        public string ProductName { get; set; }
        public string Organization { get; set; }
        public string Car { get; set; }
        public string CarNumber { get; set; }
        
        //[Required(ErrorMessage = "Номер удостоверения обязательное поле")]
        //public string DrivingLicense { get; set; }
        //public string Driver { get; set; }
        public string LoadingPoint { get; set; }

        public int DriverId { get; set; }
        public Driver CurrentDriver { get; set; }

    }
}
