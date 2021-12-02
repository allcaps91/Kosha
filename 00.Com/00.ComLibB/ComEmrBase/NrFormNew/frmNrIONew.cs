using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNrIONew : Form
    {
        EmrForm pForm = null;
        EmrPatient p = null;

        #region 폼에 사용하는 변수를 코딩하는 부분
        /// <summary>
        ///    //기록지 작성방향(H: 옆으로, V:아래로)
        /// </summary>
        private const string mDirection = "H";
        /// <summary>
        /// //해드 칼럼 수
        /// </summary>
        private const int mintTCol = 5;
        ////해드 로우 수
        private const int mintTRow = 4;
        /// <summary>
        /// //밑줄
        /// </summary>
        private const int mintBRow = 3;
        /// <summary>
        /// //밑줄
        /// </summary>
        private const int mintColW_I = 120;
        /// <summary>
        ///   //밑줄
        /// </summary>
        private const int mintColW_V = 70;

        /// <summary>
        /// //JOBGB Colume
        /// </summary>
        private const int COL_JOB = 0;
        /// <summary>
        /// //ITEM Colume
        /// </summary>
        private const int COL_ITEM = 1;
        /// <summary>
        /// //확장 Colume
        /// </summary>
        private const int COL_EXP = 2;
        /// <summary>
        /// //그룹 이름 Colume
        /// </summary>
        private const int COL_GNAME = 3;
        /// <summary>
        /// //아이템 이름 Colume
        /// </summary>
        private const int COL_INAME = 4;
        /// <summary>
        /// //DATA 시작 Colume
        /// </summary>
        private const int COL_DATAS = 5;

        public string mstrFormNameGb = "기록지관리";
        public string mstrFormNameWard = "임상관찰";
        /// <summary>
        /// IVT : 혈액학적 ssVital_Sheet1, IIO : 섭취배셜 ssVital_Sheet1
        /// </summary>
        //private string mJOBGB = "IVT";

        //private string mstrNightTimeAm = "00:00/07:00";
        //private string mstrDayTime = "07:01/15:00";
        //private string mstrEveTime = "15:01/23:00";
        //private string mstrNightTime = "23:01/24:00";

        private Font BoldFont = null;

        //FarPoint.Win.Spread.FpSpread mSpd;
        //FarPoint.Win.Spread.SheetView mSpdView;

        //private frmNrIcuTimeSet frmNrIcuTimeSetX = null;

        //ContextMenu PopupMenu = null;
        //int mPopRow = 0;
        //int mPopCol = 0;

        //private frmEmrChartNew frmEmrChartX = null;
        //private frmOcrPrint frmOcrPrintX = null;

     
        /// <summary>
        /// 쿼리 두번 도는거 방지
        /// </summary>
        //bool isFormLoad = false;
        #endregion

        public frmNrIONew(EmrPatient pAcp)
        {
            p = pAcp;
            InitializeComponent();
        }

        private void frmNrIONew_Load(object sender, EventArgs e)
        {
            if(p == null)
            {
                Close();
            }

            BoldFont = new Font("굴림", 10, FontStyle.Bold);

            ssPat_Sheet1.Cells[0, 0].Text = p.ptNo;
            ssPat_Sheet1.Cells[0, 1].Text = p.ptName;

            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3150");

            GetIoTot();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetIoTot();
        }

        #region IO합계함수
        private void GetIoTot()
        {
            SetIoTotDefault();

            LoadIoTot("Day", "050100", "130000");
            LoadIoTot("Eve", "130100", "210000");
            LoadIoTot("Night", "210100", "050000");
            LoadIoTot("Tot", "050100", "050000");
            //LoadIoTot("Tot", "070000", "065900");
        }

        /// <summary>
        /// IO 합계를 구한다
        /// </summary>
        /// <param name="strDuty"></param>
        /// <param name="strSTime"></param>
        /// <param name="strETime"></param>
        private void LoadIoTot(string strDuty, string strSTime, string strETime)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int j = 0;
            int k = 0;

            ssIoTot_Sheet1.Columns.Count = ssIoTot_Sheet1.Columns.Count + 1;
            j = ssIoTot_Sheet1.Columns.Count - 1;
            ssIoTot_Sheet1.Columns[j].Width = 40;

            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 0, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 1, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 2, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(ssIoTot_Sheet1, 3, j, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            ssIoTot_Sheet1.Cells[0, j, 3, j].Font = new Font("굴림", 10, FontStyle.Bold);
            ssIoTot_Sheet1.Cells[0, j, 3, j].BackColor = Color.LightBlue;
            ssIoTot_Sheet1.Cells[2, j].Text = strDuty;
            ssIoTot_Sheet1.AddSpanCell(2, j, 2, 1);

            Cursor.Current = Cursors.WaitCursor;
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    B.ITEMCD,  ";
            SQL = SQL + ComNum.VBLF + "    SUM(DECODE(B.ITEMVALUE, NULL, 0, B.ITEMVALUE)) AS ITEMVALUE , MAX(BC.BASNAME) AS ITEMNAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRBVITALTIME TM ";
            SQL = SQL + ComNum.VBLF + "    ON  A.ACPNO = TM.ACPNO ";
            SQL = SQL + ComNum.VBLF + "    AND TM.FORMNO = " + pForm.FmFORMNO;
            SQL = SQL + ComNum.VBLF + "    AND TM.SUBGB = '0' ";
            SQL = SQL + ComNum.VBLF + "    AND (A.CHARTDATE || A.CHARTTIME )= (TM.CHARTDATE || TM.TIMEVALUE || '00') ";
            if ((strDuty == "Tot") || (strDuty == "Night"))
            {
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00' >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((TM.CHARTDATE || TM.TIMEVALUE ), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
            }
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO ";
            SQL = SQL + ComNum.VBLF +@"   AND REGEXP_LIKE(B.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
            SQL = SQL + ComNum.VBLF + "   ON BC.BASCD = B.ITEMCD  ";
            SQL = SQL + ComNum.VBLF + "   AND BC.BSNSCLS = '" + mstrFormNameGb + "'  ";
            SQL = SQL + ComNum.VBLF + "   AND BC.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "   AND BC.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + pForm.FmFORMNO;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + pForm.FmUPDATENO;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTUSEID <> '합계' ";

            if ((strDuty == "Tot") || (strDuty == "Night"))
            {
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.AddDays(+1).ToString("yyyyMMdd") + strETime + "' ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strSTime + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR((A.CHARTDATE || A.CHARTTIME), 1, 12) || '00'  <= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + strETime + "' ";
            }

            SQL = SQL + ComNum.VBLF + "GROUP BY  B.ITEMCD ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (ssIoTot_Sheet1.Cells[3, j].Text.Trim() == "합계")
                {
                    continue;
                }

                for (k = 4; k < ssIoTot_Sheet1.RowCount; k++)
                {
                    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssIoTot_Sheet1.Cells[k, COL_ITEM].Text.Trim())
                    {
                        if ((dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000030623"))
                        {
                            ssIoTot_Sheet1.Cells[k, j].BackColor = Color.LightCyan;
                            if (strDuty == "Tot")
                            {
                                ssIoTot_Sheet1.Cells[k, j].Font = BoldFont;
                            }
                        }
                        //if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) == 0)
                        //{
                        //    ssIoTot_Sheet1.Cells[k, j].Text = "";
                        //}
                        //else
                        //{
                            ssIoTot_Sheet1.Cells[k, j].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        //}
                    }
                }
                if (i >= dt.Rows.Count) break;
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// IO 함계 스프래드 세팅
        /// </summary>
        private void SetIoTotDefault()
        {
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SetTopRow(ssIoTot_Sheet1);

            string strBASEXNAME = "";
            int intS = 0;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설'";
            SQL = SQL + ComNum.VBLF + "    AND B.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '섭취배설그룹'";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + pForm.FmFORMNO;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB = 'IIO'";
            //SQL = SQL + ComNum.VBLF + "ORDER BY B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssIoTot_Sheet1.RowCount += 1;

                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", true);
                    clsSpread.SetTypeAndValue(ssIoTot_Sheet1, ssIoTot_Sheet1.RowCount - 1, 4, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_L, "", false);

                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_JOB].Text = "IIO";

                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_ITEM].Text = dt.Rows[i]["BASCD"].ToString().Trim();

                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssIoTot_Sheet1.AddSpanCell(intS, 3, ssIoTot_Sheet1.RowCount - 1 - intS, 1);
                        }
                        intS = ssIoTot_Sheet1.RowCount - 1;
                    }


                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                    if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].BackColor = Color.LightCyan;
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].BackColor = Color.LightCyan;
                    }

                    ssIoTot_Sheet1.SetRowHeight(ssIoTot_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                }
                dt.Dispose();
                dt = null;

                ssIoTot_Sheet1.AddSpanCell(intS, 3, ssIoTot_Sheet1.RowCount - intS, 1);

                //SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, strJOBGB);
            }
            else
            {
                dt.Dispose();
                dt = null;
                //SetButtonRow(ssIoTot_Sheet1);
                //SetTimeSet(ssIoTot_Sheet1, strJOBGB);
            }

            ssIoTot_Sheet1.Columns[2].Visible = false;
            //ssIoTot_Sheet1.Columns[2].Visible = false;
        }
        /// <summary>
        /// 스프래드 기초 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void InitSpdSet(FarPoint.Win.Spread.SheetView SpdView)
        {
            SpdView.RowCount = 0;
            SpdView.ColumnCount = 0;

            SpdView.RowCount = mintTRow;
            SpdView.ColumnCount = mintTCol;
            SpdView.SetRowHeight(-1, ComNum.SPDROWHT);
            //SpdView.SetColumnWidth(-1, mintColW_I);
            SpdView.SetColumnWidth(COL_JOB, 60);
            SpdView.SetColumnWidth(COL_ITEM, 60);
            SpdView.SetColumnWidth(COL_EXP, 18);
            SpdView.SetColumnWidth(COL_GNAME, 80);
            SpdView.SetColumnWidth(COL_INAME, 160);

            SpdView.Columns[COL_JOB].Visible = false;
            SpdView.Columns[COL_ITEM].Visible = false;

            SpdView.Rows[0].Visible = false; //작성일자
            SpdView.Rows[1].Visible = false; //Duty
            SpdView.Rows[3].Visible = true; //작성자
        }

        /// <summary>
        /// 해드 값 세팅
        /// </summary>
        /// <param name="SpdView"></param>
        private void SetTopRow(FarPoint.Win.Spread.SheetView SpdView)
        {
            InitSpdSet(SpdView);

            clsSpread.SetTypeAndValue(SpdView, 0, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 2, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);
            clsSpread.SetTypeAndValue(SpdView, 3, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "", false);

            clsSpread.SetTypeAndValue(SpdView, 0, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성일자", false);
            SpdView.Cells[0, 0, 0, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(0, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "Duty", false);
            SpdView.Cells[1, 0, 1, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(1, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 2, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "작성시간", false);
            SpdView.Cells[2, 0, 2, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(2, 3, 1, 2);

            clsSpread.SetTypeAndValue(SpdView, 3, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, "구   분", false);
            SpdView.Cells[3, 0, 3, 4].BackColor = Color.LightBlue;
            SpdView.AddSpanCell(3, 3, 1, 2);
        }



        #endregion IO 함수

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mbtnBeforeTot_Click(object sender, EventArgs e)
        {
            dtpFrDateTot.Value = dtpFrDateTot.Value.AddDays(-1);
        }

        private void mbtnNextTot_Click(object sender, EventArgs e)
        {
            dtpFrDateTot.Value = dtpFrDateTot.Value.AddDays(1);
        }

        private void dtpFrDateTot_ValueChanged(object sender, EventArgs e)
        {
            GetIoTot();
        }

        private void btnSearchTot_Click(object sender, EventArgs e)
        {
            GetIoTot();
        }
    }
}
