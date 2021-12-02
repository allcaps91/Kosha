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
    /// File Name       : frmPmpaViewTodayJepList.cs
    /// Description     : 당일접수 및 재원자 내역
    /// Author          : 안정수
    /// Create Date     : 2017-09-11
    /// Update History  : 
    /// <history>       
    /// 실제 데이터로 테스트 필요
    /// d:\psmh\OPD\oiguide\Frm접수명단.frm(Frm접수명단) => frmPmpaViewTodayJepList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\Frm접수명단.frm(Frm접수명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewTodayJepList : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string FstrPANO = "";
        string mstrRetValue = "";

        public frmPmpaViewTodayJepList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewTodayJepList(string GstrRetValue)
        {
            InitializeComponent();
            setEvent();
            mstrRetValue = GstrRetValue;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

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

            ssList_Sheet1.Columns[5].Visible = false;
            FstrPANO = mstrRetValue;


            READ_OPD_MASTER(FstrPANO);

            mstrRetValue = "";
        }

        void READ_OPD_MASTER(string ArgPano)
        {
            int i = 0;

            ssList_Sheet1.Rows.Count = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                  ";
            SQL += ComNum.VBLF + "  '외래' GBN, PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                               ";
            SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                                       ";
            SQL += ComNum.VBLF + "      AND BDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                           ";
            SQL += ComNum.VBLF + "GROUP BY PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(BDATE,'YYYY-MM-DD')                         ";

            SQL += ComNum.VBLF + "UNION ALL                                                                               ";

            SQL += ComNum.VBLF + "SELECT                                                                                  ";
            SQL += ComNum.VBLF + "  '입원' GBN, PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(INDATE,'YYYY-MM-DD') BDATE             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                               ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                               ";
            SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                                       ";
            SQL += ComNum.VBLF + "      AND (JDate =TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE =TRUNC(SYSDATE) )       ";
            SQL += ComNum.VBLF + "GROUP BY PANO,SNAME,DEPTCODE,DRCODE,TO_CHAR(INDATE,'YYYY-MM-DD')                        ";
            SQL += ComNum.VBLF + "ORDER BY PANO,SName                                                                     ";

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
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Gbn"].ToString().Trim();
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

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                READ_OPD_MASTER(FstrPANO);
            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            mstrRetValue = "";

            if (e.Row >= 0)
            {
                mstrRetValue = ssList_Sheet1.Cells[e.Row, 3].Text + "^^";
                mstrRetValue += ssList_Sheet1.Cells[e.Row, 5].Text + "^^";
            }
        }
    }
} 
