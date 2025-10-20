using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TTNAppCore.UI.Wrapper
{

    public class TtnWrapper : ModelWrapper<Ttn>
    {
        public TtnWrapper(Ttn model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Num
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
            }
        }

        public float Amount
        {
            get { return GetValue<float>(); }
            set
            {
                SetValue(value);
            }
        }

        public string ProductName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Organization
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Car
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string CarNumber
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        //public string DrivingLicense
        //{
        //    get { return GetValue<string>(); }
        //    set
        //    {
        //        SetValue(value);
        //    }
        //}

        //public string Driver
        //{
        //    get { return GetValue<string>(); }
        //    set
        //    {
        //        SetValue(value);
        //    }
        //}

        public string LoadingPoint
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public int DriverId
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }

        //protected override IEnumerable<string> ValidateProperty(string propertyName)
        //{
        //    switch (propertyName)
        //    {
        //        case nameof(Driver):
        //            if (string.Equals(Driver, "Robot", StringComparison.OrdinalIgnoreCase))
        //            {
        //                yield return "Robots are not valid friends";
        //            }
        //            break;
        //    }
        //}


    }
}
