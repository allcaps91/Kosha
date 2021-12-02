using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스using System;

namespace ComPmpaLibB
{
    public partial class frmPmpaBonRate : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread cSP = new clsSpread();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsIpdAcct cIAct = new clsIpdAcct();

        public frmPmpaBonRate()
        {
            InitializeComponent();
            SetEvent();
        }
        public frmPmpaBonRate(string ArgPano)
        {
            InitializeComponent();

            SetEvent();


            txtPano.Text = ArgPano;
        }
        private void SetEvent()
        {
            //this.dtpSDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            //this.dtpDelDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.cboBi.MouseWheel += new MouseEventHandler(eCboWheel);
            this.cboMCode.MouseWheel += new MouseEventHandler(eCboWheel);
            this.cboVCode.MouseWheel += new MouseEventHandler(eCboWheel);
            this.cboIO.MouseWheel += new MouseEventHandler(eCboWheel);
        }

        private void eCboWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable Dt = null;
            int i = 0;

            if (e.KeyChar == 13)
            {
                if (txtPano.Text.Trim() == "")
                {
                    return;
                }

                txtPano.Text = VB.Format(VB.Val(txtPano.Text), "0#######");

                Dt = cPF.Get_BasPatient(clsDB.DbCon, txtPano.Text);
                if (Dt == null || Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록된 환자가 없습니다.", "등록번호 확인요망!");
                    return;
                }

                lblSName.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtAge.Text = ComFunc.AgeCalcEx(Dt.Rows[0]["JUMIN1"].ToString() + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString()), clsPublic.GstrSysDate).ToString();
                txtJumin.Text = Dt.Rows[0]["JUMIN1"].ToString() + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString());

                for (i = 0; i < cboBi.Items.Count; i++)
                {
                    if (VB.Left(cboBi.Items[i].ToString(), 2) == Dt.Rows[0]["Bi"].ToString().Trim())
                    {
                        cboBi.SelectedIndex = i;
                        break;
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
        }

        private void SS1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            string strCode = string.Empty;
            
            if (e.Row < 0 || e.Column != 0)
            {
                return;
            }

            try
            {
                strCode = SS1_Sheet1.Cells[e.Row, 0].Text.ToUpper().Trim();
                SS1_Sheet1.Cells[e.Row, 0].Text = strCode;

                //수가 Read  Suga_Read
                SQL = "";
                SQL += ComNum.VBLF + " SELECT t.Sunext, SunameK,SuGbAA,t.Bun,t.Nu,SugbA,SugbB,SugbC,SugbD,SugbE,SugbF,SugbG,SUGBL, ";
                SQL += ComNum.VBLF + "        SugbH,SugbI,SugbJ,SugbK,SugbM,SugbO,SugbS,SugbQ,SugbR,SugbW,SugbX,Iamt,Tamt,Bamt,SugbY,SugbZ, ";
                SQL += ComNum.VBLF + "        TO_CHAR(Sudate, 'yyyy-mm-dd') SuDate,OldIamt,OldTamt,OldBamt, ";
                SQL += ComNum.VBLF + "        TO_CHAR(Sudate3, 'yyyy-mm-dd') SuDate3,Iamt3,Tamt3,Bamt3, ";
                SQL += ComNum.VBLF + "        TO_CHAR(Sudate4, 'yyyy-mm-dd') SuDate4,Iamt4,Tamt4,Bamt4, ";
                SQL += ComNum.VBLF + "        TO_CHAR(Sudate5, 'yyyy-mm-dd') SuDate5,Iamt5,Tamt5,Bamt5, ";
                SQL += ComNum.VBLF + "        DayMax, TotMax, Hcode,n.DtlBun,n.GbTax, ";
                SQL += ComNum.VBLF + "        TO_CHAR(DelDate, 'yyyy-mm-dd') DelDate";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT t, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN n ";
                SQL += ComNum.VBLF + "  WHERE t.Sucode = '" + strCode + "' ";
                SQL += ComNum.VBLF + "    AND t.Sunext = n.Sunext ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    cSP.Spread_Clear_Range(SS1, e.Row, 0, 1, SS1_Sheet1.ColumnCount);
                    SS1.Focus();
                    return;
                }
                else
                {
                    if (Dt.Rows[0]["DelDate"].ToString().Trim() != "" && string.Compare(Dt.Rows[0]["DelDate"].ToString(), clsPublic.GstrSysDate) <= 0)
                    {
                        ComFunc.MsgBox(strCode + " 삭제된 코드입니다", "확인");
                        Dt.Dispose();
                        Dt = null;
                        cSP.Spread_Clear_Range(SS1, e.Row, 0, 1, SS1_Sheet1.ColumnCount);
                        SS1.Focus();
                        return;
                    }

                    SS1_Sheet1.Cells[e.Row, 4].Text = Dt.Rows[0]["SunameK"].ToString().Trim();          //수가명
                    SS1_Sheet1.Cells[e.Row, 1].Text = Dt.Rows[0]["BUN"].ToString().Trim();              //분류코드
                    SS1_Sheet1.Cells[e.Row, 2].Text = Dt.Rows[0]["DTLBUN"].ToString().Trim();           //상세분류

                    switch (Dt.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                            SS1_Sheet1.Cells[e.Row, 3].Text = "진찰료";
                            break;
                        case "72":
                        case "73":
                            SS1_Sheet1.Cells[e.Row, 3].Text = "CT/MRI";
                            break;
                        case "74":
                            SS1_Sheet1.Cells[e.Row, 3].Text = "식대비";
                            break;
                        default:
                            SS1_Sheet1.Cells[e.Row, 3].Text = "진료비";
                            break;
                    }

                    if (Dt.Rows[0]["DTLBUN"].ToString().Trim() == "4004")
                    {
                        SS1_Sheet1.Cells[e.Row, 3].Text = "틀니";
                    }
                    else if (Dt.Rows[0]["DTLBUN"].ToString().Trim() == "4003")
                    {
                        SS1_Sheet1.Cells[e.Row, 3].Text = "임플란트";
                    }

                    //급여 비급여
                    clsSpread.gSpreadComboFind(SS1, e.Row, 5, 2, "급여");
                    
                    if (string.Compare(Dt.Rows[0]["Sudate"].ToString(), clsPublic.GstrSysDate) <= 0)
                    {
                        SS1_Sheet1.Cells[e.Row, 6].Text = VB.Val(Dt.Rows[0]["Bamt"].ToString().Trim()).ToString("###,###,###");
                    }
                    else if (string.Compare(Dt.Rows[0]["Sudate3"].ToString(), clsPublic.GstrSysDate) <= 0)
                    {
                        SS1_Sheet1.Cells[e.Row, 6].Text = VB.Val(Dt.Rows[0]["OldBamt"].ToString().Trim()).ToString("###,###,###");
                    }
                    else if (string.Compare(Dt.Rows[0]["Sudate4"].ToString(), clsPublic.GstrSysDate) <= 0)
                    {
                        SS1_Sheet1.Cells[e.Row, 6].Text = VB.Val(Dt.Rows[0]["Bamt3"].ToString().Trim()).ToString("###,###,###");
                    }
                    else if (string.Compare(Dt.Rows[0]["Sudate5"].ToString(), clsPublic.GstrSysDate) <= 0)
                    {   
                        SS1_Sheet1.Cells[e.Row, 6].Text = VB.Val(Dt.Rows[0]["Bamt4"].ToString().Trim()).ToString("###,###,###");
                    }
                    else
                    {
                        SS1_Sheet1.Cells[e.Row, 6].Text = VB.Val(Dt.Rows[0]["Bamt5"].ToString().Trim()).ToString("###,###,###");
                    }
                    //전액본인부담
                    clsSpread.gSpreadComboFind(SS1, e.Row, 7, 1, "");
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmPmpaBonRate_Load(object sender, EventArgs e)
        {
            Set_Combo();
            Screen_Clear();
        }

        private void Set_Combo()
        {
            cboIO.Items.Clear();
            cboIO.Items.Add("O.외래");
            cboIO.Items.Add("I.입원");

            CF.Combo_BCode_SET(clsDB.DbCon, cboBi, "BAS_환자종류", true, 1, "");

            cboMCode.Items.Clear();
            cboMCode.Items.Add("");
            cboMCode.Items.Add("C000.차상위계층환자(건강보험)");
            cboMCode.Items.Add("E000.차상위계층2종 만성질환,18세미만(건강보험)");
            cboMCode.Items.Add("F000.차상위계층2종 장애인 만성질환,18세미만(건강보험)");
            cboMCode.Items.Add("H000.희귀.난치성질환자H(건강보험)");
            cboMCode.Items.Add("V000.산정특례 희귀난치성질환자V(건강보험)");

            CF.Combo_BCode_SET(clsDB.DbCon, cboVCode, "BAS_중증암환자", true, 1, "");
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "2", 2);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void Screen_Clear()
        {
            txtPano.Text = "";
            lblSName.Text = "";
            cboBi.SelectedIndex = 0;
            txtAge.Text = "";
            cboIO.SelectedIndex = 0;
            cboMCode.SelectedIndex = 0;
            cboVCode.SelectedIndex = 0;

            cSP.Spread_Clear(SS1, 10, 8);
            cSP.Spread_Clear_Range(SS_Gesan, 0, 1, SS_Gesan_Sheet1.RowCount, SS_Gesan_Sheet1.ColumnCount);

            txtJumin.Text = "";
            txtTAmt.Text = "0";
            txtJAmt.Text = "0";
            txtBAmt.Text = "0";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();
            clsOumsad cOUM = new clsOumsad();
            int i = 0;
            
            long nJin = 0,      nCTMRI = 0,     nFood = 0,      nBohum = 0,      nDT1 = 0,      nDT2 = 0;           //총금액
            long nBAmt_Jin = 0, nBAmt_CT = 0,   nBAmt_Food = 0, nBAmt_Bohum = 0, nBAmt_Dt1 = 0, nBAmt_Dt2 = 0;      //본인부담금
            long nJAmt_Jin = 0, nJAmt_CT = 0,   nJAmt_Food = 0, nJAmt_Bohum = 0, nJAmt_Dt1 = 0, nJAmt_Dt2 = 0;      //공단부담금

            long nJinB = 0, nCTMRIB = 0, nFoodB = 0, nBohumB = 0, nDT1B = 0, nDT2B = 0;           //비급여 구분

            long nJin_100 = 0,      nCTMRI_100 = 0,   nFood_100 = 0,      nBohum_100 = 0,      nDT1_100 = 0,      nDT2_100 = 0;           //전액본인부담구분(총액)
            long nBAmt_Jin_100 = 0, nBAmt_CT_100 = 0, nBAmt_Food_100 = 0, nBAmt_Bohum_100 = 0, nBAmt_Dt1_100 = 0, nBAmt_Dt2_100 = 0;      //전액본인부담구분(본인부담)
            long nJAmt_Jin_100 = 0, nJAmt_CT_100 = 0, nJAmt_Food_100 = 0, nJAmt_Bohum_100 = 0, nJAmt_Dt1_100 = 0, nJAmt_Dt2_100 = 0;      //전액본인부담구분(공단부담)
            
            long nTotAmt = 0, nTotJAmt = 0, nTotBAmt = 0;
            long nAmt = 0,    nFAmt = 0;
            bool bFlag_FAmt = false;

            string strJuminNo = txtJumin.Text;
            
            cBON.BI = VB.Left(cboBi.Text, 2).Trim();
            cBON.IO = VB.Left(cboIO.Text, 1).Trim();
            cBON.CHILD = "0";                                  //기본 성인
            cBON.MCODE = VB.Left(cboMCode.Text, 4).Trim();
            cBON.VCODE = VB.Left(cboVCode.Text, 4).Trim();
            cBON.DEPT = VB.Left(cboDept.Text, 2).Trim();
            
            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            if (cboBi.Text.Trim() == "")
            {
                return;
            }

            try
            {
                //기준일자 세팅
                cBON.SDATE = clsPublic.GstrSysDate;
                //나이구분 체크
                cBON.CHILD = cPF.Acct_Age_Gubun(Convert.ToInt32(txtAge.Text), strJuminNo, clsPublic.GstrSysDate, cBON.IO);
                cBON.OGPDBUN = "";
                //수납구분
             
                //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
                cBON.FCODE = "";

                if (cBON.IO == "I")
                {
                    //기본 부담율 계산
                    if (cIAct.Read_IBon_Rate(clsDB.DbCon, cBON) == false)
                    {
                        clsAlert cA = new ComPmpaLibB.clsAlert();
                        cA.Alert_BonRate(cBON);
                        return;
                    }
                    
                }
                else
                {
                    //기본 부담율 계산
                    if (cOUM.Read_OBon_Rate(clsDB.DbCon, cBON) == false)
                    {
                        clsAlert cA = new ComPmpaLibB.clsAlert();
                        cA.Alert_BonRate(cBON);
                        return;
                    }
                }
                
                
                //본인부담율  DIsplay(우측) - 스프레드 숨김
                SS_Gesan_Sheet1.Cells[0, 1].Text = clsPmpaType.IBR.Jin.ToString();
                SS_Gesan_Sheet1.Cells[1, 1].Text = clsPmpaType.IBR.Bohum.ToString();
                SS_Gesan_Sheet1.Cells[2, 1].Text = clsPmpaType.IBR.CTMRI.ToString();
                SS_Gesan_Sheet1.Cells[3, 1].Text = clsPmpaType.IBR.Food.ToString();
                SS_Gesan_Sheet1.Cells[4, 1].Text = clsPmpaType.IBR.Dt1.ToString();
                SS_Gesan_Sheet1.Cells[5, 1].Text = clsPmpaType.IBR.Dt2.ToString();

                //본인부담율  DIsplay(하단부)
                SS_Rate_Sheet1.Cells[0, 1].Text = clsPmpaType.IBR.Jin.ToString();
                SS_Rate_Sheet1.Cells[0, 2].Text = clsPmpaType.IBR.Bohum.ToString();
                SS_Rate_Sheet1.Cells[0, 3].Text = clsPmpaType.IBR.CTMRI.ToString();
                SS_Rate_Sheet1.Cells[0, 4].Text = clsPmpaType.IBR.Food.ToString();
                SS_Rate_Sheet1.Cells[0, 5].Text = clsPmpaType.IBR.Dt1.ToString();
                SS_Rate_Sheet1.Cells[0, 6].Text = clsPmpaType.IBR.Dt2.ToString();
                SS_Rate_Sheet1.Cells[0, 7].Text = clsPmpaType.IBR.FAmt1.ToString();
                SS_Rate_Sheet1.Cells[0, 8].Text = clsPmpaType.IBR.FAmt2.ToString();

                #region 진료비 합산 (항목별 집계)
                for (i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Trim() != "")
                    {
                        nAmt = Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, 6].Text, ",", ""));
                        
                        if (SS1_Sheet1.Cells[i, 2].Text.Trim() == "4004")
                        {
                            if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                            {
                                nDT1_100 += nAmt;                          
                                nBAmt_Dt1_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                nJAmt_Dt1_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0)); 
                            }
                            else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0") //비급여
                            {
                                nDT1B += nAmt;
                            }
                            else
                            {
                                nDT1 += nAmt;
                                nBAmt_Dt1 += clsPmpaType.IBR.FAmt1;
                                nJAmt_Dt1 += nAmt - clsPmpaType.IBR.FAmt1;
                            }
                        }
                        else if (SS1_Sheet1.Cells[i, 2].Text.Trim() == "4003")
                        {
                            if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                            {
                                nDT2_100 += nAmt;
                                nBAmt_Dt2_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                nJAmt_Dt2_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                            }
                            else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0")
                            {
                                nDT2B += nAmt;
                            }
                            else
                            {
                                nDT2 += nAmt;
                                nBAmt_Dt2 += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Dt2 / 100.0));
                                nJAmt_Dt2 += nAmt - (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Dt2 / 100.0));
                            }
                        }
                        else
                        {

                            nFAmt = clsPmpaType.IBR.FAmt1;  //진료비 정액
                            
                            //진찰료
                            if (SS1_Sheet1.Cells[i, 1].Text.Trim() == "01" || SS1_Sheet1.Cells[i, 1].Text.Trim() == "02")
                            {
                                if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                                {
                                    nJin_100 += nAmt;
                                    nBAmt_Jin_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                    nJAmt_Jin_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                }
                                else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0")
                                {
                                    nJinB += nAmt;
                                }
                                else
                                {
                                    nJin += nAmt;
                                    if (nFAmt > 0)  //진료비정액
                                    {
                                        if (bFlag_FAmt == false)
                                        {
                                            nBAmt_Jin += nFAmt;
                                            nJAmt_Jin += nAmt - nFAmt;
                                            bFlag_FAmt = true;      //정액적용 여부
                                        }
                                        else
                                        {
                                            nBAmt_Jin += 0;
                                            nJAmt_Jin += nAmt;
                                        }
                                    }
                                    else
                                    {
                                        nBAmt_Jin += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Jin / 100.0));
                                        nJAmt_Jin += nAmt - (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Jin / 100.0));
                                    }
                                }
                            }
                            //CT,MRI
                            else if (SS1_Sheet1.Cells[i, 1].Text.Trim() == "72" || SS1_Sheet1.Cells[i, 1].Text.Trim() == "73")
                            {   
                                if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                                {
                                    nCTMRI_100 += nAmt;
                                    nBAmt_CT_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                    nJAmt_CT_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                }
                                else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0")
                                {
                                    nCTMRIB += nAmt;
                                }
                                else
                                {
                                    nCTMRI += nAmt;
                                    nBAmt_CT += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.CTMRI / 100.0));
                                    nJAmt_CT += nAmt - (long)Math.Truncate(nAmt * (clsPmpaType.IBR.CTMRI / 100.0));
                                }
                            }
                            //식대료
                            else if (SS1_Sheet1.Cells[i, 1].Text.Trim() == "74")
                            {
                                if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                                {
                                    nFood_100 += nAmt;
                                    nBAmt_Food_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                    nJAmt_Food_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                }
                                else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0")
                                {
                                    nFoodB += nAmt;
                                }
                                else
                                {
                                    nFood += nAmt;
                                    nBAmt_Food += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Food / 100.0));
                                    nJAmt_Food += nAmt - (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Food / 100.0));
                                }
                            }
                            //진료비
                            else
                            {
                                if (VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1) != "")   //전액본인부담 계산
                                {
                                    nBohum_100 += nAmt;
                                    nBAmt_Bohum_100 += (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                    nJAmt_Bohum_100 += nAmt - (long)Math.Truncate((nAmt * Bon_Rate_100(VB.Left(SS1_Sheet1.Cells[i, 7].Text.Trim(), 1)) / 100.0));
                                }
                                else if (VB.Left(SS1_Sheet1.Cells[i, 5].Text.Trim(), 1) != "0")
                                {
                                    nBohumB += nAmt;
                                }
                                else
                                {
                                    nBohum += nAmt;
                                    if (nFAmt > 0)  //진료비정액
                                    {
                                        if (bFlag_FAmt == false)
                                        {
                                            nBAmt_Bohum += nFAmt;
                                            nJAmt_Bohum += nAmt - nFAmt;
                                            bFlag_FAmt = true;      //정액적용 여부
                                        }
                                        else
                                        {
                                            nBAmt_Bohum += 0;
                                            nJAmt_Bohum += nAmt;
                                        }
                                    }
                                    else
                                    {
                                        nBAmt_Bohum += (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Bohum / 100.0));
                                        nJAmt_Bohum += nAmt - (long)Math.Truncate(nAmt * (clsPmpaType.IBR.Bohum / 100.0));
                                    }
                                }  
                            }
                        }
                    } //End if
                } //End For
                #endregion

                //총금액  DIsplay (합산)
                SS_Gesan_Sheet1.Cells[0, 2].Text = (nJin + nJin_100 + nJinB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[1, 2].Text = (nBohum + nBohum_100 + nBohumB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[2, 2].Text = (nCTMRI + nCTMRI_100 + nCTMRIB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[3, 2].Text = (nFood + nFood_100 + nFoodB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[4, 2].Text = (nDT1 + nDT1_100 + nDT1B).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[5, 2].Text = (nDT2 + nDT2_100 + nDT2B).ToString("###,###,##0");

                nTotAmt += nJin + nJin_100 + nJinB;
                nTotAmt += nBohum + nBohum_100 + nBohumB;
                nTotAmt += nCTMRI + nCTMRI_100 + nCTMRIB;
                nTotAmt += nFood + nFood_100 + nFoodB;
                nTotAmt += nDT1 + nDT1_100 + nDT1B;
                nTotAmt += nDT2 + nDT2_100 + nDT2B;

                //본인부담금  Display (합산)
                SS_Gesan_Sheet1.Cells[0, 4].Text = (nBAmt_Jin + nBAmt_Jin_100 + nJinB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[1, 4].Text = (nBAmt_Bohum + nBAmt_Bohum_100 + nBohumB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[2, 4].Text = (nBAmt_CT + nBAmt_CT_100 + nCTMRIB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[3, 4].Text = (nBAmt_Food + nBAmt_Food_100 + nFoodB).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[4, 4].Text = (nBAmt_Dt1 + nBAmt_Dt1_100 + nDT1B).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[5, 4].Text = (nBAmt_Dt2 + nBAmt_Dt2_100 + nDT2B).ToString("###,###,##0");

                nTotBAmt += nBAmt_Jin + nBAmt_Jin_100 + nJinB;
                nTotBAmt += nBAmt_CT + nBAmt_CT_100 + nCTMRIB;
                nTotBAmt += nBAmt_Food + nBAmt_Food_100 + nFoodB;
                nTotBAmt += nBAmt_Bohum + nBAmt_Bohum_100 + nBohumB;
                nTotBAmt += nBAmt_Dt1 + nBAmt_Dt1_100 + nDT1B;
                nTotBAmt += nBAmt_Dt2 + nBAmt_Dt2_100 + nDT2B;
                
                //조합부담금  Display (합산)
                SS_Gesan_Sheet1.Cells[0, 3].Text = (nJAmt_Jin + nJAmt_Jin_100).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[1, 3].Text = (nJAmt_Bohum + nJAmt_Bohum_100).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[2, 3].Text = (nJAmt_CT + nJAmt_CT_100).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[3, 3].Text = (nJAmt_Food + nJAmt_Food_100).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[4, 3].Text = (nJAmt_Dt1 + nJAmt_Dt1_100).ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[5, 3].Text = (nJAmt_Dt2 + nJAmt_Dt2_100).ToString("###,###,##0");

                nTotJAmt += nJAmt_Jin + nJAmt_Jin_100;
                nTotJAmt += nJAmt_CT + nJAmt_CT_100;
                nTotJAmt += nJAmt_Food + nJAmt_Food_100;
                nTotJAmt += nJAmt_Bohum + nJAmt_Bohum_100;
                nTotJAmt += nJAmt_Dt1 + nJAmt_Dt1_100;
                nTotJAmt += nJAmt_Dt2 + nJAmt_Dt2_100;
                
                //총합계 계산 & Display
                SS_Gesan_Sheet1.Cells[6, 2].Text = nTotAmt.ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[6, 3].Text = nTotJAmt.ToString("###,###,##0");
                SS_Gesan_Sheet1.Cells[6, 4].Text = nTotBAmt.ToString("###,###,##0");

                nTotBAmt = nTotBAmt / 100 * 100;    //100단위 미만 절사
                nTotJAmt = nTotAmt - nTotBAmt;

                txtTAmt.Text = nTotAmt.ToString("###,###,##0");
                txtJAmt.Text = nTotJAmt.ToString("###,###,##0");
                txtBAmt.Text = nTotBAmt.ToString("###,###,##0");
                

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        private int Bon_Rate_100(string strGbn)
        {
            int rtnRate = 0;

            switch (strGbn)
            {
                case "1": rtnRate = 100; break;
                case "4": rtnRate = 80;  break;
                case "5": rtnRate = 50;  break;
                case "6": rtnRate = 80;  break;
                case "7": rtnRate = 50;  break;
                default:
                    rtnRate = 0;
                    break;
            }
            return rtnRate;
        }        
    }
}
