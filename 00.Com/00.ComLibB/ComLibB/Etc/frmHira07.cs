using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmHira07
    /// Description     : 심평원자료-임부금기
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira07) >> frmHira07.cs 폼이름 재정의" />
    public partial class frmHira07 : Form
    {
        public frmHira07()
        {
            InitializeComponent();
        }

        private void frmHira07_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //'     ELMT_CD     VARCHAR2(9)     성분코드
                //' ADPT_FR_DT  VARCHAR2(8)     적용개시일자
                //' ADPT_TO_DT  VARCHAR2(8)     적용종료일자
                //' EXAM_TYPE   VARCHAR2(1)     점검구분(A:정보제공, B:사유전송, C.절대금기
                //' CONTRAD_GRADE   VARCHAR2(1)     등급구분(1:1등급, 2:2등급)
                //' ADPT_TYPE   VARCHAR2(1)     적용구분(0.적용, 1.해지)
                //' INCOMP_REASON   VARCHAR2(2000)  금기사유

                SQL = "   SELECT C.SUNEXT SUNEXT, C.SUNAMEK,  A.EXAM_TYPE, A.CONTRAD_GRADE, A.ADPT_TYPE, A.INCOMP_REASON, ";
                SQL += ComNum.VBLF + "    A.ADPT_FR_DT,  A.ADPT_TO_DT , D.DELDATE";
                SQL += ComNum.VBLF + "    FROM ADMIN.HIRA_TBJBD63 A, ADMIN.EDI_SUGA B,  ADMIN.BAS_SUN C, ADMIN.BAS_SUT D";
                SQL += ComNum.VBLF + "    WHERE A.ELMT_CD = B.SCODE";
                SQL += ComNum.VBLF + "    AND  B.CODE = C.BCODE";
                SQL += ComNum.VBLF + "    AND C.SUNEXT = D.SUNEXT(+)";

                if (rdoExam1.Checked == true) SQL += ComNum.VBLF + " AND A.EXAM_TYPE = 'A' ";
                if (rdoExam2.Checked == true) SQL += ComNum.VBLF + " AND A.EXAM_TYPE = 'B' ";
                if (rdoExam3.Checked == true) SQL += ComNum.VBLF + " AND A.EXAM_TYPE = 'C' ";

                SQL += ComNum.VBLF + "  GROUP BY C.SUNEXT , C.SUNAMEK,  A.EXAM_TYPE, A.CONTRAD_GRADE, A.ADPT_TYPE, A.INCOMP_REASON, ";
                SQL += ComNum.VBLF + "    A.ADPT_FR_DT,  A.ADPT_TO_DT , D.DELDATE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();

                    switch(dt.Rows[i]["EXAM_TYPE"].ToString().Trim())
                    {
                        case "A":
                            ss1_Sheet1.Cells[i, 2].Text = "A.정보제공";
                            break;
                        case "B":
                            ss1_Sheet1.Cells[i, 2].Text = "B.사유전송";
                            break;
                        case "C":
                            ss1_Sheet1.Cells[i, 2].Text = "C.절대금기";
                            break;
                    }

                    if (dt.Rows[i]["CONTRAD_GRADE"].ToString().Trim() == "1")
                    {
                        ss1_Sheet1.Cells[i, 3].Text = "1등급";
                    }
                    else if (dt.Rows[i]["CONTRAD_GRADE"].ToString().Trim() == "2")
                    {
                        ss1_Sheet1.Cells[i, 3].Text = "2등급";
                    }

                    ss1_Sheet1.Cells[i, 4].Text = VB.IIf(dt.Rows[i]["ADPT_TYPE"].ToString().Trim() == "0", "적용", "해지").ToString();

                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INCOMP_REASON"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["DelDate"].ToString().Trim(), "D");
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ADPT_FR_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ADPT_TO_DT"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "심평원자료- 임부금기 약제 LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력 일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(82) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 180, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            ComFunc.MsgBox(ss1_Sheet1.Cells[e.Row, 5].Text);
        }
    }
}
