using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmOpdIlji3YearPrint.cs
    /// Description     : 외래월보(과별 의사별 진료실적)
    /// Author          : 안정수
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong34.frm(FrmOpdIlji3YearPrint) 폼 frmOpdIlji3YearPrint.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong34.frm(FrmOpdIlji3YearPrint) >> frmOpdIlji3YearPrint.cs 폼이름 재정의" />
    public partial class frmOpdIlji3YearPrint : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsNurse CN = new clsNurse();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        int nSSRowCount = 0;
        //int ERow = 0;
        //int ECol = 0;

        FarPoint.Win.EmptyBorder emptyBorder = new FarPoint.Win.EmptyBorder();
        
        //bottom
        FarPoint.Win.LineBorder lineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, false, false, false, true);

        //Top
        FarPoint.Win.LineBorder TlineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, false, true, false, false);

        //Right
        FarPoint.Win.LineBorder RlineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, false, false, true, false);

        //all
        FarPoint.Win.LineBorder AlineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, true, true, true, true);

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

        public frmOpdIlji3YearPrint(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOpdIlji3YearPrint()
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
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.cboYYMM.GotFocus += new EventHandler(eControl_GotFocus);

            this.cboYYMM.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);

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

                Set_Init();
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
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnCancel)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnCancel_Click();
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

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if(sender == this.cboYYMM)
            {
                SCREEN_CLEAR();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.cboYYMM)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void Set_Init()
        {
            int nYY = 0;
            int i = 0;
            string strYY = "";

            nYY = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
            strYY = ComFunc.SetAutoZero(nYY.ToString(), 4);

            cboYYMM.Items.Clear();

            for(i = 1; i <= 24; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYY, 4) + "년");

                strYY = (Convert.ToInt32(strYY) - 1).ToString();

                if(strYY == "1997")
                {
                    break;
                }
            }

            cboYYMM.SelectedIndex = 1;
        }

        void SCREEN_CLEAR()
        {
            ssList.ActiveSheet.Cells[2, 0, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Text = "";
            ssList.ActiveSheet.Cells[2, 0, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Border = emptyBorder;
            ssList.Enabled = false;

            ssList.ActiveSheet.Cells[2, 0, ssList.ActiveSheet.Rows.Count - 1, 0].Border = emptyBorder;

            ssList.ActiveSheet.Cells[2, 5, ssList.ActiveSheet.Rows.Count - 1, 6].Border = emptyBorder;

            ssList.ActiveSheet.Cells[2, 6, ssList.ActiveSheet.Rows.Count - 1, 6].Border = emptyBorder;
        }

        void btnCancel_Click()
        {
            btnView.Enabled = true;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            cboYYMM.Focus();
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;         

            strTitle = "외 래 환 자 통계 (" + cboYYMM.SelectedItem.ToString().Trim() + ")";
            strSubTitle = clsPublic.GstrSysDate;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);          
        }

        void eGetData()
        {
            int i = 0;
            //int j = 0;
            int nCount = 0;
            int nDayCount = 0;
            //int nGigan = 0;
            int nRow = 0;

            string strOldDept = "";
            string strSDATE = "";
            string strEDATE = "";

            double nGwaTotal = 0;
            double nTotal = 0;
            double nTotal2 = 0;
            double nTotal3 = 0;

            btnView.Enabled = false;

            //SCREEN_CLEAR();

            strSDATE = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + "-01-01";
            strEDATE = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + "-12-31"; 

            //nGigan = Convert.ToInt32(strEDATE) - Convert.ToInt32(strSDATE) + 1;

            //SQL = "";
            //SQL += ComNum.VBLF + "SELECT ";
            //SQL += ComNum.VBLF + "  COUNT(HOLYDAY) holyday";
            //SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_JOB";
            //SQL += ComNum.VBLF + "WHERE 1=1";
            //SQL += ComNum.VBLF + "      AND JOBDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
            //SQL += ComNum.VBLF + "      AND JOBDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
            //SQL += ComNum.VBLF + "      AND HOLYDAY ='*'";

            //SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //    return;
            //}

            //if(dt.Rows.Count > 0)
            //{
            //    nGigan = nGigan - Convert.ToInt32(VB.Val(dt.Rows[0]["Holyday"].ToString().Trim()));
            //}

            //dt.Dispose();
            //dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "A.DEPT, B.DRNAME, SUM(SININWON+GUINWON+ILBAN)  SUMINWON, COUNT(ACTDATE) NDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_OPDILJI A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.ACTDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND A.ACTDATE <=TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND A.DRCODE = B.DRCODE(+)";
            SQL += ComNum.VBLF + "      AND B.DRDEPT1 = C.DEPTCODE(+)";
            SQL += ComNum.VBLF + "      AND SININWON+GUINWON+ILBAN <>'0'";
            SQL += ComNum.VBLF + "      AND A.DEPT NOT IN (SELECT WARDCODE FROM BAS_WARD)";
            SQL += ComNum.VBLF + "GROUP BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME ";
            SQL += ComNum.VBLF + "ORDER BY C.PRINTRANKING,B.PRINTRANKING,A.DEPT,B.DRNAME ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    nCount += 1;
                }

                ssList.ActiveSheet.Rows.Count = nCount + 4;
                SCREEN_CLEAR();
                strOldDept = "m";

                nRow = 3;
                i = 0;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    if(strOldDept != dt.Rows[i]["Dept"].ToString().Trim())
                    {
                        ssList.ActiveSheet.Cells[i + 2, 0, i + 2, ssList.ActiveSheet.Columns.Count - 1].Border = TlineBorder;

                        #region 진료과 세팅
                        switch (dt.Rows[i]["Dept"].ToString().Trim())
                        {
                            case "MD":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "내과";
                                break;

                            case "MR":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "류마티스내과";
                                break;

                            case "MC":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "심장내과";
                                break;

                            case "MI":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "감염내과";
                                break;

                            case "MG":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "소화기내과";
                                break;

                            case "MN":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "신장내과";
                                break;

                            case "ME":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "내분비내과";
                                break;

                            case "RM":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "재활의학과";
                                break;

                            case "MP":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "호흡기내과";
                                break;

                            case "FM":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "가정의학과";
                                break;

                            case "GS":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "외과";
                                break;

                            case "OG":                            
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "산부인과";
                                break;

                            case "PD":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "소아청소년과";
                                break;

                            case "OS":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "정형외과";
                                break;

                            case "NS":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "신경외과";
                                break;

                            case "CS":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "흉부외과";
                                break;

                            case "NP":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "정신건강의학과";
                                break;

                            case "EN":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "이비인후과";
                                break;

                            case "OT":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "안과";
                                break;

                            case "UR":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "비뇨기과";
                                break;

                            case "DM":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "피부과";
                                break;

                            case "DT":                            
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "치과";
                                break;

                            case "PC":                            
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "통증치료과";
                                break;

                            case "JU":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "주사실";
                                break;

                            case "SI":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "심전도실";
                                break;

                            case "NE":
                                ssList.ActiveSheet.Cells[i + 2, 0].Text = "신경과";
                                break;
                        }

                        #endregion

                        if (strOldDept != "m")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 5].Text = nGwaTotal.ToString();
                            ssList.ActiveSheet.Cells[nRow - 1, 6].Text = String.Format("{0:##0.0}", (double) nGwaTotal / (double) nDayCount);
                        }

                        strOldDept = dt.Rows[i]["Dept"].ToString().Trim();
                        nGwaTotal = 0;
                        nDayCount = 0;
                        nRow = i + 3;
                    }

                    ssList.ActiveSheet.Cells[i + 2, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();   //의사성명
                    ssList.ActiveSheet.Cells[i + 2, 2].Text = dt.Rows[i]["Suminwon"].ToString().Trim();
                    nGwaTotal = nGwaTotal + VB.Val(dt.Rows[i]["Suminwon"].ToString().Trim());

                    ssList.ActiveSheet.Cells[i + 2, 3].Text = dt.Rows[i]["nDate"].ToString().Trim();
                    if(nDayCount < Convert.ToInt32(VB.Val(dt.Rows[i]["nDate"].ToString().Trim())))
                    {
                        nDayCount = Convert.ToInt32(VB.Val(dt.Rows[i]["nDate"].ToString().Trim()));
                    }

                    if(dt.Rows[i]["nDate"].ToString().Trim() != "0")
                    {
                        ssList.ActiveSheet.Cells[i + 2, 4].Text = String.Format("{0:##0.0}", VB.Val(dt.Rows[i]["Suminwon"].ToString().Trim()) / VB.Val(dt.Rows[i]["nDate"].ToString().Trim()));
                    }
                }

                ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 18;
            }

            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Cells[nRow - 1, 5].Text = nGwaTotal.ToString();
            ssList.ActiveSheet.Cells[nRow - 1, 6].Text = String.Format("{0:##0.0}", (double)nGwaTotal / (double)nDayCount);

            for(i = 1; i <= nCount + 2; i++)
            {
                nTotal = nTotal + VB.Val(ssList.ActiveSheet.Cells[i - 1, 2].Text);
                nTotal2 = nTotal2 + VB.Val(ssList.ActiveSheet.Cells[i - 1, 5].Text);
                nTotal3 =  nTotal3 + VB.Val(ssList.ActiveSheet.Cells[i - 1, 6].Text);
            }

            ssList.ActiveSheet.Cells[nCount + 3, 0].Text = "합계";
            ssList.ActiveSheet.Cells[nCount + 3, 2].Text = nTotal.ToString();
            ssList.ActiveSheet.Cells[nCount + 3, 5].Text = nTotal2.ToString();
            ssList.ActiveSheet.Cells[nCount + 3, 6].Text = nTotal3.ToString();

            nSSRowCount = nCount + 4;

            //ssList.ActiveSheet.Cells[nCount + 3, 0, nCount + 3, ssList.ActiveSheet.Columns.Count - 1].Border = lineBorder;

            ssList.Enabled = true;

            btnCancel.Enabled = true;
            btnPrint.Enabled = true;

            ssList.ActiveSheet.Rows[ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data)].Border = TlineBorder;
        }
    }
}
