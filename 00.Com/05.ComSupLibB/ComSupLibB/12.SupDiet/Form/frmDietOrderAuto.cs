using ComBase;
using System;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using System.Drawing;

namespace ComSupLibB
{

    /// <summary>
    /// Class Name      : ComSupLibB.SupDiet
    /// File Name       : 
    /// Description     : 
    /// Author          : 김경동
    /// Create Date     : 2019-01-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\diet\dietorder\FrmDietorder밥" >> frmDietOrderAuto.cs 폼이름 재정의" />
    public partial class frmDietOrderAuto : Form
    {
        FpSpread spd;

        public frmDietOrderAuto()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmDietOrderAuto(FpSpread ArgSpd)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            spd = ArgSpd;
        }

        void SetControl()
        {
            clsSpread cSPD = new clsSpread();
            clsSupDiet cSD = new clsSupDiet();
            
            cSPD.Spread_All_Clear(SS1);
            cSD.sSpd_enmHcGroup(SS1, cSD.sDietAll, cSD.nDietAll, 30, 0);

            cSPD = null;
            cSD = null;
        }

        void SetEvent()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnExit.Click  += new EventHandler(eBtnClick);
            this.btnAll.Click   += new EventHandler(eBtnClick);
            this.btnSave.Click  += new EventHandler(eBtnClick);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnAll)
            {
                Set_Check_All();
            }
            else if (sender == btnSave)
            {
                eSave(clsDB.DbCon);
            }
        }

        void eSave(PsmhDb pDbCon)
        {
            string strWardCode = string.Empty;
            string strDate = string.Empty;
            string strDIETCODE = string.Empty;
            string strDietName = string.Empty;
            string strSucode = string.Empty;
            string strBun = string.Empty;
            string strDietDay = string.Empty;
            string strROOMCODE = string.Empty;
            string strGBSUNAP  = string.Empty;
            string strPANO     = string.Empty;
            string strDeptCode = string.Empty;
            string strBi       = string.Empty;
            string strSNAME    = string.Empty;
            string strDrCode   = string.Empty;
            string strIPDNO = string.Empty;
            string strUnit = string.Empty;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            clsSupDiet cSD = new clsSupDiet();
            
            ComFunc.ReadSysDate(pDbCon);

            if (string.Compare(clsPublic.GstrSysTime,  "11:30") >= 0 && clsType.User.IdNumber != "444")
            {
                MessageBox.Show("지금 시간에는 식이변경이 불가능합니다.", "확인");
                return;
            }

            if (cSD.GET_DIETPRT_LOCK(pDbCon) == true)
            {
                MessageBox.Show("영양실에서 식표인쇄 중입니다. 3분후에 작업하시기 바랍니다.", "확인");
                return;
            }

            if (ComFunc.MsgBoxQ("일괄입력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                strWardCode = "HD";
                strDate = clsPublic.GstrSysDate;
                strDIETCODE = "10";
                strDietName = "밥";
                strSucode = "FD01";
                strBun = "01";
                strDietDay = "2";
                strUnit = "";

                for (j = 0; j < SS1.ActiveSheet.RowCount; j++)
                {
                    if (SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.AFT].Text == "True" &&
                        SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.SNAME].Text.Trim() != "")
                    {
                        strROOMCODE = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.ROOM].Text.Trim();
                        strGBSUNAP  = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.OUTC].Text.Trim();
                        strPANO     = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.PTNO].Text.Trim();
                        strDeptCode = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.DEPT].Text.Trim();
                        strBi       = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.BI].Text.Trim();
                        strSNAME    = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.SNAME2].Text.Trim();
                        strDrCode   = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.DRCODE].Text.Trim();
                        strIPDNO    = SS1.ActiveSheet.Cells[j, (int)clsSupDiet.enmDietAll.IPDNO].Text.Trim();

                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NEWORDER(ACTDATE, PANO, BI, DEPTCODE, DRCODE, ";
                        SQL += " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY, ";
                        SQL += " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT )";
                        SQL += " VALUES (TO_DATE('" + strDate + "', 'YYYY-MM-DD') , '" + strPANO + "', '" + strBi + "',";
                        SQL += "         '" + strDeptCode + "', '" + strDrCode + "', '" + strWardCode + "', '" + strROOMCODE + "', ";
                        SQL += "         '" + strDIETCODE + "' , '" + strDietName + "' ,'" + strSucode + "' , '" + strDietDay + "', '1', ";
                        SQL += "         '" + strUnit + "', '" + strBun + "', SYSDATE,";
                        SQL += "         '" + clsType.User.IdNumber + "', '" + strGBSUNAP + "','' ) ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    
                }

                //clsDB.setRollbackTran(pDbCon);
                clsDB.setCommitTran(pDbCon);

                MessageBox.Show("일괄 등록 완료 되었습니다.", "확인");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        void Set_Check_All()
        {
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                SS1.ActiveSheet.Cells[i, (int)clsSupDiet.enmDietAll.AFT].Value = "True";
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (spd != null)
            {
                READ_MOTHERSHEET();
            }
            
        }

        void READ_MOTHERSHEET()
        {
            clsSpread cSPD = new clsSpread();

            SS1.ActiveSheet.RowCount = spd.ActiveSheet.RowCount;

            spd.Sheets[0].ClipboardCopy(new CellRange(0, 0, spd.Sheets[0].NonEmptyRowCount , spd.Sheets[0].NonEmptyColumnCount - 1), ClipboardCopyOptions.All);
            SS1.Sheets[0].ClipboardPaste(ClipboardPasteOptions.All);

            cSPD.setColStyle(SS1, -1, -1, clsSpread.enmSpdType.Label);
            cSPD.setColStyle(SS1, -1, (int)clsSupDiet.enmDietAll.AFT, clsSpread.enmSpdType.CheckBox);

            cSPD.SetfpsRowHeight(SS1, 38);
            

            cSPD = null;
        }
        
    }
}
