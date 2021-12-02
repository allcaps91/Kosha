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
    /// File Name       : frmPmpaViewSearchSuga.cs
    /// Description     : 수가 조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-09-19
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\oumsad\Frm수가조회.frm(Frm수가조회) => frmPmpaViewSearchSuga.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm수가조회.frm(Frm수가조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSearchSuga : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaViewSearchSuga()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtSuga.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtSuga.Text.Trim() == "")
                {
                    return;
                }
                if (optGbn0.Checked == true)
                {
                    txtSuga.Text = txtSuga.Text.Trim().ToUpper();
                }
                eGetData();
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
            optGbn0.Checked = true;

            CS.Spread_All_Clear(ssList);

            txtSuga.Text = "";

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                if (txtSuga.Text.Trim() == "")
                {
                    return;
                }
                else
                {
                    eGetData();
                }
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  Nu,Bun,SuCode,Gbn,SuNext,SugbSS,SugbBi,SuQty,SugbA,SugbB,SugbC,SugbD,SugbE,";
            SQL += ComNum.VBLF + "  SugbF,SugbG,SugbH,SugbI,SugbJ,SugbK,SugbL,SugbM,SugbN,SugbO,";
            SQL += ComNum.VBLF + "  SUGBP, SUGBQ, SUGBR, SUGBS, SUGBT, SUGBU,";
            SQL += ComNum.VBLF + "  SugbN, SugbV, DayMax,TotMax,IAmt,TAmt,BAmt,";
            SQL += ComNum.VBLF + "  TO_CHAR(SuDate,'YYYY-MM-DD')  SuDate,OldIAmt,OldtAmt,OldBAmt,";
            SQL += ComNum.VBLF + "  TO_CHAR(SuDate3,'YYYY-MM-DD') SuDate3,IAmt3,TAmt3,BAmt3,";
            SQL += ComNum.VBLF + "  TO_CHAR(SuDate4,'YYYY-MM-DD') SuDate4,IAmt4,TAmt4,BAmt4,";
            SQL += ComNum.VBLF + "  TO_CHAR(SuDate5,'YYYY-MM-DD') SuDate5,IAmt5,TAmt5,BAmt5,";
            SQL += ComNum.VBLF + "  SuNameK,SuNameE,SunameG,Unit,DaiCode,HCode,BCode,";
            SQL += ComNum.VBLF + "  SuHam,EdiJong,TO_CHAR(EdiDate,'YYYY-MM-DD') EdiDate,";
            SQL += ComNum.VBLF + "  OldBCode,OldGesu,OldJong,WonCode,WonAmt,NurCode, NROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (optGbn0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND SuNext  like '%" + txtSuga.Text + "%' ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND SuNamek  like '%" + txtSuga.Text + "%'";
            }
            SQL += ComNum.VBLF + "      AND DELDATE IS NULL";

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
                    nRow = 0;

                    ssList_Sheet1.Rows.Count = 0;
                    ssList_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bamt"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Tamt"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Iamt"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    }

                    ssList_Sheet1.Rows.Count = nRow;
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
