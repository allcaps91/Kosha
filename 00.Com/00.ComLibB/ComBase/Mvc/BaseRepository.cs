using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class BaseRepository
    {
        /// <summary>
        /// 파라미터 생성
        /// </summary>
        /// <returns></returns>
        protected MParameter CreateParameter()
        {
            return new MParameter();
        }
        public long GetSequenceNextVal(string sequenceName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT " + sequenceName + ".NEXTVAL FROM DUAL");
            return ExecuteScalar<long>(parameter);
        }
        /// <summary>
        /// 싱글데이터
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="mParameter"></param>
        /// <returns></returns>
        protected DTO ExecuteReaderSingle<DTO>(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteReaderSingle<DTO>(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 리스트 데이터
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="mParameter"></param>
        /// <returns></returns>
        protected List<DTO> ExecuteReader<DTO>(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteReader<DTO>(mParameter, clsDB.DbCon);
        }

        protected BindingList<DTO> BindingReader<DTO>(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.BindingReader<DTO>(mParameter, clsDB.DbCon);

        }

        /// <summary>
        /// 한컬럼 데이터 조회
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mParameter"></param>
        /// <returns></returns>
        protected T ExecuteScalar<T>(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteScalar<T>(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {            
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected Dictionary<string, object> ExecuteProc(MParameter parameter, bool isLog = true)
        {
            return clsDB.ExecuteProc(parameter, clsDB.DbCon);
        }

        /// <summary>
        /// 딕셔너리 리스트로 반환
        /// </summary>
        /// <param name="mParameter"></param>
        /// <returns>List<Dictionary<string, object>></returns>
        protected List<Dictionary<string, object>> ExecuteReader(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 딕셔너리로 반환
        /// </summary>
        /// <param name="mParameter"></param>
        /// <returns>List<Dictionary<string, object>></returns>
        protected Dictionary<string, object> ExecuteReaderSingle(MParameter mParameter, bool isLog = true, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            QueryLogging(mParameter.GetStringBuilder, mParameter, isLog, filePath, memberName, sourceLineNumber);
            return clsDB.ExecuteReaderSingle(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// 쿼리 로깅
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mParameter"></param>
        /// <param name="filePath"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceLineNumber"></param>
        protected void QueryLogging(StringBuilder sql, MParameter mParameter, bool isLog, string filePath, string memberName, int sourceLineNumber)
        {
            string generateSql = GenerateSql(sql.ToString(), mParameter);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Log.Debug("filePath:{}, method:{}, srcLineNum:{},  QUERY :  \r\n {}", filePath, memberName, sourceLineNumber, generateSql);
            }

            if(isLog)
            {
                //  실서버일경우만 로그 남김
                if(clsDB.DbCon.strDbIp.Equals("192.168.100.31"))
                {
                    clsDB.SaveSqlLog(generateSql, clsDB.DbCon);
                }
            }
        }
        /// <summary>
        /// PreparedStatement 쿼리 문자열 생성
        /// 20190828 dhkim
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mParameter"></param>
        /// <returns></returns>
        public string GenerateSql(string sql, MParameter mParameter = null)
        {
            if (mParameter != null)
            {
                foreach (OracleParameter oracleParameter in mParameter.Parameters)
                {
                    if(oracleParameter.ParameterName == "WRTNO")
                    {
                        string x = "";
                    }
                    switch (oracleParameter.OracleDbType)
                    {
                        case OracleDbType.Date:
                            //sql = sql.Replace(":" + oracleParameter.ParameterName, oracleParameter.Value.ToString());
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                            break;
                        case OracleDbType.TimeStamp:
                            //sql = sql.Replace(":" + oracleParameter.ParameterName, oracleParameter.Value.ToString());
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                            break;
                        case OracleDbType.Int16:
                        case OracleDbType.Int32:
                        case OracleDbType.Int64:
                            // sql = sql.Replace(":" + oracleParameter.ParameterName, oracleParameter.Value.ToString());
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, true);
                            break;
                        case OracleDbType.Varchar2:
                        case OracleDbType.NVarchar2:
                        case OracleDbType.Char:
                            //if (oracleParameter.Value == null)
                            //{
                            //    sql = sql.Replace(":" + oracleParameter.ParameterName, "''");
                            //}
                            //else
                            //{
                            //    sql = sql.Replace(":" + oracleParameter.ParameterName, "'" + oracleParameter.Value.ToString() + "'");
                            //}
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);
                            break;
                        default:
                            //if (oracleParameter.Value == null)
                            //{
                            //    sql = sql.Replace(oracleParameter.ParameterName, "");
                            //}
                            //else
                            //{
                            //    sql = sql.Replace(oracleParameter.ParameterName, oracleParameter.Value.ToString());
                            //}
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);

                            break;

                    }
                }
            }

            mParameter.NativeQuerySql = sql;
            return sql;
        }

        /// <summary>
        /// 매칭되는 파라미터는 모두 replace하도록 수정됨
        /// 2019-10-05 dhkim
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="isNumber"></param>
        /// <param name="isDate"></param>
        /// <returns></returns>
        private string ReplaceQuery(string sqls, string parameterName, object value, bool isNumber, bool isDate = false)
        {
            string searchText = ":" + parameterName;
            string pattern =  @":" + parameterName + "\\b";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(sqls);  //Regex.Matches(sqls, searchText);
            int matchCount = matches.Count;
            string replaceSql = sqls;

            for (int i = 0; i < matchCount; i++)
           
            {
                int startIndex = replaceSql.IndexOf(searchText);
                if (startIndex > -1)
                {
                    //string findvalue = sql.Substring(startIndex, searchText.Length);
                    string front = replaceSql.Substring(0, startIndex);
                    string back = replaceSql.Replace(front + searchText, "");
                    if (value == null)
                    {
                        if (isNumber)
                        {
                            replaceSql = front + value.ToString() + back;
                        }
                        else
                        {
                            replaceSql = front + "''" + back;
                        }
                    }
                    else
                    {
                        if (isNumber)
                        {
                            replaceSql = front + value.ToString() + back;
                        }
                        else
                        {
                            if (isDate)
                            {
                                replaceSql = front + "'" + DateUtil.DateTimeToStrig((DateTime)value, DateTimeType.YYYY_MM_DD_HH_MM_SS) + "'" + back;
                                //DateUtil.DateTimeToStrig((DateTime)value , Core.Enums.MTSDateTimeType.YYYY_MM_DD_HH_MM_SS)
                                //return front + "TO_DATE('"+value.ToString()+"', 'YYYY/MM/DD HH:MI:SS')" + back;

                            }
                            else
                            {
                                replaceSql = front + "'" + value.ToString() + "'" + back;
                            }
                        }
                    }
                }
            }

     
            return replaceSql;
        }

    }
}
