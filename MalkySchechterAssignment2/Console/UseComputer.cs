using MalkySchechterAssignment2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console
{
    class UseComputer
    {
        Computer defaultPrototype = new Computer("Default", false, 3000, 1200, new Int32?[] { 3, 2, 1 });
        Computer userPrototype = null;
        Computer[] computers = new Computer[10];
        int numComputers = 0;

        #region main
        static void Main(string[] args)
        {
            UseComputer instance = new UseComputer();
            int option;
            do
            {
                option = menu();

                switch (option)
                {
                    case 1:
                        instance.computers[instance.numComputers++] = createComputer();
                        break;
                    case 2:
                        instance.userPrototype = createComputer();
                        break;
                    case 3:
                        System.Console.WriteLine("Enter index of the computer: ");
                        int index = int.Parse(System.Console.ReadLine());
                        Computer comp = instance.computers[index] ?? instance.defaultPrototype;
                        System.Console.WriteLine(comp.ToString());

                        break;
                    case 4:

                        if (instance.numComputers == 0)
                        {
                            System.Console.WriteLine("there are no computers at this time.");
                        }
                        else
                        {
                            System.Console.WriteLine(summaryOfCompsInArray(instance).ToString());
                        }

                        break;

                    case 5:
                        System.Console.WriteLine("Please enter the first index of the range of computers: ");
                        int beg = int.Parse(System.Console.ReadLine());
                        System.Console.WriteLine("Please enter the last index of the range of computers: ");
                        int end = int.Parse(System.Console.ReadLine());

                        Computer[] specificComputers = new List<Computer>(instance.computers).GetRange(beg, end - beg + 1).ToArray();

                        System.Console.WriteLine(specificSummary(specificComputers, instance.userPrototype ?? instance.defaultPrototype));

                        break;
                }
                System.Console.WriteLine("");
            } while (option != 0);
        }
        #endregion

        #region methods
        public static int menu()
        {
            System.Console.WriteLine("Please Select a Menu Item:" +
                "\n1. Add a Computer"
                + "\n2. Specify Prototype Computer"
                + "\n3. Summary of Specific Computer by Index"
                + "\n4. Summary of Statistics of all Computers"
                + "\n5. Summary of Statistics of Specified Range of Computers"
                + "\n0. Exit");

            string option = System.Console.ReadLine();


            //As per specs validation unnecessary

            return int.Parse(option);
        }
        
        private static Computer createComputer()
        {
            string id;
            bool hasAntenna;
            double capacity;
            int ram;
            int?[] swWithLicenses;

            System.Console.WriteLine("Please enter the computer's specs separated by commas:");
            string specs = System.Console.ReadLine();

            System.Console.WriteLine("Is there any software installed on the machine? ");
            string swInstalled = System.Console.ReadLine();
            bool isInstalled;
            isInstalled = swInstalled.ToLower().Equals("true") ? true : false;

            string[] splitVals = specs.Split(',');
            id = splitVals[0];
            hasAntenna = bool.Parse(splitVals[1]);
            capacity = double.Parse(splitVals[2]);
            ram = int.Parse(splitVals[3]);

            swWithLicenses = null;
            if (isInstalled)
            {
                System.Console.WriteLine("Please write the number of licenses for up to 5 pieces of software separated by commas:");
                string softwareLics = System.Console.ReadLine();

                string[] lics = softwareLics.Split(',');
                swWithLicenses = new int?[5];
                int i = 0;
                foreach (string numLic in lics)
                {
                    swWithLicenses[i++] = int.Parse(numLic);
                }

            }

            return new Computer(id, hasAntenna, capacity, ram, swWithLicenses);
        }
        #endregion

        #region summaries
        private static StringBuilder specificSummary(Computer[] specificComputers, Computer defaultPrototype)
        {
            StringBuilder sb = new StringBuilder();
            int sumRAM = 0, hdCompatible = 0, antennaApplicable = 0, numWithAntenna = 0, machinesWithSWInstalled = 0; ;
            int[] totNumLicsPerSWForAllMachines = new int[5];
            int[] totNumLicsPerMachine = new int[specificComputers.Length];
            double hdCap = 0.0;

            for (int i = 0; i < specificComputers.Length; i++)
            {
                specificComputers[i] = specificComputers[i] ?? defaultPrototype;

                sumRAM += specificComputers[i].RAM;

                if (specificComputers[i].HasAntenna != null)
                {
                    antennaApplicable++;
                    if ((bool)specificComputers[i].HasAntenna)
                    {
                        numWithAntenna++;
                    }
                }

                if (specificComputers[i].HdCapacity != null)
                {
                    hdCompatible++;
                    hdCap += (double)specificComputers[i].HdCapacity;
                }


                int ctr = 0;
                if (specificComputers[i].NumLicenses > 0)
                {
                    machinesWithSWInstalled++;
                    while (ctr < specificComputers[i].Licenses.Length)
                    {
                        totNumLicsPerSWForAllMachines[ctr] += specificComputers[i].Licenses[ctr] ?? 0;
                        ctr++;
                    }
                }


                if (specificComputers[i].NumLicenses > 0)
                {
                    int totLic = 0;
                    foreach (int? lic in specificComputers[i].Licenses)
                    {
                        totLic += lic ?? 0;
                    }
                    totNumLicsPerMachine[i] = totLic;
                }


            }
            double avgRAM = sumRAM / specificComputers.Length;
            decimal percAntenna = numWithAntenna * 100 / antennaApplicable;
            double avgHD = hdCap / hdCompatible;

            sb.Append("Average Statistics for all computers: \n");
            sb.Append($"Average RAM: {avgRAM}" + "\n");
            sb.Append($"Average HD Capacity: {avgHD}" + "\n");
            sb.Append($"Percentage of compatible devices that have antenna: {percAntenna}" + "%\n");

            int finalCount = 0;

            for (int ctr = 0; ctr < totNumLicsPerMachine.Length; ctr++)
            {
                finalCount = totNumLicsPerMachine[ctr];
            }
            if (machinesWithSWInstalled > 0)
            {
                sb.Append($"Average Number of Licenses Per Machine (only including computers with SW Installed):  {finalCount / machinesWithSWInstalled}" + "\n");
                

                for (int ctr = 0; ctr < totNumLicsPerSWForAllMachines.Length; ctr++)
                {
                        //reset:
                        finalCount = 0;

                        finalCount += (int)totNumLicsPerSWForAllMachines[ctr];
                    
                    //average will be truncated int
                    sb.Append($"Average Number of Licenses for Program {ctr + 1}: { finalCount / machinesWithSWInstalled}" + "\n");
                }
            }
            else
            {
                sb.Append($"Software not Installed on any of these computers.");
            }
            return sb;
        }

        private static StringBuilder summaryOfCompsInArray(UseComputer instance)
        {
            StringBuilder sb = new StringBuilder();
            int sumRAM = 0, hdCompatible = 0, antennaApplicable = 0, numWithAntenna = 0, machinesWithSWInstalled = 0;
            int[] totNumLicsPerSWForAllMachines = new int[5];
            int[] totNumLicsPerMachine = new int[instance.numComputers];
            double hdCap = 0.0;

            for (int i = 0; i < instance.numComputers; i++)
            {
                sumRAM += instance.computers[i].RAM;

                if (instance.computers[i].HasAntenna != null)
                {
                    antennaApplicable++;
                    if ((bool)instance.computers[i].HasAntenna)
                    {
                        numWithAntenna++;
                    }
                }

                if (instance.computers[i].HdCapacity != null)
                {
                    hdCompatible++;
                    hdCap += (double)instance.computers[i].HdCapacity;
                }


                int ctr = 0;
                if (instance.computers[i].NumLicenses > 0)
                {
                    machinesWithSWInstalled++;
                    while (ctr < instance.computers[i].Licenses.Length)
                    {
                        totNumLicsPerSWForAllMachines[ctr] += instance.computers[i].Licenses[ctr] ?? 0;
                        ctr++;
                    }
                }


                if (instance.computers[i].NumLicenses > 0)
                {
                    int totLic = 0;
                    foreach (int? lic in instance.computers[i].Licenses)
                    {
                        totLic += lic ?? 0;
                    }
                    totNumLicsPerMachine[i] = totLic;
                }


            }
            double avgRAM = sumRAM / instance.numComputers;
            decimal percAntenna = numWithAntenna * 100 / antennaApplicable;
            double avgHD = hdCap / hdCompatible;

            sb.Append("Average Statistics for all computers: \n");
            sb.Append($"Average RAM: {avgRAM}" + "\n");
            sb.Append($"Average HD Capacity: {avgHD}" + "\n");
            sb.Append($"Percentage of compatible devices that have antenna: {percAntenna}" + "%\n");

            int finalCount = 0;
            for (int ctr = 0; ctr < totNumLicsPerSWForAllMachines.Length; ctr++)
            {

                finalCount = (int)totNumLicsPerSWForAllMachines[ctr];

                //average will be truncated int
                sb.Append($"Average Number of Licenses for Program {ctr + 1}: { finalCount / machinesWithSWInstalled}" + "\n");
            }
        

            if (machinesWithSWInstalled > 0)
            {
                //average will be truncated int
                sb.Append($"Average Number of Licenses Per Machine:  {finalCount / machinesWithSWInstalled}" + "\n");

                for (int ctr = 0; ctr < totNumLicsPerSWForAllMachines.Length; ctr++)
                {

                    //reset:
                    finalCount = 0;

                    for (int compNum = 0; compNum < instance.numComputers; compNum++)
                    {

                        finalCount += totNumLicsPerSWForAllMachines[ctr];
                        
                    }

                    sb.Append($"Average Number of Licenses for Program {ctr + 1}: { finalCount / machinesWithSWInstalled}" + "\n");
                }
            }
            else
            {
                sb.Append($"Software not Installed on any of these computers.");
            }

                return sb;
            }
            #endregion
        }
    }
