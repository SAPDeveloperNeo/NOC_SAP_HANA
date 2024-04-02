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


Namespace SAP_HANA
    Public Class C_CalibrationProcess : Implements ISAP_HANA

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

        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\CalibrationProcess.srf"
                Dim sFormName As String = "SAP_UDO_OCALP"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OCALP", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_4", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)
                    oForm.Freeze(True)
                    oForm.Mode = BoFormMode.fm_ADD_MODE

                    DefulatSetting(oForm.UniqueID, BubbleEvent)




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

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "Item_20" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        Fill_Matrix(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FixedAssetCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Truck_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_1_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "showrev" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_22" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_2_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_29" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_3_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_28" And pVal.ColUID = "Chamber" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m3" And pVal.ColUID = "Chamber" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        Oil_AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)

                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "Item_1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FixedAssetCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    End If
                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)

                    End If

                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try

                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_34").Specific
                Dim docentry As Integer
                docentry = oForm.Items.Item("Item_4").Specific.value
                If pVal.ItemUID = "showrev" Then


                    IsBaseForm = oForm
                    IsBaseItemID = "Item_4"
                    IsBaseUDF = "DocEntry"
                    IsBase_DN_UDF = "DocNum"
                    Dim c_CALPRevHist As CalpRevHist = New CalpRevHist(__Application, __bobCompany)
                    c_CALPRevHist.Form_Creation(IsBaseForm, docentry.ToString, "List of Calibration Revision History")
                    'ElseIf pVal.ItemUID = "1" And oForm.Mode <> BoFormMode.fm_FIND_MODE Then

                    '    If chkEXT.Checked Then
                    '        chkEXT.Checked = False
                    '        Exit Sub
                    '    End If
                    '    '  ARInvoice_Creation(FormUID, pVal, BubbleEvent, docentry, "")

                End If

                ' Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_39").Specific
                If chkEXT.Checked Then
                    chkEXT.Checked = False
                    ' oForm.Mode = BoFormMode.fm_OK_MODE
                End If
            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try

                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then


                    Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OCALP")
                    BubbleEvent = True
                    Dim trsting As String = "CP"
                    Dim trnumber As String = "00000001"
                    Dim ithReturnValue As Integer
                    Dim docentry As Integer
                    Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_34").Specific
                    'Dim txtTruckNo As SAPbouiCOM.EditText = oForm.Items.Item("Item_9").Specific
                    'Dim txtTranspCode As SAPbouiCOM.EditText = oForm.Items.Item("Item_10").Specific
                    'Dim txtTranspName As SAPbouiCOM.EditText = oForm.Items.Item("Item_12").Specific
                    'Dim txtModalNo As SAPbouiCOM.EditText = oForm.Items.Item("Item_24").Specific
                    'Dim txtVType As SAPbouiCOM.EditText = oForm.Items.Item("Item_26").Specific
                    'Dim txtbranch As SAPbouiCOM.EditText = oForm.Items.Item("Item_37").Specific
                    'If txtbranch.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Branch can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If

                    'If txtTruckNo.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Truck No. can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If

                    'If txtTranspCode.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Transporter code can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If

                    'If txtTranspName.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Transporter Name can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If

                    'If txtModalNo.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Model No. can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If

                    'If txtVType.Value = "" Then
                    '    __oApplication.MessageBox("[ItemEvent] - Vehicle Type can not be Blank", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If



                    'Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    'Dim rowcount As Integer = m1.VisualRowCount
                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    'If rowcount > 0 Then

                    '    For iRow As Integer = 1 To rowcount
                    '        If (m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value) = "Calibration Renewal" Then
                    '            Dim SqlQuery As String = "Select ""U_CDate"",""U_CEDate""  from ""@SAP_OCALP"" where ""U_VC""='" & txtTruckNo.Value & "'"

                    '            oRset.DoQuery(SqlQuery)
                    '            If (m1.Columns.Item("Col_1").Cells.Item(iRow).Specific.Value) <> oRset.Fields.Item("U_CDate").Value.ToString() Then
                    '                __oApplication.MessageBox("[ItemEvent] - Calibration for this truck date not match.Please modify the record before proceed", 1, "Ok", "", "")
                    '                BubbleEvent = False
                    '                Exit Sub
                    '            End If
                    '            If (m1.Columns.Item("Col_2").Cells.Item(iRow).Specific.Value) <> oRset.Fields.Item("U_CEDate").Value.ToString() Then
                    '                __oApplication.MessageBox("[ItemEvent] - Calibration expiration date not match.Please modify the record before proceed", 1, "Ok", "", "")
                    '                BubbleEvent = False
                    '                Exit Sub
                    '            End If

                    '        End If
                    '    Next
                    'End If

                    chkEXT.Item.AffectsFormMode = False
                    If chkEXT.Checked Then
                        ' oForm.Mode = BoFormMode.fm_OK_MODE
                        Exit Sub
                    End If

                    docentry = oForm.Items.Item("Item_4").Specific.value
                    ithReturnValue = __oApplication.MessageBox("You are going to modify/create ......... Do You want to  Modify this record ? ", 1, "Yes", "No")
                    Dim REVNO As Integer
                    If ithReturnValue = 1 Then
                        Dim SqlQuery As String = "Select count(*)  from ""@SAP_OCALP"" where ""DocEntry""=" & docentry

                        oRset.DoQuery(SqlQuery)

                        Dim count As Integer = 0
                        count = oRset.RecordCount
                        If count > 0 Then
                            If oForm.Items.Item("Item_33").Specific.value = "CP0000001" Or oForm.Items.Item("Item_33").Specific.value = "" Then
                                dbsrc.SetValue("U_PREVNO", dbsrc.Offset, trsting & trnumber)
                                dbsrc.SetValue("U_NREVNO", dbsrc.Offset, trsting & trnumber)
                            Else
                                REVNO = Right((oForm.Items.Item("Item_33").Specific.value), 8)
                                REVNO = REVNO + 1
                                dbsrc.SetValue("U_PREVNO", dbsrc.Offset, oForm.Items.Item("Item_33").Specific.value)
                                dbsrc.SetValue("U_NREVNO", dbsrc.Offset, trsting & REVNO.ToString("D8"))
                            End If
                        Else
                            dbsrc.SetValue("U_PREVNO", dbsrc.Offset, (oForm.Items.Item("Item_33").Specific.value))
                            dbsrc.SetValue("U_NREVNO", dbsrc.Offset, (oForm.Items.Item("Item_33").Specific.value))
                        End If


                    Else
                        BubbleEvent = False
                        Exit Sub

                    End If
                End If
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                ' If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then Exit Sub : BubbleEvent = False



                If pVal.MenuUID = CType(menuID.Add, String) And pVal.BeforeAction = False Then

                    DefulatSetting(oForm.UniqueID, BubbleEvent)
                ElseIf pVal.MenuUID = CType(menuID.Delete_Row, String) And pVal.BeforeAction = True Then


                ElseIf pVal.MenuUID = CType(menuID.Add_Row, String) And pVal.BeforeAction = False Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                    If m1.RowCount <= 0 Then
                        AddRowInMatrix(oForm, "@SAP_CALP1", "m1")

                    Else
                        If m1.Columns.Item("Col_0").Cells.Item(m1.RowCount).Specific.Value <> "" Then
                            AddRowInMatrix(oForm, "@SAP_CALP1", "m1")
                        End If
                    End If





                    Dim m3 As SAPbouiCOM.Matrix = oForm.Items.Item("m3").Specific

                    If m3.RowCount <= 0 Then
                        AddRowInMatrix(oForm, "@SAP_CALP3", "m3")

                    Else
                        If m3.Columns.Item("Chamber").Cells.Item(m3.RowCount).Specific.Value <> "" Then
                            AddRowInMatrix(oForm, "@SAP_CALP3", "m3")
                        End If
                    End If




                End If
            Catch ex As Exception

            End Try
        End Sub


        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                Dim ToDate As Date = Nothing
                Dim sc As String = __oApplication.Company.ServerDate
                ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_6").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_3").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OCALP")
                oForm.Items.Item("Item_4").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OCALP")

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_CALP1", "m1")

                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_28").Specific
                AddRowInMatrix(oForm, "@SAP_CALP2", "Item_28")

                Dim m3 As SAPbouiCOM.Matrix = oForm.Items.Item("m3").Specific
                AddRowInMatrix(oForm, "@SAP_CALP3", "m3")


                oForm.PaneLevel = "1"
            Catch ex As Exception

            End Try
        End Sub


        Private Sub Fill_Matrix(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                If m1.RowCount > 0 Then

                    If __oApplication.MessageBox("Are you sure you want To Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                        BubbleEvent = False
                        Exit Sub


                    End If
                End If

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_CALP1")
                m1.Clear()
                dbsrc.Clear()
                For iRow As Integer = 1 To CInt(oForm.Items.Item("Item_20").Specific.Value)
                    'AddRowInMatrix(oForm, "@SAP_CALP1", "m1")

                    dbsrc.Offset = dbsrc.Size - 1
                    m1.AddRow(1, m1.VisualRowCount)
                    m1.GetLineData(m1.VisualRowCount)
                    dbsrc.SetValue("U_CHN", dbsrc.Offset, iRow & "")
                    m1.SetLineData(m1.VisualRowCount)
                    m1.FlushToDataSource()



                Next

            Catch ex As Exception

            End Try
        End Sub


        Private Sub FixedAssetCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OCALP")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_FAC", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_Name", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")

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

        Private Sub Truck_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OCALP")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_VC", dbsrc.Offset, oDataTable.GetValue("U_TRegNo", 0) & "")
                                dbsrc.SetValue("U_Name", dbsrc.Offset, oDataTable.GetValue("U_Trailor", 0) & "")

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

        Private Sub FixedAssetCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim SqlQuery As String = ""


                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_Category"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "Main Tank"
                oCondition.BracketCloseNum = 1

                'oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_CR"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = "Y"
                'oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Pane_1_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "1"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Pane_2_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "2"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub AddRow_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_28").Specific


                If m2.Columns.Item("Chamber").Cells.Item(pVal.Row).Specific.Value <> "" Then
                    AddRowInMatrix(oForm, "@SAP_CALP2", "Item_28")
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Oil_AddRow_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim m3 As SAPbouiCOM.Matrix = oForm.Items.Item("m3").Specific


                If m3.Columns.Item("Chamber").Cells.Item(pVal.Row).Specific.Value <> "" Then
                    AddRowInMatrix(oForm, "@SAP_CALP3", "m3")
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Pane_3_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "3"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

    End Class
End Namespace

