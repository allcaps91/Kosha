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
    /// File Name       : frmPmpaViewSname.cs
    /// Description     : 수진자명단 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWASN_new.FRM(FrmSname_new) => frmPmpaViewSname.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWASN_new.FRM(FrmSname_new)
    /// </seealso>
    /// </summary>   

    public partial class frmPmpaViewSname : Form
    {
        string mstrView1 = "";

        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendText(string strText);
        public event SendText rSendText;

        public frmPmpaViewSname()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSname(string GstrView1)
        {
            InitializeComponent();
            mstrView1 = GstrView1;
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
            ssList_Sheet1.Columns[7].Visible = true;

            eGetData();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                rEventExit();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                eGetData();
            }
        }

        void eGetData()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.Rows.Count - 1, ssList_Sheet1.Columns.Count - 1].Text = "";

            btnExit.Enabled = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  Pano, to_single_byte(Sname),Sex,Jumin1,Jumin2,                                                              ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'yyyy-mm-dd') StartDate,                                                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(LastDate,'yyyy-mm-dd') LastDate,JiName,P.ZipCode1,                                                  ";
            SQL += ComNum.VBLF + "  P.ZipCode2,ZipName1,ZipName2,ZipName3,Juso,Tel, Hphone, P.ROWID                                             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, " + ComNum.DB_PMPA + "BAS_AREA A, " + ComNum.DB_PMPA + "BAS_ZIPS Z  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "      AND P.JiCode = A.JiCode(+)                                                                              ";
            SQL += ComNum.VBLF + "      AND P.ZipCode1 = Z.ZipCode1(+)                                                                          ";
            SQL += ComNum.VBLF + "      AND P.ZipCode2 = Z.ZipCode2(+)                                                                          ";

            //성명
            if (VB.Pstr(mstrView1, "^^", 1) == "1")
            {
                SQL += ComNum.VBLF + "  AND SName LIKE '" + VB.Pstr(mstrView1, "^^", 2) + "%'                                                   ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                    ";
            }
            //병록번호
            else if (VB.Pstr(mstrView1, "^^", 1) == "2")
            {
                SQL += ComNum.VBLF + "  AND Pano = '" + VB.Pstr(mstrView1, "^^", 2) + "'                                                        ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                    ";
            }
            //증번호
            else if (VB.Pstr(mstrView1, "^^", 1) == "3")
            {
                SQL += ComNum.VBLF + "  AND GKIHO = '" + VB.Pstr(mstrView1, "^^", 2) + "'                                                       ";
                SQL += ComNum.VBLF + "ORDER BY Jumin1,Jumin2                                                                                    ";
            }

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
                    if (dt.Rows.Count < 20)
                    {
                        ssList_Sheet1.Rows.Count = 20;
                    }
                    else
                    {
                        ssList_Sheet1.Rows.Count = dt.Rows.Count;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["to_single_byte(Sname)"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + " - " + dt.Rows[i]["Jumin2"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["LastDate"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JiName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

            btnExit.Enabled = true;
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row >= 0)
            {
                clsPmpaPb.GnChoice = ssList_Sheet1.ActiveRowIndex;

                clsPmpaPb.gstrView2 = ssList_Sheet1.Cells[e.Row, 7].Text;

                rSendText(clsPmpaPb.gstrView2);

                rEventExit();
            }
        }
    }
}
