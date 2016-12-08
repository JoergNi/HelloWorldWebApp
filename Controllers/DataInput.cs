using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorldWebApp.Controllers
{
    public class DataInput: IEquatable<DataInput>
    {
        public string Text { get; set; }

        public string SelectedCountry { get; set; }


        // override object.Equals
        public override bool Equals(object obj)
        {
          

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (DataInput)obj;
            
            return Equals(other);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here

            return Text.GetHashCode() + 7 * SelectedCountry.GetHashCode();
        }

        public bool Equals(DataInput other)
        {
            return Text == other.Text && SelectedCountry == other.SelectedCountry;
        }
    }
}
