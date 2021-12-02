using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaGamCode.cs
/// Description     : 검진 청구 기준금액 등록 (공단검진, 암검진)
/// Author          : 김민철
/// Create Date     : 2020-02-20
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm공단청구기준금액등록1(Frm공단청구기준금액등록1.frm)" />
/// <seealso cref= "Frm공단청구기준금액등록2(Frm공단청구기준금액등록2.frm)" />
namespace HC_Bill
{
    public partial class frmHcMirAmtSet : Form
    {
        clsSpread cSpd = null;
        HicAmtBohumService hicAmtBohumService = null;
        HicAmtCancerService hicAmtCancerService = null;

        string FstrRowid = string.Empty;

        public frmHcMirAmtSet()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicAmtBohumService = new HicAmtBohumService();
            hicAmtCancerService = new HicAmtCancerService();
        }

        private void SetEvent()
        {
            this.Load                           += new EventHandler(eFormLoad);
            this.btnNew.Click                   += new EventHandler(eBtnClick);
            this.btnExit.Click                  += new EventHandler(eBtnClick);
            this.btnDelete.Click                += new EventHandler(eBtnClick);
            this.btnSave.Click                  += new EventHandler(eBtnClick);
            this.btnCancel.Click                += new EventHandler(eBtnClick);
            this.lstDate.DoubleClick            += new EventHandler(elstDblClick);
            this.lstDate2.DoubleClick           += new EventHandler(elstDblClick);
        }

        private void elstDblClick(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strDate = string.Empty;
            string strAmt = string.Empty;

            long nAmt = 0;

            if (sender == lstDate)
            {
                strDate = lstDate.SelectedItem.ToString();

                if (!strDate.IsNullOrEmpty())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT A.*, A.ROWID                          ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_AMT_BOHUM A ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                 ";
                    SQL = SQL + ComNum.VBLF + "   AND SDATE = '" + strDate + "'             ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    else
                    {
                        dtpSDate.Text = Dt.Rows[0]["SDate"].ToString();
                        FstrRowid = Dt.Rows[0]["ROWID"].ToString();
                        //1차
                        for (int i = 0; i < 25; i++)
                        {
                            nAmt = Dt.Rows[0]["One_Amt" + VB.Format(i + 1, "00")].ToString().To<long>();
                            SS1.ActiveSheet.Cells[i, 1].Text = nAmt.ToString();
                        }
                        //2차
                        for (int i = 0; i < 15; i++)
                        {
                            nAmt = Dt.Rows[0]["two_Amt" + VB.Format(i + 1, "00")].ToString().To<long>();
                            SS1.ActiveSheet.Cells[i, 4].Text = nAmt.ToString();
                        }
                        //구강
                        SS1.ActiveSheet.Cells[20, 4].Text = Dt.Rows[0]["ONE_Dent1"].ToString();
                        SS1.ActiveSheet.Cells[21, 4].Text = Dt.Rows[0]["ONE_Dent2"].ToString();
                    }
                }
            }
            else if (sender == lstDate2)
            {
                strDate = lstDate2.SelectedItem.ToString();

                if (!strDate.IsNullOrEmpty())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT A.*, A.ROWID                          ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_AMT_CANCER A ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                 ";
                    SQL = SQL + ComNum.VBLF + "   AND SDATE = '" + strDate + "'             ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    else
                    {
                        dtpSDate2.Text = Dt.Rows[0]["SDate"].ToString();
                        FstrRowid = Dt.Rows[0]["ROWID"].ToString();
                        
                        for (int i = 0; i < 29; i++)
                        {
                            strAmt = Dt.Rows[0]["Amt" + VB.Format(i + 1, "00")].ToString();
                            SS2.ActiveSheet.Cells[i, 1].Text = VB.Pstr(strAmt, ";", 1);
                            SS2.ActiveSheet.Cells[i, 2].Text = VB.Pstr(strAmt, ";", 2);
                            SS2.ActiveSheet.Cells[i, 3].Text = VB.Pstr(strAmt, ";", 3);
                        }
                    }
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnDelete)
            {
                Data_Delete();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnNew)
            {
                Screen_Clear();
                dtpSDate.Text = DateTime.Now.ToShortDateString();
                dtpSDate2.Text = DateTime.Now.ToShortDateString();

                if (superTabControl1.SelectedTab == superTabItem1)
                {
                    dtpSDate.Focus();
                }
                else
                {
                    dtpSDate2.Focus();
                }
                
            }
        }

        private void Data_Delete()
        {
            if (FstrRowid.IsNullOrEmpty()) { return; }

            if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            if (superTabControl1.SelectedTab == superTabItem1)
            {
                int result = hicAmtBohumService.DeleteByRowid(FstrRowid);

                if (result <= 0)
                {
                    MessageBox.Show("삭제실패", "작업실패");
                    return;
                }
            }
            else
            {
                int result = hicAmtCancerService.DeleteByRowid(FstrRowid);

                if (result <= 0)
                {
                    MessageBox.Show("삭제실패", "작업실패");
                    return;
                }
            }

            Screen_Clear();

            MessageBox.Show("삭제하였습니다.", "작업완료");
        }

        private void Data_Save()
        {
            int intRowAffected = 0;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string[] strAmt = new string[29];
            StringBuilder strTemp = new StringBuilder();

            if (superTabControl1.SelectedTab == superTabItem1)
            {
                long[] nOneAmt = new long[25];
                long[] nTwoAmt = new long[25];
                long nDent1 = 0, nDent2 = 0;

                //공단검진
                for (int i = 0; i < 25; i++)
                {
                    nOneAmt[i] = SS1.ActiveSheet.Cells[i, 1].ToString().To<long>();
                }

                for (int i = 0; i < 15; i++)
                {
                    nTwoAmt[i] = SS1.ActiveSheet.Cells[i, 4].ToString().To<long>();
                }

                nDent1 = SS1.ActiveSheet.Cells[20, 4].ToString().To<long>();
                nDent2 = SS1.ActiveSheet.Cells[21, 4].ToString().To<long>();

                if (FstrRowid.IsNullOrEmpty())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "HIC_AMT_BOHUM  ( ";
                    SQL = SQL + ComNum.VBLF + "     SDATE,ONE_AMT01,ONE_AMT02,ONE_AMT03,ONE_AMT04,ONE_AMT05,";
                    SQL = SQL + ComNum.VBLF + "    ,ONE_AMT06,ONE_AMT07,ONE_AMT08,ONE_AMT09,ONE_AMT10,ONE_AMT11,ONE_AMT12,ONE_AMT13,ONE_AMT14,ONE_AMT15,";
                    SQL = SQL + ComNum.VBLF + "    ,ONE_AMT16,ONE_AMT17,ONE_AMT18,ONE_AMT19,ONE_AMT20,ONE_AMT21,ONE_AMT22,ONE_AMT23,ONE_AMT24,ONE_AMT25,";
                    SQL = SQL + ComNum.VBLF + "    ,TWO_AMT01,TWO_AMT02,TWO_AMT03,TWO_AMT04,TWO_AMT05,TWO_AMT06,TWO_AMT07,TWO_AMT08,TWO_AMT09,TWO_AMT10,";
                    SQL = SQL + ComNum.VBLF + "    ,TWO_AMT11,TWO_AMT12,TWO_AMT13,TWO_AMT14,TWO_AMT15,ONE_DENT1,ONE_DENT2   )";
                    SQL = SQL + ComNum.VBLF + "   VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "   '" + dtpSDate.Text + "' ";
                    for (int i = 0; i < 25; i++) { SQL = SQL + ComNum.VBLF + "," + nOneAmt[i]; }
                    for (int i = 0; i < 15; i++) { SQL = SQL + ComNum.VBLF + "," + nTwoAmt[i]; }
                    SQL = SQL + ComNum.VBLF + " ," + nDent1 + "," + nDent2 + " ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "HIC_AMT_BOHUM   ";
                    SQL = SQL + ComNum.VBLF + "   SET SDATE = '" + dtpSDate.Text + "' ";
                    for (int i = 0; i < 25; i++) { SQL = SQL + ComNum.VBLF + ", ONE_AMT" + VB.Format(i + 1, "00") + " = " + nOneAmt[i]; }
                    for (int i = 0; i < 15; i++) { SQL = SQL + ComNum.VBLF + ", TWO_AMT" + VB.Format(i + 1, "00") + " = " + nTwoAmt[i]; }
                    SQL = SQL + ComNum.VBLF + " , ONE_DENT1 =" + nDent1.ToString() + " ";
                    SQL = SQL + ComNum.VBLF + " , ONE_DENT2 =" + nDent2.ToString() + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrRowid + "' ";
                }
            }
            else
            {
                //암검진
                for (int i = 0; i < 29; i++)
                {
                    strTemp.Clear();
                    strTemp.Append(SS2.ActiveSheet.Cells[i, 1].Text.Trim());
                    strTemp.Append(";");
                    strTemp.Append(SS2.ActiveSheet.Cells[i, 2].Text.Trim());
                    strTemp.Append(";");
                    strTemp.Append(SS2.ActiveSheet.Cells[i, 3].Text.Trim());
                    strTemp.Append(";");
                    strAmt[i] = strTemp.ToString();
                }

                if (FstrRowid.IsNullOrEmpty())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "HIC_AMT_CANCER  (                      ";
                    SQL = SQL + ComNum.VBLF + "     SDATE,AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,  ";
                    SQL = SQL + ComNum.VBLF + "     AMT11,AMT12,AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,        ";
                    SQL = SQL + ComNum.VBLF + "     AMT21,AMT22,AMT23,AMT24,AMT25,AMT26,AMT27    )                      ";
                    SQL = SQL + ComNum.VBLF + " VALUES (                                                                ";
                    SQL = SQL + ComNum.VBLF + "   '" + dtpSDate.Text + "' ";
                    for (int i = 0; i < 29; i++) { SQL = SQL + ComNum.VBLF + "," + strAmt[i]; }
                    SQL = SQL + ComNum.VBLF + " ) ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "HIC_AMT_CANCER   ";
                    SQL = SQL + ComNum.VBLF + "   SET SDATE = '" + dtpSDate.Text + "' ";
                    for (int i = 0; i < 29; i++) { SQL = SQL + ComNum.VBLF + ", AMT" + VB.Format(i + 1, "00") + " = '" + strAmt[i] + "' "; }
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrRowid + "' ";
                }
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void Screen_Clear()
        {
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                SS1.ActiveSheet.Cells[0, 1].Text = "";
                SS1.ActiveSheet.Cells[0, 4].Text = "";
            }

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                SS2.ActiveSheet.Cells[0, 1].Text = "";
                SS2.ActiveSheet.Cells[0, 2].Text = "";
                SS2.ActiveSheet.Cells[0, 3].Text = "";
            }

            IList<HIC_AMT_BOHUM> list = hicAmtBohumService.FindAll();

            lstDate.Items.Clear();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    lstDate.Items.Add(list[i].SDATE);
                }
            }

            IList<HIC_AMT_CANCER> list2 = hicAmtCancerService.FindAll();

            lstDate2.Items.Clear();
            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    lstDate2.Items.Add(list2[i].SDATE);
                }
            }
        }
    }
}
