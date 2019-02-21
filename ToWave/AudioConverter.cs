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
        public bool IsDoneConverting { get; set; }

        private WaveFileWriter writer;
        private string FileBeingConverted;

        public AudioConverter(string filename, string outFile, string desDir)
        {
            IsDoneConverting = true;//Set to true to avoid starting process that are not needed
            DestinationDirectory = desDir;
            OutPutFile = outFile;
            Filename = filename;
        }

        public AudioConverter(List<string> filenames, string[] outFiles, string desDir)
        {
            IsDoneConverting = true;//Set to true to avoid starting process that are not needed
            DestinationDirectory = desDir;
            FilenameArray = ListToString(filenames);
            OutPutFileArray = outFiles;

        }

        private void ConvertFile(string Filename, string outputfile)
        {
            
            using (AudioFileReader reader = new AudioFileReader(Filename))
            {
                writer = new WaveFileWriter(outputfile, reader.WaveFormat);
                IsDoneConverting = false;
                reader.CopyTo(writer);
            }
            IsDoneConverting = true;
            writer.Close();
            writer.Dispose();
        }
        
        //Converts Multiple files at once      
        private void ConvertFiles()
        {
            int count = 0;
            foreach (string filename in FilenameArray)
            {
                FileBeingConverted = filename;
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
                ConvertFile(FilenameArray[0], Path.Combine(DestinationDirectory, OutPutFile));
                FileBeingConverted = FilenameArray[0];
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
        /// <summary>
        /// Returns progress of conversion of file in terms of bytes written
        /// </summary>
        public long GetCurrentFileSize()
        {
            return writer.Length;
        }
        /// <summary>
        /// Returns name of file being converted
        /// </summary>
        /// <returns>String</returns>
        public string GetNameFileBeingConverted()
        {
            return FileBeingConverted;
        }
    }
}
