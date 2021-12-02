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

namespace ComLibB
{
    public partial class frmwardConsult : Form
    {
        /// <summary>
        /// 2017 . 06 .01 : TODO  :\nurse\nrQI\registrybas\ FormInfo_History 함수 생성 요청
        /// 
        /// </summary>
        //string fstrCode = "";

        public frmwardConsult ()
        {
            InitializeComponent ();
        }

        private void frmwardConsult_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            dtpyyyy.Value = Convert.ToDateTime (ComFunc.FormatStrToDate (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D"));
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }


        private void Search ()
        {
            //int i = 0;
            //int j = 0;
            //int k = 0;
            //int intRead = 0;
            //int intRead2 = 0;
            //int intCol = 0;
            //int intTot = 0;
            //string strCode = "";
            //string strAVG = "";
            //string strYYMM = "";
            //bool rtnVal = false;

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;//권한 확인

            //조회

            if (optrdoWard.Checked == true)
            {
                //fstrCode = "92053";
            }
            else if (optrdomedical.Checked == true)
            {
                //fstrCode = "92054";
            }
            else if (optrdodoctor.Checked == true)
            {
                //fstrCode = "92052";
            }

            //if(chkAuto.Checked == true)
            //{
            //    if (ComFunc.MsgBoxQ ("식이이월 작업을 수행 하시겠습니까?" , "확인" , MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) ;

            //}



        }

        private void btnSearch_Click (object sender , EventArgs e)
        {

        }
    }
}
