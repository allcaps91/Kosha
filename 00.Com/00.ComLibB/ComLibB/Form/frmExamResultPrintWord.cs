using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmExamResultPrintWord.cs
    /// Description     : 진단검사 결과 출력(입원, 외래) 기록실용 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-13
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\mid\midout\FrmExamResultPrint.frm(FrmExamResultPrint) => frmExamResultPrintWord.cs 으로 변경함
    /// VB IsMissing, Typename 구현 필요, VB PP함수 구현 필요
    /// 기존소스와 비교했을 때, sAllReference에 담기는 값은 같으나 TR함수를 거쳐 sReference에 담기는 값이 다름
    /// 그로 인해 아래 i변수에 담기는 값도 다름
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mid\midout\FrmExamResultPrint.frm(FrmExamResultPrint)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mid\midout\midout.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmExamResultPrintWord : Form
    {
        string strFlagIM = "";
        string strBi = "";
        string strBiName = "";
        string strDEPTCODE = "";
        string strDeptName = "";
        string strSName = "";
        string strInDate = "";
        string strOutDate = "";
        string strIlsu = "";
        string sSexAge = "";
        string strGbSpc = "";

        int FnRowExam = 0;        

        public frmExamResultPrintWord()
        {
            InitializeComponent();
        }
        
        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmExamResultPrintWord_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Load_Combos();

            optGbn2.Checked = true;
            lblSName.Text = "";           
        }

        void Load_Combos()
        {
            int i = 0;
            string strDept = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //환자종류
            cboBi.Items.Clear();
            cboBi.Items.Add("00.전체");
            cboBi.Items.Add("11.공단");
            cboBi.Items.Add("12.연합회");
            cboBi.Items.Add("13.지역");
            cboBi.Items.Add("21.보호1종");
            cboBi.Items.Add("22.보호2종");
            cboBi.Items.Add("23.의료부조");
            cboBi.Items.Add("24.행려환자");
            cboBi.Items.Add("31.산재");
            cboBi.Items.Add("32.공무원공상");
            cboBi.Items.Add("33.산재공상");
            cboBi.Items.Add("41.공단100%");
            cboBi.Items.Add("42.직장100%");
            cboBi.Items.Add("43.지역100%");
            cboBi.Items.Add("44.가족계획");
            cboBi.Items.Add("45.보험계약");
            cboBi.Items.Add("51.일반");
            cboBi.Items.Add("52.TA보험");
            cboBi.Items.Add("53.계약");
            cboBi.Items.Add("54.미확인");
            cboBi.Items.Add("55.TA일반");
            cboBi.SelectedIndex = 0;

            //진료과
            cboDept.Items.Clear();
            cboDept.Items.Add("00.전체");

            strDept = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + " * FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE PrintRanking < 30";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strDept += dt.Rows[i]["DeptNameK"].ToString().Trim();
                    cboDept.Items.Add(strDept);
                }
            }

            cboDept.SelectedIndex = 0;

            dt.Dispose();
            dt = null;

            
        }

        static public void IsMissing(int? i1_optional, object f_optional)
        {
            int i1 = ((i1_optional == null) ? 0 : i1_optional.Value);
            Form f = ((f_optional == Type.Missing) ? null : f_optional as Form);
            if (i1_optional == null)
            {
                MessageBox.Show("i1 is Missing", Application.ProductName);
            }
            if (f_optional == Type.Missing)
            {
                MessageBox.Show("f is Missing", Application.ProductName);
            }
        }

        void Clear_Screen()
        {
            ssList_Sheet1.RowCount = 0;
            txtPano.Text = "";
            lblSName.Text = "";
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            GetData();
        }

        void GetData()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strBi = VB.Left(cboBi.SelectedItem.ToString(), 2);
            strDEPTCODE = VB.Left(cboDept.SelectedItem.ToString(), 2);

            if(dtpFDate.Text == "")
            {
                ComFunc.MsgBox("시작일자를 입력하세요 !!");
            }

            if(dtpTDate.Text == "")
            {
                ComFunc.MsgBox("종료일자를 입력하세요 !!");
            }

            FnRowExam = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SPECNO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_SPECMST";
            SQL += ComNum.VBLF + "WHERE PANO = '" + txtPano.Text + "'";
            SQL += ComNum.VBLF + "  AND BDATE >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND BDATE <=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND STATUS = '05'";
            if(strBi != "00")
            {
                SQL += ComNum.VBLF + "  AND Bi   = '" + strBi + "' ";
            }

            if(strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "  AND DeptCode = '" + strDEPTCODE + "'  ";
            }

            if(optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND IPDOPD ='O' ";
            }

            if(optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND IPDOPD ='I'  ";
            }
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for(i = 0; i < dt.Rows.Count; i++)
            {
                Exam_Result_Display(dt.Rows[i]["SPECNO"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            cmdPrint();
        }

        void cmdPrint()
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ComFunc.MsgBoxQ("인쇄를 하시겠습니까?", "알림", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFoot = "";
            string strPrintTitle = "";
            string strIO = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strIO = " [입원/외래]";
            if(optGbn0.Checked == true)
            {
                strIO = " [외래]";
            }
            else if(optGbn1.Checked == true)
            {
                strIO = " [입원]";
            }

            strPrintTitle = txtPano.Text + "  " + strSName + "  ";
            strPrintTitle += "구분: " + cboBi.SelectedItem.ToString() + VB.Space(5) + "진료과 : " + cboDept.SelectedItem.ToString();
            strPrintTitle += "   기간: " + dtpFDate.Text + "-" + dtpTDate.Text;

            // 프린트 헤드 지정
            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1/c" + "진단검사 결과" + strIO + "/n/n";
            strHead2 = "/l/f2" + strPrintTitle;

            //프린트 바디 
            ssList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssList_Sheet1.PrintInfo.Footer = strFont2 + strFoot;
            
            ssList_Sheet1.PrintInfo.Margin.Top = 700;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 1000;
            ssList_Sheet1.PrintInfo.Margin.Left = 700;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;

            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;


            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = true;
            ssList_Sheet1.PrintInfo.ShowGrid = false;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = true;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList.PrintSheet(0);

            clsDB.setBeginTran(clsDB.DbCon);
                                  
        }

        void cboBi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        string Reference(string Code, string Age, string Sex)
        {
            string rtnVal = "";

            int i = 0;
            int j = 0;
            int k = 0;

            string sCode = "";
            string sNormal = "";
            string sSex = "";
            string sAgeFrom = "";
            string sAgeTo = "";
            string sRefValFrom = "";
            string sRefValTo = "";

            string sAllReference = "";
            string sReference = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;           

            try
            {
                SQL = "";
                SQL = " SELECT MasterCode, Normal, Sex, AgeFrom, AgeTo, RefvalFrom, RefvalTo ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "Exam_Master_Sub ";
                SQL = SQL + ComNum.VBLF + "WHERE MasterCode = '" + Code + "' AND Gubun = '41'";   //41:Reference Value

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    sCode = dt.Rows[i]["MasterCode"].ToString().Trim();
                    sNormal = dt.Rows[i]["Normal"].ToString().Trim();
                    sSex = dt.Rows[i]["Sex"].ToString();
                    sAgeFrom = dt.Rows[i]["AgeFrom"].ToString();
                    sAgeTo = dt.Rows[i]["AgeTo"].ToString().Trim();
                    sRefValFrom = dt.Rows[i]["RefvalFrom"].ToString().Trim();
                    sRefValTo = dt.Rows[i]["RefvalTo"].ToString();

                    sAllReference = sAllReference + sCode + "|" + sNormal + "|" + sSex + "|" + sAgeFrom 
                                + "|" + sAgeTo + "|" + sRefValFrom + "|" + sRefValTo + "|" + "|";
                }

                // TODO VB.TR 함수 소스 점검 필요
                // 기존소스와 비교했을 때, sAllReference에 담기는 값은 같으나 TR함수를 거쳐 sReference에 담기는 값이 다름
                // 그로 인해 아래 i변수에 담기는 값도 다름
                //sReference = VB.TR(sAllReference, "" + sCode + "", "^");
                sReference = TR(sAllReference, "" + sCode + "", "^");


                i = VB.I(sReference, "^");

                //i 값이 1이 계속 담기기때문에 아래부분 테스트 진행X
                if (i == 1)
                {
                    return rtnVal;
                }

                for (j = 2; j < i; j++)
                {
                    //sNormal = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 2);
                    sNormal = PP(PP(sReference, "^", j), "|", 2);
                    //sSex = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 3);
                    sSex = PP(PP(sReference, "^", j), "|", 3);
                    //sAgeFrom = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 4);
                    sAgeFrom = PP(PP(sReference, "^", j), "|", 4);
                    //sAgeTo = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 5);
                    sAgeTo = PP(PP(sReference, "^", j), "|", 5);
                    //sRefValFrom = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 6);
                    sRefValFrom = PP(PP(sReference, "^", j), "|", 6);
                    //sRefValTo = VB.SinglePiece(VB.SinglePiece(sReference, "^", j), "|", 7);
                    sRefValTo = PP(PP(sReference, "^", j), "|", 7);

                    if (sNormal != "")
                    {
                        rtnVal = sNormal;
                        return rtnVal;
                    }

                    if (sSex == "" || sSex == Sex)
                    {
                        if (sAgeFrom != "" && sAgeTo != "")
                        {
                            if (VB.Val(sAgeFrom) <= VB.Val(Age) && VB.Val(Age) <= VB.Val(sAgeTo))
                            {
                                rtnVal = sRefValFrom + " ~ " + sRefValTo;
                                return rtnVal;
                            }
                        }
                    }
                }

                return rtnVal;

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

            
        }

        void Exam_Result_Display(string ArgSpecno)
        {          
            int i = 0;
            int j = 0;
            int iRow = 0;
            int intCNT = 0;
            int nCnt1 = 0;
            int nCNT = 0;

            string strDate = "";
            string strSpecNo = "";
            string strCompare = "";
            string strRef = "";
            string strFDate = "";
            string strTDate = "";
            string strWsCode = "";   //WS약어
            string strResultDate = "";   //결과일자
            string strStatus = "";   //상태
            string strResult = "";   //결과
            string strOK = "";   //Display여부
            string strFootNote = "";   //FootNote

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL = "SELECT R.Status,R.MasterCode,R.SubCode, R.Result, R.Refer, R.Panic, ";
                SQL = SQL + ComNum.VBLF + " R.Delta, R.Unit, R.SeqNo, M.ExamName, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL = SQL + ComNum.VBLF + " R.ResultSabun, TO_CHAR(S.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(S.ResultDate,'YYYY-MM-DD HH24:MI') RESULTDATE2, S.AGE, S.SEX, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(S.BDATE, 'YYYY-MM-DD') BDATE, P.KORNAME, D.DRNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M, ";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED + "EXAM_SPECMST S, " + ComNum.DB_ERP + "INSA_MST P, ";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + "WHERE R.SpecNo='" + ArgSpecno + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.SPECNO = S.SPECNO(+)";
                SQL = SQL + ComNum.VBLF + "  AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.RESULTSABUN = P.SABUN(+)";
                SQL = SQL + ComNum.VBLF + "  AND S.DRCODE = D.DRCODE(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SeqNo ";

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

                nCNT = dt.Rows.Count;
                strCompare = "";
                ssList_Sheet1.RowCount = FnRowExam;

                for (i = 0; i < nCNT; i++)
                {
                    if (i == 0)
                    {
                        FnRowExam += 1;
                        ssList_Sheet1.Rows.Count = FnRowExam;

                        ssList_Sheet1.Cells[FnRowExam -1, 0].Text = "  ▶ 처방일자: " + dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[FnRowExam- 1, 1].Text = "   DR.: " + dt.Rows[i]["DRNAME"].ToString().Trim();
                    }

                    strResultDate = dt.Rows[i]["ResultDate"].ToString().Trim();
                    strStatus = dt.Rows[i]["Status"].ToString().Trim();
                    strResult = dt.Rows[i]["Result"].ToString().Trim();

                    if (strStatus == "H")
                    {
                        strOK = "OK";
                    }
                    else if (strStatus == "V")
                    {
                        strOK = "OK";

                        if (strResult == "")
                        {
                            strOK = "NO";
                        }

                        if (dt.Rows[i]["MasterCode"].ToString().Trim() == dt.Rows[i]["SubCode"].ToString().Trim())
                        {
                            strOK = "OK";
                        }
                    }

                    else
                    {
                        strOK = "OK";
                        strResult = "-< 검사중 >-";
                    }

                    //Foot Note를 READ
                    //strSpecNo에 Null값 들어감
                    SQL = "";
                    SQL = " SELECT FootNote FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                    SQL = SQL + ComNum.VBLF + "WHERE SpecNo = '" + strSpecNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SeqNo =  " + dt.Rows[i]["SeqNo"].ToString().Trim() + "  ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    //if (dt1.Rows.Count == 0)
                    //{
                    //    dt1.Dispose();
                    //    dt1 = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    nCnt1 = dt1.Rows.Count;
                    
                    if (nCnt1 > 0)
                    {
                        strOK = "OK";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strOK == "OK")
                    {
                        FnRowExam += 1;
                        if(FnRowExam > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = FnRowExam + 20;
                        }
                         
                        ssList_Sheet1.Cells[FnRowExam -1, 0].Text = "    " + dt.Rows[i]["ExamName"].ToString().Trim(); //검사이름
                        ssList_Sheet1.Cells[FnRowExam -1, 1].Text = "    " + dt.Rows[i]["Result"].ToString().Trim();   //결과치
                        ssList_Sheet1.Cells[FnRowExam -1, 2].Text = dt.Rows[i]["Refer"].ToString().Trim();
                        ssList_Sheet1.Cells[FnRowExam -1, 3].Text = "    " + dt.Rows[i]["Unit"].ToString().Trim();     //결과단위

                        strRef = Reference(dt.Rows[i]["SubCode"].ToString().Trim(), dt.Rows[i]["AGE"].ToString().Trim(), dt.Rows[i]["SEX"].ToString().Trim());
                        ssList_Sheet1.Cells[FnRowExam -1, 4].Text = "    " + strRef; //참고치

                        if (dt.Rows[i]["UNIT"].ToString().Trim() != "None")
                        {
                            ssList_Sheet1.Cells[FnRowExam -1, 6].Text = dt.Rows[i]["KORNAME"].ToString().Trim(); //검사자
                        }
                    }

                    if (nCnt1 > 0)
                    {
                        //Foot Note를 READ
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  FootNote";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "Exam_ResultCf";
                        SQL += ComNum.VBLF + "WHERE SpecNo = '" + ArgSpecno + "' ";
                        SQL += ComNum.VBLF + "  AND SeqNo =  " + dt.Rows[i]["SeqNo"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        for (j = 0; j < nCnt1; j++)
                        {
                            FnRowExam += 1;
                            if (FnRowExam > ssList_Sheet1.RowCount)
                            {
                                ssList_Sheet1.RowCount = FnRowExam + 20;
                            }
                            ssList_Sheet1.Cells[FnRowExam, 0].Text = "  " + dt1.Rows[j]["FootNote"].ToString().Trim();
                            ssList_Sheet1.Cells[FnRowExam, 0].ForeColor = Color.Blue;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                if (FnRowExam > 21)
                {
                    ssList_Sheet1.RowCount = FnRowExam;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
           
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Clear_Screen();

            txtPano.Enabled = true;
            txtPano.Text = "";
            txtPano.Focus();
        }

        void Read_Bas_Patient()
        {
            int i = 0;
            int j = 0;
            string strMsg = "";
            string strList = "";

            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  Sname";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE Pano = '" + txtPano.Text + "' ";
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

            strSName = dt.Rows[0]["Sname"].ToString().Trim();
            lblSName.Text = strSName;

            dt.Dispose();
            dt = null;
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if (!VB.IsNumeric(txtPano.Text))
                {
                    //Beep
                    //TxtPano.SelStart = 0
                    //TxtPano.SelLength = Len(TxtPano.Text)
                }
                else
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                    Read_Bas_Patient();
                    SendKeys.Send("{TAB}");
                }
            }
        }

        string PP(string var, string Del, int FromCnt, int optArg1 = 0, string optArg2 = "")
        {
            int Prt = 0;
            int Srt = 0;
            int Nxt = 0;
            int ToCnt = 0;
            string XCH = "";
            int FromBuf = 0;
            int ChkArg = 0;

            string rtnVal = "";

            //TODO IsMissing, Typename구현필요
            //ChkArg = IsMissing(optArg1) + IsMissing(optArg2)
            
            //IsMissing 함수를 대신한다.
            if(optArg1 == 0 && optArg2 == null)
            {
                ChkArg = -2;
            }
            else if((optArg1 == 0 && optArg2 != null) || (optArg1 != 0 && optArg2 == null))
            {
                ChkArg = -1;
            }
            else
            {
                ChkArg = 0;
            }

            switch (ChkArg)
            {
                case -2:
                    rtnVal = VB.SinglePiece(var, Del, FromCnt);
                    break;

                case -1:
                    //If TypeName(optArg1) = "Integer" Or TypeName(optArg1) = "Long" Then ' multi piece

                    // PP = MultiPiece(var, Del, FromCnt, optArg1)

                    //Else    ' single piece set

                    // PP = SinglePieceSet(var, Del, FromCnt, optArg1)

                    //End If
                    break;

                case 0:
                    rtnVal = VB.MultiPieceSet(var, Del, FromCnt, optArg1, optArg2);
                    break;
               
            }

            return rtnVal;
        }

        string TR(string var, string Del, string XCH)
        {
            string rtnVal = "";

            int Srt = 0;
            int Nxt = 0;
            int TMP = 0;
            string BUF = "";

            if(Del == "")
            {
                rtnVal = var;
                return rtnVal;
            }

            Nxt = (VB.Len(Del) * -1) + 1;

            do
            {
                Srt = Nxt + VB.Len(Del);
                TMP = Nxt + VB.Len(Del) - 1;
                Nxt = VB.InStr(Srt, var, Del);
                if (Nxt != 0)
                {
                    BUF += VB.Mid(var, Srt, (Nxt - Srt)) + XCH;
                }
                else
                {
                    BUF += VB.Right(var, VB.Len(var) - TMP);
                }
            } while (Nxt == 0);

            rtnVal = BUF;

            return rtnVal;
        }
    }
}
