
using System.Linq;

namespace AddressValidation.Models
{
    public class Address
    {
        public string Name { get; set; }
        public string[] Line { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ZipExt { get; set; }
        public string Country { get; set; }

        // These equality overrides are needed for tests
        protected bool Equals(Address other)
        {
            return string.Equals(Name, other.Name) && Line.SequenceEqual(other.Line) && string.Equals(City, other.City) &&
                   string.Equals(State, other.State) && string.Equals(Zip, other.Zip) &&
                   string.Equals(ZipExt, other.ZipExt) && string.Equals(Country, other.Country);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Line != null ? Line.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Zip != null ? Zip.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ZipExt != null ? ZipExt.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Country != null ? Country.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Address) obj);
        }
    }
}