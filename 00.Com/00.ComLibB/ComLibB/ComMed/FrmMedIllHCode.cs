using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedIllHCode.cs
    /// Description     : 희귀난치성 질환자 산정특례 등록 기준
    /// Author          : 안정수
    /// Create Date     : 2017-11-23
    /// Update History  : 
    /// <history>       
    /// d:\psmh\FrmILLHCode.frm(FrmIllHCode) => FrmMedIllHCode.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\FrmILLHCode.frm(FrmIllHCode) => FrmMedIllHCode.cs
    /// </seealso>
    /// </summary>
    /// //2019-09-05 사용안합니다. 구 기준입니다.
    public partial class FrmMedIllHCode : Form
    {
        clsSpread CS = new clsSpread();
        string FstrROWID = "";

        public FrmMedIllHCode()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);

            this.txtIllCode_S.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.optGubun0.Click  += new EventHandler(eControl_Checked);
            this.optGubun1.Click += new EventHandler(eControl_Checked);

            #region 상병코드 알파벳 이벤트처리
            this.btnA.Click += new EventHandler(eBtn_Index);
            this.btnB.Click += new EventHandler(eBtn_Index);
            this.btnC.Click += new EventHandler(eBtn_Index);
            this.btnD.Click += new EventHandler(eBtn_Index);
            this.btnE.Click += new EventHandler(eBtn_Index);
            this.btnF.Click += new EventHandler(eBtn_Index);
            this.btnG.Click += new EventHandler(eBtn_Index);
            this.btnH.Click += new EventHandler(eBtn_Index);
            this.btnI.Click += new EventHandler(eBtn_Index);
            this.btnJ.Click += new EventHandler(eBtn_Index);
            this.btnK.Click += new EventHandler(eBtn_Index);
            this.btnL.Click += new EventHandler(eBtn_Index);
            this.btnM.Click += new EventHandler(eBtn_Index);
            this.btnN.Click += new EventHandler(eBtn_Index);
            this.btnO.Click += new EventHandler(eBtn_Index);
            this.btnP.Click += new EventHandler(eBtn_Index);
            this.btnQ.Click += new EventHandler(eBtn_Index);
            this.btnR.Click += new EventHandler(eBtn_Index);
            this.btnS.Click += new EventHandler(eBtn_Index);
            this.btnT.Click += new EventHandler(eBtn_Index);
            this.btnU.Click += new EventHandler(eBtn_Index);
            this.btnV.Click += new EventHandler(eBtn_Index);
            this.btnW.Click += new EventHandler(eBtn_Index);
            this.btnX.Click += new EventHandler(eBtn_Index);
            this.btnY.Click += new EventHandler(eBtn_Index);
            this.btnZ.Click += new EventHandler(eBtn_Index);
            this.btnAll.Click += new EventHandler(eBtn_Index);
            #endregion

        }

        void eControl_Checked(object sender, EventArgs e)
        {
            if (sender == this.optGubun0)
            {
                eBtn_Index(btnA, e);
            }

            else
            {
                eBtn_Index(btnA, e);
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtIllCode_S)
            {
                if (e.KeyChar == 13)
                {
                    btnView.Focus();
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
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnView)
            {
                //                

                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSave();
            }
        }

        void eBtn_Index(object sender, EventArgs e)
        {
            string strIllCode = "";
            int Index = 0;

            #region 버튼 인덱스 설정

            if (sender == btnA)
            {
                strIllCode = btnA.Text + "%";
                Index = 0;
            }

            else if (sender == btnB)
            {
                strIllCode = btnB.Text + "%";
                Index = 1;
            }

            else if (sender == btnC)
            {
                strIllCode = btnC.Text + "%";
                Index = 2;
            }

            else if (sender == btnD)
            {
                strIllCode = btnD.Text + "%";
                Index = 3;
            }

            else if (sender == btnE)
            {
                strIllCode = btnE.Text + "%";
                Index = 4;
            }

            else if (sender == btnF)
            {
                strIllCode = btnF.Text + "%";
                Index = 5;
            }

            else if (sender == btnG)
            {
                strIllCode = btnG.Text + "%";
                Index = 6;
            }

            else if (sender == btnH)
            {
                strIllCode = btnH.Text + "%";
                Index = 7;
            }

            else if (sender == btnI)
            {
                strIllCode = btnI.Text + "%";
                Index = 8;
            }

            else if (sender == btnJ)
            {
                strIllCode = btnJ.Text + "%";
                Index = 9;
            }

            else if (sender == btnK)
            {
                strIllCode = btnK.Text + "%";
                Index = 10;
            }

            else if (sender == btnL)
            {
                strIllCode = btnL.Text + "%";
                Index = 11;
            }

            else if (sender == btnM)
            {
                strIllCode = btnM.Text + "%";
                Index = 12;
            }

            else if (sender == btnN)
            {
                strIllCode = btnN.Text + "%";
                Index = 13;
            }

            else if (sender == btnO)
            {
                strIllCode = btnO.Text + "%";
                Index = 14;
            }

            else if (sender == btnP)
            {
                strIllCode = btnP.Text + "%";
                Index = 15;
            }

            else if (sender == btnQ)
            {
                strIllCode = btnQ.Text + "%";
                Index = 16;
            }

            else if (sender == btnR)
            {
                strIllCode = btnR.Text + "%";
                Index = 17;
            }

            else if (sender == btnS)
            {
                strIllCode = btnS.Text + "%";
                Index = 18;
            }

            else if (sender == btnT)
            {
                strIllCode = btnT.Text + "%";
                Index = 19;
            }

            else if (sender == btnU)
            {
                strIllCode = btnU.Text + "%";
                Index = 20;
            }

            else if (sender == btnV)
            {
                strIllCode = btnV.Text + "%";
                Index = 21;
            }

            else if (sender == btnW)
            {
                strIllCode = btnW.Text + "%";
                Index = 22;
            }

            else if (sender == btnX)
            {
                strIllCode = btnX.Text + "%";
                Index = 23;
            }

            else if (sender == btnY)
            {
                strIllCode = btnY.Text + "%";
                Index = 23;
            }

            else if (sender == btnZ)
            {
                strIllCode = btnZ.Text + "%";
                Index = 25;
            }

            else if (sender == btnAll)
            {
                strIllCode = btnAll.Text + "%";
                Index = 26;
            }

            #endregion

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList1.ActiveSheet.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  ILLCODE, ILLNAMEK, ROWID ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS_H";
            SQL += ComNum.VBLF + "WHERE 1=1";

            if (Index != 26)
            {
                SQL += ComNum.VBLF + "      AND ILLCODE LIKE  ('" + strIllCode + "' ) ";

                if(optGubun0.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND Gubun ='3' ";
                }

                else if(optGubun1.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND Gubun ='4' ";
                }
            }

            else
            {
                if(optGubun0.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '3'";
                }

                else if(optGubun1.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND Gubun = '4'";
                }
            }
            SQL += ComNum.VBLF + "ORDER BY ILLCODE ";

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
                    ssList1.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    ssList1.ActiveSheet.Columns[2].Visible = false;
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

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optGubun0.Checked = true;

            SCREEN_CLEAR();

            eBtn_Index(btnA, e);

            //If UCase(App.EXEName) = "BUSUGA" Then

            //   CmdSave.Visible = True

            //Else
            //   CmdSave.Visible = False
            //End If

        }

        void SCREEN_CLEAR()
        {
            txtIllCode.Text = "";
            txtIllNameK.Text = "";
            txtIllNameE.Text = "";
            txtGiJun.Text = "";
            txtGiJun2.Text = "";

            for (int i = 0; i < ssGB.ActiveSheet.Rows.Count; i++)
            {
                ssGB.ActiveSheet.Cells[i, 1].Text = "";
            }

            for(int j = 0; j < ssGB2.ActiveSheet.Rows.Count; j++)
            {
                ssGB2.ActiveSheet.Cells[j, 1].Text = "";
            }

            txtHak1.Text = "";
            txtHak2.Text = "";
            txtHak3.Text = "";
            txtHak4.Text = "";

            txtVCode.Text = "";
        }

        void eSave()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_ILLS_H SET";
            SQL += ComNum.VBLF + "GIJUN = '" + txtGiJun.Text+ "',   ";
            SQL += ComNum.VBLF + "GIJUN2 = '" + txtGiJun2.Text + "' ";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ROWID = '" + FstrROWID + "' ";            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            try
            {
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;

            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                
            }
        }

        void eGetData()
        {
            int i = 0;

            if(txtIllCode_S.Text == "")
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            ssList1.ActiveSheet.Rows.Count = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ILLCODE, ILLNAMEK, ROWID ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS_H";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "AND ILLCODE = '" + txtIllCode_S.Text + "' ";

            if(optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Gubun = '3' ";  //2014변경
            }

            else if(optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Gubun = '4' ";  //2014변경
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
                    ssList1.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FstrROWID = ssList1.ActiveSheet.Cells[e.Row, 2].Text;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ILLCODE, ILLNAMEK, ILLNAMEE,VCode, ";
            SQL += ComNum.VBLF + "  GIJUN, GBN1, GBN2, GBN3, GBN4, GBN5, GBN6,GBN7, HAK1, HAK2, HAK3, HAK4,";
            SQL += ComNum.VBLF + "  GIJUN2, GBN21, GBN22, GBN23, GBN24, GBN25, GBN26,GBN27 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS_H";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ROWID = '" + FstrROWID + "'";

            if(optGubun0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Gubun ='3'  ";
            }

            else if(optGubun1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND Gubun ='4' ";
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
                    txtIllCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    txtIllNameK.Text = dt.Rows[0]["IllNameK"].ToString().Trim();
                    txtIllNameE.Text = dt.Rows[0]["IllNameE"].ToString().Trim();
                    txtGiJun.Text = dt.Rows[0]["GIJUN"].ToString().Trim();

                    ssGB.ActiveSheet.Cells[0, 1].Text = dt.Rows[0]["GBN1"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[1, 1].Text = dt.Rows[0]["GBN2"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[2, 1].Text = dt.Rows[0]["GBN3"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[3, 1].Text = dt.Rows[0]["GBN4"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[4, 1].Text = dt.Rows[0]["GBN5"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[5, 1].Text = dt.Rows[0]["GBN6"].ToString().Trim();
                    ssGB.ActiveSheet.Cells[6, 1].Text = dt.Rows[0]["GBN7"].ToString().Trim();


                    //2014-11-06 add
                    txtGiJun2.Text = dt.Rows[0]["GIJUN2"].ToString().Trim();

                    ssGB2.ActiveSheet.Cells[0, 1].Text = dt.Rows[0]["GBN21"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[1, 1].Text = dt.Rows[0]["GBN22"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[2, 1].Text = dt.Rows[0]["GBN23"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[3, 1].Text = dt.Rows[0]["GBN24"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[4, 1].Text = dt.Rows[0]["GBN25"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[5, 1].Text = dt.Rows[0]["GBN26"].ToString().Trim();
                    ssGB2.ActiveSheet.Cells[6, 1].Text = dt.Rows[0]["GBN27"].ToString().Trim();

                    txtHak1.Text = dt.Rows[0]["HAK1"].ToString().Trim();
                    txtHak2.Text = dt.Rows[0]["HAK2"].ToString().Trim();
                    txtHak3.Text = dt.Rows[0]["HAK3"].ToString().Trim();
                    txtHak4.Text = dt.Rows[0]["HAK4"].ToString().Trim();

                    txtVCode.Text = dt.Rows[0]["VCode"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }
    }
}
