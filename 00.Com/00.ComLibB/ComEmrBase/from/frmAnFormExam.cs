using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmAnFormExam : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetPatientInfo(string Hb, string Hct, string Plt, string Wbc, string Na, string K, string GOT, string GPT, string PT, string PTT);
        public event GetPatientInfo rGetPatientInfo;

        //폼이 Close 될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string mstrPANO = "";
        string mstrSNAME = "";
        int mintAge = 0;
        string mstrSex = "";
        string mstrJumin1 = "";
        string mstrJumin2 = "";

        public frmAnFormExam()
        {
            InitializeComponent();
        }

        public frmAnFormExam(string PANO, string Hb, string Hct, string Plt, string Wbc, string Na, string K, string GOT, string GPT, string PT, string PTT)
        {
            InitializeComponent();
            mstrPANO = PANO;
            txtHb.Text = Hb;
            txtHct.Text = Hct;
            txtPLT.Text = Plt;
            txtWBC.Text = Wbc;
            txtNa.Text = Na;
            txtK.Text = K;
            txtGOT.Text = GOT;
            txtGPT.Text = GPT;
            txtPT.Text = PT;
            txtPTT.Text = PTT;
        }

        private void frmAnFormExam_Load(object sender, EventArgs e)
        {
            READ_PATIENT(mstrPANO);
            SetList();
        }

        private void SetList()
        {
            ssList.ActiveSheet.RowCount = 0;
            ssList.ActiveSheet.RowCount = 10;

            ssList.ActiveSheet.Cells[0, 0].Text = "Hb";
            ssList.ActiveSheet.Cells[0, 1].Text = "Hgb";
            
            ssList.ActiveSheet.Cells[1, 0].Text = "Hct";
            ssList.ActiveSheet.Cells[1, 1].Text = "Hct";

            ssList.ActiveSheet.Cells[2, 0].Text = "PLT";
            ssList.ActiveSheet.Cells[2, 1].Text = "PLT";

            ssList.ActiveSheet.Cells[3, 0].Text = "WBC";
            ssList.ActiveSheet.Cells[3, 1].Text = "WBC";

            ssList.ActiveSheet.Cells[4, 0].Text = "GOT";
            ssList.ActiveSheet.Cells[4, 1].Text = "AST";

            ssList.ActiveSheet.Cells[5, 0].Text = "GPT";
            ssList.ActiveSheet.Cells[5, 1].Text = "ALT";

            ssList.ActiveSheet.Cells[6, 0].Text = "Na";
            ssList.ActiveSheet.Cells[6, 1].Text = "Sodium";
            
            ssList.ActiveSheet.Cells[7, 0].Text = "K";
            ssList.ActiveSheet.Cells[7, 1].Text = "Potassium";

            ssList.ActiveSheet.Cells[8, 0].Text = "PT";
            ssList.ActiveSheet.Cells[8, 1].Text = "PT";

            ssList.ActiveSheet.Cells[9, 0].Text = "PTT";
            ssList.ActiveSheet.Cells[9, 1].Text = "aPTT";
        }

        private void ssList_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            string EXAMYNAME = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, 1].Text;
            GetData(mstrPANO, EXAMYNAME);
        }

        private void GetData(string PANO, string EXAMYNAME)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strRef = ""; //참고치
            string strRDate = "";
            ssView.ActiveSheet.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SPECNO, A.RESULTDATE, C.EXAMNAME, B.MASTERCODE, B.SUBCODE, B.RESULT, B.REFER, B.UNIT, B.SEQNO   ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                                         ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.EXAM_RESULTC B                                                                    ";
                SQL = SQL + ComNum.VBLF + "    ON A.SPECNO = B.SPECNO                                                                               ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_OCS.EXAM_MASTER C                                                                     ";
                SQL = SQL + ComNum.VBLF + "    ON B.SUBCODE = C.MASTERCODE                                                                          ";
                SQL = SQL + ComNum.VBLF + "   AND C.EXAMYNAME = '" + EXAMYNAME + "'                                                                 ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + PANO + "'                                                                           ";
                SQL = SQL + ComNum.VBLF + "   AND A.RESULTDATE IS NOT NULL                                                                          ";
                //SQL = SQL + ComNum.VBLF + "   AND TRUNC(A.RESULTDATE) <= TO_DATE('2020-01-08', 'YYYY-MM-DD')                                      ";
                SQL = SQL + ComNum.VBLF + " ORDER BY RESULTDATE DESC                                                                                ";
                                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                ssView.ActiveSheet.Rows.Count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["RESULTDATE"].ToString();
                    ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["EXAMNAME"].ToString();
                    if (EXAMYNAME.Equals("PLT") || EXAMYNAME.Equals("WBC"))
                    {
                        if (dt.Rows[i]["RESULT"].ToString().Trim() != "")
                        {
                            ssView.ActiveSheet.Cells[i, 2].Text = string.Format("{0:###,###,###}",VB.Val(dt.Rows[i]["RESULT"].ToString()) * 1000);
                            //ssView.ActiveSheet.Cells[i, 2].Text = Convert.ToString(VB.Val(dt.Rows[i]["RESULT"].ToString()) * 1000);
                        }
                        else
                        {
                            ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RESULT"].ToString();
                        }
                    }
                    else
                    {
                        ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["RESULT"].ToString();
                    }
                    ssView.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["REFER"].ToString();
                    ssView.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["UNIT"].ToString();

                    strRDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                    if (VB.IsDate(strRDate) == true)
                    {
                        strRef = GetReference(dt.Rows[i]["SUBCODE"].ToString().Trim(), Convert.ToString(mintAge), mstrSex, Convert.ToDateTime(strRDate).ToString("yyyy-MM-dd"));
                        ssView.ActiveSheet.Cells[i, 5].Text = strRef;
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
        }

        private void READ_PATIENT(string PANO)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            { 
                SQL = "";
                SQL = "SELECT PANO,SNAME,JUMIN1,JUMIN2,JUMIN3,SEX ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + PANO + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {                
                    mstrSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                    clsAES.Read_Jumin_AES(clsDB.DbCon, PANO);
                    mstrJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    mstrJumin2 = clsAES.GstrAesJumin2;
                    mstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                    mintAge = ComFunc.AgeCalc(clsDB.DbCon, mstrJumin1 + mstrJumin2);

                    lblPatient.Text = mstrSNAME + " (" + mstrPANO + ") " + mstrSex + "/" + mintAge;
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
        }

        private string GetReference(string astrCode, string astrAge, string astrSex, string argRdate)
        {
            int i = 0;
            string strVal = "";
            string strCode = "";
            string strNormal = "";
            string strSex = "";
            string strAgeFrom = "";
            string strAgeTo = "";
            string strRefValFrom = "";
            string strRefValTo = "";

            string strAllReference = "";
            string strReference = "";
            string strReferenceVal = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT MASTERCODE, NORMAL, SEX, AGEFROM, AGETO, REFVALFROM, REFVALTO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_MASTER_SUB ";
                SQL = SQL + ComNum.VBLF + "WHERE  1=1"; //'41:Reference Value
                //2018-12-12 안정수 조건 추가함 
                SQL = SQL + ComNum.VBLF + "AND MASTERCODE = '" + astrCode + "'";
                SQL = SQL + ComNum.VBLF + "AND GUBUN = '41'";
                SQL = SQL + ComNum.VBLF + "AND (SEX IS NULL OR SEX = ' ' OR SEX= '" + astrSex + "')  ";
                SQL = SQL + ComNum.VBLF + "AND ((AGEFROM = 0 AND AGETO = 99) OR  (AGEFROM <= '" + astrAge + "' AND AGETO >= '" + astrAge + "'))  ";
                if (argRdate.Length > 1)
                {
                    SQL = SQL + ComNum.VBLF + "AND ((EXPIREDATE IS NOT NULL AND EXPIREDATE >= '" + argRdate + "') OR (EXPIREDATE IS NULL)) ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY EXPIREDATE";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCode = dt.Rows[i]["MASTERCODE"].ToString().Trim();
                        strNormal = dt.Rows[i]["NORMAL"].ToString().Trim();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        strAgeFrom = dt.Rows[i]["AGEFROM"].ToString().Trim();
                        strAgeTo = dt.Rows[i]["AGETO"].ToString().Trim();
                        strRefValFrom = dt.Rows[i]["REFVALFROM"].ToString().Trim();
                        strRefValTo = dt.Rows[i]["REFVALTO"].ToString().Trim();

                        strAllReference = strAllReference + "|" + strCode + "|" + strNormal + "|" + strSex + "|" + strAgeFrom + "|" +
                                        strAgeTo + "|" + strRefValFrom + "|" + strRefValTo;
                    }
                }

                dt.Dispose();
                dt = null;

                if (strCode == "")
                {
                    return strVal;
                }

                strReference = strAllReference.Replace(strCode, "^");

                for (i = 1; i < VB.Split(strReference, "|^").Length; i++)
                {
                    strReferenceVal = VB.Split(strReference, "|^")[i];

                    strNormal = VB.Split(strReferenceVal, "|")[1];
                    strSex = VB.Split(strReferenceVal, "|")[2];
                    strAgeFrom = VB.Split(strReferenceVal, "|")[3];
                    strAgeTo = VB.Split(strReferenceVal, "|")[4];
                    strRefValFrom = VB.Split(strReferenceVal, "|")[5];
                    strRefValTo = VB.Split(strReferenceVal, "|")[6];

                    if (strNormal != "")
                    {
                        strVal = strNormal;
                        break;
                    }

                    if (strSex == "" || strSex == astrSex)
                    {
                        if (strAgeFrom != "" && strAgeTo != "")
                        {
                            if (VB.Val(strAgeFrom) <= VB.Val(astrAge) && VB.Val(astrAge) <= VB.Val(strAgeTo))
                            {
                                strVal = strRefValFrom + " ~ " + strRefValTo;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            string strItem = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, 0].Text.Trim();
            string strValue = ssView.ActiveSheet.Cells[e.Row, 2].Text.Trim();

            switch (strItem)
            {
                case "Hb":
                    txtHb.Text = strValue;                    
                    break;
                case "Hct":
                    txtHct.Text = strValue;
                    break;
                case "PLT":
                    txtPLT.Text = strValue;
                    break;
                case "WBC":
                    txtWBC.Text = strValue;
                    break;
                case "Na":
                    txtNa.Text = strValue;
                    break;
                case "K":
                    txtK.Text = strValue;
                    break;
                case "GOT":
                    txtGOT.Text = strValue;
                    break;
                case "GPT":
                    txtGPT.Text = strValue;
                    break;
                case "PT":
                    txtPT.Text = strValue;
                    break;
                case "PTT":
                    txtPTT.Text = strValue;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            rGetPatientInfo(txtHb.Text, txtHct.Text, txtPLT.Text, txtWBC.Text, txtNa.Text, txtK.Text, txtGOT.Text, txtGPT.Text, txtPT.Text, txtPTT.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = "";
        }
    }
}
