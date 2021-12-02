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
    /// File Name       : frmORInwonYearPrint2.cs
    /// Description     : 월별 마취, 분만실 현황
    /// Author          : 안정수
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong33.frm(FrmORInwonYearPrint2) 폼 frmORInwonYearPrint2.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong33.frm(FrmORInwonYearPrint2) >> frmORInwonYearPrint2.cs 폼이름 재정의" />
    public partial class frmORInwonYearPrint2 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        double[,,] nDATA = new double[3, 22, 22];   //병동
        int[,] nDATA2 = new int[3, 22];             //수술실
        int[,] nDATA3 = new int[5, 22];             //응급실

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

        public frmORInwonYearPrint2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmORInwonYearPrint2()
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

            //this.eControl.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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
                btnView.Enabled = true;
                btnPrint.Enabled = false;
                btnExit.Enabled = true;
            }
        }

        void Set_Init()
        {
            int nYY = 0;
            string strYY = "";
            int i = 0;

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
            ssList1.ActiveSheet.Cells[1, 2, 9, ssList1.ActiveSheet.Columns.Count - 1].Text = "";
            ssList1.Enabled = false;

            ssList1.ActiveSheet.Cells[13, 3, 13, ssList1.ActiveSheet.Columns.Count - 1].Text = "";
            ssList1.Enabled = false;
        }

        void btnCancel_Click()
        {
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

            string PrintDate = "";
            string strJobDate = "";
            string strJobMan = "";

            PrintDate = clsPublic.GstrSysDate;
            strJobDate = cboYYMM.SelectedItem.ToString().Trim();
            strJobMan = clsType.User.JobMan;

            strTitle = "월별  마취.응급실.신생아.분만 통계";
            strSubTitle = "통 계 월 : " + strJobDate;
            strSubTitle += "\r\n" + "출 력 자 : " + strJobMan;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, false, true, true, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            string strYYMM = "";
            string strBYYMM = "";

            int nRow = 0;
            int nCol = 0;

            string strDept = "";
            string strWard = "";

            int nIlsu = 0;
            int nBIlsu = 0;
            //int nTobed = 0;

            //int nRNTot = 0;
            //int nNATot = 0;
            //int nTotal1 = 0;
            //int nTotal2 = 0;
            int[] nDDCount = new int[12];

            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4);
            strBYYMM = (Convert.ToInt32(strYYMM) - 1).ToString() + "01";
            strYYMM = VB.Left(strYYMM, 4) + "12";

            //clear
            for(i = 0; i < 3; i++)
            {
                for(j = 0; j < 22; j++)
                {
                    for(k = 0; k < 22; k++)
                    {
                        nDATA[i, j, j] = 0;
                    }
                }
            }

            for(i = 0; i < nDDCount.Length; i++)
            {
                nDDCount[i] = 0;
            }

            for(i = 0; i < 22; i++)
            {
                nDATA3[1, i] = 0;
                nDATA3[2, i] = 0;
                nDATA3[3, i] = 0;
                nDATA3[4, i] = 0;
            }

            for(i = 0; i < 22; i++)
            {
                nDATA2[1, i] = 0;
                nDATA2[2, i] = 0;
            }

            //입원 / 재원 환자수 및 병상 가동율
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  YYMM, DEPTCODE, WARDCODE,SUM(IPINWON) IPINWON,";
            SQL += ComNum.VBLF + "  SUM(JEWON) JEWON, SUM(TEWON) TEWON,SUM(DELIVERY) DELIVERY,";
            SQL += ComNum.VBLF + "  SUM(DELIVERY2) DELIVERY2,SUM(DELIVERY3) DELIVERY3";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TONG1";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND YYMM >= '" + strBYYMM + "'";
            SQL += ComNum.VBLF + "      AND YYMM <= '" + strYYMM + "'";
            SQL += ComNum.VBLF + "      AND WARDCODE in ('NR','DR','ER','ND','3C','6A')";
            SQL += ComNum.VBLF + "GROUP BY YYMM,DEPTCODE,WARDCODE";

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
                ComFunc.MsgBox("해당월에는 아직 간호부 월통계 BUILD가  되지 않았습니다");
                return;
            }

            nIlsu = 365;
            nBIlsu = 365;

            if (dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strWard = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();

                    #region SEARCH_WARD(GoSub)

                    switch (strWard.Trim().ToUpper())
                    {
                        case "8W":
                            nRow = 1;
                            break;

                        case "7W":
                            nRow = 2;
                            break;

                        case "6W":
                            nRow = 3;
                            break;

                        case "5W":
                            nRow = 4;
                            break;

                        case "4A":
                            nRow = 5;
                            break;

                        case "3A":
                            nRow = 6;
                            break;

                        case "3B":
                            nRow = 7;
                            break;

                        case "SICU":
                            nRow = 8;
                            break;

                        case "MICU":
                            nRow = 9;
                            break;

                        case "2W":
                            nRow = 10;
                            break;

                        case "NR":
                        case "ND":
                            nRow = 11;
                            break;

                        case "DR":
                        case "3C":
                        case "6A":
                            nRow = 12;
                            break;
                    }

                    #endregion

                    #region SEARCH_DEPT(GoSub)

                    switch (strDept.Trim().ToUpper())
                    {
                        case "MD":
                            nCol = 1;
                            break;

                        case "GS":
                            nCol = 2;
                            break;

                        case "OG":
                        case "GY":
                            nCol = 3;
                            break;

                        case "PD":
                        case "IQ":
                        case "DB":
                            nCol = 4;
                            break;

                        case "OS":
                            nCol = 5;
                            break;

                        case "NS":
                            nCol = 6;
                            break;

                        case "CS":
                            nCol = 7;
                            break;

                        case "NP":
                            nCol = 8;
                            break;

                        case "EN":
                            nCol = 9;
                            break;

                        case "OT":
                            nCol = 10;
                            break;

                        case "UR":
                            nCol = 11;
                            break;

                        case "DM":
                            nCol = 12;
                            break;

                        case "DT":
                            nCol = 13;
                            break;

                        case "PC":
                            nCol = 14;
                            break;

                        case "RM":
                            nCol = 15;
                            break;

                        default:
                            nCol = 0;
                            break;
                    }

                    #endregion

                    if (strWard.Trim().ToUpper() == "ER")
                    {
                        if(VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) == VB.Left(strBYYMM, 4))
                        {
                            nDATA3[1, nCol] = nDATA3[1, nCol] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));
                            nDATA3[1, 20] = nDATA3[1, 20] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));
                            nDATA3[2, nCol] = nDATA3[2, nCol] + Convert.ToInt32(VB.Val(dt.Rows[i]["Tewon"].ToString().Trim()));
                            nDATA3[2, 20] = nDATA3[2, 20] + Convert.ToInt32(VB.Val(dt.Rows[i]["Tewon"].ToString().Trim()));
                        }

                        else
                        {
                            nDATA3[3, nCol] = nDATA3[3, nCol] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));
                            nDATA3[3, 20] = nDATA3[3, 21] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));
                            nDATA3[4, nCol] = nDATA3[4, nCol] + Convert.ToInt32(VB.Val(dt.Rows[i]["Tewon"].ToString().Trim()));
                            nDATA3[4, 20] = nDATA3[4, 21] + Convert.ToInt32(VB.Val(dt.Rows[i]["Tewon"].ToString().Trim()));
                        }
                    }

                    else
                    {
                        if (VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) == VB.Left(strBYYMM, 4))
                        {
                            nDATA[1, nRow, 20] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                            nDATA[2, nRow, 20] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());

                            //분만건수(전년)
                            if(strWard.Trim() == "DR" || strWard.Trim() == "3C" || strWard.Trim() == "6A")
                            {
                                nDDCount[1] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery"].ToString().Trim()));    //정상분만
                                nDDCount[2] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim()));   //이상분만
                                nDDCount[3] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim()));   //재왕절개
                            }

                            //정상신생아(전년)
                            if((strWard.Trim() == "NR" || strWard.Trim() == "ND") && dt.Rows[i]["Deptcode"].ToString().Trim() == "")
                            {
                                nDDCount[7] += Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));    //입원
                                nDDCount[8] += Convert.ToInt32(VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()));      //재원
                            }
                        }

                        else
                        {
                            //분만건수(당월)
                            if (strWard.Trim() == "DR" || strWard.Trim() == "3C" || strWard.Trim() == "6A")
                            {
                                nDDCount[4] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery"].ToString().Trim()));    //정상분만
                                nDDCount[5] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim()));   //이상분만
                                nDDCount[6] += Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim()));   //재왕절개
                            }

                            //정상신생아(당년)
                            if ((strWard.Trim() == "NR" || strWard.Trim() == "ND") && dt.Rows[i]["Deptcode"].ToString().Trim() == "")
                            {
                                nDDCount[9] += Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));     //입원
                                nDDCount[10] += Convert.ToInt32(VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()));      //재원
                            }

                            if(dt.Rows[i]["Deptcode"].ToString().Trim() != "")
                            {
                                nDATA[1, nRow, 21] += VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                                nDATA[2, nRow, 21] += VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                            }
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            for(i = 1; i <= 15; i++)
            {
                ssList1.ActiveSheet.Cells[3, i + 1].Text = nDATA3[1, i].ToString();
                ssList1.ActiveSheet.Cells[4, i + 1].Text = nDATA3[3, i].ToString();
                ssList1.ActiveSheet.Cells[5, i + 1].Text = nDATA3[2, i].ToString();
                ssList1.ActiveSheet.Cells[6, i + 1].Text = nDATA3[4, i].ToString();
                ssList1.ActiveSheet.Cells[7, i + 1].Text = String.Format("{0:##0.0}", nDATA3[3, i] / nIlsu);
                ssList1.ActiveSheet.Cells[8, i + 1].Text = String.Format("{0:##0.0}", nDATA3[4, i] / nIlsu);
            }

            ssList1.ActiveSheet.Cells[13, 2].Text = "(" + nDDCount[7] + ")";
            ssList1.ActiveSheet.Cells[13, 3].Text = nDDCount[9].ToString();

            ssList1.ActiveSheet.Cells[13, 4].Text = "(" + nDDCount[8] + ")";
            ssList1.ActiveSheet.Cells[13, 5].Text = nDDCount[10].ToString();

            ssList1.ActiveSheet.Cells[13, 6].Text = "(" + nDDCount[1] + ")";
            ssList1.ActiveSheet.Cells[13, 8].Text = nDDCount[4].ToString();

            ssList1.ActiveSheet.Cells[13, 9].Text = "(" + nDDCount[2] + ")";
            ssList1.ActiveSheet.Cells[13, 11].Text = nDDCount[5].ToString();

            ssList1.ActiveSheet.Cells[13, 12].Text = "(" + nDDCount[3] + ")";
            ssList1.ActiveSheet.Cells[13, 15].Text = nDDCount[6].ToString();

            ssList1.ActiveSheet.Cells[13, 16].Text = "(" + (nDDCount[1] + nDDCount[2] + nDDCount[3]) + ")";
            ssList1.ActiveSheet.Cells[13, 19].Text = (nDDCount[4] + nDDCount[5] + nDDCount[6]).ToString();

            //Total
            ssList1.ActiveSheet.Cells[3, 17].Text = nDATA3[1, 20].ToString();
            if(nBIlsu != 0)
            {
                ssList1.ActiveSheet.Cells[3, 20].Text = String.Format("{0:##0.0}", nDATA3[1, 20] / nBIlsu);
            }

            ssList1.ActiveSheet.Cells[4, 18].Text = nDATA3[3, 21].ToString();
            if (nBIlsu != 0)
            {
                ssList1.ActiveSheet.Cells[4, 20].Text = String.Format("{0:##0.0}", nDATA3[3, 21] / nBIlsu);
            }

            ssList1.ActiveSheet.Cells[5, 17].Text = nDATA3[2, 20].ToString();
            if (nBIlsu != 0)
            {
                ssList1.ActiveSheet.Cells[5, 20].Text = String.Format("{0:##0.0}", nDATA3[2, 20] / nBIlsu);
            }

            ssList1.ActiveSheet.Cells[6, 18].Text = nDATA3[4, 21].ToString();
            if (nBIlsu != 0)
            {
                ssList1.ActiveSheet.Cells[6, 20].Text = String.Format("{0:##0.0}", nDATA3[4, 21] / nBIlsu);
            }

            btnPrint.Enabled = true;
            ssList1.Enabled = true;

            //수술실
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  YYMM, DEPTCODE, CODE, QTY";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_TONG";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND YYMM >='" + VB.Right(strBYYMM, 6) + "'";
            SQL += ComNum.VBLF + "      AND YYMM <='" + VB.Right(strYYMM, 6) + "'";
            SQL += ComNum.VBLF + "      AND JONG='1'";

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
                ComFunc.MsgBox("수술실 통계가 BUILD 되지 않았습니다");
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();

                    #region SEARCH_DEPT(GoSub)

                    switch (strDept.Trim().ToUpper())
                    {
                        case "MD":
                            nCol = 1;
                            break;

                        case "GS":
                            nCol = 2;
                            break;

                        case "OG":
                        case "GY":
                            nCol = 3;
                            break;

                        case "PD":
                        case "IQ":
                        case "DB":
                            nCol = 4;
                            break;

                        case "OS":
                            nCol = 5;
                            break;

                        case "NS":
                            nCol = 6;
                            break;

                        case "CS":
                            nCol = 7;
                            break;

                        case "NP":
                            nCol = 8;
                            break;

                        case "EN":
                            nCol = 9;
                            break;

                        case "OT":
                            nCol = 10;
                            break;

                        case "UR":
                            nCol = 11;
                            break;

                        case "DM":
                            nCol = 12;
                            break;

                        case "DT":
                            nCol = 13;
                            break;

                        case "PC":
                            nCol = 14;
                            break;

                        case "RM":
                            nCol = 15;
                            break;

                        default:
                            nCol = 0;
                            break;
                    }

                    #endregion

                    if (VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) == VB.Left(strBYYMM, 4))
                    {
                        if (dt.Rows[i]["Code"].ToString().Trim() == "L")
                        {
                            nDATA2[2, 20] = nDATA2[2, 20] + Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        }

                        else
                        {
                            nDATA2[1, 20] = nDATA2[1, 20] +  Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        }                      
                    }

                    else
                    {
                        if (dt.Rows[i]["Code"].ToString().Trim() == "L")
                        {
                            nDATA2[2, nCol] = nDATA2[2, nCol] + Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                            nDATA2[2, 21] = nDATA2[2, 21] + Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        }

                        else
                        {
                            nDATA2[1, nCol] = nDATA2[1, nCol] +  Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                            nDATA2[1, 21] = nDATA2[1, 21] + Convert.ToInt32(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //수술실 Display
            for(i = 1; i <= 15; i++)
            {
                ssList1.ActiveSheet.Cells[1, i + 1].Text = nDATA2[1, i].ToString();
                ssList1.ActiveSheet.Cells[2, i + 1].Text = nDATA2[2, i].ToString();
            }

            ssList1.ActiveSheet.Cells[1, 16].Text = nDATA2[1, 20].ToString();   //전월
            ssList1.ActiveSheet.Cells[2, 16].Text = nDATA2[2, 20].ToString();

            ssList1.ActiveSheet.Cells[1, 17].Text = nDATA2[1, 21].ToString();   //금월
            ssList1.ActiveSheet.Cells[2, 17].Text = nDATA2[2, 21].ToString();

            if(nDATA2[1, 20] != 0)
            {
                ssList1.ActiveSheet.Cells[1, 18].Text = String.Format("{0:##,##0.0}", (((double)nDATA2[1, 21] / (double)nDATA2[1, 20]) * 100) - 100);                
            }

            if(nDATA2[2, 20] != 0)
            {
                ssList1.ActiveSheet.Cells[2, 18].Text = String.Format("{0:##,##0.0}", (((double)nDATA2[2, 21] / (double)nDATA2[2, 20]) * 100) - 100);
            }

            ssList1.ActiveSheet.Cells[1, 19].Text = String.Format("{0:##,##0.0}", (nDATA2[1, 21] / nIlsu));
            ssList1.ActiveSheet.Cells[2, 19].Text = String.Format("{0:##,##0.0}", (nDATA2[2, 21] / nIlsu));

            ssList1.Enabled = true;
        }
    }
}

