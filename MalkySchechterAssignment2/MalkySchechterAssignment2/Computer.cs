using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MalkySchechterAssignment2
{
    public class Computer
    {

        #region properties
        readonly string id;
        public string Id
        {
            get
            {
                return id;
            }
        }

        private bool? hasAntenna;
        public bool? HasAntenna
        {
            get
            {
                return hasAntenna;
            }
            set
            {
                hasAntenna = value;
            }
        }

        private double? hdCapacity;
        public double? HdCapacity
        {
            get
            {
                return hdCapacity;
            }

            set
            {
                if (!(value < 0))
                {
                    this.hdCapacity = value;
                }
                else throw new Exception("Negative Numbers Not Allowed.");
            }
        }

        private int numLicenses = 0;
        public int NumLicenses
        {
            get { return numLicenses; }
        }

        private int?[] licenses;
        public int?[] Licenses
        {
            get
            {
            return licenses;
            }
            set
            {
                if(value == null)
                {
                    licenses = value;
                    numLicenses = -1;
                }
                else if (value.Length <= 5)
                {
                    licenses = value;

                    foreach (int? lic in licenses)
                    {
                        if (lic != 0 && lic != null)
                        {
                            numLicenses++;
                        }
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        private Int32 ram;
        public Int32 RAM
        {
            get
            {
                Int32 availRAM = ram;

                if (hasAntenna != null && (bool)hasAntenna)
                {
                    availRAM -= 100;
                }
                else
                {
                    availRAM -= 50;
                }

                availRAM -= (10 * numLicenses);

                return availRAM;
            }

            set
            {
                if (value >= 1000)
                {
                    ram = value;
                }
                else
                {
                    throw new Exception("Insufficient amount of memory.");
                }
            }
        }
        #endregion

        #region constructor
        public Computer(string id, bool? hasAntenna, double hdCapacity, int ram, int?[] softwareWithLicenses/*up to 5*/)
        {
            this.id = id;
            this.HasAntenna = hasAntenna;
            this.RAM = ram;
            this.HdCapacity = hdCapacity;
            

            if (this.Licenses == null)
            {
                this.Licenses = softwareWithLicenses;
            }
        }
        #endregion

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.Append("\nID: " + this.Id);
            if (this.HasAntenna.HasValue)
            {
                sb.Append("\nHas Antenna: " + this.HasAntenna);
            } else sb.Append("\nHas Antenna: N/A");
            
            sb.Append("\nHard Drive Capacity: " + this.HdCapacity);
            sb.Append("\nRAM: " + this.RAM);
            if (numLicenses > -1)
            {
                sb.Append("\nNumber of Licensed Software: " + numLicenses);
            }
            else
            {
                sb.Append("\nNo Software installed on this machine");
            }
            if (NumLicenses > 0)
            {
                for (int i = 0; i < NumLicenses; i++)
                {
                    sb.Append("\nSoftware " + (i+1) + ": Num Licenses " + licenses[i]);
                }
            }
            return sb.ToString();
        }
    }
}
