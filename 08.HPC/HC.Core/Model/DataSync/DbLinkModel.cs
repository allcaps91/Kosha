namespace HC_Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    public class DbLinkModel : BaseDto
    {
        public string TableName { get; set; }
        public string Description { get; set; }

        public string IsComplete { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSchemaOnly { get; set; }
        public DbLinkModel()
        {

        }
        public DbLinkModel(string tableName)
        {
            this.TableName = tableName;
        }
        public DbLinkModel(string tableName, bool isSchemaOnly)
        {
            this.TableName = tableName;
            this.IsSchemaOnly = isSchemaOnly;
        }
    }
}
