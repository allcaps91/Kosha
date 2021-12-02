namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicCodeRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicCodeRepository()
        {
        }

        public List<HIC_CODE> FindOne(string v, string SortColumn = "CODE")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TRIM(Code) AS CODE, Name, GCODE                 ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_CODE                  ");
            parameter.AppendSql("  WHERE 1 = 1                                          ");

            if (!string.IsNullOrEmpty(v))
            {
                parameter.AppendSql("  AND GUBUN =:Gubun                               ");
            }

            if (v == "72")
            {
                parameter.AppendSql("  AND Code NOT IN ('0','Z')                        ");
            }

            if (string.Compare(SortColumn, "CODE") == 0)
            {
                parameter.AppendSql(" ORDER BY Code                                     ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY " + SortColumn + "                       ");
            }

            if (!string.IsNullOrEmpty(v))
            {
                parameter.Add("Gubun", v.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> Hic_Part_Jepsu(string v, string SortColumn = "CODE")
        {
            MParameter parameter = CreateParameter();
            //parameter.AppendSql("SELECT '**' AS CODE, '선택안함' as Name, '' GCODE      ");
            //parameter.AppendSql("  FROM DUAL                                            ");
            //parameter.AppendSql(" UNION ALL                                             ");
            parameter.AppendSql("SELECT TRIM(Code) AS CODE, Name, GCODE, TO_NUMBER(NVL(SORT, '999')) SORT, ROWID  ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_CODE                          ");
            parameter.AppendSql("  WHERE 1 = 1                                                  ");

            if (!string.IsNullOrEmpty(v))
            {
                parameter.AppendSql("  AND GUBUN = :GUBUN                                       ");
            }

            if (v == "72")
            {
                parameter.AppendSql("  AND Code NOT IN ('0','Z')                                ");
            }

            if (string.Compare(SortColumn, "CODE") == 0)
            {
                parameter.AppendSql(" ORDER BY CODE                                             ");
            }
            else
            {
                if (SortColumn == "SORT")
                {
                    parameter.AppendSql(" ORDER BY TO_NUMBER(SORT)                              ");
                }
                else
                {
                    parameter.AppendSql(" ORDER BY " + SortColumn + "                           ");
                }
            }

            if (!string.IsNullOrEmpty(v))
            {
                parameter.Add("GUBUN", v.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetCode1(string fstrGubun, string txtData, string gstrRetValue)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT             CODE,NAME,GCODE                                                         ");
            parameter.AppendSql("  FROM             KOSMOS_PMPA.HIC_CODE                                                    ");
            parameter.AppendSql(" WHERE             GUBUN = :GUBUN                                                          ");

            if(txtData.Trim() != "")
            {
                parameter.AppendSql("   AND         NAME LIKE :NAME                                                         ");
            }

            if(fstrGubun.Trim() == "10")
            {
                switch (VB.Right(gstrRetValue, 1))
                {
                    case "1":
                        parameter.AppendSql("   AND         GCODE ='A'                                                       ");
                        break;
                    case "2":
                        parameter.AppendSql("   AND         GCODE ='B'                                                       ");
                        break;
                    case "3":
                        parameter.AppendSql("   AND         GCODE ='C1'                                                      ");
                        break;
                    case "4":
                        parameter.AppendSql("   AND         GCODE ='C2'                                                      ");
                        break;
                    case "5":
                        parameter.AppendSql("   AND         GCODE ='D1'                                                      ");
                        break;
                    case "6":
                        parameter.AppendSql("   AND         GCODE ='D2'                                                      ");
                        break;
                    case "7":
                        parameter.AppendSql("   AND         GCODE ='R'                                                       ");
                        break;
                    case "8":
                        parameter.AppendSql("   AND         GCODE ='U'                                                       ");
                        break;
                    case "9":
                        parameter.AppendSql("   AND         GCODE ='CN'                                                      ");
                        break;
                    case "A":
                        parameter.AppendSql("   AND         GCODE ='DN'                                                      ");
                        break;
                }
            }
            parameter.AppendSql(" ORDER BY                  CODE                                                             ");

            if (txtData.Trim() != "")
            {
                parameter.AddLikeStatement("NAME", txtData);
            }


            parameter.Add("GUBUN", fstrGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetItemByLikeAll(string argCode, string argKeyWard)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,GBDEL,GCODE,GCODE1,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                     ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                           ");
            if (!argKeyWard.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND (NAME LIKE :NAME OR GCODE LIKE :GCODE OR GCODE1 LIKE :GCODE1) ");
            }

            parameter.AppendSql(" ORDER BY CODE                                  ");

            parameter.Add("GUBUN", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            if (!argKeyWard.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", argKeyWard);
                parameter.AddLikeStatement("GCODE", argKeyWard);
                parameter.AddLikeStatement("GCODE1", argKeyWard);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetListByCode(string argGbn, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,GBDEL,GCODE,GCODE1,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                     ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                           ");
            if (!strCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE = :CODE                            ");
            }
            
            parameter.AppendSql(" ORDER BY CODE                                  ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            if (!strCode.IsNullOrEmpty())
            {
                parameter.Add("CODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            
            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetListByGubun(string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN,CODE,NAME,GBDEL,GCODE,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                     ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                           ");
            parameter.AppendSql(" ORDER BY NAME                                  ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> BogunSearch(string txtBogun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         CODE,NAME,GCODE                          ");
            parameter.AppendSql("FROM           KOSMOS_PMPA.HIC_CODE                     ");
            parameter.AppendSql("WHERE          GUBUN='25'                               ");
            parameter.AppendSql("   AND         CODE = :CODE                             ");

            parameter.Add("CODE", txtBogun, Oracle.DataAccess.Client.OracleDbType.Char);
            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetItembyGubunName(string strJong, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,GBDEL,GCODE,GCODE1,GCODE2,SORT,ROWID                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                                    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                                          ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND NAME LIKE :NAME                                                     ");
            }
            parameter.AppendSql(" ORDER BY Sort,CODE                                                            ");

            parameter.Add("GUBUN", strJong, Oracle.DataAccess.Client.OracleDbType.Char);
            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetItembyGubun(string strGubun, string idNumber)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN,CODE,NAME,GBDEL,GCODE,ROWID                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                                    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                                          ");
            parameter.AppendSql("   AND GCODE = :GCODE                                                          ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCODE", idNumber);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public int InsertCode(string strGubun, string strCODE, string strName, string idNumber)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CODE       ");
            parameter.AppendSql("       (Gubun, Code, Name, GCode)      ");
            parameter.AppendSql("VALUES                                 ");
            parameter.AppendSql("       (:GUBUN, :CODE, :NAME, :GCODE)  ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", strName);
            parameter.Add("GCODE", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateSortbyRowId(string cODE, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CODE SET    ");
            parameter.AppendSql("       SORT    = :SORT             ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("SORT", cODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CODE> GetCodeNameGcode1(string strOldData)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         CODE, NAME, GCODE    ");
            parameter.AppendSql("FROM           HIC_CODE             ");
            parameter.AppendSql("WHERE          GUBUN='25'     ");
            parameter.AppendSql("   AND         CODE =:CODE          ");

            parameter.Add("CODE", strOldData, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public int GetCodeNameGcode(string strOldData)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         COUNT(CODE) CNT1, COUNT(NAME) CNT2, COUNT(GCODE) CNT3    ");
            parameter.AppendSql("FROM           HIC_CODE                                                 ");
            parameter.AppendSql("WHERE          GUBUN='25'                                               ");
            parameter.AppendSql("   AND         CODE =:CODE                                              ");
            
            parameter.Add("CODE", strOldData);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateCodeName(string strCODE, string strName, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CODE SET    ");
            parameter.AppendSql("       CODE    = :CODE             ");
            parameter.AppendSql("     , NAME    = :NAME             ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", strName);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CODE> GetCodeNamebyGubunNameGcode(string strGubun, string strName, string strstrRetValue = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");            
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                      ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND NAME LIKE :NAME                                 ");
            }
            if (strGubun == "10")
            {
                switch (strstrRetValue)
                {
                    case "1":
                        parameter.AppendSql("   AND GCODE = 'A'                             ");
                        break;
                    case "2":
                        parameter.AppendSql("   AND GCODE = 'B'                             ");
                        break;
                    case "3":
                        parameter.AppendSql("   AND GCODE = 'C1'                            ");
                        break;
                    case "4":
                        parameter.AppendSql("   AND GCODE = 'C2'                            ");
                        break;
                    case "5":
                        parameter.AppendSql("   AND GCODE = 'D1'                            ");
                        break;
                    case "6":
                        parameter.AppendSql("   AND GCODE = 'D2'                            ");
                        break;
                    case "7":
                        parameter.AppendSql("   AND GCODE = 'R'                             ");
                        break;
                    case "8":
                        parameter.AppendSql("   AND GCODE = 'U'                             ");
                        break;
                    case "9":
                        parameter.AppendSql("   AND GCODE = 'CN'                            ");
                        break;
                    case "A":
                        parameter.AppendSql("   AND GCODE = 'DN'                            ");
                        break;
                    default:
                        break;
                }
                parameter.AppendSql(" ORDER BY CODE                                         ");
            }

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }
            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public int DeleteCode(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_CODE        ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGCode2byRowId(string fstrROWID, string strBun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CODE SET    ");
            parameter.AppendSql("       GCODE2  = :GCODE2           ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("GCODE2", strBun);
            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CODE> GetCodeNamebyName(string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE NAME LIKE :NAME                                 ");                
            }
            parameter.AppendSql("   AND ROWNUM <= 30                                        ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetCodeNamebyGubun(string gstrRetValue)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LPAD(CODE, 5) CODE, NAME, GBDEL, GCODE, GCODE2, ROWID    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                      ");
            parameter.AppendSql("   AND GBDEL IS NULL                                       ");
            parameter.AppendSql(" ORDER BY CODE, NAME                                       ");

            parameter.Add("GUBUN", gstrRetValue, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetItembyGubun(string fstrJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, GBDEL, GCODE, ROWID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql(" ORDER BY GCODE, NAME                  ");

            parameter.Add("GUBUN", fstrJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string GetMaxCodebyGubun(string fstrJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(CODE) MAXCODE               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");

            parameter.Add("GUBUN", fstrJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> GetCodeNameListByGubunCodeIN(string argGubun, List<string> lstCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CODE IN (:CODE)                 ");
            parameter.AppendSql(" GROUP BY CODE, NAME                   ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("CODE", lstCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public long GetCodebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                            "); 
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND (GbDel = '' or GbDel IS NULL)   ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public int Delete(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_CODE GetItembyGubunGCode(string argGbn, List<string> argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, GCODE           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE        ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN              ");
            parameter.AppendSql("   AND GCODE  IN (:GCODE)          ");
            parameter.AppendSql(" ORDER BY GCODE, NAME              ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GCODE", argCode);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public string GetGCodebyGubunCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GCODE                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE        ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN              ");
            parameter.AppendSql("   AND CODE  = :CODE              ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetCodebyName(string argJisa)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE        ");
            parameter.AppendSql(" WHERE GUBUN = '21'                ");
            parameter.AppendSql("   AND NAME LIKE :NAME             ");

            parameter.AddLikeStatement("NAME", argJisa);

            return ExecuteScalar<string>(parameter);
        }

        public string GetGCode1ByGubunCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DECODE(GCODE1, '', NAME, GCODE1) NEWNAME    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                        ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              "); 
            parameter.AppendSql("   AND CODE  = :CODE                               ");
            parameter.AppendSql("   AND CODE NOT IN ('3', '6')                      ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> GetListCodebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LPAD(CODE, 5) CODE              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public HIC_CODE GetItembyGubunCode2(string argGbn, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Code,Name,DECODE(GCode1,'',Name,GCode1) NewName     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                      ");
            parameter.AppendSql("   AND CODE  = :CODE                                       ");
            parameter.AppendSql(" ORDER BY GCode,Name                                       ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public HIC_CODE GetItembyCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,GCODE,GCODE1,GCODE2,ROWID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      "); //특검문진항목
            parameter.AppendSql("   AND CODE  = :CODE                       ");
            parameter.AppendSql(" ORDER BY CODE                             ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", strCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetCodeGCodebyCode(List<string> strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, GCODE                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");
            parameter.AppendSql(" WHERE GUBUN = '52'                                        "); //특검문진항목
            parameter.AppendSql("   AND CODE  IN (SELECT UCode FROM KOSMOS_PMPA.HIC_MCODE   ");
            parameter.AppendSql("                  WHERE CODE IN (:CODE)                    ");
            parameter.AppendSql(" ORDER BY GCODE, CODE                                      ");

            parameter.AddInStatement("CODE", strCodes, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetCodeNamebyCode(string strTempMGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE Gubun = '52'                    "); //특검문진항목
            parameter.AppendSql("   AND CODE  = :CODE                   ");
            parameter.AppendSql(" ORDER BY Code                         ");

            parameter.Add("CODE", strTempMGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string GetNamebyCode(string strMGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE            ");
            parameter.AppendSql(" WHERE Gubun = '52'                    "); //특검문진항목
            parameter.AppendSql("   AND CODE  = :CODE                   ");

            parameter.Add("CODE", strMGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Update(string strName, string strGCode, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_CONSENT SET         ");
            parameter.AppendSql("       NAME  = :NAME                       ");
            parameter.AppendSql("     , GCODE = :GCODE                      ");            
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("NAME", strName);
            parameter.Add("GCODE", strGCode);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(string v1, string v2, string strName, string v3, string strGCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CODE                   ");
            parameter.AppendSql("       (GUBUN,CODE, NAME,GBDEL, GCODE)             ");
            parameter.AppendSql("VALUES                                             ");
            parameter.AppendSql("       (:GUBUN, :CODE, :NAME, :GBDEL, :GCODE)      ");

            parameter.Add("GUBUN", v1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", v2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", strName);
            parameter.Add("GBDEL", v3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCODE", strGCode);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_CODE> FindCodeIn(string v, string argCODE = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TRIM(Code) AS CODE, NAME, GCODE                 ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_CODE                  ");
            parameter.AppendSql("  WHERE 1 = 1                                          ");
            if (!string.IsNullOrEmpty(v))
            {
                parameter.AppendSql("  AND GUBUN = :GUBUN                               ");
            }

            if (!string.IsNullOrEmpty(argCODE) && argCODE != "")
            {
                parameter.AppendSql("   AND Code IN (" + argCODE + ")                   ");
            }

            parameter.AppendSql(" ORDER BY Code                                         ");

            if (!string.IsNullOrEmpty(v))
            {
                parameter.Add("GUBUN", v.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public HIC_CODE GetItembyGubunCode(string argGbn, List<string> argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Code,Name,DECODE(GCode1,'',Name,GCode1) NewName     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                      ");
            parameter.AppendSql("   AND CODE  IN (:CODE)                                    ");
            parameter.AppendSql(" ORDER BY GCode,Name                                       ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("CODE", argCode);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public HIC_CODE GetGCode2byGubunCode(string argGbn, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, GCODE2      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");
            parameter.AppendSql(" ORDER BY GCODE, NAME          ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetGrpByNameGcode1ByGubun(string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,GCODE1,GCODE2, ROWID      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql(" ORDER BY CODE                             ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string GetNameByGubunGcode1(string argGubun, string argGcode1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN =:GUBUN           ");
            parameter.AppendSql("   AND GCODE1=:GCODE1          ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCODE1", argGcode1);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> GetCodeGubunbyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,GCODE1,GCODE2     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN         ");
            
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> Read_Hic_Code_All(string strGubun, List<string> strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GCODE                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE IN (:CODE)         ");
            
            parameter.AddInStatement("CODE", strCode);
            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public List<HIC_CODE> GetListByCodeName(string strGubun, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,GCODE,GCODE1                      ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_CODE              ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            if (!strGubun.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN = :GUBUN                          ");
            }
            if (!string.IsNullOrEmpty(strName))
            {
                parameter.AppendSql("  AND NAME   LIKE :NAME                        ");
            }
            parameter.AppendSql(" ORDER BY SORT,CODE,GCODE                          ");

            if (!strGubun.IsNullOrEmpty())
            {
                parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string GetNameByGubunCode(string v1, string v2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN      = :GUBUN     ");
            parameter.AppendSql("   AND TRIM(CODE) = :CODE      ");

            parameter.Add("GUBUN", v1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", v2, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> AutoCompleteText(string fstrGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Name                                       ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_CODE             ");
            parameter.AppendSql(" WHERE 1 = 1                                      ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                             ");
            parameter.AppendSql(" ORDER BY Sort,Code                               ");

            parameter.Add("GUBUN", fstrGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string Read_JisaCode(string argJisaName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN  = '21'            ");
            parameter.AppendSql("   AND NAME   = :NAME          ");

            parameter.Add("NAME", argJisaName);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_HicCode2_GCodeNew1(string argGubun, string argCode1, string argCode2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GCODE  = :GUBUN         ");
            parameter.AppendSql("   AND GCODE  = :GCODE         ");
            parameter.AppendSql("   AND GCODE1 = :GCODE1        ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCODE", argCode1);
            parameter.Add("GCODE1", argCode2);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_SCODE Read_SCode2_GCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Code, JCode, Chasu      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCODE   ");
            parameter.AppendSql(" WHERE SCODE  = :SCODE         ");

            parameter.Add("SCODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SCODE>(parameter);
        }

        public string Read_Hic_Jibung(string argGbn, string argPan, string argSogen1, string argSogen2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GCODE1                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN         ");
            parameter.AppendSql("   AND GCODE  = :GCODE         ");
            parameter.AppendSql("   AND CODE   = :CODE          ");
            if (argSogen2 != string.Empty)
            {
                parameter.AppendSql("   AND GCODE2 = :GCODE2    ");
            }

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCODE", argPan);
            parameter.Add("CODE", argSogen1, Oracle.DataAccess.Client.OracleDbType.Char);
            if (argSogen2 != string.Empty)
            {
                parameter.Add("GCODE2", argSogen2);
            }

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> Read_Combo_HisDoctor()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE DRCODE, NAME SNAME, GCODE SABUN    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE                    ");
            parameter.AppendSql("  WHERE GUBUN = '30'                           ");
            parameter.AppendSql("  AND Code NOT IN ('178','361','669','64282')  ");

            return ExecuteReader<HIC_CODE>(parameter);
        }

        public string Read_HicCode2_GCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GCODE                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_CODE Read_Hic_Code(string GUBUN, string CODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,GCODE         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql("  WHERE GUBUN = :GUBUN         ");
            parameter.AppendSql("    AND CODE  = :CODE          ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

        public string Read_Hic_Code2(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND GCODE  = :CODE          ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_Code3(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GCODE2                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_CODE> Read_Hic_Code_Multi(string GUBUN, List<string> listCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql("  WHERE GUBUN = :GUBUN         ");
            parameter.AppendSql("    AND CODE IN (:CODE)        ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("CODE", listCode);

            return ExecuteReader<HIC_CODE>(parameter);
        }


        public string Read_Hic_Name2(string GUBUN, string CODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql("  WHERE GUBUN = :GUBUN         ");
            parameter.AppendSql("    AND TRIM(CODE)  = :CODE    ");

            parameter.Add("GUBUN", GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_CodeName(string CODE = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE        ");
            parameter.AppendSql("  WHERE CODE  = :CODE.substr(0, 1) ");
            parameter.AppendSql("    AND GUBUN = '07'               ");

            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_CODE GetNameGcodebyGubunCode(string argGbn, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT  NAME, GCODE            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_CODE>(parameter);
        }

    }
}