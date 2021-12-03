using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 병실조회 </summary>
    public partial class frmViewHosil : Form
    {
        int intSearch = 0;
        string strRoomFlag = "";
        string[] strwardcode = null;
        int[,] intCntgwa = null;
        string[] strBis = null;

        /// <summary> 병실조회 </summary>
        public frmViewHosil()
        {
            InitializeComponent();
        }

        void frmViewHosil_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }

            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등


            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM BAS_WARD WHERE WardCode > ' ' ";

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

                ssByeongdong_Sheet1.RowCount = dt.Rows.Count;
                ssByeongdong_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                strwardcode = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssByeongdong_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WardName"].ToString().Trim();
                    strwardcode[i] = dt.Rows[i]["WardCode"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                DungAddList();

                //panTitleSub1.Visible = false;
                //panTitleSub2.Visible = false;
                //ssByeongdong.Visible = false;
                //ssHosil.Visible = false;
                panHosil.Visible = true;
                ssView2.Visible = false;
                panBed.Visible = false;

                ssView1.Dock = DockStyle.Fill;
                panHosil.Dock = DockStyle.Fill;

                SS1Resize();

                optSearch0.Checked = true;

                optSearch0.ForeColor = System.Drawing.Color.Blue;
                optSearch1.ForeColor = System.Drawing.Color.Gray;
                optSearch2.ForeColor = System.Drawing.Color.Gray;
                optSearch3.ForeColor = System.Drawing.Color.Gray;
                optSearch4.ForeColor = System.Drawing.Color.Gray;
                optSearch5.ForeColor = System.Drawing.Color.Gray;

                ssByeongdong.Focus();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //ssView1 Title변경
        void SS1Resize()
        {
            ssView1_Sheet1.ColumnCount = 11;
            ssView1_Sheet1.RowCount = 20;
            ssView1_Sheet1.Cells[0, 0, 19, 10].Text = "";
            ssView1_Sheet1.Columns[0, 10].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

            //Title 변경
            if (optSearch0.Checked == true || optSearch1.Checked == true || optSearch3.Checked == true)
            {
                ssView1_Sheet1.RowCount = 9;
                ssView1_Sheet1.Columns[0].Width = 95F;
                ssView1_Sheet1.Columns[0].Label = "호실Code";
                ssView1_Sheet1.Columns[1].Width = 70F;
                ssView1_Sheet1.Columns[1].Label = "과목";
                ssView1_Sheet1.Columns[2].Width = 60F;
                ssView1_Sheet1.Columns[2].Label = "성별";
                ssView1_Sheet1.Columns[3].Width = 80F;
                ssView1_Sheet1.Columns[3].Label = "총Bed";
                ssView1_Sheet1.Columns[4].Width = 80F;
                ssView1_Sheet1.Columns[4].Label = "현Bed";
                ssView1_Sheet1.Columns[5].Width = 80F;
                ssView1_Sheet1.Columns[5].Label = "빈Bed";
                ssView1_Sheet1.Columns[6].Width = 80F;
                ssView1_Sheet1.Columns[6].Label = "퇴원";
                ssView1_Sheet1.Columns[7].Width = 80F;
                ssView1_Sheet1.Columns[7].Label = "가퇴원";
                ssView1_Sheet1.Columns[8].Width = 100F;
                ssView1_Sheet1.Columns[8].Label = "입원료";
            }
            else if (optSearch2.Checked == true)
            {
                ssView1_Sheet1.RowCount = 10;
                ssView1_Sheet1.Columns[0].Width = 85F;
                ssView1_Sheet1.Columns[0].Label = "병록번호";
                ssView1_Sheet1.Columns[1].Width = 85F;
                ssView1_Sheet1.Columns[1].Label = "수진자명";
                ssView1_Sheet1.Columns[2].Width = 85F;
                ssView1_Sheet1.Columns[2].Label = "환자구분";
                ssView1_Sheet1.Columns[3].Width = 35F;
                ssView1_Sheet1.Columns[3].Label = "Sex";
                ssView1_Sheet1.Columns[4].Width = 35F;
                ssView1_Sheet1.Columns[4].Label = "Age";
                ssView1_Sheet1.Columns[5].Width = 85F;
                ssView1_Sheet1.Columns[5].Label = "보호자명";
                ssView1_Sheet1.Columns[6].Width = 85F;
                ssView1_Sheet1.Columns[6].Label = "입원일자";
                ssView1_Sheet1.Columns[7].Width = 45F;
                ssView1_Sheet1.Columns[7].Label = "과목";
                ssView1_Sheet1.Columns[8].Width = 95F;
                ssView1_Sheet1.Columns[8].Label = "주치의";
                ssView1_Sheet1.Columns[9].Width = 93F;
                ssView1_Sheet1.Columns[9].Label = "상태";
            }
            else if (optSearch4.Checked == true)
            {
                ssView1_Sheet1.RowCount = 7;
                ssView1_Sheet1.Columns[0].Width = 95F;
                ssView1_Sheet1.Columns[0].Label = "병동Code";
                ssView1_Sheet1.Columns[1].Width = 200F;
                ssView1_Sheet1.Columns[1].Label = "병동명";
                ssView1_Sheet1.Columns[2].Width = 85F;
                ssView1_Sheet1.Columns[2].Label = "총Bed수";
                ssView1_Sheet1.Columns[3].Width = 85F;
                ssView1_Sheet1.Columns[3].Label = "현Bed수";
                ssView1_Sheet1.Columns[4].Width = 85F;
                ssView1_Sheet1.Columns[4].Label = "퇴원수";
                ssView1_Sheet1.Columns[5].Width = 95F;
                ssView1_Sheet1.Columns[5].Label = "가퇴원수";
                ssView1_Sheet1.Columns[6].Width = 85F;
                ssView1_Sheet1.Columns[6].Label = "가동율";
            }
        }

        //ssHosil_Sheet1 인실 데이터 입력
        void DungAddList()
        {
            ssHosil_Sheet1.RowCount = 8;
            ssHosil_Sheet1.Cells[0, 0].Text = " S   특실";
            ssHosil_Sheet1.Cells[1, 0].Text = " 1   인실";
            ssHosil_Sheet1.Cells[2, 0].Text = " 2   인실";
            ssHosil_Sheet1.Cells[3, 0].Text = " 3   인실";
            ssHosil_Sheet1.Cells[4, 0].Text = " 4   인실";
            ssHosil_Sheet1.Cells[5, 0].Text = " 5   인실";
            ssHosil_Sheet1.Cells[6, 0].Text = " 6   인실";
            ssHosil_Sheet1.Cells[7, 0].Text = " 다  인실";
        }

        void Roomset()
        {
            switch (intSearch)
            {
                case 1:
                    Room1();
                    break;
                case 2:
                    Room2();
                    break;
            }
        }

        //ssHosil_Sheet1 인실 데이터 입력
        void Room1()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            string[] strClass = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TBed FROM BAS_ROOM GROUP BY Tbed ";

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

                ssHosil_Sheet1.RowCount = 0;
                ssHosil_Sheet1.Rows.Count = dt.Rows.Count + 1;
                ssHosil_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                strClass = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strClass[i] = dt.Rows[i]["TBed"].ToString().Trim();
                    ssHosil_Sheet1.Cells[i, 0].Text = strClass[i] + "인실";
                }

                dt.Dispose();
                dt = null;

                ssHosil_Sheet1.Cells[i, 0].Text = "특     실 ";

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        //ssHosil_Sheet1 인실 데이터 입력
        void Room2()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int i = 0;
            string[] strRoom = null;
            string[] strClass = null;

            try
            {
                strwardcode = new string[ssByeongdong_Sheet1.RowCount];

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT RoomCode,TBed FROM BAS_ROOM                        ";
                SQL = SQL + ComNum.VBLF + "  Where WardCode = '" + strwardcode[ssByeongdong_Sheet1.ActiveRowIndex] + "'  ";
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

                ssHosil_Sheet1.RowCount = 0;

                ssHosil_Sheet1.Rows.Count = dt.Rows.Count;
                ssHosil_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                strRoom = new string[dt.Rows.Count];
                strClass = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strRoom[i] = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strClass[i] = dt.Rows[i]["TBed"].ToString().Trim();

                    if (strClass[i] != "S")
                    {
                        ssHosil_Sheet1.Cells[i, 0].Text = strRoom[i] + "( " + strClass[i] + " 인실 )";
                    }
                    else
                    {
                        ssHosil_Sheet1.Cells[i, 0].Text = strRoom[i] + "( 특  실 )";
                    }
                }

                dt.Dispose();
                dt = null;

                if (ssHosil_Sheet1.RowCount != 0)
                {
                    ssHosilCellDoubleClick(0);
                }

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        //Visible, Dock
        void optSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == false)
            {
                return;
            }

            if (Convert.ToInt32(((RadioButton)sender).Name.Replace("optSearch", "")) == intSearch)
            {
                return;
            }

            SS1Resize();

            optSearch0.ForeColor = System.Drawing.Color.Gray;
            optSearch1.ForeColor = System.Drawing.Color.Gray;
            optSearch2.ForeColor = System.Drawing.Color.Gray;
            optSearch3.ForeColor = System.Drawing.Color.Gray;
            optSearch4.ForeColor = System.Drawing.Color.Gray;
            optSearch5.ForeColor = System.Drawing.Color.Gray;

            ((RadioButton)sender).ForeColor = System.Drawing.Color.Blue;
            
            intSearch = Convert.ToInt32(((RadioButton)sender).Name.Replace("optSearch", ""));

            if (intSearch != 5)
            {
                panTitleSub0.Visible = true;
                ssByeongdong_Sheet1.Visible = true;
            }
            else
            {
                panTitleSub0.Visible = false;
                ssByeongdong_Sheet1.Visible = false;
            }
           
            if (optSearch0.Checked == true)
            {
                ssByeongdong.Enabled = true;

                panHosil.Visible = true;
                panHosil.Dock = DockStyle.Fill;

                panBed.Visible = false;
                panBed.Dock = DockStyle.None;

                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;

                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                BedTotalStatus();

                //List2.Visible = False
                //List3.Visible = False
                //Panel(1).Visible = False
                //Panel(2).Visible = False
            }
            else if (optSearch1.Checked == true)
            {
                ssByeongdong.Enabled = true;

                panHosil.Visible = true;
                panHosil.Dock = DockStyle.Fill;

                panBed.Visible = false;
                panBed.Dock = DockStyle.None;

                lblTitleSub2.Text = "인  실";

                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;

                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                Roomset();

                BedTotalStatus();
            }
            else if (optSearch2.Checked == true)
            {
                ssByeongdong.Enabled = true;

                panHosil.Visible = true;
                panHosil.Dock = DockStyle.Fill;

                panBed.Visible = false;
                panBed.Dock = DockStyle.None;

                ssHosil_Sheet1.RowCount = 0;

                lblTitleSub2.Text = "호  실";

                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;

                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                BedTotalStatus();
            }
            else if (optSearch3.Checked == true)
            {
                ssByeongdong.Enabled = false;

                panHosil.Visible = false;
                panHosil.Dock = DockStyle.None;

                panBed.Visible = true;
                panBed.Dock = DockStyle.Fill;

                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;

                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                BedTotalStatus();
            }
            else if (optSearch4.Checked == true)
            {
                ssByeongdong.Enabled = false;

                panByeongdong.Visible = false;

                panHosil.Visible = false;
                panHosil.Dock = DockStyle.None;

                panBed.Visible = false;
                panBed.Dock = DockStyle.None;

                ssView1.Visible = true;
                ssView1.Dock = DockStyle.Fill;

                ssView2.Visible = false;
                ssView2.Dock = DockStyle.None;

                BedTotalStatus();
            }
            else if (optSearch5.Checked == true)
            {
                ssByeongdong.Enabled = false;

                panHosil.Visible = false;
                panHosil.Dock = DockStyle.None;

                panBed.Visible = false;
                panBed.Dock = DockStyle.None;

                panTitleSub2.Visible = false;
                panTitleSub1.Visible = false;

                ssView1.Visible = false;
                ssView1.Dock = DockStyle.None;

                ssView2.Visible = true;
                ssView2.Dock = DockStyle.Fill;

                BedTotalStatus();
            }

            
        }

        void ssBed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (ssBed_Sheet1.RowCount < 0)
            {
                return;
            }

            SSRoomClear();
            Bedstatus();
        }

        //ssView1_Sheet1 클리어
        void SSRoomClear()
        {
            ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].Text = "";
            ssView1_Sheet1.RowCount = 19;
        }

        //TODO: Screen.ActiveControl Is List1, numberCellType1
        void Bedstatus()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int k = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                if (optSearch0.Checked == true)
                {
                    if (ssByeongdong_Sheet1.ActiveRowIndex < 0)
                    {
                        return;
                    }
                }
                else if (optSearch3.Checked == true)
                {
                    if (ssBed_Sheet1.ActiveRowIndex < 0)
                    {
                        return;
                    }
                }
                else if (optSearch1.Checked == true)
                {
                    //if () //Screen.ActiveControl Is List1
                    //{
                    //    if (ssByeongdong_Sheet1.ActiveRowIndex < 0)
                    //    {
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    if (ssHosil_Sheet1.ActiveRowIndex < 0)
                    //    {
                    //        return;
                    //    }
                    //}

                }

                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT RoomCode,DeptCode,Sex,                                                     ";
                SQL = SQL + ComNum.VBLF + "        Tbed,Hbed,Bbed,Gbed,WardAmt                                                ";
                SQL = SQL + ComNum.VBLF + "   From Bas_Room                                                                   ";

                if (optSearch0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  Where WardCode = '" + strwardcode[ssByeongdong_Sheet1.ActiveRowIndex] + "'              ";
                }
                else if (optSearch3.Checked == true)
                {
                    k = ssBed_Sheet1.ActiveRowIndex;

                    if (k == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "  Where RoomClass = 'S'                                                ";
                    }
                    else if (k == 7)
                    {
                        SQL = SQL + ComNum.VBLF + "  Where Tbed >  6                                                     ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  Where Tbed = " + VB.Val(VB.Mid(ssBed_Sheet1.Cells[ssBed_Sheet1.ActiveRowIndex, 0].Text, 6, 1));
                    }

                }
                else if (optSearch1.Checked == true)
                {
                    //if () //Screen.ActiveControl Is List1
                    //{
                    //    SQL = SQL + ComNum.VBLF + "  Where WardCode = '" + strwardcode[ssByeongdong_Sheet1.ActiveRowIndex] + "'              ";
                    //}
                    //else
                    //{
                    //    if (ssHosil_Sheet1.ActiveRowIndex == ssHosil_Sheet1.RowCount)
                    //    {
                    //        SQL = SQL + ComNum.VBLF + "  Where RoomClass = 'S'                                            ";
                    //    }
                    //    else
                    //    {
                    //        SQL = SQL + ComNum.VBLF + "  Where Tbed = " + VB.Val(VB.Mid(ssHosil_Sheet1.Cells[ssHosil_Sheet1.ActiveRowIndex, 0].Text, 6, 3));
                    //    }

                    //    SQL = SQL + ComNum.VBLF + "    AND Tbed > Hbed                                                        ";
                    //}
                }

                SQL = SQL + ComNum.VBLF + "  Order By RoomCode                                                                ";
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

                if (ssView1_Sheet1.RowCount < dt.Rows.Count)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                }

                ssView1_Sheet1.Cells[0, 3, ssView1_Sheet1.RowCount - 1, 7].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;

                FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();

                numberCellType.DecimalPlaces = 0;
                numberCellType.MinimumValue = 0D;
                numberCellType.ShowSeparator = true;

                ssView1_Sheet1.Cells[0, 7, ssView1_Sheet1.RowCount - 1, 8].CellType = numberCellType;

                ssView1_Sheet1.Rows.Count = dt.Rows.Count;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView1_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["Tbed"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 4].Text = dt.Rows[i]["Hbed"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 5].Text = (Convert.ToInt32(VB.Val(dt.Rows[i]["Tbed"].ToString().Trim())) - Convert.ToInt32(VB.Val(dt.Rows[i]["Hbed"].ToString().Trim()))).ToString();
                    ssView1_Sheet1.Cells[i + 1, 6].Text = dt.Rows[i]["Bbed"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 7].Text = dt.Rows[i]["Gbed"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 8].Text = VB.Format(dt.Rows[i]["WardAmt"].ToString().Trim(), "###,###,##0");
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void ssByeongdong_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (optSearch2.Checked == true)
            {
                SSRoomClear();
                Roomset();
            }
        }

        void ssByeongdong_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SSRoomClear();

            if (optSearch0.Checked == true || optSearch1.Checked == true)
            {
                Bedstatus();
            }
        }

        void ssByeongdong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                return;
            }

            SSRoomClear();
            Bedstatus();
        }

        void ssHosil_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssHosilCellDoubleClick(e.Row);
        }

        private void ssHosilCellDoubleClick(int intRow)
        {
            if (ssHosil_Sheet1.RowCount < 1)
            {
                return;
            }

            SSRoomClear();

            if (optSearch1.Checked == true)
            {
                Bedstatus();
            }
            else if (optSearch2.Checked == true)
            {
                ListEvent(intRow);
            }
        }

        void ssHosil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (ssHosil_Sheet1.RowCount < 1)
            {
                return;
            }

            SSRoomClear();

            if (optSearch1.Checked == true)
            {
                Bedstatus();
            }
            else if (optSearch2.Checked == true)
            {
                ListEvent(ssHosil_Sheet1.ActiveRowIndex);
            }
        }

        //ssView1_Sheet1 데이터 입력
        void ListEvent(int intRow)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string strBi = "";
            string strDcdate = "";
            string strsysdate = "";

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Bi,Sname,Pname,Sex,Age,Dcdate,                                ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(InDate, 'yy-mm-dd') InDate,DeptCode,DrName,Amset1,Sysdate  ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_MASTER M,BAS_DOCTOR D                                          ";
                SQL = SQL + ComNum.VBLF + "  Where WardCode = '                                                       ";
                SQL = SQL + ComNum.VBLF + strwardcode[ssByeongdong_Sheet1.ActiveRowIndex] + "'                                           ";
                SQL = SQL + ComNum.VBLF + "    AND M.RoomCode = " + Convert.ToInt32(VB.Mid(ssHosil_Sheet1.Cells[intRow, 0].Text, 1, 4));
                SQL = SQL + ComNum.VBLF + "    AND Amset6 != '*'                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND M.DrCode = D.DrCode                                                ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Sname                                                           ";
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

                if (dt.Rows.Count > 20)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                }

                ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.Rows.Count - 1, ssView1_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                strBis = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strsysdate = dt.Rows[i]["Sysdate"].ToString().Trim();
                    strDcdate = dt.Rows[i]["Dcdate"].ToString().Trim();

                    ssView1_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 2].Text = strBis[Convert.ToInt32(VB.Val(strBi))];
                    ssView1_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 5].Text = dt.Rows[i]["Pname"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView1_Sheet1.Cells[i + 1, 8].Text = dt.Rows[i]["DrName"].ToString().Trim();

                    switch (dt.Rows[i]["Amset1"].ToString().Trim())
                    {
                        case "0":
                            ssView1_Sheet1.Cells[i + 1, 9].Text = "";
                            break;
                        case "1":
                            ssView1_Sheet1.Cells[i + 1, 9].Text = "수납";
                            break;
                        case "2":
                            ssView1_Sheet1.Cells[i + 1, 9].Text = "계산중";
                            break;
                        case "3":
                            if (VB.Val(strDcdate) < VB.Val(strsysdate))
                            {
                                ssView1_Sheet1.Cells[i + 1, 9].Text = "보관금퇴원";
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[i + 1, 9].Text = "가퇴원";
                            }
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void ssBed_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssBed_Sheet1.RowCount < 0)
            {
                return;
            }

            SSRoomClear();
            Bedstatus();
        }

        void optSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BedTotalStatus();
            }
        }

        //ssView1_Sheet1 데이터 입력
        void BedTotalStatus()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int intTbed = 0;
            int intHbed = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                if (optSearch4.Checked == true)
                {
                    if (strRoomFlag == "**")
                    {
                        return;
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Sum(Tbed) Num1,Sum(Hbed) Num2, ";
                    SQL = SQL + ComNum.VBLF + "        Sum(Bbed) Num3, Sum(Gbed) Num4 ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_ROOM                       ";
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
                        ssView1_Sheet1.Rows.Count = dt.Rows.Count;
                        ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        ssView1_Sheet1.Cells[0, 0].Text = "Total";
                        ssView1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Num1"].ToString().Trim();
                        intTbed = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[0, 2].Text));

                        ssView1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Num2"].ToString().Trim();
                        intHbed = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[0, 3].Text));

                        ssView1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["Num3"].ToString().Trim();
                        ssView1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["Num4"].ToString().Trim();
                        ssView1_Sheet1.Cells[0, 6].Text = VB.Format((intHbed / intTbed * 100), "###.#") + " %";

                        dt.Dispose();
                        dt = null;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT A.WardCode, WardName, Sum(Tbed) Num1,          ";
                    SQL = SQL + ComNum.VBLF + "        Sum(Hbed) Num2, Sum(Bbed) Num3, Sum(Gbed) Num4 ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_ROOM A, BAS_WARD B                         ";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.WardCode = B.WardCode                        ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.WardCode, WardName                        ";
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

                    if (ssView1_Sheet1.RowCount < dt.Rows.Count + 2)
                    {
                        ssView1_Sheet1.RowCount = dt.Rows.Count + 2;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i + 2, 0].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 2, 1].Text = dt.Rows[i]["WardName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 2, 2].Text = dt.Rows[i]["Num1"].ToString().Trim();
                        intTbed = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i + 2, 2].Text));
                        ssView1_Sheet1.Cells[i + 2, 3].Text = dt.Rows[i]["Num2"].ToString().Trim();
                        intHbed = Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i + 2, 3].Text));
                        ssView1_Sheet1.Cells[i + 2, 4].Text = dt.Rows[i]["Num3"].ToString().Trim();
                        ssView1_Sheet1.Cells[i + 2, 5].Text = dt.Rows[i]["Num4"].ToString().Trim();

                        if (intTbed != 0)
                        {
                            ssView1_Sheet1.Cells[i + 2, 6].Text = VB.Format((intHbed / intTbed * 100), "##0.0") + " %";
                        }
                    }

                    Application.DoEvents();

                    dt.Dispose();
                    dt = null;


                    ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.Rows.Count - 1, 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    ssView1_Sheet1.Cells[0, 2, ssView1_Sheet1.Rows.Count - 1, 6].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;

                    Cursor.Current = Cursors.Default;
                    strRoomFlag = "**";
                }
                else if (optSearch5.Checked == true)
                {
                    GwaBedTotalStatus();
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        //TODO: IPD_MASTER이 없음  >> ADMIN.IPD_NEW_MASTER로 바뀐거 같음
        void GwaBedTotalStatus()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int j = 0;
            int intCol = 0;
            int intRow = 0;
            int intsum = 0;
            string strwcode = "";
            string strtwcode = "";
            string strdcode = "";

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT wardcode,deptcode,count(pano) nhwp ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_MASTER                         ";
                SQL = SQL + ComNum.VBLF + "  WHERE Amset6 != '*'                      ";
                SQL = SQL + ComNum.VBLF + "    AND Amset1 = '0'                       ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY wardcode,deptcode               ";
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
                    SS2ClearProcess();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strwcode = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        strdcode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        intsum = Convert.ToInt32(dt.Rows[i]["NHWP"].ToString().Trim());

                        switch (strwcode)
                        {
                            case "2W":
                                intCol = 0;
                                break;
                            case "3A":
                                intCol = 1;
                                break;
                            case "4A":
                                intCol = 2;
                                break;
                            case "5W":
                                intCol = 3;
                                break;
                            case "6W":
                                intCol = 4;
                                break;
                            case "7W":
                                intCol = 5;
                                break;
                            case "8W":
                                intCol = 6;
                                break;
                            case "IU":
                                intCol = 7;
                                break;
                            case "DR":
                                intCol = 8;
                                break;
                            case "IQ":
                                intCol = 9;
                                break;
                            case "NR":
                                intCol = 10;
                                break;

                        }

                        switch (strdcode)
                        {
                            case "MD":
                                intRow = 0;
                                break;
                            case "GS":
                                intRow = 1;
                                break;
                            case "OG":
                                intRow = 2;
                                break;
                            case "PD":
                                intRow = 3;
                                break;
                            case "OS":
                                intRow = 4;
                                break;
                            case "NS":
                                intRow = 5;
                                break;
                            case "CS":
                                intRow = 6;
                                break;
                            //case "KK":
                            //    intRow = 7;
                            //    break;
                            case "NP":
                                intRow = 8;
                                break;
                            case "EN":
                                intRow = 9;
                                break;
                            case "OT":
                                intRow = 10;
                                break;
                            case "UR":
                                intRow = 11;
                                break;
                            case "DM":
                                intRow = 12;
                                break;
                        }

                        intCntgwa = new int[intRow, intCol];

                        intCntgwa[intRow, intCol] = intCntgwa[intRow, intCol] + intsum;
                        intCntgwa[intRow, 11] = intCntgwa[intRow, 11] + intsum;
                        intCntgwa[14, intCol] = intCntgwa[14, intCol] + intsum;
                        intCntgwa[14, 11] = intCntgwa[14, 11] + intsum;
                    }

                    dt.Dispose();
                    dt = null;
                }

                for (i = 0; i < 15; i++)
                {
                    for (j = 0; j < 12; j++)
                    {
                        ssView2_Sheet1.Cells[i + 1, j + 2].Text = VB.Format((intCntgwa[i, j]), "##0");
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.WardCode, Sum(Tbed) Num1 ";
                SQL = SQL + ComNum.VBLF + "   FROM BAS_ROOM A, BAS_WARD B     ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.WardCode = B.WardCode    ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY A.WardCode              ";
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
                    if (ssView2_Sheet1.RowCount < dt.Rows.Count + 1)
                    {
                        ssView2_Sheet1.RowCount = dt.Rows.Count + 1;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strtwcode = dt.Rows[i]["WardCode"].ToString().Trim();

                        switch (strtwcode)
                        {
                            case "2W":
                                intCol = 1;
                                break;
                            case "3A":
                                intCol = 2;
                                break;
                            case "4A":
                                intCol = 3;
                                break;
                            case "5W":
                                intCol = 4;
                                break;
                            case "6W":
                                intCol = 5;
                                break;
                            case "7W":
                                intCol = 6;
                                break;
                            case "8W":
                                intCol = 7;
                                break;
                            case "IU":
                                intCol = 8;
                                break;
                            case "DR":
                                intCol = 9;
                                break;
                            case "IQ":
                                intCol = 10;
                                break;
                            case "NR":
                                intCol = 11;
                                break;
                        }

                        ssView2_Sheet1.Cells[13, intCol].Text = VB.Format(dt.Rows[i]["Num1"].ToString().Trim(), "##0");
                    }

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sum(Tbed) Num1 FROM BAS_ROOM";
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


                ssView2_Sheet1.Cells[13, 12].Text = dt.Rows[0]["Num1"].ToString().Trim();

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        //ssView2_Sheet1 초기화
        void SS2ClearProcess()
        {
            int i = 0;
            int j = 0;

            //if (nCount > 0)
            //{
            ssView2_Sheet1.Cells[0, 1, 15, 12].Text = "";
            //}

            intCntgwa = new int[15, 12];

            for (i = 0; i < 15; i++)
            {
                for (j = 0; j < 12; j++)
                {
                    intCntgwa[i, j] = 0;
                }
            }
        }
    }
}