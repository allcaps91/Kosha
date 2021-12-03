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
    /// File Name       : frmHira03
    /// Description     : 심평원자료-안전성
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira03) >> frmHira03.cs 폼이름 재정의" />
    public partial class frmHira03 : Form
    {
        public frmHira03()
        {
            InitializeComponent();
        }

        private void frmHira03_Load(object sender, EventArgs e)
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
                SQL = "  SELECT B.SUNEXT, B.SUNAMEK,  MEDC_CD, MEDC_INF_TYPE, TO_CHAR(C.DELDATE,'YYYYMMDD') DELDATE,     ";
                SQL += ComNum.VBLF + " A.ADPT_FR_DT,"; //'   적용시작일자"
                SQL += ComNum.VBLF + " A.ADPT_TO_DT"; //'   적용종료일자"
                SQL += ComNum.VBLF + "   FROM ADMIN.HIRA_TBJBD48 A, ADMIN.BAS_SUN B , ADMIN.BAS_SUT C";
                SQL += ComNum.VBLF + " WHERE ADPT_TYPE = '0' "; //' 0:        적용 , 1: 해지
                SQL += ComNum.VBLF + "   AND ADPT_FR_DT <=  '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "' ";
                SQL += ComNum.VBLF + "   AND A.MEDC_CD =  B.BCODE ";
                SQL += ComNum.VBLF + "   AND B.SUNEXT = C.SUNEXT ";
                SQL += ComNum.VBLF + " ORDER BY B.SUNEXT, ADPT_FR_DT DESC";

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

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEDC_CD"].ToString().Trim();

                    //'(A:안전성 관련, B:허가취소, c.급여중지, d.제한적사용)
                    switch(dt.Rows[i]["MEDC_INF_TYPE"].ToString().Trim())
                    {
                        case "A":
                            ss1_Sheet1.Cells[i, 3].Text = "안전성 관련";
                            break;
                        case "B":
                            ss1_Sheet1.Cells[i, 3].Text = "허가취소";
                            break;
                        case "C":
                            ss1_Sheet1.Cells[i, 3].Text = "급여중지";
                            break;
                        case "D":
                            ss1_Sheet1.Cells[i, 3].Text = "제한적사용";
                            break;
                    }
                    ss1_Sheet1.Cells[i, 4].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["DelDate"].ToString().Trim(), "D");
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ADPT_FR_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ADPT_TO_DT"].ToString().Trim();
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
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

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

            strTitle = "심평원자료- 안전성 금기 약제 LIST";

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
    }
}
