' Developer Express Code Central Example:
' How to implement drag and drop between a scheduler and a grid
' 
' This example illustrates how to implement drag and drop for appointments between
' a SchedulerControl and a GridControl. Note that we handle the PreviewMouseDown
' and PreviewMouseMove events of the TableView Class
' (http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewtopic) to
' initiate drag-and-drop operations for the GridControl (we use
' DragDrop.DoDragDrop Method
' (http://msdn.microsoft.com/en-us/library/system.windows.dragdrop.dodragdrop.aspx)
' for this purpose), whereas this operation is initiated automatically for the
' SchedulerControl (if the OptionsCustomization.AllowAppointmentDrag Property
' (http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerOptionsCustomization_AllowAppointmentDragtopic)
' has its default value). The PreviewDrop event of the TableView and
' SchedulerControl.AppointmentDrop Event
' (http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerSchedulerControl_AppointmentDroptopic)
' are handled to process SchedulerDragData dropped in the GridControl and
' SchedulerControl respectively.
' Note: Rows in the GridControl are dragged via
' the RowIndicatior (see the screenshot in the TableViewHitInfo Class
' (http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewHitInfotopic)
' help section) in this example.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E3808

Imports System
Imports System.Data
Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Grid
Imports DevExpress.XtraScheduler

Namespace SchedulerGridDragDrop
    Partial Public Class MainWindow
        Inherits Window

        Private startPoint As Point
        Private startDrag As Boolean = False
        Private currentRowHandle As Integer
        Private dragFromGrid As Boolean = False

        Public Sub New()
            InitializeComponent()

            gridControl1.ItemsSource = DemoUtils.GenerateScheduleTasks()
        End Sub

        Private Sub tableView1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If e.LeftButton <> MouseButtonState.Pressed Then
                Return
            End If

            Me.startPoint = e.GetPosition(Nothing)
            Me.startDrag = IsGridRowAvailable(e)
            Me.currentRowHandle = gridControl1.View.GetRowHandleByMouseEventArgs(e)
        End Sub

        Private Sub tableView1_PreviewMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            Dim position As Point = e.GetPosition(Nothing)

            If Me.startDrag AndAlso e.LeftButton = MouseButtonState.Pressed AndAlso IsGridRowAvailable(e) AndAlso (Math.Abs(position.X - Me.startPoint.X) > 1 OrElse Math.Abs(position.Y - Me.startPoint.Y) > 1) Then
                Me.startDrag = False
                Me.dragFromGrid = True

                DragDrop.DoDragDrop(gridControl1, ObtainSchedulerDataFromRow(currentRowHandle), DragDropEffects.Move)
            End If

            Me.startPoint = e.GetPosition(Nothing)
        End Sub

        Private Sub tableView1_PreviewDrop(ByVal sender As Object, ByVal e As DragEventArgs)
            If e.Data.GetDataPresent(GetType(SchedulerDragData)) Then
                Dim schedulerData As SchedulerDragData = (DirectCast(e.Data.GetData(GetType(SchedulerDragData)), SchedulerDragData))
                Dim dataRow As DataRow = ObtainDataRowFromSchedulerData(schedulerData)
                Dim dataTable As DataTable = CType(gridControl1.ItemsSource, DataTable)
                Dim hitInfo As TableViewHitInfo = tableView1.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))
                Dim rowIndex As Integer = dataTable.Rows.Count

                If hitInfo.RowHandle <> GridControl.InvalidRowHandle Then
                    rowIndex = gridControl1.GetListIndexByRowHandle(hitInfo.RowHandle)
                End If

                If Me.dragFromGrid Then
                    tableView1.DeleteRow(currentRowHandle)
                End If

                dataTable.Rows.InsertAt(dataRow, rowIndex)
                schedulerData.PrimaryAppointment.Delete()
            End If
        End Sub

        Private Sub schedulerControl1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me.dragFromGrid = False
        End Sub

        Private Sub schedulerControl1_AppointmentDrop(ByVal sender As Object, ByVal e As AppointmentDragEventArgs)
            Dim createEventMsg As String = "Creating an event at {0} on {1}."
            Dim moveEventMsg As String = "Moving the event from {0} on {1} to {2} on {3}."

            Dim srcStart As Date = e.SourceAppointment.Start
            Dim newStart As Date = e.EditedAppointment.Start

            Dim msg As String = If(srcStart = Date.MinValue, String.Format(createEventMsg, newStart.ToShortTimeString(), newStart.ToShortDateString()), String.Format(moveEventMsg, srcStart.ToShortTimeString(), srcStart.ToShortDateString(), newStart.ToShortTimeString(), newStart.ToShortDateString()))

            Dim mbResult As MessageBoxResult = DXMessageBox.Show(msg & ControlChars.CrLf & "Proceed?", "Demo", MessageBoxButton.YesNo, MessageBoxImage.Question)

            schedulerControl1.Focus()

            If mbResult = MessageBoxResult.Yes Then
                If Not Me.dragFromGrid Then
                    Return
                End If

                schedulerControl1.Storage.AppointmentStorage.Add(e.EditedAppointment.Copy())
                tableView1.DeleteRow(currentRowHandle)
            End If

            e.Allow = False

        End Sub

        Private Function ObtainSchedulerDataFromRow(ByVal rowHandle As Integer) As Object
            Dim appointments As New AppointmentBaseCollection()
            Dim apt As Appointment = schedulerControl1.Storage.CreateAppointment(AppointmentType.Normal)
            apt.Subject = DirectCast(ObtainDataFromRow(rowHandle, "Subject"), String)
            apt.LabelId = DirectCast(ObtainDataFromRow(rowHandle, "Severity"), Integer)
            apt.StatusId = DirectCast(ObtainDataFromRow(rowHandle, "Priority"), Integer)
            apt.Duration = TimeSpan.FromMinutes(DirectCast(ObtainDataFromRow(rowHandle, "Duration"), Integer))
            apt.Description = DirectCast(ObtainDataFromRow(rowHandle, "Description"), String)
            appointments.Add(apt)
            Return New SchedulerDragData(appointments, 0)
        End Function

        Private Function ObtainDataRowFromSchedulerData(ByVal schedulerData As SchedulerDragData) As DataRow
            Dim apt As Appointment = schedulerData.PrimaryAppointment
            Dim dataTable As DataTable = CType(gridControl1.ItemsSource, DataTable)
            Dim dataRow As DataRow = dataTable.NewRow()

            dataRow("Subject") = apt.Subject
            dataRow("Severity") = apt.LabelId
            dataRow("Priority") = apt.StatusId
            dataRow("Duration") = CInt((apt.Duration.TotalMinutes))
            dataRow("Description") = apt.Description

            Return dataRow
        End Function

        Private Function ObtainDataFromRow(ByVal rowHandle As Integer, ByVal columnName As String) As Object
            Return gridControl1.GetCellValue(rowHandle, gridControl1.Columns(columnName))
        End Function

        Private Function IsGridRowAvailable(ByVal e As MouseEventArgs) As Boolean
            Dim rowHandle As Integer = gridControl1.View.GetRowHandleByMouseEventArgs(e)
            Dim hitInfo As TableViewHitInfo = tableView1.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))

            Return gridControl1.GetRow(rowHandle) IsNot Nothing AndAlso hitInfo.HitTest = TableViewHitTest.RowIndicator
        End Function
    End Class
End Namespace