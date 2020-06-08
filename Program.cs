using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            processAll(args[0], args[1], Boolean.Parse(args[2]));
            Console.ReadLine();
        }
        static void processAll(String rootDirectory, String outputPath, bool sub)
        {
            File.WriteAllText(outputPath, "");
            String[] system = new String[1];
            if (sub == false)
            {
                try  
                {
                    system = Directory.GetFiles(rootDirectory, "*", SearchOption.TopDirectoryOnly); 
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               
            }
            else if (sub == true)
            {
                try  
                {
                   system = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories); 
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            foreach (String folder in system)
            {
                if (File.Exists(folder))
                {
                    processFile(folder, outputPath);
                }
            }          
        }
        static void processFile(String file, String outPath)
        {
            String fullPath = Path.GetFullPath(file);
            var info = new FileInfo(file);
            String type = "";
            String contents = File.ReadAllText(fullPath);
            //print path, file type and md5 hash if pdf or jpg, else do nothing 
            if (!contents.StartsWith("0xFFD8") && !contents.StartsWith("0x25504446"))
            {
                return;
            }
            if (contents.StartsWith("0xFFD8"))//JPG
            {
               type = "JPG";
            }
            else if (contents.StartsWith("0x25504446"))//PDF           
            { 
                type = "PDF";  
            }
            string newLine = Environment.NewLine;
            File.AppendAllText(outPath, newLine + "Full Path: " + fullPath);
            File.AppendAllText(outPath, "   File Type: " + type);
            File.AppendAllText(outPath, "   MD5 Hash: " + GetMD5HashFromFile(file));
        }
       
        //copied method from online
        protected static string GetMD5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
    }
}
