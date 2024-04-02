


Imports System.Globalization
Imports System.Text
Imports SAPbouiCOM

Namespace SAP_HANA
    Public Class AccountBudget : Implements ISAP_HANA



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
        Public Property ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property


        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\AccountBudget.srf"
                Dim sFormName As String = "SAP_UDO_OABU"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OABU", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_8", "1")
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

                    If pVal.ItemUID = "m1" And pVal.ColUID = "Col_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_4" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_Account_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_Branch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        AccountBudgetUpload_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then



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
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_7").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_6").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OABU")
                oForm.Items.Item("Item_8").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OABU")




                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_ABU1", "m1")

            Catch ex As Exception

            End Try
        End Sub

        Private Sub Matrix1_Branch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ABU1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_BPLId", dbsrc.Offset, oDataTable.GetValue("BPLId", 0) & "")
                                dbsrc.SetValue("U_BPLName", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")


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

        Private Sub Matrix1_OCRC_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ABU1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_CostCode", dbsrc.Offset, oDataTable.GetValue("PrcCode", 0) & "")

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

        Private Sub Matrix1_Account_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ABU1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_ActCode", dbsrc.Offset, oDataTable.GetValue("AcctCode", 0) & "")

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

        Private Sub AccountBudgetUpload_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                    'Dim BGT As SAPbobsCOM.Budget ' The budget object we will update
                    'Dim ACC As SAPbobsCOM.ChartOfAccounts
                    'Dim bFreeBudget As Boolean
                    'Dim r As SAPbobsCOM.Recordset
                    'r = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    'ACC = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oChartOfAccounts)

                    'BGT = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBudget)

                    'r.DoQuery("SELECT * FROM OBGT")
                    'BGT.Browser.Recordset = r
                    'BGT.Browser.MoveFirst()

                    'While BGT.Numerator <> (1)
                    '    BGT.Browser.MoveNext()
                    'End While

                    'Dim numSum As Decimal = BGT.TotalAnnualBudgetDebitSys + BGT.TotalAnnualBudgetCreditSys


                    'Dim i As Int16
                    'For i = 0 To 11
                    '    Dim tmp As Double
                    '    BGT.Lines.SetCurrentLine(i)
                    '    BGT.Lines.BudgetTotDebit = CDbl(100)
                    'Next
                    'Dim s As String

                    '__bobCompany.GetLastError(i, s)

                    'i = BGT.Update()




                    Dim Sql As String = "CALL SAP_GETDATA_Account_Budget_Upload ('" + oForm.Items.Item("Item_8").Specific.Value + "')"
                    Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs.DoQuery(Sql)
                    If oRs.RecordCount > 0 Then


                        While oRs.EoF = False
                            Dim oBudget As SAPbobsCOM.Budget = Nothing
                            oBudget = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBudget)

                            oBudget.AccountCode = oRs.Fields.Item("U_ActCode").Value
                            oBudget.DivisionCode = -1
                            oBudget.BudgetScenario = CInt(GetValue("SELECT IFNULL(T0.""AbsId"",0) AS ""AbsId"" FROM OBGS T0 WHERE Year(T0.""FinancYear"")='" + oRs.Fields.Item("U_Year").Value.ToString + "'", "AbsId"))

                            oBudget.ParentAccPercent = 0
                            'oBudget.TotalAnnualBudgetDebitLoc = oRs.Fields.Item("U_Year").Value
                            'oBudget.TotalAnnualBudgetCreditLoc = 0
                            'oBudget.TotalAnnualBudgetDebitSys = oRs.Fields.Item("U_Year").Value
                            'oBudget.TotalAnnualBudgetCreditSys = 0




                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.TotalAnnualBudgetDebitLoc = oRs.Fields.Item("U_Debit").Value
                                oBudget.TotalAnnualBudgetCreditLoc = 0
                                oBudget.TotalAnnualBudgetDebitSys = oRs.Fields.Item("U_Debit").Value
                                oBudget.TotalAnnualBudgetCreditSys = 0
                            Else
                                oBudget.TotalAnnualBudgetDebitLoc = 0
                                oBudget.TotalAnnualBudgetCreditLoc = oRs.Fields.Item("U_Credit").Value
                                oBudget.TotalAnnualBudgetDebitSys = 0
                                oBudget.TotalAnnualBudgetCreditSys = oRs.Fields.Item("U_Credit").Value
                            End If


                            oBudget.CostAccountingLines.Dimension = CInt(GetValue("SELECT ""DimCode"" From OPRC Where ""PrcCode""='" + oRs.Fields.Item("U_CostCode").Value.ToString + "'", "DimCode"))
                            oBudget.CostAccountingLines.DistrRuleCode = oRs.Fields.Item("U_CostCode").Value
                            oBudget.CostAccountingLines.DistrRuleDebitLC = oRs.Fields.Item("U_Debit").Value
                            oBudget.CostAccountingLines.DistrRuleCreditSC = oRs.Fields.Item("U_Credit").Value



                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(0)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Jul").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(0)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Jul").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(1)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Agust").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(1)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Agust").Value)
                                oBudget.Lines.Add()
                            End If
                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(2)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Sep").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(2)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Sep").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(3)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Oct").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(3)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Oct").Value)
                                oBudget.Lines.Add()
                            End If
                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(4)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Nov").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(4)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Nov").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(5)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Dec").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(5)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Dec").Value)
                                oBudget.Lines.Add()
                            End If


                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(6)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Jan").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(6)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Jan").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(7)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Feb").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(7)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Feb").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(8)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Mar").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(8)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Mar").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(9)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_April").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(9)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_April").Value)
                                oBudget.Lines.Add()
                            End If

                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(10)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_May").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(10)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_May").Value)
                                oBudget.Lines.Add()
                            End If
                            If oRs.Fields.Item("U_Debit").Value <> 0 Then
                                oBudget.Lines.SetCurrentLine(11)
                                oBudget.Lines.BudgetTotDebit = CDbl(oRs.Fields.Item("U_Jun").Value)
                                oBudget.Lines.Add()
                            Else
                                oBudget.Lines.SetCurrentLine(11)
                                oBudget.Lines.BudgetTotCredit = CDbl(oRs.Fields.Item("U_Jun").Value)
                                oBudget.Lines.Add()
                            End If




                            Dim result As Integer = oBudget.Add()
                            If result <> 0 Then
                                __oApplication.StatusBar.SetText("Error: " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                Dim SqlQuery As New StringBuilder

                                SqlQuery.Append("Update ""@SAP_ABU1"" Set ""U_Error""='" + bobCompany.GetLastErrorDescription + "'")

                                SqlQuery.Append("Where ""DocEntry""='" + oForm.Items.Item("Item_8").Specific.value.ToString + "'  And ""U_ActCode""='" + oRs.Fields.Item("U_ActCode").Value.ToString + "'")

                                Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRset1.DoQuery(SqlQuery.ToString)

                            Else

                                Dim SqlQuery As New StringBuilder

                                SqlQuery.Append("Update ""@SAP_ABU1"" Set ""U_Upload""='Y'")

                                SqlQuery.Append("Where ""DocEntry""='" + oForm.Items.Item("Item_8").Specific.value.ToString + "'  And ""U_ActCode""='" + oRs.Fields.Item("U_ActCode").Value.ToString + "'")

                                Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRset1.DoQuery(SqlQuery.ToString)


                            End If




                            oRs.MoveNext()
                        End While

                    End If
                End If

                __oApplication.MessageBox("Budget Added Successfully.   ", 1, "Ok", "", "")



            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
    End Class
End Namespace

