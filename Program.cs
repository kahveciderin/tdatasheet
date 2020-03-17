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
        #region constants
        const string help = "This program is a transistor datasheet downloader.\nUsage:\ntdatasheet -t BC557\tto download the datasheet of transistor BC557 or \ntdatasheet -a\tto download all\nWhen downloading, press any key to stop.\n\ntdatasheet -f\tto filter transistors downloaded\n\ntdatasheet -i\tto download the datasheets of the transistors which you have.\n\nEdit which transistors you have:\ntdatasheet -e\n\nYou can launch the program with the arguments OR you can use the built-in terminal.\n\nDo NOT forget to put this executable to an empty folder.";
        const string license = @"                                 Apache License
                           Version 2.0, January 2004
                        http://www.apache.org/licenses/

   TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION

   1. Definitions.

      ""License"" shall mean the terms and conditions for use, reproduction,
      and distribution as defined by Sections 1 through 9 of this document.

      ""Licensor"" shall mean the copyright owner or entity authorized by
      the copyright owner that is granting the License.

      ""Legal Entity"" shall mean the union of the acting entity and all
      other entities that control, are controlled by, or are under common
      control with that entity. For the purposes of this definition,
      ""control"" means (i) the power, direct or indirect, to cause the
      direction or management of such entity, whether by contract or
      otherwise, or (ii) ownership of fifty percent (50%) or more of the
      outstanding shares, or (iii) beneficial ownership of such entity.

      ""You"" (or ""Your"") shall mean an individual or Legal Entity
      exercising permissions granted by this License.

      ""Source"" form shall mean the preferred form for making modifications,
      including but not limited to software source code, documentation
      source, and configuration files.

      ""Object"" form shall mean any form resulting from mechanical
      transformation or translation of a Source form, including but
      not limited to compiled object code, generated documentation,
      and conversions to other media types.

      ""Work"" shall mean the work of authorship, whether in Source or
      Object form, made available under the License, as indicated by a
      copyright notice that is included in or attached to the work
      (an example is provided in the Appendix below).

      ""Derivative Works"" shall mean any work, whether in Source or Object
      form, that is based on (or derived from) the Work and for which the
      editorial revisions, annotations, elaborations, or other modifications
      represent, as a whole, an original work of authorship. For the purposes
      of this License, Derivative Works shall not include works that remain
      separable from, or merely link (or bind by name) to the interfaces of,
      the Work and Derivative Works thereof.

      ""Contribution"" shall mean any work of authorship, including
      the original version of the Work and any modifications or additions
      to that Work or Derivative Works thereof, that is intentionally
      submitted to Licensor for inclusion in the Work by the copyright owner
      or by an individual or Legal Entity authorized to submit on behalf of
      the copyright owner. For the purposes of this definition, ""submitted""
      means any form of electronic, verbal, or written communication sent
      to the Licensor or its representatives, including but not limited to
      communication on electronic mailing lists, source code control systems,
      and issue tracking systems that are managed by, or on behalf of, the
      Licensor for the purpose of discussing and improving the Work, but
      excluding communication that is conspicuously marked or otherwise
      designated in writing by the copyright owner as ""Not a Contribution.""

      ""Contributor"" shall mean Licensor and any individual or Legal Entity
      on behalf of whom a Contribution has been received by Licensor and
      subsequently incorporated within the Work.

   2. Grant of Copyright License. Subject to the terms and conditions of
      this License, each Contributor hereby grants to You a perpetual,
      worldwide, non-exclusive, no-charge, royalty-free, irrevocable
      copyright license to reproduce, prepare Derivative Works of,
      publicly display, publicly perform, sublicense, and distribute the
      Work and such Derivative Works in Source or Object form.

   3. Grant of Patent License. Subject to the terms and conditions of
      this License, each Contributor hereby grants to You a perpetual,
      worldwide, non-exclusive, no-charge, royalty-free, irrevocable
      (except as stated in this section) patent license to make, have made,
      use, offer to sell, sell, import, and otherwise transfer the Work,
      where such license applies only to those patent claims licensable
      by such Contributor that are necessarily infringed by their
      Contribution(s) alone or by combination of their Contribution(s)
      with the Work to which such Contribution(s) was submitted. If You
      institute patent litigation against any entity (including a
      cross-claim or counterclaim in a lawsuit) alleging that the Work
      or a Contribution incorporated within the Work constitutes direct
      or contributory patent infringement, then any patent licenses
      granted to You under this License for that Work shall terminate
      as of the date such litigation is filed.

   4. Redistribution. You may reproduce and distribute copies of the
      Work or Derivative Works thereof in any medium, with or without
      modifications, and in Source or Object form, provided that You
      meet the following conditions:

      (a) You must give any other recipients of the Work or
          Derivative Works a copy of this License; and

      (b) You must cause any modified files to carry prominent notices
          stating that You changed the files; and

      (c) You must retain, in the Source form of any Derivative Works
          that You distribute, all copyright, patent, trademark, and
          attribution notices from the Source form of the Work,
          excluding those notices that do not pertain to any part of
          the Derivative Works; and

      (d) If the Work includes a ""NOTICE"" text file as part of its
          distribution, then any Derivative Works that You distribute must
          include a readable copy of the attribution notices contained
          within such NOTICE file, excluding those notices that do not
          pertain to any part of the Derivative Works, in at least one
          of the following places: within a NOTICE text file distributed
          as part of the Derivative Works; within the Source form or
          documentation, if provided along with the Derivative Works; or,
          within a display generated by the Derivative Works, if and
          wherever such third-party notices normally appear. The contents
          of the NOTICE file are for informational purposes only and
          do not modify the License. You may add Your own attribution
          notices within Derivative Works that You distribute, alongside
          or as an addendum to the NOTICE text from the Work, provided
          that such additional attribution notices cannot be construed
          as modifying the License.

      You may add Your own copyright statement to Your modifications and
      may provide additional or different license terms and conditions
      for use, reproduction, or distribution of Your modifications, or
      for any such Derivative Works as a whole, provided Your use,
      reproduction, and distribution of the Work otherwise complies with
      the conditions stated in this License.

   5. Submission of Contributions. Unless You explicitly state otherwise,
      any Contribution intentionally submitted for inclusion in the Work
      by You to the Licensor shall be under the terms and conditions of
      this License, without any additional terms or conditions.
      Notwithstanding the above, nothing herein shall supersede or modify
      the terms of any separate license agreement you may have executed
      with Licensor regarding such Contributions.

   6. Trademarks. This License does not grant permission to use the trade
      names, trademarks, service marks, or product names of the Licensor,
      except as required for reasonable and customary use in describing the
      origin of the Work and reproducing the content of the NOTICE file.

   7. Disclaimer of Warranty. Unless required by applicable law or
      agreed to in writing, Licensor provides the Work (and each
      Contributor provides its Contributions) on an ""AS IS"" BASIS,
      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
      implied, including, without limitation, any warranties or conditions
      of TITLE, NON-INFRINGEMENT, MERCHANTABILITY, or FITNESS FOR A
      PARTICULAR PURPOSE. You are solely responsible for determining the
      appropriateness of using or redistributing the Work and assume any
      risks associated with Your exercise of permissions under this License.

   8. Limitation of Liability. In no event and under no legal theory,
      whether in tort (including negligence), contract, or otherwise,
      unless required by applicable law (such as deliberate and grossly
      negligent acts) or agreed to in writing, shall any Contributor be
      liable to You for damages, including any direct, indirect, special,
      incidental, or consequential damages of any character arising as a
      result of this License or out of the use or inability to use the
      Work (including but not limited to damages for loss of goodwill,
      work stoppage, computer failure or malfunction, or any and all
      other commercial damages or losses), even if such Contributor
      has been advised of the possibility of such damages.

   9. Accepting Warranty or Additional Liability. While redistributing
      the Work or Derivative Works thereof, You may choose to offer,
      and charge a fee for, acceptance of support, warranty, indemnity,
      or other liability obligations and/or rights consistent with this
      License. However, in accepting such obligations, You may act only
      on Your own behalf and on Your sole responsibility, not on behalf
      of any other Contributor, and only if You agree to indemnify,
      defend, and hold each Contributor harmless for any liability
      incurred by, or claims asserted against, such Contributor by reason
      of your accepting any such warranty or additional liability.

   END OF TERMS AND CONDITIONS

   APPENDIX: How to apply the Apache License to your work.

      To apply the Apache License to your work, attach the following
      boilerplate notice, with the fields enclosed by brackets ""[]""
      replaced with your own identifying information. (Don't include
      the brackets!)  The text should be enclosed in the appropriate
      comment syntax for the file format. We also recommend that a
      file or class name and description of purpose be included on the
      same ""printed page"" as the copyright notice for easier
      identification within third-party archives.

   Copyright 2020 Derin İlkcan Karakoç

   Licensed under the Apache License, Version 2.0 (the ""License"");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an ""AS IS"" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.";
        #endregion
        static string globalname;
        public static void Main(string[] args)
        {

            string download = "";
            if (args.Length == 0)
            {
                Console.WriteLine("For more information, type \"help\" or \"license\"\n");

                Console.Write("Enter commands starting with tdatasheet here.");

                while (true)
                {

                    Console.Write("\n>>> ");


                    var sel = Console.ReadLine().Split(' ');
                    if (sel[0] == "tdatasheet" || sel[0] == "tds" || sel[0] == "datasheet" || sel[0] == " tdatasheet" )
                    {
                        switch (sel[1])
                        {
                            case "-t":


                                download = sel[2];
                                downloadone(download);
                                break;

                            case "-a":
                                Console.WriteLine("Press any key while downloading to stop");

                                downloadall();
                                break;

                            case "-i":
                                updatedatabase();
                                break;

                            case "-f":
                                filter();
                                break;

                            case "-h":

                                Console.WriteLine(help);

                                break;

                            case "-e":
                                Console.WriteLine("Create a new text file named \"ihave.txt\" (without the quotes) and appent the transistors you have line by line.");

                                break;
                            case "-l":
                                Console.WriteLine(license);
                                break;
                            default:
                                Console.WriteLine("Unknown argument {0}.", sel[1]);
                                break;
                        }

                    }else if(sel[0] == "help")
                    {
                        Console.WriteLine(help);
                    }
                    else if (sel[0] == "license")
                    {
                        Console.WriteLine(license);
                    }
                    else
                    {


                        Console.WriteLine("Unknown command {0}.", sel[0]);
                    }
                }
            }
            else
            {


                Directory.CreateDirectory("rawdsdata");


                if (args[0] == "-l")
                {
                    Console.WriteLine(license);
                }
                if (args[0] == "-h")
                {
                    Console.WriteLine(help);
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
                if (Console.KeyAvailable) break;

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
            Console.Write("Material (Si/Ge) = ");
            string nmaterial = Console.ReadLine();
            Console.Write("Polarity (NPN/PNP) = ");
            string npolarity = Console.ReadLine();
            Console.Write("Maximum Collector Power Dissipation (Pc) (W) > ");
            string nmcpd = Console.ReadLine();
            Console.Write("Maximum Collector-Base Voltage |Vcb| (V) > ");
            string nmcbv = Console.ReadLine();
            Console.Write("Maximum Collector-Emitter Voltage |Vce| (V) > ");
            string nmcev = Console.ReadLine();
            Console.Write("Maximum Emitter-Base Voltage |Veb| (V) > ");
            string nmebv = Console.ReadLine();
            Console.Write("Maximum Collector Current |Ic max| (A) > ");
            string nmcc = Console.ReadLine();
            Console.Write("Max. Operating Junction Temperature (Tj) (°C) > ");
            string nmojt = Console.ReadLine();
            Console.Write("Transition Frequency (ft) (Mhz) > ");
            string nfreq = Console.ReadLine();
            Console.Write("Collector Capacitance (Cc) (pF) < ");
            string ncap = Console.ReadLine();
            Console.Write("Forward Current Transfer Ratio (hFE), MIN < ");
            string nfctr = Console.ReadLine();
            Console.Write("Noise Figure (dB) < ");
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

            int transistorcounter = 0;
            int ftransistorcounter = 0;
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

                int pFrom = ds.IndexOf("Package: ") + "Package: ".Length;
                string package = ds.Substring(pFrom, ds.Length-1 - pFrom);



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
                                                                var pr = package.Replace(",", "");
                                                                var ps = pr.Split(' ');
                                                                var exists = Array.Exists(ps, element => element == npackage);

                                                                    if (npackage == String.Empty || package == String.Empty || exists)
                                                                {

                                                                    if (onlyihave != "y")
                                                                    {

                                                                        Console.WriteLine(name);
                                                                        transistors.Add(name);
                                                                        ftransistorcounter++;
                                                                    }
                                                                    else if (contents.Contains(name))
                                                                    {
                                                                        Console.WriteLine(name);
                                                                        transistors.Add(name);
                                                                        ftransistorcounter++;
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
                transistorcounter++;
            }



            Console.WriteLine("\n{0} result(s) from {1} transistors", ftransistorcounter,transistorcounter );
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
