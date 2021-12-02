using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewChul.cs
    /// Description     : 응급실 출입증교부 대상자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oumsad\FrmChul.frm(FrmChul) => frmPmpaViewChul.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\FrmChul.frm(FrmChul)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewChul : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewChul()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등              

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            CS.Spread_All_Clear(ssList);
        }

        void eBtnEvent(object sender, EventArgs e)
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

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;
            nRow = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(BDate,'YYYY-MM-DD') BDate, Pano, CHUL, SNAME            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND BDate >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND BDate <=TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND DEPTCODE = 'ER'                                         ";
            SQL += ComNum.VBLF + "ORDER By BDate,SNAME                                              ";

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
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;

                        if (ssList_Sheet1.Rows.Count < nRow)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        switch (dt.Rows[i]["CHUL"].ToString().Trim())
                        {
                            case "Y":
                                ssList_Sheet1.Cells[nRow - 1, 3].Text = "Y.출입증교부";
                                break;
                            case "S":
                                ssList_Sheet1.Cells[nRow - 1, 3].Text = "S.출입증반납";
                                break;
                            default:
                                ssList_Sheet1.Cells[nRow - 1, 3].Text = "";
                                break;
                        }
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
        }

        void eSaveData()
        {
            int i = 0;

            string strBDate = "";
            string strPano = "";
            string strFlag = "";

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                strBDate = ssList_Sheet1.Cells[i, 0].Text;
                strPano = ssList_Sheet1.Cells[i, 1].Text;
                strFlag = VB.Left(ssList_Sheet1.Cells[i, 3].Text, 1);

                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_MASTER " + "SET          ";
                SQL += ComNum.VBLF + "  Chul = '" + strFlag + "'                                ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                SQL += ComNum.VBLF + "      AND Pano ='" + strPano + "'                         ";
                SQL += ComNum.VBLF + "      AND DEPTCODE ='ER'                                  ";
                SQL += ComNum.VBLF + "      AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                try
                {
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            Cursor.Current = Cursors.Default;
        }

    }
}
