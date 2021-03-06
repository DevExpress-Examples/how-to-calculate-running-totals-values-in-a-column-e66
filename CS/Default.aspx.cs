using System;
using System.Data;
using System.Collections.Generic;
using DevExpress.Web.ASPxGridView;


public partial class Running_Totals : System.Web.UI.Page 
{
	private int previousValue = 0;

    protected void Page_Init(object sender, EventArgs e) {
		ASPxGridView1.DataSource = CreateDataSource();
        ASPxGridView1.KeyFieldName = "ID";
        ASPxGridView1.DataBind();
    }
	
    protected void ASPxGridView1_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) {
        if(e.Column.FieldName == "Total") {
			if (IsFirstRow(e))
				previousValue = 0;
			
			int quantity = (int)e.GetListSourceFieldValue("Quantity");
			int price = (int)e.GetListSourceFieldValue("Price");
			previousValue = quantity * price - previousValue;
			e.Value = previousValue;
        }
    }
	private bool IsFirstRow(ASPxGridViewColumnDataEventArgs e) {
		ASPxGridView grid = e.Column.Grid;
		object keyValue = e.GetListSourceFieldValue(grid.KeyFieldName);
		return grid.FindVisibleIndexByKeyValue(keyValue) == 0;
	}

	private DataTable CreateDataSource() {
		DataTable dt = new DataTable();
		dt.Columns.Add("ID", typeof(int));
		dt.Columns.Add("Price", typeof(int));
		dt.Columns.Add("Quantity", typeof(int));
		dt.Rows.Add(new object[] { 0, 200, 100 });
		dt.Rows.Add(new object[] { 1, 450, 200 });
		dt.Rows.Add(new object[] { 2, 50, 150 });
		return dt;
	}
}
