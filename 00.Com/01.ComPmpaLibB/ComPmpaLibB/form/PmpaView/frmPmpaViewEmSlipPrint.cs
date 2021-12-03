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
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewEmSlipPrint.cs
    /// Description     : 응급실 Slip 대조List
    /// Author          : 안정수
    /// Create Date     : 2017-08-18
    /// Update History  : 2017-10-25
    /// 출력 부분, 버튼 Enable 부분 수정
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA13.FRM(FrmEmSlipPrint) => frmPmpaViewEmSlipPrint.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA13.FRM(FrmEmSlipPrint)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewEmSlipPrint : Form
    {
        public frmPmpaViewEmSlipPrint()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
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

            ssList_Sheet1.Rows.Count = 0;
            Set_Combo();
        }

        void Set_Combo()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboPart.Items.Clear();
            cboPart.Items.Add("*.전체");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  PART,NAME                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PASS       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "      AND PART >= 'A'                     ";
            SQL += ComNum.VBLF + "GROUP BY Part,Name                        ";
            SQL += ComNum.VBLF + "ORDER BY Name                             ";

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboPart.Items.Add(dt.Rows[i]["Part"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            cboPart.SelectedIndex = 0;

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
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;


                #region //시트 히든

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


                #endregion

                strTitle = dtpDate.Text + "일 응급실 SLIP CHECK-LIST";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 160, 120);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, false, true, false, false, false);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

                #region //시트 히든 복원

                //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
                #endregion
            }
        }

        void eGetData()
        {
            int i = 0;
            int nMaxRow = 0;
            int nRow = 0;

            string strEOF = "";
            string strData1 = "";
            string strData2 = "";


            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  a.Part,a.SeqNo,TO_CHAR(a.Bdate,'YY-MM-DD') BIlja,                                                           ";
            SQL += ComNum.VBLF + "  a.Pano,b.Sname,a.Bi,a.DeptCode,a.SuCode,a.SuNext,a.Qty,a.Nal,                                               ";
            SQL += ComNum.VBLF + "  a.GbSelf,a.Amt1,c.SuNameK                                                                                   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "BAS_PATIENT b, " + ComNum.DB_PMPA + "BAS_SUN c   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "      AND a.ActDate = TO_DATE('" + dtpDate.Text + "','yyyy-mm-dd')                                            ";
            SQL += ComNum.VBLF + "      AND a.DeptCode = 'ER'                                                                                   ";
            SQL += ComNum.VBLF + "      AND a.Bun < '85'                                                                                        ";
            if (VB.Left(cboPart.SelectedItem.ToString(), 1) != "*")
            {
                SQL += ComNum.VBLF + "      AND a.Part = '" + VB.Left(cboPart.SelectedItem.ToString(), 1).ToUpper() + "'                        ";
            }
            SQL += ComNum.VBLF + "      AND a.Pano = b.Pano                                                                                     ";
            SQL += ComNum.VBLF + "      AND a.SuNext = c.SuNext                                                                                 ";
            SQL += ComNum.VBLF + "ORDER BY a.Part,a.SeqNo,a.Bun,a.SuNext                                                                        ";

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

                    btnView.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = false;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                nRow = 0;
                nMaxRow = 0;
                strData1 = "";
                strData2 = "";

                if (dt.Rows.Count > 0)
                {
                    nMaxRow += dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    if (nMaxRow > ssList_Sheet1.Rows.Count)
                    {
                        ssList_Sheet1.Rows.Count = nMaxRow;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        strData1 = dt.Rows[i]["Part"].ToString().Trim() + ComFunc.SetAutoZero(dt.Rows[i]["SeqNo"].ToString().Trim(), 4);
                        if (strData1 != strData2)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Part"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SeqNo"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["BIlja"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            strData2 = strData1;
                        }

                        ssList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:###0.0}", VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        ssList_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Nal"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 11].Text = String.Format("{0:#,##0}", VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()));
                        ssList_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
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

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }

        void cboPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
        }
    }
}
