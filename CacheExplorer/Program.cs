using System;
using System.IO;
using IdSharp.Tagging.ID3v2;

namespace CacheExplorer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // C:\Users\cent082\AppData\Local\Google\Chrome\User Data\Default\Cache
            string searchDirectory = "";
            string destinationDirectory = "";

            if (args.Length > 0)
                searchDirectory = args[0];
            if (args.Length > 1)
                destinationDirectory = args[1];

            if (String.IsNullOrWhiteSpace(searchDirectory) || String.IsNullOrWhiteSpace(destinationDirectory))
            {
                searchDirectory = String.Format(@"C:\Users\{0}\AppData\Local\Google\Chrome\User Data\Default\Cache",
                                                Environment.UserName);
                destinationDirectory = String.Format(@"C:\Users\{0}\Desktop\tt-rip-{1}",
                                                     Environment.UserName, DateTime.Now.ToString("MMddyyyy"));

                Console.WriteLine("Where do you want to look? (" + searchDirectory + ")");

                string temp = Console.ReadLine();
                searchDirectory = String.IsNullOrEmpty(temp) ? searchDirectory : temp;

                Console.WriteLine(String.Format("Where do you want to move the files to? ({0})", destinationDirectory));
                temp = Console.ReadLine();
                destinationDirectory = String.IsNullOrEmpty(temp) ? destinationDirectory : temp;
            }

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            var directory = new DirectoryInfo(searchDirectory);
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo fileInfo in files)
            {
                Console.Write(fileInfo.Name);

                var tag = new ID3v2Tag(fileInfo.FullName);
                if (!String.IsNullOrWhiteSpace(tag.Title))
                {
                    Console.WriteLine(" -> " + tag.Title);
                    fileInfo.CopyTo(String.Format("{0}/{1}.mp3", destinationDirectory, tag.Title), true);
                }
            }

            Console.ReadLine();
        }
    }
}