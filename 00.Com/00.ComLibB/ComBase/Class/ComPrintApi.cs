using System;
//추가네임스페이스
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ComBase
{
    public class BaseAPI
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int n, int m); // n은 주파수, m은 소리내는 시간(단위: 1/1000초)
    }


    public class ComPrintApi : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(m_bm != null)
                {
                    m_bm.Dispose();
                    m_bm = null;
                }
            }
            base.Dispose(disposing);
        }

        private Bitmap m_bm;

        // 스트럭쳐 api 호출.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter([In]System.IntPtr hPrinter, [In, Out]string pBuf, [In]int cbBug, ref int pcWritten);


        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            // ANSI 텍스트를 프린터로 보내기.
            bool b = SendBytesToPrinter(szPrinterName, szString);
            //Marshal.FreeCoTaskMem(pBytes);
            return b;
        }

        public static bool SendBytesToPrinter(string szPrinterName, string strPrint)
        {
            Int32 dwError = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // 성공 실패유무.
            // 기록된 바이트 수 
            int dwWritten = 0;
            int dwBytesOfText = System.Text.Encoding.GetEncoding(949).GetByteCount(strPrint);

            di.pDocName = "PrinterBarcode";
            di.pDataType = "RAW";

            // 프린터 오픈.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // 문서 시작.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // 시작 페이지
                    if (StartPagePrinter(hPrinter))
                    {
                        // 바이트 쓰기
                        bSuccess = WritePrinter(hPrinter, strPrint, dwBytesOfText, ref dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }

            //실패시. 마샬 을 통한 에러메세지 반환.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // 파일오픈
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // 파일을 바이너리로.
            BinaryReader br = new BinaryReader(fs);
            // 바이트고정.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // 포인터.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // 파일의 내용을 배열로.
            bytes = br.ReadBytes(nLength);
            // 할당되지않은 메모리에 할당.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // 바이트 배열관리
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // 바이트를 프린터로 보내기.
            bSuccess = SendBytesToPrinter(szPrinterName, szFileName);
            // 메모리 마샬.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }



        public static bool RawPrint(string szPrinterName, string szString)
        {
            bool bSuccess = false; // 성공 실패유무.

            Int32 dwError = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            // 기록된 바이트 수 
            int dwWritten = 0;
            int dwBytesOfText = System.Text.Encoding.GetEncoding(949).GetByteCount(szPrinterName);

            di.pDocName = "EXAM";
            di.pDataType = "RAW";


            // 프린터 오픈.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // 문서 시작.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // 시작 페이지
                    if (StartPagePrinter(hPrinter))
                    {
                        // 바이트 쓰기
                        bSuccess = WritePrinter(hPrinter, szPrinterName, dwBytesOfText, ref dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }

            //실패시. 마샬 을 통한 에러메세지 반환.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }

            return bSuccess;
        }

        public string GetHangul2(string st)
        {
            Encoding oEnc = System.Text.Encoding.GetEncoding(949);
            Byte[] buff = oEnc.GetBytes(st);
            Byte[] oResult = new Byte[2];
            int i, j;

            i = 0;
            j = i;
            while (i <= buff.Length - 1)
            {
                Array.Resize(ref oResult, j + 2);
                if (buff[i] > 128)
                {
                    oResult[j] = buff[i];
                    i = i + 1;
                    oResult[j + 1] = buff[i];
                }
                else
                {
                    oResult[j] = 201;
                    oResult[j + 1] = (byte)(buff[i] + 129);
                }
                i = i + 1;
                j = j + 2;

            }
            return Encoding.Default.GetString(oResult, 0, oResult.Length);
        }

        public string GetHangul(string st)
        {
            Encoding oEnc = System.Text.Encoding.GetEncoding(949);
            Byte[] buff = oEnc.GetBytes(st);
            Byte[] oResult = new Byte[2];
            int i, j;

            i = 0;
            j = i;
            while (i <= buff.Length - 1)
            {
                Array.Resize(ref oResult, j + 2);
                if (buff[i] > 128)
                {
                    oResult[j] = buff[i];
                    i = i + 1;
                    oResult[j + 1] = buff[i];
                }
                else
                {
                    oResult[j] = 163;
                    oResult[j + 1] = (byte)(buff[i] + 128);
                }
                i = i + 1;
                j = j + 2;

            }
            return Encoding.Default.GetString(oResult, 0, oResult.Length);
        }

        void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
        {
            int index = y * bmd.Stride + (x >> 3);

            byte p = System.Runtime.InteropServices.Marshal.ReadByte(bmd.Scan0, index);

            byte mask = (byte)(0x80 >> (x & 0x7));

            if (pixel)
            {
                p = (byte)(p | mask);
            }
            else
            {
                p = (byte)(p & mask ^ 0xff);
            }

            System.Runtime.InteropServices.Marshal.WriteByte(bmd.Scan0, index, p);
        }

        Bitmap Make1bitIndexed(Bitmap b, float brightness)
        {
            Bitmap cb = new Bitmap(b.Width, b.Height, PixelFormat.Format1bppIndexed);
            BitmapData bmdn = cb.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            Color c = new Color();

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    c = b.GetPixel(x, y);
                    if (c.GetBrightness() > brightness)
                    {
                        SetIndexedPixel(x, y, bmdn, true); // set it if its bright.             
                    }
                }
            }

            cb.UnlockBits(bmdn);

            return cb;
        }

        public string CreateGRF(string argSname, string imagename, int nFontSize)
        {
            BitmapData imgData = null;
            byte[] pixels;
            int x, y, width;
            StringBuilder sb;
            IntPtr ptr;

            try
            {
                m_bm = new Bitmap(500, 30);
                using (Graphics grf = Graphics.FromImage(m_bm))
                {
                    grf.Clear(Color.White);
                    using (StringFormat format = new StringFormat(StringFormatFlags.LineLimit))
                    {
                        grf.DrawString(argSname, new Font("돋움체", nFontSize, FontStyle.Bold), new SolidBrush(Color.Black), new Rectangle(0, 0, 500, 30), format);
                        //grf.DrawString(argSname, new Font("돋움체", 16, FontStyle.Bold), new SolidBrush(Color.Black), new Rectangle(0, 0, 500, 30), format); 과거 CreateGRF
                        //grf.DrawString(argSname, new Font("돋움체", 18, FontStyle.Bold), new SolidBrush(Color.Black), new Rectangle(0, 0, 500, 30), format); 과거 CreateGRF2
                    }

                    m_bm = Make1bitIndexed(m_bm, 0F);
                    imgData = m_bm.LockBits(new Rectangle(0, 0, m_bm.Width, m_bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
                    width = (m_bm.Width + 7) / 8;
                    pixels = new byte[width];
                    sb = new StringBuilder(width * m_bm.Height * 2);
                    ptr = imgData.Scan0;
                    for (y = 0; y < m_bm.Height; y++)
                    {
                        Marshal.Copy(ptr, pixels, 0, width);
                        for (x = 0; x < width; x++)
                            sb.AppendFormat("{0:X2}", (byte)~pixels[x]);
                        ptr = (IntPtr)(ptr.ToInt64() + imgData.Stride);
                    }
                }
                return String.Format("~DG{0}, {1}, {2},", imagename, width * y, width) + sb.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {
                if (m_bm != null)
                {
                    if (imgData != null) m_bm.UnlockBits(imgData);
                    m_bm.Dispose();
                }
            }

        }
    }
}
