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
    Public Class ASTMCalculation : Implements ISAP_HANA

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
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\ASTM_Calculation.srf"
                Dim sFormName As String = "SAP_ASTM_CALC"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_ASTM_CALC", __oApplication.Forms.ActiveForm.TypeCount)

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
                    If pVal.ItemUID = "BtnCal1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then

                    End If


                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "BtnCal1" Or pVal.ItemUID = "BtnCal2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then

                        BtnClick(FormUID, pVal, BubbleEvent)

                    End If
                End If
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

                If pVal.MenuUID = CType(menuID.Add, String) And pVal.BeforeAction = False Then

                    DefulatSetting(oForm.UniqueID, BubbleEvent)
                ElseIf pVal.MenuUID = CType(menuID.Delete_Row, String) And pVal.BeforeAction = True Then


                ElseIf pVal.MenuUID = CType(menuID.Add_Row, String) And pVal.BeforeAction = False Then
                    '    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    '    AddRowInMatrix(oForm, "@SAP_ASTM2", "m1")
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
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_7").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_5").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OASTM")
                oForm.Items.Item("Item_6").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OASTM")

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_ASTM1", "m1")

            Catch ex As Exception

            End Try
        End Sub

        Public Sub BtnClick(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                If pVal.ItemUID = "BtnCal1" Then

                    Dim oForm As SAPbouiCOM.Form = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                    Dim editText1 As SAPbouiCOM.EditText = oForm.Items.Item("Item_4").Specific
                    Dim editText2 As SAPbouiCOM.EditText = oForm.Items.Item("Item_3").Specific

                    SetFocus(oForm, "Item_3")

                    If String.IsNullOrEmpty(editText1.Value) OrElse String.IsNullOrEmpty(editText2.Value) Then
                        BubbleEvent = True
                        __Application.StatusBar.SetSystemMessage("Please Enter The Numeric value", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    Else
                        Dim value1 As Double
                        Dim value2 As Double

                        If Double.TryParse(editText1.Value, value1) AndAlso Double.TryParse(editText2.Value, value2) Then
                            Dim query As String = "SELECT * FROM ""@SAP_ASTM1"" WHERE ""U_Temp"" = '" & editText1.Value & "' AND ""U_ObValue"" = '" & editText2.Value & "'"
                            Dim recordSet As SAPbobsCOM.Recordset = __oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                            recordSet.DoQuery(query)

                            If Not recordSet.EoF Then
                                Dim resultValue As String = recordSet.Fields.Item("U_Density").Value.ToString()
                                Dim resultTextBox As SAPbouiCOM.EditText = oForm.Items.Item("TxtResult").Specific
                                resultTextBox.Value = resultValue
                            Else
                                __Application.StatusBar.SetSystemMessage("Value Doesnot Exits", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            End If
                        Else
                            __Application.StatusBar.SetSystemMessage("Value Is Not Valid Please Input Numeric Value Only", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                End If

                If pVal.ItemUID = "BtnCal2" Then

                    Dim oForm As SAPbouiCOM.Form = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                    Dim editText3 As SAPbouiCOM.EditText = oForm.Items.Item("Item_5").Specific
                    Dim editText4 As SAPbouiCOM.EditText = oForm.Items.Item("Item_6").Specific

                    SetFocus(oForm, "Item_5")


                    If String.IsNullOrEmpty(editText3.Value) OrElse String.IsNullOrEmpty(editText4.Value) Then
                        BubbleEvent = True
                        __Application.StatusBar.SetSystemMessage("Please Enter The Numeric Value", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    Else
                        Dim value3 As Double
                        Dim value4 As Double

                        If Double.TryParse(editText3.Value, value3) AndAlso Double.TryParse(editText4.Value, value4) Then
                            Dim query As String = "SELECT * FROM ""@SAP_ASTM1"" WHERE ""U_Density"" = '" & editText3.Value & "' AND ""U_Temp"" = '" & editText4.Value & "'"
                            Dim recordSet As SAPbobsCOM.Recordset = __oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                            recordSet.DoQuery(query)

                            If Not recordSet.EoF Then
                                Dim resultValue As String = recordSet.Fields.Item("U_ObValue").Value.ToString()
                                Dim resultTextBox As SAPbouiCOM.EditText = oForm.Items.Item("TxtResult1").Specific
                                resultTextBox.Value = resultValue
                            Else
                                __Application.StatusBar.SetSystemMessage("Value Doesnot Exits", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                            End If
                        Else
                            __Application.StatusBar.SetSystemMessage("Value Is Not Valid Please Input Numeric Value Only", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                End If
            Catch ex As Exception
                __oApplication.MessageBox("[Item_Pressed] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Public Sub SetFocus(ByVal oForm As SAPbouiCOM.Form, ByVal textBoxID As String)
            Try
                oForm.Select()
                oForm.Items.Item(textBoxID).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            Catch ex As Exception
                Console.WriteLine("Exception: " & ex.Message)
            End Try
        End Sub
    End Class
End Namespace

