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

    Public Class CalpRevHist : Implements ISAP_HANA

#Region "Variable Declaration"

        Private __oApplication As SAPbouiCOM.Application
        Public __oCompany As SAPbobsCOM.Company
        Private oForm As SAPbouiCOM.Form

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

                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\CalpRevHist.srf"
                Dim sFormName As String = "CalpRevHist"
                Dim FormUID1 As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("CalpRevHist", __oApplication.Forms.ActiveForm.TypeCount)
                oForm.Title = IIf(FormTitle <> "", FormTitle, "List of Calibration Revision History")
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
                    oForm.DataSources.DataTables.Add("Records2")
                    oForm.DataSources.DataTables.Add("Records3")
                    oForm.DataSources.DataTables.Add("Records4")

                    'oForm.DataSources.UserDataSources.Add("db5", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 2)


                    'Dim chkSelect As SAPbouiCOM.CheckBox = oForm.Items.Item("chkSelect").Specific
                    'chkSelect.DataBind.SetBound(True, "", "db5")

                    'oForm.Items.Item("chkSelect").Visible = False

                    '-----------Fill The Grid---------------------
                    Dim grd1 As SAPbouiCOM.Grid = oForm.Items.Item("grd1").Specific
                    Dim grd2 As SAPbouiCOM.Grid = oForm.Items.Item("grd2").Specific
                    Dim grd3 As SAPbouiCOM.Grid = oForm.Items.Item("grd3").Specific
                    Dim grd4 As SAPbouiCOM.Grid = oForm.Items.Item("grd4").Specific
                    Dim SqlQuery As String = ""

                    'SqlQuery = sQuery + "('" + __bobCompany.UserSignature.ToString() + "')"

                    SqlQuery = "Select ""DocEntry"",""DocNum"",U_FAC as ""StorageTank"",""U_Name"" as ""TrailorNo"" ,U_VC as ""TruckNo"",""U_PDate"" As ""PostingDate"" ,""U_CDate"" As ""CalibrationDate"",""U_CEDate"" ""ExpiryDate"",U_TS As ""TireSize"",""U_temp"" As ""Temprature"",U_TP As ""TirePresure"",U_TC As ""TotalChamber"",""U_Status"" ""Status"",U_PREVNO ""OldRevNo"",U_NREVNO As ""NewRevNo"",U_EMPCODE As ""EmpCode"" from ""@SAP_OCALPREV"" where ""DocEntry""='" & sQuery & "'"

                    oForm.DataSources.DataTables.Item("Records1").ExecuteQuery(SqlQuery)

                    grd1.DataTable = oForm.DataSources.DataTables.Item("Records1")

                    SqlQuery = "Select ""DocEntry"",U_CHN AS ""Chamber"",U_CAP as ""Capacity"",U_OVRDIP as ""OverDip"" ,U_OILDIP as ""Oil Dip"" ,U_REVNO as ""RevisionNo"" from ""@SAP_CALP1REV""  where ""DocEntry""='" & sQuery & "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records2").ExecuteQuery(SqlQuery)

                    grd2.DataTable = oForm.DataSources.DataTables.Item("Records2")

                    SqlQuery = "Select ""DocEntry"",U_CHN ""Chember"",""U_Hight"" as ""Height"",""U_Qty"" ""Quantity"" , U_REVNO as ""RevisionNo""  from ""@SAP_CALP2REV"" where ""DocEntry""='" & sQuery & "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records3").ExecuteQuery(SqlQuery)

                    grd3.DataTable = oForm.DataSources.DataTables.Item("Records3")

                    SqlQuery = "Select ""DocEntry"",U_CHN ""Chember"",""U_Hight"" as ""Height"",""U_Qty"" ""Quantity"" , U_REVNO as ""RevisionNo"" from ""@SAP_CALP3REV"" where ""DocEntry""='" & sQuery & "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records4").ExecuteQuery(SqlQuery)

                    grd4.DataTable = oForm.DataSources.DataTables.Item("Records4")


                    For iCol As Integer = 0 To grd1.Columns.Count - 1
                        grd1.Columns.Item(iCol).Editable = False
                    Next

                    For iCol As Integer = 0 To grd2.Columns.Count - 1
                        grd2.Columns.Item(iCol).Editable = False
                    Next

                    For iCol As Integer = 0 To grd3.Columns.Count - 1
                        grd3.Columns.Item(iCol).Editable = False
                    Next

                    For iCol As Integer = 0 To grd4.Columns.Count - 1
                        grd3.Columns.Item(iCol).Editable = False
                    Next

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
            Throw New NotImplementedException()
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
                Dim grd1 As SAPbouiCOM.Grid = oForm.Items.Item("grd1").Specific
                Dim grd2 As SAPbouiCOM.Grid = oForm.Items.Item("grd2").Specific
                Dim grd3 As SAPbouiCOM.Grid = oForm.Items.Item("grd3").Specific
                Dim grd4 As SAPbouiCOM.Grid = oForm.Items.Item("grd4").Specific
                Dim SqlQuery As String = ""

                'SqlQuery = sQuery + "('" + __bobCompany.UserSignature.ToString() + "')"


                SqlQuery = "Select ""DocEntry"",""DocNum"",U_FAC as ""StorageTank"",""U_Name"" as ""TrailorNo"" ,U_VC as ""TruckNo"",""U_PDate"" As ""PostingDate"" ,""U_CDate"" As ""CalibrationDate"",""U_CEDate"" ""ExpiryDate"",U_TS As ""TireSize"",""U_temp"" As ""Temprature"",U_TP As ""TirePresure"",U_TC As ""TotalChamber"",""U_Status"" ""Status"",U_PREVNO ""OldRevNo"",U_NREVNO As ""NewRevNo"",U_EMPCODE As ""EmpCode"" from ""@SAP_OCALPREV"" where ""U_VC""='" + oForm.Items.Item("Item_0").Specific.Value + "' and ""U_NREVNO""='" + oForm.Items.Item("Item_3").Specific.Value + "'"

                oForm.DataSources.DataTables.Item("Records1").ExecuteQuery(SqlQuery)

                grd1.DataTable = oForm.DataSources.DataTables.Item("Records1")

                Dim docentry1 As String
                If grd1.DataTable.Rows.Count > 0 Then
                    docentry1 = grd1.DataTable.GetValue(0, 0).ToString
                    SqlQuery = "Select ""DocEntry"",U_CHN AS ""Chamber"",U_CAP as ""Capacity"",U_OVRDIP as ""OverDip"" ,U_OILDIP as ""Oil Dip"" ,U_REVNO as ""RevisionNo"" from ""@SAP_CALP1REV""  where ""DocEntry""='" & docentry1 & "'  and ""U_REVNO""='" + oForm.Items.Item("Item_3").Specific.Value + "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records2").ExecuteQuery(SqlQuery)

                    grd2.DataTable = oForm.DataSources.DataTables.Item("Records2")
                    SqlQuery = "Select ""DocEntry"",U_CHN ""Chember"",""U_Hight"" as ""Height"",""U_Qty"" ""Quantity"" , U_REVNO as ""RevisionNo""  from ""@SAP_CALP2REV"" where ""DocEntry""='" & docentry1 & "'  and ""U_REVNO""='" + oForm.Items.Item("Item_3").Specific.Value + "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records3").ExecuteQuery(SqlQuery)

                    grd3.DataTable = oForm.DataSources.DataTables.Item("Records3")

                    SqlQuery = "Select ""DocEntry"",U_CHN ""Chember"",""U_Hight"" as ""Height"",""U_Qty"" ""Quantity"" , U_REVNO as ""RevisionNo"" from ""@SAP_CALP3REV"" where ""DocEntry""='" & docentry1 & "'  and ""U_REVNO""='" + oForm.Items.Item("Item_3").Specific.Value + "' and ifnull(""U_CHN"",0)<>0"

                    oForm.DataSources.DataTables.Item("Records4").ExecuteQuery(SqlQuery)

                    grd4.DataTable = oForm.DataSources.DataTables.Item("Records4")

                End If
                For iCol As Integer = 0 To grd1.Columns.Count - 1
                    grd1.Columns.Item(iCol).Editable = False
                Next

                For iCol As Integer = 0 To grd2.Columns.Count - 1
                    grd2.Columns.Item(iCol).Editable = False
                Next

                For iCol As Integer = 0 To grd3.Columns.Count - 1
                    grd3.Columns.Item(iCol).Editable = False
                Next

                For iCol As Integer = 0 To grd4.Columns.Count - 1
                    grd3.Columns.Item(iCol).Editable = False
                Next



                oForm.Freeze(False)
            Catch ex As Exception
                oForm.Freeze(False)
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub





    End Class

End Namespace

