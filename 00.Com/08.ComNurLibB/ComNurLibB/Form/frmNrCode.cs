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

namespace ComNurLibB
{
    public partial class frmNrCode : Form
    {
        enum eNurCode  { Gubun = 0, Code, Name, PrintRanking, Jik, GubunOld, CodeOld, NameOld, PrintRankingOld, JikOld, ROWID }
        
        public frmNrCode()
        {            
            InitializeComponent();
        }

        void frmNrCode_Load(object sender, EventArgs e)
        {

            SPREADSET(ssNrCode_Sheet1);

            SetSpreadHeader("01");

            // 특정 칼럼을 숨긴다.
            int i;
            //for (i = 5; i < 9; i++)
            //{
            //    ssNrCode_Sheet1.Columns[i].Visible = false;
            //}

            //TODO : 안정수 조건결정

            //cboCode.Items.Add("A.주간당직");
            //cboCode.Items.Add("B.야간당직");

            cboCode.Items.Add("1.직책코드");
            cboCode.Items.Add("2.병동코드");
            cboCode.Items.Add("3.주사분류");
            cboCode.Items.Add("4.근무형태");
            cboCode.Items.Add("5.외래부서");
            cboCode.Items.Add("6.특수검사");
            cboCode.Items.Add("7.가동병상수");
            cboCode.Items.Add("7.특수,주사(파트별");

            //TODO : 안정수 BAS_BCODE 콤보체크 > Call Combo_BCode_SET(ComboJong, "NUR_검사항목", False, True)

            cboCode.SelectedIndex = 0;

            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            ssNrCode.Enabled = true;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            //cboCode.Focused = true;
        }

        public void SCREEN_CLEAR()
        {
            //모든 셀값을 클리어
            for(int i = 0; i < ssNrCode_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssNrCode_Sheet1.ColumnCount; j++)
                {
                    ssNrCode_Sheet1.Cells[i, j].Text = " ";
                }
            }           
            ssNrCode.Enabled = false;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        void Search()
        {
            int i = 0;
            DataTable dt = null;
            string strSql = string.Empty;
            ssNrCode_Sheet1.RowCount = 0;
            ssNrCode.Enabled = true;

            //SCREEN_CLEAR();
            btnSearch.Enabled = false;
            btnRegist.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = false;

            strSql = "";
            strSql = strSql + ComNum.VBLF + "SELECT";
            strSql = strSql + ComNum.VBLF + "    Code, Name, PrintRanking, ROWID ";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CODE";
            strSql = strSql + ComNum.VBLF + "WHERE Gubun = '" + VB.Left(cboCode.SelectedItem.ToString(), 1) + "'";
            strSql = strSql + ComNum.VBLF + " and Code ='24' ";
            strSql = strSql + ComNum.VBLF + "ORDER BY PrintRanking, Code";
            dt = clsDB.GetDataTable(strSql);

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssNrCode_Sheet1.RowCount += 1;

                //if (Spd.Cells[i, 0].Text == "True")
                //{
                //    Spd.Cells[i, 0].Text = "true";
                //}
                //else
                //    Spd.Cells[i, 0].Text = "false";

                ssNrCode_Sheet1.Cells[i, 0].Text = ""; //clear
                ssNrCode_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Code"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Name"].ToString().Trim();
                ssNrCode_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            ssNrCode_Sheet1.RowCount = ssNrCode_Sheet1.RowCount + 10;
        }

        void btnRegist_Click(object sender, EventArgs e)
        {
            Regist(ssNrCode_Sheet1);
        }

        void Regist(FarPoint.Win.Spread.SheetView Spd)
        {
            int i, j = 0;
            string strCode, strName = "";
            string strRowid = "";
            string strOldCode="", strOldName = "";
            string strDel = "";
            int nSeq=0, nOldSeq = 0;

            //TODO : 안정수 저장 일시 막음
            //MessageBox.Show("저장일시 막음");
            //return;
            
            DataTable dt = null;
            
            string strSql = string.Empty;            

            btnRegist.Enabled = false;
            btnCancel.Enabled = false;

            //for (i = 0; i < Spd.RowCount; i++)
             for (i = 0; i < Spd.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data + 1); i++)
            {
                strDel = Spd.Cells[i, 0].Text == "True" ? "1" : "0";
                strCode = Spd.Cells[i, 1].Text.Trim();
                strName = Spd.Cells[i, 2].Text.Trim();

                if (Spd.Cells[i, 3].Text != "")
                {
                    nSeq = Convert.ToInt16(Spd.Cells[i, 3].Text);
                }

                strRowid = Spd.Cells[i, 5].Text;
                strOldCode = Spd.Cells[i, 6].Text.Trim();
                strOldName = Spd.Cells[i, 7].Text.Trim();

                if (Spd.Cells[i, 8].Text != "")
                {
                    nOldSeq = Convert.ToInt16(Spd.Cells[i, 8].Text);
                }


                if (strCode.Trim() == "99")
                {
                    i = i;
                }

                // 기존데이터 삭제
                if (strDel == "1" && strRowid != "")
                {
                    clsDB.setBeginTran();

                    try
                    {
                        strSql = "";
                        strSql = strSql + ComNum.VBLF + " DELETE FROM ";
                        strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                        strSql = strSql + ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                        clsDB.ExecuteNonQuery(strSql);

                        clsDB.setCommitTran();
                        ComFunc.MsgBox("삭제하였습니다.");                        
                    }
                    catch(Exception e)
                    {
                        clsDB.setRollbackTran();
                        ComFunc.MsgBox("");
                    }

                }

                // 기존데이터 수정 or 신규등록
                else if(strDel != "1" && strCode != "")
                {
                    if(strCode != strOldCode || strName != strOldName || nSeq != nOldSeq)
                    {
                        if(strRowid == "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "INSERT INTO ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                                strSql = strSql + ComNum.VBLF + " (Gubun, Code, Name, PrintRanking) ";
                                strSql = strSql + ComNum.VBLF + " VALUES ('" + VB.Left(cboCode.Text,1) + "','"+ strCode +"','"+ strName +"', " + nSeq + " )";
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("등록하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox("");
                            }
                        }

                        if(strRowid != "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "UPDATE ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                                strSql = strSql + ComNum.VBLF + " SET Code = '" + strCode + "', ";
                                strSql = strSql + ComNum.VBLF + " Name = '" + strName + "', PrintRanking = " + nSeq + " ";
                                strSql = strSql + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("수정하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox("");
                            }
                        }
                    }
                }

            }

            SCREEN_CLEAR();

            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
        }

        private void ssNrCode_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strSunext = "";
            string first = "";
            string strSql = string.Empty;
            DataTable dt = null;

            //TODO : 안정수 기능 일시 막음
            return;

            for (int i = 0; i < ssNrCode_Sheet1.ColumnCount; i++)
            {
                if (i != 2)
                {
                    strSunext = ssNrCode_Sheet1.Cells[e.Row, 1].Text.Trim();
                }
            }

            if(strSunext == "")
            {
                first = VB.Left(cboCode.SelectedItem.ToString(), 1);

                if(first == "E" || first == "F" || first == "G" || first == "H" || first == "I")
                {
                    strSql = "";
                    strSql = strSql + ComNum.VBLF + "SELECT";
                    strSql = strSql + ComNum.VBLF + "    SUNAMEK";
                    strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                    strSql = strSql + ComNum.VBLF + "WHERE SUNEXT = '" + strSunext + "'";
                    dt = clsDB.GetDataTable(strSql);

                    if(dt == null)
                    {
                        ssNrCode_Sheet1.Cells[e.Row, 1].Text = "";
                    }
                    else
                        ssNrCode_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();

                }
            }
        }

        private void cboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSpreadHeader("01");
        }

        void SetSpreadHeader(string argGubun)
        {
            if (argGubun == "01")
            {
                ssNrCode_Sheet1.Columns[(int)eNurCode.Code].Visible = false;

                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "코드명칭";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "순위";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "비고";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ROWID";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "OLDCODE";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "OLDNAME";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "OLDORDER";
            }
            else if (argGubun == "02")
            {
                ssNrCode_Sheet1.Columns[(int)eNurCode.Code].Visible = true;
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "순번";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "사번";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "직급";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "직책명";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "ROWID";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "OLDSEQ";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "OLDCODE";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "OLDNAME";
                //ssNrCode_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "OLDJIK";

            }
        }

        void SPREADSET(FarPoint.Win.Spread.SheetView Spd, int colCnt = 0)
        {

            Spd.ColumnCount = Enum.GetValues(typeof(eNurCode)).Length;

            if (colCnt != 0) Spd.ColumnCount = colCnt;

            //enum eNurCode { Gubun = 0, Code, Name, PrintRanking, Jik, GubunOld, CodeOld, NameOld, PrintRankingOld, JikOld, ROWID }

            //Gubun(Checkbox)
            FarPoint.Win.Spread.CellType.CheckBoxCellType CheckBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Gubun).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Gubun).Value = "Gubun";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Gubun).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34;
            Spd.Columns.Get(0).CellType = CheckBoxCellType1;
            Spd.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(0).Label = "Gubun";
            Spd.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(0).Width = 80;


            //Code
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Code).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Code).Value = "Code";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Code).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(1).CellType = textCellType1;
            Spd.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(1).Label = "Code";
            Spd.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(1).Width = 100;
            //Spd.ColumnHeader.Rows.Get(1).Height = 34;

            
            // Name
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Name).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Name).Value = "Name";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Name).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.Name).CellType = textCellType2;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.Name)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(2).Label = "Name";
            Spd.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(2).Width = 50;

            // PrintRanking
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRanking).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRanking).Value = "PrintRanking";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRanking).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.PrintRanking).CellType = textCellType3;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.PrintRanking)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(3).Label = "PrintRanking";
            Spd.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(3).Width = 100;

            // Jik
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Jik).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Jik).Value = "Jik";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.Jik).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.Jik).CellType = textCellType4;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.Jik)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(4).Label = "Jik";
            Spd.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(4).Width = 50;

            // GubunOld
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.GubunOld).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.GubunOld).Value = "GubunOld";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.GubunOld).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.GubunOld).CellType = textCellType5;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.GubunOld)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(5).Label = "GubunOld";
            Spd.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(5).Width = 100;

            // CodeOld
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.CodeOld).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.CodeOld).Value = "CodeOld";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.CodeOld).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.CodeOld).CellType = textCellType6;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.CodeOld)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(6).Label = "CodeOld";
            Spd.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(6).Width = 100;

            // NameOld
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.NameOld).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.NameOld).Value = "NameOld";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.NameOld).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.NameOld).CellType = textCellType7;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.NameOld)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(7).Label = "NameOld";
            Spd.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(7).Width = 100;

            // PrintRankingOld
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRankingOld).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRankingOld).Value = "PrintRankingOld";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.PrintRankingOld).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.PrintRankingOld).CellType = textCellType8;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.PrintRankingOld)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(8).Label = "PrintRankingOld";
            Spd.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(8).Width = 100;

            // JikOld
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.JikOld).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.JikOld).Value = "JikOld";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.JikOld).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.JikOld).CellType = textCellType9;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.JikOld)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(9).Label = "JikOld";
            Spd.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(9).Width = 50;

            // ROWID
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.ROWID).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.ROWID).Value = "ROWID";
            Spd.ColumnHeader.Cells.Get(0, (int)eNurCode.ROWID).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.ColumnHeader.Rows.Get(0).Height = 34F;
            Spd.Columns.Get((int)eNurCode.ROWID).CellType = textCellType10;
            Spd.Columns.Get(Convert.ToInt32(eNurCode.ROWID)).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns.Get(10).Label = "ROWID";
            Spd.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns.Get(10).Width = 50;



        }

    }
}
