
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.IO
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports System.Text

Namespace SAP_HANA
    Public Class C_ChamberAllocation : Implements ISAP_HANA

#Region "Constructors"
        Private __oApplication As SAPbouiCOM.Application
        Public __oCompany As SAPbobsCOM.Company
        Private oForm As SAPbouiCOM.Form
        Private oItem As SAPbouiCOM.Item
        Dim oLabel As SAPbouiCOM.StaticText
        Dim ocombo As SAPbouiCOM.ComboBox
        Dim oEditText1 As SAPbouiCOM.EditText
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Dim oButton As SAPbouiCOM.Button
        Dim oFolder As SAPbouiCOM.Folder
        Dim oItem1 As SAPbouiCOM.Item
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim sCFL_ID As String
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
        Dim oDataTable1 As SAPbouiCOM.DataTable
        Dim oDataTable2 As SAPbouiCOM.DataTable
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Private oMatrix As SAPbouiCOM.Matrix
        Private oColumns As SAPbouiCOM.Columns
        Private odataset As SAPbouiCOM.DataSource
        Dim oComboBox As SAPbouiCOM.ComboBox

        Public Property ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property

        Public Sub New(ByRef sApp As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
            __oApplication = sApp
            __oCompany = oCompany '.Company.GetDICompany()
        End Sub

#End Region

#Region "Const Enumeration"
        Public Enum menuID
            Next_Record = 1288
            Previous_Record = 1289
            First_Record = 1290
            Last_Record = 1291
            Duplicate_Row = 1287
            Duplicate = 1287
            Delete_Row = 1293
            Add_Row = 1292
            Remove = 1283
            Find = 1281
            Add = 1282
            Undo = 769
            Cut = 771
            Copy = 772
            Paste = 773
            Delete = 774
        End Enum

#End Region


        Public Sub Form_Creation(ByVal TruckNum As String, ByVal DocNum As String, ByVal BaseType As String)
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\ChamberAllocation.srf"
                Dim sFormName As String = "SAP_UDO_OPKL"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OPKL", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    DeativateMenu(oForm)
                    oForm.Freeze(True)

                    Try

                        oForm.Mode = BoFormMode.fm_ADD_MODE

                        Dim ToDate As Date = Nothing
                        Dim sc As String = __oApplication.Company.ServerDate
                        ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                        Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_19").Specific
                        PODate.String = ToDate.ToString("yyyyMMdd")


                        oForm.Items.Item("Item_17").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OPKL")
                        oForm.Items.Item("Item_18").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OPKL")

                        Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                        ' AddRowInMatrix(oForm, "@SAP_DLN1", "m1")


                        oForm.Items.Item("Item_9").Specific.Value = TruckNum
                        oForm.Items.Item("Item_24").Specific.Value = DocNum
                        ' oForm.Items.Item("Item_19").Specific.Value = DDate


                        Dim dbsrcRow As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_PKL1")
                        Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OPKL")


                        Dim SQL As String = ""
                        SQL = "CALL SAP_GET_ChamberAllocation_ItemList ('" + DocNum + "')"
                        Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs1Count.DoQuery(SQL)
                        If oRs1Count.RecordCount > 0 Then
                            Dim ItemCode As String = oRs1Count.Fields.Item("ItemCode").Value
                            Dim Dscription As String = oRs1Count.Fields.Item("Dscription").Value
                            Dim CardCode As String = oRs1Count.Fields.Item("CardCode").Value
                            Dim CardName As String = oRs1Count.Fields.Item("CardName").Value
                            Dim WhsCode As String = oRs1Count.Fields.Item("WhsCode").Value
                            Dim WhsName As String = oRs1Count.Fields.Item("WhsName").Value
                            dbsrc.SetValue("U_WhsCode", dbsrc.Offset, WhsCode & "")
                            dbsrc.SetValue("U_WhsName", dbsrc.Offset, WhsName & "")



                            Dim SQLCAP As String = ""
                            SQLCAP = "CALL SAP_GET_ChamberAllocation ('" + TruckNum + "')"
                            Dim oRs1CAP As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs1CAP.DoQuery(SQLCAP)
                                If oRs1CAP.RecordCount > 0 Then
                                    For CAP As Integer = 1 To oRs1CAP.RecordCount

                                        dbsrcRow.Offset = dbsrc.Size - 1
                                        m1.AddRow(1, m1.VisualRowCount)
                                        m1.GetLineData(m1.VisualRowCount)

                                    dbsrcRow.SetValue("U_CHN", dbsrc.Offset, oRs1CAP.Fields.Item("Chamber").Value & "")
                                    dbsrcRow.SetValue("U_ItemCode", dbsrc.Offset, ItemCode & "")
                                    dbsrcRow.SetValue("U_ItemName", dbsrc.Offset, Dscription & "")
                                    dbsrcRow.SetValue("U_CardCode", dbsrc.Offset, CardCode & "")

                                    dbsrcRow.SetValue("U_CardName", dbsrc.Offset, CardName & "")
                                    dbsrcRow.SetValue("U_WhsCode", dbsrc.Offset, oRs1CAP.Fields.Item("WhsCode").Value & "")
                                    dbsrcRow.SetValue("U_WhsName", dbsrc.Offset, oRs1CAP.Fields.Item("WhsName").Value & "")


                                    dbsrcRow.SetValue("U_Dip", dbsrc.Offset, oRs1CAP.Fields.Item("Dip").Value & "")
                                    dbsrcRow.SetValue("U_CAP", dbsrc.Offset, oRs1CAP.Fields.Item("Qty").Value & "")
                                    dbsrcRow.SetValue("U_Qty", dbsrc.Offset, oRs1CAP.Fields.Item("Qty").Value & "")


                                    dbsrcRow.SetValue("U_Density", dbsrc.Offset, oRs1CAP.Fields.Item("Density").Value & "")
                                    dbsrcRow.SetValue("U_FBP", dbsrc.Offset, oRs1CAP.Fields.Item("FBP").Value & "")
                                    dbsrcRow.SetValue("U_Temp", dbsrc.Offset, oRs1CAP.Fields.Item("Temp").Value & "")


                                    m1.SetLineData(m1.VisualRowCount)
                                        m1.FlushToDataSource()
                                    oRs1CAP.MoveNext()
                                Next
                                End If

                        End If

                        'If Count = 0 Then
                        '    Count = 1
                        'End If

                        '   Qty = Qty / Count










                    Catch ex As Exception
                        __oApplication.MessageBox("[MenuEvent] - " & ex.Message, 1, "Ok", "", "")
                    End Try
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



        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\ChamberAllocation.srf"
                Dim sFormName As String = "SAP_UDO_OPKL"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OPKL", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    oForm.Freeze(True)
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

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_5" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        QtyCalculation(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_5" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TempLostFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_15" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        DensityLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_7" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_CardCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_Itemcode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_WhsCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        FBPostFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        WhsCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_7" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_CardCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_ItemCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_WhsCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_WhsCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try

        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub


        Private Sub Matrix1_CardCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()
                Dim SqlQuery2 As String

                SqlQuery2 = "CALL SAP_GET_ChamberAllocation_ItemList ('" + oForm.Items.Item("Item_24").Specific.Value + "')"

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "CardCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("CardCode").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If






            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_CardCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_PKL1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_CardCode", dbsrc.Offset, oDataTable.GetValue("CardCode", 0) & "")
                                dbsrc.SetValue("U_CardName", dbsrc.Offset, oDataTable.GetValue("CardName", 0) & "")


                                m1.SetLineData(pVal.Row)
                                m1.FlushToDataSource()

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_ItemCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()
                Dim SqlQuery2 As String

                SqlQuery2 = "CALL SAP_GET_ChamberAllocation_ItemList ('" + oForm.Items.Item("Item_24").Specific.Value + "')"

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "ItemCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("ItemCode").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If






            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_Itemcode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_PKL1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oDataTable.GetValue("ItemCode", 0) & "")
                                dbsrc.SetValue("U_ItemName", dbsrc.Offset, oDataTable.GetValue("ItemName", 0) & "")

                                m1.SetLineData(pVal.Row)
                                m1.FlushToDataSource()

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_WhsCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()
                Dim SqlQuery2 As String

                SqlQuery2 = "CALL SAP_GET_PickList_Warehouse ('" + oForm.Items.Item("Item_24").Specific.Value + "')"

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "WhsCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("WhsCode").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If






            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_WhsCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_PKL1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_WhsCode", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_WhsName", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")

                                m1.SetLineData(pVal.Row)
                                m1.FlushToDataSource()

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub WhsCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OPKL")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_WhsCode", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_WhsName", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")
                                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                                For ADDRow As Integer = 1 To m1.RowCount
                                    Try
                                        m1.Columns.Item("Col_10").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsCode", 0) & ""
                                        m1.Columns.Item("Col_12").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsName", 0) & ""
                                    Catch ex As Exception

                                    End Try

                                Next

                            End If

                        Catch ex As Exception

                        End Try
                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub





        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                PKLTRNO = ""
                PKLDOCNUM = ""

                PKLTRNO = oForm.Items.Item("Item_9").Specific.Value
                PKLDOCNUM = oForm.Items.Item("Item_24").Specific.Value




            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)




                If oForm.Mode = BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then
                    'Dim Codes As String = CHAMBERDOCENTRY
                    'Dim Names As String = CHAMBERDOCNUM
                    Dim SqlQuery2 As New StringBuilder

                    SqlQuery2.Append("SELECT T0.""DocEntry"", T0.""DocNum"" FROM ""@SAP_OPKL""  T0 ")
                    SqlQuery2.Append("WHERE T0.""U_DocNum""='" + PKLDOCNUM + "' And T0.""U_TruckNum"" ='" + PKLTRNO + "'  ")

                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRset.DoQuery(SqlQuery2.ToString)

                    If oRset.RecordCount > 0 Then
                        Dim oRefHashtable As Hashtable = New Hashtable
                        oRefHashtable.Clear()
                        oRefHashtable.Add("Code", oRset.Fields.Item("DocNum").Value)
                        oRefHashtable.Add("Name", oRset.Fields.Item("DocEntry").Value)
                        SendData(oRefHashtable, IsBaseForm)
                        oForm.Close()

                        PKLTRNO = ""
                        PKLDOCNUM = ""

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub QtyCalculation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific





                Dim Query As String = ""
                Query = "Select (IFNULL(T1.""U_CAP"",0)/IFNULL(T1.""U_OILDIP"",0)) As ""QtyValu"" ,""U_OILDIP"" From ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP1""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where (T0.""U_VC"" ='" + oForm.Items.Item("Item_9").Specific.Value + "' And  T1.""U_CHN"" ='" + m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value + "')"


                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then

                    Dim OilDip As Decimal = oRs.Fields.Item("U_OILDIP").Value

                    If CDec(m1.Columns.Item("Col_5").Cells.Item(pVal.Row).Specific.Value) > OilDip Then
                        __oApplication.MessageBox("Can Not Enter Dip More Than Oil Dip (" + OilDip.ToString + ")")
                        m1.Columns.Item("Col_5").Cells.Item(pVal.Row).Specific.Value = 0
                        BubbleEvent = False
                        Exit Sub
                    End If


                    Dim Value As Decimal = oRs.Fields.Item("QtyValu").Value
                    m1.Columns.Item("Col_3").Cells.Item(pVal.Row).Specific.Value = (Value * CDec(m1.Columns.Item("Col_5").Cells.Item(pVal.Row).Specific.Value))
                End If
                Marshal.ReleaseComObject(oRs)
            Catch ex As Exception

            End Try
        End Sub

        Private Sub TempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_4").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_5").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub


        Private Sub DensityLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_6").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_15").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub FBPostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_9").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_2").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub




    End Class

End Namespace
