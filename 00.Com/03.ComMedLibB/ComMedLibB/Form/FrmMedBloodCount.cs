using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmMedBloodCount.cs
    /// Description     : 혈액 재고 조회
    /// Author          : 안정수
    /// Create Date     : 2017-11-23
    /// Update History  :
    /// <history>
    /// d:\psmh\FrmBloodCount.frm(FrmBloodCount) => FrmMedBloodCount.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\FrmBloodCount.frm(FrmBloodCount)
    /// </seealso>
    /// </summary>
    public partial class FrmMedBloodCount : Form
    {
        public FrmMedBloodCount()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            eGetData();
        }

        private void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnView)
            {
                //
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        private void eGetData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            ssList_Sheet1.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  ABO, DECODE(COMPONENT,'BT021','P/C','BT041','FFP','BT023','PLT/C','BT011','W/B','BT051','Cyro','BT071','W/RBC','BT101','WBC/C','기타') COMPONENT,";
            SQL += ComNum.VBLF + "  CAPACITY, COUNT(*) CNT ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_BLOOD_IO";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND OutDate Is Null";
            SQL += ComNum.VBLF + "      AND  Daeyudate is null";
            SQL += ComNum.VBLF + "      AND  Expire  > TRUNC(SYSDATE)";
            SQL += ComNum.VBLF + "      AND  ( GBSTATUS NOT IN ('M','C','P','B','D')  OR GBSTATUS IS NULL )";
            SQL += ComNum.VBLF + "      AND  STORERDATE  IS NULL";
            SQL += ComNum.VBLF + "GROUP BY  COMPONENT, CAPACITY, ABO";

            try
            {
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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["COMPONENT"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ABO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CAPACITY"].ToString().Trim() + "ml";
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }
    }
}