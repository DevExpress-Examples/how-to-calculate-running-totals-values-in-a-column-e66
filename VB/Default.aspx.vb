Imports System
Imports System.Data
Imports System.Collections.Generic
Imports DevExpress.Web.ASPxGridView


Partial Public Class Running_Totals
    Inherits System.Web.UI.Page

    Private previousValue As Integer = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        ASPxGridView1.DataSource = CreateDataSource()
        ASPxGridView1.KeyFieldName = "ID"
        ASPxGridView1.DataBind()
    End Sub

    Protected Sub ASPxGridView1_CustomUnboundColumnData(ByVal sender As Object, ByVal e As ASPxGridViewColumnDataEventArgs)
        If e.Column.FieldName = "Total" Then
            If IsFirstRow(e) Then
                previousValue = 0
            End If

            Dim quantity As Integer = CInt((e.GetListSourceFieldValue("Quantity")))
            Dim price As Integer = CInt((e.GetListSourceFieldValue("Price")))
            previousValue = quantity * price - previousValue
            e.Value = previousValue
        End If
    End Sub
    Private Function IsFirstRow(ByVal e As ASPxGridViewColumnDataEventArgs) As Boolean
        Dim grid As ASPxGridView = e.Column.Grid
        Dim keyValue As Object = e.GetListSourceFieldValue(grid.KeyFieldName)
        Return grid.FindVisibleIndexByKeyValue(keyValue) = 0
    End Function

    Private Function CreateDataSource() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("Price", GetType(Integer))
        dt.Columns.Add("Quantity", GetType(Integer))
        dt.Rows.Add(New Object() { 0, 200, 100 })
        dt.Rows.Add(New Object() { 1, 450, 200 })
        dt.Rows.Add(New Object() { 2, 50, 150 })
        Return dt
    End Function
End Class
