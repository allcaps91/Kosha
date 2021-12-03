using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ComBase
{
    /// <summary>
    /// 이미지 관련 함수 모음
    /// 2018-05-01 박웅규
    /// </summary>
    public class clsFuncImage
    {
        /// <summary>
        /// ImageToBase64
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Base64ToImage
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// 바이트 배열을 String으로 변환 
        /// </summary>
        /// <param name="strByte"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] strByte)
        {
            //string str = Encoding.Default.GetString(strByte);
            string str = Encoding.ASCII.GetString(strByte);
            return str;
        }

        /// <summary>
        /// String을 바이트 배열로 변환 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str);
            return StrByte;
        }

        /// <summary>
        /// 이미지를 Arry
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imgF"></param>
        /// <returns></returns>
        public static byte[] ImageToByte2(Image img, ImageFormat imgF)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, imgF);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        /// <summary>
        /// BytesToBitmap
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Bitmap img = (Bitmap)Image.FromStream(ms);
                return img;
            }
        }

    }
}
