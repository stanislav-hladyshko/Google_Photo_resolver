using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        public static Dictionary<string, string> FilesAndMetaDataDictionary = new Dictionary<string, string>();
        private static readonly string generalDirectoryPath = @"f:\FirstPart\";
        static void Main(string[] args)
        {
            GetSubDirectories(generalDirectoryPath);
            WriteToFileMethod(FilesAndMetaDataDictionary);

            Console.WriteLine("Finished, press any key");
            Console.Read();
        }

        public static void GetSubDirectories(string generalDirectoryPath)
        {
            string[] subdirectoriesEntries = Directory.GetDirectories(generalDirectoryPath);
            foreach (string t in subdirectoriesEntries)
            {
                OneDirectoryResolver(t);
                Console.WriteLine(t + ": ");
            }
        }

        public static void OneDirectoryResolver(string oneDirectoryPath)
        {
            string[] files = Directory.GetFiles(oneDirectoryPath);

            List<string> metadataFilesList = new List<string>();
            List<string> filesList = new List<string>();

            foreach (string t in files)
            {
                if (t.EndsWith(".json"))
                    metadataFilesList.Add(t);
                if (t.EndsWith(".jpg"))
                    filesList.Add(t);
            }
            FilesAndMetaDataDictionary = DuplicateFinder(filesList, metadataFilesList);
        }

        public static Dictionary<string, string> DuplicateFinder(List<string> filesList, List<string> metadataFilesList)
        {
            for (int i = 0; i < filesList.Count; i++)
            {
                for (int j = 0; j < metadataFilesList.Count; j++)
                {
                    try
                    {
                        if (metadataFilesList[j].Contains(filesList[i])
                            && FilesAndMetaDataDictionary.ContainsKey(filesList[i]) != true
                            && FilesAndMetaDataDictionary.ContainsKey(metadataFilesList[j]) != true)
                        {
                            FilesAndMetaDataDictionary.Add(filesList[i], metadataFilesList[j]);
                            metadataFilesList.RemoveAt(j);
                            filesList.RemoveAt(i);
                        }
                    }
                    catch (SystemException exception)
                    {
                        Console.WriteLine("DuplicateFinderException");
                    }
                }
            }
            return FilesAndMetaDataDictionary;
        }

        public static void WriteToFileMethod(Dictionary<string, string> dictionary)
        {
            foreach (var valuePair in FilesAndMetaDataDictionary)
                File.AppendAllText(@"C:\Users\stani\Desktop\new.txt", valuePair.Key + ": " + valuePair.Value + "\n");
        }

        public static void MetadataEditor(Dictionary<string, string> dictionary)
        {
            DateTime dateTime = new DateTime(1999, 10, 20);
            json
            foreach (var VARIABLE in FilesAndMetaDataDictionary)
            {
                VARIABLE.
                File.SetCreationTime(VARIABLE.Key, dateTime);
                File.SetLastWriteTime(VARIABLE.Key, dateTime);
                File.SetLastAccessTimeUtc(VARIABLE.Key, dateTime);

            }


        }

    }
}
