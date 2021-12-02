using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmWorkListAntiCancer : Form, MainFormMessage
    {
        
        
        #region //MainFormMessage
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

        string fstrPANO = "";
        string fstrIO = "";

        public frmWorkListAntiCancer()
        {
            InitializeComponent();
            setEvent();
        }

        public frmWorkListAntiCancer(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            setEvent();
        }

        public frmWorkListAntiCancer(string strPANO, string strIO)
        {
            InitializeComponent();
            fstrPANO = strPANO;
            fstrIO = strIO;
            setEvent();

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

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            SCREEN_CLEAR();

            TxtSDATE.Text = strSysDate;

            btnSearch.PerformClick();


        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                
                eGetData(fstrPANO, TxtSDATE.Text);             
            }
            else if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        private void eGetData(string strPANO, string strBDATE)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;
            int nRead = 0;

            int nRow = 0;
            //string strBUN = "";
            //string strPO_START = ""; //BUN = 11인 약제 시작 시. 최소 1회

            SCREEN_CLEAR();
            FarPoint.Win.ComplexBorder complexBorder_TOP = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOP2 = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOPLEFT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOPRIGHT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            FarPoint.Win.ComplexBorder complexBorder_MIDLEFT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_MID = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_MIDRIGHT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            FarPoint.Win.ComplexBorder complexBorder_BOTTOM = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            if (fstrIO == "O")
            {
                SET_HEADER_O(strPANO, strBDATE);
                
            }
            else
            {
                SET_HEADER_I(strPANO, strBDATE);
            }

            SQL = "  SELECT PTNO, BDATE, BUN, TUYEOPOINT, GBGROUP, ORDERCODE, SUCODE, REMARK, ";
            SQL += ComNum.VBLF + " REALQTY, BCONTENTS, GBDIV, TUYEOTIME, ORDERNAME, DOSNAMEK, CONTENTS_TIMES, QTY_TIMES ";
            SQL += ComNum.VBLF + " FROM ( ";
            SQL += ComNum.VBLF + "   SELECT PTNO,         BDATE,         BUN,         TUYEOPOINT, ";
            SQL += ComNum.VBLF + "         GBGROUP,      ORDERCODE,     SUCODE,      REMARK, ";
            SQL += ComNum.VBLF + "         REALQTY,      BCONTENTS,     GBDIV,       TUYEOTIME, ";
            SQL += ComNum.VBLF + "         (SELECT SUNAMEK FROM KOSMOS_PMPA.BAS_SUN WHERE SUNEXT = A.SUCODE AND ROWNUM = 1) ORDERNAME, ";
            SQL += ComNum.VBLF + "         (SELECT DOSNAME FROM KOSMOS_OCS.OCS_ODOSAGE WHERE DOSCODE = A.DOSCODE) DOSNAMEK, ";
            SQL += ComNum.VBLF + "         CASE ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV = 1 THEN A.CONTENTS  ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV > 1 AND A.CONTENTS > 0 THEN A.CONTENTS / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "            ELSE 0 ";
            SQL += ComNum.VBLF + "         END CONTENTS_TIMES, ";
            SQL += ComNum.VBLF + "         CASE ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV = 1 THEN TO_NUMBER(A.REALQTY) ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV > 1 AND A.CONTENTS = 0 THEN TO_NUMBER (A.REALQTY) / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "            ELSE ( (A.CONTENTS / A.BCONTENTS) * TO_NUMBER (A.REALQTY)) / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "         END QTY_TIMES, QTY, NAL ";
            if (fstrIO == "O")
            {
                SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_OORDER A ";
            }
            else
            {
                SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_IORDER A ";
            }
            SQL += ComNum.VBLF + "   WHERE     BDATE = TO_DATE ('" + strBDATE + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "         AND PTNO = '" + strPANO + "' ";
            SQL += ComNum.VBLF + "         AND TUYEOPOINT IS NOT NULL ";
            if (fstrIO == "I")
            {
                SQL += ComNum.VBLF + "       AND (NURSEID = ' ' OR NURSEID IS NULL) ";
            }
            SQL += ComNum.VBLF + " )";
            SQL += ComNum.VBLF + "    GROUP BY PTNO, BDATE, BUN, TUYEOPOINT, GBGROUP, ORDERCODE, SUCODE, REMARK, ";
            SQL += ComNum.VBLF + "             REALQTY, BCONTENTS, GBDIV, TUYEOTIME, ORDERNAME, DOSNAMEK, CONTENTS_TIMES, QTY_TIMES ";
            SQL += ComNum.VBLF + "    HAVING SUM(QTY * NAL) > 0 ";
            SQL += ComNum.VBLF + "  ORDER BY PTNO, BDATE, TO_NUMBER(TUYEOPOINT), GBGROUP ASC, DECODE (BUN,  '20', 1,  '11', 2,  3), TUYEOTIME, ORDERNAME ";
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

            if (dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;
                ssS_Sheet1.RowCount = 8;
                nRow = ssS_Sheet1.RowCount;

                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    ssS_Sheet1.RowCount = nRow;
                    ssS_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim() + "hr";
                    ssS_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 1].Text == "") ssS_Sheet1.Cells[nRow - 1, 1].Text = "-";
                    ssS_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["CONTENTS_TIMES"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["QTY_TIMES"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 6].Text = "#" + dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 7].Text == "") ssS_Sheet1.Cells[nRow - 1, 7].Text = "-";
                    ssS_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["DOSNAMEK"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 9].Text == "") ssS_Sheet1.Cells[nRow - 1, 9].Text = "-";
                    if (dt.Rows[i]["BUN"].ToString().Trim() == "11")
                    {
                        ssS_Sheet1.Rows[nRow - 1].BackColor = Color.LightGray;
                    }
                    else
                    {
                        ssS_Sheet1.Rows[nRow - 1].BackColor = Color.White;
                    }

                    ssS_Sheet1.Rows[nRow - 1].Height = ssS_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 15;
                    for (j = 0; j < 10; j++)
                    {
                        if (j == 0)
                        {
                            this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MIDLEFT;
                        }
                        else if (j == 9)
                        {
                            this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MIDRIGHT;
                        }
                        else
                        {
                            this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MID;
                        }
                    }

                }
                ssS_Sheet1.RowCount += 1;
                for (i = 0; i < 10; i++)
                {
                    ssS_Sheet1.Cells[ssS_Sheet1.RowCount - 1, i].Text = "ㅤ";
                    this.ssS_Sheet1.Cells.Get(ssS_Sheet1.RowCount - 1, i).Border = complexBorder_TOP;
                }
                ssS_Sheet1.Rows[ssS_Sheet1.RowCount - 1].Visible = true;

                ssS_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);

                CELL_MERGE();
            }
        }

        private void eGetData_OLD( string strPANO, string strBDATE)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;
            int nRead = 0;

            int nRow = 0;
            string strBUN = "";
            string strPO_START = ""; //BUN = 11인 약제 시작 시. 최소 1회

            SCREEN_CLEAR();
            FarPoint.Win.ComplexBorder complexBorder_TOP = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOP2 = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOPLEFT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_TOPRIGHT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            FarPoint.Win.ComplexBorder complexBorder_MIDLEFT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_MID = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder complexBorder_MIDRIGHT = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            FarPoint.Win.ComplexBorder complexBorder_BOTTOM = new FarPoint.Win.ComplexBorder(
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine),
               new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            SET_HEADER_I(strPANO, strBDATE);

            SQL = "   SELECT PTNO,         BDATE,         BUN,         TUYEOPOINT, ";
            SQL += ComNum.VBLF + "         GBGROUP,      ORDERCODE,     SUCODE,      REMARK, ";
            SQL += ComNum.VBLF + "         REALQTY,      BCONTENTS,     GBDIV,       TUYEOTIME, ";
            SQL += ComNum.VBLF + "         (SELECT SUNAMEK FROM KOSMOS_PMPA.BAS_SUN WHERE SUNEXT = A.SUCODE AND ROWNUM = 1) ORDERNAME, ";
            SQL += ComNum.VBLF + "         (SELECT DOSNAME FROM KOSMOS_OCS.OCS_ODOSAGE WHERE DOSCODE = A.DOSCODE) DOSNAMEK, ";
            SQL += ComNum.VBLF + "         CASE ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV = 1 THEN A.CONTENTS  ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV > 1 AND A.CONTENTS > 0 THEN A.CONTENTS / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "            ELSE 0 ";
            SQL += ComNum.VBLF + "         END CONTENTS_TIMES, ";
            SQL += ComNum.VBLF + "         CASE ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV = 1 THEN TO_NUMBER(A.REALQTY) ";
            SQL += ComNum.VBLF + "            WHEN A.GBDIV > 1 AND A.CONTENTS = 0 THEN TO_NUMBER (A.REALQTY) / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "            ELSE ( (A.CONTENTS / A.BCONTENTS) * TO_NUMBER (A.REALQTY)) / TO_NUMBER (GBDIV) ";
            SQL += ComNum.VBLF + "         END QTY_TIMES ";
            SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_IORDER A ";
            SQL += ComNum.VBLF + "   WHERE     BDATE = TO_DATE ('" + strBDATE + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "         AND PTNO = '" + strPANO + "' ";
            SQL += ComNum.VBLF + "         AND TUYEOPOINT IS NOT NULL ";
            SQL += ComNum.VBLF + "  ORDER BY PTNO, BDATE, DECODE (BUN,  '20', 1,  '11', 2,  3), TO_NUMBER(TUYEOPOINT), GBGROUP ASC, TUYEOTIME, ORDERNAME ";
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

            if (dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;
                ssS_Sheet1.RowCount = 8;
                nRow = ssS_Sheet1.RowCount;

                for (i = 0; i < nRead; i++)
                {

                    strBUN = dt.Rows[i]["BUN"].ToString().Trim();

                    if (strBUN == "11" && strPO_START == "")
                    {
                        strPO_START = "OK";
                    }

                    if (strPO_START == "OK")
                    {
                        nRow += 1;
                        ssS_Sheet1.RowCount = nRow;
                        for (j = 0; j < 10; j++)
                        {
                            ssS_Sheet1.Cells[nRow - 1, j].Text = "ㅤ";
                            this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_TOP;
                        }
                        nRow += 1;
                        ssS_Sheet1.RowCount = nRow;
                        ssS_Sheet1.Rows[nRow - 1].Height = 25;
                        ssS_Sheet1.Cells[nRow - 1, 0].Text = "■ [ P O ]";
                        strPO_START = "START";
                    }

                    nRow += 1;
                    ssS_Sheet1.RowCount = nRow;
                    ssS_Sheet1.Cells[nRow-1, 0].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim() + "hr";
                    ssS_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 1].Text == "") ssS_Sheet1.Cells[nRow - 1, 1].Text = "-";
                    ssS_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["CONTENTS_TIMES"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["QTY_TIMES"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 6].Text = "#" + dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 7].Text == "") ssS_Sheet1.Cells[nRow - 1, 7].Text = "-";
                    ssS_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["DOSNAMEK"].ToString().Trim();
                    ssS_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    if (ssS_Sheet1.Cells[nRow - 1, 9].Text == "") ssS_Sheet1.Cells[nRow - 1, 9].Text = "-";
                    

                    ssS_Sheet1.Rows[nRow - 1].Height = ssS_Sheet1.Rows[nRow-1].GetPreferredHeight() + 15;
                    for (j = 0; j < 10; j++)
                    {
                        if (strPO_START == "START")
                        {
                            if (j == 0)
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_TOPLEFT;
                            }
                            else if (j == 9)
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_TOPRIGHT;
                                strPO_START = "PASS";
                            }
                            else
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_TOP2;
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MIDLEFT;
                            }
                            else if (j == 9)
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MIDRIGHT;
                            }
                            else
                            {
                                this.ssS_Sheet1.Cells.Get(nRow - 1, j).Border = complexBorder_MID;
                            }
                        }
                    }

                    


                }
                ssS_Sheet1.RowCount += 1;
                for (i = 0; i < 10; i++)
                {
                    ssS_Sheet1.Cells[ssS_Sheet1.RowCount-1, i].Text = "ㅤ";
                    this.ssS_Sheet1.Cells.Get(ssS_Sheet1.RowCount-1, i).Border = complexBorder_TOP;
                }
                ssS_Sheet1.Rows[ssS_Sheet1.RowCount-1].Visible = true;

                ssS_Sheet1.SetColumnMerge(-1, FarPoint.Win.Spread.Model.MergePolicy.Always);
            }

            dt.Dispose();
            dt = null;

            return;

        }

        private void CELL_MERGE()
        {
            int i = 0;
            //int j = 0;
            string strTemp = "";
            string strTemp_Old = "";

            int nStart = 0;
            int nEnd = 0;

            nStart = 8;

            for (i = 8; i < ssS_Sheet1.RowCount - 1; i++)
            {
                strTemp = ssS_Sheet1.Cells[i, 1].Text.Trim();
                
                if (strTemp != strTemp_Old)
                {
                    strTemp_Old = strTemp;
                    if (i != 8 && nEnd > 0)
                    {
                        if ( nEnd-nStart+ 1 > 1)
                        {
                            ssS_Sheet1.AddSpanCell(nStart, 1, (nEnd - nStart) + 1, 1);
                            CELL_MERGE_SUB(nStart, nEnd, 5);
                            CELL_MERGE_SUB(nStart, nEnd, 6);
                            CELL_MERGE_SUB(nStart, nEnd, 7);
                            CELL_MERGE_SUB(nStart, nEnd, 8);
                            CELL_MERGE_SUB(nStart, nEnd, 9);
                        }
                    }
                    if (strTemp != "-")
                    nStart = i;
                }
                else
                {
                    if (strTemp != "-")
                        nEnd = i;
                }
            }
            
        }

        private void CELL_MERGE_SUB(int nSRow, int nERow, int nCol)
        {
            int i = 0;
            //int j = 0;
            string strTemp = "";
            string strTemp_Old = "";

            int nStart = 0;
            int nEnd = 0;

            nStart = nSRow;

            for (i = nSRow; i <= nERow; i++)
            {
                strTemp = ssS_Sheet1.Cells[i, nCol].Text.Trim();

                if (strTemp != strTemp_Old )
                {
                    strTemp_Old = strTemp;
                    nStart = i;
                }
                else
                {
                    nEnd = i;
                    ssS_Sheet1.AddSpanCell(nStart, nCol, (nEnd - nStart) + 1, 1);
                }
            }

        }


        private void SCREEN_CLEAR()
        {
            //int i = 0;

            ssS_Sheet1.Cells[2, 0].Text = "";
            ssS_Sheet1.Cells[3, 0].Text = "";
            ssS_Sheet1.Cells[4, 0].Text = "";
            ssS_Sheet1.Rows[6].Visible = false;

            ssS_Sheet1.Cells[6, 0].Text = "";

            ssS_Sheet1.RowCount = 8;
        }

        private void SET_HEADER_O(string strPANO, string strBDATE)
        {
            string str1 = "";
            string str2 = "";
            string strINDATE = "";
            string strIPDNO = "";
            string strILLCODE = "";
            string strILLNAME = "";
            string strWARD = "";
            string strROOM = "";
            string strSNAME = "";
            string strSEX = "";
            string strAGE = "";
            string strJUMIN = "";
            string strWEIGHT = "";
            string strIBW = "";
            string strHEIGHT = "";
            string strBSA = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SQL = " SELECT 0 IPDNO, PANO, SNAME, SEX, AGE, 'OPD' WARDCODE, 'OPD' ROOMCODE, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, ";
            SQL += ComNum.VBLF + " (SELECT JUMIN1 || '-' || JUMIN2 FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO AND ROWNUM = 1) JUMIN ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A ";
            SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND PANO = '" + strPANO + "' ";
            SQL += ComNum.VBLF + "   AND DEPTCODE = 'MO' ";
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
                ComFunc.MsgBox("환자정보가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                strPANO = dt.Rows[0]["PANO"].ToString().Trim();
                strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                strROOM = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                strSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                strSEX = dt.Rows[0]["SEX"].ToString().Trim();
                strAGE = dt.Rows[0]["AGE"].ToString().Trim();
                strJUMIN = dt.Rows[0]["JUMIN"].ToString().Trim();
                strINDATE = dt.Rows[0]["INDATE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT ILLCODE, ";
            SQL += ComNum.VBLF + " (SELECT ILLNAMEK FROM KOSMOS_PMPA.BAS_ILLS WHERE ILLCLASS = '1' AND ILLCODE = A.ILLCODE AND ROWNUM = 1) ILLNAMEK ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OILLS A ";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + strPANO + "' ";
            SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND DEPTCODE = 'MO'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strILLCODE = dt.Rows[0]["ILLCODE"].ToString().Trim();
                strILLNAME = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            strBSA = ComFunc.Get_Opd_BSA(strPANO, TxtSDATE.Text.Trim());
            if (strSEX == "F")
            {
                strIBW = (45.5 + (0.91 * (VB.Val(strHEIGHT) - 152.4))).ToString("N2");
            }
            else
            {
                strIBW = (50 + (0.91 * (VB.Val(strHEIGHT) - 152.4))).ToString("N2");
            }

            str1 = "주 상 병 : " + strILLCODE + "        " + strILLNAME;
            str2 = "환자정보 : " + strPANO + "     " + strSNAME + "     " + strSEX + "   " + strAGE + "   " + strJUMIN + "         ◆ 체중 : " + strWEIGHT + "kg      IBW : "
                                 + strIBW + "kg      신장 : " + strHEIGHT + "cm    BSA : " + strBSA;
            //"▶ 처방기간 : 2020-12-03 ~ 2020-12-06               [4일간]                  [DR 김양수]

            ssS_Sheet1.Cells[2, 0].Text = str1;
            ssS_Sheet1.Cells[3, 0].Text = str2;
            //ssS_Sheet1.Rows[4].Visible = false;

            ssS_Sheet1.Cells[4, 0].Text = "■ 처방일자 :  " + strBDATE;

        }

        private void SET_HEADER_I(string strPANO, string strBDATE)
        {
            string str1 = "";
            string str2 = "";
            string strINDATE = "";
            string strIPDNO = "";
            string strILLCODE = "";
            string strILLNAME = "";
            string strWARD = "";
            string strROOM = "";
            string strSNAME = "";
            string strSEX = "";
            string strAGE = "";
            string strJUMIN = "";
            string strWEIGHT = "";
            string strIBW = "";
            string strHEIGHT = "";
            string strBSA = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            SQL = " SELECT IPDNO, PANO, SNAME, SEX, AGE, WARDCODE, ROOMCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, ";
            SQL += ComNum.VBLF + " (SELECT JUMIN1 || '-' || JUMIN2 FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO AND ROWNUM = 1) JUMIN ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A ";
            SQL += ComNum.VBLF + " WHERE JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND PANO = '" + strPANO + "' ";
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
                ComFunc.MsgBox("환자정보가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                strPANO  = dt.Rows[0]["PANO"].ToString().Trim();
                strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                strROOM = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                strSNAME = dt.Rows[0]["SNAME"].ToString().Trim();
                strSEX = dt.Rows[0]["SEX"].ToString().Trim();
                strAGE = dt.Rows[0]["AGE"].ToString().Trim();
                strJUMIN = dt.Rows[0]["JUMIN"].ToString().Trim();
                strINDATE = dt.Rows[0]["INDATE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT ILLCODE, ";
            SQL += ComNum.VBLF + " (SELECT ILLNAMEK FROM KOSMOS_PMPA.BAS_ILLS WHERE ILLCLASS = '1' AND ILLCODE = A.ILLCODE AND ROWNUM = 1) ILLNAMEK ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IILLS A ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
            SQL += ComNum.VBLF + "   AND SEQNO = 1 ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strILLCODE = dt.Rows[0]["ILLCODE"].ToString().Trim();
                strILLNAME = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            strHEIGHT = ComFunc.READ_IPD_HEIGHT(clsDB.DbCon, strPANO, strINDATE);
            strWEIGHT = ComFunc.READ_IPD_WEIGHT(clsDB.DbCon, strPANO, strINDATE);

            strBSA = ComFunc.Get_Ipd_BSA(strPANO, TxtSDATE.Text.Trim());
            if (strSEX == "F")
            {
                strIBW = (45.5 + (0.91 * (VB.Val(strHEIGHT) - 152.4))).ToString("N2");
            }
            else
            {
                strIBW = (50 + (0.91 * (VB.Val(strHEIGHT) - 152.4))).ToString("N2");
            }

            str1 = "주 상 병 : " + strILLCODE + "        " + strILLNAME + "                 병동/호실 :  " + strWARD + " / " + strROOM;
            str2 = "환자정보 : " + strPANO + "     " + strSNAME + "     " + strSEX + "   " + strAGE + "   " + strJUMIN + "         ◆ 체중 : " + strWEIGHT + "kg      IBW : "
                                 + strIBW + "kg      신장 : " + strHEIGHT + "cm    BSA : " + strBSA;
            //"▶ 처방기간 : 2020-12-03 ~ 2020-12-06               [4일간]                  [DR 김양수]

            ssS_Sheet1.Cells[2, 0].Text = str1;
            ssS_Sheet1.Cells[3, 0].Text = str2;
            //ssS_Sheet1.Rows[4].Visible = false;

            ssS_Sheet1.Cells[4, 0].Text = "■ 처방일자 :  " + strBDATE;

        }

        private int SET_HEADER_DAY(string strBDATE, int nROW )
        {
            nROW = nROW + 1;
            ssS.ActiveSheet.RowCount = nROW;
            ssS_Sheet1.Cells[nROW, 10].Text = "";

            return nROW;
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false,true, 0.9f);

            SPR.setSpdPrint(ssS, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
