using System;
using System.Threading.Tasks;
using NAudio.Wave;
using System.IO;
using System.Collections.Generic;



namespace ToWave
{

    class AudioConverter
    {
        public string OutPutFile { get; set; }
        public string Filename { get; set; }
        public string[] FilenameArray { get; set; }
        public string[] OutPutFileArray { get; set; }
        public string DestinationDirectory { get; set; }

        public AudioConverter(string filename, string outFile, string desDir)
        {
            DestinationDirectory = desDir;
            OutPutFile = outFile;
            Filename = filename;
        }
        
        public AudioConverter(List<string> filenames, string[] outFiles, string desDir)
        {
            DestinationDirectory = desDir;
            FilenameArray = ListToString(filenames);
            OutPutFileArray = outFiles;

        }

        private void ConvertFile(string Filename, string outputfile)
        {
            using (AudioFileReader reader = new AudioFileReader(Filename))
            using (WaveFileWriter writer = new WaveFileWriter(outputfile, reader.WaveFormat))
            {
                reader.CopyTo(writer);
            }

        }
        
        //Converts Multiple files at once      
        private void ConvertFiles()
        {
            int count = 0;
            foreach (string filename in FilenameArray)
            {
                ConvertFile(filename, Path.Combine(DestinationDirectory, OutPutFileArray[count]));
                count++;
            }
        }
        
        /// <summary>
        /// Converts audio files passed to AudioConverter object
        /// </summary>
        public void Convert()
        {
            if(FilenameArray.Length > 0)
            {
                ConvertFiles();
            }
            else
            {
                ConvertFile(Filename, Path.Combine(DestinationDirectory, OutPutFile));
            }
        }

        private string[] ListToString(List<string> str)
        {
            string[] names = new string[str.Capacity];
            int count = 0;

            foreach (string name in str)
            {
                names[count] = name;
                count++;
            }

            return names;
        }
    }
}
