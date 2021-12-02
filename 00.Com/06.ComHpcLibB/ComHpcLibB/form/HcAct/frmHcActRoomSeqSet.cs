using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
//using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcActRoomSeqSet.cs
/// Description     : 계측항목 순번 관리
/// Author          : 이상훈
/// Create Date     : 2020-10-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "신규" />

namespace ComHpcLibB
{
    public partial class frmHcActRoomSeqSet : Form
    {
        HicCodeService hicCodeService = null;
        HeaCodeService heaCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcActRoomSeqSet()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            heaCodeService = new HeaCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.rdoGubun1.Click += new EventHandler(eRdoClick);
            this.rdoGubun2.Click += new EventHandler(eRdoClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(ssList);
            ssList_Sheet1.SetRowHeight(-1, 22);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string sGubun = "";

                if (rdoGubun1.Checked == true)      //일반검진
                {
                    sGubun = "1";

                    List<HIC_CODE> listCode = hicCodeService.Hic_Part_Jepsu("72", "SORT"); //일검
                    ssList.ActiveSheet.RowCount = listCode.Count;
                    ssList_Sheet1.SetRowHeight(-1, 22);

                    if (listCode.Count > 0)
                    {
                        for (int i = 0; i < listCode.Count; i++)
                        {
                            ssList.ActiveSheet.Cells[i, 0].Text = listCode[i].CODE.Trim();
                            ssList.ActiveSheet.Cells[i, 1].Text = listCode[i].NAME;
                            ssList.ActiveSheet.Cells[i, 2].Text = listCode[i].SORT.To<string>();
                            ssList.ActiveSheet.Cells[i, 3].Text = listCode[i].ROWID;
                        }
                    }
                }
                else if (rdoGubun2.Checked == true) //종합검진
                {
                    sGubun = "2";
                    List<HEA_CODE> listCode = heaCodeService.Hea_Part_Jepsu("11"); //종검
                    ssList.ActiveSheet.RowCount = listCode.Count;
                    ssList_Sheet1.SetRowHeight(-1, 22);

                    if (listCode.Count > 0)
                    {
                        for (int i = 0; i < listCode.Count; i++)
                        {
                            ssList.ActiveSheet.Cells[i, 0].Text = listCode[i].CODE.Trim();
                            ssList.ActiveSheet.Cells[i, 1].Text = listCode[i].NAME;
                            ssList.ActiveSheet.Cells[i, 2].Text = listCode[i].SORT.To<string>();
                            ssList.ActiveSheet.Cells[i, 3].Text = listCode[i].RID;
                        }
                    }
                }
            }
            else if (sender == btnSave)
            {
                int result = 0;

                if (rdoGubun1.Checked == true)      //일반검진
                {
                    HIC_CODE item = new HIC_CODE();

                    if (ssList.ActiveSheet.RowCount > 0)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                        {
                            if (ssList.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show(i + 1 + " 행 순서가 공란입니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);                                
                                ssList.ActiveSheet.SetActiveCell(i, 2);
                                return;
                            }

                            item.SORT = ssList.ActiveSheet.Cells[i, 2].Text.To<decimal>();
                            item.ROWID = ssList.ActiveSheet.Cells[i, 3].Text.Trim();

                            result = hicCodeService.UpdateSortbyRowId(item.SORT.To<string>(), item.ROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("일반검진 계측항목 순서 저장 중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        MessageBox.Show("저장 되었습니다!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (rdoGubun2.Checked)
                {
                    HEA_CODE item = new HEA_CODE();

                    if (ssList.ActiveSheet.RowCount > 0)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                        {
                            if (ssList.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show(i + 1 + " 행 순서가 공란입니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ssList.ActiveSheet.SetActiveCell(i, 2);
                                return;
                            }

                            item.SORT = ssList.ActiveSheet.Cells[i, 2].Text.Trim().To<decimal>();
                            item.RID = ssList.ActiveSheet.Cells[i, 3].Text.Trim();

                            result = heaCodeService.UpdateSortbyRowId(item.SORT.To<string>(), item.RID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("종합검진 계측항목 순서 저장 중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        MessageBox.Show("저장 되었습니다!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                eBtnClick(btnExit, new EventArgs());
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }
    }
}
