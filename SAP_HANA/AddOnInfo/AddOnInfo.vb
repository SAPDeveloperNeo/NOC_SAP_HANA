Imports System.Data
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Reflection
Imports System.Globalization
Imports System.Xml
Imports System.IO
Imports System
Imports System.Net
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Namespace SAP_HANA

    Public Class AddOnInfo

#Region "Declare Veriabls"

        Public WithEvents oApplication As SAPbouiCOM.Application
        Private oFilters As SAPbouiCOM.EventFilters
        Private oFilter As SAPbouiCOM.EventFilter
        Public oCompany As SAPbobsCOM.Company
        Public _bobCompany As SAPbobsCOM.Company = Nothing
        Private menuFlag As Boolean = True
        Dim sForm As SAPbouiCOM.Form
        Dim ssForm As SAPbouiCOM.Form
        Dim oProgressBar As SAPbouiCOM.ProgressBar
        Public oItem As SAPbouiCOM.Item
        Dim SelectedRow As Integer
        Dim val As String
        Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal process As IntPtr, ByVal minimumWorkingSetSize As Integer, ByVal maximumWorkingSetSize As Integer) As Integer
        'Private WithEvents SetProcessWorkingSetSize( process as ,minimumWorkingSetSize as Integer,maximumWorkingSetSize as Integer) 
        'private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
#End Region

#Region " Declare Class here"
        Private xC_ISAP_HANA As ISAP_HANA
#End Region

#Region "Const Enumeration"
        Public Enum menuID
            Next_Record = 1288
            Previous_Record = 1289
            First_Record = 1290
            Last_Record = 1291
            Duplicate_Row = 1287
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

        Public Sub New()
            MyBase.New()
            Class_Initialize_Renamed()
        End Sub

#Region "Set Application"

        Private Sub SetApplication()
            Dim SboGuiApi As SAPbouiCOM.SboGuiApi
            Dim sConnectionString As String

            SboGuiApi = New SAPbouiCOM.SboGuiApi
            If Environment.GetCommandLineArgs.Length > 1 Then
                sConnectionString = Environment.GetCommandLineArgs.GetValue(1)
            Else
                sConnectionString = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056"
            End If

            'SboGuiApi = New SAPbouiCOM.SboGuiApi
            'sConnectionString = Environment.GetCommandLineArgs.GetValue(1)
            SboGuiApi.Connect(sConnectionString)
            oApplication = SboGuiApi.GetApplication(-1)
        End Sub

        Private Function SetConnectionContext() As Integer
            Dim sCookie As String
            Dim sConnectionContext As String

            oCompany = New SAPbobsCOM.Company
            sCookie = oCompany.GetContextCookie
            sConnectionContext = oApplication.Company.GetConnectionContext(sCookie)
            If oCompany.Connected = True Then
                oCompany.Disconnect()
            End If
            SetConnectionContext = oCompany.SetSboLoginContext(sConnectionContext)
        End Function

        Private Function ConnectToCompany() As Integer
            Try
                'oApplication.MessageBox("1..Set company Connectted to ...")
                ConnectToCompany = oCompany.Connect
                'oApplication.MessageBox("2..Set company Connectted to ..." + ConnectToCompany.ToString)
                _bobCompany = oApplication.Company.GetDICompany()
                '_bobCompany = oCompany ' CType(oApplication.Company.GetDICompany(), SAPbobsCOM.Company)
                'oApplication.MessageBox("3..Set company Connectted to ..." + _bobCompany.Connected.ToString)
                'oCompany = _bobCompany
                If _bobCompany.Connected = True Then
                    'oApplication.MessageBox("4..Set company Connectted to ..." + _bobCompany.Connected.ToString)
                    DBSerName = _bobCompany.Server
                    DBName = _bobCompany.CompanyDB
                    DBUserName = _bobCompany.DbUserName
                    DBPassword = _bobCompany.DbPassword
                    SAPApplication = oApplication
                    bobCompany = _bobCompany

                End If
            Catch ex As Exception
                oApplication.MessageBox(ex.Message)
            End Try

        End Function

        Private Sub Class_Initialize_Renamed()
            SetApplication()
            If Not SetConnectionContext() = 0 Then
                oApplication.MessageBox("SAP Customization AddOn-Failed setting a connection to DI API")
                Exit Sub
            End If
            oApplication.StatusBar.SetText("SAP Customization AddOn-Connectting to the company Data Base", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            If Not ConnectToCompany() = 0 Then
                oApplication.MessageBox("SAP Customization AddOn-Failed connecting to the company's Data Base")
                Exit Sub
            Else
                oApplication.StatusBar.SetText("SAP Customization AddOn-Connected to the company Data Base", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            End If

            '--------------Set Date Format -----------------------
            SetDateFormat()


            '------------ Filters Events Here ----------------------------
            oFilters = New SAPbouiCOM.EventFilters
            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_ALL_EVENTS)


            oFilter.AddEx("SAP_UDO_OITR")    '--Inventory Transfer Request
            oFilter.AddEx("SAP_UDO_OIT")    '--Inventory Transfer
            oFilter.AddEx("720")    '--Goods Issue
            oFilter.AddEx("TSSIPL_MulSelSOR")
            oFilter.AddEx("TruckRevHist")
            oFilter.AddEx("Acdnt")

            oFilter.AddEx("CalpRevHist")
            oFilter.AddEx("SAP_UDO_OOTM")
            oFilter.AddEx("SAP_UDO_ODM")
            oFilter.AddEx("SAP_UDO_OASTM")
            oFilter.AddEx("SAP_ASTM_CALC")
            oFilter.AddEx("SAP_UDO_OCSM")
            oFilter.AddEx("SAP_UDO_OAC")
            oFilter.AddEx("SAP_UDO_OCALP")
            oFilter.AddEx("SAP_UDO_OTMD")
            oFilter.AddEx("142") 'Purchase Order
            oFilter.AddEx("SAP_UDO_OAAM")
            ''''''''''''Budget--------------
            oFilter.AddEx("SAP_UDO_OABU")
            oFilter.AddEx("SAP_UDO_OPPBU")
            oFilter.AddEx("SAP_UDO_OLBS")
            oFilter.AddEx("SAP_UDO_OIBS")
            oFilter.AddEx("140") '--Delivery

            oFilter.AddEx("SAP_UDO_ODLN") '--Delivery
            oFilter.AddEx("IBudget") '--Budget

            oFilter.AddEx("SAP_UDO_OTRC")
            oFilter.AddEx("SAP_UDO_OTOC")
            oFilter.AddEx("SAP_UDO_OTTP")
            oFilter.AddEx("143")
            oFilter.AddEx("85")
            oFilter.AddEx("SAP_UDO_OPKL")
            oFilter.AddEx("141") 'AP InVoice'
            oFilter.AddEx("133") 'Ar InVoice'
            oFilter.AddEx("179") 'Ar InVoice'
            oFilter.AddEx("181") 'AP InVoice'
            oFilter.AddEx("81") 'Pick
            oFilter.AddEx("992") 'Pick

            oApplication.SetFilter(oFilters)

            '------------- Create UDO ----------
            ' CreateUDOTable()

            '------------- Create Menu ---------

            Dim oMenus As SAPbouiCOM.Menus
            Dim oMenuItem As SAPbouiCOM.MenuItem
            oMenus = oApplication.Menus
            'If Not oMenus.Exists("MNU_QC") Then
            AddRemoveMenus("HANA.xml")
            'End If

            'oMenuItem = oApplication.Menus.Item("SAP_BT")
            'oMenuItem.Image = getApplicationPath() & "\Images\Inv.bmp"

            ' ------------- GetDefaultForm MenuID ---------
            Dim i As Integer
            oMenus = __Application.Menus
            oMenus = __Application.Menus
            If __Application.Menus.Exists("47616") Then
                oMenuItem = __Application.Menus.Item("47616")
                oMenus = oMenuItem.SubMenus
                Try
                    For i = 0 To oMenus.Count - 1
                        If (oMenus.Item(i).String.StartsWith("SAP_UDO_OAC")) Then
                            oMenuOutStandingMaster = oMenus.Item(i).UID

                        ElseIf (oMenus.Item(i).String.StartsWith("SAP_UDO_ODM")) Then
                            oMenuDocumentMaster = oMenus.Item(i).UID

                        ElseIf (oMenus.Item(i).String.StartsWith("SAP_UDO_OAAM")) Then
                            oMenuAccountMApping = oMenus.Item(i).UID

                        ElseIf (oMenus.Item(i).String.StartsWith("SAP_UDO_OLBS")) Then
                            oMenuLedgerBasicSetup = oMenus.Item(i).UID

                        ElseIf (oMenus.Item(i).String.StartsWith("SAP_UDO_OIBS")) Then
                            oMenuItemBasicSetup = oMenus.Item(i).UID
                        End If
                    Next
                Catch ex As Exception
                    __Application.MessageBox(ex.Message)
                End Try
            End If


            GC.Collect()
            FlushMemory()
            AddReportLayOut()
        End Sub
#End Region

#Region "Add/Remove Menus"
        Public Sub AddRemoveMenus(ByVal sFileName As String)
            Dim oXMLDoc As New XmlDocument
            Dim sFilePath As String
            Try
                sFilePath = getApplicationPath() & "\" & sFileName
                oXMLDoc.Load(sFilePath)
                oApplication.LoadBatchActions(oXMLDoc.InnerXml)

                sFilePath = oApplication.GetLastBatchResults()

            Catch ex As Exception
                Throw ex
            Finally
                oXMLDoc = Nothing
            End Try
        End Sub
#End Region

        Private Sub oApplication_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles oApplication.MenuEvent
            Try
                BubbleEvent = True


                If pVal.BeforeAction = False Then
                    If pVal.MenuUID = "SAP_UDO" Then
                        CreateUDOTable()
                    ElseIf pVal.MenuUID = "SAP_AC" Then
                        oApplication.ActivateMenuItem(oMenuOutStandingMaster)

                    ElseIf pVal.MenuUID = "SAP_AM" Then
                        oApplication.ActivateMenuItem(oMenuAccountMApping)

                    ElseIf pVal.MenuUID = "SAP_BLBS" Then
                        oApplication.ActivateMenuItem(oMenuLedgerBasicSetup)

                    ElseIf pVal.MenuUID = "SAP_BIBS" Then
                        oApplication.ActivateMenuItem(oMenuItemBasicSetup)

                    ElseIf pVal.MenuUID = "SAP_BAB" Then
                        xC_ISAP_HANA = New AccountBudget(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OABU"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_BPPB" Then
                        xC_ISAP_HANA = New C_ProcurementPlanBudget(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OPPBU"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)





                    ElseIf pVal.MenuUID = "SAP_ITR" Then
                        xC_ISAP_HANA = New C_Inv_TransReq(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OITR"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)
                    ElseIf pVal.MenuUID = "SAP_IT" Then
                        xC_ISAP_HANA = New Inv_Transfer(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OIT"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_TM" Then
                        xC_ISAP_HANA = New C_TruckMaster(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OOTM"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)
                    ElseIf pVal.MenuUID = "SAP_AS" Then
                        xC_ISAP_HANA = New Acdnt(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "Acdnt"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_VDM" Then
                        oApplication.ActivateMenuItem(oMenuDocumentMaster)

                    ElseIf pVal.MenuUID = "SAP_ASTMPM" Then
                        xC_ISAP_HANA = New C_ASTM(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OASTM"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)




                    ElseIf pVal.MenuUID = "SAP_ASTMCLC" Then
                        xC_ISAP_HANA = New ASTMCalculation(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_ASTM_CALC"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)





                    ElseIf pVal.MenuUID = "SAP_CMM" Then
                        xC_ISAP_HANA = New C_Consumer(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OCSM"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_CALP" Then
                        xC_ISAP_HANA = New C_CalibrationProcess(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OCALP"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)


                    ElseIf pVal.MenuUID = "SAP_TDM" Then
                        xC_ISAP_HANA = New C_TempMaster(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTMD"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_CHINFO" Then
                        xC_ISAP_HANA = New C_ChamberInfo(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_ODLN"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_BIFE" Then
                        xC_ISAP_HANA = New C_ImportBudget(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "IBudget"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_TRCH" Then
                        xC_ISAP_HANA = New C_TruckRouteChnge(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTRC"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                    ElseIf pVal.MenuUID = "SAP_TOWCH" Then
                        xC_ISAP_HANA = New C_TruckOwnerChange(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTOC"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)


                    ElseIf pVal.MenuUID = "SAP_CHALLOCATION" Then
                        xC_ISAP_HANA = New C_ChamberAllocation(oApplication, oCompany)
                        xC_ISAP_HANA.ObjectCode = "SAP_UDO_OPKL"
                        xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)

                        'ElseIf pVal.MenuUID = "142" Then
                        '    xC_ISAP_HANA = New C_PurchaseOrder(oApplication, oCompany)
                        '    xC_ISAP_HANA.ObjectCode = "22"
                        '    xC_ISAP_HANA.Form_Creation_MenuEvent(pVal, BubbleEvent)
                    End If

                End If


                Dim FrmUID As String = oApplication.Forms.ActiveForm.UniqueID
                Dim FrmID As String = oApplication.Forms.ActiveForm.Type
                If FrmUID.StartsWith("SAP_UDO_OITR") Then
                    xC_ISAP_HANA = New C_Inv_TransReq(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                ElseIf FrmUID.StartsWith("SAP_UDO_OIT") Then
                    xC_ISAP_HANA = New Inv_Transfer(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                ElseIf FrmUID.StartsWith("SAP_UDO_OOTM") Then
                    xC_ISAP_HANA = New C_TruckMaster(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                ElseIf FrmUID.StartsWith("SAP_UDO_OASTM") Then
                    xC_ISAP_HANA = New C_ASTM(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)





                    'ElseIf FrmUID.StartsWith("SAP_ASTM_CALC") Then
                    '    xC_ISAP_HANA = New C_ASTM(oApplication, oCompany)
                    '    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)





                ElseIf FrmUID.StartsWith("SAP_UDO_OCALP") Then
                    xC_ISAP_HANA = New C_CalibrationProcess(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                ElseIf FrmUID.StartsWith("SAP_UDO_OTMD") Then
                    xC_ISAP_HANA = New C_TempMaster(oApplication, oCompany)
                    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                    'ElseIf FrmUID.StartsWith("22") Then
                    '    xC_ISAP_HANA = New C_PurchaseOrder(oApplication, oCompany)
                    '    xC_ISAP_HANA.Form_TMenuEvent(pVal, BubbleEvent)

                End If








                GC.Collect()
                FlushMemory()
            Catch ex As Exception
                ' oApplication.MessageBox("[MenuEvent] - " & ex.Message, 1, "Ok", "", "")
                GC.Collect()
                FlushMemory()
            End Try
        End Sub

        Private Sub oApplication_ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
            Try
                BubbleEvent = True

                If pVal.FormTypeEx = "SAP_UDO_OITR" Then
                    xC_ISAP_HANA = New C_Inv_TransReq(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OITR"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OIT" Then
                    xC_ISAP_HANA = New Inv_Transfer(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OIT"


                ElseIf pVal.FormTypeEx = "TSSIPL_MulSelSOR" Then
                    xC_ISAP_HANA = New c_G_ChooseFromList(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "TSSIPL_MulSelSOR"

                ElseIf pVal.FormTypeEx = "TruckRevHist" Then
                    xC_ISAP_HANA = New TruckRevHist(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "TruckRevHist"
                ElseIf pVal.FormTypeEx = "Acdnt" Then
                    xC_ISAP_HANA = New Acdnt(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "Acdnt"
                ElseIf pVal.FormTypeEx = "CalpRevHist" Then
                    xC_ISAP_HANA = New CalpRevHist(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "CalpRevHist"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OOTM" Then
                    xC_ISAP_HANA = New C_TruckMaster(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OOTM"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OASTM" Then
                    xC_ISAP_HANA = New C_ASTM(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OASTM"



                ElseIf pVal.FormTypeEx = "SAP_ASTM_CALC" Then
                    xC_ISAP_HANA = New ASTMCalculation(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_ASTM_CALC"



                ElseIf pVal.FormTypeEx = "SAP_UDO_OCSM" Then
                    xC_ISAP_HANA = New C_Consumer(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OCSM"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OAC" Then
                    xC_ISAP_HANA = New C_Applicable_Charges(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OAC"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OCALP" Then
                    xC_ISAP_HANA = New C_CalibrationProcess(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OCALP"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OTMD" Then
                    xC_ISAP_HANA = New C_TempMaster(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTMD"

                ElseIf pVal.FormTypeEx = "142" Then
                    xC_ISAP_HANA = New C_PurchaseOrder(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "142"

                ElseIf pVal.FormTypeEx = "140" Then
                    xC_ISAP_HANA = New C_Delivery(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "140"

                ElseIf pVal.FormTypeEx = "143" Then
                    xC_ISAP_HANA = New C_GRN(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "143"


                ElseIf pVal.FormTypeEx = "SAP_UDO_OLBS" Then
                    xC_ISAP_HANA = New C_LedgerBasicSetup(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OLBS"


                ElseIf pVal.FormTypeEx = "SAP_UDO_OIBS" Then
                    xC_ISAP_HANA = New C_ItemBasicSetUp(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OIBS"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OABU" Then
                    xC_ISAP_HANA = New AccountBudget(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OABU"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OPPBU" Then
                    xC_ISAP_HANA = New C_ProcurementPlanBudget(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OPPBU"

                ElseIf pVal.FormTypeEx = "SAP_UDO_ODLN" Then
                    xC_ISAP_HANA = New C_ChamberInfo(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_ODLN"

                ElseIf pVal.FormTypeEx = "IBudget" Then
                    xC_ISAP_HANA = New C_ImportBudget(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "IBudget"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OTRC" Then
                    xC_ISAP_HANA = New C_TruckRouteChnge(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTRC"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OTOC" Then
                    xC_ISAP_HANA = New C_TruckOwnerChange(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OTOC"

                ElseIf pVal.FormTypeEx = "85" Then
                    xC_ISAP_HANA = New C_PickList(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "85"

                ElseIf pVal.FormTypeEx = "SAP_UDO_OPKL" Then
                    xC_ISAP_HANA = New C_ChamberAllocation(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "SAP_UDO_OPKL"

                ElseIf pVal.FormTypeEx = "141" Then
                    xC_ISAP_HANA = New C_APInvocie(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "141"

                ElseIf pVal.FormTypeEx = "133" Then
                    xC_ISAP_HANA = New C_ARInvocie(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "133"
                ElseIf pVal.FormTypeEx = "179" Then
                    xC_ISAP_HANA = New C_ARCreditNote(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "179"


                ElseIf pVal.FormTypeEx = "181" Then
                    xC_ISAP_HANA = New C_APCreditNote(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "181"

                ElseIf pVal.FormTypeEx = "81" Then
                    xC_ISAP_HANA = New C_PickPackProductionManager(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "81"

                ElseIf pVal.FormTypeEx = "992" Then
                    xC_ISAP_HANA = New C_LandedCost(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Process_ItemEvents(FormUID, pVal, BubbleEvent)
                    xC_ISAP_HANA.ObjectCode = "992"


                End If

                GC.Collect()
                FlushMemory()
            Catch ex As Exception
                oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                GC.Collect()
                FlushMemory()
            End Try
        End Sub

        Private Sub oApplication_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles oApplication.FormDataEvent
            Try
                BubbleEvent = True
                Dim FrmUID As String = BusinessObjectInfo.FormTypeEx


                If FrmUID = "SAP_UDO_OIT" Then
                    xC_ISAP_HANA = New Inv_Transfer(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Load_DataEvent(BusinessObjectInfo, BubbleEvent)

                ElseIf FrmUID = "SAP_UDO_OTMD" Then
                    xC_ISAP_HANA = New C_TempMaster(oApplication, oCompany)
                    xC_ISAP_HANA.Form_Load_DataEvent(BusinessObjectInfo, BubbleEvent)


                End If
                GC.Collect()
                FlushMemory()
            Catch ex As Exception
                oApplication.MessageBox("[DataEvent] - " & ex.Message, 1, "Ok", "", "")
                GC.Collect()
                FlushMemory()
            End Try
        End Sub

        'Private Sub oApplication_ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
        '    If pVal.EventType = BoEventTypes.et_ITEM_PRESSED And pVal.BeforeAction = False Then
        '        ' Check the item ID to identify the button/item pressed
        '        If pVal.ItemUID = "YourButtonID" Then
        '            ' Perform actions when the specific button is pressed
        '            ' Here you can handle different form types based on your conditions
        '            If FormUID = "SAP_UDO_OIT" Then
        '                Dim xC_ISAP_HANA As ISAP_HANA = New Inv_Transfer(SBO_Application, oCompany)
        '                xC_ISAP_HANA.Form_Load_DataEvent(BusinessObjectInfo, BubbleEvent)
        '            ElseIf FormUID = "SAP_UDO_OTMD" Then
        '                Dim xC_ISAP_HANA As ISAP_HANA = New C_TempMaster(SBO_Application, oCompany)
        '                xC_ISAP_HANA.Form_Load_DataEvent(BusinessObjectInfo, BubbleEvent)
        '            End If
        '            GC.Collect()
        '            FlushMemory()
        '        End If
        '    End If
        'End Sub
        Private Sub oApplication_AppEvent(ByVal EventType As SAPbouiCOM.BoAppEventTypes) Handles oApplication.AppEvent
            Select Case EventType
                Case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged
                    Application.Exit()
                Case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition
                    Application.Exit()
                Case SAPbouiCOM.BoAppEventTypes.aet_ShutDown
                    Application.Exit()
            End Select
            GC.Collect()
            FlushMemory()
        End Sub



        'Private Sub oApplication_RightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean) Handles oApplication.RightClickEvent

        '    Dim FrmID As String = oApplication.Forms.ActiveForm.Type

        '    If FrmID = "139" Then
        '        If (eventInfo.BeforeAction = True) Then
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        '                oCreationPackage = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)

        '                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
        '                oCreationPackage.UniqueID = "GI"
        '                oCreationPackage.String = "Good Issue"
        '                oCreationPackage.Enabled = True

        '                oMenuItem = oApplication.Menus.Item("1280") 'Data'
        '                oMenus = oMenuItem.SubMenus
        '                oMenus.AddEx(oCreationPackage)

        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try
        '        Else
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                oApplication.Menus.RemoveEx("GI")
        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try

        '        End If
        '    Else
        '        Dim oMenuItem As SAPbouiCOM.MenuItem
        '        Dim oMenus As SAPbouiCOM.Menus


        '        Try
        '            oApplication.Menus.RemoveEx("GI")
        '        Catch ex As Exception
        '            '  MessageBox.Show(ex.Message)
        '        End Try
        '    End If





        '    If FrmID = "720" Then
        '        If (eventInfo.BeforeAction = True) Then
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        '                oCreationPackage = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)

        '                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
        '                oCreationPackage.UniqueID = "GR"
        '                oCreationPackage.String = "Good Receipt"
        '                oCreationPackage.Enabled = True

        '                oMenuItem = oApplication.Menus.Item("1280") 'Data'
        '                oMenus = oMenuItem.SubMenus
        '                oMenus.AddEx(oCreationPackage)

        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try

        '            Try
        '                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        '                oCreationPackage = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)

        '                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
        '                oCreationPackage.UniqueID = "GIR"
        '                oCreationPackage.String = "Good Issue Reversal"
        '                oCreationPackage.Enabled = True

        '                oMenuItem = oApplication.Menus.Item("1280") 'Data'
        '                oMenus = oMenuItem.SubMenus
        '                oMenus.AddEx(oCreationPackage)

        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try


        '        Else
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                oApplication.Menus.RemoveEx("GR")
        '            Catch ex As Exception
        '                '  MessageBox.Show(ex.Message)
        '            End Try

        '            Try
        '                oApplication.Menus.RemoveEx("GIR")
        '            Catch ex As Exception
        '                'MessageBox.Show(ex.Message)
        '            End Try

        '        End If
        '    Else
        '        Dim oMenuItem As SAPbouiCOM.MenuItem
        '        Dim oMenus As SAPbouiCOM.Menus


        '        Try
        '            oApplication.Menus.RemoveEx("GR")
        '        Catch ex As Exception
        '            ' MessageBox.Show(ex.Message)
        '        End Try
        '        Try
        '            oApplication.Menus.RemoveEx("GIR")
        '        Catch ex As Exception
        '            'MessageBox.Show(ex.Message)
        '        End Try
        '    End If


        '    If FrmID = "721" Then
        '        If (eventInfo.BeforeAction = True) Then
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        '                oCreationPackage = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)

        '                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
        '                oCreationPackage.UniqueID = "GRGI"
        '                oCreationPackage.String = "Good Issue"
        '                oCreationPackage.Enabled = True

        '                oMenuItem = oApplication.Menus.Item("1280") 'Data'
        '                oMenus = oMenuItem.SubMenus
        '                oMenus.AddEx(oCreationPackage)

        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try

        '            Try
        '                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        '                oCreationPackage = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)

        '                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
        '                oCreationPackage.UniqueID = "GRR"
        '                oCreationPackage.String = "Good Receipt Reversal"
        '                oCreationPackage.Enabled = True

        '                oMenuItem = oApplication.Menus.Item("1280") 'Data'
        '                oMenus = oMenuItem.SubMenus
        '                oMenus.AddEx(oCreationPackage)

        '            Catch ex As Exception
        '                ' MessageBox.Show(ex.Message)
        '            End Try


        '        Else
        '            Dim oMenuItem As SAPbouiCOM.MenuItem
        '            Dim oMenus As SAPbouiCOM.Menus


        '            Try
        '                oApplication.Menus.RemoveEx("GRGI")
        '            Catch ex As Exception
        '                '  MessageBox.Show(ex.Message)
        '            End Try

        '            Try
        '                oApplication.Menus.RemoveEx("GRR")
        '            Catch ex As Exception
        '                'MessageBox.Show(ex.Message)
        '            End Try

        '        End If
        '    Else
        '        Dim oMenuItem As SAPbouiCOM.MenuItem
        '        Dim oMenus As SAPbouiCOM.Menus


        '        Try
        '            oApplication.Menus.RemoveEx("GRGI")
        '        Catch ex As Exception
        '            ' MessageBox.Show(ex.Message)
        '        End Try
        '        Try
        '            oApplication.Menus.RemoveEx("GRR")
        '        Catch ex As Exception
        '            'MessageBox.Show(ex.Message)
        '        End Try
        '    End If



        'End Sub

        Public Shared Sub FlushMemory()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1)
            End If
        End Sub

        Private Sub oApplication_LayoutKeyEvent(ByRef eventInfo As SAPbouiCOM.LayoutKeyInfo, ByRef BubbleEvent As Boolean) Handles oApplication.LayoutKeyEvent
            Try
                Dim FrmUID As String = oApplication.Forms.ActiveForm.UniqueID

                If FrmUID.StartsWith("SAP_UDO_OITR") Then

                    If eventInfo.BeforeAction = True Then
                        sForm = oApplication.Forms.GetForm(oApplication.Forms.ActiveForm.TypeEx, oApplication.Forms.ActiveForm.TypeCount)


                        If sForm.Items.Item("Item_15").Specific.String <> "" Then
                            eventInfo.LayoutKey = sForm.Items.Item("Item_15").Specific.value
                        Else
                            eventInfo.LayoutKey = "0"

                            BubbleEvent = False
                            Exit Sub
                        End If

                    End If
                ElseIf FrmUID.StartsWith("SAP_UDO_OIT") Then

                    If eventInfo.BeforeAction = True Then
                        sForm = oApplication.Forms.GetForm(oApplication.Forms.ActiveForm.TypeEx, oApplication.Forms.ActiveForm.TypeCount)

                        If sForm.Items.Item("Item_15").Specific.String <> "" Then
                            eventInfo.LayoutKey = sForm.Items.Item("Item_15").Specific.value
                        Else
                            eventInfo.LayoutKey = "0"

                            BubbleEvent = False
                            Exit Sub
                        End If
                    End If


                End If
            Catch ex As Exception
                oApplication.MessageBox("[LayoutEvent] - " & ex.Message, 1, "Ok", "", "")
                GC.Collect()
                FlushMemory()
            End Try

        End Sub

        Public Sub AddReportLayOut()
            Try

                Dim oReportTypeService As SAPbobsCOM.ReportTypesService
                oReportTypeService = bobCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)

                Dim oReportType As SAPbobsCOM.ReportType
                Dim oExists As Boolean = False

                oReportType = oReportTypeService.GetDataInterface(SAPbobsCOM.ReportTypesServiceDataInterfaces.rtsReportType)

                oReportType.TypeName = "InvTransfer"
                oReportType.AddonName = "NOCADDON"
                oReportType.AddonFormType = "InvTransfer"
                oReportType.MenuID = "SAP_RPT_INVTRSFER"

                Dim oReportTypeParam As SAPbobsCOM.ReportTypeParams



                Dim ReportTypeCount = oReportTypeService.GetReportTypeList().Count
                For iRow As Integer = 1 To ReportTypeCount - 1

                    If oReportTypeService.GetReportTypeList().Item(iRow).AddonFormType = "InvTransfer" Then
                        oExists = True
                        Exit For

                    End If
                Next


                If oExists = False Then
                    'Add Report Layout
                    oReportTypeParam = oReportTypeService.AddReportType(oReportType)

                End If
            Catch ex As Exception

            End Try


            Try

                Dim oReportTypeService As SAPbobsCOM.ReportTypesService
                oReportTypeService = bobCompany.GetCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportTypesService)

                Dim oReportType As SAPbobsCOM.ReportType
                Dim oExists As Boolean = False

                oReportType = oReportTypeService.GetDataInterface(SAPbobsCOM.ReportTypesServiceDataInterfaces.rtsReportType)

                oReportType.TypeName = "InvRequest"
                oReportType.AddonName = "NOCADDON"
                oReportType.AddonFormType = "InvRequest"
                oReportType.MenuID = "SAP_RPT_INVREQUEST"

                Dim oReportTypeParam As SAPbobsCOM.ReportTypeParams



                Dim ReportTypeCount = oReportTypeService.GetReportTypeList().Count
                For iRow As Integer = 1 To ReportTypeCount - 1

                    If oReportTypeService.GetReportTypeList().Item(iRow).AddonFormType = "InvRequest" Then
                        oExists = True
                        Exit For

                    End If
                Next


                If oExists = False Then

                    oReportTypeParam = oReportTypeService.AddReportType(oReportType)

                End If
            Catch ex As Exception

            End Try



        End Sub

    End Class
End Namespace



