<!-- default file list -->
*Files to look at*:

* [DemoUtils.cs](./CS/DemoUtils.cs) (VB: [DemoUtils.vb](./VB/DemoUtils.vb))
* [MainWindow.xaml](./CS/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/MainWindow.xaml.cs) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
<!-- default file list end -->
# How to implement drag and drop between a legacy scheduler and a grid


<p><strong>Important:</strong> This example demonstrates a manual drag-and-drop implementation for <strong>DXGrid</strong> and a <a href="https://documentation.devexpress.com/WPF/8649/Controls-and-Libraries/Scheduler-legacy">legacy Scheduler control</a>. To learn more on how to implement a similar functionality using our new <a href="https://documentation.devexpress.com/WPF/114881/Controls-and-Libraries/Scheduler">Scheduler</a> and the built-in <a href="https://documentation.devexpress.com/WPF/11346/Controls-and-Libraries/Data-Grid/Drag-and-Drop">Drag-and-Drop</a> functionality which became available in version <strong>17.2</strong>, refer to the <strong>Data Grid > Scheduler Integration</strong> <a href="https://documentation.devexpress.com/WPF/14978/What-s-Installed/Interactive-Demos">interactive demo</a>.<br><br>This example illustrates how to implement drag and drop for appointments between a SchedulerControl and a GridControl. Note that we handle the <strong>PreviewMouseDown </strong>and <strong>PreviewMouseMove </strong>events of the <a href="http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewtopic"><u>TableView Class</u></a> to initiate drag-and-drop operations for the GridControl (we use <a href="http://msdn.microsoft.com/en-us/library/system.windows.dragdrop.dodragdrop.aspx"><u>DragDrop.DoDragDrop Method</u></a> for this purpose), whereas this operation is initiated automatically for the SchedulerControl (if the <a href="http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerOptionsCustomization_AllowAppointmentDragtopic"><u>OptionsCustomization.AllowAppointmentDrag Property</u></a> has its default value). The <strong>PreviewDrop </strong>event of the TableView and <a href="http://documentation.devexpress.com/#WPF/DevExpressXpfSchedulerSchedulerControl_AppointmentDroptopic"><u>SchedulerControl.AppointmentDrop Event</u></a> are handled to process <strong>SchedulerDragData </strong>dropped in the GridControl and SchedulerControl respectively.</p>
<p><strong>Note:</strong> Rows in the GridControl are dragged via the RowIndicatior (see the screenshot in the <a href="http://documentation.devexpress.com/#WPF/clsDevExpressXpfGridTableViewHitInfotopic"><u>TableViewHitInfo Class</u></a> help section) in this example.</p>

<br/>


