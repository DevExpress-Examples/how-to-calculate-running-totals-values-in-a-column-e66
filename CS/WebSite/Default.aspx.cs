using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Xpo;
using System.Drawing;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Collections.Generic;
using DevExpress.Data;
using DevExpress.Web.ASPxDataView;
using System.Collections;
using System.Data.OleDb;
using DevExpress.Web.ASPxClasses.Internal;

public partial class Running_Totals : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e) {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Price", typeof(int));
        dt.Columns.Add("Quantity", typeof(int));
        dt.Rows.Add(new object[] {0, 200, 100 });
        dt.Rows.Add(new object[] {1, 450, 200 });
        dt.Rows.Add(new object[] {2, 50, 150 });
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.KeyFieldName = "ID";
        Session["PrevValues"] = new List<int>();
        ASPxGridView1.DataBind();
    }
    protected void ASPxGridView1_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) {
        if(e.Column.FieldName == "Total") {
            ASPxGridView grid = sender as ASPxGridView;
            object keyValue = e.GetListSourceFieldValue(grid.KeyFieldName);
            int visibleIndex = GetVisibleRowIndexByKeyValue(grid, keyValue);
            int price = (int)e.GetListSourceFieldValue("Price"); 
            int quantity = Convert.ToInt32(e.GetListSourceFieldValue("Quantity")); 
            e.Value = price * quantity;
            List<int> values = Session["PrevValues"] as List<int>;
            if(visibleIndex > 0) {
                if(values.Count > visibleIndex - 1)
                    e.Value = (int)e.Value - values[visibleIndex - 1];
                else
                    e.Value = (int)e.Value - (int)grid.GetRowValues(visibleIndex - 1, new string[] { "Total" });
            }
            if(values.Count > visibleIndex)
                values[visibleIndex] = (int)e.Value;
            else
                values.Insert(visibleIndex, (int)e.Value);
        }
    }

    private int GetVisibleRowIndexByKeyValue(ASPxGridView grid, object keyValue) {
        string[] keyfield = new string[] { grid.KeyFieldName };
        for(int i = 0;i < grid.VisibleRowCount;i++)
            if(grid.GetRowValues(i, keyfield).Equals(keyValue))
                return i;
        return -1;
    }
}
