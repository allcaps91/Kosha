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
    /// File Name       : frmPmpaViewMailHelp.cs
    /// Description     : 동명칭으로 우편번호 찾기
    /// Author          : 안정수
    /// Create Date     : 2017-10-18
    /// Update History  :  
    /// <history>       
    /// d:\psmh\OPD\olrepa\MAILHELP_test.FRM(FrmMailH_test) => frmPmpaViewMailHelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\MAILHELP_test.FRM(FrmMailH_test)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMailHelp : Form
    {
        string[] strData = new string[5000];
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        public frmPmpaViewMailHelp()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.txtDongNm.GotFocus += new EventHandler(eControl_GotFocus);

            this.txtDongNm.LostFocus += new EventHandler(eControl_LostFocus);

            this.txtDongNm.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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

            txtDongNm.Text = "";
            clsPublic.GstrMsgList = "";

            optAll.Checked = true;
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtDongNm)
            {
                txtDongNm.ImeMode = ImeMode.Hangul;
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if(txtDongNm.Text.Length >= 2)
            {
                Juso_display();
            }
            
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
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
                if (txtDongNm.Text.Length >= 2)
                {
                    Juso_display();
                }
            }

            else if (sender == this.btnCancel)
            {
                btnCancel_Click();
            }
        }

        void btnCancel_Click()
        {
            clsPmpaPb.GstrValue = "";
            this.Close();
        }

        void Juso_display()
        {
            string strshow = "";
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            for(i = 0; i < strData.Length; i++)
            {
                strData[i] = "";
            }

            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  Mailcode,Mailjuso,Maildong,MailJiyek,GbDel,Gubun,DECODE(Gubun,'2','◈','') Gubun2,GbDel ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "Bas_MailNew";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (optAll.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND MailDong LIKE '%" + txtDongNm.Text + "%' ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND MailDong LIKE '%" + VB.RTrim(txtDongNm.Text) + "%' ";
            }

            if(optRoad.Checked == false)
            {
                SQL += ComNum.VBLF + "  AND Gubun ='1' ";
            }
            
            SQL += ComNum.VBLF + "ORDER BY MailJiyek,MailCode,MailDong";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    txtDongNm.Focus();
                    dt.Dispose();
                    dt = null;                    
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 5000)
                    {
                        ComFunc.MsgBox("자료건수가 5000개가 넘었습니다.. !! 명칭을 자세히 해서 다시 조회하십시오!!");
                        txtDongNm.Focus();
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    // ***** 읽은 Record가 1건이면 변수에 저장후 종료함 *****
                    if (dt.Rows.Count == 1 && dt.Rows[0]["GbDel"].ToString().Trim() != "*")
                    {
                        strshow = dt.Rows[i]["mailcode"].ToString().Trim();
                        strshow += VB.Space(1) + VB.Left(dt.Rows[i]["mailJuso"].ToString().Trim() + VB.Space(50), 50);
                        clsPmpaPb.GstrValue = strshow + " " + VB.Left(dt.Rows[i]["maildong"].ToString().Trim() + VB.Space(50), 50) + VB.Left(dt.Rows[i]["MailJiYek"].ToString().Trim() + VB.Space(2), 2) + VB.Left(dt.Rows[i]["Gubun2"].ToString().Trim() + VB.Space(1), 1);
                        dt.Dispose();
                        dt = null;
                        //Unload Me
                        return;
                    }

                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MailCode"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MailJuso"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MailDong"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MailJiyek"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GbDel"].ToString().Trim();

                        if(ssList_Sheet1.Cells[i, 4].Text == "*")
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.Red;
                        }
                        else
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.White;
                        }

                        if(dt.Rows[i]["Gubun"].ToString().Trim() == "2")
                        {
                            ssList_Sheet1.Cells[i, 5].Text = "◈";
                            ssList_Sheet1.Rows[i].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.White;
                        }

                        strshow = dt.Rows[i]["mailcode"].ToString().Trim();
                        strshow += VB.Space(1) + VB.Left(dt.Rows[i]["mailJuso"].ToString().Trim() + VB.Space(50), 50);
                        strData[i + 1] = strshow + " " + VB.Left(dt.Rows[i]["maildong"].ToString().Trim() + VB.Space(50), 50) + VB.Left(dt.Rows[i]["MailJiYek"].ToString().Trim() + VB.Space(2), 2) + VB.Left(dt.Rows[i]["Gubun2"].ToString().Trim() + VB.Space(1), 1);

                        ssList_Sheet1.Rows[i].Height = 23;
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

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(ssList_Sheet1.Cells[e.Row, 4].Text == "*")
            {
                if (MessageBox.Show("삭제되어 없어진 우편번호입니다..다른주소를 선택하세요!!" + "\r\n" + "\r\n" + "삭제된 주소를 사용하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clsPmpaPb.GstrValue = strData[e.Row];
                    
                    if(optAll.Checked == true)
                    {
                        clsPublic.GstrMsgList = "Y";
                        this.Close();
                    }
                }
            }

            else
            {
                clsPmpaPb.GstrValue = strData[e.Row];
                clsPmpaPb.GstrValue_2 = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();

                if(optAll.Checked == true)
                {
                    clsPublic.GstrMsgList = "Y";
                }
                this.Close();
            }
        }

        void ssList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                int a = ssList_Sheet1.ActiveRowIndex;

                if(ssList_Sheet1.Cells[a, 4].Text == "*")
                {
                    ComFunc.MsgBox("삭제되어 없어진 우편번호입니다..다른주소를 선택하세요.");
                }

                else
                {
                    clsPmpaPb.GstrValue = strData[a];
                    clsPmpaPb.GstrValue_2 = ssList_Sheet1.Cells[a, 3].Text.Trim();
                    this.Close();
                }
            }
        }
    }
}
