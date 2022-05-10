using ComBase.Controls;
using ComBase.Mvc;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core.Service;
using System.Collections.Generic;

namespace HC.Core.Repository
{
    public class MacrowordRepository : BaseRepository
    {
        public MacrowordDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *    ");
            parameter.AppendSql("  FROM HIC_MACROWORD ");
            parameter.AppendSql(" WHERE ID = :ID ");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<MacrowordDto>(parameter);

        }

        public List<MacrowordDto> FindAll(string formName, string controlId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *             ");
            parameter.AppendSql("  FROM HIC_MACROWORD A");
            parameter.AppendSql(" WHERE FORMNAME = :formName ");
            parameter.AppendSql(" AND CONTROL = :controlId ");
            parameter.AppendSql(" ORDER BY DISPSEQ  ");
            parameter.Add("formName", formName);
            parameter.Add("controlId", controlId);

            return ExecuteReader <MacrowordDto>(parameter);

        }
        public List<MacrowordDto> FindAll(string formName, string controlId, string title)
        {
            MParameter parameter = CreateParameter();
            //parameter.AppendSql("SELECT A.ID"   );
            //parameter.AppendSql("     , A.FORMNAME" );
            //parameter.AppendSql("     , A.CONTENT");
            parameter.AppendSql("SELECT *             ");
            parameter.AppendSql(" FROM HIC_MACROWORD A");
            parameter.AppendSql(" WHERE FORMNAME = :formName ");
            parameter.AppendSql(" AND CONTROL = :controlId ");
            if (title != "") parameter.AppendSql(" AND TITLE LIKE '%"+title+"%' ");
            parameter.AppendSql(" ORDER BY DISPSEQ  ");
            parameter.Add("formName", formName);
            parameter.Add("controlId", controlId);

            return ExecuteReader<MacrowordDto>(parameter);

        }
        public MacrowordDto Update(MacrowordDto item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_MACROWORD");
            parameter.AppendSql("    SET ");
            parameter.AppendSql("        FORMNAME = :FORMNAME");
            parameter.AppendSql("      , CONTROL = :CONTROL");
            parameter.AppendSql("      , TITLE = :TITLE");
            parameter.AppendSql("      , SUBTITLE = :SUBTITLE");
            parameter.AppendSql("      , CONTENT = :CONTENT");
            parameter.AppendSql("      , CONTENT2 = :CONTENT2");
            parameter.AppendSql("      , DISPSEQ = :DISPSEQ");
            parameter.AppendSql("      , MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("     WHERE ID =:ID");

            parameter.Add("ID", item.ID);
            parameter.Add("FORMNAME", item.FORMNAME);
            parameter.Add("CONTROL", item.CONTROL);
            parameter.Add("TITLE", item.TITLE);
            parameter.Add("SUBTITLE", item.SUBTITLE);
            parameter.Add("CONTENT", item.CONTENT);
            parameter.Add("CONTENT2", item.CONTENT2);
            parameter.Add("DISPSEQ", item.DISPSEQ);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

          
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_MACROWORD", item.ID);

            return FindOne(item.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_MACROWORD   ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_MACROWORD", id);

        }
        public MacrowordDto Insert(MacrowordDto item)
        {
            item.ID = GetSequenceNextVal("HC_MACROWORD_ID_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_MACROWORD");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , FORMNAME");
            parameter.AppendSql("  , CONTROL");
            parameter.AppendSql("  , TITLE");
            parameter.AppendSql("  , SUBTITLE");
            parameter.AppendSql("  , CONTENT");
            parameter.AppendSql("  , CONTENT2");
            parameter.AppendSql("  , DISPSEQ");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :FORMNAME");
            parameter.AppendSql("  , :CONTROL");
            parameter.AppendSql("  , :TITLE");
            parameter.AppendSql("  , :SUBTITLE");
            parameter.AppendSql("  , :CONTENT");
            parameter.AppendSql("  , :CONTENT2");
            parameter.AppendSql("  , :DISPSEQ");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("ID", item.ID);
            parameter.Add("FORMNAME", item.FORMNAME);
            parameter.Add("CONTROL", item.CONTROL);
            parameter.Add("TITLE", item.TITLE);
            parameter.Add("SUBTITLE", item.SUBTITLE);
            parameter.Add("CONTENT", item.CONTENT);
            parameter.Add("CONTENT2", item.CONTENT2);
            parameter.Add("DISPSEQ", item.DISPSEQ);
            if (item.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("MODIFIEDUSER", item.CREATEDUSER);
                parameter.Add("CREATEDUSER", item.CREATEDUSER);
            }
           


            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_MACROWORD", item.ID);

            return FindOne(item.ID);
        }
    }
}
