/*
 * Nicolas Achter & Nickolas Schmidt
 * ENSE496AE - Object Shuffling Assignment Req. 5
 */

using System;
using System.IO;

namespace Mp3FrameShuffler
{
    class Mp3
    {
        public byte[] mp3Bytes;
        public byte[] syncWord = new byte[2];
        public int frameLength; //length of frame, minus the header;
        public int numFrames = 0; //frame counter
        public int dataStart = 0;
    }

    class Mp3FrameShuffler
    {
        static void Main(string[] args)
        {
            int key = 802797117; //shuffling key - taken from requirement 3 
            Mp3 mp3 = new Mp3();
            mp3.mp3Bytes = File.ReadAllBytes("E:/Documents/GitHub/ENSE496-Assignment4/Mp3FrameShuffler/mp3.mp3"); //get file
            mp3.syncWord[0] = 0xff; //mp3 sync word first byte
            mp3.syncWord[1] = 0xfb; //mp3 sync word second byte

            //get mp3 data
            GetMp3Data(mp3);
            byte[][] frames = GetFrameArray(mp3);

            //
            //SHUFFLE DATA BY FRAMES
            //
            byte[][] shuffledFrames = Shuffle(frames, mp3.numFrames, mp3.frameLength, key);
            //create new mp3 for export
            Mp3 mp3Shuffled = new Mp3();
            mp3Shuffled.mp3Bytes = new byte[mp3.mp3Bytes.Length];
            mp3Shuffled.frameLength = mp3.frameLength;
            mp3Shuffled.numFrames = mp3.numFrames;
            mp3Shuffled.dataStart = mp3.dataStart;
            OutputShuffledFrames(mp3Shuffled, mp3, shuffledFrames, "mp3Shuffled");

            //
            //UNSHUFFLE DATA BY FRAMES
            //
            byte[][] unshuffledFrames = Unshuffle(shuffledFrames, mp3.numFrames, mp3.frameLength, key);
            //create new mp3 for export
            Mp3 mp3Unshuffled = new Mp3();
            mp3Unshuffled.mp3Bytes = new byte[mp3.mp3Bytes.Length];
            mp3Unshuffled.frameLength = mp3.frameLength;
            mp3Unshuffled.numFrames = mp3.numFrames;
            mp3Unshuffled.dataStart = mp3.dataStart;
            OutputShuffledFrames(mp3Unshuffled, mp3, unshuffledFrames, "mp3Unshuffled");

            //
            //SHUFFLE ALL DATA
            //
            byte[] mp3Data = new byte[mp3.mp3Bytes.Length - mp3.dataStart];
            //get data minus ID3 tag
            for (int i = mp3.dataStart; i < mp3.mp3Bytes.Length; i++)
                mp3Data[i - mp3.dataStart] = mp3.mp3Bytes[i];
            byte[] shuffledData = ShuffleAll(mp3Data, key);
            //create new mp3 for export
            Mp3 mp3ShuffledData = new Mp3();
            mp3ShuffledData.mp3Bytes = new byte[mp3.mp3Bytes.Length];
            mp3ShuffledData.frameLength = mp3.frameLength;
            mp3ShuffledData.numFrames = mp3.numFrames;
            mp3ShuffledData.dataStart = mp3.dataStart;
            OutputAllData(mp3ShuffledData, mp3, shuffledData, "mp3ShuffledData");

            //
            //UNSHUFFLE ALL DATA
            //
            byte[] unshuffledData = UnshuffleAll(shuffledData, key);
            //create new mp3 for export
            Mp3 mp3UnshuffledData = new Mp3();
            mp3UnshuffledData.mp3Bytes = new byte[mp3.mp3Bytes.Length];
            mp3UnshuffledData.frameLength = mp3.frameLength;
            mp3UnshuffledData.numFrames = mp3.numFrames;
            mp3UnshuffledData.dataStart = mp3.dataStart;
            OutputAllData(mp3UnshuffledData, mp3, unshuffledData, "mp3UnshuffledData");
        }

        public static void GetMp3Data(Mp3 mp3)
        {
            int i = 0;
            int temp;

            //get mp3 data
            //check for ID3 header
            if (mp3.mp3Bytes[i] == 49 && mp3.mp3Bytes[i + 1] == 44 && mp3.mp3Bytes[i + 2] == 33)
            {
                //find end of ID3 header
                while (mp3.mp3Bytes[i] == mp3.syncWord[0] && mp3.mp3Bytes[i + 1] == mp3.syncWord[1])
                    i++;
                mp3.dataStart = i;   //get index of where the id3 header ends
            }

            //ensure we are at the start of the mp3 data
            while (mp3.mp3Bytes[i] != mp3.syncWord[0] || mp3.mp3Bytes[i + 1] != mp3.syncWord[1])
                i++;
            mp3.dataStart = i;
            mp3.numFrames++;

            temp = i - 1;
            i = i + 1;
            //count iterations until next header
            while (mp3.mp3Bytes[i] != mp3.syncWord[0] || mp3.mp3Bytes[i + 1] != mp3.syncWord[1])
            {
                i++;
            }
            mp3.frameLength = i - temp;

            mp3.numFrames = (mp3.mp3Bytes.Length - mp3.dataStart) / mp3.frameLength;
        }

        public static byte[][] GetFrameArray(Mp3 mp3)
        {
            int i = mp3.dataStart; //reset iterator to start of first header
            byte[][] frames = new byte[mp3.numFrames][];
            //intialize array columns
            for (int k = 0; k < frames.Length; k++)
                frames[k] = new byte[mp3.frameLength];
            int f = 0; //frame tracker
            bool newFrame;

            //iterate through mp3 again to collect all the frames into an array
            while (f < mp3.numFrames)
            {
                byte[] frame = new byte[mp3.frameLength]; //current frame

                //ensure i does not extend out of range of the array
                if (i >= mp3.mp3Bytes.Length -  1)
                    break;

                //get frame
                //ensure we are at the start of a frame
                if (mp3.mp3Bytes[i] == mp3.syncWord[0] && mp3.mp3Bytes[i + 1] == mp3.syncWord[1])
                {
                    newFrame = true;

                    //iterate though and collect the frame
                    for (int j = 0; j < mp3.frameLength; j++)
                    {
                        frame[j] = mp3.mp3Bytes[i];
                        i++;
                        //ensure we aren't going out of range again
                        if (i >= mp3.mp3Bytes.Length)
                            break;
                    }
                }
                //if we aren't increment and continue looping
                else
                {
                    i++;
                    newFrame = false;
                }

                //if we were infact at a new frame, then it will be populated and can be added to the 2d array
                if (newFrame)
                {
                    frames[f] = frame;
                    f++;
                }
            }

            return frames;
        }

        //Fisher-Yates shuffling function for a 2D array - for shuffling frames
        //shuffles using a list of random numbers generated by a key so that it may be unshuffled later
        public static byte[][] Shuffle(byte[][] arr, int numRows, int numCols, int key)
        {
            byte[][] returnArr = arr;
            var shuffles = GetShuffles(numRows, key); //list of shuffles to make
            int shuffle;
            byte[] temp = new byte[numCols];

            //iterate through, shuffling through the array
            for (int i = numRows - 1; i > 0; i--)
            {
                shuffle = shuffles[numRows - 1 - i]; //get next shuffle position
                //shuffle
                for(int j = 0; j < numCols; j++)
                    temp[j] = returnArr[i][j];
                for (int j = 0; j < numCols; j++)
                    returnArr[i][j] = returnArr[shuffle][j];
                for (int j = 0; j < numCols; j++)
                    returnArr[shuffle][j] = temp[j];
            }

            return returnArr;
        }

        //same implementation as above, but for a 1d array - for shuffling all data
        public static byte[] ShuffleAll(byte[] arr, int key)
        {
            byte[] returnArr = arr;
            var shuffles = GetShuffles(arr.Length, key); //list of shuffles to make
            int shuffle;
            byte temp;

            //iterate through, shuffling through the array
            for (int i = arr.Length - 1; i > 0; i--)
            {
                shuffle = shuffles[arr.Length - 1 - i]; //get next shuffle position
                //shuffle
                temp = returnArr[i];
                returnArr[i] = returnArr[shuffle];
                returnArr[shuffle] = temp;
            }

            return returnArr;
        }

        //Fisher-Yates unshuffling function for a 2D array
        //unshuffles using a list of random numbers generated by the key that it was shuffled with
        public static byte[][] Unshuffle(byte[][] arr, int numRows, int numCols, int key)
        {
            byte[][] returnArr = arr;
            var shuffles = GetShuffles(numRows, key); //list of shuffles to make to get to original
            int shuffle;
            byte[] temp = new byte[numCols];

            //iterate through, unshuffling through the array - iterates oppposite to shuffling
            for (int i = 1; i < numRows; i++)
            {
                shuffle = shuffles[numRows - i - 1]; //get next shuffle position
                //unshuffle
                for (int j = 0; j < numCols; j++)
                    temp[j] = returnArr[i][j];
                for (int j = 0; j < numCols; j++)
                    returnArr[i][j] = returnArr[shuffle][j];
                for (int j = 0; j < numCols; j++)
                    returnArr[shuffle][j] = temp[j];
            }

            return returnArr;
        }

        //same implementation as above, but for a 1d array - for unshuffling all data
        public static byte[] UnshuffleAll(byte[] arr, int key)
        {
            byte[] returnArr = arr;
            var shuffles = GetShuffles(arr.Length, key); //list of shuffles to make to get to original
            int shuffle;
            byte temp;

            //iterate through, unshuffling through the array - iterates oppposite to shuffling
            for (int i = 1; i < arr.Length; i++)
            {
                shuffle = shuffles[arr.Length - i - 1]; //get next shuffle position
                //unshuffle
                temp = returnArr[i];
                returnArr[i] = returnArr[shuffle];
                returnArr[shuffle] = temp;
            }

            return returnArr;
        }

        //function to get a list of shuffles to make given a key,
        //this is used to ensure the both the shuffling and unshuffling functions generate the same list of shuffles
        public static int[] GetShuffles(int numRows, int key)
        {
            int[] shuffles = new int[numRows - 1];
            var rand = new Random(key); //random number gen, given key
            int r;

            //fill the shuffles array such that it will have (numRows - 1) elements so there are enough for a perfect shuffle
            for (int i = numRows - 1; i > 0; i--)
            {
                r = rand.Next(i + 1);
                shuffles[numRows - 1 - i] = r;
            }

            return shuffles;
        }


        public static void OutputShuffledFrames(Mp3 shuffled, Mp3 unshuffled, byte[][] shuffledFrames, string filename)
        {
            //counters
            int j = shuffled.dataStart;
            int f = 0;

            //get metadata in the new file
            if (shuffled.dataStart != 0)
                for (int i = 0; i < shuffled.dataStart; i++)
                    shuffled.mp3Bytes[i] = unshuffled.mp3Bytes[i];
            //fill frames
            while (f < shuffled.numFrames)
            {
                if (shuffledFrames[f] == null)
                    break;

                //get frame
                for (int k = 0; k < shuffled.frameLength; k++)
                {
                    shuffled.mp3Bytes[j] = shuffledFrames[f][k];
                    j++;
                }

                f++;
            }
            File.WriteAllBytes("E:/Documents/GitHub/ENSE496-Assignment4/Mp3FrameShuffler/" + filename + ".mp3", shuffled.mp3Bytes); //output frame-shuffled file
        }

        public static void OutputAllData(Mp3 shuffled, Mp3 unshuffled, byte[] shuffledData, string filename)
        {
            //get metadata in the new file
            if (shuffled.dataStart != 0)
                for (int i = 0; i < shuffled.dataStart; i++)
                    shuffled.mp3Bytes[i] = unshuffled.mp3Bytes[i];
            //insert shuffled mp3 data after id3 tag, if there is one
            for (int i = shuffled.dataStart; i < unshuffled.mp3Bytes.Length; i++)
                shuffled.mp3Bytes[i] = shuffledData[i - shuffled.dataStart];

            File.WriteAllBytes("E:/Documents/GitHub/ENSE496-Assignment4/Mp3FrameShuffler/" + filename + ".mp3", shuffled.mp3Bytes); //output frame-shuffled file
        }
    }
}
