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

<PhoebusCommand(Desc:="VUS Branch")>
<Serializable()> _
Public Class BR
    Inherits Csla.BusinessBase(Of BR)
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


    Private _branchid As Integer
    <System.ComponentModel.DataObjectField(True, False)> _
    Public ReadOnly Property Branchid() As String
        Get
            Return _branchid
        End Get
    End Property

    Private _branchcode As String = String.Empty
    Public Property Branchcode() As String
        Get
            Return _branchcode
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Branchcode", True)
            If value Is Nothing Then value = String.Empty
            If Not _branchcode.Equals(value) Then
                _branchcode = value
                PropertyHasChanged("Branchcode")
            End If
        End Set
    End Property

    Private _idxorder As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
    Public Property Idxorder() As String
        Get
            Return _idxorder.Text
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Idxorder", True)
            If value Is Nothing Then value = String.Empty
            If Not _idxorder.Equals(value) Then
                _idxorder.Text = value
                PropertyHasChanged("Idxorder")
            End If
        End Set
    End Property


    'Get ID
    Protected Overrides Function GetIdValue() As Object
        Return _branchid
    End Function

    'IComparable
    Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
        Dim ID = IDObject.ToString
        Dim pBranchid As Integer = ID.Trim.ToInteger
        If _branchid < pBranchid Then Return -1
        If _branchid > pBranchid Then Return 1
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

        For Each _field As ClassField In ClassSchema(Of BR)._fieldList
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

    Public Shared Function BlankBR() As BR
        Return New BR
    End Function

    Public Shared Function NewBR(ByVal pBranchid As String) As BR
        If KeyDuplicated(pBranchid) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("BR")))
        Return DataPortal.Create(Of BR)(New Criteria(pBranchid.ToInteger))
    End Function

    Public Shared Function NewBO(ByVal ID As String) As BR
        Dim pBranchid As String = ID.Trim

        Return NewBR(pBranchid)
    End Function

    Public Shared Function GetBR(ByVal pBranchid As String) As BR
        Return DataPortal.Fetch(Of BR)(New Criteria(pBranchid.ToInteger))
    End Function

    Public Shared Function GetBO(ByVal ID As String) As BR
        Dim pBranchid As String = ID.Trim

        Return GetBR(pBranchid)
    End Function

    Public Shared Sub DeleteBR(ByVal pBranchid As String)
        DataPortal.Delete(New Criteria(pBranchid.ToInteger))
    End Sub

    Public Overrides Function Save() As BR
        If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
        If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("BR")))

        Me.ApplyEdit()
        BRInfoList.InvalidateCache()
        Return MyBase.Save()
    End Function

    Public Function CloneBR(ByVal pBranchid As String) As BR

        If BR.KeyDuplicated(pBranchid) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

        Dim cloningBR As BR = MyBase.Clone
        cloningBR._branchid = 0

        'Todo:Remember to reset status of the new object here 
        cloningBR.MarkNew()
        cloningBR.ApplyEdit()

        cloningBR.ValidationRules.CheckRules()

        Return cloningBR
    End Function

#End Region ' Factory Methods

#Region " Data Access "

    <Serializable()> _
    Private Class Criteria
        Public _branchid As Integer

        Public Sub New(ByVal pBranchid As String)
            _branchid = pBranchid.ToInteger

        End Sub
    End Class

    <RunLocal()> _
    Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
        _branchid = criteria._branchid

        ValidationRules.CheckRules()
    End Sub

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()
                cm.CommandType = CommandType.Text
                'cm.CommandText = <SqlText>SELECT * FROM ListBranch WHERE DTB='<%= _DTB %>'  AND BranchID= '<%= criteria._branchid %>' </SqlText>.Value.Trim
                cm.CommandText = <SqlText>SELECT * FROM ListBranch WHERE BranchID= <%= criteria._branchid %></SqlText>.Value.Trim
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
        _branchid = dr.GetInt32("BranchID")
        _branchcode = dr.GetString("BranchCode").TrimEnd
        _idxorder.Text = dr.GetInt32("IdxOrder")

    End Sub

    Private Shared _lockObj As New Object
    Protected Overrides Sub DataPortal_Insert()
        SyncLock _lockObj
            Using ctx = ConnectionManager.GetManager
                Using cm = ctx.Connection.CreateCommand()

                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "pbs_BR_InsertUpdate"

                    AddInsertParameters(cm)
                    cm.ExecuteNonQuery()

                End Using
            End Using
        End SyncLock
    End Sub

    Private Sub AddInsertParameters(ByVal cm As SqlCommand)
        cm.Parameters.AddWithValue("@BranchID", _branchid)
        cm.Parameters.AddWithValue("@BranchCode", _branchcode.Trim)
        cm.Parameters.AddWithValue("@IdxOrder", _idxorder.DBValue)
    End Sub


    Protected Overrides Sub DataPortal_Update()
        DataPortal_Insert()
    End Sub

    Protected Overrides Sub DataPortal_DeleteSelf()
        DataPortal_Delete(New Criteria(_branchid))
    End Sub

    Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
        Using ctx = ConnectionManager.GetManager
            Using cm = ctx.Connection.CreateCommand()

                cm.CommandType = CommandType.Text
                'cm.CommandText = <SqlText>DELETE ListBranch WHERE DTB='<%= _DTB %>'  AND BranchID= '<%= criteria._branchid %>' </SqlText>.Value.Trim
                cm.CommandText = <SqlText>DELETE ListBranch WHERE BranchID= <%= criteria._branchid %></SqlText>.Value.Trim
                cm.ExecuteNonQuery()

            End Using
        End Using

    End Sub

    'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
    '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
    '        BRInfoList.InvalidateCache()
    '    End If
    'End Sub


#End Region 'Data Access                           

#Region " Exists "
    Public Shared Function Exists(ByVal pBranchid As String) As Boolean
        Return BRInfoList.ContainsCode(pBranchid)
    End Function

    Public Shared Function KeyDuplicated(ByVal pBranchid As String) As Boolean
        'Dim SqlText = <SqlText>SELECT COUNT(*) FROM ListBranch WHERE DTB='<%= Context.CurrentBECode %>'  AND BranchID= '<%= pBranchid %>'</SqlText>.Value.Trim
        Dim SqlText = <SqlText>SELECT COUNT(*) FROM ListBranch WHERE BranchID= '<%= pBranchid %>'</SqlText>.Value.Trim
        Return SQLCommander.GetScalarInteger(SqlText) > 0
    End Function
#End Region

#Region " IGenpart "

    Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
        Return CloneBR(id)
    End Function

    Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
        Return GetBO(id)
    End Function

    Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
        Return pbs.Helper.Action.StandardReferenceCommands
    End Function

    Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
        Return GetType(BR).ToString
    End Function

    Public Function myName() As String Implements Interfaces.IGenPartObject.myName
        Return GetType(BR).ToString.Leaf
    End Function

    Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
        Return BRInfoList.GetBRInfoList
    End Function
#End Region

#Region "IDoclink"
    Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
        Return String.Format("{0}#{1}", Get_TransType, _branchid)
    End Function

    Public Function Get_TransType() As String Implements IDocLink.Get_TransType
        Return Me.GetType.ToClassSchemaName.Leaf
    End Function
#End Region

End Class

'End Namespace