Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web.ASPxTabControl
Imports DevExpress.Xpo
Imports System.Drawing
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxEditors
Imports System.Collections.Generic
Imports DevExpress.Data
Imports DevExpress.Web.ASPxDataView
Imports System.Collections
Imports System.Data.OleDb
Imports DevExpress.Web.ASPxClasses.Internal

Partial Public Class Running_Totals
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Dim dt As New DataTable()
		dt.Columns.Add("ID", GetType(Integer))
		dt.Columns.Add("Price", GetType(Integer))
		dt.Columns.Add("Quantity", GetType(Integer))
		dt.Rows.Add(New Object() {0, 200, 100 })
		dt.Rows.Add(New Object() {1, 450, 200 })
		dt.Rows.Add(New Object() {2, 50, 150 })
		ASPxGridView1.DataSource = dt
		ASPxGridView1.KeyFieldName = "ID"
		Session("PrevValues") = New List(Of Integer)()
		ASPxGridView1.DataBind()
	End Sub
	Protected Sub ASPxGridView1_CustomUnboundColumnData(ByVal sender As Object, ByVal e As ASPxGridViewColumnDataEventArgs)
		If e.Column.FieldName = "Total" Then
			Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
			Dim keyValue As Object = e.GetListSourceFieldValue(grid.KeyFieldName)
			Dim visibleIndex As Integer = GetVisibleRowIndexByKeyValue(grid, keyValue)
			Dim price As Integer = CInt(Fix(e.GetListSourceFieldValue("Price")))
			Dim quantity As Integer = Convert.ToInt32(e.GetListSourceFieldValue("Quantity"))
			e.Value = price * quantity
			Dim values As List(Of Integer) = TryCast(Session("PrevValues"), List(Of Integer))
			If visibleIndex > 0 Then
				If values.Count > visibleIndex - 1 Then
					e.Value = CInt(Fix(e.Value)) - values(visibleIndex - 1)
				Else
					e.Value = CInt(Fix(e.Value)) - CInt(Fix(grid.GetRowValues(visibleIndex - 1, New String() { "Total" })))
				End If
			End If
			If values.Count > visibleIndex Then
				values(visibleIndex) = CInt(Fix(e.Value))
			Else
				values.Insert(visibleIndex, CInt(Fix(e.Value)))
			End If
		End If
	End Sub

	Private Function GetVisibleRowIndexByKeyValue(ByVal grid As ASPxGridView, ByVal keyValue As Object) As Integer
		Dim keyfield() As String = { grid.KeyFieldName }
		For i As Integer = 0 To grid.VisibleRowCount - 1
			If grid.GetRowValues(i, keyfield).Equals(keyValue) Then
				Return i
			End If
		Next i
		Return -1
	End Function
End Class
