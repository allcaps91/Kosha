using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirExamView.cs
    /// Description     : 검사결과조회
    /// Author          : 박성완
    /// Create Date     : 2018-05-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\EXAMVIEW.FRM
    public partial class frmComMirExamView : Form
    {
        private string FstrPano = "";

        private string FstrPano_B = "";
        private string FstrJinDate_B = "";
        private string FstrOutDate_B = "";
        private string FstrSName_B = "";
        private string FstrBi_B = "";
        private int FnAge_Mir = 0;
        private string FstrSex_MIR = "";

        public frmComMirExamView()
        {
            InitializeComponent();

            setEvent();
        }

        public frmComMirExamView(string GstrPano)
        {
            FstrPano = GstrPano;

            InitializeComponent();

            setEvent();
        }
        public frmComMirExamView(string GstrPano_B, string GstrJinDate_B, string GstrOutDate_B, string GstrSName_B, string GstrBi_B, int GnAge_Mir, string GstrSex_MIR)
        {
            FstrPano_B = GstrPano_B;
            FstrJinDate_B = GstrJinDate_B;
            FstrOutDate_B = GstrOutDate_B;
            FstrSName_B = GstrSName_B;
            FstrBi_B = GstrBi_B;
            FnAge_Mir = GnAge_Mir;
            FstrSex_MIR = GstrSex_MIR;

            InitializeComponent();

            setEvent();
        }

        private void setEvent()
        {
            Load += FrmComMirExamView_Load;

            btnSearch.Click += BtnSearch_Click;
            btnExit.Click += BtnExit_Click;

            txtPano.KeyPress += TxtPano_KeyPress;

            ss1.CellClick += Ss1_CellClick;

        }

        private void Ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                return; 
            } 

            int I = 0;
            int j = 0;
            int iRow = 0;
            string sSpecNo = "";
            string sRef = "";
            string sResultDate = "";   //결과일자
            string sStatus = "";   //상태
            string sResult = "";   //결과
            string strOK = "";   //Display여부
            int nCNT = 0;
            int nCnt1 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            sSpecNo = ss1.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            ss2.ActiveSheet.Rows.Count = 0;
            ss2.ActiveSheet.Rows.Count = 20;

            SQL = "";
            SQL += "SELECT R.Status,R.MasterCode,R.SubCode, R.Result, R.Refer, R.Panic, " + ComNum.VBLF;
            SQL += "       R.Delta, R.Unit, R.SeqNo, M.ExamName, r.resultdate  " + ComNum.VBLF;
            SQL += "  FROM KOSMOS_OCS.Exam_ResultC R, KOSMOS_OCS.Exam_Master M " + ComNum.VBLF;
            SQL += " WHERE SpecNo='" + sSpecNo + "' " + ComNum.VBLF;
            SQL += "   AND R.SubCode = M.MasterCode(+) " + ComNum.VBLF;
            SQL += " ORDER BY R.SeqNo " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("자료가 1건도 없습니다.");
                dt.Dispose();
                dt = null;
                return;
            }


            nCNT = dt.Rows.Count;
            iRow = 0;


            for (int i = 0; i < nCNT; i++)
            {
                sResultDate = dt.Rows[i]["ResultDate"].ToString().Trim();
                sStatus = dt.Rows[i]["Status"].ToString();
                sResult = dt.Rows[i]["Result"].ToString().Trim();
                if (sStatus == "H")
                {
                    strOK = "OK";
                }
                else if (sStatus == "V")
                {
                    strOK = "OK";
                    if (sResult == "") { strOK = "NO"; }
                    if (dt.Rows[i]["MasterCode"].ToString() == dt.Rows[i]["SubCode"].ToString())
                    {
                        strOK = "OK";
                    }
                }
                else
                {
                    strOK = "OK";
                    sResult = "-< 검사중 >-";
                }

                //Foot Note를 READ
                SQL = "";
                SQL += "SELECT FootNote FROM KOSMOS_OCS.Exam_ResultCf ";
                SQL += " WHERE SpecNo = '" + sSpecNo + "' ";
                SQL += "   AND SeqNo =  '" + string.Format("{0:00}", VB.Val(dt.Rows[i]["SeqNo"].ToString())) + "' ";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                nCnt1 = dt1.Rows.Count;
                if (nCnt1 > 0)
                {
                    strOK = "OK";
                }

                if (strOK == "OK")
                {
                    iRow = iRow + 1;
                    if (iRow > ss2.ActiveSheet.Rows.Count)
                    {
                        ss2.ActiveSheet.Rows.Count = iRow + 20;
                    }


                    ss2.ActiveSheet.Cells[iRow - 1, 0].Text = dt.Rows[i]["ExamName"].ToString();
                    ss2.ActiveSheet.Cells[iRow - 1, 1].Text = dt.Rows[i]["Result"].ToString();
                    ss2.ActiveSheet.Cells[iRow - 1, 2].Text = dt.Rows[i]["Refer"].ToString();
                    ss2.ActiveSheet.Cells[iRow - 1, 3].Text = dt.Rows[i]["Unit"].ToString();

                    sRef = Reference(dt.Rows[i]["SubCode"].ToString(), FnAge_Mir.ToString(), FstrSex_MIR);
                    ss2.ActiveSheet.Cells[iRow - 1, 4].Text = sRef;
                    ss2.ActiveSheet.Cells[iRow - 1, 5].Text = dt.Rows[i]["SubCode"].ToString();
                }

                dt1.Dispose();
                dt1 = null;

                if (nCnt1 > 0)
                {
                    //Foot Note를 READ
                    SQL = "";
                    SQL += "SELECT FootNote FROM KOSMOS_OCS.Exam_ResultCf ";
                    SQL += " WHERE SpecNo = '" + sSpecNo + "' ";
                    SQL += "   AND SeqNo =  '" + string.Format("{0:00}", VB.Val(dt.Rows[i]["SeqNO"].ToString())) + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    for (j = 0; j < nCnt1; j++)
                    {
                        iRow = iRow + 1;
                        if (iRow > ss2.ActiveSheet.Rows.Count)
                        {
                            ss2.ActiveSheet.Rows.Count = iRow + 20;
                        }

                        ss2.ActiveSheet.Cells[iRow - 1, 0].Text = "  " + dt1.Rows[j]["FootNote"].ToString();
                        ss2.ActiveSheet.Cells[iRow - 1, 0].ForeColor = Color.Blue;
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
            }

            if (iRow > 21)
            {
                ss2.ActiveSheet.Rows.Count = iRow;
            }

            //혈액배양검사
            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ";
            SQL += " RESULT FROM KOSMOS_OCS.EXAM_RESULT_BAE ";
            SQL += " WHERE SPECNO  = '" + sSpecNo + "' ";
            SQL += "   AND PANO = '" + txtPano.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

            ss3.ActiveSheet.Rows.Count = 0;

            for (I =0; I< dt1.Rows.Count; I++)
            {
                ss3.ActiveSheet.Rows.Count++;

                ss3.ActiveSheet.Cells[I, 0].Text = dt1.Rows[I]["BDATE"].ToString().Trim();
                ss3.ActiveSheet.Cells[I, 1].Text = dt1.Rows[I]["RESULT"].ToString().Trim();
            }

            dt1.Dispose();
            dt1 = null;

        }

        private string Reference(string Code, string Age, string Sex)
        {
            string rtnVal = "";

            int I = 0;
            int j = 0;
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

            SQL = "";
            SQL += "SELECT MasterCode, Normal, Sex, AgeFrom, AgeTo, RefvalFrom, RefvalTo ";
            SQL += "  FROM KOSMOS_OCS.Exam_Master_Sub ";
            SQL += " WHERE MasterCode = '" + Code + "' AND Gubun = '41'";  //41:Reference Value
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            for (I = 0; I < dt.Rows.Count; I++)
            {
                sCode = dt.Rows[I]["MasterCode"].ToString().Trim();
                sNormal = dt.Rows[I]["Normal"].ToString().Trim();
                sSex = dt.Rows[I]["Sex"].ToString().Trim();
                sAgeFrom = dt.Rows[I]["AgeFrom"].ToString().Trim();
                sAgeTo = dt.Rows[I]["AgeTo"].ToString().Trim();
                sRefValFrom = dt.Rows[I]["RefvalFrom"].ToString().Trim();
                sRefValTo = dt.Rows[I]["RefvalTo"].ToString().Trim();

                sAllReference = sAllReference + sCode + "|" + sNormal + "|" + sSex + "|" + sAgeFrom + "|" + sAgeTo + "|" + sRefValFrom + "|" + sRefValTo + "|" + "|";
            }

            dt.Dispose();
            dt = null;

            sReference = VB.TR(sAllReference, "" + sCode + "", "^");

            I = (int)VB.L(sReference, "^");

            if (I == 1)
            {
                return rtnVal;
            }

            for (j = 2; j <= I; j++)
            {
                sNormal = VB.Pstr(VB.Pstr(sReference, "^", j), "|", 2);
                sSex = VB.Pstr(VB.Pstr(sReference, "^", j), "|", 3);
                sAgeFrom= VB.Pstr(VB.Pstr(sReference, "^", j), "|", 4);
                sAgeTo = VB.Pstr(VB.Pstr(sReference, "^", j), "|", 5);
                sRefValFrom = VB.Pstr(VB.Pstr(sReference, "^", j), "|", 6);
                sRefValTo = VB.Pstr(VB.Pstr(sReference, "^", j), "|", 7);

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
        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            txtPano.Text = string.Format("{0:00000000}", VB.Val(txtPano.Text));

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT SNAME, BI FROM KOSMOS_PMPA.BAS_PATIENT " + ComNum.VBLF;
            SQL += " WHERE PANO = '" + txtPano.Text + "' " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                lblName.Text = dt.Rows[0]["SNAME"].ToString();
                lblBi.Text = dt.Rows[0]["BI"].ToString();
            }
            else
            {
                MessageBox.Show("해당하는 환자가 없습ㄴ디ㅏ.");
                lblName.Text = "";
                lblBi.Text = "";
            }

            dt.Dispose();
            dt = null;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ViewData();
        }

        private void ViewData()
        {
            string strSpecNo = "";
            string strStatus = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ss1.ActiveSheet.Rows.Count = 0;
            ss1.ActiveSheet.Rows.Count = 20;
            ss2.ActiveSheet.Rows.Count = 0;
            ss2.ActiveSheet.Rows.Count = 20;

            SQL = "";
            SQL += "SELECT E.SpecNo, E.DeptCode, E.Room, E.DrCode, E.WorkSTS, E.SpecCode, E.Status, " + ComNum.VBLF;
            SQL += "       E.IpdOpd, E.Bi, TO_CHAR(E.BDate, 'YYYY-MM-DD') BDate, S.NAME," + ComNum.VBLF;
            SQL += "       TO_CHAR(E.BloodDate, 'YYYY-MM-DD HH24:MI') BloodDate," + ComNum.VBLF;
            SQL += "       TO_CHAR(E.ReceiveDate, 'YYYY-MM-DD HH24:MI') ReceiveDate," + ComNum.VBLF;
            SQL += "       TO_CHAR(E.ResultDate, 'YYYY-MM-DD HH24:MI') ResultDate,Print " + ComNum.VBLF;
            SQL += "  FROM KOSMOS_OCS.Exam_Specmst E, " + ComNum.VBLF;
            SQL += "       KOSMOS_OCS.EXAM_SPECODE S" + ComNum.VBLF;
            SQL += " WHERE Pano ='" + txtPano.Text + "' " + ComNum.VBLF;
            SQL += "   AND BDate >= TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += "   AND BDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += "   AND E.SPECCODE = S.CODE " + ComNum.VBLF;
            SQL += "   AND S.GUBUN ='14'" + ComNum.VBLF;
            SQL += "   AND WorkSTS NOT IN ('A','T') " + ComNum.VBLF;    //세포학,조직학은 제외
            //'SQL = SQL & "   AND Status IN ('04','14','05') "
            SQL += " ORDER BY ReceiveDate DESC,SpecNo " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            ss1.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i=0; i< dt.Rows.Count; i++)
            {
                strSpecNo = dt.Rows[i]["SpecNo"].ToString();

                ss1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SpecNo"].ToString();
                ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString();
                ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ReceiveDate"].ToString();

                switch (dt.Rows[i]["IpdOpd"].ToString())
                {
                    case "I": ss1.ActiveSheet.Cells[i, 3].Text = "입원"; break;
                    default:
                        {
                            switch (dt.Rows[i]["BI"].ToString())
                            {
                                case "61": ss1.ActiveSheet.Cells[i, 3].Text = "종검"; break;
                                case "71": ss1.ActiveSheet.Cells[i, 3].Text = "건진"; break;
                                case "81": ss1.ActiveSheet.Cells[i, 3].Text = "수탁"; break;
                                default: ss1.ActiveSheet.Cells[i, 3].Text = "외래"; break;
                            }
                            break;
                        }
                }

                ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString();
                ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Room"].ToString();
                ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DrCode"].ToString();
                ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["WorkSTS"].ToString();

                ss1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Name"].ToString();
                ss1.ActiveSheet.Cells[i, 9].Text = READ_Specno_ExamName(strSpecNo);

                switch (dt.Rows[i]["Status"].ToString())
                {
                    case "00": strStatus = "미접수"; break;
                    case "01": strStatus = "검사중"; break;
                    case "02": strStatus = "부분입력"; break;
                    case "03": strStatus = "모두입력"; break;
                    case "04":
                    case "14":
                        strStatus = "부분완료"; break;
                    case "05":
                        if (VB.Val(dt.Rows[i]["Print"].ToString()) == 0) { strStatus = "검사완료"; }
                        if (VB.Val(dt.Rows[i]["Print"].ToString()) > 0) { strStatus = "인쇄완료"; }
                        break;
                    case "06": strStatus = "취소"; break;
                    default: strStatus = "ERROR"; break;
                }

                ss1.ActiveSheet.Cells[i, 10].Text = strStatus;
                if (strStatus == "일부완료")
                {
                    ss1.ActiveSheet.Cells[i, 10].BackColor = Color.Pink;
                }
                else if (strStatus == "검사완료" || strStatus == "인쇄완료")
                {
                    ss1.ActiveSheet.Cells[i, 10].BackColor = Color.LightGreen;
                }
                else if (dt.Rows[i]["Bi"].ToString() == "61" && strStatus == "전송완료")
                {
                    ss1.ActiveSheet.Cells[i, 10].BackColor = Color.OrangeRed;
                }
                else if (strStatus == "전송완료")
                {
                    ss1.ActiveSheet.Cells[i, 10].BackColor = Color.SkyBlue;
                }
            }

            dt.Dispose();
            dt = null;
        }

        private string READ_Specno_ExamName(string strSpecNo)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT b.WsCode1,b.WsCode1Pos,a.MasterCode,b.ExamName,COUNT(a.MasterCode) CNT " + ComNum.VBLF;
            SQL += "  FROM KOSMOS_OCS.EXAM_RESULTC a,KOSMOS_OCS.EXAM_MASTER b " + ComNum.VBLF;
            SQL += " WHERE a.SpecNo = '" + strSpecNo +"' " + ComNum.VBLF;
            SQL += "   AND a.MasterCode = a.SubCode " + ComNum.VBLF;
            SQL += "   AND a.MasterCode = b.MasterCode(+) " + ComNum.VBLF;
            SQL += " GROUP BY b.WsCode1,b.WsCode1Pos,a.MasterCode,b.ExamName " + ComNum.VBLF;
            SQL += " ORDER BY b.WsCode1,b.WsCode1Pos,a.MasterCode,b.ExamName " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rtnVal = rtnVal + dt.Rows[i]["ExamName"].ToString().Trim();
                if (VB.Val(dt.Rows[i]["CNT"].ToString()) > 1)
                {
                    rtnVal = rtnVal + "*" + VB.Val(dt.Rows[i]["CNT"].ToString()) + ",";
                }
                else
                {
                    rtnVal = rtnVal + ",";
                }
            }


            if (rtnVal != "")
            {
                rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void FrmComMirExamView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";

            if (FstrPano_B == "")
            {
                txtPano.Text = FstrPano;
                dtpSDate.Text = "2018-01-01";
                dtpEDate.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                txtPano.Text = FstrPano_B;
                dtpSDate.Text = FstrJinDate_B;
                dtpEDate.Text = FstrOutDate_B;
                lblName.Text = FstrSName_B;
                lblBi.Text = FstrBi_B;
            }

            if (txtPano.Text != "")
            {                
                TxtPano_KeyPress(txtPano, new KeyPressEventArgs((char)13));
                ViewData();
            }
        }
    }
}
