Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.IO
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports SAPbouiCOM

Namespace SAP_HANA

    Public Class Acdnt : Implements ISAP_HANA

#Region "Variable Declaration"

        Private __oApplication As SAPbouiCOM.Application
        Public __oCompany As SAPbobsCOM.Company
        Private oForm As SAPbouiCOM.Form
        Public grd1 As SAPbouiCOM.Grid

#End Region

#Region "Constructors"

        Public Sub New(ByRef sApp As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
            __oApplication = sApp
            __oCompany = oCompany '.Company.GetDICompany()
        End Sub

        Private Property ISAP_HANA_ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property

#End Region


        Public Sub Form_Creation(ByVal IsBaseForm As SAPbouiCOM.Form, ByVal sQuery As String, ByVal FormTitle As String)
            Try

                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\AccidentSettlement.srf"
                Dim sFormName As String = "TruckRevHist"
                Dim FormUID1 As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("Acdnt", __oApplication.Forms.ActiveForm.TypeCount)
                oForm.Title = IIf(FormTitle <> "", FormTitle, "AccidentSettlement")
                If Not String.IsNullOrEmpty(FormUID1) Then

                    oForm.Freeze(True)

                    oForm.EnableMenu("1290", False)        '//Move First Menu
                    oForm.EnableMenu("1289", False)        '//Move Previous Menu
                    oForm.EnableMenu("1288", False)        '//Move Next Menu
                    oForm.EnableMenu("1291", False)       '//Move Last Menu
                    oForm.EnableMenu("1299", False)        '//Close Row
                    oForm.EnableMenu("4870", False)        '//Filter Table
                    oForm.EnableMenu("1293", False)        '//Delete Row
                    oForm.EnableMenu("1281", False)       '//Find Menu
                    oForm.EnableMenu("1282", False)

                    oForm.EnableMenu("1283", False)
                    oForm.EnableMenu("1284", False)
                    oForm.EnableMenu("1286", False)
                    oForm.EnableMenu("1293", False)
                    oForm.EnableMenu("1299", False)

                    oForm.DataSources.DataTables.Add("Records1")


                    'oForm.DataSources.UserDataSources.Add("db5", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 2)


                    'Dim chkSelect As SAPbouiCOM.CheckBox = oForm.Items.Item("chkSelect").Specific
                    'chkSelect.DataBind.SetBound(True, "", "db5")

                    'oForm.Items.Item("chkSelect").Visible = False

                    '-----------Fill The Grid---------------------
                    '                    Dim grd1 As SAPbouiCOM.Grid = oForm.Items.Item("grd1").Specific
                    '                    Dim SqlQuery As String = ""

                    '                    'SqlQuery = sQuery + "('" + __bobCompany.UserSignature.ToString() + "')"

                    '                    SqlQuery = "Select ""DocEntry"",""DocNum"", ""U_Date"" as ""Date"" ,""U_TRegNo"" as ""TruckNumber"",""U_Trailor"" as ""Trailor"",""U_Owner"" as ""Owner"",""U_Driver"" as ""Driver"",""U_CardCode"" as ""U_CardCode"",
                    '""U_Product"" as ""Product"",""U_Active"" as ""Active"",""U_ModelNum"" as ""ModelNum"",
                    '""U_TType"" as ""TruckType"" ,""U_Enum"" as ""EnginNum"",""U_ChNum"" as ""Chassis Number""
                    ',""U_OTT"" as ""Old TT Number"",""U_REVNO"" as ""Old Rev Num"",""U_NEWREVNO"" as ""NewRevNum"" from ""@SAP_OTMREV"" where ""DocEntry""='" & sQuery & "'"

                    '                    oForm.DataSources.DataTables.Item("Records1").ExecuteQuery(SqlQuery)

                    '                    grd1.DataTable = oForm.DataSources.DataTables.Item("Records1")


                    '                    Dim ds As DataTable
                    '                    ds = CType(oForm.DataSources.DataTables.Item("Records3"), DataTable)
                    '                    For iCol As Integer = 0 To grd1.Columns.Count - 1
                    '                        grd1.Columns.Item(iCol).Editable = False
                    '                    Next




                    ' grd.Columns.Item("Select").Editable = True
                    ' grd.Columns.Item("Select").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox

                    'Dim oCol1 As SAPbouiCOM.EditTextColumn
                    'oCol1 = grd3.Columns.Item("Invoice")
                    'oCol1.LinkedObjectType = "13"
                    'Dim oCol2 As SAPbouiCOM.EditTextColumn
                    'oCol2 = grd2.Columns.Item("Invoice")
                    'oCol2.LinkedObjectType = "13"
                    oForm.Freeze(False)
                    oForm.Refresh()
                    oForm.Update()
                End If
            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Choose_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)
                Dim m1 As SAPbouiCOM.Grid = oForm.Items.Item("1").Specific
                Dim aFlag As Boolean = False


                Dim Codes As String = ""
                Dim Names As String = ""


                Dim oCheck As SAPbouiCOM.CheckBoxColumn = m1.Columns.Item(0)
                Dim Code As SAPbouiCOM.EditTextColumn = m1.Columns.Item(1)
                Dim Name As SAPbouiCOM.EditTextColumn = m1.Columns.Item(2)

                For iRow As Integer = m1.Rows.Count To 1 Step -1
                    If oCheck.IsChecked(iRow - 1) = True Then
                        Codes = Code.GetText(iRow - 1).ToString.Trim
                        Names = Name.GetText(iRow - 1).ToString.Trim

                        Exit For
                    End If
                Next


                'If DocNum.Length > 0 Then
                '    DocNum = DocNum.Substring(0, DocNum.Length - 1)
                'End If
                oForm.Freeze(False)

                Dim oRefHashtable As Hashtable = New Hashtable

                oRefHashtable.Clear()
                oRefHashtable.Add("Code", Codes)
                oRefHashtable.Add("Name", Names)

                SendData(oRefHashtable, IsBaseForm)
                oForm.Close()

            Catch ex As Exception
                oForm.Freeze(False)
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub



        Private Sub ISAP_HANA_Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            'Throw New NotImplementedException()
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\AccidentSettlement.srf"
                Dim sFormName As String = "Acdnt"
                Dim FormUID1 As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("Acdnt", __oApplication.Forms.ActiveForm.TypeCount)
                '  oForm.Title = IIf(FormTitle <> "", FormTitle, "AccidentSettlement")
                If Not String.IsNullOrEmpty(FormUID1) Then

                    oForm.Freeze(True)

                    oForm.EnableMenu("1290", False)        '//Move First Menu
                    oForm.EnableMenu("1289", False)        '//Move Previous Menu
                    oForm.EnableMenu("1288", False)        '//Move Next Menu
                    oForm.EnableMenu("1291", False)       '//Move Last Menu
                    oForm.EnableMenu("1299", False)        '//Close Row
                    oForm.EnableMenu("4870", False)        '//Filter Table
                    oForm.EnableMenu("1293", False)        '//Delete Row
                    oForm.EnableMenu("1281", False)       '//Find Menu
                    oForm.EnableMenu("1282", False)

                    oForm.EnableMenu("1283", False)
                    oForm.EnableMenu("1284", False)
                    oForm.EnableMenu("1286", False)
                    oForm.EnableMenu("1293", False)
                    oForm.EnableMenu("1299", False)

                    oForm.DataSources.DataTables.Add("Records1")


                    oForm.Freeze(False)
                    oForm.Refresh()
                    oForm.Update()
                End If
            Catch ex As Exception
                oForm.Freeze(False)
                oForm.Refresh()
                oForm.Update()
                __oApplication.MessageBox("[MenuEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try


        End Sub

        Private Sub ISAP_HANA_Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Throw New NotImplementedException()
        End Sub

        Private Sub ISAP_HANA_Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Private Sub ISAP_HANA_Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = True Then
                    If pVal.ItemUID = "4" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Choose_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        ShowData_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_5" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then

                        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                        Dim m1 As SAPbouiCOM.Grid = oForm.Items.Item("grd1").Specific
                        Dim oCheck As SAPbouiCOM.CheckBoxColumn = m1.Columns.Item(0)
                        Dim Code As SAPbouiCOM.EditTextColumn = m1.Columns.Item(1)
                        For iRow As Integer = m1.Rows.Count To 1 Step -1
                            If oCheck.IsChecked(iRow - 1) = True Then
                                Dim docentry = Code.GetText(iRow - 1).ToString.Trim
                                GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, docentry)

                                Exit For
                            End If
                        Next

                        'grd1 = oForm.Items.Item("grd1").Specific



                    End If
                End If
            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub ISAP_HANA_Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub

        Private Sub ShowData_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oForm.Freeze(True)
                '-----------Fill The Grid---------------------
                '-----------Fill The Grid---------------------


                Dim SqlQuery As String = ""

                'SqlQuery = sQuery + "('" + __bobCompany.UserSignature.ToString() + "')"

                SqlQuery = "Select  '' ""Select"" ,""DocEntry"",""DocNum"", ""DocDate"" as ""Date"" ,""CardCode"" ,""CardName"" 
  from opdn where ""U_ACDNT""='Y' AND  ""U_RETFLAG""='N'"
                grd1 = oForm.Items.Item("grd1").Specific
                oForm.DataSources.DataTables.Item("Records1").ExecuteQuery(SqlQuery)

                grd1.DataTable = oForm.DataSources.DataTables.Item("Records1")


                For iCol As Integer = 0 To grd1.Columns.Count - 1
                        grd1.Columns.Item(iCol).Editable = False
                    Next
                    Dim oCol As SAPbouiCOM.EditTextColumn


                'Dim oCol1 As SAPbouiCOM.EditTextColumn
                '    oCol1 = grd3.Columns.Item("Invoice")
                '    oCol1.LinkedObjectType = "13"

                grd1.Columns.Item("Select").Editable = True
                grd1.Columns.Item("Select").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox

                oForm.Freeze(False)
            Catch ex As Exception
                oForm.Freeze(False)
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub GoodReceiptPO_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""





                ''----Fetch the Reocrd for Purchase Order Creation----
                Sql = "CALL SAP_GET_GRPO_Data ('" + DocEntry + "')"
                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    Dim oGoodReceiptPO As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns)

                    oGoodReceiptPO.CardCode = oRs.Fields.Item("CardCode").Value.ToString()
                    oGoodReceiptPO.DocDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oGoodReceiptPO.DocDueDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oGoodReceiptPO.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                    oGoodReceiptPO.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("U_ITDocE").Value & ""
                    oGoodReceiptPO.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("U_ITDocN").Value & ""
                    ' oGoodReceiptPO.NumAtCard = oRs.Fields.Item("U_Ref").Value & ""

                    'removed : cause -> multiple values updating on single column
                    oGoodReceiptPO.UserFields.Fields.Item("U_TRNO").Value = oRs.Fields.Item("U_TRNO").Value & ""


                    ''---This is for Purchase Order Line---
                    Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oGoodReceiptPO.Lines
                    While oRs.EoF = False

                        oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                        oPurchaseOrder_Line.Quantity = oRs.Fields.Item("Quantity").Value
                        oPurchaseOrder_Line.UnitPrice = oRs.Fields.Item("Price").Value

                        oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("OcrCode").Value & ""
                        oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("OcrCode2").Value & ""
                        oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("OcrCode3").Value & ""
                        oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("OcrCode4").Value & ""
                        oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("OcrCode5").Value & ""
                        oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""

                        oPurchaseOrder_Line.BaseLine = oRs.Fields.Item("LineNum").Value & ""
                        oPurchaseOrder_Line.BaseType = "20"
                        oPurchaseOrder_Line.BaseEntry = oRs.Fields.Item("BaseEntry").Value & ""


                        oPurchaseOrder_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oGoodReceiptPO.Add()
                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error: In Generating Good Return- " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else

                        Dim SqlQuery As New System.Text.StringBuilder
                        Dim str As String = "Update opdn Set ""U_RETFLAG""= 'Y' Where ""DocEntry""='" + DocEntry + "'"


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(str)
                        __oApplication.StatusBar.SetText("Good Return Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


    End Class

End Namespace

