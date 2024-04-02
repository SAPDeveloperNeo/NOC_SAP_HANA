
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

    Public Class C_ARCreditNote : Implements ISAP_HANA

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
            Throw New NotImplementedException()
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


        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                '    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then

                '        oMatrix = oForm.Items.Item("38").Specific

                '        Dim TaxCode As String = GetValue("Select  TOP 1 ""U_Tax"" As ""Tax"" From ""@SAP_OVH"" Where  IFNULL(""U_Tax"" ,'')<>''", "Tax")
                '        Dim Flag As Boolean = False
                '        For iRow As Integer = 1 To oMatrix.VisualRowCount
                '            If TaxCode = oMatrix.Columns.Item("160").Cells.Item(1).Specific.Value Then
                '                Flag = True
                '                Exit For
                '            End If
                '        Next


                '        If Flag = True Then
                '            Dim Per As String = GetValue("Select  TOP 1 ""U_Per"" As ""Per"" From ""@SAP_OVH"" Where  IFNULL(""U_Tax"" ,'')<>''", "Per")
                '            If __oApplication.MessageBox("Do you want to deduct " + Per.ToString + "% from VAT Amount", 1, "Yes", "No", "") = 2 Then
                '                'BubbleEvent = False

                '                'Exit Sub
                '            Else
                '                APIDOCNUM = ""
                '                APICARDCODE = ""
                '                APIDate = ""
                APIDOCNUM = oForm.Items.Item("8").Specific.Value.ToString
                APICARDCODE = oForm.Items.Item("4").Specific.Value.ToString
                APIDate = oForm.Items.Item("10").Specific.Value.ToString
                '            End If
                '        End If








                '    End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_OK_MODE Then


                    Dim DocEntry As String
                    If APIDOCNUM <> "" And APICARDCODE <> "" Then
                        DocEntry = GetValue(" Select TOP 1  T0.""DocEntry"" FROM ""ORIN""  T0 WHERE T0.""DocNum"" ='" + APIDOCNUM + "' And T0.""CardCode"" ='" + APICARDCODE + "' ORDER BY ""DocEntry"" DESC ", "DocEntry")
                        If DocEntry <> "" Then
                            VatJounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                            'Temp Commented by Nitesh suggested by Rajan sir
                            'VatJounralEntry1(FormUID, pVal, BubbleEvent, DocEntry)
                        End If

                    End If




                    APIDOCNUM = ""
                    APICARDCODE = ""
                    APIDate = ""


                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                APIDOCNUM = ""
                APICARDCODE = ""
            End Try
        End Sub





        Private Sub VatJounralEntry1(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Sql = "CALL SAP_GETDATA_ARCREDITNOTE_POLLUTION_JE ('" + DocEntry + "')"
                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)
                If oRecordset.RecordCount > 0 Then


                    Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                    oJounalEntry = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJounalEntry.ReferenceDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.DueDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "PLS"

                    Dim amount As Double = 0.0



                    oJounalEntry.Memo = "Pollution Based On AR Credit No  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "

                    For i = 0 To oRecordset.RecordCount - 1

                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.ShortName = oRecordset.Fields.Item("DebitAccount").Value
                        amount = amount + Convert.ToDouble(oRecordset.Fields.Item("Amt").Value)
                        oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.Credit = 0
                        oJounalEntry.Lines.DueDate = Now 'oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = Now ' oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                        oJounalEntry.Lines.CostingCode2 = "DOSUPPD"
                        oJounalEntry.Lines.CostingCode3 = "NA1"
                        oJounalEntry.Lines.CostingCode4 = "NA"




                        'oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        'oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        'oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        'oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        'oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oJounalEntry.Lines.Add()
                        oRecordset.MoveNext()

                    Next
                    oRecordset.MoveFirst()
                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                    oJounalEntry.Lines.Debit = 0
                    oJounalEntry.Lines.Credit = amount 'oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.DueDate = Now ' oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                    oJounalEntry.Lines.CostingCode2 = "DOSUPPD"
                    oJounalEntry.Lines.CostingCode3 = "NA1"
                    oJounalEntry.Lines.CostingCode4 = "NA"
                    'oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    'oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    'oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    'oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    'oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                    oJounalEntry.Lines.Add()




                    Dim Result As Integer = oJounalEntry.Add()
                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error:PLS JE  Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""ORIN"" Set ""U_POLTRANSIDS""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='PLS'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)

                        __oApplication.StatusBar.SetText("POLLUTION JE  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    End If




                End If





            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub VatJounralEntry(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Sql = "CALL SAP_GETDATA_ARCREDITNOTE_PSF_JE ('" + DocEntry + "')"
                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)
                If oRecordset.RecordCount > 0 Then


                    Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                    oJounalEntry = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJounalEntry.ReferenceDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.DueDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "PSF"

                    Dim amount As Double = 0.0



                    oJounalEntry.Memo = "PSF Based On AR Credit No  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "

                    For i = 0 To oRecordset.RecordCount - 1

                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.ShortName = oRecordset.Fields.Item("DebitAccount").Value
                        amount = amount + Convert.ToDouble(oRecordset.Fields.Item("Amt").Value)
                        oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.Credit = 0
                        oJounalEntry.Lines.DueDate = Now 'oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = Now ' oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                        oJounalEntry.Lines.CostingCode2 = "DOSUPPD"
                        oJounalEntry.Lines.CostingCode3 = "NA1"
                        oJounalEntry.Lines.CostingCode4 = "NA"




                        'oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        'oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        'oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        'oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        'oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oJounalEntry.Lines.Add()
                        oRecordset.MoveNext()

                    Next
                    oRecordset.MoveFirst()
                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                    oJounalEntry.Lines.Debit = 0
                    oJounalEntry.Lines.Credit = amount 'oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.DueDate = Now ' oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = Now 'oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                    oJounalEntry.Lines.CostingCode2 = "DOSUPPD"
                    oJounalEntry.Lines.CostingCode3 = "NA1"
                    oJounalEntry.Lines.CostingCode4 = "NA"
                    'oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    'oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    'oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    'oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    'oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                    oJounalEntry.Lines.Add()




                    Dim Result As Integer = oJounalEntry.Add()
                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error:PSF JE  Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""ORIN"" Set ""U_PSFTRANSID""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='PSF'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)

                        __oApplication.StatusBar.SetText("PSF JE  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    End If




                End If





            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
    End Class
End Namespace

