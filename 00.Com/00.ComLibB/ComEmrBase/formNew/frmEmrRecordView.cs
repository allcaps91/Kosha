using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread.CellType;

namespace ComEmrBase
{
    public partial class frmEmrRecordView : Form
    {
        EmrPatient AcpEmr = null;
        Dictionary<string, int> keyItems = null;

        public frmEmrRecordView(EmrPatient p)
        {
            AcpEmr = p;
            InitializeComponent();
        }

        private void frmEmrRecordView_Load(object sender, EventArgs e)
        {
            ssList1_Sheet1.RowCount = 0;
            keyItems = new Dictionary<string, int>();
        }


        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (cboGBN.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "조회할 항목을 선택해주세요.");
                return;
            }
            SetItem();
            GetChartList();
        }

        /// <summary>
        /// 인공호흡기, CRRT 아이템 설정
        /// </summary>
        private void SetItem()
        {
            keyItems.Clear();
            ssList1_Sheet1.ColumnCount = 2;
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT BASCD, BASNAME, NFLAG1, NFLAG2, NFLAG3, DISSEQNO";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "  AND UNITCLS = '특수치료'";
            SQL = SQL + ComNum.VBLF + "  AND BASEXNAME = '" + cboGBN.Text + "'";

            SQL = SQL + ComNum.VBLF + "UNION ALL";
            SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, NFLAG1, NFLAG2, NFLAG3, DISSEQNO";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "  AND UNITCLS = '임상관찰'";
            SQL = SQL + ComNum.VBLF + "  AND BASEXNAME = '" + cboGBN.Text + "'";

            SQL = SQL + ComNum.VBLF + "ORDER BY NFLAG1, NFLAG2, NFLAG3, DISSEQNO";

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
                ssList1_Sheet1.ColumnCount += dt.Rows.Count;
                TextCellType cellType = new TextCellType();
                cellType.Multiline = true;
                cellType.WordWrap = true;
                cellType.MaxLength = 32000;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList1_Sheet1.Columns[i + 2].Label = dt.Rows[i]["BASNAME"].ToString();
                    ssList1_Sheet1.Columns[i + 2].CellType = cellType;
                    ssList1_Sheet1.Columns[i + 2].Width = 80;

                    keyItems.Add(dt.Rows[i]["BASCD"].ToString(), i + 2);
                }
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 인공호흡기, CRRT 아이템 설정
        /// </summary>
        private void GetChartList()
        {
            ssList1_Sheet1.RowCount = 0;

            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT CHARTDATE, CHARTTIME, R.ITEMCD, R.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
            SQL = SQL + ComNum.VBLF + "     ON A.EMRNO    = R.EMRNO";
            SQL = SQL + ComNum.VBLF + "    AND A.EMRNOHIS = R.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMVALUE IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "    AND EXISTS";
            SQL = SQL + ComNum.VBLF + "    (";
            SQL = SQL + ComNum.VBLF + "         SELECT 1";
            SQL = SQL + ComNum.VBLF + "           FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "          WHERE B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "            AND B.UNITCLS = '특수치료'";
            SQL = SQL + ComNum.VBLF + "            AND B.BASEXNAME = '" + cboGBN.Text + "'";
            SQL = SQL + ComNum.VBLF + "            AND B.BASCD = R.ITEMCD";
            SQL = SQL + ComNum.VBLF + "         UNION ALL";
            SQL = SQL + ComNum.VBLF + "         SELECT 1";
            SQL = SQL + ComNum.VBLF + "           FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "          WHERE BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "            AND UNITCLS = '임상관찰'";
            SQL = SQL + ComNum.VBLF + "            AND BASEXNAME = '" + cboGBN.Text + "'";
            SQL = SQL + ComNum.VBLF + "            AND B.BASCD = R.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    )";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = 3150";
            SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND CHARTDATE >= '" + dtpSDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND CHARTDATE <= '" + dtpEDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME)";

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
                //ssList1_Sheet1.RowCount = dt.Rows.Count;

                string ChartDateTime = string.Empty;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ChartDateTime.Equals(dt.Rows[i]["CHARTDATE"].ToString() + dt.Rows[i]["CHARTTIME"].ToString()) == false)
                    {
                        ssList1_Sheet1.RowCount += 1;
                        ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["CHARTDATE"].To<int>().ToString("0000-00-00");
                        ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["CHARTTIME"].ToString().Substring(0, 4).To<int>().ToString("00:00");
                        ChartDateTime = dt.Rows[i]["CHARTDATE"].ToString() + dt.Rows[i]["CHARTTIME"].ToString();
                    }

                    int intCol = -1;
                    if (keyItems.TryGetValue(dt.Rows[i]["ITEMCD"].ToString(), out intCol))
                    {
                        if (intCol > -1)
                        {
                            ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, intCol].Text = dt.Rows[i]["ITEMVALUE"].ToString();
                            if (ssList1_Sheet1.Rows[ssList1_Sheet1.RowCount - 1].GetPreferredHeight() >  ComNum.SPDROWHT)
                            {
                                ssList1_Sheet1.Rows[ssList1_Sheet1.RowCount - 1].Height = ssList1_Sheet1.Rows[ssList1_Sheet1.RowCount - 1].GetPreferredHeight() + 16;
                            }
                        }
                    }

                }

                ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            }
            dt.Dispose();
            dt = null;
        }

    }
}
