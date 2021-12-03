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
    public partial class frmXRaySuga1 : Form
    {
        public frmXRaySuga1()
        {
            InitializeComponent();
        }

        void frmXRaySuga1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetInit();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SetInit()
        {
            int i = 0;

            ssSuga1_Sheet1.RowCount = 0;

            string strBCode = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            DataTable dt2 = null;

            dtpSuga.Text = "2001-01-01";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   BCode,COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE";
                SQL = SQL + ComNum.VBLF + "WHERE Bun >= '65' AND Bun <= '73'";
                SQL = SQL + ComNum.VBLF + "     AND SugbE = '0' ";
                SQL = SQL + ComNum.VBLF + "     AND SugbF = '0' ";
                SQL = SQL + ComNum.VBLF + "     AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "      AND BCode NOT IN ('000000','999999','AAAAAA') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY BCode ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssSuga1_Sheet1.RowCount = dt.Rows.Count;
                for(i = 0; i < ssSuga1_Sheet1.RowCount; i++)
                {
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                    // 표준코드명, 단위, 규격을 READ
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   PName,Danwi1,Danwi2,Spec";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_SUGA";
                    SQL = SQL + ComNum.VBLF + "WHERE Code='" + strBCode + "'";
                    SQL = SQL + ComNum.VBLF + "     AND Jong='8' ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt2.Rows.Count > 0)
                    {
                        ssSuga1_Sheet1.Cells[i, 0].Text = strBCode;
                        ssSuga1_Sheet1.Cells[i, 2].Text = dt2.Rows[0]["Danwi1"].ToString().Trim() + dt2.Rows[0]["Danwi2"].ToString().Trim();
                        ssSuga1_Sheet1.Cells[i, 3].Text = dt2.Rows[0]["PName"].ToString().Trim() + dt2.Rows[0]["Spec"].ToString().Trim();

                    }
                    dt2.Dispose();
                    dt2 = null;
                }
                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ss_Clear(ssSuga1_Sheet1);
            ss_Clear(ssSuga2_Sheet1);
        }

        void ss_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            int i, j;

            for(i = 0; i < Spd.RowCount; i++)
            {
                for(j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            int i = 0;

            string strBcode = "";

            int nPrice = 0;
            string strOK = "";
            string strName = "";
            int nIlsu = 0;
            int nJobCNT = 0;

            string GstrMsgList = "";

            string day = DateTime.Now.ToString("yyyy-MM-dd");
            if(String.Compare(dtpSuga.Text, day) < 0)
            {
                nIlsu = Convert.ToInt16(DATE_ILSU(dtpSuga.Text.Trim(), day));
            }
            else
            {
                nIlsu = Convert.ToInt16(DATE_ILSU(dtpSuga.Text.Trim(), day));
            }

            if(nIlsu > 10)
            {
                ComFunc.MsgBox("작업일자는 현재일 기준 전후 10일간만 가능함.");
            }

            GstrMsgList = "변경할 수가 적용일자가 정말로 " + dtpSuga.Text.Trim() + "일이" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "맞습니까? 만일 오류날짜를 지정하면 " + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 복구가 불가능 합니다. ";

            if (MessageBox.Show(GstrMsgList, "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            btnStart.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = false;

            strOK = "OK";
            ssSuga2_Sheet1.RowCount = 50;
            ss_Clear(ssSuga2_Sheet1);

            for(i = 0; i < ssSuga1_Sheet1.RowCount; i++)
            {
                strBcode = ssSuga1_Sheet1.Cells[i, 0].Text;
                nPrice = Convert.ToInt16(ssSuga1_Sheet1.Cells[i, 1].Text);
                strName = ssSuga1_Sheet1.Cells[i, 3].Text;
            

                if(nPrice > 0)
                {
                    nJobCNT = Convert.ToInt16(BCode_SUGA_UPDATE(dtpSuga.Text.Trim(), strBcode, nPrice, 0, 0));
                    if(nJobCNT != -2 && nJobCNT != 0)
                    {
                        int nRow = 0;
                        nRow += 1;
                        if(ssSuga2_Sheet1.RowCount < nRow)
                        {
                            ssSuga2_Sheet1.RowCount = nRow;
                        }
                        ssSuga2_Sheet1.Cells[nRow, 0].Text = strBcode;
                        ssSuga2_Sheet1.Cells[nRow, 1].Text = String.Format("{0:#,##0}", nPrice); 
                        if(nJobCNT == -1)
                        {
                            ssSuga2_Sheet1.Cells[nRow, 2].Text = "** ERROR **";
                        }
                        else
                        {
                            ssSuga2_Sheet1.Cells[nRow, 2].Text = String.Format("{0:#,##0}", nJobCNT);
                        }

                        ssSuga2_Sheet1.Cells[nRow, 3].Text = strName;
                    }
                }
            }

            if(strOK == "OK")
            {
                btnStart.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = true;

                ComFunc.MsgBox("정상적으로 처리됨");
            }
        }

        string DATE_ILSU(string ArgTdate, string ArgFdate, string ArgGb = "")
        {
            //TODO Busuga1, VbFunc.bas
            DataTable dt = null;
            string SQL = "";
            string rtnVal = "";
            string SqlErr = "";

            if(VB.Len(ArgFdate) != 10 || VB.IsDate(ArgFdate) || VB.Len(ArgTdate) != 10 || VB.IsDate(ArgFdate))
            {
                return rtnVal;
            }

           if(String.Compare(ArgFdate, ArgTdate) > 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ArgTdate + "','YYYY-MM-DD') - ";
                SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ArgFdate + "','YYYY-MM-DD') Gigan ";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["Gigan"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                if(ArgGb != "ALL")
                {
                    if (VB.Val(rtnVal) >= 1000)  //'일수 계산 제한 옵션으로 풀도록 함수 수정
                    {
                        rtnVal = "999";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        string BCode_SUGA_UPDATE(string ArgDate, string ArgBCode, int ArgPrice, Double ArgJemsu, Double ArgJemsuPrice)
        {
            //TODO, Busuga00.bas 수가 관련 코드 구현필요
            string rtnval = "";

            return rtnval;
        }
    }
}
