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
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPano.cs
    /// Description     : 환자상세내역 
    /// Author          : 안정수
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oiguide\oiguide07.frm(FrmPanoView) => frmPmpaViewPano.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\oiguide07.frm(FrmPanoView)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewPano : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewPano()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtData.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.optJob0.CheckedChanged += new EventHandler(eOpt_Change);
            this.optJob1.CheckedChanged += new EventHandler(eOpt_Change);

        }
    
        void eOpt_Change(object sender, EventArgs e)
        {
            if(sender == this.optJob0)
            {
                txtGbn.Text = "병록번호";
                txtData.Text = "";
            }

            else if(sender == this.optJob1)
            {
                txtGbn.Text = "수진자명";
                txtData.Text = "";
            }
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

            optJob0.Checked = true;

            CS.Spread_All_Clear(ssList);

            Screen_Clear();

            txtGbn.Text = "병록번호";
            txtData.Text = "";

            for(int i = 3; i <= 8; i++)
            {
                ssList_Sheet1.Columns[i].Visible = false;
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtData)
            {
                if (e.KeyChar == 13)
                {
                    eGetData();
                }
            }
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
                eGetData();

                ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
        }

        void Screen_Clear()
        {
            ComFunc.SetAllControlClear(panel5);
        }      

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                else if (e.RowHeader == true)
                {
                    return;
                }
                else
                {
                    Screen_Clear();

                    txtPano.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 0].Text;
                    txtSName.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 1].Text;
                    txtJumin.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 2].Text;
                    txtSex.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 3].Text;
                    txtDept.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 4].Text;
                    txtBi.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 5].Text;
                    txtTel.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 6].Text;
                    txtLastD.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 7].Text;
                    txtJuso.Text = VB.Space(2) + ssList_Sheet1.Cells[e.Row, 8].Text;

                }
            }
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            ssList_Sheet1.Rows.Count = 0;

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  Pano, SName,jumin1,jumin2,BI,TEL,ZIPCODE1,ZIPCODE2,JUSO,SEX,deptcode, ";
            SQL += ComNum.VBLF + "  TO_CHAR(LASTDATE,'YYYY-MM-DD') LASTDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (optJob0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Pano = '" + ComFunc.SetAutoZero(txtData.Text, 8) + "'";
            }
            else if(optJob1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Sname = '" + txtData.Text.Trim() + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY  Pano";

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
                    nRead = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nRead;                    

                    for(i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["jumin1"].ToString().Trim() + "-" + dt.Rows[i]["jumin2"].ToString().Trim();

                        if(dt.Rows[i]["sex"].ToString().Trim() == "M")
                        {
                            ssList_Sheet1.Cells[i, 3].Text = "남자";
                        }
                        else if(dt.Rows[i]["sex"].ToString().Trim() == "F")
                        {
                            ssList_Sheet1.Cells[i, 3].Text = "여자";
                        }

                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["bi"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["lastdate"].ToString().Trim();
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
    }
}
