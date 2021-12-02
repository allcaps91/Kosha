using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmORInwonYearPrint.cs
    /// Description     : 월별 병동별 입원(재원) 환자수
    /// Author          : 안정수
    /// Create Date     : 2018-02-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 nrtong35.frm(FrmORIpwonYearPrint) 폼 frmORInwonYearPrint.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrTong\nrtong35.frm(FrmORIpwonYearPrint) >> frmORInwonYearPrint.cs 폼이름 재정의" />
    public partial class frmORInwonYearPrintNew : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        double[,,] nDATA = new double[3, 27, 28];   //병동
        int[,] nDATA2 = new int[3, 28];             //수술실
        int[,] nDATA3 = new int[5, 28];             //응급실

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

        public frmORInwonYearPrintNew(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmORInwonYearPrintNew()
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
            ssList.ActiveSheet.Cells[1, 2, ssList.ActiveSheet.Rows.Count - 1, ssList.ActiveSheet.Columns.Count - 1].Text = "";
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
            bool PrePrint = false;

            string PrintDate = "";
            string JobDate = "";
            string JobMan = "";

            PrintDate = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Mid(clsPublic.GstrSysDate, 9, 2) + "일 ";
            JobDate = cboYYMM.SelectedItem.ToString().Trim() + "분";
            JobMan = clsType.User.JobName;            

            strTitle = "월별 병동별  입원.재원 환자수";

            strSubTitle = "통 계 월 : " + JobDate + "";
            strSubTitle += "\r\n" + "출력일자 : " + PrintDate + "";
            strSubTitle += "\r\n" + "출 력 자 : " + JobMan + "";            

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, false, false, (float)0.85);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);       
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
            //int nBIlsu = 0;
            int nTobed = 0;

            //int nRNTot = 0;
            //int nNATot = 0;
            //int nTotal1 = 0;
            //int nTotal2 = 0;

            double nSum1 = 0;
            double nSum2 = 0;
            int nCnt1 = 0;

            int[] nDDCount = new int[11];

            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4);
            strBYYMM = (Convert.ToInt32(strYYMM) - 1).ToString() + "01";
            //strBYYMM = "202001";
            strYYMM += "12";
            //strYYMM += "01";

            for (i = 0; i <= 2; i++)
            {
                for(j = 0; j <= 20; j++)
                {
                    for(k = 0; k <= 20; k++)
                    {
                        nDATA[i, j, k] = 0;
                    }
                }
            }

            for(i = 0; i <= 10; i++)
            {
                nDDCount[i] = 0;
            }

            for(i = 0; i <= 20; i++)
            {
                nDATA3[1, i] = 0;
                nDATA3[2, i] = 0;
                nDATA3[3, i] = 0;
                nDATA3[4, i] = 0;

                nDATA2[1, i] = 0;
                nDATA2[2, i] = 0;
            }

            //입원 / 재원 환자수 및 병상 가동율
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  YYMM, DEPTCODE, WARDCODE, SUM(TOTBED) TOTBED, SUM(IPINWON) IPINWON,";
            SQL += ComNum.VBLF + "  SUM(JEWON) JEWON, SUM(TEWON) TEWON, SUM(DELIVERY) DELIVERY,";
            SQL += ComNum.VBLF + "  SUM(DELIVERY2) DELIVERY2, SUM(DELIVERY3) DELIVERY3 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TONG1";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND YYMM >= '" + strBYYMM + "'";
            SQL += ComNum.VBLF + "      AND YYMM <= '" + strYYMM + "'";
            SQL += ComNum.VBLF + "      AND WARDCODE IN ('NR','33','35','40','4H','50','53','55','60','63','65','70','73','75','80','83')";
            SQL += ComNum.VBLF + "GROUP BY YYMM, DEPTCODE, WARDCODE";

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

            if (dt.Rows.Count > 0)
            {
                nIlsu = 365;
                //nBIlsu = 365;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strWard = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();

                    #region SEARCH_WARD(GoSub)

                    switch (strWard.Trim().ToUpper())
                    {
                        case "NR":
                            nRow = 1;
                            break;

                        case "33":
                            nRow = 2;
                            break;

                        case "35":
                            nRow = 3;
                            break;

                        case "40":
                            nRow = 4;
                            break;

                        case "4H":
                            nRow = 5;
                            break;

                        case "50":
                            nRow = 6;
                            break;

                        case "53":
                            nRow = 7;
                            break;

                        case "55":
                            nRow = 8;
                            break;

                        case "60":
                            nRow = 9;
                            break;

                        case "63":
                            nRow = 10;
                            break;

                        case "65":
                            nRow = 11;
                            break;

                        case "70":
                            nRow = 12;
                            break;

                        case "73":
                            nRow = 13;
                            break;

                        case "75":
                            nRow = 14;
                            break;

                        case "80":
                            nRow = 15;
                            break;

                        case "83":
                            nRow = 16;
                            break;

                        default:
                            nRow = 0;
                            break;
                    }



                    #endregion

                    #region SEARCH_DEPT(GoSub)

                    switch (strDept.Trim().ToUpper())
                    {
                        case "MD":                        
                            nCol = 1;
                            break;

                        case "MC":
                            nCol = 2;
                            break;

                        case "ME":
                            nCol = 3;
                            break;

                        case "MG":
                            nCol = 4;
                            break;

                        case "MN":
                            nCol = 5;
                            break;

                        case "MP":
                            nCol = 6;
                            break;

                        case "MR":
                            nCol = 7;
                            break;

                        case "MI":
                            nCol = 8;
                            break;

                        case "GS":
                            nCol = 9;
                            break;

                        case "OG":
                        case "GY":
                            nCol = 10;
                            break;

                        case "PD":
                        case "IQ":
                        case "DB":
                            nCol = 11;
                            break;

                        case "OS":
                            nCol = 12;
                            break;

                        case "NS":
                            nCol = 13;
                            break;

                        case "CS":
                            nCol = 14;
                            break;

                        case "NP":
                            nCol = 15;
                            break;

                        case "EN":
                            nCol = 16;
                            break;

                        case "OT":
                            nCol = 17;
                            break;

                        case "UR":
                            nCol = 18;
                            break;

                        case "DM":
                            nCol = 19;
                            break;

                        case "DT":
                            nCol = 20;
                            break;

                        case "PC":
                            nCol = 21;
                            break;

                        case "NE":
                            nCol = 22;
                            break;

                        default:
                            nCol = 0;
                            break;
                    }

                    #endregion

                    if (VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) == VB.Left(strBYYMM, 4))
                    {
                        nDATA[1, nRow, 23] = nDATA[1, nRow, 23] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, nRow, 23] = nDATA[2, nRow, 23] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());

                        //분만건수(전년)
                        if(strWard.Trim() == "55")
                        {
                            nDDCount[1] = nDDCount[1] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery"].ToString().Trim()));    //정상분만
                            nDDCount[2] = nDDCount[2] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim()));   //이상분만
                            nDDCount[3] = nDDCount[3] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim()));   //재왕절개
                        }

                        //정상신생아(전년)
                        if(strWard.Trim() == "NR" || strWard.Trim() == "ND" && dt.Rows[i]["DeptCode"].ToString().Trim() == "")
                        {
                            nDDCount[7] = nDDCount[7] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));     //입원
                            nDDCount[8] = nDDCount[8] + Convert.ToInt32(VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()));       //재원
                        }
                    }
                    else
                    {
                        //분만건수(금년)
                        if(strWard.Trim() == "55")
                        {
                            nDDCount[4] = nDDCount[4] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery"].ToString().Trim()));    //정상분만
                            nDDCount[5] = nDDCount[5] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery2"].ToString().Trim()));   //이상분만
                            nDDCount[6] = nDDCount[6] + Convert.ToInt32(VB.Val(dt.Rows[i]["Delivery3"].ToString().Trim()));   //재왕절개
                        }

                        //정상신생아(당월)
                        //정상신생아(금년)
                        if (strWard.Trim() == "NR" || strWard.Trim() == "ND" && dt.Rows[i]["DeptCode"].ToString().Trim() == "")
                        {
                            nDDCount[9] = nDDCount[9] + Convert.ToInt32(VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim()));     //입원
                            nDDCount[10] = nDDCount[10] + Convert.ToInt32(VB.Val(dt.Rows[i]["Jewon"].ToString().Trim()));     //재원
                        }

                        if(dt.Rows[i]["DeptCode"].ToString().Trim() != "")
                        {
                            nDATA[1, nRow, 24] = nDATA[1, nRow, 24] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                            nDATA[2, nRow, 24] = nDATA[2, nRow, 24] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        }
                    }

                    //전년
                    if (VB.Left(dt.Rows[i]["YYMM"].ToString().Trim(), 4) == VB.Left(strBYYMM, 4))
                    {
                        nDATA[1, 17, nCol] = nDATA[1, 17, nCol] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, 17, nCol] = nDATA[2, 17, nCol] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                    }
                    else
                    {
                        //bed수total에서 NR,EM,HD 제외
                        if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "NR" || dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER" || dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "HD")
                        {
                            nDATA[0, nRow, 20] = VB.Val(dt.Rows[i]["Totbed"].ToString().Trim());
                        }

                        nDATA[1, 18, nCol] = nDATA[1, 18, nCol] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, 18, nCol] = nDATA[2, 18, nCol] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                        nDATA[1, nRow, nCol] = nDATA[1, nRow, nCol] + VB.Val(dt.Rows[i]["IpInwon"].ToString().Trim());
                        nDATA[2, nRow, nCol] = nDATA[2, nRow, nCol] + VB.Val(dt.Rows[i]["Jewon"].ToString().Trim());
                    }
                }
            }

            dt.Dispose();
            dt = null;
            
            //Display
            for(i = 1; i <= 17;i++)
            {
                for (j = 1; j <= 22; j++)
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, j + 1].Text = nDATA[1, i, j].ToString();
                    ssList.ActiveSheet.Cells[i * 2 , j + 1].Text = nDATA[2, i, j].ToString();

                }

                if(i == 2)
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, 24].Text = (nDATA[1, i, 23] - Convert.ToDouble(nDDCount[7])).ToString();
                    nDATA[1, 18, 23] = nDATA[1, 18, 23] + nDATA[1, i, 23] - Convert.ToDouble(nDDCount[7]);
                }
                else
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, 24].Text = nDATA[1, i, 23].ToString();
                    nDATA[1, 18, 23] = nDATA[1, 18, 23] + nDATA[1, i, 23];
                }

                if(i == 2)
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, 25].Text = (nDATA[1, i, 24] - Convert.ToDouble(nDDCount[9])).ToString();
                    nDATA[1, 18, 24] = nDATA[1, 18, 24] + nDATA[1, i, 24] - Convert.ToDouble(nDDCount[9]);
                }
                else
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, 25].Text = nDATA[1, i, 24].ToString();
                    nDATA[1, 18, 24] = nDATA[1, 18, 24] + nDATA[1, i, 24];
                }

                if (i == 2)
                {
                    if (nDATA[1, i, 23] != 0)
                    {
                        ssList.ActiveSheet.Cells[(i * 2) - 1, 26].Text = String.Format("{0:##0.0}", ((nDATA[1, i, 24] / (nDATA[1, i, 23] - Convert.ToDouble(nDDCount[7])))) * 100 - 100);                            
                    }
                }
                else
                {
                    if (nDATA[1, i, 23] != 0)
                    {
                        ssList.ActiveSheet.Cells[(i * 2) - 1, 26].Text = String.Format("{0:##0.0}", (nDATA[1, i, 24] / nDATA[1, i, 23]) * 100 - 100);                            
                    }
                }

                ssList.ActiveSheet.Cells[(i * 2) - 1, 27].Text = String.Format("{0:##0.0}", nDATA[1, i, 24] / (double) nIlsu);

                if(nDATA[1, i, 24] != 0)
                {
                    ssList.ActiveSheet.Cells[(i * 2) - 1, 28].Text = String.Format("{0:##0.0}", ((nDATA[2, i, 24] / (double)nIlsu) / nDATA[0, i, 20]) * 100);
                }

                if(i == 11)
                {
                    ssList.ActiveSheet.Cells[i * 2, 24].Text = (nDATA[2, i, 23] - Convert.ToDouble(nDDCount[8])).ToString();
                    nDATA[2, 18, 23] = nDATA[2, 18, 23] + nDATA[2, i, 23] - Convert.ToDouble(nDDCount[8]);
                }
                else
                {
                    ssList.ActiveSheet.Cells[i * 2, 24].Text = nDATA[2, i, 23].ToString();
                    nDATA[2, 18, 23] = nDATA[2, 18, 23] + nDATA[2, i, 23];
                }

                if(i == 2)
                {
                    ssList.ActiveSheet.Cells[i * 2, 25].Text = (nDATA[2, i, 24] - Convert.ToDouble(nDDCount[10])).ToString();
                    nDATA[2, 18, 24] = nDATA[2, 18, 24] + nDATA[2, i, 24] - Convert.ToDouble(nDDCount[10]);
                }
                else
                {
                    ssList.ActiveSheet.Cells[i * 2, 25].Text = nDATA[2, i, 24].ToString();
                    nDATA[2, 18, 24] = nDATA[2, 18, 24] + nDATA[2, i, 24];
                }

                if (i == 2)
                {
                    if (nDATA[2, i, 23] != 0)
                    {
                        ssList.ActiveSheet.Cells[i * 2, 26].Text = String.Format("{0:##0.0}", ((nDATA[2, i, 24] / (nDATA[2, i, 23] - Convert.ToDouble(nDDCount[8])))) * 100 - 100);
                    }
                }
                else
                {
                    if (nDATA[2, i, 23] != 0)
                    {
                        ssList.ActiveSheet.Cells[i * 2, 19].Text = String.Format("{0:##0.0}", (nDATA[2, i, 24] / nDATA[2, i, 23]) * 100 - 100);
                    }
                    
                }

                ssList.ActiveSheet.Cells[i * 2, 27].Text = String.Format("{0:##0.0}", nDATA[2, i, 24] / (double)nIlsu);


                //총 Bed수
                if(i != 2)
                {
                    nTobed = Convert.ToInt32(Convert.ToDouble(nTobed) + nDATA[0, i, 24]);
                }
            }

            for(i = 1; i <= 22; i++)
            {
                ssList.ActiveSheet.Cells[33, i + 1].Text = nDATA[1, 18, i].ToString();
                ssList.ActiveSheet.Cells[34, i + 1].Text = nDATA[2, 18, i].ToString();

                if(nDATA[1, 17, i] != 0)
                {
                    ssList.ActiveSheet.Cells[35, i + 1].Text = String.Format("{0:##0.0}", (nDATA[1, 18, i] / nDATA[1, 17, i]) * 100 - 100);
                }

                if (nDATA[2, 17, i] != 0)
                {
                    ssList.ActiveSheet.Cells[36, i + 1].Text = String.Format("{0:##0.0}", (nDATA[2, 18, i] / nDATA[2, 17, i]) * 100 - 100);
                }

                //ssList의 MaxRowCount 수 보다 많으므로 주석처리함
                ssList.ActiveSheet.Cells[37, i + 1].Text = String.Format("{0:##0.0}", nDATA[1, 18, i] / (double)nIlsu);
                ssList.ActiveSheet.Cells[38, i + 1].Text = String.Format("{0:##0.0}", nDATA[2, 14, i] / (double)nIlsu);
            }

            for(j = 3; j <= 28; j++)
            {
                nSum1 = 0;
                nSum2 = 0;
                for(i = 2; i <= 33; i++)
                {
                    //입원
                    if(i % 2 == 0)
                    {
                        nSum1 = nSum1 + VB.Val(ssList.ActiveSheet.Cells[i - 1, j - 1].Text);
                    }

                    //재원
                    else if(i % 2 == 1)
                    {
                        nSum2 = nSum2 + VB.Val(ssList.ActiveSheet.Cells[i - 1, j - 1].Text);
                    }
                }

                ssList.ActiveSheet.Cells[33, j - 1].Text = String.Format("{0:##,###,##0.0}", nSum1);
                ssList.ActiveSheet.Cells[34, j - 1].Text = String.Format("{0:##,###,##0.0}", nSum2);
            }

            //병상이용율
            for(i = 2; i <= 33; i++)
            {
                //입원
                if(i % 2 == 0)
                {
                    nSum1 = nSum1 + VB.Val(ssList.ActiveSheet.Cells[i - 1, 28].Text);

                    if(VB.Val(ssList.ActiveSheet.Cells[i - 1, 28].Text) > 0)
                    {
                        nCnt1 = nCnt1 + 1;
                    }
                }
            }

            if(nCnt1 > 0)
            {
                ssList.ActiveSheet.Cells[33, 28].Text = String.Format("{0:##,###,##0.0}", nSum1 / (double) nCnt1);
            }

            else
            {
                ssList.ActiveSheet.Cells[33, 28].Text = "0.0";
            }

            btnPrint.Enabled = true;
        }

    }
}
