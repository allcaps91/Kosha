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
using ComDbB;
using System.Threading;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayVIEW05.cs
    /// Description     : 조영제 사용 동의서 현황
    /// Author          : 안정수
    /// Create Date     : 2018-03-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 XUMAIN37.frm(FrmAgreeReport) 폼 frmComSupXrayVIEW05.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\xray\xumain\XUMAIN37.frm(FrmAgreeReport) >> frmComSupXrayVIEW05.cs 폼이름 재정의" />
    public partial class frmComSupXrayVIEW05 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        Thread thread;
        FpSpread spd;

        long[] nSubtot = new long[33];
        long[] nClasstot = new long[33];

        #endregion

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmComSupXrayVIEW05(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmComSupXrayVIEW05()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                #region Combo_Set

                cboJong.Items.Clear();
                cboJong.Items.Add("0.전체");
                cboJong.Items.Add("1.단순촬영");
                cboJong.Items.Add("2.특수촬영");
                cboJong.Items.Add("3.SONO");
                cboJong.Items.Add("4.CT");
                cboJong.Items.Add("5.MRI");
                cboJong.Items.Add("6.R-I");
                cboJong.Items.Add("7.BMD");
                cboJong.Items.Add("8.PET-CT");
                cboJong.Items.Add("9.기타");
                cboJong.Items.Add("A.CT(복부)");
                cboJong.Items.Add("Q.ANGIO");
                cboJong.SelectedIndex = 0;

                #endregion

                thread = new Thread(tProcess);
                thread.Start();
                spd = ssList;
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                thread = new Thread(tProcess);
                thread.Start();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string JobMan = "";
            string JobDate = "";
            string PrintDate = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            
            if(ssList.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            btnPrint.Enabled = false;

            JobMan = clsType.User.JobName;
            JobDate = dtpFDate.Text + " ~ " + dtpTDate.Text;
            PrintDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            strTitle = " 조영제 사용 동의서 현황 LIST  ";
            strSubTitle = "작 성 자 : " + JobMan;
            strSubTitle += "\r\n" + "촬영일자 : " + JobDate;
            strSubTitle += "\r\n" + "출력시간 : " + PrintDate;
            
            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;
                       
            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                
                else
                {
                   
                }

            }

        }

        public DataTable GetDataTable()
        {
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  TO_CHAR(EnterDate,'YYYY-MM-DD') EDate,";
            SQL += ComNum.VBLF + "  TO_CHAR(SeekDate,'YYYY-MM-DD') SDate,";
            SQL += ComNum.VBLF + "  Pano, Sname, IpdOpd,";
            SQL += ComNum.VBLF + "  Sex, Age, Deptcode, Drcode,   Wardcode, Roomcode,";
            SQL += ComNum.VBLF + "  D.Xcode,    Xname,    Qty,      ExID,   XrayRoom, GbNgt, Remark";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_DETAIL D, " + ComNum.DB_PMPA + "XRAY_CODE C";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND SeekDate >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND SeekDate <  TO_DATE('" + Convert.ToDateTime(dtpTDate.Text).AddDays(1).ToShortDateString() + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND D.GbReserved > '5'";
            SQL += ComNum.VBLF + "      AND (D.GbHIC IS NULL OR D.GbHIC <> 'Y')";
            SQL += ComNum.VBLF + "      AND D.Pano <> '88888888'";
            SQL += ComNum.VBLF + "      AND D.XJong < 'A'";

            if(chkCode.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND TRIM(D.XCODE) IN ( SELECT TRIM(XCODE) FROM KOSMOS_PMPA.XRAY_BASUSE WHERE AGREE ='1' )";
            }

            else
            {
                SQL += ComNum.VBLF + "  AND D.AGREE ='1'";  //동의서 받은것
            }
            
            SQL += ComNum.VBLF + "      AND D.Xcode = C.Xcode";

            if(String.Compare(VB.Left(cboJong.Text, 1), "0") > 0 && String.Compare(VB.Left(cboJong.Text, 1), "9") <= 0)
            {
                SQL += ComNum.VBLF + "  AND XJong = '" + VB.Left(cboJong.Text, 1) +"'";
            }

            else if(VB.Left(cboJong.Text, 1) == "A")
            {
                SQL += ComNum.VBLF + "  AND D.XJONG ='4'";
                SQL += ComNum.VBLF + "  AND D.XCODE LIKE 'HA475%'";
            }

            else if(VB.Left(cboJong.Text, 1) == "Q")
            {
                SQL += ComNum.VBLF + "  AND XJong = '" + VB.Left(cboJong.Text, 1) +"'";
            }

            SQL += ComNum.VBLF + "ORDER BY Pano,SName";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }               
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return dt;
        }

        void tProcess()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            //콤보박스에 대한 접근 또는 수정으로 인한 크로스스레드에 관련 오류를 무시한다.
            CheckForIllegalCrossThreadCalls = false;

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnView.BeginInvoke(new System.Action(() => this.btnView.Enabled = false));            
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;

            dt = GetDataTable();

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));            
            this.btnView.BeginInvoke(new System.Action(() => this.btnView.Enabled = true));
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);

        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            if (dt == null) return;

            int i = 0;
            //int j = 0;
            long nRowCnt = 0;
            
            string strOldPano = "";            
            string strPano = "";

            CS.Spread_All_Clear(ssList);

            if (dt.Rows.Count > 0)
            {
                nRowCnt = dt.Rows.Count;
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for(i = 0; i < nRowCnt; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SDate"].ToString().Trim();

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    if(strPano != strOldPano)
                    {
                        ssList.ActiveSheet.Cells[i, 1].Text = strPano;
                        ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        strOldPano = strPano;
                    }

                    else
                    {
                        ssList.ActiveSheet.Cells[i, 1].Text = "";
                        ssList.ActiveSheet.Cells[i, 2].Text = "";
                    }

                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Drcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Wardcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["Roomcode"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 10].Text = " " + dt.Rows[i]["Xname"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 11].Text = " " + dt.Rows[i]["Remark"].ToString().Trim();                    
                }
            }

            dt.Dispose();
            dt = null;
        }

        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

    }
}
