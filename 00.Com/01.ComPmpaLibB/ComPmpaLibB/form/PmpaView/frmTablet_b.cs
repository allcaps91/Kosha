using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ComBase;

namespace ComPmpaLibB
{
    public partial class frmTablet_b : Form
    {
        /// <summary>
        /// 다른폼에서 호출시 아래내용처럼 호출
        /// </summary>
        //이미 폼이 떠있는지 확인한다.
        //if (!hWnd.Equals(IntPtr.Zero))
        //{
        //    // 윈도우가 최소화 되어 있다면 활성화 시킨다
        //    ShowWindowAsync(hWnd, SW_SHOWNORMAL);
        //}
        //else
        //{     
        //    frmTablet_b frm = new frmTablet_b("홍민철", "9000");     
        //    ShowWindow(frm.Handle, WM_SHOWNOACTIVATE);
        //}
        
        Point lastPoint = Point.Empty;
        bool isMouseDown = new Boolean();
        
        string FstrName = string.Empty;
        string FstrAmt = string.Empty;
        string[] Fstr = new string[2];
        
        public frmTablet_b()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
            Display_Secondary_Monitor();
        }
        
        public frmTablet_b(string ArgName, string ArgAmt)
        {
            InitializeComponent();
            SetEvents();
            SetControl();
            Display_Secondary_Monitor();

            FstrName = ArgName;
            FstrAmt = ArgAmt;

            Form1_Load(null, null);
        }

        void SetEvents()
        {
            this.KeyPreview = true;
            this.Load                   += new EventHandler(Form1_Load);            
            this.KeyPress               += new KeyPressEventHandler(eKeyPress);
            //this.pictureBox1.MouseDown  += new MouseEventHandler(pictureBox1_MouseDown);
            //this.pictureBox1.MouseMove  += new MouseEventHandler(pictureBox1_MouseMove);
            //this.pictureBox1.MouseUp    += new MouseEventHandler(pictureBox1_MouseUp);
            //this.btnSave.Click          += new EventHandler(eSaveSign);

            //frmPmpaEntryCardDaou.rCSign += new frmPmpaEntryCardDaou.rEventCSign(SaveImageOld);
        }
        
        void SetControl()
        {
            this.ShowInTaskbar = false;

            this.lblName.BackColor = Color.Transparent;
            this.lblAmt.BackColor = Color.Transparent;
            this.lblMsg1.BackColor = Color.Transparent;
            this.lblMsg2.BackColor = Color.Transparent;
            this.lblMsg3.BackColor = Color.Transparent;

            this.lblName.Parent = pictureBox2;
            this.lblAmt.Parent = pictureBox2;
            this.lblMsg1.Parent = pictureBox2;
            this.lblMsg2.Parent = pictureBox2;
            this.lblMsg3.Parent = pictureBox2;

            this.lblName.Visible = false;
            this.lblAmt.Visible = false;
            this.lblMsg1.Visible = false;
            this.lblMsg2.Visible = false;
            this.lblMsg3.Visible = false;
        }

        void Display_Secondary_Monitor()
        {
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)     // Has more screen
            {
                Screen scrn = (screens[0].WorkingArea.Contains(this.Location))
                                         ? screens[1] : screens[0];
                this.Location = new Point(scrn.Bounds.Left, 0);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {   
           
            //pictureBox1.Cursor = Cursors.Hand;

            //pictureBox1.Image = null;
            Invalidate();
            
            if (FstrName.Trim() != "")
            {
                lblName.Text = FstrName.Substring(0, 1) + "♡";
                if (FstrName.Length > 2)
                {
                    lblName.Text += FstrName.Substring(2, FstrName.Length - 2);
                }
                this.lblName.Visible = true;
            }

            if (FstrAmt.Trim() != "")
            {
                lblAmt.Text = '\u005c' + " " + Convert.ToInt64(VB.Val(VB.Replace(FstrAmt, ",", ""))).ToString("###,###,###") + " 원";
                this.lblAmt.Visible = true;
                this.lblMsg1.Visible = true;
                this.lblMsg2.Visible = true;
                this.lblMsg3.Visible = true;
            }
            
        }
        
        //void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    lastPoint = e.Location;
        //    isMouseDown = true;
        //}

        //void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (isMouseDown == true)
        //    {
        //        if (lastPoint != null)
        //        {
        //            if (pictureBox1.Image == null)
        //            {
        //                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        //                pictureBox1.Image = bmp; 
        //            }
        //            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
        //            {
        //                g.DrawLine(new Pen(Color.Black, 2), lastPoint, e.Location);
        //                g.SmoothingMode = SmoothingMode.AntiAlias;
        //            }

        //            pictureBox1.Invalidate();
        //            lastPoint = e.Location;
        //        }

        //    }

        //}

        //void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    isMouseDown = false;
        //    lastPoint = Point.Empty;
        //}
       
        //void eSaveSign(object sender, EventArgs e)
        //{
        //    SaveImageOld(Card.GstrpFile);

        //    this.Close();
        //}

        //void SaveImageOld(string path)
        //{
        //    int intWidth = 128; //저장할 이미지 너비
        //    int intHeight = 64; //저장할 이미지 높이

        //    if (pictureBox1.Image == null)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        using (var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height)) //
        //        {
        //            pictureBox1.DrawToBitmap(bitmap, pictureBox1.ClientRectangle);

        //            ImageFormat imageFormat = ImageFormat.Bmp;

        //            //string extension = Path.GetExtension(path);
        //            //switch (extension)
        //            //{
        //            //    case ".bmp":
        //            //        imageFormat = ImageFormat.Bmp;
        //            //        break;
        //            //    case ".png":
        //            //        imageFormat = ImageFormat.Png;
        //            //        break;
        //            //    case ".jpeg":
        //            //    case ".jpg":
        //            //        imageFormat = ImageFormat.Jpeg;
        //            //        break;
        //            //    case ".gif":
        //            //        imageFormat = ImageFormat.Gif;
        //            //        break;
        //            //    default:
        //            //        throw new NotSupportedException("File extension is not supported");
        //            //}

        //            // bitmap.Save(path, imageFormat); //원본이미지 그대로 저장

        //            //사이즈를 변경해서 저장
        //            Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                    
        //            Graphics graphics_1 = Graphics.FromImage(newImage);
        //            graphics_1.CompositingQuality = CompositingQuality.HighQuality;
        //            graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            graphics_1.SmoothingMode = SmoothingMode.HighQuality;
        //            graphics_1.DrawImage(bitmap, 0, 0, intWidth, intHeight);
                    
        //            newImage.Save(path, imageFormat);
                    
        //            graphics_1.Dispose();
        //            graphics_1 = null;

        //            newImage.Dispose();
        //            newImage = null;

        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString());
        //        return;
        //    }
        //}
        
    }
}
