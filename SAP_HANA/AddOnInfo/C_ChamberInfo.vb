
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
    Public Class C_ChamberInfo : Implements ISAP_HANA

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


        Public Sub Form_Creation(ByVal CardCode As String, ByVal TCode As String, ByVal TName As String, ByVal TruckNum As String, ByVal DDate As String, ByVal DocNum As String, ByVal ItemCode As String, ByVal ItemName As String, ByVal Qty As Decimal, ByVal BaseType As String)
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\Chamber.srf"
                Dim sFormName As String = "SAP_UDO_ODLN"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_ODLN", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    DeativateMenu(oForm)
                    oForm.Freeze(True)


                    Try
                        oForm.Title = "Chamber Info"
                        oForm.Mode = BoFormMode.fm_ADD_MODE


                        Dim ToDate As Date = Nothing
                        Dim sc As String = __oApplication.Company.ServerDate
                        ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                        Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_19").Specific
                        PODate.String = ToDate.ToString("yyyyMMdd")


                        oForm.Items.Item("Item_17").Specific.value = GenrateDocEntry("DocEntry", "@SAP_ODLN")
                        oForm.Items.Item("Item_18").Specific.value = GenrateDocEntry("DocEntry", "@SAP_ODLN")

                        Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                        ' AddRowInMatrix(oForm, "@SAP_DLN1", "m1")


                        oForm.Items.Item("Item_22").Specific.Value = CardCode
                        oForm.Items.Item("Item_10").Specific.Value = TCode
                        oForm.Items.Item("Item_12").Specific.Value = TName
                        oForm.Items.Item("Item_9").Specific.Value = TruckNum
                        oForm.Items.Item("Item_24").Specific.Value = DocNum
                        oForm.Items.Item("Item_19").Specific.Value = DDate
                        oForm.Items.Item("Item_3").Specific.Value = BaseType

                        Dim dbsrcRow As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_DLN1")
                        Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ODLN")

                        Dim Count As Int16

                        Dim CountQ As String = ""
                        CountQ = "CALL SAP_GET_CalibrationCount ('" + TruckNum + "')"


                        Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs1Count.DoQuery(CountQ)
                        If oRs1Count.RecordCount > 0 Then
                            ' Count = oRs1Count.Fields.Item("Count").Value


                            For ADDRow As Integer = 1 To oRs1Count.RecordCount
                                dbsrcRow.Offset = dbsrc.Size - 1
                                m1.AddRow(1, m1.VisualRowCount)
                                m1.GetLineData(m1.VisualRowCount)

                                dbsrcRow.SetValue("U_CHN", dbsrc.Offset, ADDRow & "")
                                dbsrcRow.SetValue("U_ItemCode", dbsrc.Offset, ItemCode & "")
                                dbsrcRow.SetValue("U_ItemName", dbsrc.Offset, ItemName & "")
                                dbsrcRow.SetValue("U_Qty", dbsrc.Offset, oRs1Count.Fields.Item("QtyValu").Value & "")
                                dbsrcRow.SetValue("U_Dip", dbsrc.Offset, oRs1Count.Fields.Item("U_OILDIP").Value & "")

                                m1.SetLineData(m1.VisualRowCount)
                                m1.FlushToDataSource()
                                oRs1Count.MoveNext()
                            Next

                        Else

                            dbsrcRow.Offset = dbsrc.Size - 1
                            m1.AddRow(1, m1.VisualRowCount)
                            m1.GetLineData(m1.VisualRowCount)

                            dbsrcRow.SetValue("U_CHN", dbsrc.Offset, 1 & "")
                            dbsrcRow.SetValue("U_ItemCode", dbsrc.Offset, ItemCode & "")
                            dbsrcRow.SetValue("U_ItemName", dbsrc.Offset, ItemName & "")
                            dbsrcRow.SetValue("U_Qty", dbsrc.Offset, 0 & "")
                            dbsrcRow.SetValue("U_Dip", dbsrc.Offset, 0 & "")

                            m1.SetLineData(m1.VisualRowCount)
                            m1.FlushToDataSource()

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
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\Chamber.srf"
                Dim sFormName As String = "SAP_UDO_ODLN"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_ODLN", __oApplication.Forms.ActiveForm.TypeCount)
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
                        ' QtyCalculation(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_5" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TempLostFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_15" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        DensityLostFocus(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub


        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                Dim ToDate As Date = Nothing
                Dim sc As String = __oApplication.Company.ServerDate
                ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_19").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_17").Specific.value = GenrateDocEntry("DocEntry", "@SAP_DLN1")
                oForm.Items.Item("Item_18").Specific.value = GenrateDocEntry("DocEntry", "@SAP_DLN1")

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_DLN1", "m1")





            Catch ex As Exception

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



        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                CHCARDCODE = ""
                CHTRNO = ""
                CHBASETYEP = ""
                CHDOCNUM = ""
                CHTCODE = ""



                CHTRNO = oForm.Items.Item("Item_9").Specific.Value
                CHTCODE = oForm.Items.Item("Item_10").Specific.Value
                CHCARDCODE = oForm.Items.Item("Item_22").Specific.Value
                CHDOCNUM = oForm.Items.Item("Item_24").Specific.Value
                CHBASETYEP = oForm.Items.Item("Item_3").Specific.Value

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                For i = 1 To m1.RowCount
                    If m1.Columns.Item("Col_1").Cells.Item(i).Specific.value = "TR100001" Then
                        If String.IsNullOrEmpty(oForm.Items.Item("Item_13").Specific.Value) Then
                            __oApplication.SetStatusBarMessage("Final Boiling Point should not be blank for the Item Code : TR100001", SAPbouiCOM.BoMessageTime.bmt_Short, True)
                            oForm.Items.Item("Item_13").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            BubbleEvent = False
                        ElseIf Not String.IsNullOrEmpty(oForm.Items.Item("Item_13").Specific.Value) Then
                            If Convert.ToDouble(oForm.Items.Item("Item_13").Specific.Value) <= 0 Then
                                __oApplication.SetStatusBarMessage("Final Boiling Point should not be less or eqlual zero for the Item Code : TR100001", SAPbouiCOM.BoMessageTime.bmt_Short, True)
                                oForm.Items.Item("Item_13").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                BubbleEvent = False
                            End If
                        End If

                    End If
                Next



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

                    SqlQuery2.Append("SELECT T0.""DocEntry"", T0.""DocNum"" FROM ""@SAP_ODLN""  T0 ")
                    SqlQuery2.Append("WHERE T0.""U_DocNum""='" + CHDOCNUM + "' And  T0.""U_CardCode"" ='" + CHCARDCODE + "' And T0.""U_TruckNum"" ='" + CHTRNO + "'  ")
                    SqlQuery2.Append("And  T0.""U_TCode"" ='" + CHTCODE + "' And  T0.""U_BaseType"" ='" + CHBASETYEP + "' ")
                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRset.DoQuery(SqlQuery2.ToString)

                    If oRset.RecordCount > 0 Then
                        Dim oRefHashtable As Hashtable = New Hashtable
                        oRefHashtable.Clear()
                        oRefHashtable.Add("Code", oRset.Fields.Item("DocNum").Value)
                        oRefHashtable.Add("Name", oRset.Fields.Item("DocEntry").Value)
                        SendData(oRefHashtable, IsBaseForm)
                        oForm.Close()


                        CHCARDCODE = ""
                        CHTRNO = ""
                        CHBASETYEP = ""
                        CHDOCNUM = ""
                        CHTCODE = ""
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
                    m1.Columns.Item("Col_3").Cells.Item(pVal.Row).Specific.Value = (Value * CDec(m1.Columns.Item("Col_5").Cells.Item(pVal.Row).Specific.Value)) / 1000
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


    End Class
End Namespace

