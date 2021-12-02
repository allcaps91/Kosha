using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Drawing;

namespace ComLibB
{
    /// <summary> 수가찾기 설정 </summary>
    public partial class frmSearchSugaSQL : Form
    {
        string GstrHelpCode = ""; //global

        //이벤트를 전달할 경우
        public delegate void SetSuGaCodeSQL(string argSQL);
        public event SetSuGaCodeSQL rSetSuGaCodeSQL;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        frmWonhangHelp frmWonhangHelpX = null;

        public frmSearchSugaSQL()
        {
            InitializeComponent();
        }

        void frmSearchSugaSQL_Load(object sender, EventArgs e)
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 13;

            ssView_Sheet1.Cells[0, 0].Text = "00";
            ssView_Sheet1.Cells[1, 0].Text = "A1";
            ssView_Sheet1.Cells[2, 0].Text = "A2";
            ssView_Sheet1.Cells[3, 0].Text = "A3";
            ssView_Sheet1.Cells[4, 0].Text = "A4";
            ssView_Sheet1.Cells[5, 0].Text = "A5";
            ssView_Sheet1.Cells[6, 0].Text = "A6";
            ssView_Sheet1.Cells[7, 0].Text = "A7";
            ssView_Sheet1.Cells[8, 0].Text = "A8";
            ssView_Sheet1.Cells[9, 0].Text = "A9";
            ssView_Sheet1.Cells[10, 0].Text = "AA";
            ssView_Sheet1.Cells[11, 0].Text = "AB";
            ssView_Sheet1.Cells[12, 0].Text = "AC";

            ssView_Sheet1.Cells[0, 1].Text = "전체수가코드";
            ssView_Sheet1.Cells[1, 1].Text = "진찰,입원료";
            ssView_Sheet1.Cells[2, 1].Text = "투 약 료";
            ssView_Sheet1.Cells[3, 1].Text = "주 사 료";
            ssView_Sheet1.Cells[4, 1].Text = "마 취 료";
            ssView_Sheet1.Cells[5, 1].Text = "물리치료";
            ssView_Sheet1.Cells[6, 1].Text = "신경정신";
            ssView_Sheet1.Cells[7, 1].Text = "처치및수술";
            ssView_Sheet1.Cells[8, 1].Text = "기능검사";
            ssView_Sheet1.Cells[9, 1].Text = "일반검사";
            ssView_Sheet1.Cells[10, 1].Text = "방 사 선";
            ssView_Sheet1.Cells[11, 1].Text = "SONO,CT,MRI";
            ssView_Sheet1.Cells[12, 1].Text = "식대등";

            cboSelect.Items.Clear();
            cboSelect.Items.Add("*.적용안함");
            cboSelect.Items.Add("1.진찰");
            cboSelect.Items.Add("2.의학관리");
            cboSelect.Items.Add("3.검사");
            cboSelect.Items.Add("4.영상진단 및 방사선치료");
            cboSelect.Items.Add("5.마취");
            cboSelect.Items.Add("6.정신요법");
            cboSelect.Items.Add("7.처치수술");

            txtDaiCode.Text = "";
            txtDaiCode1.Text = "";
           
            txtWon.Text = "";
            txtWon1.Text = "";
            txtNurCode.Text = "";
            txtNurCode1.Text = "";
            txtAntiClass1.Text = "";

            txtDRGCode.Text = "";
            txtDrgName.Text = "";

            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboDtlBun, "BAS_수가상세분류", 1, true, "");

            try
            {
                //분류코드에서 분류코드 Select
                SQL = "";
                SQL = "SELECT Code,Name ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE Jong = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND Code <= '84'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                ssView_Sheet1.RowCount = dt.Rows.Count + 14;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i + 13, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 13, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                ssView_Sheet1.Cells[i + 13, 0].Text = "92";
                ssView_Sheet1.Cells[i + 13, 1].Text = "감액";

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string strBun = "";

            string SQL = "";

            //ListBox에서 선택한 분류코드를 찾음
            strBun = "";

            strBun = CellClick(ssView_Sheet1.ActiveRowIndex);


            SQL = "";
            SQL = "     SELECT Nu,Bun,SuCode,Gbn,SuNext,SugbSS,SugbBi,SuQty,SugbA,SugbB,SugbC,SugbD,SugbE,";
            SQL = SQL + ComNum.VBLF + "      SugbF,SugbG,SugbH,SugbI,SugbJ,SugbK,SugbL,SugbM,SugbN,SugbO,";
            SQL = SQL + ComNum.VBLF + "      SUGBP, SUGBQ, SUGBR, SUGBS, SUGBT, SUGBU, ";
            SQL = SQL + ComNum.VBLF + "      SugbN, SugbV, SugbW, DayMax,TotMax,IAmt,TAmt,BAmt,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate,'YYYY-MM-DD')  SuDate,OldIAmt,OldTAmt,OldBAmt,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate3,'YYYY-MM-DD') SuDate3,IAmt3,TAmt3,BAmt3,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate4,'YYYY-MM-DD') SuDate4,IAmt4,TAmt4,BAmt4,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate5,'YYYY-MM-DD') SuDate5,IAmt5,TAmt5,BAmt5,";
            SQL = SQL + ComNum.VBLF + "      SuNameK,SuNameE,SunameG,Unit,DaiCode,HCode,BCode,";
            SQL = SQL + ComNum.VBLF + "      SuHam,EdiJong,TO_CHAR(EdiDate,'YYYY-MM-DD') EdiDate,";
            SQL = SQL + ComNum.VBLF + "      OldBCode,OldGesu,OldJong,WonCode,WonAmt,NurCode, NROWID ";
            SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";

            //'분류코드를 SET
            if (strBun == "ALL")
            {
                SQL = SQL + ComNum.VBLF + " WHERE  Bun >= '01' AND Bun <= '84' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE  Bun IN (" + VB.Left(strBun, strBun.Length - 1) + ") ";
            }

            //'행위,재료대
            if (rdoGbE1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbE =  '0' ";
            if (rdoGbE2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbE =  '1' ";
            //'급여,비급여
            if (rdoGbF1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbF =  '0' ";
            if (rdoGbF2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbF <> '0' ";
            //'특진,비특진
            if (rdoGbH1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbH =  '1' ";
            if (rdoGbH2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbH <> '1' ";
            //'소아가산
            if (rdoGbB1.Checked == true) SQL = SQL + ComNum.VBLF + " AND  SugbB IN ('A','B','C','D','E','F','G', 'H','H','J','Z') ";
            if (rdoGbB2.Checked == true) SQL = SQL + ComNum.VBLF + " AND  SugbB NOT IN ('A','B','C','D','E','F','G', 'H','H','J','Z') ";
            //'심야가산
            if (rdoGbC1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbC ='1'  ";
            if (rdoGbC2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbC <>'1'  ";



            //'외부의뢰
            if (rdoGbJ1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbJ <> '9' ";
            if (rdoGbJ2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbJ =  '9' ";
            //'감액구분
            if (rdoGbK1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbK =  '0' ";
            if (rdoGbK2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbK =  '1' ";
            if (rdoGbK3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbK =  '2' ";
            //'표준코드 종류별
            if (rdoGbL1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbL =  '1' ";
            if (rdoGbL2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbL =  '2' ";
            if (rdoGbL3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbL =  '3' ";
            if (rdoGbL4.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbL =  '7' ";
            if (rdoGbL5.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbL =  '8' ";
            //'퇴장방지의약품
            if (rdoGbM1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbM =  '1' ";
            //'선수납수가
            if (rdoGbN1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbN =  '1' ";
            //'의약분업(원내조제)
            if (rdoGbO1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbO >  '0' ";
            if (rdoGbO2.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbO <  '1' ";

            //'비급여부분
            if (rdoGbP1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbP =  '1' ";
            if (rdoGbP2.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbP =  '2' ";
            if (rdoGbP3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbP =  '9' "; //'제외
            //'산재급역(0적용안함, 1급여)
            if (rdoGbQ1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbQ =  '0' ";
            if (rdoGbQ2.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbQ =  '1' ";
            //'자보비급여(0.급여, 1.비급여)
            if (rdoGbR1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbR =  '0' ";
            if (rdoGbR2.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbR =  '1' ";
            //'100/100
            if (rdoGbS1.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbS =  '0' ";
            if (rdoGbS2.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbS =  '1' ";
            //'if( OptGbS(3).Checked == true ) SQL = SQL + ComNum.VBLF + " AND SugbS =  '4' "
            if (rdoGbS3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbS IN ('4','6')  "; //'2016-09-20
            if (rdoGbS4.Checked == true)
                SQL = SQL + ComNum.VBLF + " AND SugbS =  '3' ";

            //'DRG비급여 ( 1.비급여)
            if (rdoGbT1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbT =  '0' ";
            if (rdoGbT2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbT =  '1' ";

            //'고가약제( 1.고가)
            if (rdoGbU1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbU =  '1' "; //' 고가
            if (rdoGbU3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbU =  '0' "; //' 표준가
            if (rdoGbU3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbU =  '2' "; //' 저가


            //'고가약제( 1.고가)
            if (rdoGbV1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbV =  '1' "; //' 약제
            if (rdoGbV2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbV =  '2' "; //' 재료


            //'산재 자보 행위,재료대
            if (rdoGbX1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbX =  '0' ";
            if (rdoGbX2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbX =  '1' ";

            //'외과가산
            if (rdoGbY1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbY =  '0' ";
            if (rdoGbY2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbY =  '1' "; //'20%
            if (rdoGbY3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbY =  '2' "; //'30%

            //'흉부외과가산
            if (rdoGbZ1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbZ =  '0' ";
            if (rdoGbZ2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbZ =  '1' "; //'100%
            if (rdoGbZ3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbZ =  '2' "; //'70%
            if (rdoGbZ4.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbZ =  '3' "; //'40%
            if (rdoGbZ5.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbZ =  '4' "; //'20%

            //'응급가산
            //'if( OptGbAA(1).Checked == true ) SQL = SQL + ComNum.VBLF + " AND SugbAA =  '0' "
            if (rdoGbAA1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAA =  '1' "; //'응급가산 50%
            if (rdoGbAA2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAA =  '2' "; //'중증응급가산 50%
            if (rdoGbAA3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAA =  '3' "; //'중증응급가산 50%

            //'판독가산
            if (rdoGbAB1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAB =  '0' "; //' '0 %
            if (rdoGbAB2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAB =  '1' "; //' '10 %

            //'마취가산
            if (rdoGbAC1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAD =  '1' "; //' '
            if (rdoGbAC2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAD =  '2' "; //' '
            if (rdoGbAC3.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAD =  '3' "; //' '

            //'화상가산
            if (rdoGbAD1.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAD =  '0' "; //' '0 %
            if (rdoGbAD2.Checked == true) SQL = SQL + ComNum.VBLF + " AND SugbAD =  '1' "; //' '10 %

            if (VB.Trim(txtDaiCode.Text) != "")
            {
                SQL = SQL + ComNum.VBLF + " AND DAICODE ='" + txtDaiCode.Text + "'";
            }

            if (VB.Trim(txtNurCode.Text) != "")
            {
                SQL = SQL + ComNum.VBLF + " AND NURCODE = '" + txtNurCode.Text + "' ";
            }

            if (VB.Trim(txtAntiClass.Text) != "")
            {
                SQL = SQL + ComNum.VBLF + " AND ANTICLASS = '" + txtAntiClass.Text + "' ";
            }

            if (VB.Trim(txtWon.Text) == "1121")
            {
                SQL = SQL + ComNum.VBLF + " AND WONCODE IN ('1121','1122','1615')";
            }
            else
            {
                if (VB.Trim(txtWon.Text) != "") SQL = SQL + ComNum.VBLF + " AND WONCODE ='" + txtWon.Text + "'";
            }
            //'코드 삭제여부
            if (rdoGbDel1.Checked == true) SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";
            if (rdoGbDel2.Checked == true) SQL = SQL + ComNum.VBLF + "  AND DelDate IS NOT NULL ";

            if (rdoDate1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND SUDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " AND SUDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
            }

            if (rdoDelDate1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND DELDATE = TO_DATE('" + dtpDelDate.Text + "','YYYY-MM-DD')";
            }

            //'단가 0로는 표시 않함
            if (rdoZeor1.Checked == true) //'Zero 제외
            {
                SQL = SQL + ComNum.VBLF + "   AND BAMT <> 0 ";
            }
            if (rdoZeor2.Checked == true) //'Zero 만
            {
                SQL = SQL + ComNum.VBLF + "   AND BAMT = 0 ";
            }

            if (rdoCM1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND SUGBW = '0'";
            }
            else if (rdoCM2.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND SUGBW = '1'";
            }
            else if (rdoCM3.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND SUGBW = '2'";
            }

            if (VB.Left(cboSelect.Text, 1) != "")
            {
                SQL = SQL + ComNum.VBLF + " AND GBSELECT  = '" + VB.Left(cboSelect.Text, 1) + "' ";
            }

            if (VB.Left(cboDtlBun.Text, 4) != "")
            {
                SQL = SQL + ComNum.VBLF + " AND DTLBUN  = '" + VB.Left(cboDtlBun.Text, 4) + "' ";
            }
            
            if (chkOpt0.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBGOJI = 'Y'"     ; //'고지혈정
            if (chkOpt1.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBGANJANG = 'Y'"  ; //'간장용제
            if (chkOpt2.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBSUGBF = 'Y'"    ; //'F항관리
            if (chkOpt3.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBANTI = 'Y'"     ; //'항생제
            if (chkOpt4.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBRARE = 'Y'"     ; //'희기난치
            if (chkOpt5.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBBONE = 'Y'"     ; //'골다공
            if (chkOpt6.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBANTICAN = 'Y'"  ; //'항암제
            if (chkOpt7.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBPPI = 'Y'"      ; //'PPI제제
            if (chkOpt8.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBDEMENTIA = 'Y'" ; //'치매약제
            if (chkOpt9.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBDia = 'Y'"      ; //'당뇨약제
            if (chkOpt10.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBDrug = 'Y'"    ; //'저가약제관리
            if (chkOpt11.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBOCSF = 'Y'"    ; //'OCS 급여가능
            if (chkOpt12.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBWONF = 'Y'"    ; //'급여전환 원무과 메세지
            if (chkOpt13.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBGABA = 'Y'"    ; //'GABAPENTIN 약제
            if (chkOpt14.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBDRUGNO = 'Y'"  ; //'저가약제제외
            if (chkOpt15.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBNS = 'Y'"      ; //'신경차단술(소아 노인가산 않함)
            if (chkOpt16.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBOCSDRUG = 'Y'" ; //'향정신의약품 OCS 사유
            if (chkOpt17.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBOpRoom = 'Y'"  ; //'수술예방적대상(항생제)
            if (chkOpt18.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBTAX = 'Y'"     ; //'부가세대상
            if (chkOpt19.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBTB = 'Y' "     ; //'항결핵(지원금)
            if (chkOpt20.Checked == true) SQL = SQL + ComNum.VBLF + " AND GBMT004 = 'Y' "; ; //004


            if (chkDRG100.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRG100 ='Y' ";
            if (chkDRGF.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRGF ='Y' ";
            if (chkDRGadd.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRGADD ='Y' ";

            if (chkdrgogadd.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRGOgADD ='Y' ";

            if (chkDRGOpen.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRGOPEN ='Y' ";
            if (chkdrgogadd.Checked == true) SQL = SQL + ComNum.VBLF + "   AND DRGogadd ='Y' ";
            if (txtDRGCode.Text != "") SQL = SQL + ComNum.VBLF + "   AND DRGCODE = '" + txtDRGCode.Text + "' ";
                                
            //'SORT순서(누적,분류,수가코드,품명코드순)          
            SQL = SQL + ComNum.VBLF + "ORDER BY SuCode,Gbn DESC,SuNext ";

            rSetSuGaCodeSQL(SQL);
            rEventClosed();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string CellClick(int intRow)
        {
            string strVal = "";
            string strBun = "";
            int i = 0;

            if (intRow == 0)
            {
                strBun = "ALL";
            }
            else
            {
                for (i = 0; i < ssView_Sheet1.RowCount;i++)
                {
                    if (ssView_Sheet1.IsSelected(i, 0) == true)
                    {
                        switch (ssView_Sheet1.Cells[i, 0].Text)
                        {
                            case "A1":
                                strBun += "'01','02','03','04','05','06','07','08','09','10',";
                                break;
                            case "A2":
                                strBun += "'11','12','13','14','15',";
                                break;
                            case "A3":
                                strBun += "'16','17','18','19','20','21',";
                                break;
                            case "A4":
                                strBun += "'22','23',";
                                break;
                            case "A5":
                                strBun += "'24','25',";
                                break;
                            case "A6":
                                strBun += "'26','27',";
                                break;
                            case "A7":
                                strBun += "'28','29','30','31','32','33','34','35','36','37','38','39','40',";
                                break;
                            case "A8":
                                strBun += "'41','42','43','44','45','46','47','48','49','50','51',";
                                break;
                            case "A9":
                                strBun += "'52','53','54','55','56','57','58','59','60','61','62','63','64',";
                                break;
                            case "AA":
                                strBun += "'65','66','67','68','69','70',";
                                break;
                            case "AB":
                                strBun += "'71','72','73',";
                                break;
                            case "AC":
                                strBun += "'74','75','76','77','78','79','80','81','82','83','84',";
                                break;
                            default:
                                strBun += "'" + ssView_Sheet1.Cells[i, 0].Text + "',";
                                break;
                        }
                    }
                }
            }

            if (strBun == "")
            {
                ComFunc.MsgBox("분류코드를 1건도 선택 안함", "오류");
                ssView.Focus();
                return strVal;
            }

            strVal = strBun;
            return strVal;
        }

        void rdoDate_Click(object sender, EventArgs e)
        {
            if (rdoDate0.Checked == true)
            {
                dtpFDate.Visible = false;
                dtpTDate.Visible = false;
            }
            else
            {
                dtpFDate.Visible = true;
                dtpTDate.Visible = true;
            }
        }

        void rdoDelDate_Click(object sender, EventArgs e)
        {
            if (rdoDelDate0.Checked == true)
            {
                dtpDelDate.Visible = true;
            }
            else if (rdoDelDate1.Checked == false)
            {
                dtpDelDate.Visible = false;
            }
        }

        void txtAntiClass_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            frmAntiHelp frm = new frmAntiHelp();
            frm.rSetHelpCode += frm_rSetHelpCode;
            frm.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtAntiClass.Text = GstrHelpCode;
                txtAntiClass1.Text = READ_AntiClassName(txtAntiClass.Text);
                GstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        private void frm_rSetHelpCode(string strHelpCode)
        {
            this.GstrHelpCode = strHelpCode;
        }


        void txtDaiCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            frmYAKHelp frmYAKHelpX = new frmYAKHelp();
            frmYAKHelpX.StartPosition = FormStartPosition.CenterParent;
            frmYAKHelpX.rSetHelpCode += frmYAKHelpX_rSetHelpCode;
            frmYAKHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtDaiCode.Text = GstrHelpCode;
                txtDaiCode1.Text = READ_DaicodeName(txtDaiCode.Text);
                GstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        private void frmYAKHelpX_rSetHelpCode(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
        }

        //void txtDaiCode_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        txtDaiCode1.Text = READ_DaicodeName(txtDaiCode.Text);
        //    }
        //}

        void txtDRGCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            frmDrgBaseCode frmDrgBaseCodeX = new frmDrgBaseCode();
            frmDrgBaseCodeX.StartPosition = FormStartPosition.CenterParent;
            frmDrgBaseCodeX.rSetHelpCode += frmDrgBaseCodeX_rSetHelpCode;
            frmDrgBaseCodeX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtDRGCode.Text = GstrHelpCode;
                txtDrgName.Text = READ_DRGNAME(txtDRGCode.Text);
                GstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        private void frmDrgBaseCodeX_rSetHelpCode(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
        }

        void txtDRGCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDrgName.Text = READ_DRGNAME(txtDRGCode.Text);
            }
        }

        void txtNurCode_DoubleClick(object sender, EventArgs e)
        {
            GstrHelpCode = "";

            frmNurCodeHelp frmNurCodeHelpX = new frmNurCodeHelp();
            frmNurCodeHelpX.StartPosition = FormStartPosition.CenterParent;
            frmNurCodeHelpX.rSetHelpCode += frmNurCodeHelpX_rSetHelpCode;
            frmNurCodeHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtNurCode.Text = VB.Left(GstrHelpCode, 4).Trim();
                txtNurCode1.Text = VB.Mid(GstrHelpCode, 0, 5);
                GstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        private void frmNurCodeHelpX_rSetHelpCode(string strHelpCode)
        {
            GstrHelpCode = strHelpCode;
        }

        void txtWon_DoubleClick(object sender, EventArgs e)
        {
            frmWonhangHelpX = new frmWonhangHelp();
            frmWonhangHelpX.StartPosition = FormStartPosition.CenterParent;
            frmWonhangHelpX.rSetCodeName += new frmWonhangHelp.SetCodeName(frmWonhangHelpX_rSetCodeName);
            frmWonhangHelpX.rEventClosed += new frmWonhangHelp.EventClosed(frmWonhangHelpX_rEventExit);
            frmWonhangHelpX.ShowDialog();

            if (GstrHelpCode != "")
            {
                txtWon.Text = GstrHelpCode;
                txtWon1.Text = READ_WonName(txtWon.Text);
                GstrHelpCode = "";
                SendKeys.Send("{TAB}");
            }
        }

        private void frmWonhangHelpX_rSetCodeName(string strCode)
        {
            GstrHelpCode = strCode;
        }

        private void frmWonhangHelpX_rEventExit()
        {
            frmWonhangHelpX.Dispose();  
            frmWonhangHelpX = null;
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            CellClick(e.Row);
        }

        private string READ_AntiClassName(string ArgCode) // '항생제 분류명칭
        {

            string ArgReturn = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = " SELECT CODE, NAME FROM BAS_BCODE";
                SQL = SQL + " WHERE GUBUN ='BAS_항생제계열'";
                SQL = SQL + "   AND CODE = '" + ArgCode + "' ";
                SQL = SQL + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return ArgReturn;
        }

        private string READ_DaicodeName(string ArgCode)
        {
            string ArgReturn = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = "SELECT ClassName FROM BAS_CLASS ";
                SQL = SQL + ComNum.VBLF + "WHERE ClassCode=" + VB.Trim(ArgCode) + " ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["ClassName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return ArgReturn;
        }

        private string READ_DRGNAME(string ArgCode)
        {
            string ArgReturn = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "" || ArgCode == "")
                {
                    return ArgReturn;
                }

                SQL = " SELECT DCODE, DNAME ";
                SQL = SQL + "  FROM KOSMOS_PMPA.DRG_CODE_NEW ";
                SQL = SQL + " WHERE DCODE = '" + ArgCode + "' ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["DNAME"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "";
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return ArgReturn;
        }

        private string READ_WonName(string ArgCode)
        {
            string ArgReturn = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ArgReturn; //권한 확인

                if (VB.Trim(ArgCode) == "")
                {
                    return ArgReturn;
                }

                SQL = "SELECT HANGNAME FROM KOSMOS_ADM.WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE HANG='" + VB.Trim(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return ArgReturn;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgReturn = dt.Rows[0]["HangName"].ToString().Trim();
                }
                else
                {
                    ArgReturn = "** ERROR **";
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return ArgReturn;
        }

        private void rdoGbV0_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rdoGb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = sender as RadioButton;
            if(rdo.Checked)
            {
                rdo.Font = new Font(rdo.Font.Name, rdo.Font.SizeInPoints, FontStyle.Bold);
                rdo.ForeColor = Color.Red;
            }
            else
            {
                rdo.Font = new Font(rdo.Font.Name, rdo.Font.SizeInPoints, FontStyle.Regular);
                rdo.ForeColor = Color.Black;
            }
            rdo = null;
        }

        private void chkOpt13_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.Checked)
            {
                chk.Font = new Font(chk.Font.Name, chk.Font.SizeInPoints, FontStyle.Bold);
                chk.ForeColor = Color.Red;
            }
            else
            {
                chk.Font = new Font(chk.Font.Name, chk.Font.SizeInPoints, FontStyle.Regular);
                chk.ForeColor = Color.Black;
            }
            chk = null;
        }

        private void txtDaiCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtDaiCode1.Text = READ_DaicodeName(txtDaiCode.Text); 
            }
        }

        private void txtAntiClass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtAntiClass1.Text = READ_AntiClassName(txtAntiClass.Text);
            }
        }
    }
}
