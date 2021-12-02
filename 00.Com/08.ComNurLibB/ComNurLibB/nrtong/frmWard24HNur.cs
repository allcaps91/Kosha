using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using System.Drawing.Printing;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmWard24HNur.cs
    /// Description     : 병동 24시간 보고서
    /// Author          : 안정수
    /// Create Date     : 2018-02-03
    /// Update History  :     
    /// 
    /// </summary>
    /// <history>  
    /// 기존 nrTong70_new.frm(Frm24보고서_NEW) 폼 frmWard24HNur.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrTong70_new.frm(Frm24보고서_NEW) >> frmWard24HNur.cs 폼이름 재정의" />
    public partial class frmWard24HNur : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;       

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

        public frmWard24HNur(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmWard24HNur()
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

            this.dtpDate.LostFocus += new EventHandler(eControl_LostFocus);
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

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if(sender == this.dtpDate)
            {
                if (!VB.IsDate(dtpDate.Text))
                {
                    dtpDate.Text = clsPublic.GstrSysDate;
                    dtpDate.Focus();
                }
            }
        }

        void Set_Init()
        {
            int i = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','ND','DR','IQ','ER')";
            SQL += ComNum.VBLF + "      AND USED = 'Y'";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("*.전체");

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                //cboWard.Items.Add("SICU");
                //cboWard.Items.Add("MICU");
                cboWard.Items.Add("ER");

                cboWard.SelectedIndex = 0;
            }

            dt.Dispose();
            dt = null;


            FarPoint.Win.ComplexBorder cb = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            for (i = 0; i < ssList1_Sheet1.ColumnCount - 1; i++)
            {
                ssList1_Sheet1.ColumnHeader.Cells[0, i].Border = cb;
            }

            for (i = 0; i < ssList2_Sheet1.ColumnCount - 1; i++)
            {
                ssList2_Sheet1.ColumnHeader.Cells[0, i].Border = cb;
            }
        }

        void SCREEN_CLEAR()
        {
            ssList1.ActiveSheet.Cells[0, 1, ssList1.ActiveSheet.Rows.Count - 1, ssList1.ActiveSheet.Columns.Count - 1].Text = "";

            ssList2.ActiveSheet.Cells[0, 4, ssList2.ActiveSheet.Rows.Count - 2, 4].Text = "";

            ssList2.ActiveSheet.Cells[10, 1].Text = "";
            ssList2.ActiveSheet.Cells[10, 4].Text = "";
        }

        void btnCancel_Click()
        {
            dtpDate.Focus();
        }

        void ePrint()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            PrintDocument pd;
            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = clsPrint.gGetDefaultPrinter();

            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
            pd.Print();    //프린트
        }

        private void eBarBARPrint(object sender, PrintPageEventArgs e)
        {
            Rectangle r1 = new Rectangle(10, 170, 900, 800);
            Rectangle r2 = new Rectangle(40, 450, 900, 500);

            e.Graphics.DrawString("간호부 24시간 보고서", new Font("맑은 고딕", 15f), Brushes.Black, 280, 100, new StringFormat());    //헤더 그려주기
            e.Graphics.DrawString("조회일자 : " + dtpDate.Value.Year + "년" + dtpDate.Value.Month + "월" + dtpDate.Value.Day + "일", new Font("맑은 고딕", 10f), Brushes.Black, 10, 140, new StringFormat());    //헤더 그려주기
            e.Graphics.DrawString("과별 재원 입원환자 현황", new Font("맑은 고딕", 15f), Brushes.Black, 260, 400, new StringFormat());

            ssList1.OwnerPrintDraw(e.Graphics, r1, 0, 1);
            ssList2.OwnerPrintDraw(e.Graphics, r2, 0, 1);
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;

            string strNowDate = "";
            int nCol = 0;

            int[] nTotal1 = new int[7];
            int[] nTotal2 = new int[29];
            int[,] nTotal3 = new int[3, 24];
            int[,] nTotal4 = new int[3, 24];
            int nMaxTotal = 0;
            int nBunMan = 0;

            string strIcu = "";
            int nNBcount = 0;
            int nNRNBin = 0;        //정상아가 입원
            int nNRNBOut = 0;       //정상아가 퇴원
            int nPedIN = 0;         //신생아실에서 (정상아 => 환아, 미숙아)
            int nPedOUT = 0;        //신생아실에서 (환아,미숙아 => 정상아)

            int nTotalCnt1 = 0;     //전체 재원
            int nTotalCnt2 = 0;     //전체 입원
            int nTotalCnt3 = 0;     //정상신생아 재원
            int nTotalCnt4 = 0;     //정상신생아 입원
            int nTotalCnt5 = 0;     //산부인과 입원
            int nTotalCnt6 = 0;     //중환아 재원
            int nTotalCnt7 = 0;     //중환아 입원
            int nTotalEr = 0;       //ER 과별 입원현황

            //Clear
            SCREEN_CLEAR();
            for(i = 0; i < nTotal1.Length; i++)
            {
                nTotal1[i] = 0;
            }

            for(i = 0; i < nTotal2.Length; i++)
            {
                nTotal2[i] = 0;
            }

            for(i = 0; i < 24; i++)
            {
                nTotal3[1, i] = 0;
                nTotal3[2, i] = 0;
                nTotal4[1, i] = 0;
                nTotal4[2, i] = 0;
            }

            strNowDate = dtpDate.Text;

            //간호부 24시간 보고서
            //응급실
            //SQL = "";
            //SQL += ComNum.VBLF + "SELECT ";
            //SQL += ComNum.VBLF + "  INWON11+INWON12+INWON13+INWON14+INWON15 INWON1,";
            //SQL += ComNum.VBLF + "  INWON21+INWON22+INWON23+INWON24+INWON25 INWON2,";
            //SQL += ComNum.VBLF + "  INWON31+INWON32+INWON33+INWON34+INWON35 INWON3,";
            //SQL += ComNum.VBLF + "  INWON41+INWON42+INWON43+INWON44+INWON45 INWON4,";
            //SQL += ComNum.VBLF + "  INWON51+INWON52+INWON53+INWON54+INWON55 INWON5, OP ";
            //SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EMINWON";
            //SQL += ComNum.VBLF + "WHERE 1=1";
            //SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";

            //SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //    return;
            //}

            //if (dt.Rows.Count > 0)
            //{
            //    for(i = 0;i < dt.Rows.Count; i++)
            //    {
            //        nTotal1[1] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon1"].ToString().Trim()));
            //        nTotal1[2] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon2"].ToString().Trim()));
            //        nTotal1[3] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon4"].ToString().Trim()));
            //        nTotal1[4] += Convert.ToInt32(VB.Val(dt.Rows[i]["Op"].ToString().Trim()));
            //        nTotal1[5] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon5"].ToString().Trim()));
            //        nTotal1[6] += Convert.ToInt32(VB.Val(dt.Rows[i]["Inwon3"].ToString().Trim()));
            //    }

            //    ssList1.ActiveSheet.Cells[0, 16].Text = nTotal1[1].ToString();
            //    ssList1.ActiveSheet.Cells[1, 16].Text = nTotal1[2].ToString();
            //    ssList1.ActiveSheet.Cells[2, 16].Text = nTotal1[3].ToString();
            //    ssList1.ActiveSheet.Cells[3, 16].Text = nTotal1[4].ToString();
            //    ssList1.ActiveSheet.Cells[4, 16].Text = nTotal1[5].ToString();
            //    ssList1.ActiveSheet.Cells[5, 16].Text = nTotal1[6].ToString();
            //    ssList1.ActiveSheet.Cells[6, 16].Text = (nTotal1[1] + nTotal1[2] + nTotal1[5]).ToString();
            //}

            //dt.Dispose();
            //dt = null;

            for(i = 0; i < nTotal1.Length; i++)
            {
                nTotal1[i] = 0;
            }

            //응급실 제외한 나머지 부서
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WARDCODE, CODE, QTY1+QTY2+QTY3+QTY4 QTY, QTY4";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_INOUT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";

            if(VB.Left(cboWard.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND WARDCODE ='" + cboWard.SelectedItem.ToString().Trim() + "'";
            }

            else
            {
                SQL += ComNum.VBLF + "  AND WARDCODE NOT IN ('GAN')";
            }

            SQL += ComNum.VBLF + "ORDER BY WARDCODE, CODE";

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

            if (dt.Rows.Count > 0)
            {
                nMaxTotal = 0;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper())
                    {
                        case "33":
                            nCol = 2;
                            nTotal2[1] = nTotal2[1] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "35":
                            nCol = 3;
                            nTotal2[2] = nTotal2[2] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;
                       
                        case "40":
                            nCol = 4;
                            nTotal2[3] = nTotal2[3] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "4H":
                            nCol = 5;
                            nTotal2[4] = nTotal2[4] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;
                        case "50":
                            nCol = 6;
                            nTotal2[5] = nTotal2[5] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "53":
                            nCol = 7;
                            nTotal2[6] = nTotal2[6] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "55":
                            nCol = 8;
                            nTotal2[7] = nTotal2[7] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "60":
                            nCol = 9;
                            nTotal2[8] = nTotal2[8] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "63":
                            nCol = 10;
                            nTotal2[9] = nTotal2[9] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "65":
                            nCol = 11;
                            nTotal2[10] = nTotal2[10] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "70":
                            nCol = 12;
                            nTotal2[11] = nTotal2[11] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "73":
                            nCol = 13;
                            nTotal2[12] = nTotal2[12] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "75":
                            nCol = 14;
                            nTotal2[13] = nTotal2[13] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "80":
                            nCol = 15;
                            nTotal2[14] = nTotal2[14] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "83":
                            nCol = 16;
                            nTotal2[15] = nTotal2[15] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "ER":
                            nCol = 17;
                            nTotal2[16] = nTotal2[16] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            break;

                        case "NR":
                            nCol = 18;   //입퇴원에는 중환아 제외
                            if (String.Compare(dt.Rows[i]["CODE"].ToString().Trim(), "14") < 0)
                            {
                                nTotal2[17] = nTotal2[17] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            }
                            break;

                    }


                    switch (dt.Rows[i]["CODE"].ToString().Trim().ToUpper())
                    {
                        case "02":
                            if(nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[0, nCol - 1].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            }

                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nTotal1[1] = nTotal1[1] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            }
                            break;

                        case "03":
                            if (nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[1, nCol - 1].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            }
                            
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nTotal1[2] = nTotal1[2] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            }
                            break;

                        case "04":
                        case "09":
                            if (nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[2, nCol - 1].Text = (VB.Val(ssList1.ActiveSheet.Cells[2, nCol - 1].Text) + VB.Val(dt.Rows[i]["QTY"].ToString().Trim())).ToString();
                            }
                            
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nTotal1[3] = nTotal1[3] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            }
                            break;

                        case "05":
                            if (nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[3, nCol - 1].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            }
                            
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nTotal1[4] = nTotal1[4] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            }
                            break;

                        case "06":
                            if (nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[5, nCol - 1].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            }
                            
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nTotal1[6] = nTotal1[6] + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            }
                            break;

                        case "01":
                            if (nCol != 0)
                            {
                                ssList1.ActiveSheet.Cells[6, nCol - 1].Text = dt.Rows[i]["QTY4"].ToString().Trim();
                            }
                            
                            if (dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() != "ER")
                            {
                                nMaxTotal = nMaxTotal + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY4"].ToString().Trim()));
                            }
                            break;

                        case "08":
                            nBunMan = nBunMan + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                            break;

                        case "10":
                            nPedIN = nPedIN + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));     //Ped_in(신생아실)
                            break;

                        case "11":
                            nPedOUT = nPedOUT + Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));   //NB_out
                            break;
                    }
                    
                }
            }

            dt.Dispose();
            dt = null;

            //전체 재원 입원 환자
            //과별 재원 입원 환자 현황
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  SUM(CNT11+CNT12) CNT1, SUM(CNT51+CNT52) CNT5, SUM(CNT21+CNT22) CNT2";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_JEWON";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND DeptCode NOT IN ('XX')";

            if(VB.Left(cboWard.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND WARDCODE ='" + cboWard.SelectedItem.ToString().Trim() + "'";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nTotalCnt1 = Convert.ToInt32(VB.Val(dt.Rows[0]["Cnt5"].ToString().Trim()));
                nTotalCnt2 = Convert.ToInt32(VB.Val(dt.Rows[0]["Cnt1"].ToString().Trim()));
            }

            dt.Dispose();
            dt = null;

            //정상신생아 in, out
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WardCode, SUM(CNT11+CNT12) CNT1, SUM(CNT51+CNT52) CNT5, SUM(CNT21+CNT22) CNT2 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_JEWON";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND DeptCode ='NB'";

            if(VB.Left(cboWard.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND WARDCODE ='" + cboWard.SelectedItem.ToString().Trim() + "'";
            }
            
            SQL += ComNum.VBLF + "GROUP BY  WardCode";
            SQL += ComNum.VBLF + "ORDER BY  WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    ComFunc.MsgBox("해당 DATA가 없습니다.");
            //    return;
            //}

            if (dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["WARDCODE"].ToString().Trim().ToUpper() == "NR")
                    {
                        nNRNBin = nNRNBin + Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()));
                        nNRNBOut = nNRNBOut + Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()));
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //과별 재원 입원 환자 현황
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DEPTCODE, SUM(CNT11+CNT12) CNT1, SUM(CNT51+CNT52) CNT5, SUM(CNT21+CNT22) CNT2";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_JEWON";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";

            if (VB.Left(cboWard.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND WARDCODE ='" + cboWard.SelectedItem.ToString().Trim() + "'";
            }
            
            SQL += ComNum.VBLF + "GROUP BY  DEPTCODE";
            SQL += ComNum.VBLF + "ORDER BY  DEPTCODE";

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
                    switch (dt.Rows[i]["DeptCode"].ToString().Trim().ToUpper())
                    {
                        case "MD":
                            ssList2.ActiveSheet.Cells[0, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[0, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "MC":
                            ssList2.ActiveSheet.Cells[1, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[1, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "ME":
                            ssList2.ActiveSheet.Cells[2, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[2, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "MG":
                            ssList2.ActiveSheet.Cells[3, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[3, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "MN":
                            ssList2.ActiveSheet.Cells[4, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[4, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "MP":
                            ssList2.ActiveSheet.Cells[5, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[5, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "MR":
                            ssList2.ActiveSheet.Cells[6, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[6, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "GS":
                            ssList2.ActiveSheet.Cells[8, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[8, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "PD":
                            ssList2.ActiveSheet.Cells[9, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[9, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "IQ":
                        case "DB":
                            ssList2.ActiveSheet.Cells[10, 1].Text = (VB.Val(ssList2.ActiveSheet.Cells[10, 1].Text) + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim())).ToString();
                            ssList2.ActiveSheet.Cells[10, 2].Text = (VB.Val(ssList2.ActiveSheet.Cells[10, 2].Text) + VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim())).ToString();
                            break;

                        case "NB":
                            ssList2.ActiveSheet.Cells[11, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            nTotalCnt3 = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()));
                            ssList2.ActiveSheet.Cells[11, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            nTotalCnt4 = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()));
                            nNBcount = nNBcount + Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt2"].ToString().Trim()));
                            break;

                        case "IB":
                            ssList2.ActiveSheet.Cells[12, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            nTotalCnt6 = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()));
                            ssList2.ActiveSheet.Cells[12, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            nTotalCnt7 = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()));
                            break;

                        case "GY":
                            ssList2.ActiveSheet.Cells[13, 1].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[13, 2].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            nTotalCnt5 = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()));
                            break;

                        case "OS":
                            ssList2.ActiveSheet.Cells[0, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[0, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "NS":
                            ssList2.ActiveSheet.Cells[1, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[1, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "NE":
                            ssList2.ActiveSheet.Cells[2, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[2, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "CS":
                            ssList2.ActiveSheet.Cells[3, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[3, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "NP":
                            ssList2.ActiveSheet.Cells[4, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[4, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "EN":
                            ssList2.ActiveSheet.Cells[5, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[5, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "OT":
                            ssList2.ActiveSheet.Cells[6, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[6, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "UR":
                            ssList2.ActiveSheet.Cells[7, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[7, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "DM":
                            ssList2.ActiveSheet.Cells[8, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[8, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "DT":
                            ssList2.ActiveSheet.Cells[9, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[9, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "PC":
                            ssList2.ActiveSheet.Cells[10, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[10, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "RM":
                            ssList2.ActiveSheet.Cells[11, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            ssList2.ActiveSheet.Cells[11, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;

                        case "ER":
                            ssList2.ActiveSheet.Cells[12, 4].Text = VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString();
                            nTotalEr = Convert.ToInt32(VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()));
                            ssList2.ActiveSheet.Cells[12, 5].Text = VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim()).ToString();
                            break;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //중환자실 과별 재원 입원 환자 현황
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  b.Printranking,a.DEPTCODE, SUM(a.CNT11+a.CNT12) CNT1, SUM(a.CNT51+a.CNT52) CNT5, SUM(a.CNT21+a.CNT22) CNT2";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_JEWON a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND a.WardCode IN ('33','35')";
            SQL += ComNum.VBLF + "      AND a.DeptCode = b.DeptCode(+)";

            if(VB.Left(cboWard.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND a.WARDCODE ='" + cboWard.SelectedItem.ToString().Trim() + "'";
            }

            SQL += ComNum.VBLF + "GROUP BY  b.Printranking,a.DEPTCODE";
            SQL += ComNum.VBLF + "ORDER BY  b.Printranking,a.DEPTCODE";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            strIcu = "";

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    if(VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()) > 0)
                    {
                        //switch (dt.Rows[i]["DeptCode"].ToString().Trim().ToUpper())
                        //{
                        //    case "MD":
                        //        strIcu += "MD:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "GS":
                        //        strIcu += "GS:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "PD":
                        //        strIcu += "PD:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "IQ":
                        //        strIcu += "IQ:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "GY":
                        //        strIcu += "GY:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "OS":
                        //        strIcu += "OS:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "NS":
                        //        strIcu += "NS:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "NE":
                        //        strIcu += "NE:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "NP":
                        //        strIcu += "NP:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "EN":
                        //        strIcu += "EN:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "OT":
                        //        strIcu += "OT:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "UR":
                        //        strIcu += "UR:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "DM":
                        //        strIcu += "DM:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "DT":
                        //        strIcu += "DT:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "PC":
                        //        strIcu += "PC:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "RM":
                        //        strIcu += "RM:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "ER":
                        //        strIcu += "ER:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "CS":
                        //        strIcu += "CS:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "MG":
                        //        strIcu += "MG:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "MC":
                        //        strIcu += "MC:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "MP":
                        //        strIcu += "MP:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "MN":
                        //        strIcu += "MN:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;

                        //    case "MI":
                        //        strIcu += "MI:" + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim()).ToString() + " ";
                        //        break;
                        //}

                        strIcu = (VB.Val(strIcu) + VB.Val(dt.Rows[i]["Cnt5"].ToString().Trim())).ToString();
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //과별 SUM
            ssList2.ActiveSheet.Cells[14, 1].Text = nBunMan.ToString();
            ssList2.ActiveSheet.Cells[14, 2].Text = nTotalCnt5.ToString();

            ssList2.ActiveSheet.Cells[13, 4].Text = strIcu;

            ssList2.ActiveSheet.Cells[14, 4].Text = (nTotalCnt1 - nTotalCnt3 - nTotalCnt6).ToString();   //정상아 입원제외, 중환아 제외
            ssList2.ActiveSheet.Cells[14, 5].Text = (nTotalCnt2 - nTotalCnt4 - nTotalCnt7).ToString();

            //24보고서 SUM
            for(i = 1; i <= 6; i++)
            {
                if(i == 1)
                {
                    ssList1.ActiveSheet.Cells[i - 1, 18].Text = (nTotal1[i] - nTotalCnt4 + nPedIN).ToString();        //입원자(정상아 입원 제외, PED_IN 포함)
                }

                else
                {
                    ssList1.ActiveSheet.Cells[i - 1, 18].Text = nTotal1[i].ToString();
                }
            }

            ssList1.ActiveSheet.Cells[0, 17].Text = (VB.Val(ssList1.ActiveSheet.Cells[0, 17].Text) - nNRNBin + nPedIN).ToString();
            ssList1.ActiveSheet.Cells[1, 17].Text = (VB.Val(ssList1.ActiveSheet.Cells[1, 17].Text) - nNBcount + nPedOUT).ToString();

            ssList1.ActiveSheet.Cells[1, 18].Text = (VB.Val(ssList1.ActiveSheet.Cells[1, 18].Text) - nNBcount + nPedOUT).ToString();
            ssList1.ActiveSheet.Cells[6, 18].Text = (nMaxTotal - nTotalCnt3).ToString();    //재원자(정상아 입원 제외)

            //총계 계산 (위의 총계를 덮어씀)
            for (j = 0; j < ssList1_Sheet1.RowCount; j++)
            {
                ssList1_Sheet1.Cells[j, ssList1_Sheet1.ColumnCount - 1].Text = "0";

                for (i = 1; i < ssList1_Sheet1.ColumnCount - 1; i++)
                {
                    ssList1_Sheet1.Cells[j, ssList1_Sheet1.ColumnCount - 1].Text =
                        (VB.Val(ssList1_Sheet1.Cells[j, ssList1_Sheet1.ColumnCount - 1].Text) + VB.Val(ssList1_Sheet1.Cells[j, i].Text)).ToString();
                }
            }


            //0값 없앰
            //for(i = 1; i <= 15; i++)
            //{
            //    for(j = 2; j <= 3; j++)
            //    {
            //        if(VB.Val(ssList2.ActiveSheet.Cells[i - 1, j - 1].Text) == 0)
            //        {
            //            ssList2.ActiveSheet.Cells[i - 1, j - 1].Text = "";
            //        }
            //    }
            //}

            //for (i = 1; i <= 15; i++)
            //{
            //    if (i != 10)
            //    {
            //        for (j = 5; j <= 6; j++)
            //        {
            //            if (VB.Val(ssList2.ActiveSheet.Cells[i - 1, j - 1].Text) == 0)
            //            {
            //                ssList2.ActiveSheet.Cells[i - 1, j - 1].Text = "";
            //            }
            //        }
            //    }                
            //}
        }    
    }
}
