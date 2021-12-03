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
    /// File Name       : frmOPDInwonYearPrint.cs
    /// Description     : 월별 외래환자수 및 주사실 현황
    /// Author          : 안정수
    /// Create Date     : 2018-02-02
    /// TODO : 실데이터로 검증 필요함, 본소스에서는 조회하자마자 오류발생하여 확인 불가
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 frmnrtong32.frm(FrmOPDInwonYearPrint) 폼 frmOPDInwonYearPrint.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\frmnrtong32.frm(FrmOPDInwonYearPrint) >> frmOPDInwonYearPrint.cs 폼이름 재정의" />
    public partial class frmOPDInwonYearPrint : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsNurse CN = new clsNurse();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        //int nCurrRow = 0;
        //int nCurrCol = 0;

        string[,] strData = new string[6, 16];

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

        public frmOPDInwonYearPrint(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOPDInwonYearPrint()
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

            this.cboYYMM.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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
            int nMM = 0;
            string strYYMM = "";
            int i = 0;

            nYY = Convert.ToInt32(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = Convert.ToInt32(VB.Mid(clsPublic.GstrSysDate, 6, 2));
            strYYMM = ComFunc.SetAutoZero(nYY.ToString(), 4) + ComFunc.SetAutoZero(nMM.ToString(), 2);

            cboYYMM.Items.Clear();

            for(i = 1; i <= 24; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년" + VB.Right(strYYMM, 2) + "월");
                strYYMM = CN.DATE_YYMM_ADD(strYYMM, -1);

                if(strYYMM == "199712")
                {
                    break;
                }
            }

            cboYYMM.SelectedIndex = 1;
        }

        void SCREEN_CLEAR()
        {
            ssList.ActiveSheet.Cells[2, 1, ssList.ActiveSheet.Rows.Count - 1, 8].Text = "";
            ssList.Enabled = false;

            ssList.ActiveSheet.Cells[2, 10, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Text = "";
            ssList.Enabled = false;
        }

        void btnCancel_Click()
        {
            SCREEN_CLEAR();
            btnView.Enabled = true;
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
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

            string PrintDate = "";
            string JobDate = "";
            string JobMan = "";

            PrintDate = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월" + VB.Mid(clsPublic.GstrSysDate, 9, 2) + "일 " + VB.Right(clsPublic.GstrSysDate, 5);
            JobDate = cboYYMM.SelectedItem.ToString().Trim() + "분";
            JobMan = clsType.User.JobMan;

            strTitle = "외래환자수 및 주사실 현황";
            strSubTitle = "통 계 월 : " + JobDate;
            strSubTitle += "\r\n" + "출 력 자 : " + JobMan;
            

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);     
        }

        void eGetData()
        {
            int i = 0;
            //int j = 0;

            string strYYMM = "";
            string strBYYMM = "";

            int nGita = 0;
            int nGigan = 0;
            double nJusaTot = 0;
            string strSDATE = "";
            string strEDATE = "";
            int nRow = 0;
            //int nCol = 0;
            int[] nTotal = new int[8];
            int nInwon1 = 0;
            int nInwon2 = 0;

            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + VB.Mid(cboYYMM.SelectedItem.ToString().Trim(), 6, 2);
            strBYYMM = CN.DATE_YYMM_ADD(strYYMM, -1);

            strSDATE = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + "-" + VB.Mid(cboYYMM.SelectedItem.ToString().Trim(), 6, 2) + "-01";
            strEDATE = CF.READ_LASTDAY(clsDB.DbCon, strSDATE);

            nGigan = Convert.ToInt32(strEDATE.Replace("-", "")) - Convert.ToInt32(strSDATE.Replace("-", "")) + 1;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  COUNT(HOLYDAY) holyday";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_JOB";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND JOBDATE >=TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND JOBDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND HOLYDAY ='*'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nGigan = nGigan - Convert.ToInt32(VB.Val(dt.Rows[0]["Holyday"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //외래 환자 통계
            for (i = 1; i <= 7; i++)
            {
                nTotal[i] = 0;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  YYMM, DEPTCODE, SININWON+GUINWON INWON, RNINWON, NAINWON, DNNAINWON, ETCINWON";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TONG2";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND YYMM >= '" + strBYYMM + "'";
            SQL += ComNum.VBLF + "      AND YYMM <= '" + strYYMM + "'";
            SQL += ComNum.VBLF + "      AND DEPTCODE IN ('MD','GS','OG','PD','OS','NS','CS','NP','EN','OT','UR','DM','DT','JU','SI','NE','RM', 'MC','ME','MG','MN','MP','MR','MI')";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    #region Dept_Check_Row(GoSub)

                    switch (dt.Rows[i]["DeptCode"].ToString().Trim())
                    {
                        case "MD":
                        case "MC":
                        case "ME":
                        case "MG":
                        case "MN":
                        case "MP":
                        case "MR":
                        case "MI":
                            nRow = 0;
                            break;

                        case "GS":
                            nRow = 1;
                            break;

                        case "OG":
                            nRow = 2;
                            break;

                        case "PD":
                            nRow = 3;
                            break;

                        case "OS":
                            nRow = 4;
                            break;

                        case "NS":
                            nRow = 5;
                            break;

                        case "CS":
                            nRow = 6;
                            break;

                        case "NE":
                            nRow = 7;
                            break;

                        case "NP":
                            nRow = 8;
                            break;

                        case "EN":
                            nRow = 9;
                            break;

                        case "OT":
                            nRow = 10;
                            break;

                        case "UR":
                            nRow = 11;
                            break;

                        case "DM":
                            nRow = 12;
                            break;

                        case "DT":
                            nRow = 13;
                            break;

                        case "JU":
                            nRow = 14;
                            break;

                        case "SI":
                            nRow = 15;
                            break;

                        case "ED":
                            nRow = 16;
                            break;

                        case "RM":
                            nRow = 17;
                            break;

                        //default:
                        //    nRow = 0;
                        //    break;
                    }

                    #endregion

                    if (dt.Rows[i]["YYMM"].ToString().Trim() == strBYYMM)
                    {
                        ssList.ActiveSheet.Cells[nRow + 2, 1].Text = dt.Rows[i]["Inwon"].ToString().Trim();
                        nTotal[1] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon"].ToString().Trim()));
                    }

                    else
                    {
                        ssList.ActiveSheet.Cells[nRow + 2, 2].Text = VB.Val(dt.Rows[i]["Inwon"].ToString().Trim()).ToString();
                        nTotal[2] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon"].ToString().Trim()));

                        ssList.ActiveSheet.Cells[nRow + 2, 4].Text = String.Format("{0:##0.0}", Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon"].ToString().Trim())) / nGigan);

                        ssList.ActiveSheet.Cells[nRow + 2, 5].Text = String.Format("{0:##0.0}", Convert.ToInt32(VB.Val(dt.Rows[i]["RnInwon"].ToString().Trim())) / nGigan);
                        nTotal[3] += Convert.ToInt32(VB.Val(dt.Rows[i]["RnInwon"].ToString().Trim()));

                        ssList.ActiveSheet.Cells[nRow + 2, 6].Text = String.Format("{0:##0.0}", Convert.ToInt32(VB.Val(dt.Rows[i]["NaInwon"].ToString().Trim())) / nGigan);
                        nTotal[4] += Convert.ToInt32(VB.Val(dt.Rows[i]["NaInwon"].ToString().Trim()));

                        ssList.ActiveSheet.Cells[nRow + 2, 7].Text = String.Format("{0:##0.0}", Convert.ToInt32(VB.Val(dt.Rows[i]["Dnnainwon"].ToString().Trim())) + Convert.ToInt32(VB.Val(dt.Rows[i]["NaInwon"].ToString().Trim())) / nGigan);
                        nTotal[5] += Convert.ToInt32(VB.Val(dt.Rows[i]["Dnnainwon"].ToString().Trim())) + Convert.ToInt32(VB.Val(dt.Rows[i]["EtcInwon"].ToString().Trim()));
                    }
                }
            }

            dt.Dispose();
            dt = null;

            for(i = 2; i < ssList.ActiveSheet.Rows.Count; i++)
            {
                nInwon1 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 1].Text));
                nInwon2 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 2].Text));

                if(nInwon1 != 0)
                {
                    ssList.ActiveSheet.Cells[i, 3].Text = String.Format("{0:##,###,##0.0}", ((double)nInwon2 / (double)nInwon1) * 100 - 100);
                }
            }

            ssList.ActiveSheet.Cells[20, 1].Text = nTotal[1].ToString();
            ssList.ActiveSheet.Cells[20, 2].Text = nTotal[2].ToString();

            if(nTotal[1] != 0)
            {
                ssList.ActiveSheet.Cells[20, 3].Text = String.Format("{0:##,###,##0.0}", ((double)nTotal[2] / (double)nTotal[1]) * 100 - 100);                
            }

            ssList.ActiveSheet.Cells[20, 4].Text = String.Format("{0:##0.0}", ((double)nTotal[2] / nGigan));
            ssList.ActiveSheet.Cells[20, 5].Text = String.Format("{0:##0.0}", ((double)nTotal[3] / nGigan));
            ssList.ActiveSheet.Cells[20, 6].Text = String.Format("{0:##0.0}", ((double)nTotal[4] / nGigan));
            ssList.ActiveSheet.Cells[20, 7].Text = String.Format("{0:##0.0}", ((double)nTotal[5] / nGigan));

            //주사실

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.QTY1 , B.NAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TONG3 A, " + ComNum.DB_PMPA + "NUR_CODE B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.CODE = B.CODE";
            SQL += ComNum.VBLF + "      AND A.YYMM= '" + strYYMM + "'";
            SQL += ComNum.VBLF + "      AND A.WARDCODE ='JU'";
            SQL += ComNum.VBLF + "      AND B.GUBUN = '3'";
            SQL += ComNum.VBLF + "ORDER BY B.PRINTRANKING";

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
                ComFunc.MsgBox("해당월에는 주사통계가 없습니다.");
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    nJusaTot += VB.Val(dt.Rows[i]["Qty1"].ToString().Trim());

                    if(i > 16)
                    {
                        nGita += Convert.ToInt32(VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()));
                    }

                    else
                    {
                        ssList.ActiveSheet.Cells[i + 2, 9].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i + 2, 10].Text = dt.Rows[i]["Qty1"].ToString().Trim();

                        if(VB.Val(dt.Rows[i]["Qty1"].ToString().Trim()) != 0)
                        {
                            ssList.ActiveSheet.Cells[i + 2, 11].Text = String.Format("{0:##0.0}", Convert.ToInt32(VB.Val(dt.Rows[i]["Qty1"].ToString().Trim())) / nGigan);
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[i + 2, 11].Text = 0.ToString();
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Cells[20, 10].Text = nGigan.ToString();

            if(nGigan != 0)
            {
                ssList.ActiveSheet.Cells[20, 11].Text = String.Format("{0:##0.0}", nGita / nGigan);
            }

            else
            {
                ssList.ActiveSheet.Cells[20, 11].Text = 0.ToString();
            }

            ssList.ActiveSheet.Cells[21, 10].Text = nJusaTot.ToString();

            if(nJusaTot != 0)
            {
                ssList.ActiveSheet.Cells[21, 11].Text = String.Format("{0:##0.0}", nJusaTot / nGigan);
            }

            else
            {
                ssList.ActiveSheet.Cells[21, 11].Text = 0.ToString();
            }

            btnPrint.Enabled = true;

        }


    }
}
