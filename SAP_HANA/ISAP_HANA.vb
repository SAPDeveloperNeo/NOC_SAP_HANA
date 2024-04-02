Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace SAP_HANA

    Public Interface ISAP_HANA



        Sub Form_Creation_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        '  Sub Form_ItemPressedEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        'Sub Form_TMenuFindEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Sub Form_TMenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Sub Form_Process_ItemEvents(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Sub Form_Report_Display(ByRef paramarraylist As Object)
        'Sub ShowList()
        'Sub OpenLink(ByRef isNew As Boolean, ByRef DocEntry As String)
        Property ObjectCode() As String

    End Interface
End Namespace
