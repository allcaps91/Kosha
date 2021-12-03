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
    /// File Name       : frmPmpaTongWonmu.cs
    /// Description     : 의사별 내시경건수 통계
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 2017-11-02
    /// <history>  
    /// 빌드형성 부분 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\Frm원무과통계자료.frm(Frm원무과통계작업) => frmPmpaTongWonmu.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm원무과통계자료.frm(Frm원무과통계작업)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongSinhwan : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread(); 

        public frmPmpaTongSinhwan()
        {
            InitializeComponent();
            setEvent();
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

            
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            ssList2_Sheet1.Rows.Count = 1;
            ssList2_Sheet1.Rows.Count = 1;
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

                eGetData1();
                eGetData2();
                eGetData3();
             
            }

        }

        void eGetData1()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            double nTot = 0;

            ssList1_Sheet1.Rows.Count = 1;

            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUM(1) CNT ";
            SQL += ComNum.VBLF + "   FROM ADMIN.OPD_MASTER ";
            SQL += ComNum.VBLF + " WHERE SINGU = '1' ";
            SQL += ComNum.VBLF + "   AND BDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND BDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + " GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + " ORDER BY BDATE ASC ";
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
                ssList1_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CNT"].ToString().Trim();
                }

            }
            dt.Dispose();
            dt = null;

            for (i = 0; i < ssList1_Sheet1.RowCount; i++)
            {
                nTot = nTot + VB.Val(ssList1_Sheet1.Cells[i, 1].Text.Trim());
            }
            
            if (nTot >0 )
            {
                ssList1_Sheet1.RowCount += 1;
                ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 0].Text = "합계";
                ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 1].Text = nTot.ToString();
            }

            ssList1_Sheet1.SetRowHeight(-1, 25);
        }

        void eGetData2()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            double nTot = 0;

            string strDEPTCODE = "";
            string strBDATE = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList2_Sheet1.Rows.Count = 1;

            //외래통계 자료 구함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE ";
            SQL += ComNum.VBLF + "   FROM ADMIN.BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  WHERE GBJUPSU = '1' ";
            SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ASC ";
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
                ssList2_Sheet1.Columns.Count = dt.Rows.Count + 1;

                for (i = 1; i < ssList2_Sheet1.Columns.Count; i++)
                {
                    ssList2_Sheet1.Cells[0, i].Text = dt.Rows[i - 1]["DEPTCODE"].ToString().Trim();
                    ssList2_Sheet1.SetColumnWidth(i, 30);
                    ssList2_Sheet1.Columns.Get(i).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                    ssList2_Sheet1.Columns.Get(i).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                }
            }
            dt.Dispose();
            dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(JOBDATE,'YYYY-MM-DD') JOBDATE ";
            SQL += ComNum.VBLF + "   FROM ADMIN.BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE JOBDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND JOBDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY JOBDATE ASC ";
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
                ssList2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                for (i = 1; i < ssList2_Sheet1.Rows.Count; i++)
                {
                    ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i - 1]["JOBDATE"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;


            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, SUM(1) CNT ";
            SQL += ComNum.VBLF + "   FROM ADMIN.OPD_MASTER ";
            SQL += ComNum.VBLF + " WHERE SINGU = '1' ";
            SQL += ComNum.VBLF + "   AND BDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND BDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + " GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'), DEPTCODE ";
            SQL += ComNum.VBLF + " ORDER BY BDATE ASC ";
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
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();
                    strDEPTCODE = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                    for (j = 0; j < ssList2_Sheet1.RowCount; j++)
                    {
                        if (ssList2_Sheet1.Cells[j, 0].Text.Trim() == strBDATE)
                        {
                            break;
                        }
                    }

                    for (k = 0; k < ssList2_Sheet1.ColumnCount; k++)
                    {
                        if (ssList2_Sheet1.Cells[0, k].Text.Trim() == strDEPTCODE)
                        {
                            break;
                        }
                    }

                    //ssList2_Sheet1.Cells[j, k].Text = (VB.Val(ssList2_Sheet1.Cells[j, k].Text.Trim()) + VB.Val(dt.Rows[i]["CNT"].ToString().Trim())).ToString();
                    ssList2_Sheet1.Cells[j, k].Text = dt.Rows[i]["CNT"].ToString().Trim();

                }
            }
            dt.Dispose();
            dt = null;

            ssList2_Sheet1.RowCount += 1;
            ssList2_Sheet1.Cells[ssList2_Sheet1.RowCount - 1, 0].Text = "합계";

            for (j = 1; j < ssList2_Sheet1.ColumnCount; j++)
            {
                nTot = 0;
                for (i = 1; i < ssList2_Sheet1.RowCount - 1; i++)
                {
                    nTot = nTot + VB.Val(ssList2_Sheet1.Cells[i, j].Text.Trim());
                }

                if (nTot > 0)
                {
                    ssList2_Sheet1.Cells[ssList2_Sheet1.RowCount - 1, j].Text = nTot.ToString();
                }
            }

            ssList2_Sheet1.SetRowHeight(-1, 25);
        }

        void eGetData3()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList3_Sheet1.Rows.Count = 0;

            //외래통계 자료구함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.DEPTCODE, (SELECT DRNAME FROM ADMIN.BAS_DOCTOR WHERE DRCODE = A.DRCODE AND ROWNUM = 1) DRNAME,  ";
            SQL += ComNum.VBLF + " A.PANO, A.SNAME, A.SEX, A.AGE, A.BI ";
            SQL += ComNum.VBLF + "   FROM ADMIN.OPD_MASTER A ";
            SQL += ComNum.VBLF + "  WHERE SINGU = '1' ";
            SQL += ComNum.VBLF + "   AND BDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND BDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY BDATE ASC, DEPTCODE ASC ";
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
                ssList3_Sheet1.RowCount = dt.Rows.Count;
                                        
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssList3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["BI"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

            ssList3_Sheet1.SetRowHeight(-1, 25);
        }

    }
}
