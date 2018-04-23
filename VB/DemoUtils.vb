Imports Microsoft.VisualBasic
Imports System
Imports System.Data

Namespace SchedulerGridDragDrop
	Public Class DemoUtils
		Public Shared RandomInstance As New Random()

		Private Shared taskDescriptions() As String = { "Implementing Developer Express MasterView control into Accounting System.", "Web Edition: Data Entry Page. The issue with date validation.", "Payables Due Calculator. It is ready for testing.", "Web Edition: Search Page. It is ready for testing.", "Main Menu: Duplicate Items. Somebody has to review all menu items in the system.", "Receivables Calculator. Where can I found the complete specs", "Ledger: Inconsistency. Please fix it.", "Receivables Printing. It is ready for testing.", "Screen Redraw. Somebody has to look at it.", "Email System. What library we are going to use?", "Adding New Vendors Fails. This module doesn't work completely!", "History. Will we track the sales history in our system?", "Main Menu: Add a File menu. File menu is missed!!!", "Currency Mask. The current currency mask in completely inconvinience.", "Drag & Drop. In the schedule module drag & drop is not available.", "Data Import. What competitors databases will we support?", "Reports. The list of incomplete reports.", "Data Archiving. This features is still missed in our application", "Email Attachments. How to add the multiple attachment? I did not find a way to do it.", "Check Register. We are using different paths for different modules.", "Data Export. Our customers asked for export into Excel"}

		Public Shared Function GenerateScheduleTasks() As DataTable
			Dim table As New DataTable()
			table = New DataTable("ScheduleTasks")
			table.Columns.Add("ID", GetType(Integer))
			table.Columns.Add("Subject", GetType(String))
			table.Columns.Add("Severity", GetType(Integer))
			table.Columns.Add("Priority", GetType(Integer))
			table.Columns.Add("Duration", GetType(Integer))
			table.Columns.Add("Description", GetType(String))
			For i As Integer = 0 To 20
				Dim description As String = taskDescriptions(i)
				Dim index As Integer = description.IndexOf("."c)
				Dim subject As String
				If index <= 0 Then
					subject = "task" & Convert.ToInt32(i + 1)
				Else
					subject = description.Substring(0, index)
				End If
				table.Rows.Add(New Object() { i + 1, subject, RandomInstance.Next(3), RandomInstance.Next(3), Math.Max(1, RandomInstance.Next(8)) * 60, description })
			Next i
			Return table
		End Function
	End Class
End Namespace