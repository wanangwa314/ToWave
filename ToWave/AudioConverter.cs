using System;
using System.Threading.Tasks;
using NAudio.Wave;
using System.IO;


namespace ToWave
{

    class AudioConverter
    {
        public string OutPutFile { get; set; }
        public string Filename { get; set; }

        public AudioConverter(string filename, string outFile)
        {
            OutPutFile = outFile;
            Filename = filename;
        }

        public void Convert()
        {
            using (AudioFileReader reader = new AudioFileReader(Filename))
            using (WaveFileWriter writer = new WaveFileWriter(OutPutFile, reader.WaveFormat))
            {
                reader.CopyTo(writer);
            }
        }
    }
}
