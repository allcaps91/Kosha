using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ComDbB;
using ComBase;

namespace ComMirLibB.Com
{

    /// <summary>
    /// Class Name      : ComMirLibB.Com
    /// File Name       : clsComMirSql.cs
    /// Description     : 청구 공통 자주사용하는 SQL 모음
    /// Author          : 전종윤
    /// Create Date     : 2017-12-05
    /// Update History  : 
    /// </summary>
    /// <history>       
    /// </history>
    /// <seealso cref= "신규" />
    public class clsComMirSQL
    {


        public DataTable sel_BAS_CLINICDEPT_COMBO(PsmhDb pDbCon, List<string> lstNotInclude = null)
        {

            DataTable dt = null;

            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            string strSqlWhere = string.Empty;


            SQL = "";
            SQL += " SELECT                                                " + ComNum.VBLF;
            SQL += "       DEPTCODE                                        " + ComNum.VBLF;
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT            " + ComNum.VBLF;
            SQL += " WHERE PrintRanking < 30                               " + ComNum.VBLF;
            //SQL += "   AND DeptCode <> 'PT'                                " + ComNum.VBLF;
            
            
            if (lstNotInclude != null && lstNotInclude.Count > 0)
            {
                for (int i = 0; i < lstNotInclude.Count; i++)
                {
                    strSqlWhere += "'" + lstNotInclude[i] + "',";
                }

                strSqlWhere = strSqlWhere.Substring(0, strSqlWhere.Length - 1);

                SQL += "   AND DEPTCODE NOT IN (" + strSqlWhere + ")";

            }

            SQL += " ORDER BY Printranking                                 " + ComNum.VBLF;

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        public DataTable sel_BAS_CLINICDEPT_COMBO(PsmhDb pDbCon, string strDeptCode= null)
        {

            DataTable dt = null;

            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            string strSqlWhere = string.Empty;


            SQL = "";
            SQL += " SELECT                                                " + ComNum.VBLF;
            SQL += "       DEPTCODE                                        " + ComNum.VBLF;
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT            " + ComNum.VBLF;
            SQL += " WHERE PrintRanking < 30                               " + ComNum.VBLF;
            //SQL += "   AND DeptCode <> 'PT'                                " + ComNum.VBLF;
            strSqlWhere = strDeptCode;
            SQL += " ORDER BY Printranking                                 " + ComNum.VBLF;

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
    }
}
