using ComLibB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupEXSET02.cs
    /// Description     : 진료진원 EKG 메모 보기 및 등록 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmEkgMemo.frm(FrmEkgMemo) 폼 frmComSupEXSET02.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\FrmEkgMemo.frm >> frmComSupEXSET02.cs 폼이름 재정의" />
    public partial class frmComSupEXSET02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSup.cBasPatient cBasPatient = null;

        string gstrPano = "";

        #endregion

        public frmComSupEXSET02(string strPano)
        {
            InitializeComponent();

            gstrPano = strPano;

            lblinfo.Text = "환자선택후 작업하세요!!";
            txtMemo.Text = "";
                       
            DataTable dt = sup.sel_Bas_Patient(clsDB.DbCon,gstrPano);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtMemo.Text = dt.Rows[0]["EkgMsg"].ToString().Trim();
                lblinfo.Text = "등록번호:"+ dt.Rows[0]["Pano"].ToString().Trim() + " 성명:"+ dt.Rows[0]["SName"].ToString().Trim() + " 주민번호:"+ dt.Rows[0]["JuminFull"].ToString().Trim();
            }
            dt = null;
            

            setEvent();
        }

        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            read_sysdate();

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
                        
            this.btnSave.Click += new EventHandler(eBtnEvent);
            
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }            
            else if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon, gstrPano,txtMemo.Text.Trim());
                this.Close();

            }
                        
        }               

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData(clsDB.DbCon);

                screen_clear();
            }           


        }

        void screen_clear()
        {
            //
            read_sysdate();          

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void eSave(PsmhDb pDbCon, string argPano,string argMemo)
        {

            if (argPano == "") return;
            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                cBasPatient = new clsComSup.cBasPatient();
                cBasPatient.Pano = argPano;
                cBasPatient.EkgMsg = argMemo;

                //갱신
                SqlErr = sup.up_BasPatient(pDbCon, cBasPatient, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                }
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            

        }

    }
}
