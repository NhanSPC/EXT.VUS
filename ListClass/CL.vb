Imports pbs.Helper
Imports System.Data
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.BusinessRules


'Namespace EXT.VUS
<PhoebusCommand(Desc:="VUS Class")>
<Serializable()> _
Public Class CL
    Inherits Csla.BusinessBase(Of CL)
    Implements Interfaces.IGenPartObject
    Implements IComparable
    Implements IDocLink



#Region "Property Changed"
    Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
        MyBase.OnDeserialized(context)
        AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
    End Sub

    Private Sub BO_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        'Select Case e.PropertyName

        '    Case "OrderType"
        '        If Not Me.GetOrderTypeInfo.ManualRef Then
        '            Me._orderNo = POH.AutoReference
        '        End If

        '    Case "OrderDate"
        '        If String.IsNullOrEmpty(Me.OrderPrd) Then Me._orderPrd.Text = Me._orderDate.Date.ToSunPeriod

        '    Case "SuppCode"
        '        For Each line In Lines
        '            line._suppCode = Me.SuppCode
        '        Next

        '    Case "ConvCode"
        '        If String.IsNullOrEmpty(Me.ConvCode) Then
        '            _convRate.Float = 0
        '        Else
        '            Dim conv = pbs.BO.LA.CVInfoList.GetConverter(Me.ConvCode, _orderPrd, String.Empty)
        '            If conv IsNot Nothing Then
        '                _convRate.Float = conv.DefaultRate
        '            End If
        '        End If

        '    Case Else

        'End Select

        pbs.BO.Rules.CalculationRules.Calculator(sender, e)
    End Sub
#End Region

#Region " Business Properties and Methods "
    Private _DTB As String = String.Empty


    Private _classid As Integer
    <System.ComponentModel.DataObjectField(True, False)> _
    Public ReadOnly Property Classid() As String
        Get
            Return _classid
        End Get
    End Property

    Private _classcode As String = String.Empty
    Public Property Classcode() As String
        Get
            Return _classcode
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Classcode", True)
            If value Is Nothing Then value = String.Empty
            If Not _classcode.Equals(value) Then
                _classcode = value
                PropertyHasChanged("Classcode")
            End If
        End Set
    End Property

    Private _branchid As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public Property Branchid() As String
        Get
            Return _branchid.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Branchid", True)
            If value Is Nothing Then value = String.Empty
            If Not _branchid.Equals(value) Then
                _branchid.Text = value
                PropertyHasChanged("Branchid")
            End If
        End Set
    End Property

    Private _opendate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public Property Opendate() As String
        Get
            Return _opendate.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Opendate", True)
            If value Is Nothing Then value = String.Empty
            If Not _opendate.Equals(value) Then
                _opendate.Text = value
                PropertyHasChanged("Opendate")
            End If
        End Set
    End Property

    Private _closedate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public Property Closedate() As String
        Get
            Return _closedate.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Closedate", True)
            If value Is Nothing Then value = String.Empty
            If Not _closedate.Equals(value) Then
                _closedate.Text = value
                PropertyHasChanged("Closedate")
            End If
        End Set
    End Property

    Private _createddate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
    Public Property Createddate() As String
        Get
            Return _createddate.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Createddate", True)
            If value Is Nothing Then value = String.Empty
            If Not _createddate.Equals(value) Then
                _createddate.Text = value
                PropertyHasChanged("Createddate")
            End If
        End Set
    End Property

    Private _coursetypeid As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public Property Coursetypeid() As String
        Get
            Return _coursetypeid.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Coursetypeid", True)
            If value Is Nothing Then value = String.Empty
            If Not _coursetypeid.Equals(value) Then
                _coursetypeid.Text = value
                PropertyHasChanged("Coursetypeid")
            End If
        End Set
    End Property


    'Get ID
    Protected Overrides Function GetIdValue() As Object
        Return _classid
    End Function

    'IComparable
    Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
        Dim ID = IDObject.ToString
        Dim pClassid As Integer = ID.Trim.ToInteger
        If _classid < pClassid Then Return -1
        If _classid > pClassid Then Return 1
        Return 0
    End Function

#End Region 'Business Properties and Methods

#Region "Validation Rules"

    Private Sub AddSharedCommonRules()
        'Sample simple custom rule
        'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

        'Sample dependent property. when check one , check the other as well
        'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
    End Sub

    Protected Overrides Sub AddBusinessRules()
        AddSharedCommonRules()

        For Each _field As ClassField In ClassSchema(Of CL)._fieldList
            If _field.Required Then
                ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _field.FieldName, 0)
            End If
            If Not String.IsNullOrEmpty(_field.RegexPattern) Then
                ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.RegExMatch, New RegExRuleArgs(_field.FieldName, _field.RegexPattern), 1)
            End If
            '----------using lookup, if no user lookup defined, fallback to predefined by developer----------------------------
            If CATMAPInfoList.ContainsCode(_field) Then
                ValidationRules.AddRule(AddressOf LKUInfoList.ContainsLiveCode, _field.FieldName, 2)
                'Else
                '    Select Case _field.FieldName
                '        Case "LocType"
                '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationType))
                '        Case "Status"
                '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                '    End Select
            End If
        Next
        pbs.BO.Rules.BusinessRules.RegisterBusinessRules(Me)
        MyBase.AddBusinessRules()
    End Sub
#End Region ' Validation

#Region " Factory Methods "

    Private Sub New()
        _DTB = Context.CurrentBECode
    End Sub

    Public Shared Function BlankCL() As CL
        Return New CL
    End Function

    Public Shared Function NewCL(ByVal pClassid As String) As CL
        If KeyDuplicated(pClassid) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("CL")))
        Return DataPortal.Create(Of CL)(New Criteria(pClassid.ToInteger))
    End Function

    Public Shared Function NewBO(ByVal ID As String) As CL
        Dim pClassid As String = ID.Trim

        Return NewCL(pClassid)
    End Function

    Public Shared Function GetCL(ByVal pClassid As String) As CL
        Return DataPortal.Fetch(Of CL)(New Criteria(pClassid.ToInteger))
    End Function

    Public Shared Function GetBO(ByVal ID As String) As CL
        Dim pClassid As String = ID.Trim

        Return GetCL(pClassid)
    End Function

    Public Shared Sub DeleteCL(ByVal pClassid As String)
        DataPortal.Delete(New Criteria(pClassid.ToInteger))
    End Sub

    Public Overrides Function Save() As CL
        If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
        If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("CL")))

        Me.ApplyEdit()
        CLInfoList.InvalidateCache()
        Return MyBase.Save()
    End Function

    Public Function CloneCL(ByVal pClassid As String) As CL

        If CL.KeyDuplicated(pClassid) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

        Dim cloningCL As CL = MyBase.Clone
        cloningCL._classid = 0

        'Todo:Remember to reset status of the new object here 
        cloningCL.MarkNew()
        cloningCL.ApplyEdit()

        cloningCL.ValidationRules.CheckRules()

        Return cloningCL
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    <Serializable()> _
    Private Class Criteria
        Public _classid As Integer

        Public Sub New(ByVal pClassid As String)
            _classid = pClassid

        End Sub
    End Class

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
        _classid = criteria._classid

        ValidationRules.CheckRules()
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()
                cm.CommandType = CommandType.Text
                'cm.CommandText = <SqlText>SELECT * FROM ListClass WHERE DTB='<%= _DTB %>'  AND ClassID= '<%= criteria._classid %>' </SqlText>.Value.Trim
                cm.CommandText = <SqlText>SELECT * FROM ListClass WHERE ClassID= <%= criteria._classid %></SqlText>.Value.Trim

                Using dr As New SafeDataReader(cm.ExecuteReader)
                    If dr.Read Then
                        FetchObject(dr)
                        MarkOld()
                    End If
                End Using

            End Using
        End Using
    End Sub

    Private Sub FetchObject(ByVal dr As SafeDataReader)
        _classid = dr.GetInt32("ClassID")
        _classcode = dr.GetString("ClassCode").TrimEnd
        _branchid.Text = dr.GetInt32("BranchID")
        _opendate.Text = dr.GetString("OpenDate").TrimEnd
        _closedate.Text = dr.GetString("CloseDate").TrimEnd
        _createddate.Text = dr.GetString("CreatedDate").TrimEnd
        _coursetypeid.Text = dr.GetInt32("CourseTypeID")

    End Sub

    Private Shared _lockObj As New Object
    Protected Overrides Sub DataPortal_Insert()
        SyncLock _lockObj
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "pbs_CL_InsertUpdate"

                    AddInsertParameters(cm)
                    cm.ExecuteNonQuery()

                End Using
            End Using
        End SyncLock
    End Sub

    Private Sub AddInsertParameters(ByVal cm As SqlCommand)
        cm.Parameters.AddWithValue("@ClassID", _classid)
        cm.Parameters.AddWithValue("@ClassCode", _classcode.Trim)
        cm.Parameters.AddWithValue("@BranchID", _branchid.DBValue)
        cm.Parameters.AddWithValue("@OpenDate", _opendate.DBValue)
        cm.Parameters.AddWithValue("@CloseDate", _closedate.DBValue)
        cm.Parameters.AddWithValue("@CreatedDate", _createddate.DBValue)
        cm.Parameters.AddWithValue("@CourseTypeID", _coursetypeid.DBValue)
    End Sub


    Protected Overrides Sub DataPortal_Update()
        DataPortal_Insert()
    End Sub

    Protected Overrides Sub DataPortal_DeleteSelf()
        DataPortal_Delete(New Criteria(_classid))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()

                cm.CommandType = CommandType.Text
                cm.CommandText = <SqlText>DELETE ListClass WHERE ClassID= '<%= criteria._classid %>' </SqlText>.Value.Trim
                cm.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
    '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
    '        CLInfoList.InvalidateCache()
    '    End If
    'End Sub

#End Region 'Data Access                           

#Region " Exists "
    Public Shared Function Exists(ByVal pClassid As String) As Boolean
        Return CLInfoList.ContainsCode(pClassid)
    End Function

    Public Shared Function KeyDuplicated(ByVal pClassid As String) As Boolean
        Dim SqlText = <SqlText>SELECT COUNT(*) FROM ListClass WHERE ClassID= '<%= pClassid %>'</SqlText>.Value.Trim
        Return SQLCommander.GetScalarInteger(SqlText) > 0
    End Function
#End Region

#Region " IGenpart "

    Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
        Return CloneCL(id)
    End Function

    Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
        Return GetBO(id)
    End Function

    Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
        Return pbs.Helper.Action.StandardReferenceCommands
    End Function

    Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
        Return GetType(CL).ToString
    End Function

    Public Function myName() As String Implements Interfaces.IGenPartObject.myName
        Return GetType(CL).ToString.Leaf
    End Function

    Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
        Return CLInfoList.GetCLInfoList
    End Function
#End Region

#Region "IDoclink"
    Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
        Return String.Format("{0}#{1}", Get_TransType, _classid)
    End Function

    Public Function Get_TransType() As String Implements IDocLink.Get_TransType
        Return Me.GetType.ToClassSchemaName.Leaf
    End Function
#End Region

End Class

'End Namespace