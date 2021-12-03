using ComBase;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections;
using System.Data;
using static ComSupLibB.SupPthl.clsPthlSQL;

namespace ComSupLibB.SupPthl
{
    /// <summary>
    /// Class Name : ComSupLibB.SupPthl
    /// File Name : clsPthl.cs 
    /// Title or Description : 조직병리 Biz
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public class clsPthl : Com.clsMethod
    {

        clsPthlRcpSQL pthlRcpSQL = new clsPthlRcpSQL();

        public string Seq_Anat_No_NEW( string ArgCode, bool ArgOut, string strYY, bool isAC)
        {
            string strANATNO         = "";
            string strGubun  = "";

            if (ArgCode.Trim().Equals("세포") == true)
            {
                if (isAC == true)
                {
                    strGubun = "AC";
                }
                else
                {
                    strGubun = "C";
                }
            }
            else if (ArgCode.Trim().Equals("조직") == true)
            {
                strGubun = "S";

                //2018-11-20 안정수, 우수희t 요청으로 조직에서 외부의뢰시에도 S로 접수되도록 변경함 
                //2019-01-30 안정수, 조직 외부의뢰 재 시행
                if (ArgOut == true)
                {
                    strGubun = "OS";
                }
            }
            else if (ArgCode.Trim().Equals("신검(부인과)") == true)
            {
                strGubun = "P";
            }
            else if (ArgCode.Trim().Equals("면역염색") == true)
            {
                strGubun = "IH";
            }
            else if (ArgCode.Trim().Equals("신검(객담)") == true)
            {
                strGubun = "PS";
            }
            else if (ArgCode.Trim().Equals("신검(소변)") == true)
            {
                strGubun = "PU";
            }

            DataTable dt = pthlRcpSQL.sel_EXAM_ANATNO(clsDB.DbCon, strGubun);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                string SqlErr = string.Empty;
                string SQL = string.Empty;
                int intRowAffected = 0;

                try
                {
                    if (dt.Rows[0][0].ToString().Equals("N") == true)
                    {
                        SqlErr = pthlRcpSQL.ins_EXAM_ANATNO(clsDB.DbCon, strGubun, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return "-1";
                        }

                        strANATNO = strGubun + strYY + string.Format("{0:00000}", 1); 
                    }
                    else
                    {                       
                        SqlErr = pthlRcpSQL.up_EXAM_ANATNO(clsDB.DbCon, strGubun, dt.Rows[0][0].ToString(),"",  ref intRowAffected);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return "-1";
                        }

                        strANATNO = strGubun + strYY + string.Format("{0:00000}", Convert.ToInt32(dt.Rows[0][0].ToString()));

                    }
                    clsDB.setCommitTran(clsDB.DbCon);


                     
                    
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message.ToString());

                    return "-1";
                }            
            }
            else
            {
                ComFunc.MsgBox("병리번호 생성 오류");
            }

            return strANATNO;
        }
    }
}
