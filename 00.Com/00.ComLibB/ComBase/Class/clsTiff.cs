using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ComBase
{
    public class clsTiff
    {

        public static int GetNumberOfPages(string sFileName)
        {
            int rtnVal = 0;
            //setup the image
            Image img = Image.FromFile(sFileName);

            //get its guid
            Guid ID = img.FrameDimensionsList[0];

            //get the frame dimensions
            FrameDimension fd = new FrameDimension(ID);

            //Gets number of pages
            rtnVal = img.GetFrameCount(fd);
            img.Dispose();
            img = null;

            return rtnVal;
        }


        public static Image GetSpecificPage(string sFileName, int iPageNumber)
        {
            Image img = Image.FromFile(sFileName);
            MemoryStream ms = null;
            Image returnImage = Image.FromFile(sFileName);

            try
            {
                ms = new MemoryStream();
                Guid ID = img.FrameDimensionsList[0];
                FrameDimension fd = new FrameDimension(ID);

                img.SelectActiveFrame(fd, iPageNumber);
                img.Save(ms, ImageFormat.Bmp);
                returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message.ToString(), "Error");
                return null;
            }
            finally
            {
                ms.Close();
            }
        }


        public static void SetTIFFCompression(string FileName, double QualityPercentage)
        {

            //Load a bitmap from file
            Bitmap bm = (Bitmap)Image.FromFile(FileName);

            //Get the list of available encoders
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            //find the encoder with the image/jpeg mime-type
            ImageCodecInfo ici = null;

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == "image/tiff")
                    ici = codec;
            }

            //Create a collection of encoder parameters (we only need one in the collection)
            EncoderParameters ep = new EncoderParameters();

            //We'll save image with QualityPercentage as compared with the original
            //Create an encoder parameter for quality with an appropriate level setting
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)QualityPercentage);

            //Save the image with a filename that indicates the compression quality used
            string newFileName = FileName.ToLower().Replace(".tif", "_Quality_" + QualityPercentage + "%.tif");
            bm.Save(newFileName, ici, ep);
        }

        /// <summary>
        /// 멀티페이지 tiff => jpg : 동일 경로에 저장이 된다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] ConvertTiffToJpeg(string fileName)
        {
            using (Image imageFile = Image.FromFile(fileName))
            {
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] jpegPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        jpegPaths[frame] = String.Format("{0}\\{1}{2}.jpg",
                            Path.GetDirectoryName(fileName),
                            Path.GetFileNameWithoutExtension(fileName),
                            "_" + frame.ToString());
                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                    }
                }

                return jpegPaths;
            }

            //Image image = Image.FromFile(...);
            //IList images = new ArrayList();

            //int count = image.GetFrameCount(FrameDimension.Page);
            //for(int idx=0; idx < count; idx++)
            //{
            //    // save each frame to a bytestream
            //    image.SelectActiveFrame(FrameDimension.Page, idx);
            //    MemoryStream byteStream = new MemoryStream();
            //    image.Save(byteStream, ImageFormat.Bmp); 

            //    // and then create a new Image from it
            //    images.Add(Image.FromStream(byteStream));
            //}

        }

        public static string[] ConvertTiffToPng(string fileName)
        {
            using (Image imageFile = Image.FromFile(fileName))
            {
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] PngPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as jpeg. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        PngPaths[frame] = String.Format("{0}\\{1}{2}.png", Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName), "_" + frame.ToString());
                        bmp.Save(PngPaths[frame], ImageFormat.Png);
                    }
                }

                return PngPaths;
            }

            //Image image = Image.FromFile(...);
            //IList images = new ArrayList();

            //int count = image.GetFrameCount(FrameDimension.Page);
            //for(int idx=0; idx < count; idx++)
            //{
            //    // save each frame to a bytestream
            //    image.SelectActiveFrame(FrameDimension.Page, idx);
            //    MemoryStream byteStream = new MemoryStream();
            //    image.Save(byteStream, ImageFormat.Bmp); 

            //    // and then create a new Image from it
            //    images.Add(Image.FromStream(byteStream));
            //}

        }

    }
}
