using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmBandPrint : Form
    {
        string FstrPrtBand;
        public frmBandPrint()
        {
            InitializeComponent();
        }

        private void frmBandPrint_Load(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            timer1.Interval = 5000;
            timer1.Enabled = false;
            label1.Text = "환자인식밴드 준비";
            
            FstrPrtBand = VB.Left(CF.Reg_Get_Setting("BASIC", "BAND"), 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            Select_View();

            timer1.Enabled = true;
        }

        private void Select_View()
        {            
            int i = 0;            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strSName = "";
            string strAgeX = "";
            string strJumin = "";
            string strChild = "";
            string strRowid = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            string strGbn = VB.Left(FstrPrtBand, 1);
            strGbn = VB.IIf(strGbn == "0", "", strGbn).ToString();

            try
            {                
                SQL = " SELECT PANO,SNAME,AGEX,JUMIN,Child,ROWID ";
                SQL = SQL + ComNum.VBLF + "  From IPD_NAME_PRT ";
                SQL = SQL + ComNum.VBLF + " Where RDate=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                if (VB.Left(FstrPrtBand, 1) == "0")
                {
                    SQL = SQL + ComNum.VBLF + "  AND (GBN is null or GBN ='')";
                    
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBN = 'E' ";
                }
                
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                        strAgeX = dt.Rows[i]["AGEX"].ToString().Trim();
                        strJumin = dt.Rows[i]["JUMIN"].ToString().Trim();
                        strRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                        strChild = dt.Rows[i]["Child"].ToString().Trim();

                        try
                        {
                            PrtBand(strPano, strSName, strAgeX, strJumin, strChild);
                            WRITE_LOG(strPano, "P", true);
                        }
                        catch (Exception e)
                        {
                            Log.Debug(e.Message);
                            WRITE_LOG(strPano, "P", false);
                            return;
                        }

                        if (DELETE_IPD_NAME_PRT(strRowid) == false)
                        {
                            WRITE_LOG(strPano, "D", false);
                            ComFunc.MsgBox("환자인식밴드 자료삭제 실패!! 전산실 연락 바람", "작업취소");
                            return;
                        }
                        WRITE_LOG(strPano, "D", true);
                    } //for i            
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;

                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool DELETE_IPD_NAME_PRT(string strROWID)
        {
            bool rtVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE IPD_NAME_PRT WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void PrtBand(string strPano, string strSName, string strAgeX, string strJumin, string strChild)
        {
            int M2D = 8; //1mm = 8d dots(203 dpi 모델), 12dots(300 dpi 모델)
            bool result = false;
            bool bAutoCut = false;
            string strPrinterName = "";

            if (strChild == "0")
            {
                strPrinterName = "환자밴드S";
            }
            else if (strChild == "1")
            {
                strPrinterName = "환자밴드L";
            }

            // Connect Printer Driver
            if (clsBXLLib.ConnectPrinter(strPrinterName) == false)
            {
                return;
            }

            // Paper Cutting
            bAutoCut = true;

            // 203 DPI : 1mm = 8 dots
            // 300 DPI : 1mm = 12 dots
            M2D = clsBXLLib.GetPrinterResolution() < 300 ? 8 : 12;

            int nPaperWidth = 20 * M2D;     // 2cm * 8(dot)
            int nPaperHeight = 170 * M2D;   // 27cm * 8(dot)

            // Set the label start
            result = clsBXLLib.StartLabel();

            // Clear Buffer of Printer
            result = clsBXLLib.ClearBuffer();

            // Set Label and Printer(속도, 농도(~20), 인쇄방향, ...)
            result = clsBXLLib.SetConfigOfPrinter(clsBXLLib.SPEED_50, 16, clsBXLLib.TOP, bAutoCut, 0, true);
            // SetPaper(좌측 마진, 상단 마진, 용지 너비, 용지 길이, 용지타입(갭/블랙마크/연속용지), ...)
            result = clsBXLLib.SetPaper(0, 0, nPaperWidth, nPaperHeight, clsBXLLib.BLACKMARK, 0, 24);

            // Print 1D Barcode(X좌표, Y좌표, 바코드타입, 좁은 바 너비, 넓은 바 너비, 바코드 높이, 회전...
            result = clsBXLLib.Print1DBarcode(18 * M2D, 45 * M2D, clsBXLLib.CODE128, 3, 4, 6 * M2D, clsBXLLib.ROTATE_90, false, strPano);

            //  Print trueTypeFont
            //  P1 : Horizontal position (X) [dot]
            //  P2 : Vertical position (Y) [dot]
            //  P3 : Font Name
            //  P4 : Font Size
            //  P5 : Rotation : (0 : 0 degree , 1 : 90 degree, 2 : 180 degree, 3 : 270 degree)
            //  P6 : Italic
            //  P7 : Bold
            //  P8 : Underline

            // 병록번호
            result = clsBXLLib.PrintTrueFontLib(12 * M2D, 45 * M2D, "맑은 고딕", 13, clsBXLLib.ROTATE_90, false, true, false, strPano);
            // 성별나이
            result = clsBXLLib.PrintTrueFontLib(11 * M2D, 70 * M2D, "굴림체", 12, clsBXLLib.ROTATE_90, false, false, false, strAgeX);
            // 이름            
            result = clsBXLLib.PrintTrueFontLib(7 * M2D, 45 * M2D, "맑은 고딕", 14, clsBXLLib.ROTATE_90, false, true, false, strSName);
            // 주민등록번호            
            result = clsBXLLib.PrintTrueFontLib(5 * M2D, 70 * M2D, "굴림체", 12, clsBXLLib.ROTATE_90, false, false, false, strJumin);

            // Print device font string(X좌표, Y좌표, 폰트타입,
            //result = PrintDeviceFont(17 * M2D, 125 * M2D, KOR_26X26, 1, 1, ROTATE_90, false, "혈액형 : A")
            //result = PrintDeviceFont(12 * M2D, 125 * M2D, KOR_26X26, 1, 1, ROTATE_90, false, "나이 : 29/M")
            //result = PrintDeviceFont(7 * M2D, 125 * M2D, KOR_26X26, 1, 1, ROTATE_90, false, "이름 : 홍길동")

            //   Print Command
            result = clsBXLLib.Prints(1, 1);

            //   Set the Label End
            clsBXLLib.EndLabel();

            // 프린터 드라이버와 연결끊기
            result = clsBXLLib.DisconnectPrinter();
        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            label1.Text = "환자인식밴드 인쇄중!!";
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label1.Text = "환자인식밴드 준비";
        }


        private void WRITE_LOG(string strPano, string strGubun, bool bolValue)
        {
            string strMsg = "";
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (strGubun == "P")
            {
                strMsg = "[" + strPano + "] 출력 : " + (bolValue == true ? "성공" : "실패") + " ";
                strMsg = strMsg + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            if (strGubun == "D")
            {
                strMsg = "[" + strPano + "] 삭제 : " + (bolValue == true ? "성공" : "실패") + " ";
                strMsg = strMsg + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            txtLog.Text = strMsg + ComNum.VBLF + txtLog.Text;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBandPrint_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;   // 창을 보이지 않게 한다.
                this.ShowIcon = false;  // 작업표시줄에서 제거
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowIcon = true;
            notifyIcon1.Visible = false;
        }
    }
}
