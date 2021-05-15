using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CogneroToDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                string file = arg;

                // Attempt to accomodate for relative or absolute directories
                if (file[0] != '/' && file[1] != ':')
                {
                    file = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + file;
                }

                string filenameNoExt = file.Substring(0, file.LastIndexOf('.'));

                bool errorOccured = false;
                try
                {
                    CogneroTest test = CogneroXMLReader.ParseFile(file);
                    Test2Doc.CreateDocsFromTest(test, filenameNoExt);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("\nFAILED TO CONVERT " + file + "\nEXCEPTION:\n" + ex.Message + "\n");
                    errorOccured = true;
                }
                finally
                {
                    if (!errorOccured)
                    {
                        Console.WriteLine("CONVERT " + file + " OPERATION OK");
                    }
                }
            }
        }
    }
}
