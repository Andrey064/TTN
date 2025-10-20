using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;

namespace TTNAppCore.UI.Wrapper
{
    public class DriverWrapper : ModelWrapper<Driver>
    {
        public DriverWrapper(Driver model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Name { 
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string DrivingLicense
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
