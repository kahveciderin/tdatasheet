using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
namespace Transistor
{
    class MainClass
    {
        static string globalname;
        public static void Main(string[] args)
        {

            string download = "";
            if (args.Length == 0)
            {
                Console.WriteLine("For command line usage, do:\ntdatasheet -h");

                Console.Write("Select process:\n1)Download one\n2)Download all\n3)Update database\n4)Filter\n\n98)Help\n99)How to add to the \"i have\" transistors?");

                Console.Write("\nSelect: ");
                
                switch (Console.ReadLine())
                {
                    case "1":

                        Console.Write("Transistor name: ");
                        download = Console.ReadLine();
                        downloadone(download);
                        break;

                    case "2":
                        downloadall();
                        break;

                    case "3":
                        updatedatabase();
                        break;

                    case "4":
                        filter();
                        break;

                    case "98":

                        Console.WriteLine("This program is a transistor datasheet downloader.\nUsage:\ntdatasheet -t BC557\tto download the datasheet of transistor BC557 or \ntdatasheet -a\tto download all\n\ntdatasheet -f\tto filter transistors downloaded\n\ntdatasheet -i\tto download the datasheets of the transistors which you have.\n\nEdit which transistors you have:\ntdatasheet -e");

                        break;

                    case "99":
                        Console.WriteLine("Create a new text file named \"ihave.txt\" (without the quotes) and appent the transistors you have line by line.");

                        break;
                }
            }
            else
            {


                Directory.CreateDirectory("rawdsdata");



                if (args[0] == "-h")
                {
                    Console.WriteLine("This program is a transistor datasheet downloader.\nUsage:\ntdatasheet -t BC557\tto download the datasheet of transistor BC557 or \ntdatasheet -a\tto download all\n\ntdatasheet -f\tto filter transistors downloaded\n\ntdatasheet -i\tto download the datasheets of the transistors which you have.\n\nEdit which transistors you have:\ntdatasheet -e");

                }
                else
                if (args[0] == "-t")
                {

                    download = args[1];
                    downloadone(download);
                }
                else if (args[0] == "-a")
                {
                    downloadall();
                }
                else if (args[0] == "-e") {
                    Console.WriteLine("Create a new text file named \"ihave.txt\" (without the quotes) and appent the transistors you have line by line.");
                 }
                else if (args[0] == "-f")
                {
                    filter();
                }
                else if (args[0] == "-i")
                {
                    updatedatabase();
                }



            }
        }

        public static void updatedatabase()
        {

            string line;
            File.Delete("realihave.txt");
            StreamReader file = new StreamReader("ihave.txt");
            List<string> ihave = new List<string>();
            while ((line = file.ReadLine()) != null)
            {

                downloadone(line);
                ihave.Add(globalname);

            }

            file.Close();
            File.WriteAllLines("realihave.txt", ihave);
        }

        public static void downloadall()
        {

            File.WriteAllText("rawdsdata/ds.txt", string.Empty);

            for (int i = 1; i < 99999; i++)
            {
                string htmlCode;
                using (WebClient client = new WebClient())
                {
                    htmlCode = client.DownloadString("https://alltransistors.com/transistor.php?transistor=" + i);
                }

                Console.WriteLine("Transistor datasheet raw HTML code:\n{0}", htmlCode);

                string ds = editDatasheet(htmlCode);




                string name = getBetween(ds, "Type Designator: ", "\n");

                string material = getBetween(ds, "Material of Transistor: ", "\n");

                string polarity = getBetween(ds, "Polarity: ", "\n");

                string mcpd = getBetween(ds, "Maximum Collector Power Dissipation (Pc): ", "\n");

                string mcbv = getBetween(ds, "Maximum Collector-Base Voltage |Vcb|: ", "\n");

                string mcev = getBetween(ds, "Maximum Collector-Emitter Voltage |Vce|: ", "\n");

                string mebv = getBetween(ds, "Maximum Emitter-Base Voltage |Veb|: ", "\n");

                string mcc = getBetween(ds, "Maximum Collector Current |Ic max|: ", "\n");

                string mojt = getBetween(ds, "Max. Operating Junction Temperature (Tj): ", "\n");

                string freq = getBetween(ds, "Transition Frequency (ft): ", "\n");

                string cap = getBetween(ds, "Collector Capacitance (Cc): ", "\n");

                string fctr = getBetween(ds, "Forward Current Transfer Ratio (hFE), MIN: ", "\n");

                string noise = getBetween(ds, "Noise Figure, dB: ", "\n");

                string package = getBetween(ds, "Package: ", "\n");



                // FileStream file = new FileStream("rawdsdata/ds.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                //  file.WriteLine(name + "\n" + material + "\n" + polarity + "\n" + mcpd + "\n" + mcbv + "\n" + mcev + "\n" + mebv+"\n" + mcc +"\n" + mojt +"\n" +freq +"\n" + cap +"\n" + fctr +"\n" + noise+ "\n" + package +"\n"  );

                // file.

                Directory.CreateDirectory("datasheets");
                File.WriteAllText("datasheets/" + name + ".datasheet.txt", ds);

                Console.WriteLine("\n\nSaved datasheet to datasheets/{0}", name + ".datasheet.txt");

            }




        }
        public static void filter()
        {


            List<string> transistors = new List<string>();

            Console.WriteLine("Press enter to skip any value.");
            Console.Write("Material = ");
            string nmaterial = Console.ReadLine();
            Console.Write("Polarity = ");
            string npolarity = Console.ReadLine();
            Console.Write("Maximum Collector Power Dissipation (Pc) > ");
            string nmcpd = Console.ReadLine();
            Console.Write("Maximum Collector-Base Voltage |Vcb| > ");
            string nmcbv = Console.ReadLine();
            Console.Write("Maximum Collector-Emitter Voltage |Vce| > ");
            string nmcev = Console.ReadLine();
            Console.Write("Maximum Emitter-Base Voltage |Veb| > ");
            string nmebv = Console.ReadLine();
            Console.Write("Maximum Collector Current |Ic max| > ");
            string nmcc = Console.ReadLine();
            Console.Write("Max. Operating Junction Temperature (Tj) > ");
            string nmojt = Console.ReadLine();
            Console.Write("Transition Frequency (ft) (Mhz) > ");
            string nfreq = Console.ReadLine();
            Console.Write("Collector Capacitance (Cc) < ");
            string ncap = Console.ReadLine();
            Console.Write("Forward Current Transfer Ratio (hFE), MIN < ");
            string nfctr = Console.ReadLine();
            Console.Write("Noise Figure, dB < ");
            string nnoise = Console.ReadLine();
            Console.Write("Package = ");
            string npackage = Console.ReadLine();
            Console.Write("Do you only want to see the transistors which you have? (How to edit the transistors that I have? ->\ttdatasheet -e\t) (y-n): ");
            string onlyihave = Console.ReadLine();

            string contents = "";
            if (onlyihave == "y")
            {
                using (StreamReader sr = new StreamReader("realihave.txt"))
                {
                    contents = sr.ReadToEnd();
                }
            }
            DirectoryInfo d = new DirectoryInfo("datasheets");

            foreach (var file in d.GetFiles("*.txt"))
            {



                string ds = File.ReadAllText(file.ToString());


                string name = getBetween(ds, "Type Designator: ", "\n");

                string material = getBetween(ds, "Material of Transistor: ", "\n");

                string polarity = getBetween(ds, "Polarity: ", "\n");

                string mcpd = getBetween(ds, "Maximum Collector Power Dissipation (Pc): ", "\n");

                string mcbv = getBetween(ds, "Maximum Collector-Base Voltage |Vcb|: ", "\n");

                string mcev = getBetween(ds, "Maximum Collector-Emitter Voltage |Vce|: ", "\n");

                string mebv = getBetween(ds, "Maximum Emitter-Base Voltage |Veb|: ", "\n");

                string mcc = getBetween(ds, "Maximum Collector Current |Ic max|: ", "\n");

                string mojt = getBetween(ds, "Max. Operating Junction Temperature (Tj): ", "\n");

                string freq = getBetween(ds, "Transition Frequency (ft): ", "\n");

                string cap = getBetween(ds, "Collector Capacitance (Cc): ", "\n");

                string fctr = getBetween(ds, "Forward Current Transfer Ratio (hFE), MIN: ", "\n");

                string noise = getBetween(ds, "Noise Figure, dB: ", "\n");

                string package = getBetween(ds, "Package: ", "\n");



                mcpd = Regex.Replace(mcpd, "[^0-9,.]+", string.Empty);
                mcbv = Regex.Replace(mcbv, "[^0-9,.]+", string.Empty);
                mcev = Regex.Replace(mcev, "[^0-9,.]+", string.Empty);
                mebv = Regex.Replace(mebv, "[^0-9,.]+", string.Empty);
                mcc = Regex.Replace(mcc, "[^0-9,.]+", string.Empty);
                mojt = Regex.Replace(mojt, "[^0-9,.]+", string.Empty);
                freq = Regex.Replace(freq, "[^0-9,.]+", string.Empty);
                cap = Regex.Replace(cap, "[^0-9,.]+", string.Empty);
                fctr = Regex.Replace(fctr, "[^0-9,.]+", string.Empty);
                noise = Regex.Replace(noise, "[^0-9,.]+", string.Empty);

                if (nmaterial == String.Empty || material == String.Empty || material == nmaterial)
                {
                    if (npolarity == String.Empty || polarity == String.Empty || polarity == npolarity)
                    {
                        if (nmcpd == String.Empty || mcpd == String.Empty || float.Parse(mcpd) >= float.Parse(nmcpd))
                        {
                            if (nmcbv == String.Empty || mcbv == String.Empty || float.Parse(mcbv) >= float.Parse(nmcbv))
                            {
                                if (nmcev == String.Empty || mcev == String.Empty || float.Parse(mcev) >= float.Parse(nmcev))
                                {
                                    if (nmebv == String.Empty || mebv == String.Empty || float.Parse(mebv) >= float.Parse(nmebv))
                                    {
                                        if (nmcc == String.Empty || mcc == String.Empty || float.Parse(mcc) >= float.Parse(nmcc))
                                        {
                                            if (nmojt == String.Empty || mojt == String.Empty || float.Parse(mojt) >= float.Parse(nmojt))
                                            {
                                                if (nfreq == String.Empty || freq == String.Empty || float.Parse(freq) >= float.Parse(nfreq))
                                                {
                                                    if (ncap == String.Empty || cap == String.Empty || float.Parse(cap) <= float.Parse(ncap))
                                                    {
                                                        if (nfctr == String.Empty || fctr == String.Empty || float.Parse(fctr) <= float.Parse(nfctr))
                                                        {
                                                            if (nnoise == String.Empty || noise == String.Empty || float.Parse(noise) <= float.Parse(nnoise))
                                                            {
                                                                if (npackage == String.Empty || package == String.Empty || package == npackage)
                                                                {

                                                                    if (onlyihave != "y")
                                                                    {

                                                                        Console.WriteLine(name);
                                                                        transistors.Add(name);
                                                                    }
                                                                    else if (contents.Contains(name))
                                                                    {
                                                                        Console.WriteLine(name);
                                                                        transistors.Add(name);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }







                // Console.WriteLine(name + "\n" + material + "\n" + polarity + "\n" + mcpd + "\n" + mcbv + "\n" + mcev + "\n" + mebv + "\n" + mcc + "\n" + mojt + "\n" + freq + "\n" + cap + "\n" + fctr + "\n" + noise + "\n" + package + "\n");

            }




            File.Delete("apptransistors.txt");
            File.WriteAllText("apptransistors.txt", String.Join("\n", transistors.ToArray()));
            Console.WriteLine("\n\nSaved to apptransistors.txt");
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);

                try {
                    return strSource.Substring(Start, End - Start);
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                return "";
            }
        }



        public static string editDatasheet(string dataSheet)
        {
            string necesspart = getBetween(dataSheet, "\n Datasheet, Equivalent, Cross Reference Search</h1><p>", "<div id=\"cont");

            Console.WriteLine("Transistor datasheet necessary part:\n{0}", necesspart);

            Console.WriteLine("Editing datasheet...");

            List<char> npt = new List<char>();

            npt.AddRange(necesspart);

            npt.RemoveAt(0);


            necesspart = new string(npt.ToArray());

            necesspart = necesspart.Replace("\n", "");
            necesspart = necesspart.Replace("<p> ", "\n");
            Console.WriteLine("Edited datasheet:\n\n\n{0}", necesspart);
            return necesspart;
        }



        public static void downloadone(string transistor)
        {

            string download = transistor;

            Console.WriteLine("Downloading datasheet of {0} from site https://alltransistors.com", download);

            Console.WriteLine("Finding transistor id of {0}", download);
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://alltransistors.com/search.php?search=" + download);
            }

            Console.WriteLine("Raw HTML code:\n{0}", htmlCode);

            string tid = getBetween(htmlCode, "https://alltransistors.com/transistor.php?transistor=", ">");

            Console.WriteLine("\n\nTransistor ID: {0}", tid);


            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://alltransistors.com/transistor.php?transistor=" + tid);
            }

            Console.WriteLine("Transistor datasheet raw HTML code:\n{0}", htmlCode);


            Directory.CreateDirectory("datasheets");

            string ds = editDatasheet(htmlCode);
            string name = getBetween(ds, "Type Designator: ", "\n");

            globalname = name;
            File.WriteAllText("datasheets/" + name + ".datasheet.txt",ds);

            Console.WriteLine("\n\nSaved datasheet to datasheets/{0}", name + ".datasheet.txt");

        }
    }
}
