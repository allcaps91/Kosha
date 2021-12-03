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
    /// File Name       : frmERXray.cs
    /// Description     : 응급실 내원자 중 뇌영상 오더 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-05
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\nurse\nrQI\FrmERXray.frm(FrmERXray) => frmERXray.cs 으로 변경
    /// UNION 하는 쿼리부분(region부분 쿼리 확인 및 수정 필요, vb에서 테스트해도 쿼리 오류 발생)
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\nurse\nrQI\FrmERXray.frm(FrmERXray)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\nurse\nrQI\nrQI.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmERXray : Form
    {
        string SUCODE_CT = "'HA451','HA461','HA471A','HA471B','HA471AV','HA471C','HA471CV','HA471D','HA471E'";
        string SUCODE_MRI0 = "'HE101','HE101A','HE201','HE201A','HE235','HE235B','HE235D','HE535B','HE535C','HE535D','HE535E','HE535F'";
        string SUCODE_MRI1 = "'MR50','MR50A','MR50B','MR50C','MR50D','MR51','MR51A','MR51C','MR51CT','MR51D','MR51DC','MR511','MR511C','MR512','MR513','MR513A','MR513C','MR513D','MR513E','MR514'";

        clsSpread spd = new clsSpread();
        ComFunc CF = new ComFunc();

        string mstrIpAddress;
        string mstrJobSabun;
        string mstrJobPart;

        public frmERXray()
        {
            InitializeComponent();
        }

        public frmERXray(string GstrIpAddress, string GstrJobSabun, string GstrJobPart)
        {
            mstrIpAddress = GstrIpAddress;
            mstrJobSabun = GstrJobSabun;
            mstrJobPart = GstrJobPart;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {

        }

        void frmERXray_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            CF.FormInfo_History(clsDB.DbCon, this.Name, this.Text, mstrIpAddress, mstrJobSabun, mstrJobPart);

            CboSet();

            GetData();
        }

        void CboSet()
        {
            cboXray.Items.Clear();
            cboXray.Items.Add("0.전체");
            cboXray.Items.Add("1.Brain MRI(급여)");
            cboXray.Items.Add("2.Brain MRI(비급여)");
            cboXray.Items.Add("3.CT");
            cboXray.SelectedIndex = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string strPano = "";
            string strInDate = "";
            string strInTime = "";

            string strPANO_Old = "";
            string strINDATE_Old = "";

            string strGUBUN = ""; //퇴원구분
            string strDIAG = "";  //상병
            string strXrayT = ""; //영상기록일시
            string strSeekT = ""; //영상촬영일시

            int nStartRow = 0;        //merge용
            //int nEndRow = 0;          //merge용

            string strOK = "";    //대상자 여부

            spd.Spread_Clear(ssList, ssList_Sheet1.RowCount, ssList_Sheet1.ColumnCount);

            ssList_Sheet1.RowCount = 0;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  A.PANO, D.SNAME, A.AGE, A.SEX, A.INTIME, B.SUCODE, A.BALDATE, ";
                SQL += ComNum.VBLF + "  C.SEEKDATE, C.XSENDDATE, SUM(B.QTY * B.NAL) CNT ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT A, " + ComNum.DB_MED + "OCS_IORDER B, " + ComNum.DB_PMPA + "XRAY_DETAIL C, " + ComNum.DB_PMPA + "BAS_PATIENT D";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND A.PANO = B.PTNO";
                SQL += ComNum.VBLF + "  AND B.PTNO = C.PANO(+)";
                SQL += ComNum.VBLF + "  AND B.ORDERNO = C.ORDERNO(+)";
                SQL += ComNum.VBLF + "  AND A.PANO = D.PANO";
                if (String.Compare(dtpFDate.Text, "2009-08-04") > 0)
                {
                    SQL += ComNum.VBLF + "  AND B.GBIOE IN ('E','EI')";
                }
                SQL += ComNum.VBLF + "  AND B.BDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND B.BDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND A.JDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND A.JDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";
                switch (VB.Left(cboXray.Text, 1))
                {
                    case "0":
                        SQL += ComNum.VBLF + "  AND B.SUCODE IN (" + SUCODE_CT + "," + SUCODE_MRI0 + "," + SUCODE_MRI1 + ") ";
                        break;
                    case "1":
                        SQL += ComNum.VBLF + "  AND B.SUCODE IN (" + SUCODE_MRI0 + ") ";
                        break;
                    case "2":
                        SQL += ComNum.VBLF + "  AND B.SUCODE IN (" + SUCODE_MRI1 + ")";
                        break;
                    case "3":
                        SQL += ComNum.VBLF + "  AND B.SUCODE IN (" + SUCODE_CT + ") ";
                        break;
                }
                SQL += ComNum.VBLF + "GROUP BY A.PANO, D.SNAME, A.AGE, A.SEX, A.INTIME, B.SUCODE, A.BALDATE,";
                SQL += ComNum.VBLF + "C.SEEKDATE, C.XSENDDATE ";
                SQL += ComNum.VBLF + "HAVING SUM(B.QTY * B.NAL) > 0";
                SQL += ComNum.VBLF + "ORDER BY A.INTIME, A.PANO, C.SEEKDATE ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = dt.Rows.Count;

                ssList_Sheet1.RowCount = nREAD;

                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strOK = "NO";
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    //strInDate = VB.Replace(VB.Left(ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM"), 10), "-", "");
                    strInDate = VB.Replace(VB.Left(String.Format(dt.Rows[i]["INTIME"].ToString().Trim(), "{0:####-##-## ##:##}"), 10), "-", "");

                    //strInTime = VB.Replace(VB.Right(ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM"), 5), "-", "");
                    strInTime = VB.Replace(VB.Right(String.Format(dt.Rows[i]["INTIME"].ToString().Trim(), "{0:####-##-## ##:##}"), 5), ":", "");

                    //GoSub VERIFY_BRAIN
                    #region -> VERIFY_BRAIN UNION하는 부분 쿼리 오류발생 확인 및 수정 필요(ORA-01789 : 질의 블록은 부정확한 수의 결과 열을 가지고 있습니다.)
                    strOK = "OK";

                    if(CF.DATE_TIME(clsDB.DbCon, ComFunc.vbFormat(dt.Rows[i]["BALDATE"].ToString().Trim(), "YYYY-MM-DD HH:MM"), ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM")) > 180)
                    {
                        strOK = "NO";
                        return;
                    }

                    SqlErr = "";
                    
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  DGDCIDNO PANO, DGDCINDT INDATE, DGDCINTM INTIME, DGDCDIAG DIAG, 'I' GUBUN ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHDGDC";
                    SQL += ComNum.VBLF + "WHERE ((DGDCDIAG >= 'I63'   AND DGDCDIAG <= 'I639')  OR  DGDCDIAG = 'I63')";
                    SQL += ComNum.VBLF + "  AND DGDCIDNO = '" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND DGDCINDT = '" + strInDate + "' ";
                    SQL += ComNum.VBLF + "  AND DGDCINTM = '" + strInTime + "' ";
                    SQL += ComNum.VBLF + "UNION ALL ";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  DGOTIDNO PANO, DGOTINDT INDATE, DGOTINTM INTIME, DGOTDIAG DIAG, 'O' GUBUN ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHDGOT";
                    SQL += ComNum.VBLF + "WHERE ((DGOTDIAG >= 'I63'   AND DGOTDIAG <= 'I639')  OR  DGOTDIAG = 'I63') ";
                    SQL += ComNum.VBLF + "  AND DGOTIDNO = '" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND DGOTINDT = '" + strInDate + "' ";
                    SQL += ComNum.VBLF + "  AND DGOTINTM = '" + strInTime + "'";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows.Count == 0)
                    {
                        strOK = "NO";
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }
                    else
                    {
                        strGUBUN = dt2.Rows[0]["GUBUN"].ToString().Trim();
                        strDIAG = dt2.Rows[0]["DIAG"].ToString().Trim();
                    }

                    SqlErr = "";
                    dt2.Dispose();
                    dt2 = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  PTMIHIBP, PTMIEMRT, PTMIINRT, PTMINAME, PTMIAKDT, PTMIAKTM, PTMISTAT ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI";
                    SQL += ComNum.VBLF + "WHERE PTMIIDNO = '" + strPano + "'          ";
                    SQL += ComNum.VBLF + "  AND PTMIINDT = '" + strInDate + "'  ";
                    SQL += ComNum.VBLF + "  AND PTMIINTM = '" + strInTime + "'    ";
                    SQL += ComNum.VBLF + "  AND PTMIHIBP >= 90   ";
                    SQL += ComNum.VBLF + "  AND SUBSTR(PTMIEMRT, 1, 1) IN ('2','3')    ";
                    SQL += ComNum.VBLF + "  AND PTMIINRT = '1' ";
                    SQL += ComNum.VBLF + "ORDER BY SEQNO DESC ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows.Count == 0)
                    {
                        strOK = "NO";
                        dt2.Dispose();
                        dt2 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }
                    else
                    {
                        if (dt2.Rows[0]["PTMISTAT"].ToString().Trim() == "D")
                        {
                            strOK = "NO";
                            dt2.Dispose();
                            dt2 = null;
                            return;
                        }
                    }
                    SqlErr = "";
                    dt2.Dispose();
                    dt2 = null;
                    #endregion

                    if (chkBrain.Checked == true || (strOK == "OK" && chkBrain.Checked == true))
                    {
                        ssList_Sheet1.RowCount = j + 1;
                        j = j + 1;

                        //LabelCnt.Caption = j & "건 조회됨"

                        ssList_Sheet1.Cells[j + 1, 0].Text = strPano;
                        ssList_Sheet1.Cells[j + 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[j + 1, 2].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[j + 1, 3].Text = ComFunc.vbFormat(dt.Rows[i]["BALDATE"].ToString().Trim(), "YYYY-MM-DD HH:MM");
                        ssList_Sheet1.Cells[j + 1, 4].Text = ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM");
                        ssList_Sheet1.Cells[j + 1, 6].Text = dt.Rows[i]["SUCODE"].ToString().Trim();

                        ssList_Sheet1.Cells[j + 1, 8].Text = ComFunc.vbFormat(dt.Rows[i]["SEEKDATE"].ToString().Trim(), "YYYY-MM-DD HH:MM");
                        strSeekT = ssList_Sheet1.Cells[j + 1, 8].Text;

                        ssList_Sheet1.Cells[j + 1, 9].Text = ComFunc.vbFormat(dt.Rows[i]["XSENDDATE"].ToString().Trim(), "YYYY-MM-DD HH:MM");
                        strXrayT = ssList_Sheet1.Cells[j + 1, 9].Text;

                        
                        ssList_Sheet1.Cells[j + 1, 12].Text = Convert.ToString(CF.DATE_TIME(clsDB.DbCon, ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM"), strSeekT));
                        ssList_Sheet1.Cells[j + 1, 13].Text = Convert.ToString(CF.DATE_TIME(clsDB.DbCon, ComFunc.vbFormat(dt.Rows[i]["INTIME"].ToString().Trim(), "YYYY-MM-DD HH:MM"), strXrayT));

                        if (strOK == "OK" && chkBrain.Checked == true)
                        {
                            ssList_Sheet1.Cells[j + 1, 10].Text = strGUBUN;
                            ssList_Sheet1.Cells[j + 1, 11].Text = strDIAG;
                        }

                        if (strPano == strPANO_Old && strInDate == strINDATE_Old && j != 1)
                        {
                            ssList.Sheets[0].AddSpanCell(0, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(1, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(2, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(3, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(4, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(10, nStartRow, 1, j);
                            ssList.Sheets[0].AddSpanCell(11, nStartRow, 1, j);
                        }
                        else
                        {
                            strPANO_Old = strPano;
                            strINDATE_Old = strInDate;
                            nStartRow = j;
                            k = k + 1;

                            ssList_Sheet1.Cells[j, 0].Text = Convert.ToString(k);
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }


        }

        
    }
}
