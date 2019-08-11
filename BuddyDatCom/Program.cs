using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuddyDatCom
{
    class Program
    {
        static void Main(string[] args)
        {
            label.TextChanged += UpdateProgress;
            string AES_KEY = "bns_obt_kr_2014#";
            bool XOR = true;
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string name = System.IO.Path.GetFileName(codeBase);
            if (args.Length == 0 || args == null)
            {
                Console.WriteLine($"{name} -help | for a list of commands");
                Environment.Exit(0);
            }
            switch (args[0])
            {
                case("-about"):
                    Console.WriteLine("Credits to Endless, LokiReborn and ronny1982!");
                    break;
                case("-help"):
                    Console.WriteLine("-help | This list \n" +
                        "-about | Credits for the tool \n" +
                        "-compile | Compiles a dat from the input directory given to the tool \n" +
                        $"usage: {name} -compile \"C:\\fullpath\\to\\folder.dat.files\" \n" +
                        "-decompile | Decompiles a dat into the output directory given to the tool \n" +
                        $"usage: {name} -decompile \"C:\\fullpath\\to\\folder.dat\" -out \"C:\\fullpath\\to\\folder_to_extract_to\"");
                    break;
                case("-compile"):
                    if (args.Length == 2)
                    {
                        string folder = args[1];
                        if (Directory.Exists(@folder))
                        {
                            string tmp_name = new DirectoryInfo(@folder).Name;
                            var_name = tmp_name;
                            BNSDat.BNSDat temporary_worker = new BNSDat.BNSDat();
                            temporary_worker.AES_KEY = AES_KEY;
                            if (!XOR) { temporary_worker.XOR_KEY = null; }
                            action = false;
                            temporary_worker.Compress(@folder, tmp_name.Contains("64"), 1, label);
                            Console.Write($"\r Compiling {var_name} | Progress: Completed              ");
                        }
                        else
                        {
                            Console.WriteLine("The input directory does not exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please refer to -help for a quick example");
                    }
                    break;
                case ("-decompile"):
                    if (args.Length == 4)
                    {
                        string file = args[1];
                        if (File.Exists(@file))
                        {
                            if (args[2] == "-out")
                            {
                                string output = args[3];
                                string folder2 = Path.GetDirectoryName(@output);
                                if (Directory.Exists(folder2))
                                {
                                    string tmp_name = Path.GetFileName(@file);
                                    var_name = tmp_name;
                                    BNSDat.BNSDat temporary_worker = new BNSDat.BNSDat();
                                    temporary_worker.AES_KEY = AES_KEY;
                                    if (!XOR) { temporary_worker.XOR_KEY = null; }
                                    action = true;
                                    temporary_worker.Extract(@file, tmp_name.Contains("64"), label);
                                    Console.Write($"\r Decompiling {var_name} | Progress: Moving directory...    ");
                                    if (Directory.Exists(folder2 + "\\" + var_name + ".files"))
                                    {
                                        Directory.Delete(folder2 + "\\" + var_name + ".files", true);
                                    }
                                    string mov_dir = file + ".files";
                                    Directory.Move(mov_dir, folder2 + "\\" + var_name + ".files");
                                    Console.Write($"\r Decompiling {var_name} | Progress: Completed              ");
                                }
                                else
                                {
                                    Console.WriteLine("The output directory does not exist!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("You're missing the -out argument!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The input file does not exist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please refer to -help for a quick example");
                    }
                    break;
                default:
                    Console.WriteLine("Please refer to -help for a quick example");
                    break;
            }
        }

        static System.Windows.Forms.Label label = new System.Windows.Forms.Label();
        static string var_name = "";
        static bool action = false;
        private static void UpdateProgress(object sender, EventArgs e)
        {
            if (action)
                Console.Write($"\r Decompiling {var_name} | Progress: {label.Text} ");
            else
                Console.Write($"\r Compiling {var_name} | Progress: {label.Text} ");
        }
    }
}
