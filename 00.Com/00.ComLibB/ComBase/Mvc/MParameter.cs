using ComBase.Mvc.Utils;
using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Client;;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class MParameter
    {
        /// <summary>
        /// 
        /// </summary>
        private StringBuilder stringBuilder = null;
        /// <summary>
        /// 
        /// </summary>
        public StringBuilder GetStringBuilder { get { return stringBuilder; } }
        /// <summary>
        /// /
        /// </summary>
        public List<OracleParameter> Parameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CommandType commandType = CommandType.Text;
        /// <summary>
        /// 
        /// </summary>
        private string nativeQuerySql;
        /// <summary>
        /// Sql 최종 문장을 설정하거나 가져옵니다
        /// </summary>
        public string NativeQuerySql
        {
            get
            {
                string sql = this.SQL;
                if (this.Parameters != null)
                {
                    foreach (OracleParameter oracleParameter in this.Parameters)
                    {
                        switch (oracleParameter.OracleDbType)
                        {
                            case OracleDbType.Date:
                                sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                                break;
                            case OracleDbType.TimeStamp:
                                sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                                break;
                            case OracleDbType.Int16:
                            case OracleDbType.Int32:
                            case OracleDbType.Int64:
                                sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, true);
                                break;
                            case OracleDbType.Varchar2:
                            case OracleDbType.NVarchar2:
                            case OracleDbType.Char:
                                sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);
                                break;
                            default:
                                sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);

                                break;

                        }
                    }
                }

                nativeQuerySql = sql;
                return nativeQuerySql;
            }
            set
            {
                nativeQuerySql = value;
            }
        }

        public string GetQuery()
        {
            string sql = this.SQL;
            if (this.Parameters != null)
            {
                foreach (OracleParameter oracleParameter in this.Parameters)
                {
                    switch (oracleParameter.OracleDbType)
                    {
                        case OracleDbType.Date:
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                            break;
                        case OracleDbType.TimeStamp:
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false, true);
                            break;
                        case OracleDbType.Int16:
                        case OracleDbType.Int32:
                        case OracleDbType.Int64:
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, true);
                            break;
                        case OracleDbType.Varchar2:
                        case OracleDbType.NVarchar2:
                        case OracleDbType.Char:
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);
                            break;
                        default:
                            sql = ReplaceQuery(sql, oracleParameter.ParameterName, oracleParameter.Value, false);

                            break;

                    }
                }
            }

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
            MatchCollection matches = Regex.Matches(sqls, searchText);
            int matchCount = matches.Count;
            string replaceSql = sqls;

            for (int i = 0; i < matches.Count; i++)
            {
                int startIndex = replaceSql.IndexOf(searchText);
                if (startIndex > -1)
                {
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
                                replaceSql = front + "'" + DateUtil.DateTimeToStrig((DateTime)value, Controls.DateTimeType.YYYY_MM_DD_HH_MM_SS) + "'" + back;

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

        public MParameter()
        {
            stringBuilder = new StringBuilder();
            Parameters = new List<OracleParameter>();
        }

        /// <summary>
        /// Sql문장을 연결합니다
        /// </summary>
        /// <param name="sql"></param>
        public void AppendSql(string sql)
        {
            stringBuilder.AppendLine(sql);
        }

        /// <summary>
        /// sql 문장의 stringbuilder를 반환합니다
        /// </summary>
        public string SQL { get { return stringBuilder.ToString(); } }

        public bool Exists(string name)
        {
            foreach(OracleParameter p in Parameters)
            {
                if(p.ParameterName.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Prepared statement 파라미터를 add 합니다
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public void Add(string paramName, object paramValue)
        {
            Add(paramName, paramValue, false);
        }
        /// <summary>
        /// Prepared statement 파라미터를 add 합니다
        /// <history>dhkim 20191001 oracleDbType 추가, Char데이타타입의 경우 필요함 </history>
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public void Add(string paramName, object paramValue, OracleDbType oracleDbType = OracleDbType.Varchar2)
        {
            Add(paramName, paramValue, false, LikePostion.All, oracleDbType);
        }
        /// <summary>
        /// 프로시져 인파라미터
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="oracleDbType"></param>
        public void AddProcIn(string paramName, object paramValue, OracleDbType oracleDbType = OracleDbType.Varchar2)
        {
            Parameters.Add(new OracleParameter(paramName, oracleDbType, paramValue, ParameterDirection.Input));
        }

        /// <summary>
        /// 프로시져 아웃 파라미터
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="oracleDbType"></param>
        /// <param name="size"></param>
        public void AddProcOut(string paramName, OracleDbType oracleDbType = OracleDbType.RefCursor, int size = 0)
        {
            OracleParameter oracleParameter = new OracleParameter();
            oracleParameter.ParameterName = paramName;
            oracleParameter.OracleDbType = oracleDbType;
            oracleParameter.Size = size;
            oracleParameter.Direction = ParameterDirection.Output;
            Parameters.Add(oracleParameter);
        }

        /// <summary>
        /// 라이크 스테이먼트
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddLikeStatement(string paramName, object paramValue, LikePostion likePostion = LikePostion.All)
        {
            Add(paramName, paramValue, true, likePostion);
        }

        private void Add(string paramName, object paramValue, bool isLike = false, LikePostion likePostion = LikePostion.All, OracleDbType oracleDbType = OracleDbType.Varchar2)
        {
            //  동일파라미터 인경우 지우고 새로 추가
            if(Exists(paramName))
            {
                for(int i=0; i<Parameters.Count; i++)
                {
                    Parameters.RemoveAt(i);
                }
            }

            if (isLike)
            {
                if(likePostion == LikePostion.First)
                {
                    Parameters.Add(new OracleParameter(paramName, "%" + paramValue));
                }
                else if(likePostion == LikePostion.Last)
                {
                    Parameters.Add(new OracleParameter(paramName, paramValue + "%"));
                }
                else
                {
                    Parameters.Add(new OracleParameter(paramName, "%" + paramValue + "%"));
                }
            }
            else
            {

                if (paramValue == null)
                {
                    Parameters.Add(new OracleParameter(paramName, paramValue));
                }
                else
                {
                    if (paramValue.GetType().IsEnum)
                    {
                        Parameters.Add(new OracleParameter(paramName, paramValue.ToString()));
                    }
                    else if (paramValue is DateTime)
                    {
                        Parameters.Add(new OracleParameter(paramName, paramValue));
                    }
                    else
                    {
                        if (paramValue.GetType() == typeof(long))
                        {
                            oracleDbType = OracleDbType.Int64;
                        }
                        else if(paramValue.GetType() == typeof(decimal))
                        {
                            oracleDbType = OracleDbType.Decimal;
                        }

                        OracleParameter oracleParameter = new OracleParameter(paramName, oracleDbType);
                        oracleParameter.Value = paramValue;
                        
                        Parameters.Add(oracleParameter);
                    }
                }
            }
        }

        /// <summary>
        /// 문자열 IN 파라미터
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="list"></param>
        public void AddInStatement<T>(string paramName, List<T> list, OracleDbType oracleDbType = OracleDbType.Varchar2)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            AddInStatement(paramName, list.Cast<T>().ToArray(), oracleDbType);
        }
        public void AddInStatement<T>(string paramName, T[] list, OracleDbType oracleDbType = OracleDbType.Varchar2)
        {
            if (list == null || list.Length == 0)
            {
                return;
            }

            List<string> bindParams = new List<string>();
            for (int i = 0; i < list.Length; i++)
            {
                object item = list[i];
                string pName = string.Concat(paramName, "_", i);

                OracleParameter parameter = null;

                if (item.GetType() == typeof(string))
                {
                    if (oracleDbType != OracleDbType.Varchar2)
                    {
                        parameter = new OracleParameter(pName, oracleDbType);
                    }
                    else
                    {
                        parameter = new OracleParameter(pName, OracleDbType.Varchar2);
                    }
                }
                else if (item.GetType() == typeof(Int32))
                {
                    parameter = new OracleParameter(pName, OracleDbType.Int32);
                }
                else if (item.GetType() == typeof(Int64))
                {
                    parameter = new OracleParameter(pName, OracleDbType.Int64);
                }

                parameter.Value = list[i];
                parameter.Value = parameter.Value.ToString().Trim();

                Parameters.Add(parameter);
                string bindParamName = ":" + pName;
                bindParams.Add(bindParamName);
            }

            //  SQL변경
            string sql = SQL;
            string tempName = string.Concat(":", paramName);
            //  " " 문자를 짤라서 변경
            string[] sqlSplit = sql.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //  동일 바인드 변수가 있을수 있으므로 전체 체크
            //  ex) :GB, :GB1 Replace 사용이 불가함
            for (int i = 0; i < sqlSplit.Length; i++)
            {
                if(sqlSplit[i].IndexOf(tempName) > -1)
                {
                    if(sqlSplit[i].Length == tempName.Length)
                    {
                        sqlSplit[i] = string.Join(",", bindParams);
                    }
                    else
                    {
                        string temp = sqlSplit[i].Replace("(", "").Replace(")", "").Replace("\r\n", "");

                        if(temp.Equals(tempName))
                        {
                            sqlSplit[i] = sqlSplit[i].Replace(tempName, string.Join(",", bindParams));
                        }
                    }
                }
            }

            sql = string.Join(" ", sqlSplit);
            stringBuilder = new StringBuilder();
            stringBuilder.Append(sql);
        }
    }

    public enum LikePostion
    {
        All,
        First,
        Last
    }
}
