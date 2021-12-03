using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSlipView2 : Form
    {
        string GstrSysDate = "";
        string GstrSysTime = "";
        public frmSlipView2()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmSlipView2_Load(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            int i = 0;
            int j = 0;          

            string SQL = "";
            DataTable dt = null;
            DataTable dt2 = null;        

            ssSlipView2_Sheet1.Columns[7].Visible = false;
            ssSlipView2_Sheet1.Columns[8].Visible = false;

            //If GnJobSabun = 4349 Then
            //    Me.Caption = Me.Caption & "(전체)"
            //Else
            //    If GstrBuseGbn = "1" Then
            //        Me.Caption = Me.Caption & "(관리과)"
            //    ElseIf GstrBuseGbn = "2" Then
            //        Me.Caption = Me.Caption & "(공급실)"
            //    End If
            //End If

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    SLIPNO, ORDERCODE,GbGume,GbInfo,SUCODE, ORDERNAME, ItemCD,ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                //If GnJobSabun = 4349 Then
                //    SQL = SQL & " WHERE GBGUME IN ('1','2') "
                //ElseIf GstrBuseGbn = "1" Then '관리과
                //    SQL = SQL & " WHERE GbGume = '1' "
                //Else
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + " AND GbGume = '2' ";
                SQL = SQL + ComNum.VBLF + " AND SEQNO <>0    ";
                SQL = SQL + ComNum.VBLF + " AND SendDept <> 'N' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SLIPNO, ORDERCODE,GBGUME ";
                dt = clsDB.GetDataTable(SQL);

                ssSlipView2_Sheet1.RowCount = dt.Rows.Count;

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료가 등록되지 않았습니다.");
                    return;
                }               
            }

            catch(NoNullAllowedException ex)
            {

            }

            

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSlipView2_Sheet1.RowCount += 1;
                    ssSlipView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                    ssSlipView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                    ssSlipView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssSlipView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    if (dt.Rows[i]["GbGume"].ToString().Trim() == "1")
                    {
                        ssSlipView2_Sheet1.Cells[i, 4].Text = "관리";
                    }
                    else
                    {
                        ssSlipView2_Sheet1.Cells[i, 4].Text = "공급";
                    }
                    ssSlipView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ItemCd"].ToString().Trim();
                    ssSlipView2_Sheet1.Cells[i, 6].Text = READ_GumeName(dt.Rows[i]["ItemCd"].ToString().Trim());
                    ssSlipView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssSlipView2_Sheet1.Cells[i, 8].Text = "0";

                    if(dt.Rows[i]["GbInfo"].ToString().Trim() == "1")
                    {
                        try
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT";
                            SQL = SQL + ComNum.VBLF + "    SubName,SuCode,ItemCd,ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_SUBCODE";
                            SQL = SQL + ComNum.VBLF + "WHERE OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo ";
                            dt2 = clsDB.GetDataTable(SQL);

                        }

                        catch(NullReferenceException ex)
                        {

                        }

                        //ssSlipView2_Sheet1.Rows.Count = dt2.Rows.Count;
                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            ssSlipView2_Sheet1.Cells[j, 2].Text = dt2.Rows[j]["SUCODE"].ToString().Trim();
                            ssSlipView2_Sheet1.Cells[j, 3].Text = " ▶" + dt2.Rows[j]["SubName"].ToString().Trim();
                            if(dt.Rows[i]["GbGume"].ToString().Trim() == "1")
                            {
                                ssSlipView2_Sheet1.Cells[j, 4].Text = "관리";
                            }
                            else
                            {
                                ssSlipView2_Sheet1.Cells[j, 4].Text = "공급";
                            }
                            ssSlipView2_Sheet1.Cells[j, 5].Text = dt2.Rows[j]["ItemCd"].ToString().Trim();
                            ssSlipView2_Sheet1.Cells[j, 6].Text = READ_GumeName(dt2.Rows[j]["ItemCd"].ToString().Trim());
                            ssSlipView2_Sheet1.Cells[j, 7].Text = dt2.Rows[j]["ROWID"].ToString().Trim();
                            ssSlipView2_Sheet1.Cells[j, 8].Text = "S";
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        public string READ_GumeName(string str)
        {
            DataTable dt = null;
            string SQL = "";

            string rntVal = "";

            if(str == "")
            {
                return "";
            }

            //명칭을 READ
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CsrName,JepName,Unit";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
                SQL = SQL + ComNum.VBLF + "WHERE JepCode='" + str + "'";
                dt = clsDB.GetDataTable(SQL);
            }

            catch(NullReferenceException ex)
            {

            }
            rntVal = dt.Rows[0]["CsrName"].ToString().Trim();

            if (dt.Rows.Count > 0)
            {
                if (rntVal != "")
                {
                    return rntVal;
                }
                else
                {
                    rntVal = dt.Rows[0]["JepName"].ToString().Trim() + " " + dt.Rows[0]["Unit"].ToString().Trim();
                    return rntVal;
                }
            }
            else
                return "";

            dt.Dispose();
            dt = null;
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strHead1 = "";
            string strHead3 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/n/f1" + VB.Space(2) + " OCS전달 판넬" + "/n/n";

            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead3 = "/n/f2" + VB.Space(5) + " 출력시간 : " + GstrSysDate + GstrSysTime + VB.Space(10) + "Page : " + "/p";



            ssSlipView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSlipView2_Sheet1.PrintInfo.Margin.Top = 400;
            ssSlipView2_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssSlipView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSlipView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSlipView2_Sheet1.PrintInfo.ShowBorder = true;
            ssSlipView2_Sheet1.PrintInfo.ShowColor = true;
            ssSlipView2_Sheet1.PrintInfo.ShowGrid = false;
            ssSlipView2_Sheet1.PrintInfo.ShowShadows = true;
            ssSlipView2_Sheet1.PrintInfo.UseMax = false;
            ssSlipView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssSlipView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSlipView2.PrintSheet(0);
        }


    }
}
