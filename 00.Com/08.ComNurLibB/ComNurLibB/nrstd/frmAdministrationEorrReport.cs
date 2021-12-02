using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using ComLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm투약오류보고서.frm >> frmAdministrationEorrReport.cs 폼이름 재정의" />

    public partial class frmAdministrationEorrReport : Form
    {
        string FstrROWID = "";
        string strDTP = "";

        string FstrPano = "";
        string FstrBDate = "";

        string GstrHelpCode = "";
        string GstrICUWard = "";
        string GstrRowid = "";

        /// <summary>
        /// EMR VIWER
        /// </summary>
        frmEmrViewer frmEmrViewer = null;

        ComFunc CF = null;

        public frmAdministrationEorrReport(string strHelpcode, string strICUWard)
        {
            InitializeComponent();

            GstrHelpCode = strHelpcode;
            GstrICUWard = strICUWard;
        }

        public frmAdministrationEorrReport(string strHelpcode, string strICUWard, string strPano, string strBDate)
        {
            InitializeComponent();

            GstrHelpCode = strHelpcode;
            GstrICUWard = strICUWard;

            FstrPano = strPano;
            FstrBDate = strBDate;
        }

        /// <summary>
        /// 투약오류리스트 관리용
        /// </summary>
        /// <param name="strRowid"></param>
        public frmAdministrationEorrReport(string strRowid)
        {
            InitializeComponent();

            GstrRowid = strRowid;
        }

        public frmAdministrationEorrReport()
        {
            InitializeComponent();
        }

        private void Print()
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;
            int i = 0;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)

                for (i = 5; i <= 9; i++)
                {
                    SS3_Sheet1.Cells[i - 1, 2].Text = "";
                    SS3_Sheet1.Cells[i - 1, 4].Text = "";
                    SS3_Sheet1.Cells[i - 1, 6].Text = "";
                }

            SS3_Sheet1.Cells[4, 2].Text = SS1_Sheet1.Cells[4, 2].Text;
            SS3_Sheet1.Cells[4, 4].Text = SS1_Sheet1.Cells[4, 4].Text;
            SS3_Sheet1.Cells[4, 6].Text = SS1_Sheet1.Cells[4, 6].Text;

            SS3_Sheet1.Cells[6, 2].Text = SS1_Sheet1.Cells[6, 2].Text;
            SS3_Sheet1.Cells[6, 6].Text = SS1_Sheet1.Cells[6, 6].Text;

            SS3_Sheet1.Cells[7, 2].Text = SS1_Sheet1.Cells[7, 2].Text;
            SS3_Sheet1.Cells[7, 4].Text = SS1_Sheet1.Cells[7, 4].Text;
            SS3_Sheet1.Cells[7, 6].Text = SS1_Sheet1.Cells[7, 6].Text;

            SS3_Sheet1.Cells[8, 2].Text = SS1_Sheet1.Cells[8, 2].Text;
            SS3_Sheet1.Cells[8, 4].Text = SS1_Sheet1.Cells[8, 4].Text;
            SS3_Sheet1.Cells[8, 6].Text = SS1_Sheet1.Cells[8, 6].Text;

            SS1_Sheet1.Cells[51, 9].Text = " ";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);

            if (VB.Val(SS2_Sheet1.Cells[10, 2].Text) > 0)
            {
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

                CS.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
                CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;
            }

            if (Save() == true)
            {
                if (ComFunc.MsgBoxQ("자료저장완료! 저장한 자료를 인쇄하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                Print();
            }
        }

        private bool Save()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int[] nJumsu = new int[11];
            int nTemp = 0;
            int nTotal = 0;
            int j = 0;
            string strRemark = "";
            int i = 0;
            double nIpdNo = 0;
            int nAge = 0;
            string strPano = "";
            string strSName = "";
            string strDeptCode = "";
            string strRoomCode = "";
            string strDiag = "";
            string strWardCode = "";
            string strSex = "";
            string strDrCode1 = "";
            string strDrCode2 = "";
            string strJepDate = "";
            string strBDate = "";
            string strBPlace = "";
            string strBName = "";
            string strDrug1 = "";
            string strDrug2 = "";
            string strDrug2_Etc = "";
            string strDrug3 = "";
            string strDrug3_Etc = "";
            string strDrugOp = "";
            string strDrugOp_Etc = "";
            string strDrugCPR = "";
            string strDrugCPR_Etc = "";
            string strDrugBlood = "";
            string strDrugBlood_Etc = "";
            string strDrugAirWay = "";
            string strDrugAirWay_Etc = "";
            string strDrugCare = "";
            string strDrugCare_Etc = "";
            string strDrugTool = "";
            string strDrugTool1_Etc = "";
            string strDrugTool2_Etc = "";
            string strDrug7 = "";
            string strDrug8 = "";
            string strDrug9 = "";
            string strDrug10 = "";
            string strDrug11 = "";
            string strDrug12 = "";
            string strDrug13 = "";
            string strDrug14 = "";
            string strDrug15 = "";
            string strDrug16 = "";
            string strDrug17 = "";
            string strDrug18 = "";
            string strDrug18_Etc = "";
            string strDrug_J = "";
            string strDrug_K = "";
            string strDrug_L = "";
            string strDrug_M = "";
            string strDrug_N = "";
            string strBuseName = "";

            string strErrGubun = "";
            string strErrGrade = "";

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            //'변수clear
            strPano = "";
            strSName = "";
            strDeptCode = "";
            strRoomCode = "";
            strDiag = "";
            nAge = 0;
            strWardCode = "";
            strSex = "";
            strDrCode1 = "";
            strDrCode2 = "";
            strJepDate = "";
            strBDate = "";
            strBPlace = "";
            strBName = "";
            strDrug1 = "";
            strDrug2 = "";
            strDrug2_Etc = "";
            strDrug3 = "";
            strDrug3_Etc = "";
            strDrugOp = "";
            strDrugOp_Etc = "";
            strDrugCPR = "";
            strDrugCPR_Etc = "";
            strDrugBlood = "";
            strDrugBlood_Etc = "";
            strDrugAirWay = "";
            strDrugAirWay_Etc = "";
            strDrugCare = "";
            strDrugCare_Etc = "";
            strDrugTool = "";
            strDrugTool1_Etc = "";
            strDrugTool2_Etc = "";
            strDrug7 = "";
            strDrug8 = "";
            strDrug9 = "";
            strDrug10 = "";
            strDrug11 = "";
            strDrug12 = "";
            strDrug13 = "";
            strDrug14 = "";
            strDrug15 = "";
            strDrug16 = "";
            strDrug17 = "";
            strDrug18 = "";
            strDrug18_Etc = "";
            strDrug_J = "";
            strDrug_K = "";
            strDrug_L = "";
            strDrug_M = "";
            strDrug_N = "";
            strBuseName = "";

            for (i = 0; i <= 10; i++)
            {
                nJumsu[i] = 0;
            }
            strRemark = "";
            nTemp = 0;
            nTotal = 0;

            //'기본정보
            strDeptCode = SS1_Sheet1.Cells[4, 2].Text.Trim();
            strWardCode = SS1_Sheet1.Cells[4, 4].Text.Trim();
            strDiag = SS1_Sheet1.Cells[4, 6].Text.Trim();
            //.Row = 6
            strPano = SS1_Sheet1.Cells[5, 2].Text.Trim();
            strJepDate = SS1_Sheet1.Cells[5, 6].Text.Trim();
            //.Row = 7
            strSName = SS1_Sheet1.Cells[6, 2].Text.Trim();
            strBDate = SS1_Sheet1.Cells[6, 6].Text.Trim();
            //.Row = 8
            strSex = SS1_Sheet1.Cells[7, 2].Text.Trim();
            nAge = (int)VB.Val(SS1_Sheet1.Cells[7, 4].Text.Trim());
            strBPlace = SS1_Sheet1.Cells[7, 6].Text.Trim();
            //.Row = 9
            strDrCode1 = SS1_Sheet1.Cells[8, 2].Text.Trim();
            strDrCode2 = SS1_Sheet1.Cells[8, 4].Text.Trim();
            strBName = SS1_Sheet1.Cells[8, 6].Text.Trim();

            strErrGubun = SS1_Sheet1.Cells[9, 2].Text.Trim();
            strErrGrade = SS1_Sheet1.Cells[9, 4].Text.Trim();

            //.Row = 52
            nIpdNo = VB.Val(SS1_Sheet1.Cells[51, 1].Text.Trim());

            if (ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
            {
                nIpdNo = 0;
            }
            strWardCode = SS1_Sheet1.Cells[51, 2].Text.Trim();
            strRoomCode = SS1_Sheet1.Cells[51, 3].Text.Trim();

            //'내역

            for (i = 15; i <= 17; i++)  //'투약전 발견
            {
                if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 1].Value) == true)
                {
                    strDrug1 = strDrug1 + "1";
                }
                else
                {
                    strDrug1 = strDrug1 + "0";
                }
            }

            for (i = 19; i <= 28; i++)  //'투약후 발견
            {
                if (i != 27)
                {
                    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 1].Value) == true)
                    {
                        strDrug2 = strDrug2 + "1";
                    }
                    else
                    {
                        strDrug2 = strDrug2 + "0";
                    }
                    if (i == 28)
                    {
                        strDrug2_Etc = SS1_Sheet1.Cells[28, 1].Text;
                    }
                }
            }

            for (i = 14; i <= 30; i++)//   '투약후발견오류
            {
                if (i != 16 && i != 18 && i != 29)
                {
                    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 3].Value) == true)
                    {
                        strDrug3 = strDrug3 + "1";
                    }
                    else
                    {
                        strDrug3 = strDrug3 + "0";
                    }
                    if (i == 30)
                    {
                        strDrug3_Etc = SS1_Sheet1.Cells[30, 3].Text;
                    }
                }
            }

            #region 주석
            //for (i = 14; i <= 18; i++)//    '수술관련
            //{
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 5].Value) == true)
            //        strDrugOp = strDrugOp + "1";
            //    else
            //        strDrugOp = strDrugOp + "0";
            //    if (i == 18)
            //        strDrugOp_Etc = SS1_Sheet1.Cells[i - 1, 6].Text.Trim();
            //}

            //for (i = 20; i <= 26; i++)//    'CPR
            //{
            //    //.Row = i
            //    //.Col = 6:
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 5].Value) == true)
            //        strDrugCPR = strDrugCPR + "1";
            //    else
            //        strDrugCPR = strDrugCPR + "0";
            //    if (i == 26)
            //        strDrugCPR_Etc = SS1_Sheet1.Cells[i - 1, 6].Text.Trim();
            //}

            //for (i = 28; i <= 32; i++)//'수혈관련
            //{
            //    //.Row = i
            //    //.Col = 6:
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 5].Value) == true)
            //        strDrugBlood = strDrugBlood + "1";
            //    else
            //        strDrugBlood = strDrugBlood + "0";

            //    if (i == 32)
            //        strDrugBlood_Etc = SS1_Sheet1.Cells[i - 1, 6].Text;
            //}

            //for (i = 34; i <= 39; i++)    //'Airway
            //{
            //    if (i != 36)
            //    {
            //        //.Row = i
            //        //.Col = 6:
            //        if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 5].Value) == true)
            //            strDrugAirWay = strDrugAirWay + "1";
            //        else
            //            strDrugAirWay = strDrugAirWay + "0";

            //        if (i == 39)
            //            strDrugAirWay_Etc = SS1_Sheet1.Cells[i - 1, 6].Text.Trim();
            //    }
            //}


            //for (i = 14; i <= 15; i++)    //'치료/진료관련
            //{
            //    //.Row = i
            //    //.Col = 8:
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 7].Value) == true)
            //        strDrugCare = strDrugCare + "1";
            //    else
            //        strDrugCare = strDrugCare + "0";

            //    if (i == 15)
            //        strDrugCare_Etc = SS1_Sheet1.Cells[i - 1, 8].Text.Trim();
            //}


            //for (i = 17; i <= 23; i++)    //'의료장비/기구
            //{
            //    //.Row = i
            //    //.Col = 8:
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 7].Value) == true)
            //        strDrugTool = strDrugTool + "1";
            //    else
            //        strDrugTool = strDrugTool + "0";

            //    if (i == 20)
            //        strDrugTool1_Etc = SS1_Sheet1.Cells[i - 1, 8].Text.Trim();
            //    if (i == 23)
            //        strDrugTool2_Etc = SS1_Sheet1.Cells[i - 1, 8].Text.Trim();
            //}

            ////  '분만관련
            //if (Convert.ToBoolean(SS1_Sheet1.Cells[24, 7].Value) == true)
            //{
            //    strDrug7 = "1";
            //}
            //else
            //{
            //    strDrug7 = "0";
            //}

            ////  '마취관련
            //if (Convert.ToBoolean(SS1_Sheet1.Cells[25, 7].Value) == true)
            //{
            //    strDrug8 = "1";
            //}
            //else
            //{
            //    strDrug8 = "0";
            //}

            //for (i = 28; i <= 30; i++)//'검사관련
            //{
            //    //.Row = i
            //    //.Col = 8:
            //    if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 7].Value) == true)
            //        strDrug9 = strDrug9 + "1";
            //    else
            //        strDrug9 = strDrug9 + "0";
            //}

            //'화상
            //if (Convert.ToBoolean(SS1_Sheet1.Cells[30, 7].Value) == true)
            //{
            //    strDrug10 = "1";
            //}
            //else
            //{
            //    strDrug10 = "0";
            //}

            //// '감염
            //if (Convert.ToBoolean(SS1_Sheet1.Cells[31, 7].Value) == true)
            //{
            //    strDrug11 = "1";
            //}
            //else
            //{
            //    strDrug11 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[32, 7].Value) == true)
            //{
            //    strDrug12 = "1";
            //}
            //else
            //{
            //    strDrug12 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[33, 7].Value) == true)
            //{
            //    strDrug13 = "1";
            //}
            //else
            //{
            //    strDrug13 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[34, 7].Value) == true)
            //{
            //    strDrug14 = "1";
            //}
            //else
            //{
            //    strDrug14 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[35, 7].Value) == true)
            //{
            //    strDrug15 = "1";
            //}
            //else
            //{
            //    strDrug15 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[36, 7].Value) == true)
            //{
            //    strDrug16 = "1";
            //}
            //else
            //{
            //    strDrug16 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[37, 7].Value) == true)
            //{
            //    strDrug17 = "1";
            //}
            //else
            //{
            //    strDrug17 = "0";
            //}

            //if (Convert.ToBoolean(SS1_Sheet1.Cells[38, 7].Value) == true)
            //{
            //    strDrug18 = "1";
            //}
            //else
            //{
            //    strDrug18 = "0";
            //}

            //strDrug18_Etc = SS1_Sheet1.Cells[38, 8].Text.Trim();

            #endregion

            for (i = 46; i <= 48; i++)
            {
                for (j = 2; j <= 4; j = j + 2)
                {
                    if (i == 48 && j == 4)
                    {

                    }
                    else
                    {
                        if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, j - 1].Value) == true)
                        {
                            strDrug_M = strDrug_M + "1";
                        }
                        else
                        {
                            strDrug_M = strDrug_M + "0";
                        }
                    }
                }
            }

            for (i = 46; i <= 48; i++)
            {
                for (j = 6; j <= 8; j = j + 2)
                {
                    if (i == 48 && j == 8)
                    {

                    }
                    else
                    {
                        if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, j - 1].Value) == true)
                        {
                            strDrug_N = strDrug_N + "1";
                        }
                        else
                        {
                            strDrug_N = strDrug_N + "0";
                        }
                    }
                }
            }

            // '기술내역
            strDrug_J = SS1_Sheet1.Cells[12, 5].Text.Trim();
            strDrug_K = SS1_Sheet1.Cells[20, 5].Text.Trim();
            //strDrug_L = SS1_Sheet1.Cells[28, 5].Text.Trim();
            strDrug_L = SS1_Sheet1.Cells[25, 5].Text.Trim();

            //'보고정보
            //strBuseName = SS1_Sheet1.Cells[49, 6].Text.Trim();

            //시트2
            strRemark = "";

            for (i = 1; i <= 10; i++)
            {
                nTemp = (int)VB.Val(SS2_Sheet1.Cells[i - 1, 1].Text);

                if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 2].Value) == true)
                {
                    nJumsu[i] = 1;
                    nTotal = nTotal + (nTemp * 1);
                    strRemark = strRemark + SS2_Sheet1.Cells[i - 1, 3].Text + "^^";
                }
            }

            if (strPano == "")
            {
                ComFunc.MsgBox("등록번호가 공백입니다", "확인");
                return rtnVal;
            }
            if (strPano == "")
            {
                ComFunc.MsgBox("성명이 공백입니다", "확인");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID == "")        //2019-07-12 fstrrowid 전역 변수로 선언되어 있음. 기본적으로 2개의 spread 클릭할 때 fstrrowid 읽도록 되어 있음!
                {

                    SQL = "";
                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.NUR_STD_DRUG ";
                    SQL = SQL + ComNum.VBLF + " WHERE Ipdno=" + nIpdNo + " ";

                    if (nIpdNo == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + VB.Left(strJepDate, 10) + " 00:00','YYYY-MM-DD HH24:MI')";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + VB.Left(CF.DATE_ADD(clsDB.DbCon, VB.Left(strJepDate, 10), 1), 10) + " 23:59','YYYY-MM-DD HH24:MI')";
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        FstrROWID = "";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_PMPA.NUR_STD_DRUG ( ";
                    SQL = SQL + ComNum.VBLF + " JEPDATE,PANO,IPDNO,SNAME,SEX,AGE,WARDCODE,ROOMCODE,DEPTCODE,ACTDATE, ";
                    SQL = SQL + ComNum.VBLF + " BDATE,BPLACE,BNAME,DIAG,DRCODE1,DRCODE2,DRUG1,DRUG2,DRUG2_ETC,DRUG3,DRUG3_ETC, ";
                    SQL = SQL + ComNum.VBLF + " DRUGOP,DRUGOP_ETC,DRUGCPR,DRUGCPR_ETC,DRUGBLOOD,DRUGBLOOD_ETC,DRUGAIRWAY, ";
                    SQL = SQL + ComNum.VBLF + " DRUGAIRWAY_ETC,DRUGCARE,DRUGCARE_ETC,DRUGTOOL,DRUGTOOL1_ETC,DRUGTOOL2_ETC, ";
                    SQL = SQL + ComNum.VBLF + " DRUG7,DRUG8,DRUG9,DRUG10,DRUG11,DRUG12,DRUG13,DRUG14,DRUG15,DRUG16,DRUG17,DRUG18, ";
                    SQL = SQL + ComNum.VBLF + " DRUG18_ETC , DRUG_J, DRUG_K, DRUG_L, DRUG_M, DRUG_N,BuseName, ";
                    SQL = SQL + ComNum.VBLF + " Jumsu1,Jumsu2,Jumsu3,Jumsu4,Jumsu5,Jumsu6,Jumsu7,Jumsu8,Jumsu9,Jumsu10,Total,Remark, ";
                    SQL = SQL + ComNum.VBLF + "  ENTSABUN, ERRGUBUN, ERRGRADE ) VALUES ( TO_DATE('" + strSysDate + "','YYYY-MM-DD') , '" + strPano + "' , ";
                    SQL = SQL + ComNum.VBLF + " " + nIpdNo + " , '" + strSName + "','" + strSex + "' , " + nAge + ", ";
                    SQL = SQL + ComNum.VBLF + " '" + strWardCode + "' , " + VB.Val(strRoomCode) + ",'" + strDeptCode + "' , ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strJepDate + "','YYYY-MM-DD HH24:MI'),TO_DATE('" + strBDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strBPlace + "', '" + strBName + "', '" + strDiag + "', '" + strDrCode1 + "','" + strDrCode2 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrug1 + "' , '" + strDrug2 + "','" + strDrug2_Etc + "','" + strDrug3 + "','" + strDrug3_Etc + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrugOp + "' , '" + strDrugOp_Etc + "','" + strDrugCPR + "','" + strDrugCPR_Etc + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrugBlood + "' , '" + strDrugBlood_Etc + "','" + strDrugAirWay + "','" + strDrugAirWay_Etc + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrugCare + "' , '" + strDrugCare_Etc + "','" + strDrugTool + "','" + strDrugTool1_Etc + "','" + strDrugTool2_Etc + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrug7 + "' , '" + strDrug8 + "','" + strDrug9 + "','" + strDrug10 + "','" + strDrug11 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrug12 + "' , '" + strDrug13 + "','" + strDrug14 + "','" + strDrug15 + "','" + strDrug16 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrug17 + "' , '" + strDrug18 + "','" + strDrug18_Etc + "','" + strDrug_J + "','" + strDrug_K + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDrug_L + "' , '" + strDrug_M + "','" + strDrug_N + "','" + strBuseName + "', ";
                    SQL = SQL + ComNum.VBLF + " " + nJumsu[1] + "," + nJumsu[2] + "," + nJumsu[3] + "," + nJumsu[4] + "," + nJumsu[5] + ", ";
                    SQL = SQL + ComNum.VBLF + " " + nJumsu[6] + "," + nJumsu[7] + "," + nJumsu[8] + "," + nJumsu[9] + "," + nJumsu[10] + "," + nTotal + ",'" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + "  " + clsType.User.Sabun + ",'" + strErrGubun + "','" + strErrGrade + "'  ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_STD_DRUG_HISTORY ";
                    SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_STD_DRUG ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.NUR_STD_DRUG  SET ";
                    //SQL = SQL + ComNum.VBLF + " JEPDATE = TO_DATE('" + strSysDate + "','YYYY-MM-DD'), "; //보고일은 변경 안되도록 보완
                    //SQL = SQL + ComNum.VBLF + " PANO ='" + strPano + "' , ";
                    //SQL = SQL + ComNum.VBLF + " IPDNO =" + nIpdNo + " , ";
                    SQL = SQL + ComNum.VBLF + " SNAME ='" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + " SEX ='" + strSex + "' , ";
                    SQL = SQL + ComNum.VBLF + " AGE =" + nAge + ", ";
                    SQL = SQL + ComNum.VBLF + " WardCode ='" + strWardCode + "' ,  ";
                    SQL = SQL + ComNum.VBLF + " ROOMCODE = " + VB.Val(strRoomCode) + ", ";
                    SQL = SQL + ComNum.VBLF + " DeptCode ='" + strDeptCode + "' , ";
                    SQL = SQL + ComNum.VBLF + " ACTDATE =TO_DATE('" + strJepDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " BPLACE ='" + strBPlace + "', ";
                    SQL = SQL + ComNum.VBLF + " BNAME ='" + strBName + "', ";
                    SQL = SQL + ComNum.VBLF + " DIAG ='" + strDiag + "', ";
                    SQL = SQL + ComNum.VBLF + " DRCODE1 = '" + strDrCode1 + "', ";
                    SQL = SQL + ComNum.VBLF + " DRCODE2 = '" + strDrCode2 + "', ";
                    SQL = SQL + ComNum.VBLF + " DRUG1 = '" + strDrug1 + "', ";
                    SQL = SQL + ComNum.VBLF + " DRUG2 = '" + strDrug2 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG2_ETC = '" + strDrug2_Etc + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG3 = '" + strDrug3 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG3_ETC = '" + strDrug3_Etc + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUGOP = '" + strDrugOp + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGOP_ETC = '" + strDrugOp_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGCPR = '" + strDrugCPR + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGCPR_ETC ='" + strDrugCPR_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGBLOOD ='" + strDrugBlood + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGBLOOD_ETC ='" + strDrugBlood_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGAIRWAY ='" + strDrugAirWay + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUGAIRWAY_ETC ='" + strDrugAirWay_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGCARE ='" + strDrugCare + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGCARE_ETC ='" + strDrugCare_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGTOOL = '" + strDrugTool + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUGTOOL1_ETC ='" + strDrugTool1_Etc + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUGTOOL2_ETC ='" + strDrugTool2_Etc + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG7 ='" + strDrug7 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG8 ='" + strDrug8 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG9 ='" + strDrug9 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG10 ='" + strDrug10 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG11 ='" + strDrug11 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG12 ='" + strDrug12 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG13 ='" + strDrug13 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG14 ='" + strDrug14 + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG15 ='" + strDrug15 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG16 ='" + strDrug16 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG17 ='" + strDrug17 + "' ,";
                    SQL = SQL + ComNum.VBLF + " DRUG18 ='" + strDrug18 + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG18_ETC ='" + strDrug18_Etc + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG_J ='" + strDrug_J + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG_K ='" + strDrug_K + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG_L ='" + strDrug_L + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG_M ='" + strDrug_M + "' , ";
                    SQL = SQL + ComNum.VBLF + " DRUG_N = '" + strDrug_N + "' ,";
                    SQL = SQL + ComNum.VBLF + " Jumsu1 = " + nJumsu[1] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu2 = " + nJumsu[2] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu3 = " + nJumsu[3] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu4 = " + nJumsu[4] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu5 = " + nJumsu[5] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu6 = " + nJumsu[6] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu7 = " + nJumsu[7] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu8 = " + nJumsu[8] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu9 = " + nJumsu[9] + " , ";
                    SQL = SQL + ComNum.VBLF + " Jumsu10 = " + nJumsu[10] + " , ";
                    SQL = SQL + ComNum.VBLF + " Total = " + nTotal + " , ";
                    SQL = SQL + ComNum.VBLF + " Remark ='" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + " ERRGUBUN ='" + strErrGubun + "', ";
                    SQL = SQL + ComNum.VBLF + " ERRGRADE ='" + strErrGrade + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string strWARD = "";
            string strToDate = "";
            string strNextDate = "";
            string strRemark = "";
            string strOK = "";
            int nRow = 0;
            string strFlag = "";
            string strDept = "";

            SSHis.ActiveSheet.RowCount = 0;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)

                strWARD = (ComboWard.Text).Trim();
            strDept = (ComboDept.Text).Trim();

            strToDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strNextDate = CF.DATE_ADD(clsDB.DbCon, strToDate, 1);
            strWARD = (ComboWard.Text).Trim();
            FstrROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                if (strWARD == "OPD" || strWARD == "ER" || strWARD == "OP")
                {

                    SQL = " SELECT 0 IPDNO, PANO, SNAME, SEX, AGE,";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE, DRCODE, 1 ILSU, BI, '' RELIGION,";
                    SQL = SQL + ComNum.VBLF + " GBSPC, '0' WARDCODE, 0 ROOMCODE, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, TO_CHAR(BDATE,'YYYY-MM-DD') ACTDATE,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(BDATE,'YYYY-MM-DD') OUTDATE, '' JIYUK, 0 TBED, 'O' GUBUN";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + " WHERE BDate >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND BDate <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD')";
                    if (strWARD == "OP")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER ";
                        SQL = SQL + ComNum.VBLF + "                  WHERE OPDate >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "                    AND OPDate <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD'))"; 
                    }
                    else
                    {
                        if (ComboDept.Text != "" && ComboDept.Text != "전체")
                        {
                            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + ComboDept.Text + "' ";
                        }
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY SNAME, BDATE DESC   ";
                }
                else
                {
                    SQL = " SELECT a.IPDNO, a.Pano, a.SName, a.Sex, a.Age, a.DeptCode,a.DrCode,a.ILSU, ";
                    SQL = SQL + ComNum.VBLF + "  a.Bi, a.ReliGion, a.GbSpc, a.WardCode, a.RoomCode, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(A.INDATE,'YYYY-MM-DD')  INDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, a.JiYuk, b.TBed ";
                    SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,Bas_Room b ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.IpwonTime >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + " 00:01','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                    if (ComboDept.Text != "" && ComboDept.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = '" + ComboDept.Text + "' ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND a.Amset4 <> '3' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Pano <> '81000004' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.RoomCode = b.RoomCode(+) ";

                    if (chkWrite.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND A.IPDNO IN (SELECT IPDNO FROM NUR_STD_DRUG ";
                        SQL += ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                    }
                    //else if(ChkWork.Checked == true)
                    //{
                    //    SQL += ComNum.VBLF + "  AND A.IPDNO IN (SELECT IPDNO FROM NUR_STD_DRUG ";
                    //    SQL += ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    //    SQL += ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    //    SQL += ComNum.VBLF + "                     AND WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ")  )";
                    //}
                    else
                    {
                        switch (ComboWard.Text)
                        {
                            case "전체":
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '233' ";
                                break;
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '234' ";
                                break;
                            case "ND":
                            case "NR":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode IN('369','358','368','640','641','642')  ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + " AND (( a.WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ") ) ";
                                SQL = SQL + ComNum.VBLF + " OR  ( a.IPDNO IN (SELECT IPDNO FROM NUR_STD_DRUG ";
                                SQL = SQL + ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND WardCode IN (" + ReadInWard(ComboWard.Text).Trim() + ") ) ) ) ";
                                break;
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "  ORDER BY a.RoomCode,a.SName,a.Indate DESC   ";
                }

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SSList_Sheet1.RowCount = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strOK = "";
                        strFlag = "";

                        if (ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                        {
                            strFlag = "OK";
                        }

                        SQL = " SELECT Remark FROM NUR_MASTER WHERE Ipdno ='" + dt.Rows[i]["Ipdno"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strRemark = dt1.Rows[0]["Remark"].ToString().Trim();

                            if (strRemark != "")
                            {
                                if (VB.I(strRemark, "▶투약,") > 1)
                                {
                                    strFlag = "OK";
                                }
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[i]["PANO"].ToString().Trim() == "81000004")
                        { strOK = "OK"; }

                        if (ChkDaesang.Checked == true || ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                        {
                            if (strFlag == "OK")
                            {
                                nRow = nRow + 1;
                                strOK = "OK";
                            }
                        }
                        else
                        {
                            nRow = nRow + 1;
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            SSList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Indate"].ToString().Trim();
                            SSList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Ipdno"].ToString().Trim();

                            if (ChkDaesang.Checked == true || ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                            {
                                if (strFlag == "OK")
                                {
                                    SSList_Sheet1.Cells[nRow - 1, 6].Text = "OK";
                                }
                            }

                            SQL = "";
                            SQL = " SELECT ROWID ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_STD_DRUG WHERE IPDNO = " + dt.Rows[i]["Ipdno"].ToString().Trim() + " ";
                            //if(ComboWard.Text == "ER" || ComboWard.Text == "OPD")
                            //{
                            //    SQL = SQL + ComNum.VBLF + "  AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            //    SQL = SQL + ComNum.VBLF + "  AND ACTDATE >= TO_DATE('" + dt.Rows[i]["INDATE"].ToString().Trim() + " 00:00','YYYY-MM-DD HH24:MI')  ";
                            //    SQL = SQL + ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + dt.Rows[i]["INDATE"].ToString().Trim() + " 23:00','YYYY-MM-DD HH24:MI')  ";
                            //}

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                SSList_Sheet1.Cells[nRow - 1, 1].BackColor = Color.FromArgb(128, 255, 128);
                                SSList_Sheet1.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    SSList_Sheet1.RowCount = nRow;
                }


                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }


        /// <summary>
        /// 과거 병동 데이터 조회 되도록 프로그램
        /// 쿼리 사용시 IN으로 조회해야함.
        /// </summary>
        /// <param name="argWard"></param>
        /// <returns></returns>
        private string ReadInWard(string argWard)
        {
            string rtnval = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_과거병동조회' ";
                SQL = SQL + ComNum.VBLF + "    AND NAME = '" + argWard + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnval = "'" + argWard + "'";
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnval = rtnval + dt.Rows[i]["CODE"].ToString().Trim() + "','";
                    }
                    rtnval = "'" + rtnval;
                    rtnval = VB.Mid(rtnval, 1, VB.Len(rtnval) - 2);

                }
                dt.Dispose();
                dt = null;

                return rtnval;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnval;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return rtnval;
        }

        private void Screen_Clear()
        {
            int i = 0;
            int j = 0;

            FarPoint.Win.Spread.CellType.ComboBoxCellType cboCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            cboCell.Items = (new String[] { "근접오류", "위해사건", "적신호사건" });

            cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
            cboCell.Editable = false;
            cboCell.MaxDrop = 3;
            SS1_Sheet1.Cells[9, 2].CellType = cboCell;
            SS1_Sheet1.Cells[9, 2].Text = "";        //오류구분
            SS1_Sheet1.Cells[9, 4].Text = "";        //오류등급

            for (i = 5; i <= 9; i++)
            {
                SS1_Sheet1.Cells[i - 1, 2].Text = "";
                SS1_Sheet1.Cells[i - 1, 4].Text = "";
                SS1_Sheet1.Cells[i - 1, 6].Text = "";
            }

            // 'IPDNO
            SS1_Sheet1.Cells[51, 1].Text = "";

            // '텍스트 항목 Clear
            // '기타항목

            SS1_Sheet1.Cells[27, 2].Text = "";
            SS1_Sheet1.Cells[33, 4].Text = "";
            SS1_Sheet1.Cells[17, 6].Text = "";
            SS1_Sheet1.Cells[25, 6].Text = "";
            SS1_Sheet1.Cells[31, 6].Text = "";
            SS1_Sheet1.Cells[38, 6].Text = "";
            SS1_Sheet1.Cells[14, 8].Text = "";
            SS1_Sheet1.Cells[22, 8].Text = "";
            SS1_Sheet1.Cells[38, 8].Text = "";

            //'보고일, 부서, 보고자
            SS1_Sheet1.Cells[49, 1].Text = "보고일 :";
            SS1_Sheet1.Cells[49, 3].Text = "보고자 :";

            //'J,K,L
            SS1_Sheet1.Cells[12, 5].Text = "";
            SS1_Sheet1.Cells[20, 5].Text = "";
            //SS1_Sheet1.Cells[28, 5].Text = "";
            SS1_Sheet1.Cells[25, 5].Text = "";

            ////'체크박스
            //for (i = 15; i <= 17; i++)
            //{
            //    SS1_Sheet1.Cells[i - 1, 1].Value = false;
            //}

            for (i = 19; i <= 28; i++)
            {
                if (i != 27)
                {
                    SS1_Sheet1.Cells[i - 1, 1].Value = false;
                }
            }
            #region
            //for (i = 14; i <= 34; i++)
            //{
            //    if (i != 16 && i != 18 && i != 21 && i != 23 && i != 29 && i != 31 && i != 33)
            //    {
            //        SS1_Sheet1.Cells[i - 1, 3].Value = false;
            //    }
            //}

            //for (i = 14; i <= 39; i++)
            //{
            //    if (i != 19 && i != 27 && i != 33 && i != 36)
            //    {
            //        SS1_Sheet1.Cells[i - 1, 5].Value = false;
            //    }
            //}

            //for (i = 14; i <= 39; i++)
            //{
            //    if (i != 16 && i != 27)
            //    {
            //        SS1_Sheet1.Cells[i - 1, 7].Value = false;

            //        if (i == 24)
            //        {
            //            SS1_Sheet1.Cells[i - 1, 7].Text = "";
            //        }
            //    }
            //}
            #endregion 주석
            //'M,L
            for (i = 46; i <= 48; i++)
            {
                //SS1.Row = i
                for (j = 2; j <= 8; j = j + 2)
                {
                    SS1_Sheet1.Cells[i - 1, j - 1].Text = "";
                }
            }

            //'투약관련 sheet 2
            for (i = 1; i <= 11; i++)
            {
                //   SS2.Row = i
                for (j = 3; j <= 4; j++)
                {
                    SS2_Sheet1.Cells[i - 1, j - 1].Text = "";
                }
            }

         
        }

        private void ComboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboWard.Text == "OPD" || ComboWard.Text == "ER")
            {
                ComboDept.Text = "ER";
            }
            else if (ComboWard.Text == "OP")
            {
                ComboDept.Text = "전체";
            }
            else
            {
                ComboDept.Text = "전체";
            }
        }

        private void frmAdministrationEorrReport_Load(object sender, EventArgs e)
        {
            Screen_Clear();


            SS1_Sheet1.Rows[51].Visible = false;
            SS3_Sheet1.Columns[0].Visible = false;
            SS3_Sheet1.Columns[4].Visible = false;

            CF = new ComFunc();

            strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComboDept_SET();
            ComboWard_SET();
            dtpDate.Value = Convert.ToDateTime(strDTP);
            dtpEDate.Value = Convert.ToDateTime(strDTP);

            SS2_Sheet1.Columns[4].Visible = false;

            if (GstrICUWard == "ER")
            {
                ComboWard.Text = "OPD";
                ComboDept.SelectedIndex = 0;
            }

            if(GstrRowid.Length > 0)
            {
                SS_Display1("", "", GstrRowid);
                return;
            }

            if (clsType.User.JobGroup == "JOB013053")
            {
                chkWrite.Visible = true;
            }

            //2019-05-16
            btnNew.Visible = false;
            if (ComboWard.Text == "OPD" || ComboWard.Text == "OP")
            {
                btnNew.Visible = true;
            }

            //if (GstrHelpCode != "0")
            //{
            //    SS_Display1(GstrHelpCode, "");
            //}
            //else
            //{
            //    ComboWard.Text = "OPD";
            //    SS_Display1(FstrPano, FstrBDate);
            //}
        }

        private void ComboDept_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComboDept.Items.Clear();
            ComboDept.Items.Add("전체");
            ComboDept.Items.Add("ER");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "   WHERE DEPTCODE NOT IN ('ER')";
                SQL = SQL + ComNum.VBLF + "   ORDER BY PRINTRANKING ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                ComboDept.SelectedIndex = -1;

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ComboWard_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string gsWard = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ') ";
                SQL = SQL + ComNum.VBLF + "   AND USED = 'Y'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                ComboWard.Items.Clear();
                ComboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["Wardcode"].ToString().Trim());
                }
                ComboWard.Items.Add("ER");
                ComboWard.Items.Add("OPD");
                ComboWard.Items.Add("OP");

                ComboWard.SelectedIndex = 0;

                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

                if (gsWard != "")
                {
                    for (i = 0; i < ComboWard.Items.Count; i++)
                    {
                        if (ComboWard.Items.IndexOf(gsWard) == i)
                        {
                            ComboWard.SelectedIndex = i;
                            ComboWard.Enabled = false;
                            return;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SS2_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int i = 0;
            int nSum = 0;
            int nJumsu = 0;

            for (i = 1; i <= 10; i++)
            {
                if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 2].Value) == true)
                {
                    nJumsu = Convert.ToInt32(SS2_Sheet1.Cells[i - 1, 1].Text);
                    nSum = nSum + nJumsu;
                }
            }

            SS2_Sheet1.Cells[10, 2].Text = nSum.ToString();
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strBDate = "";

            SSHis.ActiveSheet.RowCount = 0;

            if (SSList_Sheet1.Cells[e.Row, 6].Text == "OK")
            {
                if (ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                {
                    GstrHelpCode = SSList_Sheet1.Cells[e.Row, 0].Text;
                    strBDate = SSList_Sheet1.Cells[e.Row, 4].Text;
                    SS_Display1(GstrHelpCode, strBDate);
                }
                else
                {
                    GstrHelpCode = SSList_Sheet1.Cells[e.Row, 5].Text;
                    SS_Display1(GstrHelpCode);
                }
                SetHistory(SSList_Sheet1.Cells[e.Row, 0].Text.Trim());
            }
            else
            {
                GstrHelpCode = "";
                Screen_Clear();
            }
        }

        private void SS_Display1(string ArgIpdNo, string ArgBDate = "", string strRowid = "")
        {
            string strTemp = "";
            int i = 0;
            int nRow = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate,TO_CHAR(BDate,'YYYY-MM-DD HH24:MI') BDate,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,  ";
                SQL = SQL + ComNum.VBLF + " JEPDATE,PANO,IPDNO,SNAME,SEX,AGE,WARDCODE,ROOMCODE,DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " BPLACE,BNAME,DIAG,DRCODE1,DRCODE2,DRUG1,DRUG2,DRUG2_ETC,DRUG3,DRUG3_ETC,";
                SQL = SQL + ComNum.VBLF + " DRUGOP,DRUGOP_ETC,DRUGCPR,DRUGCPR_ETC,DRUGBLOOD,DRUGBLOOD_ETC,DRUGAIRWAY,";
                SQL = SQL + ComNum.VBLF + " DRUGAIRWAY_ETC,DRUGCARE,DRUGCARE_ETC,DRUGTOOL,DRUGTOOL1_ETC,DRUGTOOL2_ETC,";
                SQL = SQL + ComNum.VBLF + " DRUG7,DRUG8,DRUG9,DRUG10,DRUG11,DRUG12,DRUG13,DRUG14,DRUG15,DRUG16,DRUG17,DRUG18,";
                SQL = SQL + ComNum.VBLF + " DRUG18_ETC , DRUG_J, DRUG_K, DRUG_L, DRUG_M, DRUG_N, BUSENAME,";
                SQL = SQL + ComNum.VBLF + " Jumsu1,Jumsu2,Jumsu3,Jumsu4,Jumsu5,Jumsu6,Jumsu7,Jumsu8,Jumsu9,Jumsu10,Total,Remark, ";
                SQL = SQL + ComNum.VBLF + " ENTSABUN,ROWID, ERRGUBUN, ERRGRADE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_STD_DRUG ";

                if(strRowid.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                }
                else
                {
                    if (ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgIpdNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDNO = 0";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + ArgBDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + Convert.ToDateTime(ArgBDate).AddDays(1).ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                        if (ComboDept.Text == "OP")
                        {

                        }
                        else
                        {
                            if ((ComboDept.Text) != "" && ComboDept.Text != "전체")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + (ComboDept.Text).Trim() + "' ";
                            }
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO = '" + ArgIpdNo + "'";
                    }

                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("투약오류및 기타보고서 기존등록된 자료를 불러옵니다.", "확인");
                    FstrROWID = dt.Rows[0]["RowID"].ToString().Trim();

                    SS1_Sheet1.Cells[4, 2].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[4, 4].Text = dt.Rows[0]["Wardcode"].ToString().Trim();
                    SS1_Sheet1.Cells[4, 6].Text = dt.Rows[0]["DIAG"].ToString().Trim();

                    SS1_Sheet1.Cells[5, 2].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[5, 6].Text = dt.Rows[0]["ACTDate"].ToString().Trim();

                    SS1_Sheet1.Cells[6, 2].Text = dt.Rows[0]["Sname"].ToString().Trim();
                    SS1_Sheet1.Cells[6, 6].Text = dt.Rows[0]["Bdate"].ToString().Trim();

                    SS1_Sheet1.Cells[7, 2].Text = dt.Rows[0]["sex"].ToString().Trim();
                    SS1_Sheet1.Cells[7, 4].Text = dt.Rows[0]["age"].ToString().Trim();
                    SS1_Sheet1.Cells[7, 6].Text = dt.Rows[0]["BPlace"].ToString().Trim();

                    SS1_Sheet1.Cells[8, 2].Text = dt.Rows[0]["DrCode1"].ToString().Trim();
                    SS1_Sheet1.Cells[8, 4].Text = dt.Rows[0]["DrCode2"].ToString().Trim();
                    SS1_Sheet1.Cells[8, 6].Text = dt.Rows[0]["BName"].ToString().Trim();

                    SS1_Sheet1.Cells[9, 2].Text = dt.Rows[0]["ERRGUBUN"].ToString().Trim();
                    SetError(9, 2);
                    SS1_Sheet1.Cells[9, 4].Text = dt.Rows[0]["ERRGRADE"].ToString().Trim();

                    SS1_Sheet1.Cells[51, 1].Text = ArgIpdNo;
                    SS1_Sheet1.Cells[51, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    SS1_Sheet1.Cells[51, 3].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                    

                    //'내역
                    strTemp = dt.Rows[0]["drug1"].ToString().Trim();

                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {

                            SS1_Sheet1.Cells[i + 14, 1].Value = true;
                        }
                    }

                    strTemp = dt.Rows[0]["Drug2"].ToString().Trim();

                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            if (i == 8)
                            {
                                nRow = (i + 19 + 1);
                            }
                            else
                            {
                                nRow = (i + 19);
                            }

                            SS1_Sheet1.Cells[nRow - 1, 1].Value = true;
                        }
                    }

                    SS1_Sheet1.Cells[28, 1].Text = dt.Rows[0]["Drug2_Etc"].ToString().Trim();
                    //'투약후 발견된 오류
                    strTemp = dt.Rows[0]["DRUG3"].ToString().Trim();

                    SS1_Sheet1.Cells[13, 3].Text = VB.Mid(strTemp, 1, 1);
                    SS1_Sheet1.Cells[14, 3].Text = VB.Mid(strTemp, 2, 1);
                    SS1_Sheet1.Cells[16, 3].Text = VB.Mid(strTemp, 3, 1);
                    SS1_Sheet1.Cells[18, 3].Text = VB.Mid(strTemp, 4, 1);
                    SS1_Sheet1.Cells[19, 3].Text = VB.Mid(strTemp, 5, 1);
                    SS1_Sheet1.Cells[20, 3].Text = VB.Mid(strTemp, 6, 1);
                    SS1_Sheet1.Cells[21, 3].Text = VB.Mid(strTemp, 7, 1);
                    SS1_Sheet1.Cells[22, 3].Text = VB.Mid(strTemp, 8, 1);
                    SS1_Sheet1.Cells[23, 3].Text = VB.Mid(strTemp, 9, 1);
                    SS1_Sheet1.Cells[24, 3].Text = VB.Mid(strTemp, 10, 1);
                    SS1_Sheet1.Cells[25, 3].Text = VB.Mid(strTemp, 11, 1);
                    SS1_Sheet1.Cells[26, 3].Text = VB.Mid(strTemp, 12, 1);
                    SS1_Sheet1.Cells[27, 3].Text = VB.Mid(strTemp, 13, 1);

                    SS1_Sheet1.Cells[29, 3].Text = VB.Mid(strTemp, 14, 1);
                    SS1_Sheet1.Cells[30, 3].Text = dt.Rows[0]["Drug3_Etc"].ToString().Trim();

                    strTemp = dt.Rows[0]["DrugOp"].ToString().Trim();

                    //'수술관련
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 14) - 1, 5].Value = true;
                        }
                    }

                    SS1_Sheet1.Cells[17, 6].Text = dt.Rows[0]["DrugOp_Etc"].ToString().Trim();

                    strTemp = dt.Rows[0]["DrugCPR"].ToString().Trim();
                    //'CPR
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 20) - 1, 5].Value = true;
                        }
                    }

                    SS1_Sheet1.Cells[25, 6].Text = dt.Rows[0]["DrugCPR_Etc"].ToString().Trim();

                    strTemp = dt.Rows[0]["DrugBlood"].ToString().Trim();

                    //'수혈관련
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 28) - 1, 5].Value = true;
                        }
                    }

                    SS1_Sheet1.Cells[31, 6].Text = dt.Rows[0]["DrugBlood_Etc"].ToString().Trim();

                    // 'AirWay
                    strTemp = dt.Rows[0]["DrugAirway"].ToString().Trim();

                    SS1_Sheet1.Cells[33, 5].Text = VB.Mid(strTemp, 1, 1);
                    SS1_Sheet1.Cells[34, 5].Text = VB.Mid(strTemp, 2, 1);
                    SS1_Sheet1.Cells[36, 5].Text = VB.Mid(strTemp, 3, 1);
                    SS1_Sheet1.Cells[37, 5].Text = VB.Mid(strTemp, 4, 1);
                    SS1_Sheet1.Cells[38, 5].Text = VB.Mid(strTemp, 5, 1);

                    SS1_Sheet1.Cells[38, 6].Text = dt.Rows[0]["DrugAirway_Etc"].ToString().Trim();

                    strTemp = dt.Rows[0]["DrugCare"].ToString().Trim();

                    //Care
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 14) - 1, 7].Value = true;
                        }
                    }
                    SS1_Sheet1.Cells[14, 8].Text = dt.Rows[0]["DrugCare_Etc"].ToString().Trim();

                    strTemp = dt.Rows[0]["DrugTool"].ToString().Trim();
                    //care  
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, (i + 1), 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 17) - 1, 7].Value = true;
                        }
                    }

                    SS1_Sheet1.Cells[19, 8].Text = dt.Rows[0]["DrugTool1_Etc"].ToString().Trim();
                    SS1_Sheet1.Cells[22, 8].Text = dt.Rows[0]["DrugTool2_Etc"].ToString().Trim();

                    SS1_Sheet1.Cells[24, 7].Text = dt.Rows[0]["Drug7"].ToString().Trim();
                    SS1_Sheet1.Cells[25, 7].Text = dt.Rows[0]["Drug8"].ToString().Trim();

                    strTemp = dt.Rows[0]["Drug9"].ToString().Trim();
                    for (i = 0; i < VB.Len(strTemp); i++)
                    {
                        if (VB.Mid(strTemp, i + 1, 1) == "1")
                        {
                            SS1_Sheet1.Cells[(i + 28) - 1, 7].Value = true;
                        }
                    }

                    //화상
                    SS1_Sheet1.Cells[30, 7].Text = dt.Rows[0]["Drug10"].ToString().Trim();
                    SS1_Sheet1.Cells[31, 7].Text = dt.Rows[0]["Drug11"].ToString().Trim();
                    SS1_Sheet1.Cells[32, 7].Text = dt.Rows[0]["Drug12"].ToString().Trim();
                    SS1_Sheet1.Cells[33, 7].Text = dt.Rows[0]["Drug13"].ToString().Trim();
                    SS1_Sheet1.Cells[34, 7].Text = dt.Rows[0]["Drug14"].ToString().Trim();
                    SS1_Sheet1.Cells[35, 7].Text = dt.Rows[0]["Drug15"].ToString().Trim();
                    SS1_Sheet1.Cells[36, 7].Text = dt.Rows[0]["Drug16"].ToString().Trim();
                    SS1_Sheet1.Cells[37, 7].Text = dt.Rows[0]["Drug17"].ToString().Trim();

                    SS1_Sheet1.Cells[38, 7].Text = dt.Rows[0]["Drug18"].ToString().Trim();
                    SS1_Sheet1.Cells[38, 8].Text = dt.Rows[0]["Drug18_Etc"].ToString().Trim();

                    //'J, K, L
                    SS1_Sheet1.Cells[12, 5].Text = dt.Rows[0]["Drug_J"].ToString().Trim();
                    SS1_Sheet1.Cells[20, 5].Text = dt.Rows[0]["Drug_K"].ToString().Trim();
                    //SS1_Sheet1.Cells[28, 5].Text = dt.Rows[0]["Drug_L"].ToString().Trim();
                    SS1_Sheet1.Cells[25, 5].Text = dt.Rows[0]["Drug_L"].ToString().Trim();

                    //M,N
                    SS1_Sheet1.Cells[45, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 1, 1);
                    SS1_Sheet1.Cells[45, 3].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 2, 1);

                    SS1_Sheet1.Cells[46, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 3, 1);
                    SS1_Sheet1.Cells[46, 3].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 4, 1);

                    SS1_Sheet1.Cells[47, 1].Text = VB.Mid(dt.Rows[0]["Drug_M"].ToString().Trim(), 5, 1);

                    SS1_Sheet1.Cells[45, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 1, 1);
                    SS1_Sheet1.Cells[45, 7].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 2, 1);

                    SS1_Sheet1.Cells[46, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 3, 1);
                    SS1_Sheet1.Cells[46, 7].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 4, 1);

                    SS1_Sheet1.Cells[47, 5].Text = VB.Mid(dt.Rows[0]["Drug_N"].ToString().Trim(), 5, 1);

                    //보고정보
                    SS1_Sheet1.Cells[49, 1].Text = "보고일 : " + dt.Rows[0]["JepDate"].ToString().Trim();
                    //SS1_Sheet1.Cells[49, 6].Text = dt.Rows[0]["BuseName"].ToString().Trim();
                    SS1_Sheet1.Cells[49, 3].Text = "보고자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());

                    //시트2
                    for (i = 1; i <= 10; i++)
                    {
                        SS2_Sheet1.Cells[i - 1, 2].Text = dt.Rows[0]["Jumsu" + i].ToString().Trim();
                        SS2_Sheet1.Cells[i - 1, 3].Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", i);
                    }
                    SS2_Sheet1.Cells[10, 2].Text = VB.Val(dt.Rows[0]["total"].ToString().Trim()).ToString();
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    ComFunc.MsgBox("투약오류에 대한 기존 등록한 자료가 없습니다. 신규 등록입니다", "확인");

                    FstrROWID = "";

                    if (ComboWard.Text == "OPD" || ComboWard.Text == "ER" || ComboWard.Text == "OP")
                    {
                        SQL = "";
                        SQL = " SELECT PANO, SNAME, AGE, SEX, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE, 'YYYY-MM-DD') ACTDATE, '' WARDCODE, '' DIAGNOSIS, 0 IPDNO, ";
                        SQL = SQL + ComNum.VBLF + " DEPTCODE, 0 ROOMCODE, DRCODE";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgIpdNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                        if (ComboWard.Text == "OP")
                        {

                        }
                        else
                        {
                            if (ComboDept.Text != "" && ComboDept.Text != "전체")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + (ComboDept.Text).Trim() + "' ";
                            }
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate,WardCode, ";
                        SQL = SQL + ComNum.VBLF + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode, b.Drcode";
                        SQL = SQL + ComNum.VBLF + " FROM NUR_MASTER a, IPD_NEW_MASTER b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Ipdno=b.Ipdno(+) ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpdNo =" + VB.Val(ArgIpdNo) + " ";
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[4, 2].Text = dt.Rows[0]["deptcode"].ToString().Trim();
                        SS1_Sheet1.Cells[4, 4].Text = dt.Rows[0]["wardcode"].ToString().Trim();
                        SS1_Sheet1.Cells[4, 6].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                        SS1_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();
                        SS1_Sheet1.Cells[5, 6].Text = "";

                        SS1_Sheet1.Cells[6, 2].Text = dt.Rows[0]["sname"].ToString().Trim();
                        SS1_Sheet1.Cells[6, 6].Text = "";

                        SS1_Sheet1.Cells[7, 2].Text = dt.Rows[0]["Sex"].ToString().Trim();
                        SS1_Sheet1.Cells[7, 4].Text = VB.Val(dt.Rows[0]["Age"].ToString().Replace(",", "")).ToString();
                        SS1_Sheet1.Cells[7, 6].Text = "";

                        SS1_Sheet1.Cells[8, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["Drcode"].ToString().Trim());
                        SS1_Sheet1.Cells[8, 4].Text = "";
                        SS1_Sheet1.Cells[8, 6].Text = "";

                        //확인일자
                        SS1_Sheet1.Cells[5, 6].Text = strDTP + " 00:00";

                        //보고일
                        SS1_Sheet1.Cells[49, 1].Text = "보고일 : " + strDTP;
                        SS1_Sheet1.Cells[49, 3].Text = "보고자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                        SS1_Sheet1.Cells[51, 1].Text = ArgIpdNo;
                        SS1_Sheet1.Cells[51, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                        SS1_Sheet1.Cells[51, 3].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                    }

                }
                dt.Dispose();
                dt = null;

                

                GstrHelpCode = "";
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if(SS1_Sheet1.Cells[5, 2].Text.Trim().Length > 0)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name.Equals("frmEmrViewer"))
                    {
                        if (frmEmrViewer == null)
                        {
                            frm.Dispose();
                            break;
                        }
                        else
                        {
                            frmEmrViewer.SetNewPatient(SS1_Sheet1.Cells[5, 2].Text.Trim());
                            return;
                        }
                    }
                }

                frmEmrViewer = new frmEmrViewer(SS1_Sheet1.Cells[5, 2].Text.Trim());
                frmEmrViewer.StartPosition = FormStartPosition.CenterParent;
                frmEmrViewer.Show(this);

                //clsVbEmr.EXECUTE_TextEmrViewEx(SS1_Sheet1.Cells[5, 2].Text.Trim(), clsType.User.Sabun);
                return;
            }

            if (SSList_Sheet1.RowCount == 0)
                return;

            //clsVbEmr.EXECUTE_TextEmrViewEx(SSList_Sheet1.Cells[SSList_Sheet1.ActiveRowIndex, 0].Text.Trim(), clsType.User.Sabun);

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name.Equals("frmEmrViewer"))
                {
                    if (frmEmrViewer == null)
                    {
                        frm.Dispose();
                        break;
                    }
                    else
                    {
                        frmEmrViewer.SetNewPatient(SSList_Sheet1.Cells[SSList_Sheet1.ActiveRowIndex, 0].Text.Trim());
                        return;
                    }
                }
            }

            frmEmrViewer = new frmEmrViewer(SSList_Sheet1.Cells[SSList_Sheet1.ActiveRowIndex, 0].Text.Trim());
            frmEmrViewer.StartPosition = FormStartPosition.CenterParent;
            frmEmrViewer.Show(this);
            return;
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if(e.Row == 26 && e.Column == 1)
            {
                SS1_Sheet1.Cells[25, 1].Value = !(bool)SS1_Sheet1.Cells[25,1].Value;
            }

            if(e.Row == 9 && e.Column == 2)
            {
                SetError(e.Row, e.Column);
            }

            if(e.Column == 3)
            {
                if(e.Row == 15)
                {
                    SS1_Sheet1.Cells[14, 3].Value = !(bool)SS1_Sheet1.Cells[14, 3].Value;
                }
                else if(e.Row == 17)
                {
                    SS1_Sheet1.Cells[16, 3].Value = !(bool)SS1_Sheet1.Cells[16, 3].Value;
                }
            }

            if (e.Column == 6)
            {
                frmCalendar1Time frmCalendar1TimeX = new frmCalendar1Time();
                clsPublic.GstrDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                clsPublic.GstrTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "M");

                if (e.Row == 5)
                {
                    frmCalendar1TimeX.ShowDialog();

                    SS1_Sheet1.Cells[e.Row, e.Column].Text = clsPublic.GstrDate + " " + clsPublic.GstrTime;
                }
                else if (e.Row == 6)
                {
                    frmCalendar1TimeX.ShowDialog();

                    SS1_Sheet1.Cells[e.Row, e.Column].Text = clsPublic.GstrDate + " " + clsPublic.GstrTime;
                }

                if (frmCalendar1TimeX != null)
                {
                    frmCalendar1TimeX.Dispose();
                    frmCalendar1TimeX = null;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            string strPano = "";

            DataTable dt = new DataTable();

            strPano = VB.InputBox("입원환자의 등록번호를 입력하시기 바랍니다.", "입원환자 외래 낙상");

            strPano = VB.Right("00000000" + strPano, 8);

            SQL = "";
            SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate, DECODE(b.RoomCode,233,'SICU',234,'MICU',WardCode) WardCode, ";
            SQL = SQL + ComNum.VBLF + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode, b.DRCODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_MASTER a,  " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
            SQL = SQL + ComNum.VBLF + " WHERE a.Ipdno=b.Ipdno(+) ";
            SQL = SQL + ComNum.VBLF + "  AND a.PANO ='" + strPano + "' ";
            SQL = SQL + ComNum.VBLF + "  AND b.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                SS1_Sheet1.Cells[4, 2].Text = dt.Rows[0]["deptcode"].ToString().Trim();
                SS1_Sheet1.Cells[4, 4].Text = dt.Rows[0]["wardcode"].ToString().Trim();
                SS1_Sheet1.Cells[4, 6].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                SS1_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();
                SS1_Sheet1.Cells[5, 6].Text = "";

                SS1_Sheet1.Cells[6, 2].Text = dt.Rows[0]["sname"].ToString().Trim();
                SS1_Sheet1.Cells[6, 6].Text = "";

                SS1_Sheet1.Cells[7, 2].Text = dt.Rows[0]["Sex"].ToString().Trim();
                SS1_Sheet1.Cells[7, 4].Text = VB.Val(dt.Rows[0]["Age"].ToString().Replace(",", "")).ToString();
                SS1_Sheet1.Cells[7, 6].Text = "";

                SS1_Sheet1.Cells[8, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["Drcode"].ToString().Trim());
                SS1_Sheet1.Cells[8, 4].Text = "";
                SS1_Sheet1.Cells[8, 6].Text = "";

                //확인일자
                SS1_Sheet1.Cells[5, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + " 00:00";

                //보고일
                SS1_Sheet1.Cells[49, 1].Text = "보고일 : " +  ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                SS1_Sheet1.Cells[49, 3].Text = "보고자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                SS1_Sheet1.Cells[51, 1].Text = "0";
                SS1_Sheet1.Cells[51, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                SS1_Sheet1.Cells[51, 3].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void SetHistory(string argPANO)
        {
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            DataTable dt = new DataTable();

            SQL = " SELECT PANO, SNAME, BPLACE, BNAME, ROWID, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_STD_DRUG ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + argPANO + "' ";
            SQL += ComNum.VBLF + " ORDER BY JEPDATE ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                SSHis.ActiveSheet.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SSHis.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SSHis.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SSHis.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["JEPDATE"].ToString().Trim();
                    SSHis.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["BPLACE"].ToString().Trim();
                    SSHis.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            DialogResult result = MessageBox.Show("선택한 보고서를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
               
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_STD_DRUG_HISTORY ";
                SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.NUR_STD_DRUG ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = ComNum.VBLF + " DELETE KOSMOS_PMPA.NUR_STD_DRUG ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                btnSearch.PerformClick();

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void SSHis_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 )
            {
                return;
            }

            string strRowid = SSHis.ActiveSheet.Cells[e.Row, 4].Text.Trim();

            if (strRowid != "")
            {
                SS_Display1("", "", strRowid);
            }

        }

        void SetError(int nRow, int nCol)
        {

            if (nRow == 9 && nCol == 2)
            {
                string strErrGubun = "";

                FarPoint.Win.Spread.CellType.ComboBoxCellType cboCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                strErrGubun = SS1_Sheet1.Cells[9, 2].Text.Trim();

                if (strErrGubun == "근접오류")
                {
                    cboCell.Items = (new String[] { "등급1. 오류가 발생할 위험이 있는 상황", "등급2. 오류가 발생하였으나 환자에게 도달하지 않음" });
                }
                else if (strErrGubun == "위해사건")
                {
                    cboCell.Items = (new String[] { "등급3. 환자에게 투여/적용되었으나 해가 없음", "등급4. 환자에게 투여/적용되었으며 추가적인 관찰이 필요함",
                                                    "등급5. 일시적 손상으로 중재가 필요함", "등급6. 일시적 손상으로 입원기간이 연장됨",
                                                    "등급7. 생명을 유지하기 위해 필수적인 중재가 필요함" });
                }
                else if (strErrGubun == "적신호사건")
                {
                    cboCell.Items = (new String[] { "등급8. 영구적 손상", "등급9. 환자사망" });
                }

                if (strErrGubun == "근접오류" || strErrGubun == "위해사건" || strErrGubun == "적신호사건")
                {
                    cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    cboCell.Editable = false;
                    cboCell.MaxDrop = 3;
                    SS1_Sheet1.Cells[9, 4].Text = "";
                    SS1_Sheet1.Cells[9, 4].CellType = cboCell;
                }


            }
        }

        private void SS1_Change(object sender, ChangeEventArgs e)
        {
            SetError(e.Row, e.Column);
        }

        private void frmAdministrationEorrReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewer != null)
            {
                frmEmrViewer.Dispose();
                frmEmrViewer = null;
            }
        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
